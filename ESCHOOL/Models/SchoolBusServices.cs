using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class SchoolBusServices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolBusServicesID { get; set; }
        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "PlateNo", ResourceType = typeof(Resources.Resource))]
        public string PlateNo { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "DriverName", ResourceType = typeof(Resources.Resource))]
        public string DriverName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "BusPhone", ResourceType = typeof(Resources.Resource))]
        public string BusPhone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "BusRoute", ResourceType = typeof(Resources.Resource))]
        public string BusRoute { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "BusTeacher", ResourceType = typeof(Resources.Resource))]
        public string BusTeacher { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "BusTeacherPhone", ResourceType = typeof(Resources.Resource))]
        public string BusTeacherPhone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Explanation", ResourceType = typeof(Resources.Resource))]
        public string Explanation { get; set; }

        public Nullable<int> SortOrder { get; set; }

        public Nullable<bool> IsActive { get; set; }

    }

}
