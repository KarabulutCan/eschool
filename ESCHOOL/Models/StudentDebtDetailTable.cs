using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentDebtDetailTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentDebtTableID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int SchoolFeeID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal AmountTable { get; set; }

        [Display(Name = "InstallmentTable", ResourceType = typeof(Resources.Resource))]
        public int InstallmentTable { get; set; }

        [Display(Name = "PaymentStartDateTable", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public DateTime PaymentStartDateTable { get; set; }
    }
}

