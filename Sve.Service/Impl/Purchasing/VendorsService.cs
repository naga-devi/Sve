namespace Sve.Service.Impl.Purchasing
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Service.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Models = Contract.Models.Purchasing;
    using Domain = Service.Domain.Purchasing;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class VendorsService : IVendorsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public VendorsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        //public async Task<(int? totalCount, List<Models.Vendors> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Vendors filter = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    using (var dbContext = _dbContext())
        //    {
        //        var result = await dbContext.GetAsQuerable<Domain.Vendors>()
        //            .AsNoTracking()
        //            .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

        //        if ((bool)result?.Items?.HasItems())
        //        {
        //            return (result?.TotalCount, _mapper.Map<List<Models.Vendors>>(result?.Items?.ToList()));
        //        }
        //    }

        //    return (0, null);
        //}

        public async Task<Models.Vendors> GetByTinNoAsync(string tinNo)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Vendors>().FirstOrDefaultAsync(x => x.TinNo.ToLower().Equals(tinNo.Trim().ToLower()));

            return _mapper.Map<Models.Vendors>(result);
        }

        public async Task<Models.Vendors> GetByPhoneNoAsync(string phoneNumber)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Vendors>().FirstOrDefaultAsync(x => x.PhoneNo.ToLower().Equals(phoneNumber.Trim().ToLower()));

            return _mapper.Map<Models.Vendors>(result);
        }

        public async Task<(int? totalCount, List<Models.Vendors> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Vendors filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.Vendors>()
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.Vendors>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.Vendors> GetByIdAsync(int vendorId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Vendors>().Where(x => x.VendorId == vendorId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.Vendors>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.Vendors entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Vendors)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Vendors>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.VendorId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Vendors)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.Vendors entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Vendors)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Vendors>(entity.VendorId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.CompanyName = entity.CompanyName;
                    entityToUpdate.Email = entity.Email;
                    entityToUpdate.PhoneNo = entity.PhoneNo;
                    entityToUpdate.Address = entity.Address;
                    entityToUpdate.ZipCode = entity.ZipCode;
                    entityToUpdate.Pan = entity.Pan;
                    entityToUpdate.TinNo = entity.TinNo;
                    entityToUpdate.Cstno = entity.Cstno;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Vendors)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] vendorIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Vendors)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.Vendors>(x => vendorIds.Contains(x.VendorId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Vendors)), ex);
            }

            return result;
        }
    }
}
