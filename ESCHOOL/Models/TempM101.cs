using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class TempM101
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public int SchoolNumber { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> ClassroomID { get; set; }
        public Nullable<int> GenderTypeCategoryID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string StudentName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ParentName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string ParentMobilePhone { get; set; }

        public Nullable<int> ReceiptNo { get; set; }
        public Nullable<int> AccountReceipt { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string TypeAndNo { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfRegistration { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string StrDate { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string StudentNumber { get; set; }

        public int StudentSerialNumber { get; set; }
        public bool IsPension { get; set; }


        [Column(TypeName = "nvarchar(15)")]
        public string IdNumber { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string TypeCategoryName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string ParentIdNumber { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string ClassroomName { get; set; }

        //Bond
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string BondCity { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string BondTypeTitle { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> BondDate { get; set; }
        public bool WriteDate { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string InWriting { get; set; }
        public bool WriteStudentName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ParentAddress { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string ParentTown { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string ParentCity { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string ParentZipCode { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string HomePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string WorkPhone { get; set; }



        [Column(TypeName = "nvarchar(15)")]
        public string GuarantorId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string GuarantorName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string GuarantorPhone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string GuarantorAddress { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string GuarantorCity { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string GuarantorTown { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string GuarantorZipCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string GuarantorOther { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> TotalFee { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> CashPayment { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> RefundAmount { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> CancelAmount { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Fee01 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee02 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee03 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee04 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee05 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee06 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee07 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee08 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee09 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee10 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee11 { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee12 { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Fee01Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee02Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee03Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee04Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee05Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee06Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee07Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee08Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee09Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee10Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee11Collection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee12Collection { get; set; }

        [Column(TypeName = "money")]
        public Nullable<decimal> Fee01Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee02Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee03Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee04Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee05Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee06Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee07Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee08Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee09Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee10Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee11Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> Fee12Balance { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> PreviousAmount { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> NextAmount { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> PreviousCollection { get; set; }
        [Column(TypeName = "money")]
        public Nullable<decimal> NextCollection { get; set; }

    }
}
