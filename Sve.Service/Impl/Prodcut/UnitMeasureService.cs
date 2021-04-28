namespace Sve.Service.Impl.Product
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Product;
    using Sve.Service.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Models = Contract.Models.Product;
    using Domain = Service.Domain.Product;
    using JxNet.Core;
    using JxNet.Core.Extensions;

    public class UnitMeasureService : IUnitMeasureService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public UnitMeasureService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<List<Models.UnitMeasure>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.UnitMeasure>()
                .Select(x => new Models.UnitMeasure
                {
                    UnitMeasureId = x.UnitMeasureId,
                    Name = x.Name,
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<(int? totalCount, List<Models.UnitMeasure> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.UnitMeasure filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.UnitMeasure>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.UnitMeasure>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.UnitMeasure> GetByIdAsync(int unitMeasureId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.UnitMeasure>().Where(x => x.UnitMeasureId == unitMeasureId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.UnitMeasure>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.UnitMeasure entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.UnitMeasure)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.UnitMeasure>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.UnitMeasureId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.UnitMeasure)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.UnitMeasure entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.UnitMeasure)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.UnitMeasure>(entity.UnitMeasureId);

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
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.UnitMeasure)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] unitMeasureIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.UnitMeasure)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.UnitMeasure>(x => unitMeasureIds.Contains(x.UnitMeasureId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.UnitMeasure)), ex);
            }

            return result;
        }
    }
}
