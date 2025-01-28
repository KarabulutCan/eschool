using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class UsersWorkAreasRepository : IUsersWorkAreasRepository
    {
        private SchoolDbContext _usersWorkAreasContext;
        public UsersWorkAreasRepository(SchoolDbContext usersWorkAreasContext)
        {
            _usersWorkAreasContext = usersWorkAreasContext;
        }

        IEnumerable<UsersWorkAreas> IUsersWorkAreasRepository.GetUsersWorkAreasAll()
        {
            return _usersWorkAreasContext.UsersWorkAreas.ToList();
        }
        IEnumerable<UsersWorkAreas> IUsersWorkAreasRepository.GetUserWorkAreasID(int userID)
        {
            return _usersWorkAreasContext.UsersWorkAreas.Where(b => b.UserID == userID).ToList();
        }

        public UsersWorkAreas GetUsersWorkAreas(int userID, int categoryID)
        {
            return _usersWorkAreasContext.UsersWorkAreas.Where(b => b.UserID == userID && b.CategoryID == categoryID).FirstOrDefault();
        }
        public bool CreateUsersWorkAreas(UsersWorkAreas usersWorkAreas)
        {
            _usersWorkAreasContext.Add(usersWorkAreas);
            return Save();
        }

        public bool UpdateUsersWorkAreas(UsersWorkAreas usersWorkAreas)
        {
            _usersWorkAreasContext.Update(usersWorkAreas);
            return Save();
        }

        public bool DeleteUsersWorkAreas(UsersWorkAreas usersWorkAreas)
        {
            _usersWorkAreasContext.Remove(usersWorkAreas);
            return Save();
        }

        public bool ExistUsersWorkAreas(int categoryID)
        {
            return _usersWorkAreasContext.UsersWorkAreas.Any(c => c.CategoryID == categoryID);
        }
        public bool Save()
        {
            var saved = _usersWorkAreasContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
