using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentParentAddressRepository
    {
        IEnumerable<StudentParentAddress> GetStudentParentAddressAll();
        StudentParentAddress GetStudentParentAddress(int studentID);
        bool CreateStudentParentAddress(StudentParentAddress studentParentAddress);
        bool UpdateStudentParentAddress(StudentParentAddress studentParentAddress);
        bool DeleteStudentParentAddress(StudentParentAddress studentParentAddress);
        bool ExistStudentParentAddress(int studentParentAddressID);

        bool Save();
    }
}
