using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentDebtDetailRepository : IStudentDebtDetailRepository
    {
        private SchoolDbContext _studentDebtDetailContext;
        public StudentDebtDetailRepository(SchoolDbContext studentDebtDetailContext)
        {
            _studentDebtDetailContext = studentDebtDetailContext;
        }

        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllCategory(string period, int schoolID, int studentID, int schoolFeeID, int studentDebtID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID && b.StudentDebtID == studentDebtID && b.SchoolFeeID == schoolFeeID && b.Amount > 0).ToList();
        }
        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailAll(int schoolID, string period, int studentID, int schoolFeeID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).ToList();
        }
        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllSchool(int schoolID, string period, int schoolFeeID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.Period == period && b.SchoolFeeID == schoolFeeID).ToList();
        }
        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllSchool2(int schoolID, string period)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.Period == period).ToList();
        }
        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllSchool3(int schoolID, string period, int studentID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID).ToList();
        }
        public StudentDebtDetail GetStudentDebtDetailID(int schoolID, int studentID, int studentFeeID, int studentDebtID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.StudentDebtID == studentDebtID && b.SchoolFeeID == studentFeeID).SingleOrDefault();
        }
        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailID1(int schoolID, int studentID, int studentDebtID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.StudentDebtID == studentDebtID).ToList();
        }
        public IEnumerable<StudentDebtDetail> GetStudentDebtDetailID2(int schoolID, string period, int categoryID, int schoolFeeID, int studentDebtID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Where(b => b.SchoolID == schoolID && b.Period == period && b.CategoryID == categoryID && b.SchoolFeeID == schoolFeeID && b.StudentDebtID == studentDebtID).ToList();
        }

        public bool CreateStudentDebtDetail(StudentDebtDetail studentDebtDetail)
        {
            _studentDebtDetailContext.Add(studentDebtDetail);
            return Save();
        }
        public bool UpdateStudentDebtDetail(StudentDebtDetail studentDebtDetail)
        {
            _studentDebtDetailContext.Update(studentDebtDetail);
            return Save();
        }
        public bool DeleteStudentDebtDetail(StudentDebtDetail studentDebtDetail)
        {
            _studentDebtDetailContext.Remove(studentDebtDetail);
            return Save();
        }

        public bool ExistStudentDebtDetail(string period, int studentID)
        {
            return _studentDebtDetailContext.StudentDebtDetail.Any(c => c.Period == period && c.StudentID == studentID);
        }

        public bool Save()
        {
            var saved = _studentDebtDetailContext.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}
