namespace Sve.Contract.Interface.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Sales;
    using Sve.Contract.ViewModels;
    using JxNet.Core;
    public interface IOrderHeaderService
    {
        Task<(int? totalCount, List<SalesOrderHeader> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, SalesOrderHeader filter = null);
        Task<ResponseResult> PlaceOrderAsync(SalesOrderHeader entity);
        //Task<ResponseResult> UpdateAsync(SalesOrderHeader entity);
        //Task<ResponseResult> DeleteByIdAsync(int[] salesOrderIds);
        Task<ResponseResult> ConfirmOrder(int orderId, OrderConfirmRequest request);
        Task<InvoiceModel> GetInvoiceByIdAsync(int salesOrderId);
    }
}