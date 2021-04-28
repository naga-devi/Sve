namespace Sve.Service.Mappers
{
    using System;
    using System.Linq.Expressions;
    using ContactProductCategory = Contract.Models.Product.ProductCategory;
    using DomainProductCategory = Domain.Product.ProductCategory;

    internal static class ProductCateMapper
    {
        internal static Expression<Func<DomainProductCategory, ContactProductCategory>> IQuerableDomainToContract()
        {
            return x => new ContactProductCategory()
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                HasSubCategory = x.HasSubCategory,
                ParentId = x.ParentId
            };
        }

        internal static Func<DomainProductCategory, ContactProductCategory> IEnumerableDomainToContract()
        {
            return x => new ContactProductCategory()
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                HasSubCategory = x.HasSubCategory,
                ParentId = x.ParentId
            };
        }
    }
}
