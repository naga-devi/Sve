namespace Sve.Service.Impl.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Product;
    using Sve.Service.Data;
    using Models = Contract.Models.Product;
    using Domain = Service.Domain.Product;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class ProductImagesService : IProductImagesService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public ProductImagesService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<ResponseResult> CreateAsync(List<Models.ProductImages> entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.ProductImages)));

            try
            {
                var entityToAdd = _mapper.Map<List<Domain.ProductImages>>(entity);

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                entityToAdd.ForEach(x =>
                {
                    dbContext.Add(x);
                });

                await dbContext.SaveChangesAsync();
                scope.Complete();

                return result;

            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.ProductImages)), ex);
            }

            return result;
        }

        public async Task<(ResponseResult response, List<Models.ProductImages> items)> DeleteByIdAsync(int[] imageIds)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.ProductImages)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var images = await dbContext.GetAsQuerable<Domain.ProductImages>().Where(x => imageIds.Contains(x.ImageId)).AsNoTracking().ToListAsync();
                dbContext.RemoveByWhere<Domain.ProductImages>(x => imageIds.Contains(x.ImageId));

                await dbContext.SaveChangesAsync();
                scope.Complete();

                return (response: result, items: _mapper.Map<List<Models.ProductImages>>(images));
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.ProductImages)), ex);
            }

            return (result, null);
        }
    }
}
