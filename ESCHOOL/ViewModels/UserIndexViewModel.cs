using System;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class UserIndexViewModel
    {
        public int ViewModelID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public string UserPicture { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> DateOfBird { get; set; }
        
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> LoginDate { get; set; }
        public string IsLoginToday { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsSupervisor { get; set; }
        
        public string userSendMessage { get; set; }
        public string UserMessageCounter { get; set; }
    }
}
