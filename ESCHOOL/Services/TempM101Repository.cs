using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class TempM101Repository : ITempM101Repository
    {
        private SchoolDbContext _tempM101Context;
        public TempM101Repository(SchoolDbContext tempM101Context)
        {
            _tempM101Context = tempM101Context;
        }
        public IEnumerable<TempM101> GetTempM101All(int schoolID, int userID)
        {
            return _tempM101Context.TempM101.Where(b => b.SchoolID == schoolID && b.UserID == userID).ToList();
        }
        public TempM101 GetTempM101(int ID)
        {
            return _tempM101Context.TempM101.Where(b => b.ID == ID).FirstOrDefault();
        }
        public bool CreateTempM101(TempM101 tempM101)
        {
            _tempM101Context.Add(tempM101);
            return Save();
        }

        public bool UpdateTempM101(TempM101 tempM101)
        {
            _tempM101Context.Update(tempM101);
            return Save();
        }

        public bool DeleteTempM101(TempM101 tempM101)
        {
            _tempM101Context.Remove(tempM101);
            return Save();
        }

        public bool ExistTempM101(int ID)
        {
            return _tempM101Context.TempM101.Any(c => c.ID == ID);
        }
        public bool Save()
        {
            var saved = _tempM101Context.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
