using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class SchoolInfoRepository : ISchoolInfoRepository
    {
        private SchoolDbContext _schoolInfoContext;
        public SchoolInfoRepository(SchoolDbContext schoolInfoContext)
        {
            _schoolInfoContext = schoolInfoContext;
        }

        public SchoolInfo GetSchoolInfo(int schoolID)
        {
            return _schoolInfoContext.SchoolInfo.Where(b => b.SchoolID == schoolID).FirstOrDefault();
        }

        public SchoolInfo GetSchoolInfoDefault()
        {
            return _schoolInfoContext.SchoolInfo.First();
        }
        IEnumerable<SchoolInfo> ISchoolInfoRepository.GetSchoolInfoAll()
        {
            return _schoolInfoContext.SchoolInfo.OrderBy(c => c.SortOrder).ThenBy(s => s.CompanyName);
        }
        IEnumerable<SchoolInfo> ISchoolInfoRepository.GetSchoolInfoAllTrue()
        {
            return _schoolInfoContext.SchoolInfo.Where(a => a.IsActive == true);
        }

        public bool CreateSchoolInfo(SchoolInfo schoolInfo)
        {
            _schoolInfoContext.Add(schoolInfo);
            return Save();
        }

        public bool DeleteSchoolInfo(SchoolInfo schoolInfo)
        {
            _schoolInfoContext.Remove(schoolInfo);
            return Save();
        }

        public bool UpdateSchoolInfo(SchoolInfo schoolInfo)
        {
            _schoolInfoContext.Update(schoolInfo);
            return Save();
        }

        public bool Save()
        {
            var saved = _schoolInfoContext.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}
