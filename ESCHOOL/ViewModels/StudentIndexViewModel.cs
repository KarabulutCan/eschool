using System;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class StudentIndexViewModel
    {
        public int ViewModelID { get; set; }
        public int UserID { get; set; }
        public string Period { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int StudentID { get; set; }
        public string StudentPicture { get; set; }
        public string Name { get; set; }
        public string StudentClassroom { get; set; }
        public string StatuCategory { get; set; }
        public string RegistrationTypeCategory { get; set; }
        public string StudentNumber { get; set; }
        public string IdNumber { get; set; }
        public decimal Balance { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> DateOfRegistration { get; set; }
        public string ParentName { get; set; }
        public string StudentMobilePhone { get; set; }
        public string ParentMobilePhone { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int SiblingID { get; set; }
        public bool IsPermission { get; set; }
        public bool IsPermissionCancel { get; set; }
        public string SelectedCulture { get; set; }
        public Nullable<bool> IsWebRegistration { get; set; }
    }
}
