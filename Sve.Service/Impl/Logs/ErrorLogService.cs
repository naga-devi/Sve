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

    public class ErrorLogService : IErrorLogService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public ErrorLogService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

		public async Task<(int? totalCount, List<Models.ErrorLog> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<Models.ErrorLog> filter = null)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.ErrorLog>()
                    .AsNoTracking()
                    .ToPaginateAsync(index, size, sortColumn, sortDirection, filter);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.Count, _mapper.Map<List<Models.ErrorLog>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.ErrorLog> GetByIdAsync(int errorLogId)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.FindAsync<Domain.ErrorLog>(errorLogId);

                return _mapper.Map<Models.ErrorLog>(result);
            }
        }

        public async Task<ResponseResult> CreateAsync(Models.ErrorLog entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.ErrorLog)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.ErrorLog>(entity);

				using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        dbContext.Add(entityToAdd);

                        await dbContext.SaveChangesAsync();
                        scope.Complete();
                        result.NewId = entityToAdd.ErrorLogId;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.ErrorLog)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.ErrorLog entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.ErrorLog)));

            try
            {
                using (var dbContext = _dbContext())
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var entityToUpdate = await dbContext.FindAsync<Domain.ErrorLog>(entity.ErrorLogId);

                        if (entityToUpdate != null)
                        {							entityToUpdate.ErrorTime = entity.ErrorTime;
							entityToUpdate.UserName = entity.UserName;
							entityToUpdate.ErrorNumber = entity.ErrorNumber;
							entityToUpdate.ErrorSeverity = entity.ErrorSeverity;
							entityToUpdate.ErrorState = entity.ErrorState;
							entityToUpdate.ErrorProcedure = entity.ErrorProcedure;
							entityToUpdate.ErrorLine = entity.ErrorLine;
							entityToUpdate.ErrorMessage = entity.ErrorMessage;
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

        public async Task<ResponseResult> DeleteByIdAsync(int[] errorLogIds)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.ErrorLog)));

            try
            {
				using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var dbContext = _dbContext())
                    {
                        dbContext.RemoveByWhere<Domain.ErrorLog>(x => errorLogIds.Contains(x.ErrorLogId));

                        await dbContext.SaveChangesAsync();
                        scope.Complete();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.ErrorLog)), ex);
            }

            return result;
        }
    }
}
