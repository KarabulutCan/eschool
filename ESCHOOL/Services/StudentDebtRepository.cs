using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentDebtRepository : IStudentDebtRepository
    {
        private SchoolDbContext _studentDebtContext;
        public StudentDebtRepository(SchoolDbContext studentDebtContext)
        {
            _studentDebtContext = studentDebtContext;
        }
        public IEnumerable<StudentDebt> GetDebtAll(int schoolID, string period)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.SchoolID == schoolID && b.Period == period).ToList();
        }
        public IEnumerable<StudentDebt> GetStudentDebtAllCategory(string period, int schoolID, int claassroomTypeID, int schoolFeeID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.Period == period && b.SchoolID == schoolID && b.ClassroomTypeID == claassroomTypeID && b.SchoolFeeID == schoolFeeID && b.Amount > 0).ToList();
        }
        public IEnumerable<StudentDebt> GetStudentDebtAllCategory2(string period, int schoolID, int schoolFeeID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.Period == period && b.SchoolID == schoolID && b.SchoolFeeID == schoolFeeID && b.Amount > 0).ToList();
        }
        public IEnumerable<StudentDebt> GetStudentDebtAll(int schoolID, string period, int studentID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).ToList();
        }
        public IEnumerable<StudentDebt> GetStudentDebtAll2(string period, int studentID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.StudentID == studentID && b.Period == period).ToList();
        }
        public StudentDebt GetStudentDebtID(int schoolID, int studentDebtID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.SchoolID == schoolID && b.StudentDebtID == studentDebtID).SingleOrDefault();
        }

        public StudentDebt GetStudentDebt2(string period, int schoolID, int studentID, int schoolFeeID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.StudentID == studentID && b.Period == period && b.SchoolID == schoolID && b.StudentDebtID == schoolFeeID).SingleOrDefault();
        }
        public StudentDebt GetStudentDebt22(string period, int schoolID, int studentID, int schoolFeeID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.StudentID == studentID && b.Period == period && b.SchoolID == schoolID && b.SchoolFeeID == schoolFeeID).SingleOrDefault();
        }
        public StudentDebt GetStudentDebt3(string period, int schoolID, int studentID, int schoolFeeID, int classroomTypeID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.StudentID == studentID && b.Period == period && b.SchoolID == schoolID && b.SchoolFeeID == schoolFeeID && b.ClassroomTypeID == classroomTypeID).SingleOrDefault();
        }
        public StudentDebt GetStudentDebt4(string period, int schoolID, int studentID, int studentFeeID)
        {
            return _studentDebtContext.StudentDebt.Where(b => b.StudentID == studentID && b.Period == period && b.SchoolID == schoolID && b.SchoolFeeID == studentFeeID).SingleOrDefault();
        }
        public bool CreateStudentDebt(StudentDebt studentDebt)
        {
            _studentDebtContext.Add(studentDebt);
            return Save();
        }
        public bool UpdateStudentDebt(StudentDebt studentDebt)
        {
            _studentDebtContext.Update(studentDebt);
            return Save();
        }
        public bool DeleteStudentDebt(StudentDebt studentDebt)
        {
            _studentDebtContext.Remove(studentDebt);
            return Save();
        }

        public bool ExistStudentDebt(string period, int studentID)
        {
            return _studentDebtContext.StudentDebt.Any(c => c.Period == period && c.StudentID == studentID);
        }

        public bool Save()
        {
            var saved = _studentDebtContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
