using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class UsersChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatID { get; set; }
        public int SchoolCode { get; set; }
        public int UserID { get; set; }
        public int ChatUserID1 { get; set; }
        public int ChatUserID2 { get; set; }

        [Display(Name = "ChatDate", ResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "datetime")]
        public Nullable<DateTime> ChatDate { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Messages", ResourceType = typeof(Resources.Resource))]
        public string Messages { get; set; }

        public bool IsArchive { get; set; }

    }

}
