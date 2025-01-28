using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class InvoiceConfirmViewModel
    {
        public string Period { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public int ClassroomID { get; set; }
        public string SelectedCulture { get; set; }
        public Nullable<DateTime> InvoiceBatchDate { get; set; }
        public int MaxPrint { get; set; }
        public int TotalInvoice { get; set; }
        public int RemainingInvoice { get; set; }
        public string StudentName { get; set; }
        public string ParentName { get; set; }
        public string EIIsActive { get; set; }

    }
}
