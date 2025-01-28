using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class SchoolFeeTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolFeeTableID { get; set; }
        public int SchoolID { get; set; }
        public int FeeCategory { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        public int CategoryID { get; set; }
        public int SchoolFeeID { get; set; }
        public int SchoolFeeSubID { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public Nullable<decimal> SchoolFeeTypeAmount { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string StockCode { get; set; }

        [Display(Name = "StockQuantity", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> StockQuantity { get; set; }


        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> SortOrder { get; set; }

        [Display(Name = "IsActive", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> IsActive { get; set; }
    }
}
