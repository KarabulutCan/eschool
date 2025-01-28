using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class PSerialNumberRepository : IPSerialNumberRepository
    {
        private SchoolDbContext _pSerialNumberContext;
        public PSerialNumberRepository(SchoolDbContext pSerialNumberContext)
        {
            _pSerialNumberContext = pSerialNumberContext;
        }
        IEnumerable<PSerialNumber> IPSerialNumberRepository.GetPSerialNumbers()
        {
            return _pSerialNumberContext.PSerialNumber.ToList();
        }
        public PSerialNumber GetPSerialNumber(int pSerialNumberID)
        {
            return _pSerialNumberContext.PSerialNumber.Where(a => a.PSerialNumberID == pSerialNumberID).FirstOrDefault();
        }

        public bool CreatePSerialNumber(PSerialNumber pSerialNumber)
        {
            _pSerialNumberContext.Add(pSerialNumber);
            return Save();
        }

        public bool DeletePSerialNumber(PSerialNumber pSerialNumber)
        {
            _pSerialNumberContext.Remove(pSerialNumber);
            return Save();
        }

        public bool UpdatePSerialNumber(PSerialNumber pSerialNumber)
        {
            _pSerialNumberContext.Update(pSerialNumber);
            return Save();
        }

        public bool Save()
        {
            var saved = _pSerialNumberContext.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}
