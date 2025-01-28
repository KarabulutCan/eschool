using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentPaymentID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Explanation { get; set; }

        [Display(Name = "PaymentDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }

        [Display(Name = "ReceiptNo", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> ReceiptNo { get; set; }

        [Display(Name = "PaymentAmount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal PaymentAmount { get; set; }

        [Display(Name = "BalanceAmount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal BalanceAmount { get; set; }

        [Display(Name = "AccountReceipt", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> AccountReceipt { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string InstallmentNumbers { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string InstallmentIdentities { get; set; }

        
    }
}

