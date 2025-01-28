using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentDebtDetailTableRepository
    {
        IEnumerable<StudentDebtDetailTable> GetStudentDebtDetailTable(int schoolID, string period, int studentID);
        StudentDebtDetailTable GetStudentDebtDetailTableID(int studentDebtDetailTableID);
        IEnumerable<StudentDebtDetailTable> GetStudentDebtDetailTable1(int studentID, string period);
        StudentDebtDetailTable GetStudentDebtDetailTable2(int schoolFeeID, int studentID, string period);

        bool CreateStudentDebtDetailTable(StudentDebtDetailTable studentDebtDetailTable);
        bool UpdateStudentDebtDetailTable(StudentDebtDetailTable studentDebtDetailTable);
        bool DeleteStudentDebtDetailTable(StudentDebtDetailTable studentDebtDetailTable);
        bool ExistStudentDebtDetailTable(int studentID);

        bool Save();
    }
}
