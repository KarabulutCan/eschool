using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Classroom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassroomID { get; set; }

        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }


        [Column(TypeName = "nvarchar(15)")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(15, ErrorMessage = "Name cannot be longer than 15 characters.")]

        [Display(Name = "ClassroomName", ResourceType = typeof(Resources.Resource))]
        public string ClassroomName { get; set; }

        [Display(Name = "ClassroomType", ResourceType = typeof(Resources.Resource))]
        [UIHint("ClassRoomTypeDropDown")]
        public int ClassroomTypeID { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "ClassroomTeacher", ResourceType = typeof(Resources.Resource))]
        public string ClassroomTeacher { get; set; }

        [Display(Name = "RoomQuota", ResourceType = typeof(Resources.Resource))]
        public int RoomQuota { get; set; }

        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public int SortOrder { get; set; }

        public Nullable<bool> IsActive { get; set; }

    }

}
