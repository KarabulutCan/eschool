using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentFamilyAddressRepository
    {
        StudentFamilyAddress GetStudentFamilyAddress(int studentID);
        IEnumerable<StudentFamilyAddress> GetStudentFamilyAddressAll();
        bool CreateStudentFamilyAddress(StudentFamilyAddress studentFamilyAddress);
        bool UpdateStudentFamilyAddress(StudentFamilyAddress studentFamilyAddress);
        bool DeleteStudentFamilyAddress(StudentFamilyAddress studentFamilyAddress);
        bool ExistStudentFamilyAddress(int studentFamilyAddressID);

        bool Save();
    }
}
