using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ESCHOOL.Services
{
    public class UsersRepository : IUsersRepository
    {
        private SchoolDbContext _usersContext;
        public UsersRepository(SchoolDbContext usersContext)
        {
            _usersContext = usersContext;
        }
        IEnumerable<Users> IUsersRepository.GetUsersAllLogin()
        {
            return _usersContext.Users.OrderBy(c => c.SortOrder).ToList();
        }
        IEnumerable<Users> IUsersRepository.GetUsersAll()
        {
            return _usersContext.Users.Where(a => a.IsSupervisor == false).OrderBy(c => c.SortOrder).ToList();
        }
        IEnumerable<Users> IUsersRepository.GetUsersAllSV()
        {
            return _usersContext.Users.OrderBy(c => c.SortOrder).ToList();
        }
        public Users GetUser(int userID)
        {
            return _usersContext.Users.Where(b => b.UserID == userID).FirstOrDefault();
        }

        public Users GetUserSchool(int schoolCode)
        {
            return _usersContext.Users.Where(b => b.SelectedSchoolCode == schoolCode).FirstOrDefault();
        }

        public Users GetUserLogin(int schoolID, string userName, string password)
        {
            return _usersContext.Users.Where(b => b.SchoolID == schoolID && b.UserName == userName && b.UserPassword == password && b.IsActive == true).FirstOrDefault();
        }

        public Users GetUser(string userName, string password)
        {
            return _usersContext.Users.Where(b => b.UserName == userName && b.UserPassword == password && b.IsActive == true).FirstOrDefault();
        }
        public bool CreateUsers(Users users)
        {
            _usersContext.Add(users);
            return Save();
        }

        public bool UpdateUsers(Users users)
        {
            _usersContext.Update(users);
            return Save();
        }

        public bool DeleteUsers(Users users)
        {
            _usersContext.Remove(users);
            return Save();
        }

        public bool ExistUsers(int userID)
        {
            return _usersContext.Users.Any(c => c.UserID == userID);
        }

        public bool ExistUsernameAndPassword(string userName, string password)
        {
            return _usersContext.Users.Any(c => c.UserName == userName && c.UserPassword == password);
        }
        public bool Save()
        {
            var saved = _usersContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
