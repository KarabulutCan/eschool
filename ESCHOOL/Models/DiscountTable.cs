using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class DiscountTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountTableID { get; set; }
        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string DiscountName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Language1 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Language2 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Language3 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Language4 { get; set; }

        [Display(Name = "Percent", ResourceType = typeof(Resources.Resource))]
        public byte DiscountPercent { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "money")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> IsPasswordRequired { get; set; }

        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> SortOrder { get; set; }

        [Display(Name = "IsActive", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsSelect { get; set; }
        public Nullable<bool> IsDirtySelect { get; set; }

    }
}
