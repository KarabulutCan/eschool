using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class TempDataRepository : ITempDataRepository
    {
        private SchoolDbContext _studentInstallmentContext;
        public TempDataRepository(SchoolDbContext studentInstallmentContext)
        {
            _studentInstallmentContext = studentInstallmentContext;
        }

        public IEnumerable<TempData> GetTempData(int studentID)
        {
            return _studentInstallmentContext.TempData.Where(b => b.StudentID == studentID).ToList(); ;
        }

        public bool CreateTempData(TempData studentInstallment)
        {
            _studentInstallmentContext.Add(studentInstallment);
            return Save();
        }
        public bool UpdateTempData(TempData studentInstallment)
        {
            _studentInstallmentContext.Update(studentInstallment);
            return Save();
        }
        public bool DeleteTempData(TempData studentInstallment)
        {
            _studentInstallmentContext.Remove(studentInstallment);
            return Save();
        }

        public bool ExistTempData(int studentID)
        {
            return _studentInstallmentContext.TempData.Any(c => c.StudentID == studentID);
        }

        public bool Save()
        {
            var saved = _studentInstallmentContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
