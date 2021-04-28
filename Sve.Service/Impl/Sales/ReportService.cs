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
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class ReportService : IReportService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public ReportService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }


        public async Task<List<Models.SalesOrderHeader>> GetDayLedger(DateTime saleDate)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.SalesOrderHeader>()
                .Where(x => EF.Functions.DateDiffDay(saleDate, x.OrderDate) == 0)
                .Select(x => new Domain.SalesOrderHeader
                {
                    TotalQuantity = x.TotalQuantity,
                    TotalAmount = x.TotalAmount,
                    DiscountPercentage = x.DiscountPercentage,
                    NetAmount = x.NetAmount,
                    Freight = x.Freight,
                    RoundOffAmount = x.RoundOffAmount,
                    GrandTotal = x.GrandTotal,
                    PaidAmount = x.PaidAmount,
                    BalanceAmount = x.BalanceAmount,
                    Paymode = x.Paymode,
                })
                .AsNoTracking()
                .ToListAsync();

            return result.HasItems() ? _mapper.Map<List<Models.SalesOrderHeader>>(result) : null;
        }
    }
}
