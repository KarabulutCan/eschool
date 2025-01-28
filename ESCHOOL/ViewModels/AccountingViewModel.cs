using ESCHOOL.Models;
using System;

namespace ESCHOOL.ViewModels
{
    public class AccountingViewModel
    {
        public int ViewModelID { get; set; }
        public int UserID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public string SchoolName { get; set; }
        public int AccountingID { get; set; }
        public string Period { get; set; }
        public string ProcessTypeName { get; set; }
        public Nullable<int> VoucherTypeID { get; set; }
        public int VoucherNo { get; set; }
        public Nullable<DateTime> AccountDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountCodeName { get; set; }
        public string CodeTypeName { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<DateTime> DocumentDate { get; set; }
        public string TaxNoOrId { get; set; }
        public string Explanation { get; set; }
        public Nullable<decimal> Debt { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsTransaction { get; set; }

        public Parameter Parameter { get; set; }
        public AccountCodes AccountCodes { get; set; }
        public bool IsPermission { get; set; }
        public string SelectedCulture { get; set; }

        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int StartCode { get; set; }
        public int EndCode { get; set; }

        public int StartVoucherNo { get; set; }
        public int EndVoucherNo { get; set; }

        public string CategoryName { get; set; }
        public AccountCodesDetail AccountCodesDetail { get; set; }
        public int AccountCodeID { get; set; }
        public int AccountCodeDetailID { get; set; }
        


    }
}
