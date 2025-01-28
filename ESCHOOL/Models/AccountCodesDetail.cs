using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class AccountCodesDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountCodeDetailID { get; set; }
        public int AccountCodeID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "AuthorizedPersonName", ResourceType = typeof(Resources.Resource))]
        public string AuthorizedPersonName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string Mobile { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        public string Phone1 { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public string BankName1 { get; set; }

        [Column(TypeName = "nvarchar(26)")]
        [Display(Name = "Iban", ResourceType = typeof(Resources.Resource))]
        public string BankIBAN1 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public string BankName2 { get; set; }

        [Column(TypeName = "nvarchar(26)")]
        [Display(Name = "Iban", ResourceType = typeof(Resources.Resource))]
        public string BankIBAN2 { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Explanation", ResourceType = typeof(Resources.Resource))]
        public string Explanation { get; set; }


        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "InvoiceTitle", ResourceType = typeof(Resources.Resource))]
        public string InvoiceTitle { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "InvoiceAddress", ResourceType = typeof(Resources.Resource))]
        public string InvoiceAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int InvoiceCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int InvoiceTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "Country", ResourceType = typeof(Resources.Resource))]
        public string InvoiceCountry { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string InvoiceZipCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "TaxOffice", ResourceType = typeof(Resources.Resource))]
        public string InvoiceTaxOffice { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "TaxNumber", ResourceType = typeof(Resources.Resource))]
        public string InvoiceTaxNumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string EMail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "WebAddress", ResourceType = typeof(Resources.Resource))]
        public string WebAddress { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        public string Phone2 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Fax", ResourceType = typeof(Resources.Resource))]
        public string Fax { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resource))]
        public string Notes { get; set; }

        public bool InvoiceProfile { get; set; }
        public Nullable<int> InvoiceTypeParameter { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string ParameterExceptionCode { get; set; }
    }

}
