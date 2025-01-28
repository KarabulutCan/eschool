using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentDebt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentDebtID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int ClassroomTypeID { get; set; }
        public int SchoolFeeID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "List", ResourceType = typeof(Resources.Resource))]
        public bool IsList { get; set; }
    }
}

