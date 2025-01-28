using ESCHOOL.Models;

namespace ESCHOOL.ViewModels
{
    public class UserViewModel
    {
        public int ViewModelID { get; set; }
        public int UserID { get; set; }
        public int SchoolID { get; set; }
        public string UserPasswordTmp { get; set; }
        public int SelectedLanguage { get; set; }
        public string SelectedCulture { get; set; }
        public Users Users { get; set; }
        public string CategoryName { get; set; }

    }
}
