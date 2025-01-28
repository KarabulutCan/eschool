using System;

namespace ESCHOOL.ViewModels
{
    public class SmsEmailViewModel
    {
        public int SmsEmailID { get; set; }
        public string Period { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public string StudentNumber { get; set; }
        public string Name { get; set; }
        public string StudentClassroom { get; set; }

        public Nullable<DateTime> DateOfBird { get; set; }
        public string StudentMobilePhone { get; set; }
        public string StudentEMail { get; set; }

        public string ParentMobilePhone { get; set; }
        public string ParentEMail { get; set; }

        public string FatherMobilePhone { get; set; }
        public string FatherEMail { get; set; }

        public string MotherMobilePhone { get; set; }
        public string MotherEMail { get; set; }

        public string UnpaidDebtMessage { get; set; }
        public string TotalDebtMessage { get; set; }
        public bool IsStatu { get; set; }

        public string Title { get; set; }
        public string MobilePhoneOrEMail { get; set; }
        public string Message { get; set; }
        public string TransactionMessage { get; set; }
    }
}
