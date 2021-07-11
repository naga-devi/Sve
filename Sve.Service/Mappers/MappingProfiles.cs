namespace Sve.Service.Mappers
{
    using AutoMapper;
    using Models = Contract.Models;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Models.Iam.Permissions, Domain.Iam.Permissions>();
            CreateMap<Domain.Iam.Permissions, Models.Iam.Permissions>();
            CreateMap<Models.Iam.Role, Domain.Iam.Role>();
            CreateMap<Domain.Iam.Role, Models.Iam.Role>();
            CreateMap<Models.Iam.RolePermissions, Domain.Iam.RolePermissions>();
            CreateMap<Domain.Iam.RolePermissions, Models.Iam.RolePermissions>();
            CreateMap<Models.Iam.UserRoles, Domain.Iam.UserRoles>();
            CreateMap<Domain.Iam.UserRoles, Models.Iam.UserRoles>();
            CreateMap<Models.Iam.Users, Domain.Iam.Users>();
            CreateMap<Domain.Iam.Users, Models.Iam.Users>();

            CreateMap<Models.Accounts.AccountType, Domain.Accounts.AccountType>();
            CreateMap<Domain.Accounts.AccountType, Models.Accounts.AccountType>();
            CreateMap<Models.Accounts.BankDetail, Domain.Accounts.BankDetail>();
            CreateMap<Domain.Accounts.BankDetail, Models.Accounts.BankDetail>();
            CreateMap<Models.Accounts.AccountDetail, Domain.Accounts.AccountDetail>();
            CreateMap<Domain.Accounts.AccountDetail, Models.Accounts.AccountDetail>();
            CreateMap<Models.Accounts.Customer, Domain.Accounts.Customer>();
            CreateMap<Domain.Accounts.Customer, Models.Accounts.Customer>();
            CreateMap<Models.Accounts.LedgerGroup, Domain.Accounts.LedgerGroup>();
            CreateMap<Domain.Accounts.LedgerGroup, Models.Accounts.LedgerGroup>();
            CreateMap<Models.Accounts.PayMode, Domain.Accounts.PayMode>();
            CreateMap<Domain.Accounts.PayMode, Models.Accounts.PayMode>();
            CreateMap<Models.Accounts.TransactionDetail, Domain.Accounts.TransactionDetail>();
            CreateMap<Domain.Accounts.TransactionDetail, Models.Accounts.TransactionDetail>();
            CreateMap<Models.Accounts.Transactions, Domain.Accounts.Transaction>();
            CreateMap<Domain.Accounts.Transaction, Models.Accounts.Transactions>();
            CreateMap<Models.Accounts.VoucherType, Domain.Accounts.VoucherType>();
            CreateMap<Domain.Accounts.VoucherType, Models.Accounts.VoucherType>();

            //CreateMap<Models.Logs.DatabaseLog, Domain.Logs.DatabaseLog>();
            //CreateMap<Domain.Logs.DatabaseLog, Models.Logs.DatabaseLog>();
            //CreateMap<Models.Logs.ErrorLog, Domain.Logs.ErrorLog>();
            //CreateMap<Domain.Logs.ErrorLog, Models.Logs.ErrorLog>();
            CreateMap<Models.Product.ProductCategory, Domain.Product.ProductCategory>();
            CreateMap<Domain.Product.ProductCategory, Models.Product.ProductCategory>();
            CreateMap<Models.Product.ProductDetails, Domain.Product.ProductDetails>();
            CreateMap<Domain.Product.ProductDetails, Models.Product.ProductDetails>();
            CreateMap<Models.Product.ProductImages, Domain.Product.ProductImages>();
            CreateMap<Domain.Product.ProductImages, Models.Product.ProductImages>();
            CreateMap<Models.Product.StockGroups, Domain.Product.StockGroups>();
            CreateMap<Domain.Product.StockGroups, Models.Product.StockGroups>();
            CreateMap<Models.Product.TaxSlabs, Domain.Product.ProductTaxSlabs>();
            CreateMap<Domain.Product.ProductTaxSlabs, Models.Product.TaxSlabs>();

            CreateMap<Models.Product.Grades, Domain.Product.Grades>();
            CreateMap<Domain.Product.Grades, Models.Product.Grades>();
            CreateMap<Models.Product.Colors, Domain.Product.Colors>();
            CreateMap<Domain.Product.Colors, Models.Product.Colors>();
            CreateMap<Models.Product.Brands, Domain.Product.ProductBrands>();
            CreateMap<Domain.Product.ProductBrands, Models.Product.Brands>();
            CreateMap<Models.Product.MaterialTypes, Domain.Product.ProductMaterialTypes>();
            CreateMap<Domain.Product.ProductMaterialTypes, Models.Product.MaterialTypes>();
            CreateMap<Models.Product.Sizes, Domain.Product.ProductSizes>();
            CreateMap<Domain.Product.ProductSizes, Models.Product.Sizes>();

            CreateMap<Models.Purchasing.CreditNotes, Domain.Purchasing.CreditNotes>();
            CreateMap<Domain.Purchasing.CreditNotes, Models.Purchasing.CreditNotes>();
            CreateMap<Models.Purchasing.CreditNotesInOrders, Domain.Purchasing.CreditNotesInOrders>();
            CreateMap<Domain.Purchasing.CreditNotesInOrders, Models.Purchasing.CreditNotesInOrders>();

            CreateMap<Models.Purchasing.PurchaseOrderDetail, Domain.Purchasing.PurchaseOrderDetail>();
            CreateMap<Domain.Purchasing.PurchaseOrderDetail, Models.Purchasing.PurchaseOrderDetail>();
            CreateMap<Models.Purchasing.PurchaseOrderHeader, Domain.Purchasing.PurchaseOrderHeader>();
            CreateMap<Domain.Purchasing.PurchaseOrderHeader, Models.Purchasing.PurchaseOrderHeader>();
            CreateMap<Models.Purchasing.Shipments, Domain.Purchasing.Shipments>();
            CreateMap<Domain.Purchasing.Shipments, Models.Purchasing.Shipments>();
            CreateMap<Models.Purchasing.Vendors, Domain.Purchasing.Vendors>();
            CreateMap<Domain.Purchasing.Vendors, Models.Purchasing.Vendors>();
            CreateMap<Models.Sales.Customers, Domain.Sales.Customers>();
            CreateMap<Domain.Sales.Customers, Models.Sales.Customers>();
            CreateMap<Models.Sales.SalesOrderDetails, Domain.Sales.SalesOrderDetails>();
            CreateMap<Domain.Sales.SalesOrderDetails, Models.Sales.SalesOrderDetails>();
            CreateMap<Models.Sales.SalesOrderHeader, Domain.Sales.SalesOrderHeader>();
            CreateMap<Domain.Sales.SalesOrderHeader, Models.Sales.SalesOrderHeader>();

            CreateMap<Models.Sales.CustomersInOrders, Domain.Sales.CustomersInOrders>();
            CreateMap<Domain.Sales.CustomersInOrders, Models.Sales.CustomersInOrders>();
        }
    }
}
