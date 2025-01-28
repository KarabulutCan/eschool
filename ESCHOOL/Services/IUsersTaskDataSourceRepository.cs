using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IUsersTaskDataSourceRepository
    {
        IEnumerable<UsersTaskDataSource> GetUsersTaskAll(int schoolID, int userID);
        UsersTaskDataSource GetUsersTask(int schoolID, int userID, int taskID);

        bool CreateUsersTask(UsersTaskDataSource UsersTaskDataSource);
        bool UpdateUsersTask(UsersTaskDataSource UsersTaskDataSource);
        bool DeleteUsersTask(UsersTaskDataSource UsersTaskDataSource);
        bool ExistUsersTask(int schoolID, int userID, int taskID);

        bool Save();
    }
}
