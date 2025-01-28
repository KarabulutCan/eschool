using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentDebtDetailTableRepository : IStudentDebtDetailTableRepository
    {
        private SchoolDbContext _studentDebtDetailTableContext;
        public StudentDebtDetailTableRepository(SchoolDbContext studentDebtDetailTableContext)
        {
            _studentDebtDetailTableContext = studentDebtDetailTableContext;
        }

        public IEnumerable<StudentDebtDetailTable> GetStudentDebtDetailTable(int schoolID,  string period, int studentID)
        {
            return _studentDebtDetailTableContext.StudentDebtDetailTable.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).ToList();
        }

        public StudentDebtDetailTable GetStudentDebtDetailTableID(int studentDebtDetailTableID)
        {
            return _studentDebtDetailTableContext.StudentDebtDetailTable.Where(b => b.StudentDebtTableID == studentDebtDetailTableID).SingleOrDefault();
        }
        public IEnumerable<StudentDebtDetailTable> GetStudentDebtDetailTable1(int studentID, string period)
        {
            return _studentDebtDetailTableContext.StudentDebtDetailTable.Where(b => b.StudentID == studentID && b.Period == period).ToList();
        }

        public StudentDebtDetailTable GetStudentDebtDetailTable2(int studentID, int schoolFeeID, string period)
        {
            return _studentDebtDetailTableContext.StudentDebtDetailTable.Where(b => b.StudentID == studentID && b.Period == period && b.SchoolFeeID == schoolFeeID).SingleOrDefault();
        }

        public bool CreateStudentDebtDetailTable(StudentDebtDetailTable studentDebtDetailTable)
        {
            _studentDebtDetailTableContext.Add(studentDebtDetailTable);
            return Save();
        }
        public bool UpdateStudentDebtDetailTable(StudentDebtDetailTable studentDebtDetailTable)
        {
            _studentDebtDetailTableContext.Update(studentDebtDetailTable);
            return Save();
        }
        public bool DeleteStudentDebtDetailTable(StudentDebtDetailTable studentDebtDetailTable)
        {
            _studentDebtDetailTableContext.Remove(studentDebtDetailTable);
            return Save();
        }

        public bool ExistStudentDebtDetailTable(int studentID)
        {
            return _studentDebtDetailTableContext.StudentDebtDetailTable.Any(c => c.StudentID == studentID);
        }

        public bool Save()
        {
            var saved = _studentDebtDetailTableContext.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}

