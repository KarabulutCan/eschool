using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentParentAddressRepository : IStudentParentAddressRepository
    {
        private SchoolDbContext _studentParentAddressContext;
        public StudentParentAddressRepository(SchoolDbContext studentParentAddressContext)
        {
            _studentParentAddressContext = studentParentAddressContext;
        }

        public IEnumerable<StudentParentAddress> GetStudentParentAddressAll()
        {
            return _studentParentAddressContext.StudentParentAddress.ToList();
        }

        public StudentParentAddress GetStudentParentAddress(int studentID)
        {
            return _studentParentAddressContext.StudentParentAddress.Where(b => b.StudentID == studentID).FirstOrDefault();
        }

        public bool CreateStudentParentAddress(StudentParentAddress studentParentAddress)
        {
            _studentParentAddressContext.Add(studentParentAddress);
            return Save();
        }
        public bool UpdateStudentParentAddress(StudentParentAddress studentParentAddress)
        {
            _studentParentAddressContext.Update(studentParentAddress);
            return Save();
        }

        public bool DeleteStudentParentAddress(StudentParentAddress studentParentAddress)
        {
            _studentParentAddressContext.Remove(studentParentAddress);
            return Save();
        }
        public bool ExistStudentParentAddress(int studentParentAddressID)
        {
            _studentParentAddressContext.Remove(studentParentAddressID);
            return Save();
        }

        public bool Save()
        {
            var saved = _studentParentAddressContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }

}