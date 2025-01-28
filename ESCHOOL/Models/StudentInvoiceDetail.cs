using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentInvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentInvoiceDetailID { get; set; }
        public int StudentInvoiceID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int SchoolFeeID { get; set; }
        
        [Column(TypeName = "char(9)")]
        public string Period { get; set; }
        public int InvoiceSerialNo { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Explanation", ResourceType = typeof(Resources.Resource))]
        public string Explanation { get; set; }

        [Display(Name = "UnitPrice", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public Nullable<byte> Quantity { get; set; }

        [Display(Name = "Discount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal Discount { get; set; }

        [Display(Name = "TaxPercent", ResourceType = typeof(Resources.Resource))]
        public Nullable<byte> TaxPercent { get; set; }

        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal Tax { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public bool InvoiceStatus { get; set; }
    }
}
