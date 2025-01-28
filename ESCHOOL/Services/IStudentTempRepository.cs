using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentTempRepository
    {
        StudentTemp GetStudentTemp(int schoolID, string period, int studentID);
        StudentTemp GetStudent(int studentID);
        IEnumerable<StudentTemp> GetStudentTempByPeriod(int schoolID, string period, int studentID);
        IEnumerable<StudentTemp> GetStudentTempAll(int schoolID, string period);
        IEnumerable<StudentTemp> GetStudentTempAllCash(string period);
        
        bool CreateStudentTemp(StudentTemp studentTemp);
        bool UpdateStudentTemp(StudentTemp studentTemp);
        bool DeleteStudentTemp(StudentTemp studentTemp);

        bool Save();
    }
}
