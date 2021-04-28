namespace Sve.Contract.Interface.Product
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Product;
    using Sve.Contract.ViewModels;
    using JxNet.Core;
    public interface IProductDetailsService
    {
        Task<List<ProductDetails>> GetAllProductsAsync();
        Task<(int? totalCount, List<ProductListModel> items)> GetProductsAsync(ProductFilterViewModel queryParameters);
        Task<ProductDetails> GetByIdAsync(int productId);
        Task<int?> GetCategoryIdAsync(int productId);
        Task<ResponseResult> SaveAsync(ProductDetails entity);
        Task<(ResponseResult response, List<ProductImages> images)> DeleteByIdAsync(int?[] productIds);
        Task<object> GetProductStatusAsync(int productId);
    }
}