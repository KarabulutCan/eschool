using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class MultipurposeList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MultipurposeListID { get; set; }
        public int UserID { get; set; }
        public int Lenght { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Language1 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Language2 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Language3 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Language4 { get; set; }

        public Nullable<bool> Condition { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Conditions1 { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Conditions2 { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Type { get; set; }
        public Nullable<bool> IsSelect { get; set; }
    }
}
