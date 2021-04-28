using System.Collections.Generic;

namespace JxNet.Extensions.WebHost.Models
{
    public class ChartItemsViewModel
    {
        public string Name { get; set; }
        public List<ChartSeriesViewModel> Series { get; set; }
    }

    public class ChartSeriesViewModel
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
