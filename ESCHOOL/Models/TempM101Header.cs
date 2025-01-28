using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class TempM101Header
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public int UserID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CompanyName { get; set; }

        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string BankName { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Total { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Collection { get; set; }
        

        [Column(TypeName = "nvarchar(200)")]
        public string InWriting { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee01 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee02 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee03 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee04 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee05 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee06 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee07 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee08 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee09 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee10 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee11 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string HeaderFee12 { get; set; }
    }
}
