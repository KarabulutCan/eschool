using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Accounting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountingID { get; set; }

        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        public Nullable<int> VoucherTypeID { get; set; }
        public int VoucherNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> AccountDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string AccountCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "AccountCodeName", ResourceType = typeof(Resources.Resource))]
        public string AccountCodeName { get; set; }


        [Column(TypeName = "nvarchar(20)")]
        public string CodeTypeName { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "DocumentNumber", ResourceType = typeof(Resources.Resource))]
        public string DocumentNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DocumentDate { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Explanation", ResourceType = typeof(Resources.Resource))]
        public string Explanation { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Debt { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Credit { get; set; }

        public Nullable<int> SortOrder { get; set; }

        public Nullable<bool> IsTransaction { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string ProcessTypeName { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "TaxNoOrId", ResourceType = typeof(Resources.Resource))]
        public string TaxNoOrId { get; set; }

    }

}
