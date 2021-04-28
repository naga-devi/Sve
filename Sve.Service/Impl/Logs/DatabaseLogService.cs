namespace Sve.Service.Impl.Logs
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Logs;
    using Sve.Service.Data;
    using Models = Contract.Models.Logs;

    public class DatabaseLogService : IDatabaseLogService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public DatabaseLogService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

		public async Task<(int? totalCount, List<Models.DatabaseLog> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<Models.DatabaseLog> filter = null)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.DatabaseLog>()
                    .AsNoTracking()
                    .ToPaginateAsync(index, size, sortColumn, sortDirection, filter);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.Count, _mapper.Map<List<Models.DatabaseLog>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.DatabaseLog> GetByIdAsync(int databaseLogId)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.FindAsync<Domain.DatabaseLog>(databaseLogId);

                return _mapper.Map<Models.DatabaseLog>(result);
            }
        }

        public async Task<ResponseResult> CreateAsync(Models.DatabaseLog entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.DatabaseLog)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.DatabaseLog>(entity);

				using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        dbContext.Add(entityToAdd);

                        await dbContext.SaveChangesAsync();
                        scope.Complete();
                        result.NewId = entityToAdd.DatabaseLogId;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.DatabaseLog)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.DatabaseLog entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.DatabaseLog)));

            try
            {
                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var entityToUpdate = await dbContext.FindAsync<Domain.DatabaseLog>(entity.DatabaseLogId);

                        if (entityToUpdate != null)
                        {							entityToUpdate.PostTime = entity.PostTime;
							entityToUpdate.DatabaseUser = entity.DatabaseUser;
							entityToUpdate.Event = entity.Event;
							entityToUpdate.Schema = entity.Schema;
							entityToUpdate.Object = entity.Object;
							entityToUpdate.TSQL = entity.TSQL;
							entityToUpdate.XmlEvent = entity.XmlEvent;
                            dbContext.Update(entityToUpdate);
                            await dbContext.SaveChangesAsync();
                        }

                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.ProductCategory)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int[] databaseLogIds)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.DatabaseLog)));

            try
            {
				using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var dbContext = _dbContext())
                    {
                        dbContext.RemoveByWhere<Domain.DatabaseLog>(x => databaseLogIds.Contains(x.DatabaseLogId));

                        await dbContext.SaveChangesAsync();
                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.DatabaseLog)), ex);
            }

            return result;
        }
    }
}
