using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class PSerialNumber
    {
        //[Key]

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PSerialNumberID { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "InvoiceName1", ResourceType = typeof(Resources.Resource))]
        public string InvoiceName1 { get; set; }

        [Display(Name = "InvoiceSerialNo", ResourceType = typeof(Resources.Resource))]
        public int InvoiceSerialNo1 { get; set; }

        [Display(Name = "InvoiceSerialNo", ResourceType = typeof(Resources.Resource))]
        public int InvoiceSerialNo11 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "InvoiceName2", ResourceType = typeof(Resources.Resource))]
        public string InvoiceName2 { get; set; }

        [Display(Name = "InvoiceSerialNo", ResourceType = typeof(Resources.Resource))]
        public int InvoiceSerialNo2 { get; set; }

        [Display(Name = "InvoiceSerialNo", ResourceType = typeof(Resources.Resource))]
        public int InvoiceSerialNo22 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "InvoiceName3", ResourceType = typeof(Resources.Resource))]
        public string InvoiceName3 { get; set; }

        [Display(Name = "InvoiceSerialNo", ResourceType = typeof(Resources.Resource))]
        public int InvoiceSerialNo3 { get; set; }

        [Display(Name = "InvoiceSerialNo", ResourceType = typeof(Resources.Resource))]
        public int InvoiceSerialNo33 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string InvoiceName4 { get; set; }

        public int InvoiceSerialNo4 { get; set; }

        public int InvoiceSerialNo44 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string InvoiceName5 { get; set; }

        public int InvoiceSerialNo5 { get; set; }

        public int InvoiceSerialNo55 { get; set; }


        [Display(Name = "CollectionNo", ResourceType = typeof(Resources.Resource))]
        public int CollectionNo { get; set; }

        [Display(Name = "CollectionReceiptNo", ResourceType = typeof(Resources.Resource))]
        public int CollectionReceiptNo { get; set; }

        [Display(Name = "PaymentNo", ResourceType = typeof(Resources.Resource))]
        public int PaymentNo { get; set; }

        [Display(Name = "PaymentReceiptNo", ResourceType = typeof(Resources.Resource))]
        public int PaymentReceiptNo { get; set; }

        [Display(Name = "LastRegistrationNo", ResourceType = typeof(Resources.Resource))]
        public int RegisterNo { get; set; }

        [Display(Name = "BondNo", ResourceType = typeof(Resources.Resource))]
        public int BondNo { get; set; }

        [Display(Name = "CheckNo", ResourceType = typeof(Resources.Resource))]
        public int CheckNo { get; set; }

        [Display(Name = "MailOrderNo", ResourceType = typeof(Resources.Resource))]
        public int MailOrderNo { get; set; }

        [Display(Name = "OtsNo1", ResourceType = typeof(Resources.Resource))]
        public int OtsNo1 { get; set; }

        [Display(Name = "OtsNo2", ResourceType = typeof(Resources.Resource))]
        public int OtsNo2 { get; set; }

        [Display(Name = "CreditCardNo", ResourceType = typeof(Resources.Resource))]
        public int CreditCardNo { get; set; }

        [Display(Name = "KmhNo", ResourceType = typeof(Resources.Resource))]
        public int KmhNo { get; set; }


        public int GovernmentPromotionNo { get; set; }

        [Display(Name = "LastAccountSerialNo", ResourceType = typeof(Resources.Resource))]
        public int AccountSerialNo { get; set; }
        public int Number2 { get; set; }
        public int Number3 { get; set; }
        public int Number4 { get; set; }
        public int Number5 { get; set; }
    }

}
