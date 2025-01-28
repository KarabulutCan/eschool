using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentDebtDetailRepository
    {
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailAll(int schoolID, string period, int studentID, int schoolFeeID);
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllCategory(string period, int schoolID, int studentID, int studentDebtID, int schoolFeeID);
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllSchool(int schoolID, string period, int schoolFeeID);
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllSchool2(int schoolID, string period);
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailAllSchool3(int schoolID, string period, int studentID);
        StudentDebtDetail GetStudentDebtDetailID(int schoolID, int studentID, int studentDebtID, int studentFeeID);
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailID1(int schoolID, int studentID, int studentDebtID);
        IEnumerable<StudentDebtDetail> GetStudentDebtDetailID2(int schoolID, string period, int categoryID, int schoolFeeID, int studentDebtID);
        bool CreateStudentDebtDetail(StudentDebtDetail studentDebtDetail);
        bool UpdateStudentDebtDetail(StudentDebtDetail studentDebtDetail);
        bool DeleteStudentDebtDetail(StudentDebtDetail studentDebtDetail);
        bool ExistStudentDebtDetail(string period, int studentID);

        bool Save();
    }
}
