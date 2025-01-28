using ESCHOOL.Models;
using System;

namespace ESCHOOL.ViewModels
{
    public class SchoolFeeViewModel
    {
        public int ViewModelID { get; set; }
        public int SchoolFeeID { get; set; }
        public int StudentDebtID { get; set; }
        public int FeeCategory { get; set; }
        public int CategorySubID { get; set; }
        public string SchoolFeeName { get; set; }
        public decimal SchoolFeeTypeAmount { get; set; }
        public string StockCode { get; set; }
        public int StockQuantity { get; set; }
        public Nullable<byte> Tax { get; set; }
        public int SortOrder { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public Nullable<bool> IsSelect { get; set; }
        public string Period { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public SchoolFeeTable SchoolFeeTable { get; set; }
        public SchoolFee SchoolFee { get; set; }
        public SchoolInfo SchoolInfo { get; set; }
        public Parameter Parameter { get; set; }

    }
}
