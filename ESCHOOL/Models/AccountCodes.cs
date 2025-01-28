using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class AccountCodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountCodeID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "AccountCodes", ResourceType = typeof(Resources.Resource))]
        public string AccountCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "AccountingCodeName", ResourceType = typeof(Resources.Resource))]
        public string AccountCodeName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "AccountingCodeName", ResourceType = typeof(Resources.Resource))]
        public string Language1 { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "AccountingCodeName", ResourceType = typeof(Resources.Resource))]
        public string Language2 { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "AccountingCodeName", ResourceType = typeof(Resources.Resource))]
        public string Language3 { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "AccountingCodeName", ResourceType = typeof(Resources.Resource))]
        public string Language4 { get; set; }
      
        public Nullable<bool> IsCurrentCard { get; set; }
        public Nullable<bool> IsActive { get; set; }

    }

}
