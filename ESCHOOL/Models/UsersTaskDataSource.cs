using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class UsersTaskDataSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        public int SchoolID { get; set; }
        public int UserID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Title", ResourceType = typeof(Resources.Resource))]
        public string Title { get; set; }

        [Display(Name = "Start", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy} hh:mm")]
        [Column(TypeName = "datetime")]
        public DateTime Start { get; set; }

        [Display(Name = "End", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy} hh:mm")]
        [Column(TypeName = "datetime")]
        public DateTime End { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Description", ResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string RecurrenceId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string RecurrenceRule { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string RecurrenceException { get; set; }
        public Nullable<bool> IsAllDay { get; set; }
        public Nullable<int> OwnerID { get; set; }
    }
}
