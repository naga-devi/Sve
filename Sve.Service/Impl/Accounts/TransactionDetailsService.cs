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

    internal class TransactionDetailsService : ITransactionDetailsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public TransactionDetailsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

		public async Task<(int? totalCount, List<Models.TransactionDetail> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.TransactionDetail filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.TransactionDetail>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.TransactionDetail>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.TransactionDetail> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.TransactionDetail>().Where(x => x.Id == id);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.TransactionDetail>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.TransactionDetail entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.TransactionDetail)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.TransactionDetail>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.Id;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.TransactionDetail)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.TransactionDetail entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.TransactionDetail)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.TransactionDetail>(entity.Id);

                if (entityToUpdate != null)
                {
                    entityToUpdate.TransactionId = entity.TransactionId;
                    entityToUpdate.FromAccountId = entity.FromAccountId;
                    entityToUpdate.ToAccountId = entity.ToAccountId;
                    entityToUpdate.ChequeDate = entity.ChequeDate;
                    entityToUpdate.ChequeNo = entity.ChequeNo;
                    entityToUpdate.Utrno = entity.Utrno;
                    entityToUpdate.TransactionDate = entity.TransactionDate;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.TransactionDetail)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.TransactionDetail)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.TransactionDetail>(x => ids.Contains(x.Id));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.TransactionDetail)), ex);
            }

            return result;
        }
    }
}
