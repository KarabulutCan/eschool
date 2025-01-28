using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IUsersWorkAreasRepository
    {
        IEnumerable<UsersWorkAreas> GetUsersWorkAreasAll();
        IEnumerable<UsersWorkAreas> GetUserWorkAreasID(int userID);
        UsersWorkAreas GetUsersWorkAreas(int userID, int categoryID);
        bool CreateUsersWorkAreas(UsersWorkAreas usersWorkAreas);
        bool UpdateUsersWorkAreas(UsersWorkAreas usersWorkAreas);
        bool DeleteUsersWorkAreas(UsersWorkAreas usersWorkAreas);
        bool ExistUsersWorkAreas(int categoryID);

        bool Save();
    }
}
