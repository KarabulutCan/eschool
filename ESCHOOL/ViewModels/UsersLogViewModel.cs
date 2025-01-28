using ESCHOOL.Models;
using System;

namespace ESCHOOL.ViewModels
{
    public class UsersLogViewModel
    {
        public int UsersLogID { get; set; }
        public int SchoolID { get; set; }
        public string Period { get; set; }
        public string UserName { get; set; }
        public Nullable<DateTime> UserLogDate { get; set; }
        public string Transaction { get; set; }
        public string UserLogDescription { get; set; }


    }
}
