using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentInvoiceAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentInvoiceAddressID { get; set; }
        public int StudentID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "InvoiceTitle", ResourceType = typeof(Resources.Resource))]
        public string InvoiceTitle { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
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
        public string Phone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Fax", ResourceType = typeof(Resources.Resource))]
        public string Fax { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resource))]
        public string Notes { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "AccountCode", ResourceType = typeof(Resources.Resource))]
        public string AccountCode { get; set; }

        [Display(Name = "InvoiceProfile", ResourceType = typeof(Resources.Resource))]
        public bool InvoiceProfile { get; set; }

        [Display(Name = "InvoiceType", ResourceType = typeof(Resources.Resource))]
        public int InvoiceTypeParameter { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string ParameterExceptionCode { get; set; }

        [Display(Name = "InvoiceDetailed", ResourceType = typeof(Resources.Resource))]
        public bool IsInvoiceDetailed { get; set; }

        [Display(Name = "InvoiceDiscount", ResourceType = typeof(Resources.Resource))]
        public bool IsInvoiceDiscount { get; set; }

        public bool isEmpty
        {
            get
            {
                return (
                        string.IsNullOrWhiteSpace(InvoiceTitle) &&
                        string.IsNullOrWhiteSpace(InvoiceAddress) &&
                        string.IsNullOrWhiteSpace(InvoiceCountry) &&
                        string.IsNullOrWhiteSpace(InvoiceZipCode) &&
                        string.IsNullOrWhiteSpace(InvoiceTaxOffice) &&
                        string.IsNullOrWhiteSpace(InvoiceTaxNumber) &&
                        string.IsNullOrWhiteSpace(EMail) &&
                        string.IsNullOrWhiteSpace(Notes) &&
                        string.IsNullOrWhiteSpace(AccountCode) &&

                        StudentInvoiceAddressID == 0 &&
                        InvoiceCityParameterID == 0 &&
                        InvoiceTownParameterID == 0 &&
                        InvoiceProfile == false &&
                        InvoiceTypeParameter == 0 &&
                        IsInvoiceDetailed == false &&
                        IsInvoiceDiscount == false);
            }
        }
    }
}
