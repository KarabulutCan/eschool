using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IUsersRepository
    {
        IEnumerable<Users> GetUsersAllLogin();
        IEnumerable<Users> GetUsersAll();
        IEnumerable<Users> GetUsersAllSV();
        Users GetUser(int usersID);
        Users GetUserSchool(int schoolCode);
        Users GetUserLogin(int schoolID, string userName, string password);
        Users GetUser(string userName, string password);
        bool CreateUsers(Users users);
        bool UpdateUsers(Users users);
        bool DeleteUsers(Users users);
        bool ExistUsers(int usersID);
        bool ExistUsernameAndPassword(string userName, string password);
        bool Save();
    }
}
