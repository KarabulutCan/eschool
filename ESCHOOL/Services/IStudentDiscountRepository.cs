using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentDiscountRepository
    {
        IEnumerable<StudentDiscount> GetDiscountAll();
        IEnumerable<StudentDiscount> GetDiscountID(int studentDebtID);
        IEnumerable<StudentDiscount> GetDiscount2(int schoolID, string period, int studentID);
        IEnumerable<StudentDiscount> GetDiscount4(int ID, string period, int? schoolID, int discountTableID);
        StudentDiscount GetDiscount(int ID, string period, int? schoolID, int discountTableID, int studentDebtID);
        bool CreateStudentDiscount(StudentDiscount studentDiscount);
        bool UpdateStudentDiscount(StudentDiscount studentDiscount);
        bool DeleteStudentDiscount(StudentDiscount studentDiscount);
        bool Save();
    }
}
