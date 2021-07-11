namespace Sve.Service
{
    using AutoMapper;
    using JetBrains.Annotations;
    using JxNet.Extensions.CacheManager;
    using Microsoft.EntityFrameworkCore;
    //using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sve.Service.Data;
    using Sve.Service.Mappers;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServiceInterfaces([NotNull] this IServiceCollection services/*, IConfiguration configuration*/, [NotNull] string connectionString)
        {
            var options = new DbContextOptionsBuilder<SveServiceDbContext>().UseSqlServer(connectionString).Options;
            services.AddSingleton(options);
            services.AddTransient<ISveServiceDbContext, SveServiceDbContext>();
            services.AddSingleton<Func<ISveServiceDbContext>>(prov => () => prov.GetRequiredService<ISveServiceDbContext>());

            //services.AddTransient<IProductCategoryService, ProductCategoryService>();
            //services.AddTransient<IUsersService, UserService>();

            //services.AddTransient<Sve.Contract.Interface.Iam.IPermissionsService, Sve.Service.Impl.Iam.PermissionsService>();
            //services.AddTransient<Sve.Contract.Interface.Iam.IRoleService, Sve.Service.Impl.Iam.RoleService>();
            //services.AddTransient<Sve.Contract.Interface.Iam.IRolePermissionsService, Sve.Service.Impl.Iam.RolePermissionsService>();
            //services.AddTransient<Sve.Contract.Interface.Iam.IUserRolesService, Sve.Service.Impl.Iam.UserRolesService>();
            services.AddTransient<Contract.Interface.Iam.IUsersService, Impl.Iam.UserService>();
            //services.AddTransient<Sve.Contract.Interface.Logs.IDatabaseLogService, Sve.Service.Impl.Logs.DatabaseLogService>();
            //services.AddTransient<Sve.Contract.Interface.Logs.IErrorLogService, Sve.Service.Impl.Logs.ErrorLogService>();

            services.AddTransient<Sve.Contract.Interface.Accounts.IAccountTypesService, Sve.Service.Impl.Accounts.AccountTypesService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.IBankDetailsService, Sve.Service.Impl.Accounts.BankDetailsService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.IAccountDetailsService, Sve.Service.Impl.Accounts.AccountDetailsService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.ICustomersService, Sve.Service.Impl.Accounts.CustomersService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.ILedgerGroupsService, Sve.Service.Impl.Accounts.LedgerGroupsService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.IPayModesService, Sve.Service.Impl.Accounts.PayModesService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.ITransactionDetailsService, Sve.Service.Impl.Accounts.TransactionDetailsService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.ITransactionsService, Sve.Service.Impl.Accounts.TransactionsService>();
            services.AddTransient<Sve.Contract.Interface.Accounts.IVoucherTypesService, Sve.Service.Impl.Accounts.VoucherTypesService>();

            services.AddTransient<Contract.Interface.Product.IProductCategoryService, Impl.Product.ProductCategoryService>();
            services.AddTransient<Sve.Contract.Interface.Product.IBrandsService, Sve.Service.Impl.Product.BrandsService>();
            services.AddTransient<Sve.Contract.Interface.Product.IColorsService, Sve.Service.Impl.Product.ColorsService>();
            services.AddTransient<Sve.Contract.Interface.Product.IGradesService, Sve.Service.Impl.Product.GradesService>();
            services.AddTransient<Sve.Contract.Interface.Product.IMaterialTypesService, Sve.Service.Impl.Product.MaterialTypesService>();
            services.AddTransient<Sve.Contract.Interface.Product.ISizesService, Sve.Service.Impl.Product.SizesService>();
            services.AddTransient<Contract.Interface.Product.IProductDetailsService, Impl.Product.ProductDetailsService>();
            services.AddTransient<Contract.Interface.Product.IProductImagesService, Impl.Product.ProductImagesService>();
            services.AddTransient<Contract.Interface.Product.IStockGroupsService, Impl.Product.StockGroupsService>();
            services.AddTransient<Contract.Interface.Product.ITaxSlabsService, Impl.Product.TaxSlabsService>();
            services.AddTransient<Contract.Interface.Product.IProductCartService, Impl.Product.ProductCartService>();
            services.AddTransient<Sve.Contract.Interface.Product.IUnitMeasureService, Sve.Service.Impl.Product.UnitMeasureService>();
            services.AddTransient<Contract.Interface.Purchasing.IOrderDetailService, Impl.Purchasing.OrderDetailService>();
            services.AddTransient<Contract.Interface.Purchasing.IOrderHeaderService, Impl.Purchasing.OrderHeaderService>();
            services.AddTransient<Contract.Interface.Purchasing.IShipmentsService, Impl.Purchasing.ShipmentsService>();
            services.AddTransient<Contract.Interface.Purchasing.IVendorsService, Impl.Purchasing.VendorsService>();
            services.AddTransient<Contract.Interface.Purchasing.ICreditNotesService, Impl.Purchasing.CreditNotesService>();
            services.AddTransient<Contract.Interface.Purchasing.ICreditNotesInOrdersService, Impl.Purchasing.CreditNotesInOrdersService>();
            services.AddTransient<Contract.Interface.Purchasing.IPurchaseReturnsService, Impl.Purchasing.PurchaseReturnsService>();
            services.AddTransient<Contract.Interface.Sales.ICustomersService, Impl.Sales.CustomersService>();
            services.AddTransient<Contract.Interface.Sales.IOrderDetailService, Impl.Sales.OrderDetailService>();
            services.AddTransient<Contract.Interface.Sales.IOrderHeaderService, Impl.Sales.OrderHeaderService>();
            services.AddTransient<Contract.Interface.Sales.IReportService, Impl.Sales.ReportService>();

            services.AddJxNetMemoryCacheService();

            //Registering the auto mapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfiles>(), AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
