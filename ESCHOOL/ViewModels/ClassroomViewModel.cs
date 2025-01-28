using ESCHOOL.Models;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class ClassroomViewModel
    {
        public int ViewModelID { get; set; }
        public int UserID { get; set; }
        public string SchoolName { get; set; }
        public string UserPeriod { get; set; }

        public SchoolInfo SchoolInfo { get; set; }

        public int ClassroomID { get; set; }
        public int SchoolID { get; set; }
        public string Period { get; set; }
        public string ClassroomName { get; set; }
        public int ClassroomTypeID { get; set; }
        public string ClassroomTeacher { get; set; }
        public int RoomQuota { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        public int CategoryID { get; set; }
        public int CategorySubID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryLevel { get; set; }
        public int SortOrder2 { get; set; }
        public bool IsActive2 { get; set; }
        public bool IsProtected { get; set; }
        public string NationalityCode { get; set; }
        public bool IsSelect { get; set; }
    }
}
