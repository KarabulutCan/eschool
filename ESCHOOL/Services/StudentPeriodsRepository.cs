using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentPeriodsRepository : IStudentPeriodsRepository
    {
        private SchoolDbContext _studentPeriodsContext;
        public StudentPeriodsRepository(SchoolDbContext studentPeriodsContext)
        {
            _studentPeriodsContext = studentPeriodsContext;
        }
        public StudentPeriods GetStudentPeriod(int schoolID, int studentID, string period)
        {
            return _studentPeriodsContext.StudentPeriods.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).FirstOrDefault();
        }
        public IEnumerable<StudentPeriods> GetStudentPeriod2(int schoolID, string period, int studentID)
        {
            return _studentPeriodsContext.StudentPeriods.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID).ToList();
        }
        public IEnumerable<StudentPeriods> GetStudent(int schoolID, int studentID)
        {
            return _studentPeriodsContext.StudentPeriods.Where(b => b.SchoolID == schoolID && b.StudentID == studentID).ToList();
        }
        public IEnumerable<StudentPeriods> GetStudentAll(int schoolID, string period)
        {
            return _studentPeriodsContext.StudentPeriods.Where(b => b.SchoolID == schoolID && b.Period == period).ToList();
        }
        public bool CreateStudentPeriods(StudentPeriods studentPeriods)
        {
            _studentPeriodsContext.Add(studentPeriods);
            return Save();
        }
        public bool UpdateStudentPeriods(StudentPeriods studentPeriods)
        {
            _studentPeriodsContext.Update(studentPeriods);
            return Save();
        }
        public bool DeleteStudentPeriods(StudentPeriods studentPeriods)
        {
            _studentPeriodsContext.Remove(studentPeriods);
            return Save();
        }

        public bool ExistStudentPeriods(int schoolID, int studentID, string period)
        {
            return _studentPeriodsContext.StudentPeriods.Any(c => c.SchoolID == schoolID && c.StudentID == studentID && c.Period == period);
        }

        public bool Save()
        {
            var saved = _studentPeriodsContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
