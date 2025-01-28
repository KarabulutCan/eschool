using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentFamilyAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentFamilyAddressID { get; set; }
        public int StudentID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string FatherName { get; set; }

        [Display(Name = "Profession", ResourceType = typeof(Resources.Resource))]
        public int FatherProfessionCategoryID { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.Resource))]
        public string FatherIdNumber { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "HomePhone", ResourceType = typeof(Resources.Resource))]
        public string FatherHomePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "WorkPhone", ResourceType = typeof(Resources.Resource))]
        public string FatherWorkPhone { get; set; }

        public bool FatherIsSMS { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string FatherMobilePhone { get; set; }

        public bool FatherIsEmail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string FatherEMail { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string FatherHomeAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int FatherHomeCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int FatherHomeTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string FatherHomeZipCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string FatherWorkAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int FatherWorkCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int FatherWorkTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string FatherWorkZipCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string MotherName { get; set; }

        [Display(Name = "Profession", ResourceType = typeof(Resources.Resource))]
        public int MotherProfessionCategoryID { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.Resource))]
        public string MotherIdNumber { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "HomePhone", ResourceType = typeof(Resources.Resource))]
        public string MotherHomePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "WorkPhone", ResourceType = typeof(Resources.Resource))]
        public string MotherWorkPhone { get; set; }

        public bool MotherIsSMS { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string MotherMobilePhone { get; set; }

        public bool MotherIsEmail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string MotherEMail { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string MotherHomeAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int MotherHomeCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int MotherHomeTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string MotherHomeZipCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string MotherWorkAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int MotherWorkCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int MotherWorkTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string MotherWorkZipCode { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        public bool isEmpty
        {
            get
            {
                return (
                        string.IsNullOrWhiteSpace(FatherName) &&
                        string.IsNullOrWhiteSpace(FatherIdNumber) &&
                        string.IsNullOrWhiteSpace(FatherHomeAddress) &&
                        string.IsNullOrWhiteSpace(FatherHomePhone) &&
                        string.IsNullOrWhiteSpace(FatherWorkPhone) &&
                        string.IsNullOrWhiteSpace(FatherMobilePhone) &&
                        string.IsNullOrWhiteSpace(FatherEMail) &&
                        string.IsNullOrWhiteSpace(FatherHomeZipCode) &&
                        string.IsNullOrWhiteSpace(FatherWorkZipCode) &&

                        string.IsNullOrWhiteSpace(MotherName) &&
                        string.IsNullOrWhiteSpace(MotherIdNumber) &&
                        string.IsNullOrWhiteSpace(MotherHomeAddress) &&
                        string.IsNullOrWhiteSpace(MotherHomePhone) &&
                        string.IsNullOrWhiteSpace(MotherWorkPhone) &&
                        string.IsNullOrWhiteSpace(MotherMobilePhone) &&
                        string.IsNullOrWhiteSpace(MotherEMail) &&
                        string.IsNullOrWhiteSpace(MotherHomeZipCode) &&
                        string.IsNullOrWhiteSpace(MotherWorkZipCode) &&

                        FatherProfessionCategoryID == 0 &&
                        FatherHomeCityParameterID == 0 &&
                        FatherHomeTownParameterID == 0 &&
                        FatherWorkCityParameterID == 0 &&
                        FatherWorkTownParameterID == 0 &&

                        MotherProfessionCategoryID == 0 &&
                        MotherHomeCityParameterID == 0 &&
                        MotherHomeTownParameterID == 0 &&
                        MotherWorkCityParameterID == 0 &&
                        MotherWorkTownParameterID == 0);
            }
        }
    }
}
