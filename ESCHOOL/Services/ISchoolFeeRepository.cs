using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ISchoolFeeRepository
    {
        IEnumerable<SchoolFee> GetSchoolFeeAll(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeAllTrue(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeOnlyTrue(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolServiceFeeAll(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeAllLevel(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeLevel(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeLevel2(int schoolID, int schoolFeeID);
        IEnumerable<SchoolFee> GetSchoolFeeLevel3(int schoolID, int schoolFeeID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeSelect(int schoolID, string L);
        IEnumerable<SchoolFee> GetSchoolFeeSubControl(int schoolID, int categorySubID, string L);
        SchoolFee GetSchoolFee(int schoolFeeID);
        SchoolFee GetSchoolFee2(int schoolFeeID);

        bool CreateSchoolFee(SchoolFee schoolFee);
        bool UpdateSchoolFee(SchoolFee schoolFee);
        bool DeleteSchoolFee(SchoolFee schoolFee);

        bool Save();
    }
}
