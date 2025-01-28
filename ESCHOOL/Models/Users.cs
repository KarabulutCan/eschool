using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resource))]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Resource))]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "UserPassword", ResourceType = typeof(Resources.Resource))]
        public string UserPassword { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> UserDate { get; set; }

        public bool IsLogin { get; set; }

        [Column(TypeName = "datetime")]
        public Nullable<DateTime> LoginDate { get; set; }
        public int SelectedSchoolCode { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string SelectedCulture { get; set; }

        [Display(Name = "SelectedLanguage", ResourceType = typeof(Resources.Resource))]
        public int SelectedLanguage { get; set; }

        [Display(Name = "UserShiftFrom", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Time)]
        public Nullable<TimeSpan> UserShiftFrom { get; set; }

        [Display(Name = "UserShiftTo", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Time)]
        public Nullable<TimeSpan> UserShiftTo { get; set; }

        [MaxLength(100, ErrorMessage = "Name cannot be greater than 100")]
        [Column(TypeName = "nvarchar(100)")]
        public string UserPicture { get; set; }

        [MaxLength(100, ErrorMessage = "Name cannot be greater than 100")]
        [Column(TypeName = "nvarchar(100)")]
        public string UserViewPicture { get; set; }

        [Display(Name = "GenderType", ResourceType = typeof(Resources.Resource))]
        public int GenderTypeCategoryID { get; set; }

        [Display(Name = "DateOfBird", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfBird { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.Resource))]
        public string IdNumber { get; set; }

        [Display(Name = "BloodGroup", ResourceType = typeof(Resources.Resource))]
        public int BloodGroupCategoryID { get; set; }

        [Display(Name = "Nationality", ResourceType = typeof(Resources.Resource))]
        public int NationalityID { get; set; }

        [Display(Name = "Religious", ResourceType = typeof(Resources.Resource))]
        public int ReligiousID { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "HomePhone", ResourceType = typeof(Resources.Resource))]
        public string HomePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string MobilePhone { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string HomeAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> HomeCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> HomeTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string HomeZipCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string EMail { get; set; }

        [Display(Name = "UserDateOfJoining", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> UserDateOfJoining { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "Profession", ResourceType = typeof(Resources.Resource))]
        public string Profession { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        [Display(Name = "UserComments", ResourceType = typeof(Resources.Resource))]
        public string UserComments { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string SelectedTheme { get; set; }

        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        public string UserPeriod { get; set; }

        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsSupervisor { get; set; }
        public Nullable<bool> IsActive { get; set; }

    }
}
