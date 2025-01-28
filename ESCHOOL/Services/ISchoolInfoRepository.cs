using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ISchoolInfoRepository
    {
        IEnumerable<SchoolInfo> GetSchoolInfoAll();
        IEnumerable<SchoolInfo> GetSchoolInfoAllTrue();
        SchoolInfo GetSchoolInfoDefault();
        SchoolInfo GetSchoolInfo(int schoolID);
        bool CreateSchoolInfo(SchoolInfo schoolInfo);
        bool UpdateSchoolInfo(SchoolInfo schoolInfo);
        bool DeleteSchoolInfo(SchoolInfo schoolInfo);
        bool Save();
    }
}
