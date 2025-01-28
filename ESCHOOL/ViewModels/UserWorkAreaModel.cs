using ESCHOOL.Models;
using System;

namespace ESCHOOL.ViewModels
{
    public class UsersWorkAreaModel
    {
        public int ViewModelID { get; set; }
        public int SchoolID { get; set; }
        public int UsersWorkAreaID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsSchool { get; set; }
        public bool IsSelect { get; set; }
        public Nullable<bool> IsDirtySelect { get; set; }
        public Parameter Parameter { get; set; }
    }
}
