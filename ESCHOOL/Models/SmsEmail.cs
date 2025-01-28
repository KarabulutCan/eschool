using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class SmsEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SmsEmailID { get; set; }
        public int StudentID { get; set; }
        public int SchoolID { get; set; }

        [Display(Name = "DateOfBird", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfBird { get; set; }

        public bool IsMobile { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MobilePhoneOrEMail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Title", ResourceType = typeof(Resources.Resource))]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Message", ResourceType = typeof(Resources.Resource))]
        public string Message { get; set; }

        public bool IsStatu { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string TransactionMessage { get; set; }
    }
}

