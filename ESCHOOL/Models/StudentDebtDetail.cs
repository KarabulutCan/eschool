using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentDebtDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentDebtDetailID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int CategoryID { get; set; }
        public int SchoolFeeID { get; set; }
        public int StudentDebtID { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> StockQuantity { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string StockCode { get; set; }

    }
}

