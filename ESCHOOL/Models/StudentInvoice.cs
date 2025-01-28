using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentInvoiceID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int StudentInvoiceAddressID { get; set; }

        [Column(TypeName = "char(9)")]
        public string Period { get; set; }

        public int InvoiceSerialNo { get; set; }

        [Display(Name = "InvoiceDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> InvoiceDate { get; set; }

        [Display(Name = "InvoiceCutDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> InvoiceCutDate { get; set; }
        public Nullable<TimeSpan> InvoiceCutTime { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string DExplanation { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> DUnitPrice { get; set; }

        [MaxLength(100)]
        public Nullable<byte> DQuantity { get; set; }

        [Display(Name = "Discount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public Nullable<decimal> DDiscount { get; set; }

        [Display(Name = "TaxPercent", ResourceType = typeof(Resources.Resource))]
        public Nullable<byte> DTaxPercent { get; set; }

        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public Nullable<decimal> DTax { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public Nullable<decimal> DAmount { get; set; }

        public Nullable<int> WithholdingPercent1 { get; set; }
        public Nullable<int> WithholdingPercent2 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string WithholdingCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string WithholdingExplanation { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> WithholdingTax { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> WithholdingTotal { get; set; }

        public bool IsPlanned { get; set; }
        public bool InvoiceType { get; set; }
        public bool InvoiceStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsBatchPrint { get; set; }
    }

}
