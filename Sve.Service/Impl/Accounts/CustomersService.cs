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

    internal class CustomersService : ICustomersService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public CustomersService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }
        public async Task<List<Models.Customer>> GetAllAsync(int? groupId = default, string searchText = default, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Customer>()
                //.If(groupId.IsGreaterThanZero(), x=> x.Where(x=> x.GroupId == groupId))
                .If(!searchText.IsNullOrEmpty(), x=> x.Where(t=> t.CompanyName.Contains(searchText) || t.TinNo.Contains(searchText)))
                .Select(x => new Models.Customer
                {
                    CustomerId = x.CustomerId,
                    CompanyName = x.CompanyName,
                    TinNo = x.TinNo
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<(int? totalCount, List<Models.Customer> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Customer filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.Customer>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.Customer>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.Customer> GetByIdAsync(int customerId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Customer>().Where(x => x.CustomerId == customerId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.Customer>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.Customer entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Customer)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Customer>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.CustomerId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Customer)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.Customer entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Customer)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Customer>(entity.CustomerId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.GroupId = entity.GroupId;
                    entityToUpdate.CompanyName = entity.CompanyName;
                    entityToUpdate.Email = entity.Email;
                    entityToUpdate.PhoneNo = entity.PhoneNo;
                    entityToUpdate.Address = entity.Address;
                    entityToUpdate.City = entity.City;
                    entityToUpdate.ZipCode = entity.ZipCode;
                    entityToUpdate.StateId = entity.StateId;
                    entityToUpdate.Pan = entity.Pan;
                    entityToUpdate.TinNo = entity.TinNo;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Customer)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] customerIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Customer)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.Customer>(x => customerIds.Contains(x.CustomerId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Customer)), ex);
            }

            return result;
        }
    }
}
