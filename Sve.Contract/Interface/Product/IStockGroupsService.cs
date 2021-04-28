namespace Sve.Contract.Interface.Product
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JxNet.Core;
    using Sve.Contract.Models.Product;

    public interface IStockGroupsService
    {
        Task<(int? totalCount, List<StockGroups> items)> GetByExpressionAsync(int productId, int index, int size, string sortColumn, bool isDescending, StockGroups filter = null);
        Task<StockGroups> GetByIdAsync(int stockGroupId);
        //Task<ResponseResult> CreateAsync(StockGroups entity, int stockedQty, decimal buyPrice);
        Task<ResponseResult> UpdatePriceAsync(StockGroups entity);
    }
}