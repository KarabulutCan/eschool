using System;

namespace ESCHOOL.ViewModels
{
    public class StudentDebtDetailedViewModel
    {
        public int ViewModelId { get; set; }
        public int StudentDebtTableID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int SchoolFeeID { get; set; }
        public string Period { get; set; }
        public string FeeName { get; set; }
        public decimal AmountTable { get; set; }
        public int InstallmentTable { get; set; }
        public DateTime PaymentStartDateTable { get; set; }
    }
}
