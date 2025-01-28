using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentDiscountID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }
        public int StudentDebtID { get; set; }
        public int DiscountTableID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal DiscountTotal { get; set; }

        [Display(Name = "Applied", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal DiscountApplied { get; set; }
        public byte DiscountPercent { get; set; }
    }
}

