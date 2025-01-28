namespace ESCHOOL.ViewModels
{
    public class StudentUnplanViewModel
    {
        public int ViewModelId { get; set; }
        public int UserID { get; set; }
        public int StudentInvoiceID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int SchoolFeeID { get; set; }
        public string Period { get; set; }
        public string FeeName { get; set; }
        public byte Tax { get; set; }
        public decimal Amount { get; set; }
        public decimal CutInvoiceAmount { get; set; }
        public decimal UncutInvoiceAmount { get; set; }
        public decimal NewInvoiceAmount { get; set; }
    }
}
