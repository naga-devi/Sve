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

    internal class BankDetailsService : IBankDetailsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public BankDetailsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<List<Models.BankDetail>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.BankDetail>()
                .Select(x => new Models.BankDetail
                {
                    BankId = x.BankId,
                    BankName = x.BankName,
                    BranchName = x.BranchName,
                    IFSC = x.Ifsc
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<(int? totalCount, List<Models.BankDetail> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.BankDetail filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.BankDetail>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.BankDetail>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.BankDetail> GetByIdAsync(int bankId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.BankDetail>().Where(x => x.BankId == bankId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.BankDetail>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.BankDetail entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.BankDetail)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.BankDetail>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.BankId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.BankDetail)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.BankDetail entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.BankDetail)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.BankDetail>(entity.BankId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.BankName = entity.BankName;
                    entityToUpdate.BranchName = entity.BranchName;
                    entityToUpdate.Ifsc = entity.IFSC;
                    entityToUpdate.Phone = entity.Phone;
                    entityToUpdate.Address = entity.Address;
                    entityToUpdate.DdpayableAddress = entity.DDPayableAddress;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.BankDetail)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] bankIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.BankDetail)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.BankDetail>(x => bankIds.Contains(x.BankId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.BankDetail)), ex);
            }

            return result;
        }
    }
}
