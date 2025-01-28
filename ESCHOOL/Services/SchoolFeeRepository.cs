using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class SchoolFeeRepository : ISchoolFeeRepository
    {
        private SchoolDbContext _schoolFeeContext;
        public SchoolFeeRepository(SchoolDbContext schoolFeeContext)
        {
            _schoolFeeContext = schoolFeeContext;
        }

        public SchoolFee GetSchoolFee(int schoolFeeID)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.SchoolFeeID == schoolFeeID && b.IsActive == true).FirstOrDefault();
        }
        public SchoolFee GetSchoolFee2(int schoolFeeID)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.SchoolFeeID == schoolFeeID).FirstOrDefault();
        }

        public IEnumerable<SchoolFee> GetSchoolFeeAll(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.FeeCategory == 1 && b.CategoryLevel == L && b.SchoolID == schoolID).OrderBy(c => c.SortOrder).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolFeeAllTrue(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.SchoolID == schoolID && b.FeeCategory == 1 && b.CategoryLevel == L && b.IsActive == true).OrderBy(c => c.SortOrder).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolFeeOnlyTrue(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.SchoolID == schoolID && b.FeeCategory == 1 && b.CategoryLevel == L && b.IsActive == true && b.IsSelect == true).OrderBy(c => c.SortOrder).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolServiceFeeAll(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.FeeCategory == 2 && b.CategoryLevel == L && b.SchoolID == schoolID).OrderBy(c => c.SortOrder).ToList();
        }

        public IEnumerable<SchoolFee> GetSchoolFeeAllLevel(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.CategoryLevel == L && b.SchoolID == schoolID).ToList();
        }

        public IEnumerable<SchoolFee> GetSchoolFeeLevel(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.IsActive == true &&  b.CategoryLevel == L && b.SchoolID == schoolID).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolFeeLevel2(int schoolID, int schoolFeeID)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.IsActive == true && b.SchoolID == schoolID && b.SchoolFeeSubID == schoolFeeID).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolFeeLevel3(int schoolID, int schoolFeeID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.IsActive == true && b.SchoolID == schoolID && b.SchoolFeeSubID == schoolFeeID && b.CategoryLevel == L).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolFeeSelect(int schoolID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.SchoolID == schoolID && b.FeeCategory == 1 && b.CategoryLevel == L && b.IsSelect == true).OrderBy(c => c.SortOrder).ToList();
        }
        public IEnumerable<SchoolFee> GetSchoolFeeSubControl(int schoolID, int categorySubID, string L)
        {
            return _schoolFeeContext.SchoolFee.Where(b => b.SchoolID == schoolID && b.SchoolFeeSubID == categorySubID && b.CategoryLevel == L && b.IsSelect == true).OrderBy(c => c.SortOrder).ToList();
        }

        public bool CreateSchoolFee(SchoolFee schoolFee)
        {
            _schoolFeeContext.Add(schoolFee);
            return Save();
        }

        public bool DeleteSchoolFee(SchoolFee schoolFee)
        {
            _schoolFeeContext.Remove(schoolFee);
            return Save();
        }

        public bool Save()
        {
            var saved = _schoolFeeContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateSchoolFee(SchoolFee schoolFee)
        {
            _schoolFeeContext.Update(schoolFee);
            return Save();
        }
    }
}
