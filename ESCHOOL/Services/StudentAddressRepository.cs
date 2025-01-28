using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentAddressRepository : IStudentAddressRepository
    {
        private SchoolDbContext _studentAddressContext;
        public StudentAddressRepository(SchoolDbContext studentAddressContext)
        {
            _studentAddressContext = studentAddressContext;
        }

        public StudentAddress GetStudentAddress(int studentID)
        {
            return _studentAddressContext.StudentAddress.Where(b => b.StudentID == studentID).FirstOrDefault();
        }

        public IEnumerable<StudentAddress> GetStudentAddressAll()
        {
            return _studentAddressContext.StudentAddress.OrderBy(c => c.StudentAddressID);
        }

        public bool CreateStudentAddress(StudentAddress studentAddress)
        {
            _studentAddressContext.Add(studentAddress);
            return Save();
        }
        public bool UpdateStudentAddress(StudentAddress studentAddress)
        {
            _studentAddressContext.Update(studentAddress);
            return Save();
        }
        public bool DeleteStudentAddress(StudentAddress studentAddress)
        {
            _studentAddressContext.Remove(studentAddress);
            return Save();
        }

        public bool ExistStudentAddress(int studentAddressID, int studentID)
        {
            return _studentAddressContext.StudentAddress.Any(c => c.StudentAddressID == studentAddressID && c.StudentID == studentID);
        }

        public bool Save()
        {
            var saved = _studentAddressContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
