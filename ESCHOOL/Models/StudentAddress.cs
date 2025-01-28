using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentAddressID { get; set; }
        public int StudentID { get; set; }

        [Display(Name = "IsSms", ResourceType = typeof(Resources.Resource))]
        public bool IsSms { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string MobilePhone { get; set; }

        [Display(Name = "IsEMail", ResourceType = typeof(Resources.Resource))]
        public bool IsEMail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string EMail { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address1 { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int CityParameterID1 { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int TownParameterID1 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string ZipCode1 { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address2 { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int CityParameterID2 { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int TownParameterID2 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string ZipCode2 { get; set; }

        public bool isEmpty
        {
            get
            {
                return (
                        string.IsNullOrWhiteSpace(MobilePhone) &&
                        string.IsNullOrWhiteSpace(EMail) &&
                        string.IsNullOrWhiteSpace(Address1) &&
                        string.IsNullOrWhiteSpace(ZipCode1) &&

                        string.IsNullOrWhiteSpace(Address2) &&
                        string.IsNullOrWhiteSpace(ZipCode2) &&

                        CityParameterID1 == 0 &&
                        TownParameterID1 == 0 &&
                        CityParameterID2 == 0 &&
                        TownParameterID2 == 0);
            }
        }
    }
}
