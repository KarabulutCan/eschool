using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class MultipurposeListRepository : IMultipurposeListRepository
    {
        private SchoolDbContext _multipurposeListContext;
        public MultipurposeListRepository(SchoolDbContext multipurposeListContext)
        {
            _multipurposeListContext = multipurposeListContext;
        }

        IEnumerable<MultipurposeList> IMultipurposeListRepository.GetMultipurposeListAll()
        {
            return _multipurposeListContext.MultipurposeList.OrderBy(c => c.MultipurposeListID).ToList();
        }

        IEnumerable<MultipurposeList> IMultipurposeListRepository.GetMultipurposeListTrue()
        {
            return _multipurposeListContext.MultipurposeList.Where(b => b.IsSelect == true).ToList();
        }

        public MultipurposeList GetMultipurposeListName(string name)
        {
            return _multipurposeListContext.MultipurposeList.Where(b => b.Name == name).FirstOrDefault();
        }

        public MultipurposeList GetMultipurposeListID(int ID)
        {
            return _multipurposeListContext.MultipurposeList.Where(b => b.MultipurposeListID == ID).FirstOrDefault();
        }

        public bool CreateMultipurposeList(MultipurposeList multipurposeList)
        {
            _multipurposeListContext.Add(multipurposeList);
            return Save();
        }

        public bool UpdateMultipurposeList(MultipurposeList multipurposeList)
        {
            _multipurposeListContext.Update(multipurposeList);
            return Save();
        }

        public bool DeleteMultipurposeList(MultipurposeList multipurposeList)
        {
            _multipurposeListContext.Remove(multipurposeList);
            return Save();
        }

        public bool Save()
        {
            var saved = _multipurposeListContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
