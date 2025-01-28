using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class SchoolInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Logo", ResourceType = typeof(Resources.Resource))]
        public string LogoName { get; set; }

        [NotMapped]
        [DisplayName("Logo")]
        [Display(Name = "Logo", ResourceType = typeof(Resources.Resource))]
        public IFormFile LogoFile { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Photo", ResourceType = typeof(Resources.Resource))]
        public string PhotoName { get; set; }

        [NotMapped]
        [DisplayName("Photo")]
        [Display(Name = "Photo", ResourceType = typeof(Resources.Resource))]
        public IFormFile PhotoFile { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "CompanyName", ResourceType = typeof(Resources.Resource))]
        public string CompanyName { get; set; }

        [Column(TypeName = "nvarchar(3)")]
        [Display(Name = "CompanyShortCode", ResourceType = typeof(Resources.Resource))]
        public string CompanyShortCode { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "CompanyShortName", ResourceType = typeof(Resources.Resource))]
        public string CompanyShortName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "CompanyNameForBond", ResourceType = typeof(Resources.Resource))]
        public string CompanyNameForBond { get; set; }

        [Display(Name = "CityNameForBond", ResourceType = typeof(Resources.Resource))]
        public int CityNameForBondID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DataType(DataType.MultilineText)]
        [StringLength(250)]
        [Display(Name = "CompanyAddress", ResourceType = typeof(Resources.Resource))]
        public string CompanyAddress { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        public string Phone1 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        public string Phone2 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string MobilePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Fax", ResourceType = typeof(Resources.Resource))]
        public string Fax { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "TaxOffice", ResourceType = typeof(Resources.Resource))]
        public string TaxOffice { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "TaxNo", ResourceType = typeof(Resources.Resource))]
        public string TaxNo { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "CompanyEmail", ResourceType = typeof(Resources.Resource))]
        public string CompanyEmail { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "WebSite", ResourceType = typeof(Resources.Resource))]
        public string WebSite { get; set; }


        [Display(Name = "AuthorizedPersonHeader1", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "nvarchar(30)")]
        public string AuthorizedPersonName1 { get; set; }

        [Display(Name = "AuthorizedPersonHeader2", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "nvarchar(30)")]
        public string AuthorizedPersonName2 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "AuthorizedPersonHeader3", ResourceType = typeof(Resources.Resource))]
        public string AuthorizedPersonName3 { get; set; }

        [Display(Name = "NameOnReceipt", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> NameOnReceipt { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "FormTitle", ResourceType = typeof(Resources.Resource))]
        public string FormTitle { get; set; }

        public bool FormOpt { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "EmailSMTPHost", ResourceType = typeof(Resources.Resource))]
        public string EmailSMTPHost { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
        public string EmailSMTPUserName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public string EmailSMTPPassword { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "EmailSMTPAddress", ResourceType = typeof(Resources.Resource))]
        public string EmailSMTPAddress { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "EmailSMTPPort", ResourceType = typeof(Resources.Resource))]
        public string EmailSMTPPort { get; set; }

        [Display(Name = "EmailSMTPSsl", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> EmailSMTPSsl { get; set; }

        [Display(Name = "CopiesOfForm", ResourceType = typeof(Resources.Resource))]
        public Nullable<short> CopiesOfForm { get; set; }

        [Display(Name = "PrintQuantity", ResourceType = typeof(Resources.Resource))]
        public Nullable<short> PrintQuantity { get; set; }

        [Display(Name = "InvoiceOnDate", ResourceType = typeof(Resources.Resource))]
        public bool InvoiceOnDate { get; set; }

        [Display(Name = "SchoolYearStart", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> SchoolYearStart { get; set; }

        [Display(Name = "SchoolYearEnd", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> SchoolYearEnd { get; set; }

        [Display(Name = "FinancialYearStart", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> FinancialYearStart { get; set; }

        [Display(Name = "FinancialYearEnd", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> FinancialYearEnd { get; set; }

        [Display(Name = "DefaultInstallment", ResourceType = typeof(Resources.Resource))]
        public int DefaultInstallment { get; set; }

        [Display(Name = "CurrencyDecimalPlaces", ResourceType = typeof(Resources.Resource))]
        public short CurrencyDecimalPlaces { get; set; }

        [Display(Name = "DefaultPaymentType", ResourceType = typeof(Resources.Resource))]
        public int DefaultPaymentTypeCategoryID { get; set; }

        [Display(Name = "DefaultBank", ResourceType = typeof(Resources.Resource))]
        public int DefaultBankID { get; set; }

        [Display(Name = "ShowDebt", ResourceType = typeof(Resources.Resource))]
        public bool DefaultShowDept { get; set; }

        [Display(Name = "DateFormat", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateFormat { get; set; }

        [Display(Name = "IsShowFeesBottomLine", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> IsShowFeesBottomLine { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "TimeZoneLocation", ResourceType = typeof(Resources.Resource))]
        public string TimeZoneLocation { get; set; }

        [Display(Name = "IsActive", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> IsActive { get; set; }

        [Display(Name = "EIIsActive", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> EIIsActive { get; set; }

        [Display(Name = "IntegratorName", ResourceType = typeof(Resources.Resource))]
        public int EIIntegratorNameID { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
        public string EIUserName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public string EIUserPassword { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        [Display(Name = "ASerialCode", ResourceType = typeof(Resources.Resource))]
        public string EIInvoiceSerialCode1 { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        [Display(Name = "ESerialCode", ResourceType = typeof(Resources.Resource))]
        public string EIInvoiceSerialCode2 { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Title", ResourceType = typeof(Resources.Resource))]
        public string EITitle { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string EIAddress { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int EITownID { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int EICityID { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "Country", ResourceType = typeof(Resources.Resource))]
        public string EICountry { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
        public string EIPhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Fax", ResourceType = typeof(Resources.Resource))]
        public string EIFax { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "TaxOffice", ResourceType = typeof(Resources.Resource))]
        public string EITaxOffice { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "TaxNo", ResourceType = typeof(Resources.Resource))]
        public string EITaxNo { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "TradeRegisterNo", ResourceType = typeof(Resources.Resource))]
        public string EITradeRegisterNo { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "MersisNo", ResourceType = typeof(Resources.Resource))]
        public string EIMersisNo { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string EIEMail { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "WebAddress", ResourceType = typeof(Resources.Resource))]
        public string EIWebAddress { get; set; }

        [Column(TypeName = "nvarchar(26)")]
        [Display(Name = "Iban", ResourceType = typeof(Resources.Resource))]
        public string EIIban { get; set; }

        public Nullable<bool> InvoiceProfile { get; set; }
        public Nullable<int> InvoiceTypeParameter { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string ParameterExceptionCode { get; set; }


        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo01", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID01 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo02", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID02 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo03", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID03 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo04", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID04 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo05", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID05 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo06", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID06 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo07", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID07 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo08", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID08 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo09", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID09 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo10", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID10 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo11", ResourceType = typeof(Resources.Resource))]
        public string AccountNoID11 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo12", ResourceType = typeof(Resources.Resource))]
        public string RefundDebtAccountID { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo13", ResourceType = typeof(Resources.Resource))]
        public string RefundAccountNoID1 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo14", ResourceType = typeof(Resources.Resource))]
        public string RefundAccountNoID2 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo15", ResourceType = typeof(Resources.Resource))]
        public string RefundAccountNoID3 { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo16", ResourceType = typeof(Resources.Resource))]
        public string CancelCreditNoID { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "AccountNo17", ResourceType = typeof(Resources.Resource))]
        public string CancelDebtNoID { get; set; }

        public int SmsServerNameID { get; set; }
        public int SmsCredits { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string SmsUserName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string SmsUserPassword { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string SmsTitle { get; set; }


        [Column(TypeName = "char(1)")]
        public string Char01 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Char02 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Char03 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Char04 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Char05 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Char06 { get; set; }

        public Nullable<bool> IsChar01 { get; set; }
        public Nullable<bool> IsChar02 { get; set; }
        public Nullable<bool> IsChar03 { get; set; }
        public Nullable<bool> IsChar04 { get; set; }
        public Nullable<bool> IsChar05 { get; set; }
        public Nullable<bool> IsChar06 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Char01Explanation { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Char02Explanation { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Char03Explanation { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Char04Explanation { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Char05Explanation { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Char06Explanation { get; set; }

        public Nullable<int> Char01Max { get; set; }
        public Nullable<int> Char02Max { get; set; }
        public Nullable<int> Char03Max { get; set; }
        public Nullable<int> Char04Max { get; set; }
        public Nullable<int> Char05Max { get; set; }
        public Nullable<int> Char06Max { get; set; }

        public int SortType { get; set; }
        public Nullable<bool> SortOption { get; set; }
        public Nullable<bool> IsSelect { get; set; }
        public Nullable<int> SortOrder { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "NewPeriod2", ResourceType = typeof(Resources.Resource))]
        public string NewPeriod { get; set; }

        public bool IsFormPrint { get; set; }

    }
}
