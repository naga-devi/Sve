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

    internal class PayModesService : IPayModesService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public PayModesService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }
        public async Task<List<Models.PayMode>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.PayMode>()
                .Select(x => new Models.PayMode
                {
                    PayModeId = x.PayModeId,
                    Name = x.Name,
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<(int? totalCount, List<Models.PayMode> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.PayMode filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.PayMode>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.PayMode>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.PayMode> GetByIdAsync(int payModeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.PayMode>().Where(x => x.PayModeId == payModeId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.PayMode>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.PayMode entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.PayMode)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.PayMode>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.PayModeId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.PayMode)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.PayMode entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.PayMode)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.PayMode>(entity.PayModeId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.Name = entity.Name;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.PayMode)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int[] payModeIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.PayMode)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.PayMode>(x => payModeIds.Contains(x.PayModeId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.PayMode)), ex);
            }

            return result;
        }
    }
}
