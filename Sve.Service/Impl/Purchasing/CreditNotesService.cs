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

    internal class CreditNotesService : ICreditNotesService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public CreditNotesService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<Models.CreditNotes> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.CreditNotes filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var query = dbContext.GetAsQuerable<Domain.CreditNotes>();

                query = filter.CreditNoteId > 0 ? query.Where(x => x.CreditNoteId == filter.CreditNoteId) : query;

                var result = await dbContext.GetAsQuerable<Domain.CreditNotes>()
                    .Select(x => new Models.CreditNotes
                    {
                        CreditNoteId = x.CreditNoteId,
                        Discount = x.Discount,
                        IssueDate = x.IssueDate,
                        Remarks = x.Remarks,
                        VendorId = x.VendorId,
                        Vendor = new Models.Vendors
                        {
                            CompanyName = x.Vendor.CompanyName,
                            TinNo = x.Vendor.TinNo
                        },
                        CreditOrders = x.Orders.Select(t => new Models.CreditNotesInOrders
                        {
                            PurchaseOrderId = t.PurchaseOrderId
                        }).ToList()
                    })
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken: cancellationToken);

                    return (result?.TotalCount, result?.Items?.ToList());
            }
        }

        public async Task<Models.CreditNotes> GetByIdAsync(int creditNoteId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var query = dbContext.GetAsQuerable<Domain.CreditNotes>().Where(x => x.CreditNoteId == creditNoteId);
                query = (bool)disbaleTracking ? query.AsNoTracking() : query;

                var result = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return _mapper.Map<Models.CreditNotes>(result);
            }
        }

        public async Task<ResponseResult> CreateAsync(Models.CreditNotes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.CreditNotes)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.CreditNotes>(entity);
                entityToAdd.Status = entity.Status == 0 ? (byte)EntityStatus.Active : entity.Status;

                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        dbContext.Add(entityToAdd);

                        await dbContext.SaveChangesAsync(token: cancellationToken);
                        scope.Complete();
                        result.NewId = entityToAdd.CreditNoteId;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.CreditNotes)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> CreateAsync(int purchaseOrderId, Models.CreditNotes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.CreditNotes)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.CreditNotes>(entity);
                entityToAdd.Status = entity.Status == 0 ? (byte)EntityStatus.Active : entity.Status;
                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        dbContext.Add(entityToAdd);

                        await dbContext.SaveChangesAsync(token: cancellationToken);

                        result.NewId = entityToAdd.CreditNoteId;

                        if (entityToAdd.CreditNoteId > 0)
                        {
                            var orderToAdd = new Domain.CreditNotesInOrders
                            {
                                PurchaseOrderId = purchaseOrderId,
                                CreditNoteId = entityToAdd.CreditNoteId
                            };

                            dbContext.Add(orderToAdd);

                            await dbContext.SaveChangesAsync(token: cancellationToken);
                        }

                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.CreditNotes)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.CreditNotes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.CreditNotes)));

            try
            {
                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var entityToUpdate = await dbContext.FindByIdAsync<Domain.CreditNotes>(entity.CreditNoteId);

                        if (entityToUpdate != null)
                        {
                            entityToUpdate.VendorId = entity.VendorId;
                            entityToUpdate.Discount = entity.Discount;
                            entityToUpdate.Remarks = entity.Remarks;
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
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.CreditNotes)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] creditNoteIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.CreditNotes)));

            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var dbContext = _dbContext())
                    {
                        dbContext.RemoveByWhere<Domain.CreditNotes>(x => creditNoteIds.Contains(x.CreditNoteId));

                        await dbContext.SaveChangesAsync(token: cancellationToken);
                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.CreditNotes)), ex);
            }

            return result;
        }
    }
}
