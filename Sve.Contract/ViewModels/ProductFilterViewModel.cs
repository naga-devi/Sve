namespace Sve.Contract.ViewModels
{
    public class ProductFilterViewModel
    {
        public int? CategoryId { get; set; } = 0;
        public string Name { get; set; }
        public decimal? StartPrice { get; set; } = 0;
        public decimal? EndPrice { get; set; } = 0;
        public int?[] SizeIds { get; set; }
        public int?[] BrandIds { get; set; }
        public int?[] MaterialTypeIds { get; set; }
        public int?[] ColorIds { get; set; }
        public int?[] GradeIds { get; set; }
        public int SortBy { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 12;
    }
}
