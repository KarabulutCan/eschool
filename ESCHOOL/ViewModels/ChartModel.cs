using System;

namespace ESCHOOL.ViewModels
{
    public class ChartModel
    {
        public string Source { get; set; }
        public Nullable<bool> Explode { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }

    }
}
