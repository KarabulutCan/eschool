using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankID { get; set; }

        public int SchoolID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public string BankName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "BankBranchCode", ResourceType = typeof(Resources.Resource))]
        public string BankBranchCode { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "BankGivenCode", ResourceType = typeof(Resources.Resource))]
        public string BankGivenCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "AccountDecimation", ResourceType = typeof(Resources.Resource))]
        public string AccountDecimation { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "AccountNo", ResourceType = typeof(Resources.Resource))]
        public string AccountNo { get; set; }

        [Column(TypeName = "nvarchar(26)")]
        [Display(Name = "Iban", ResourceType = typeof(Resources.Resource))]
        public string Iban { get; set; }

        [Display(Name = "PeriodOfTime", ResourceType = typeof(Resources.Resource))]
        public int PeriodOfTime { get; set; }

        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public int SortOrder { get; set; }

        public Nullable<bool> IsActive { get; set; }

    }

}
