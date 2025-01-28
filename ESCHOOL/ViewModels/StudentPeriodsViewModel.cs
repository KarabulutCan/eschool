namespace ESCHOOL.ViewModels
{
    public class StudentPeriodsViewModel
    {
        public int StudentPeriodID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public string Period { get; set; }
        public string ClassroomName { get; set; }
        public decimal Debt { get; set; }
        public decimal Payment { get; set; }
        public decimal Balance { get; set; }
    }
}
