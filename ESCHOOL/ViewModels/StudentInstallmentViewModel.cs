using System;

namespace ESCHOOL.ViewModels
{
    public class StudentInstallmentViewModel
    {
        public int StudentInstallmentID { get; set; }
        public string StudentName { get; set; }
        public string ParentName { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public Nullable<DateTime> InstallmentDate { get; set; }
        public string FeeName { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public string Category { get; set; }
        public decimal InstallmentAmount { get; set; }  
        public decimal PreviousPayment { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public int StatusCategoryID { get; set; }
        public int BankID { get; set; }
        public string Bank { get; set; }
        public string Explanation { get; set; }
        public int AccountReceiptNo { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        public string CheckBankName { get; set; }
        public string CheckNo { get; set; }
        public string Drawer { get; set; }
        public string Endorser { get; set; }

    }
}
