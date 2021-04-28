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

    internal class AccountDetailsService : IAccountDetailsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public AccountDetailsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<List<Models.AccountDetail>> GetByCustomerId(int customerId, string searchText = default, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.AccountDetail>()
                .Include(x => x.Customer)
                .Include(x => x.Bank)
                .Where(x => x.CustomerId == customerId || x.Customer.IsParentCompany == true)
                .If(!searchText.IsNullOrEmpty(), x => x.Where(t => t.Customer.CompanyName.Contains(searchText) || t.Customer.TinNo.Contains(searchText)))
                .Select(x => new Models.AccountDetail
                {
                    AccountId = x.AccountId,
                    AccountNo = x.AccountNo,
                    CustomerId = x.CustomerId,
                    Customer = new Models.Customer
                    {
                        CompanyName = x.Customer.CompanyName,
                        IsParentCompany= x.Customer.IsParentCompany
                    },
                    Bank = new Models.BankDetail
                    {
                        BankName = x.Bank.BankName,
                        IFSC = x.Bank.Ifsc
                    }
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<(int? totalCount, List<Models.AccountDetail> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.AccountDetail filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.AccountDetail>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.AccountDetail>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.AccountDetail> GetByIdAsync(int accountId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.AccountDetail>().Where(x => x.AccountId == accountId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.AccountDetail>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.AccountDetail entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.AccountDetail)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.AccountDetail>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.AccountId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.AccountDetail)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.AccountDetail entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.AccountDetail)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.AccountDetail>(entity.AccountId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.CustomerId = entity.CustomerId;
                    entityToUpdate.BankId = entity.BankId;
                    entityToUpdate.AccountNo = entity.AccountNo;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.AccountDetail)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] accountIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.AccountDetail)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.AccountDetail>(x => accountIds.Contains(x.AccountId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.AccountDetail)), ex);
            }

            return result;
        }
    }
}
