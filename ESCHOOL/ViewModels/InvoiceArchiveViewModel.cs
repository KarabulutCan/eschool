using System;

namespace ESCHOOL.ViewModels
{
    public class InvoiceArchiveViewModel
    {
        public int StudentInvoiceID { get; set; }
        public int SchoolID { get; set; }
        public string Period { get; set; }
        public int StudentID { get; set; }
        public int StudentInvoiceAddressID { get; set; }
        public string InvoiceTitle { get; set; }
        public int InvoiceSerialNo { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public Nullable<DateTime> InvoiceCutDate { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public bool IsPlanned { get; set; }

    }
}
