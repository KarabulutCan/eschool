using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentFamilyAddressRepository : IStudentFamilyAddressRepository
    {
        private SchoolDbContext _studentFamilyAddressContext;
        public StudentFamilyAddressRepository(SchoolDbContext studentFamilyAddressContext)
        {
            _studentFamilyAddressContext = studentFamilyAddressContext;
        }

        public StudentFamilyAddress GetStudentFamilyAddress(int studentID)
        {
            return _studentFamilyAddressContext.StudentFamilyAddress.Where(b => b.StudentID == studentID).FirstOrDefault();
        }
        public IEnumerable<StudentFamilyAddress> GetStudentFamilyAddressAll()
        {
            return _studentFamilyAddressContext.StudentFamilyAddress.ToList();
        }

        public bool CreateStudentFamilyAddress(StudentFamilyAddress studentFamilyAddress)
        {
            _studentFamilyAddressContext.Add(studentFamilyAddress);
            return Save();
        }
        public bool UpdateStudentFamilyAddress(StudentFamilyAddress studentFamilyAddress)
        {
            _studentFamilyAddressContext.Update(studentFamilyAddress);
            return Save();
        }

        public bool DeleteStudentFamilyAddress(StudentFamilyAddress studentFamilyAddress)
        {
            _studentFamilyAddressContext.Remove(studentFamilyAddress);
            return Save();
        }
        public bool ExistStudentFamilyAddress(int studentFamilyAddressID)
        {
            _studentFamilyAddressContext.Remove(studentFamilyAddressID);
            return Save();
        }

        public bool Save()
        {
            var saved = _studentFamilyAddressContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
