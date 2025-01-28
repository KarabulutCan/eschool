using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class UsersWorkAreas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsersWorkAreaID { get; set; }

        public int UserID { get; set; }

        public int CategoryID { get; set; }

        public bool IsSchool { get; set; }
        public bool IsSelect { get; set; }
        public Nullable<bool> IsDirtySelect { get; set; }
    }

}
