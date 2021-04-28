namespace Sve.Service.Impl.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Sales;
    using Sve.Service.Data;
    using Models = Contract.Models.Sales;
    using Domain = Domain.Sales;
    using System.Linq.Core;
    using ProductModel = Contract.Models.Product;
    using System.Threading;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class OrderDetailService : IOrderDetailService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public OrderDetailService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<Models.SalesOrderDetails> items)> GetByExpressionAsync(int salesOrderId, int index, int size, string sortColumn, bool isDescending, Models.SalesOrderDetails filter = null)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.SalesOrderDetails>()
                    .Where(x => x.SalesOrderId == salesOrderId)
                    .Select(x => new Models.SalesOrderDetails
                    {
                        SalesOrderId = x.SalesOrderId,
                        Id = x.Id,
                        StockGroupId = (int)x.StockGroupId,
                        //LineTotal = x.LineTotal,
                        OrderQty = x.OrderQty,
                        Status = x.Status,
                        UnitPrice = x.UnitPrice,
                        CgstAmount= x.CgstAmount,
                        SgstAmount= x.SgstAmount,
                        StockGroup = new ProductModel.StockGroups
                        {
                            Product = new ProductModel.ProductDetails
                            {
                                Name = x.StockGroup.Product.Name
                            }
                        }
                    })
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, result?.Items?.ToList());
                }
            }

            return (0, null);
        }

        public async Task<List<ProductModel.StockGroups>> GetByOrderId(int salesOrderId, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Service.Domain.Product.StockGroups>()
                .Where(x => x.SalesOrderDetails.Any(m => m.SalesOrderId == salesOrderId))
                .Select(x => new ProductModel.StockGroups
                {
                    ProductId = x.ProductId,
                    Product = new ProductModel.ProductDetails
                    {
                        TaxSlabId = x.Product.TaxSlabId,
                        TaxSlab = new ProductModel.TaxSlabs
                        {
                            TotalTax = x.Product.TaxSlab.TotalTax,
                            Sgst = x.Product.TaxSlab.Sgst,
                            Cgst = x.Product.TaxSlab.Cgst,
                        },
                        Name = x.Product.Name,
                        Category = new ProductModel.ProductCategory
                        {
                            Name = x.Product.Category.Name
                        }
                    },
                    MaterialType = new ProductModel.MaterialTypes
                    {
                        Name = x.MaterialType.Name
                    },
                    Size = new ProductModel.Sizes
                    {
                        Name = x.Size.Name
                    },
                    Brand = new ProductModel.Brands
                    {
                        Name = x.Brand.Name
                    },
                    Color = new ProductModel.Colors
                    {
                        Name = x.Color.Name
                    },
                    Grade = new ProductModel.Grades
                    {
                        Name = x.Grade.Name
                    },
                    SalesOrderDetails = x.SalesOrderDetails.Select(p => new Models.SalesOrderDetails
                    {
                        OrderQty = p.OrderQty,
                        UnitPrice = p.UnitPrice,
                        CgstAmount = p.CgstAmount,
                        SgstAmount = p.SgstAmount,
                        Status = p.Status,
                    }).ToList()
                });

            //var sql = query.ToSql();

            return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
