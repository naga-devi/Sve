namespace Sve.Contract.Interface.Product
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Product;
    using Sve.Contract.ViewModels;
    using JxNet.Core;
    public interface IProductCartService
    {
        Task<(int? totalCount, List<ProductListModel> items)> GetProductsAsync(ProductFilterViewModel queryParameters);
        Task<ProductViewModel> GetProductByIdAsync(int productId);
        Task<StockGroups> GetProductPriceAsync(int productId, int sizeId, int brandId, int materialTypeId, int gradeId);
    }
}
