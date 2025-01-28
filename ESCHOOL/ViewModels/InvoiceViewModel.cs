using ESCHOOL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Unity.Policy;

namespace ESCHOOL.ViewModels
{
    public class InvoiceViewModel
    {
        public int ViewModelID { get; set; }
        public int UserID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }
        public string AccountCode { get; set; }
        public int AccountingID { get; set; }
        
        public int SelectedSchoolCode { get; set; }
        public string Period { get; set; }
        public int InvoiceSerialNo { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassroomName { get; set; }

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
        public int InvoiceSerialNo2 { get; set; }
        public int InvoiceSerialNo22 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "InvoiceName3", ResourceType = typeof(Resources.Resource))]
        public string InvoiceName3 { get; set; }
        public int InvoiceSerialNo3 { get; set; }
        public int InvoiceSerialNo33 { get; set; }

        public int StudentInvoiceID { get; set; }
        public int StudentInvoiceDetailID { get; set; }
        public string Explanation { get; set; }
        public Nullable<byte> Quantity { get; set; }
        public Nullable<byte> TaxPercent { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Amount { get; set; }

        public int WithholdingPercent1 { get; set; }
        public int WithholdingPercent2 { get; set; }
        public string WithholdingCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string WithholdingExplanation { get; set; }
        public Nullable<decimal> WithholdingTax { get; set; }
        public Nullable<decimal> WithholdingTotal { get; set; }


        public Nullable<bool> InvoiceProfile { get; set; }
        public Nullable<int> InvoiceTypeParameter { get; set; }
        public string ParameterExceptionCode { get; set; }

        public string DirtyField { get; set; }
        public StudentInvoiceAddress StudentInvoiceAddress { get; set; }
        public bool IsPermission { get; set; }
        public string SelectedCulture { get; set; }

        public bool IsSuccess { get; set; }
        public bool IsSuccess2 { get; set; }

        public int SchoolCode { get; set; }
        public Nullable<bool> SchoolEInvoiceIsActive { get; set; }

        public string ConnectionString { get; set; }


    }
}
