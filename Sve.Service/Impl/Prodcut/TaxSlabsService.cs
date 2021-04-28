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

    internal class TaxSlabsService : ITaxSlabsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public TaxSlabsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<List<Models.TaxSlabs>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.ProductTaxSlabs>()
                .Select(x => new Models.TaxSlabs
                {
                    TaxSlabId = x.TaxSlabId,
                    Name = x.Name,
                    TotalTax = x.TotalTax,
                    Sgst = x.Sgst,
                    Cgst = x.Cgst,
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<(int? totalCount, List<Models.TaxSlabs> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.TaxSlabs filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.ProductTaxSlabs>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.TaxSlabs>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.TaxSlabs> GetByIdAsync(int taxSlabId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.ProductTaxSlabs>().Where(x => x.TaxSlabId == taxSlabId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.TaxSlabs>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.TaxSlabs entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.TaxSlabs)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.ProductTaxSlabs>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.TaxSlabId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.TaxSlabs)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.TaxSlabs entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.TaxSlabs)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.ProductTaxSlabs>(entity.TaxSlabId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.Name = entity.Name;
                    entityToUpdate.TotalTax = entity.TotalTax;
                    entityToUpdate.Cgst = entity.Cgst;
                    entityToUpdate.Sgst = entity.Sgst;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.TaxSlabs)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] taxSlabIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.TaxSlabs)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.ProductTaxSlabs>(x => taxSlabIds.Contains(x.TaxSlabId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.TaxSlabs)), ex);
            }

            return result;
        }
    }
}
