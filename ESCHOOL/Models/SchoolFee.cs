using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class SchoolFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolFeeID { get; set; }
        public Nullable<int> SchoolFeeSubID { get; set; }
        public int FeeCategory { get; set; }
        public int SchoolID { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FeeName", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FeeName", ResourceType = typeof(Resources.Resource))]
        public string Language1 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FeeName", ResourceType = typeof(Resources.Resource))]
        public string Language2 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FeeName", ResourceType = typeof(Resources.Resource))]
        public string Language3 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "FeeName", ResourceType = typeof(Resources.Resource))]
        public string Language4 { get; set; }


        [Column(TypeName = "char(2)")]
        public string CategoryLevel { get; set; }

        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public Nullable<byte> Tax { get; set; }

        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> SortOrder { get; set; }

        [Display(Name = "IsActive", ResourceType = typeof(Resources.Resource))]
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsSelect { get; set; }
    }
}
