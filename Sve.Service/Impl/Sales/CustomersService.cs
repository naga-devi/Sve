namespace Sve.Service.Impl.Sales
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Sales;
    using Sve.Service.Data;
    using Models = Contract.Models.Sales;
    using Domain = Domain.Sales;
    using System.Linq.Core;
    using System.Threading;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class CustomersService : ICustomersService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public CustomersService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<Models.Customers> GetByTinNoAsync(string tinNo)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Customers>().FirstOrDefaultAsync(x => x.TinNo.ToLower().Equals(tinNo.Trim().ToLower()));

            return _mapper.Map<Models.Customers>(result);
        }

        public async Task<Models.Customers> GetByPhoneNoAsync(string phoneNumber)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Customers>().FirstOrDefaultAsync(x => x.PhoneNo.ToLower().Equals(phoneNumber.Trim().ToLower()));

            return _mapper.Map<Models.Customers>(result);
        }

        public async Task<(int? totalCount, List<Models.Customers> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Customers filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.Customers>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.Customers>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.Customers> GetByIdAsync(int customerId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Customers>().Where(x => x.CustomerId == customerId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.Customers>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.Customers entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Customers)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Customers>(entity);

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
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Customers)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.Customers entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Customers)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Customers>(entity.CustomerId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.CompanyName = entity.CompanyName;
                    entityToUpdate.Name = entity.Name;
                    entityToUpdate.Email = entity.Email;
                    entityToUpdate.PhoneNo = entity.PhoneNo;
                    entityToUpdate.Address = entity.Address;
                    entityToUpdate.City = entity.City;
                    entityToUpdate.ZipCode = entity.ZipCode;
                    entityToUpdate.Pan = entity.PAN;
                    entityToUpdate.TinNo = entity.TinNo;
                    entityToUpdate.Cstno = entity.CSTNo;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Customers)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] customerIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Customers)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.Customers>(x => customerIds.Contains(x.CustomerId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Customers)), ex);
            }

            return result;
        }
    }
}
