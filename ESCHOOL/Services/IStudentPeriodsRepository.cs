using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentPeriodsRepository
    {
        StudentPeriods GetStudentPeriod(int schoolID, int studentID, string period);
        IEnumerable<StudentPeriods> GetStudentPeriod2(int schoolID, string period, int studentID);
        IEnumerable<StudentPeriods> GetStudentAll(int schoolID, string period);
        IEnumerable<StudentPeriods> GetStudent(int schoolID, int studentID);

        bool CreateStudentPeriods(StudentPeriods studentPeriods);
        bool UpdateStudentPeriods(StudentPeriods studentPeriods);
        bool DeleteStudentPeriods(StudentPeriods studentPeriods);
        bool ExistStudentPeriods(int schoolID, int studentID, string period);

        bool Save();
    }
}
