namespace Sve.Service.Impl.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading.Tasks;
    using System.Transactions;
    using AutoMapper;
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract;
    using Sve.Contract.Interface.Sales;
    using Sve.Contract.ViewModels;
    using Sve.Service.Data;
    using Models = Contract.Models.Sales;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class OrderHeaderService : IOrderHeaderService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public OrderHeaderService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<Models.SalesOrderHeader> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.SalesOrderHeader filter = null)
        {
            using (var dbContext = _dbContext())
            {
                var query = dbContext.GetAsQuerable<Domain.Sales.SalesOrderHeader>();
                query = filter.SalesOrderId > 0 ? query.Where(x => x.SalesOrderId == filter.SalesOrderId) : query;

                query = query.Select(x => new Domain.Sales.SalesOrderHeader
                {
                    SalesOrderId= x.SalesOrderId,
                    OrderDate= x.OrderDate,
                    TotalQuantity = x.TotalQuantity,
                    TotalAmount = x.TotalAmount,
                    DiscountPercentage = x.DiscountPercentage,
                    NetAmount = x.NetAmount,
                    Freight = x.Freight,
                    RoundOffAmount = x.RoundOffAmount,
                    GrandTotal = x.GrandTotal,
                    PaidAmount = x.PaidAmount,
                    BalanceAmount = x.BalanceAmount,
                    Paymode = x.Paymode,
                    TransactionNo = x.TransactionNo,
                    Status = x.Status,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    CustomersInOrders = x.CustomersInOrders.Select(c => new Domain.Sales.CustomersInOrders
                    {
                        Customer = new Domain.Sales.Customers
                        {
                            CompanyName = c.Customer.CompanyName,
                            Name = c.Customer.Name,
                            PhoneNo = c.Customer.PhoneNo,
                        }
                    }).ToList()
                }).AsQueryable();

                var result = await query.AsNoTracking().GetPaginateAsync(index, size, sortColumn, isDescending);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.SalesOrderHeader>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<ResponseResult> PlaceOrderAsync(Models.SalesOrderHeader entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.SalesOrderHeader)));

            try
            {
                var orderDetails = entity.OrderDetails;
                entity.OrderDetails = null;

                var entityToAdd = _mapper.Map<Domain.Sales.SalesOrderHeader>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);
                await dbContext.SaveChangesAsync();
                result.NewId = entityToAdd.SalesOrderId;
                var salesOrderId = entityToAdd.SalesOrderId;

                if (salesOrderId > 0)
                {
                    var saledDetailsToAdd = _mapper.Map<List<Domain.Sales.SalesOrderDetails>>(orderDetails);
                    saledDetailsToAdd.ForEach(t =>
                    {
                        t.SalesOrderId = salesOrderId;
                    });

                    dbContext.AddRange(saledDetailsToAdd.ToArray());
                    await dbContext.SaveChangesAsync();
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.SalesOrderHeader)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> ConfirmOrder(int orderId, OrderConfirmRequest request)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.SalesOrderHeader)));

            try
            {
                SalesOrderStatus orderStatus = SalesOrderStatus.Completed;

                using var dbContext = _dbContext();
                var customerId = request?.Customer?.CustomerId;
                var isCustomerExisted = customerId > 0;

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                if (customerId == 0)
                {
                    var suctomerToAdd = _mapper.Map<Domain.Sales.Customers>(request.Customer);
                    dbContext.Add(suctomerToAdd);
                    await dbContext.SaveChangesAsync();
                    result.NewId = suctomerToAdd.CustomerId;
                    customerId = suctomerToAdd.CustomerId;
                }

                if (customerId > 0)
                {
                    if (!await dbContext.GetAsQuerable<Domain.Sales.CustomersInOrders>().AnyAsync(x => x.SalesOrderId == orderId && x.CustomerId == customerId))
                    {
                        var cutomersInOrder = new Domain.Sales.CustomersInOrders()
                        {
                            SalesOrderId = orderId,
                            CustomerId = (int)customerId
                        };

                        if (isCustomerExisted)
                            dbContext.Add(cutomersInOrder);
                        else
                            dbContext.Update(cutomersInOrder);
                    }
                }

                var orderToMap = _mapper.Map<Domain.Sales.SalesOrderHeader>(request.SalesOrder);

                var orderDetailsToUpdate = orderToMap?.OrderDetails;

                var orderToUpdate = await dbContext.FindByIdAsync<Domain.Sales.SalesOrderHeader>(orderId);

                if (orderToUpdate != null)
                {
                    if (request.IsOrderChanged)
                    {
                        orderToUpdate.TotalQuantity = orderDetailsToUpdate?.Sum(x => x.OrderQty) ?? 0;
                        orderToUpdate.TotalAmount = request?.SalesOrder?.OrderDetails?.Sum(x => x.LineTotal) ?? 0;//TODO
                    }

                    orderToUpdate.DiscountPercentage = orderToMap.DiscountPercentage;
                    orderToUpdate.NetAmount = orderToMap.NetAmount;
                    orderToUpdate.Freight = orderToMap.Freight ?? 0;
                    orderToUpdate.RoundOffAmount = orderToMap.RoundOffAmount ?? 0;
                    orderToUpdate.GrandTotal = orderToMap.GrandTotal;
                    orderToUpdate.PaidAmount = orderToMap.PaidAmount;
                    orderToUpdate.Paymode = orderToMap.Paymode;
                    orderToUpdate.TransactionNo = orderToMap.TransactionNo;
                    orderToUpdate.OrderDate = DateTime.UtcNow;
                    //entityToUpdate.Comment = entity.Comment;
                    orderToUpdate.Status = (byte)orderStatus;
                    dbContext.PartialUpdate(orderToUpdate);
                }

                var orderDetailsToUpdateExisting = await dbContext.GetAsQuerable<Domain.Sales.SalesOrderDetails>().Where(x => x.SalesOrderId == orderId)
                    .ToListAsync();

                if (orderDetailsToUpdateExisting.HasItems())
                {
                    orderDetailsToUpdate.ToList().ForEach(x =>
                    {
                        orderDetailsToUpdateExisting.ForEach(m =>
                        {
                            m.Status = (int)orderStatus;
                            m.OrderQty = x.OrderQty;
                            dbContext.PartialUpdate(m);
                        });
                    });

                    await dbContext.SaveChangesAsync();
                }

                //if (orderDetailsToAdd.HasItems())
                //{
                //    orderDetailsToAdd?.ToList().ForEach(x =>
                //    {
                //        x.SalesOrderId = orderId;
                //        x.Status = (int)orderStatus;
                //    });
                //}

                //orderToUpdate.OrderDetails = orderDetailsToAdd;

                //dbContext.UpdateRange(orderToUpdate);

                //await dbContext.SaveChangesAsync();

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.SalesOrderHeader)), ex);
            }

            return result;
        }

        public async Task<InvoiceModel> GetInvoiceByIdAsync(int salesOrderId)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Sales.SalesOrderHeader>()
                //.Include(x=> x.CustomersInOrders).ThenInclude(m=> m.Select(t=> t.Customer))
                //.Include(x=> x.OrderDetails)
                .Where(x => x.SalesOrderId == salesOrderId)
                .Select(x => new InvoiceModel
                {
                    InvoiceNo = x.SalesOrderId,
                    InvoiceDate = x.OrderDate,
                    TotalAmountAfterTax = x.PaidAmount,
                    Discount = x.DiscountPercentage,
                    InvoiceItems = x.OrderDetails.Select(i => new InvoiceItems
                    {
                        Id = i.Id,
                        StockGroupId = i.StockGroupId,
                            //LineTotal = i.LineTotal,
                            Sgst = i.SgstAmount,
                        Cgst = i.CgstAmount,
                        Igst = i.IgstAmount,
                        OrderQty = i.OrderQty,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                });
            var sqlQuery = query.ToSql();

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            if (result != null)
            {
                var query2 = dbContext.GetAsQuerable<Domain.Sales.CustomersInOrders>()
                    .Where(x => x.SalesOrderId == salesOrderId)
                    .Select(x => new CustomerInfo
                    {
                        CosumerName = x.Customer.Name,
                        CellNo = x.Customer.PhoneNo,
                        GSTIN = x.Customer.TinNo,
                        Address = x.Customer.Address,
                    }).AsQueryable();

                result.Customer = await query2.AsNoTracking().FirstOrDefaultAsync();
            }

            return result;

        }

        //public async Task<Models.SalesOrderHeader> GetByIdAsync(int salesOrderId)
        //{
        //    using (var dbContext = _dbContext())
        //    {
        //        var result = await dbContext.FindByIdAsync<Domain.Sales.SalesOrderHeader>(salesOrderId);

        //        return _mapper.Map<Models.SalesOrderHeader>(result);
        //    }
        //}


        //public async Task<ResponseResult> UpdateAsync(Models.SalesOrderHeader entity)
        //{
        //    var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.SalesOrderHeader)));

        //    try
        //    {
        //        using (var dbContext = _dbContext())
        //        {
        //            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //            {
        //                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Sales.SalesOrderHeader>(entity.SalesOrderId);

        //                if (entityToUpdate != null)
        //                {
        //                    entityToUpdate.TotalQuantity = entity.TotalQuantity;
        //                    entityToUpdate.SubTotal = entity.SubTotal;
        //                    entityToUpdate.Cgst = entity.Cgst;
        //                    entityToUpdate.Sgst = entity.Sgst;
        //                    entityToUpdate.Freight = entity.Freight;
        //                    entityToUpdate.OrderDate = entity.OrderDate;
        //                    entityToUpdate.Comment = entity.Comment;
        //                    dbContext.Update(entityToUpdate);
        //                    await dbContext.SaveChangesAsync();
        //                }

        //                scope.Complete();

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.SalesOrderHeader)), ex);
        //    }

        //    return result;
        //}

        //public async Task<ResponseResult> DeleteByIdAsync(int[] salesOrderIds)
        //{
        //    var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.SalesOrderHeader)));

        //    try
        //    {
        //        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            using (var dbContext = _dbContext())
        //            {
        //                dbContext.RemoveByWhere<Domain.Sales.SalesOrderHeader>(x => salesOrderIds.Contains(x.SalesOrderId));

        //                await dbContext.SaveChangesAsync();
        //                scope.Complete();

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.SalesOrderHeader)), ex);
        //    }

        //    return result;
        //}
    }
}
