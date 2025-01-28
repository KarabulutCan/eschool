using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class UsersLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLogID { get; set; }
        public int SchoolID { get; set; }
        public int UserID { get; set; }
        
        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }
        public int TransactionID { get; set; }

        [Display(Name = "UserLogDate", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "datetime")]
        public Nullable<DateTime> UserLogDate { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [Display(Name = "UserLogDescription", ResourceType = typeof(Resources.Resource))]
        public string UserLogDescription { get; set; }

   



    }

}
