using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentAddressRepository
    {
        IEnumerable<StudentAddress> GetStudentAddressAll();
        StudentAddress GetStudentAddress(int studentID);

        bool CreateStudentAddress(StudentAddress studentAddress);
        bool UpdateStudentAddress(StudentAddress studentAddress);
        bool DeleteStudentAddress(StudentAddress studentAddress);
        bool ExistStudentAddress(int studentAddressID, int studentAddressTypeSW);

        bool Save();
    }
}
