using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentPeriods
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentPeriodID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string ClassroomName{ get; set; }

    }
}

