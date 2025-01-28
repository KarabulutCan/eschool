using ESCHOOL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class SchoolViewModel
    {
        public int UserID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }

        [StringLength(30)]
        public string SchoolName { get; set; }
        public string SchoolShortName { get; set; }
        public string LogoName { get; set; }
        
        public string UserPeriod { get; set; }
        public string NewPeriod { get; set; }
        public string SelectedTheme { get; set; }
        public Nullable<DateTime> UserDate { get; set; }
        public string UserPicture { get; set; }
        public string UserPicture1 { get; set; }
        public string UserPicture2 { get; set; }
        public string UserViewPicture { get; set; }

        public SchoolInfo SchoolInfo { get; set; }
        public PSerialNumber PSerialNumber { get; set; }
        public bool IsPermission { get; set; }
        public bool IsProtected { get; set; }
        public string ConnectionString { get; set; }
        public int SelectedSchoolCode { get; set; }
        public int RegistrationTypeSubID { get; set; }
        public int StatuCategorySubID { get; set; }

        public int StatuCategoryID { get; set; }

        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string wwwRootPath { get; set; }
        public string SelectedCulture { get; set; }

        public string CategoryName { get; set; }
        public string CategoryNameSub { get; set; }
        public string UserName { get; set; }

        public int ChatID { get; set; }
        public int ChatUserID1 { get; set; }
        public int ChatUserID2 { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> ChatDate { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> ChatDate1 { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> ChatDate2 { get; set; }

        public string Messages { get; set; }
        public string Messages1 { get; set; }
        public string Messages2 { get; set; }
        public string MessageCount { get; set; }
        public string Icon { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsRead { get; set; }
    }
}
