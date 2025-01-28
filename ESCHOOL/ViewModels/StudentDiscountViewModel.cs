using System;

namespace ESCHOOL.ViewModels
{
    public class StudentDiscountViewModel
    {
        public int ViewModelId { get; set; }
        public int StudentDiscountID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int SchoolFeeID { get; set; }
        public int StudentDebtID { get; set; }
        public int DiscountTableID { get; set; }

        public string Period { get; set; }
        public string DiscountName { get; set; }
        public byte DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal DiscountApplied { get; set; }
        public Nullable<bool> IsPasswordRequired { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public string DiscountFeeName { get; set; }
    }
}
