using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class UsersTaskDataSourceRepository : IUsersTaskDataSourceRepository
    {
        private SchoolDbContext _usersTaskDataSourceContext;
        public UsersTaskDataSourceRepository(SchoolDbContext usersTaskDataSourceContext)
        {
            _usersTaskDataSourceContext = usersTaskDataSourceContext;
        }

        IEnumerable<UsersTaskDataSource> IUsersTaskDataSourceRepository.GetUsersTaskAll(int schoolID, int userID)
        {
            return _usersTaskDataSourceContext.UsersTaskDataSource.OrderBy(c => c.Start).Where(c => c.SchoolID == schoolID && c.UserID == userID).ToList();
        }
        public UsersTaskDataSource GetUsersTask(int schoolID, int userID, int taskID)
        {
            return _usersTaskDataSourceContext.UsersTaskDataSource.Where(c => c.SchoolID == schoolID && c.UserID == userID && c.TaskID == taskID).FirstOrDefault();
        }
       
        public bool CreateUsersTask(UsersTaskDataSource UsersTaskDataSource)
        {
            _usersTaskDataSourceContext.Add(UsersTaskDataSource);
            return Save();
        }

        public bool UpdateUsersTask(UsersTaskDataSource UsersTaskDataSource)
        {
            _usersTaskDataSourceContext.Update(UsersTaskDataSource);
            return Save();
        }

        public bool DeleteUsersTask(UsersTaskDataSource UsersTaskDataSource)
        {
            _usersTaskDataSourceContext.Remove(UsersTaskDataSource);
            return Save();
        }

        public bool ExistUsersTask(int schoolID, int UserID, int taskID)
        {
            return _usersTaskDataSourceContext.UsersTaskDataSource.Any(c => c.SchoolID == schoolID && c.UserID == UserID && c.TaskID == taskID);
        }
        public bool Save()
        {
            var saved = _usersTaskDataSourceContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
