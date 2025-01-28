using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentInstallmentPaymentRepository
    {
        IEnumerable<StudentInstallmentPayment> GetStudentInstallmentPayment1(string period, int studentID);
        IEnumerable<StudentInstallmentPayment> GetStudentInstallmentPayment2(string period, int studentID, int studentPaymentID);
        IEnumerable<StudentInstallmentPayment> GetStudentInstallmentPaymentByPeriod(int schoolID, string period, int studentID);
        StudentInstallmentPayment GetStudentInstallmentNo(string period, int studentID, int studentInstallmentID);
        bool CreateStudentInstallmentPayment(StudentInstallmentPayment studentInstallmentPayment);
        bool UpdateStudentInstallmentPayment(StudentInstallmentPayment studentInstallmentPayment);
        bool DeleteStudentInstallmentPayment(StudentInstallmentPayment studentInstallmentPayment);
        bool Save();
    }
}
