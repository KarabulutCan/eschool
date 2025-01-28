using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class SchoolFeeTableRepository : ISchoolFeeTableRepository
    {
        private SchoolDbContext _schoolFeeTableContext;
        public SchoolFeeTableRepository(SchoolDbContext schoolFeeTableContext)
        {
            _schoolFeeTableContext = schoolFeeTableContext;
        }

        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTableAll(int schoolID, string period)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(d => d.SchoolID == schoolID).ToList();
        }
        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTableAllActive(int schoolID, string period)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.IsActive == true).ToList();
        }
        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllStatus(int schoolID, string period, int categoryID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.CategoryID == categoryID).ToList();
        }
        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(int schoolID, string period, int schoolFeeID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.SchoolFeeID == schoolFeeID).ToList();
        }

        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID2(int schoolID, string period, int categoryID, int schoolFeeID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.CategoryID == categoryID && c.SchoolFeeID == schoolFeeID).ToList();
        }
        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID20(int schoolID, string period, int schoolFeeID, int schoolFeeSubID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.SchoolFeeID == schoolFeeID && c.SchoolFeeSubID == schoolFeeSubID).ToList();
        }
        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID21(int schoolID, string period, int categoryID, int schoolFeeID, int schoolFeeSubID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.CategoryID == categoryID && c.SchoolFeeID == schoolFeeID && c.SchoolFeeSubID == schoolFeeSubID).ToList();
        }

        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID22(int schoolID, string period, int categoryID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.CategoryID == categoryID).ToList();
        }
        IEnumerable<SchoolFeeTable> ISchoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID3(int schoolID, string period)
        {
            return _schoolFeeTableContext.SchoolFeeTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID).ToList();
        }

        public SchoolFeeTable GetSchoolFees(string period, int classroomTypeID, int schoolFeeID, int feeCategory)
        {
            return _schoolFeeTableContext.SchoolFeeTable.Where(c => c.Period == period && c.IsActive == true && c.CategoryID == classroomTypeID && c.SchoolFeeID == schoolFeeID && c.FeeCategory == feeCategory).FirstOrDefault();
        }

        public SchoolFeeTable GetSchoolFees2(int schoolID, string period, int feeCategory, int categoryID, int schoolFeeID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.Where(c => c.SchoolID == schoolID && c.Period == period && c.FeeCategory == feeCategory && c.CategoryID == categoryID && c.SchoolFeeID == schoolFeeID).FirstOrDefault();
        }
        public SchoolFeeTable GetSchoolFees22(int schoolID, string period, int feeCategory, int categoryID, int schoolFeeID, int schoolFeeSubID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.Where(c => c.SchoolID == schoolID && c.Period == period && c.FeeCategory == feeCategory && c.CategoryID == categoryID && c.SchoolFeeID == schoolFeeID && c.SchoolFeeSubID == schoolFeeSubID).FirstOrDefault();
        }
        public SchoolFeeTable GetSchoolFeeTable(int schoolFeeTableID)
        {
            return _schoolFeeTableContext.SchoolFeeTable.Where(b => b.SchoolFeeTableID == schoolFeeTableID).FirstOrDefault();
        }
        public bool DeleteSchoolFee(SchoolFeeTable schoolFeeTable)
        {
            _schoolFeeTableContext.Remove(schoolFeeTable);
            return Save();
        }
        public bool CreateSchoolFeeTable(SchoolFeeTable schoolFeeTable)
        {
            _schoolFeeTableContext.Add(schoolFeeTable);
            return Save();
        }
        public bool Save()
        {
            var saved = _schoolFeeTableContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateSchoolFeeTable(SchoolFeeTable schoolFeeTable)
        {
            _schoolFeeTableContext.Update(schoolFeeTable);
            return Save();
        }

        public bool DeleteSchoolFeeTable(SchoolFeeTable schoolFeeTable)
        {
            _schoolFeeTableContext.Remove(schoolFeeTable);
            return Save();
        }

        public bool ExistSchoolFeeTable(int schoolFeeTable)
        {
            throw new NotImplementedException();
        }
    }
}
