using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IUsersLogRepository
    {
        IEnumerable<UsersLog> GetUsersLogAll(string period);
        UsersLog GetUsersLog(int usersLogID);

        bool CreateUsersLog(UsersLog usersLog);
        bool UpdateUsersLog(UsersLog usersLog);
        bool DeleteUsersLog(UsersLog usersLog);
        bool ExistUsersLog(int usersLogID);

        bool Save();
    }
}
