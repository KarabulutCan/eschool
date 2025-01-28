using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentNoteID { get; set; }
        public int StudentID { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        [Display(Name = "Note1", ResourceType = typeof(Resources.Resource))]
        public string Note1 { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        [Display(Name = "Note2", ResourceType = typeof(Resources.Resource))]
        public string Note2 { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        [Display(Name = "Note3", ResourceType = typeof(Resources.Resource))]
        public string Note3 { get; set; }
        public bool isEmpty
        {
            get
            {
                return (
                        string.IsNullOrWhiteSpace(Note1) &&
                        string.IsNullOrWhiteSpace(Note2) &&
                        string.IsNullOrWhiteSpace(Note3));
            }
        }
    }
}
