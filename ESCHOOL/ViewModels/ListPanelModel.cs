using ESCHOOL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.ViewModels
{
    public class ListPanelModel
    {
        public int UserID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public string SelectedCulture { get; set; }
        public int SelectedSchoolCode { get; set; }
        public bool IsPermission { get; set; }
        public int StartClassroom { get; set; }
        public int EndClassroom { get; set; }
        public string StartAccount { get; set; }
        public string EndAccount { get; set; }
        public Nullable<DateTime> StartListDate { get; set; }
        public Nullable<DateTime> EndListDate { get; set; }

        public bool List01Options0 { get; set; }
        public bool List01Options1 { get; set; }
        public bool List01Options2 { get; set; }
        public bool List01Options3 { get; set; }
        public bool List01Options4 { get; set; }
        public bool List01Options5 { get; set; }
        public bool List01Options6 { get; set; }
        public bool List01Options7 { get; set; }
        public bool List01Options8 { get; set; }
        public bool List01Options9 { get; set; }
        public bool List01Options10 { get; set; }

        public int ListOpt1 { get; set; }
        public int ListOpt2 { get; set; }
        public int FeeID { get; set; }
        public int FeeID2 { get; set; }
        public string Lsw { get; set; }
        public int Prg { get; set; }
        public int ExitID { get; set; }
        public int PaymentSW { get; set; }
        public string Title { get; set; }

        public int StartNumber { get; set; }
        public int EndNumber { get; set; }

        public bool AgreementOpt { get; set; }
        public bool KvkkOpt { get; set; }
        public string Periods { get; set; }

        public string FormTitle { get; set; }
        public bool FormOpt { get; set; }

        public string GuarantorID { get; set; }
        public string GuarantorName { get; set; }
        public string GuarantorPhone { get; set; }
        public string GuarantorAddress { get; set; }
        public int GuarantorCity { get; set; }
        public int GuarantorTown { get; set; }
        public string GuarantorZip { get; set; }
        public string GuarantorOther { get; set; }

        public bool BondType { get; set; }
        public bool BondSingle { get; set; }
        public bool WriteStudentName { get; set; }
        public int Interlocutor { get; set; }
        public bool WriteDate { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string wwwRootPath { get; set; }


        [Column(TypeName = "nvarchar(200)")]
        public string ConnectionString { get; set; }


        [Column(TypeName = "char(9)")]
        public string OldPeriod { get; set; }

        [Column(TypeName = "char(9)")]
        public string NewPeriod { get; set; }

        [Display(Name = "SchoolYearStart", ResourceType = typeof(Resources.Resource))]
        public Nullable<DateTime> SchoolYearStart { get; set; }

        [Display(Name = "SchoolYearEnd", ResourceType = typeof(Resources.Resource))]
        public Nullable<DateTime> SchoolYearEnd { get; set; }

        [Display(Name = "FinancialYearStart", ResourceType = typeof(Resources.Resource))]
        public Nullable<DateTime> FinancialYearStart { get; set; }

        [Display(Name = "FinancialYearEnd", ResourceType = typeof(Resources.Resource))]
        public Nullable<DateTime> FinancialYearEnd { get; set; }

        public string ReceiptNo { get; set; }

        public string CategoryName { get; set; }
        public string StudentName { get; set; }

    }
}
