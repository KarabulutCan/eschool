using ESCHOOL.Models;
using System;

namespace ESCHOOL.ViewModels
{
    public class SchoolSelectingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsSelect { get; set; }
    }
}
