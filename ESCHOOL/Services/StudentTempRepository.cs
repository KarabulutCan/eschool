using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentTempRepository : IStudentTempRepository
    {
        private SchoolDbContext _studentTempContext;
        public StudentTempRepository(SchoolDbContext studentTempContext)
        {
            _studentTempContext = studentTempContext;
        }
        public StudentTemp GetStudentTemp(int schoolID, string period, int studentID)
        {
            return _studentTempContext.StudentTemp.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID).FirstOrDefault();
        }
        public StudentTemp GetStudent(int studentID)
        {
            return _studentTempContext.StudentTemp.Where(b => b.StudentID == studentID).FirstOrDefault();
        }
        public IEnumerable<StudentTemp> GetStudentTempByPeriod(int schoolID, string period, int studentID)
        {
            return _studentTempContext.StudentTemp.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID).ToList();
        }

        public IEnumerable<StudentTemp> GetStudentTempAllCash(string period)
        {
            return _studentTempContext.StudentTemp.Where(c => c.CashPayment > 0 && c.Period == period).ToList();
        }

        public IEnumerable<StudentTemp> GetStudentTempAll(int schoolID, string period)
        {
            return _studentTempContext.StudentTemp.OrderBy(c => c.StudentTempID).Where(c => c.SchoolID == schoolID && c.Period == period).ToList(); 
        }

        public bool CreateStudentTemp(StudentTemp studentTemp)
        {
            _studentTempContext.Add(studentTemp);
            return Save();
        }
        public bool UpdateStudentTemp(StudentTemp studentTemp)
        {
            _studentTempContext.Update(studentTemp);
            return Save();
        }
        public bool DeleteStudentTemp(StudentTemp studentTemp)
        {
            _studentTempContext.Remove(studentTemp);
            return Save();
        }

        public bool Save()
        {
            var saved = _studentTempContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
