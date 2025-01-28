using ESCHOOL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.ViewModels
{
    public class InvoiceExcelViewModel
    {
        public int StudentInvoiceID { get; set; }
        public int UserID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }
        public string Period { get; set; }

        public string DocumentType { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public Nullable<TimeSpan> InvoiceTime { get; set; }
        public string InvoiceSerialNo { get; set; }
        public int InvoiceSerialSequence { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string AccountRate { get; set; }
        public string CurrencyCode { get; set; }
        public string Rate { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string Unit { get; set; }
        public string GroupCode { get; set; }
        public string SpecialCode { get; set; }
        
        public Nullable<byte> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> DiscountPercent { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Amount1 { get; set; }

        public Nullable<byte> TaxPercent { get; set; }
        public Nullable<decimal> Tax { get; set; }

        public Nullable<byte> AmountPercent2 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }

        public Nullable<decimal> Total { get; set; }
        public string Explanation { get; set; }

        public string Agreement { get; set; }

        public Nullable<decimal> Amount3 { get; set; }
        public Nullable<decimal> Amount4 { get; set; }
        public Nullable<decimal> Amount5 { get; set; }
        public Nullable<decimal> Amount6 { get; set; }

        public string InvoiceTaxNumber { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceIdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string InvoiceCity { get; set; }
        public string InvoiceTown { get; set; }
        public string InvoiceNeighbourhood { get; set; }

        public string CSBMType { get; set; }
        public string CSBM { get; set; }
        public string ExteriorDoorNo { get; set; }
        public string InnerDoorNo { get; set; }
        public string EArchiveType { get; set; }
        public string WaybillDate { get; set; }
        public string WaybillSerial { get; set; }
        public string WaybillSequence { get; set; }


        public string SelectedCulture { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
}
