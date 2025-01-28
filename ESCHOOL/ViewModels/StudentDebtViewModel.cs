using System;

namespace ESCHOOL.ViewModels
{
    public class StudentDebtViewModel
    {
        public int ViewModelId { get; set; }
        public int StudentDebtID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int SchoolFeeID { get; set; }
        public string Period { get; set; }
        public string FeeName { get; set; }
        public Nullable<bool> SelectFee { get; set; }
        public Nullable<bool> SelectDiscount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public bool IsList { get; set; }
    }
}
