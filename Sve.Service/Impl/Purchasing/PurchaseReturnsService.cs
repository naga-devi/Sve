namespace Sve.Service.Impl.Purchasing
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Service.Data;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Core.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Models = Contract.Models.Purchasing;
    using Domain = Domain.Purchasing;
    using Sve.Contract.ViewModels;

    internal class PurchaseReturnsService : IPurchaseReturnsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public PurchaseReturnsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<Models.PurchaseReturns> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.PurchaseReturns filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.PurchaseReturns>()
                    .Select(x => new Models.PurchaseReturns
                    {
                        Id = x.Id,
                        PurchaseOrderId = x.PurchaseOrderId,
                        PurchaseOrder = new Models.PurchaseOrderHeader
                        {
                            Vendor = new Models.Vendors
                            {
                                CompanyName = x.PurchaseOrder.Vendor.CompanyName,
                            },
                            InvoiceNo = x.PurchaseOrder.InvoiceNo
                        },
                        GrandTotal = x.GrandTotal,
                        ReturnDate = x.ReturnDate,
                        Remarks = x.Remarks,
                        RoundOff = x.RoundOff,
                        TotalAmount = x.TotalAmount
                    })
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken: cancellationToken);

                return (result?.TotalCount, result?.Items?.ToList());
            }

        }

        public async Task<Models.PurchaseReturns> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var query = dbContext.GetAsQuerable<Domain.PurchaseReturns>().Where(x => x.Id == id);
                query = (bool)disbaleTracking ? query.AsNoTracking() : query;

                var result = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return _mapper.Map<Models.PurchaseReturns>(result);
            }
        }

        public async Task<ResponseResult> CreateAsync(PurchaseReturnCreateModel request, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.PurchaseReturns)));

            try
            {
                var purchaseDetailsIdsToBeRejected = request.Details.Select(x => x.Id).ToArray();

                using (var dbContext = _dbContext())
                {
                    var detailsToBeUpdated = dbContext.GetAsQuerable<Domain.PurchaseOrderDetail>().Where(x => purchaseDetailsIdsToBeRejected.Contains(x.Id))
                        .AsNoTracking()
                        .ToList();

                    if (detailsToBeUpdated.HasItems())
                    {
                        detailsToBeUpdated.ForEach(x =>
                        {
                            var item = request.Details.FirstOrDefault(t => t.Id == x.Id);
                            if (item != null)
                            {
                                x.RejectedQty = item.RejectedQty;
                                dbContext.Update(x);
                            }
                        });

                        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            var itemToAdd = new Domain.PurchaseReturns
                            {
                                PurchaseOrderId = request.Header.PurchaseOrderId,
                                ReturnDate = request.Header.ReturnDate,
                                Remarks = request.Header.Remarks,
                                TotalAmount = request.Header.TotalAmount,
                                RoundOff = request.Header.RoundOff,
                                GrandTotal = request.Header.GrandTotal,
                                CreatedOn = DateTime.Now,
                                CreatedBy = request.Header.CreatedBy
                            };
                            dbContext.Add(itemToAdd);

                            await dbContext.SaveChangesAsync(token: cancellationToken);
                            scope.Complete();

                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.PurchaseReturns)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.PurchaseReturns entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.PurchaseReturns)));

            try
            {
                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var entityToUpdate = await dbContext.FindByIdAsync<Domain.PurchaseReturns>(entity.Id);

                        if (entityToUpdate != null)
                        {
                            entityToUpdate.PurchaseOrderId = entity.PurchaseOrderId;
                            entityToUpdate.ReturnDate = entity.ReturnDate;
                            entityToUpdate.Remarks = entity.Remarks;
                            entityToUpdate.TotalAmount = entity.TotalAmount;
                            entityToUpdate.RoundOff = entity.RoundOff;
                            entityToUpdate.GrandTotal = entity.GrandTotal;
                            dbContext.Update(entityToUpdate);
                            await dbContext.SaveChangesAsync(token: cancellationToken);
                        }

                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.PurchaseReturns)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.PurchaseReturns)));

            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var dbContext = _dbContext())
                    {
                        dbContext.RemoveByWhere<Domain.PurchaseReturns>(x => ids.Contains(x.Id));

                        await dbContext.SaveChangesAsync(token: cancellationToken);
                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.PurchaseReturns)), ex);
            }

            return result;
        }
    }
}
