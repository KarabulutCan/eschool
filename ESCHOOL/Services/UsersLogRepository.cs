using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class UsersLogRepository : IUsersLogRepository
    {
        private SchoolDbContext _usersLogContext;
        public UsersLogRepository(SchoolDbContext usersLogContext)
        {
            _usersLogContext = usersLogContext;
        }

        IEnumerable<UsersLog> IUsersLogRepository.GetUsersLogAll(string period)
        {
            return _usersLogContext.UsersLog.Where(b => b.Period == period).OrderByDescending(b=> b.UserLogDate).ToList();
        }

        public UsersLog GetUsersLog(int userLogID)
        {
            return _usersLogContext.UsersLog.Where(b => b.UserLogID == userLogID).FirstOrDefault();
        }
        public bool CreateUsersLog(UsersLog usersLog)
        {
            _usersLogContext.Add(usersLog);
            return Save();
        }

        public bool UpdateUsersLog(UsersLog usersLog)
        {
            _usersLogContext.Update(usersLog);
            return Save();
        }

        public bool DeleteUsersLog(UsersLog usersLog)
        {
            _usersLogContext.Remove(usersLog);
            return Save();
        }

        public bool ExistUsersLog(int userLogID)
        {
            return _usersLogContext.UsersLog.Any(c => c.UserLogID == userLogID);
        }
        public bool Save()
        {
            var saved = _usersLogContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
