using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class TempM101HeaderRepository : ITempM101HeaderRepository
    {
        private SchoolDbContext _tempM101HeaderContext;
        public TempM101HeaderRepository(SchoolDbContext tempM101HeaderContext)
        {
            _tempM101HeaderContext = tempM101HeaderContext;
        }

        public IEnumerable<TempM101Header> GetTempM101HeaderAll(int schoolID, int userID)
        {
            return _tempM101HeaderContext.TempM101Header.Where(b => b.SchoolID == schoolID && b.UserID == userID ).ToList();
        }


        public bool CreateTempM101Header(TempM101Header tempM101Header)
        {
            _tempM101HeaderContext.Add(tempM101Header);
            return Save();
        }

        public bool UpdateTempM101Header(TempM101Header tempM101Header)
        {
            _tempM101HeaderContext.Update(tempM101Header);
            return Save();
        }

        public bool DeleteTempM101Header(TempM101Header tempM101Header)
        {
            _tempM101HeaderContext.Remove(tempM101Header);
            return Save();
        }

        public bool ExistTempM101Header(int ID)
        {
            return _tempM101HeaderContext.TempM101Header.Any(c => c.ID == ID);
        }
        public bool Save()
        {
            var saved = _tempM101HeaderContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
