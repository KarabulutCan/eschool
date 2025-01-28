using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentDebtRepository
    {
        IEnumerable<StudentDebt> GetDebtAll(int schoolID, string period);
        IEnumerable<StudentDebt> GetStudentDebtAll(int schoolID, string period, int studentID);
        IEnumerable<StudentDebt> GetStudentDebtAll2(string period, int studentID);
        IEnumerable<StudentDebt> GetStudentDebtAllCategory(string period, int schoolID, int claassroomTypeID, int schoolFeeID);
        IEnumerable<StudentDebt> GetStudentDebtAllCategory2(string period, int schoolID, int schoolFeeID);

        StudentDebt GetStudentDebtID(int schoolID, int studentDebtID);
        StudentDebt GetStudentDebt2(string period, int schoolID, int studentID, int schoolFeeID);
        StudentDebt GetStudentDebt22(string period, int schoolID, int studentID, int schoolFeeID);
        StudentDebt GetStudentDebt3(string period, int schoolID, int studentID, int schoolFeeID, int classroomTypeID);
        StudentDebt GetStudentDebt4(string period, int schoolID, int studentID, int studentDebtID);

        bool CreateStudentDebt(StudentDebt studentDebt);
        bool UpdateStudentDebt(StudentDebt studentDebt);
        bool DeleteStudentDebt(StudentDebt studentDebt);
        bool ExistStudentDebt(string period, int studentID);

        bool Save();
    }
}
