namespace Sve.Contract.Interface.Product
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Product;
    using JxNet.Core;
    public interface IProductImagesService
    {
        Task<ResponseResult> CreateAsync(List<ProductImages> entity);
        Task<(ResponseResult response, List<ProductImages> items)> DeleteByIdAsync(int[] imageIds);
    }
}