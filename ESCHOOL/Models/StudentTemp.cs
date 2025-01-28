using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentTemp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentTempID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Display(Name = "TransactionDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }

        [Display(Name = "PaymentType", ResourceType = typeof(Resources.Resource))]
        public int PaymentTypeCategoryID { get; set; }

        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public int BankID { get; set; }

        [Display(Name = "Installment", ResourceType = typeof(Resources.Resource))]
        public int Installment { get; set; }

        [Display(Name = "Single", ResourceType = typeof(Resources.Resource))]
        public bool IsSingleNamePaper { get; set; }

        [Display(Name = "CashPayment", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal CashPayment { get; set; }
        public int ReceiptNo { get; set; }
        public int AccountReceipt { get; set; }
        public int CollectionReceipt { get; set; }

        [Display(Name = "SubTotal", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "char(1)")]
        public string RefundSW { get; set; }

        [Display(Name = "RefundAmount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal RefundAmount1 { get; set; }

        [Display(Name = "RefundAmount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal RefundAmount2 { get; set; }

        [Display(Name = "RefundAmount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal RefundAmount3 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string DebitAccountCodeID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string RefundAccountCodeID1 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string RefundAccountCodeID2 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string RefundAccountCodeID3 { get; set; }

        [Display(Name = "RefundedDueDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> RefundedDueDate { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string AboutRefund1 { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string AboutRefund2 { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string AboutRefund3 { get; set; }

        public bool IsCancel { get; set; }

        public bool IsCancelPreviousRefund { get; set; }

        public bool CancellationOption { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string CancelCreditNoID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string CancelDebtNoID { get; set; }

        [Display(Name = "Credit", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal CancelCredit { get; set; }

        [Display(Name = "Debt", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal CancelDebt { get; set; }

        [Display(Name = "CancelDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> CancelDate { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        [Display(Name = "AboutCancellation", ResourceType = typeof(Resources.Resource))]
        public string AboutCancellation { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string StatuCategoryName { get; set; }

        public bool IsRegistrationSuspension { get; set; }
        public bool IsShowDept { get; set; }
        public bool IsPension { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string AccountCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string AccountExplanation { get; set; }

    }
}

