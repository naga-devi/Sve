namespace Sve.Service.Impl.Accounts
{
	using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Accounts;
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
    using Models = Contract.Models.Accounts;
    using Domain = Service.Domain.Accounts;

    internal class TransactionsService : ITransactionsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public TransactionsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

		public async Task<(int? totalCount, List<Models.Transactions> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Transactions filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.Transaction>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.Transactions>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.Transactions> GetByIdAsync(int transactionId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Transaction>().Where(x => x.TransactionId == transactionId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.Transactions>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.Transactions entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Transactions)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Transaction>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.TransactionId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Transactions)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> CreateAsync(Models.Transactions entity, Models.TransactionDetail transactionDetail, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Transactions)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Transaction>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                result.NewId = entityToAdd.TransactionId;

                if(entityToAdd.TransactionId > 0)
                {
                    var detailsToAdd = _mapper.Map<Domain.TransactionDetail>(transactionDetail);
                    dbContext.Add(detailsToAdd);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Transactions)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.Transactions entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Transactions)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Transaction>(entity.TransactionId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.VoucherTypeId = entity.VoucherTypeId;
                    entityToUpdate.AccountTypeId = entity.AccountTypeId;
                    entityToUpdate.CustomerId = entity.CustomerId;
                    entityToUpdate.PayModeId = entity.PayModeId;
                    entityToUpdate.PaidAmount = entity.PaidAmount;
                    entityToUpdate.PaidDate = entity.PaidDate;
                    entityToUpdate.Remarks = entity.Remarks;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Transactions)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(long[] transactionIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Transactions)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.Transaction>(x => transactionIds.Contains(x.TransactionId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Transactions)), ex);
            }

            return result;
        }
    }
}
