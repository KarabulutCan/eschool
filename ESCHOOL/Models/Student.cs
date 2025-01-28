using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        [Display(Name = "Schools", ResourceType = typeof(Resources.Resource))]
        public int SchoolID { get; set; }

        [Display(Name = "StudentClassroom", ResourceType = typeof(Resources.Resource))]
        public int ClassroomID { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Resource))]
        public int StatuCategoryID { get; set; }

        [Display(Name = "RegistrationType", ResourceType = typeof(Resources.Resource))]
        public int RegistrationTypeCategoryID { get; set; }

        [Display(Name = "GenderType", ResourceType = typeof(Resources.Resource))]
        public int GenderTypeCategoryID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string StudentPicture { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resource))]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Resource))]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "StudentNumber", ResourceType = typeof(Resources.Resource))]
        public string StudentNumber { get; set; }

        [Display(Name = "StudentSerialNumber", ResourceType = typeof(Resources.Resource))]
        public int StudentSerialNumber { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.Resource))]
        public string IdNumber { get; set; }

        [Display(Name = "BloodGroup", ResourceType = typeof(Resources.Resource))]
        public int BloodGroupCategoryID { get; set; }

        [Display(Name = "Nationality", ResourceType = typeof(Resources.Resource))]
        public int NationalityID { get; set; }

        [Display(Name = "Religious", ResourceType = typeof(Resources.Resource))]
        public int ReligiousID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "ParentName", ResourceType = typeof(Resources.Resource))]
        public string ParentName { get; set; }

        [Display(Name = "DateOfBird", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfBird { get; set; }

        [Display(Name = "DateOfRegistration", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfRegistration { get; set; }

        [Display(Name = "FirstDateOfRegistration", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> FirstDateOfRegistration { get; set; }

        [Display(Name = "SchoolBusDeparture", ResourceType = typeof(Resources.Resource))]
        public int SchoolBusDepartureID { get; set; }

        [Display(Name = "SchoolBusReturn", ResourceType = typeof(Resources.Resource))]
        public int SchoolBusReturnID { get; set; }

        [Display(Name = "DriverName", ResourceType = typeof(Resources.Resource))]
        public int SchoolBusDepartureDriverID { get; set; }

        [Display(Name = "DriverName", ResourceType = typeof(Resources.Resource))]
        public int SchoolBusReturnDriverID { get; set; }

        [Display(Name = "SchoolBusStatus", ResourceType = typeof(Resources.Resource))]
        public int SchoolBusStatuID { get; set; }

        [Display(Name = "PreviousSchool", ResourceType = typeof(Resources.Resource))]
        public int PreviousSchoolID { get; set; }

        [Display(Name = "PreviousBranch", ResourceType = typeof(Resources.Resource))]
        public int PreviousBranchID { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> ScholarshipStartDate { get; set; }

        [Display(Name = "EndDate", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> ScholarshipEndDate { get; set; }

        [Display(Name = "ScholarshipRate", ResourceType = typeof(Resources.Resource))]
        public Nullable<short> ScholarshipRate { get; set; }

        [Display(Name = "ExplanationShow", ResourceType = typeof(Resources.Resource))]
        public bool IsExplanationShow { get; set; }

        public bool IsActive { get; set; }

        public int SiblingID { get; set; }

        public bool IsPension { get; set; }

        public bool IsWebRegistration { get; set; }
    }
}

