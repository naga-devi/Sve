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
    using Domain = Service.Domain.Purchasing;

    internal class CreditNotesInOrdersService : ICreditNotesInOrdersService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public CreditNotesInOrdersService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

		public async Task<(int? totalCount, List<Models.CreditNotesInOrders> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.CreditNotesInOrders filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.CreditNotesInOrders>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken: cancellationToken);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.CreditNotesInOrders>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.CreditNotesInOrders> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
			using (var dbContext = _dbContext())
            {
                var query = dbContext.GetAsQuerable<Domain.CreditNotesInOrders>().Where(x => x.Id == id);
                query = (bool)disbaleTracking ? query.AsNoTracking() : query;

                var result = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return _mapper.Map<Models.CreditNotesInOrders>(result);
            }
        }

        public async Task<ResponseResult> CreateAsync(Models.CreditNotesInOrders entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.CreditNotesInOrders)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.CreditNotesInOrders>(entity);

				using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        dbContext.Add(entityToAdd);

                        await dbContext.SaveChangesAsync(token: cancellationToken);
                        scope.Complete();
                        result.NewId = entityToAdd.Id;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.CreditNotesInOrders)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.CreditNotesInOrders entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.CreditNotesInOrders)));

            try
            {
                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var entityToUpdate = await dbContext.FindByIdAsync<Domain.CreditNotesInOrders>(entity.Id);

                        if (entityToUpdate != null)
                        {							entityToUpdate.Id = entity.Id;
							entityToUpdate.CreditNoteId = entity.CreditNoteId;
							entityToUpdate.PurchaseOrderId = entity.PurchaseOrderId;
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
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.CreditNotesInOrders)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.CreditNotesInOrders)));

            try
            {
				using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var dbContext = _dbContext())
                    {
                        dbContext.RemoveByWhere<Domain.CreditNotesInOrders>(x => ids.Contains(x.Id));

                        await dbContext.SaveChangesAsync(token: cancellationToken);
                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.CreditNotesInOrders)), ex);
            }

            return result;
        }
    }
}
