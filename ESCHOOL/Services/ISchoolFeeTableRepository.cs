using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ISchoolFeeTableRepository
    {
        IEnumerable<SchoolFeeTable> GetSchoolFeeTableAll(int schoolID, string period);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTableAllActive(int schoolID, string period);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllStatus(int schoolID, string period, int categoryID);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllSchoolFeeID(int schoolID, string period, int schoolFeeID);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllSchoolFeeID2(int schoolID, string period, int categoryID, int schoolFeeID);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllSchoolFeeID20(int schoolID, string period, int schoolFeeID, int schoolFeeSubID);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllSchoolFeeID21(int schoolID, string period, int categoryID, int schoolFeeID, int schoolFeeSubID);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllSchoolFeeID22(int schoolID, string period, int categoryID);
        IEnumerable<SchoolFeeTable> GetSchoolFeeTablePeriodAllSchoolFeeID3(int schoolID, string period);
        SchoolFeeTable GetSchoolFees(string period, int classroomTypeID, int schoolFeeID, int feeCategory);
        SchoolFeeTable GetSchoolFees2(int schoolID, string period, int feeCategory, int categoryID, int schoolFeeID);
        SchoolFeeTable GetSchoolFees22(int schoolID, string period, int feeCategory, int categoryID, int schoolFeeID, int schoolFeeSubID);

        SchoolFeeTable GetSchoolFeeTable(int schoolFeeTableID);
        bool CreateSchoolFeeTable(SchoolFeeTable schoolFeeTable);
        bool UpdateSchoolFeeTable(SchoolFeeTable schoolFeeTable);
        bool DeleteSchoolFeeTable(SchoolFeeTable schoolFeeTable);
        bool ExistSchoolFeeTable(int schoolFeeTable);
        bool Save();
    }
}

