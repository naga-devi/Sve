namespace Sve.Contract.Interface.Sales
{
    using Sve.Contract.Models.Sales;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JxNet.Core;
    public interface IReportService
    {
        Task<List<SalesOrderHeader>> GetDayLedger(DateTime saleDate);
    }
}