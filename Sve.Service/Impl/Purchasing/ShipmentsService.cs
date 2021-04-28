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
    public class ShipmentsService : IShipmentsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public ShipmentsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

		//public async Task<(int? totalCount, List<Models.Shipments> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Shipments filter = null, CancellationToken cancellationToken = default(CancellationToken))
  //      {
  //          using (var dbContext = _dbContext())
  //          {
  //              var result = await dbContext.GetAsQuerable<Domain.Shipments>()
  //                  .AsNoTracking()
  //                  .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

  //              if((bool)result?.Items?.HasItems())
  //              {
  //                  return (result?.TotalCount, _mapper.Map<List<Models.Shipments>>( result?.Items?.ToList()));
  //              }
  //          }

  //          return (0, null);
  //      }

		//public async Task<Models.Shipments> GetByIdAsync(int shipmentId, bool? disbaleTracking = false, CancellationToken cancellationToken = default(CancellationToken))
  //      {
		//	using (var dbContext = _dbContext())
  //          {
  //              var query = dbContext.GetAsQuerable<Domain.Shipments>().Where(x => x.ShipmentId == shipmentId);
  //              query = (bool)disbaleTracking ? query.AsNoTracking() : query;

  //              var result = await query.FirstOrDefaultAsync(cancellationToken);

  //              return _mapper.Map<Models.Shipments>(result);
  //          }
  //      }

  //      public async Task<ResponseResult> CreateAsync(Models.Shipments entity, CancellationToken cancellationToken = default(CancellationToken))
  //      {
  //          var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Shipments)));

  //          try
  //          {
  //              var entityToAdd = _mapper.Map<Domain.Shipments>(entity);

		//		using (var dbContext = _dbContext())
  //              {
  //                  using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
  //                  {
  //                      dbContext.Add(entityToAdd);

  //                      await dbContext.SaveChangesAsync(cancellationToken);
  //                      scope.Complete();
  //                      result.NewId = entityToAdd.ShipmentId;

  //                      return result;
  //                  }
  //              }
  //          }
  //          catch (Exception ex)
  //          {
  //              result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Shipments)), ex);
  //          }

  //          return result;
  //      }

		//public async Task<ResponseResult> UpdateAsync(Models.Shipments entity, CancellationToken cancellationToken = default(CancellationToken))
  //      {
  //          var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Shipments)));

  //          try
  //          {
  //              using (var dbContext = _dbContext())
  //              {
  //                  using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
  //                  {
  //                      var entityToUpdate = await dbContext.FindByIdAsync<Domain.Shipments>(entity.ShipmentId);

  //                      if (entityToUpdate != null)
  //                      {							entityToUpdate.MethodName = entity.MethodName;
		//					entityToUpdate.VehicleNumber = entity.VehicleNumber;
		//					entityToUpdate.Lrnumber = entity.LRNumber;
  //                          dbContext.Update(entityToUpdate);
  //                          await dbContext.SaveChangesAsync(cancellationToken);
  //                      }

  //                      scope.Complete();

  //                      return result;
  //                  }
  //              }
  //          }
  //          catch (Exception ex)
  //          {
  //              result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Shipments)), ex);
  //          }

  //          return result;
  //      }        

  //      public async Task<ResponseResult> DeleteByIdAsync(int[] shipmentIds, CancellationToken cancellationToken = default(CancellationToken))
  //      {
  //          var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Shipments)));

  //          try
  //          {
		//		using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
  //              {
  //                  using (var dbContext = _dbContext())
  //                  {
  //                      dbContext.RemoveByWhere<Domain.Shipments>(x => shipmentIds.Contains(x.ShipmentId));

  //                      await dbContext.SaveChangesAsync(cancellationToken);
  //                      scope.Complete();

  //                      return result;
  //                  }
  //              }
  //          }
  //          catch (Exception ex)
  //          {
  //              result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Shipments)), ex);
  //          }

  //          return result;
  //      }
    }
}
