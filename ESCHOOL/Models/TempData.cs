using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class TempData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TempDataID { get; set; }
        public int StudentID { get; set; }

        [Display(Name = "PaymentDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> InstallmentDate { get; set; }

        public Nullable<int> InstallmentNo { get; set; }

        [Display(Name = "PaymentType", ResourceType = typeof(Resources.Resource))]
        public int CategoryID { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal InstallmentAmount { get; set; }

        [Display(Name = "Bank", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> BankID { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "CheckCardNo", ResourceType = typeof(Resources.Resource))]
        public string CheckCardNo { get; set; }

        [Column(TypeName = "money")]
        public decimal PreviousPayment { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string FeeName { get; set; }

        [Column(TypeName = "char(10)")]
        public string Status { get; set; }

        public int StatusCategoryID { get; set; }

        public int AccountReceiptNo { get; set; }

        [Display(Name = "PaymentDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [Column(TypeName = "money")]
        public decimal CashPayment { get; set; }
    }
}


