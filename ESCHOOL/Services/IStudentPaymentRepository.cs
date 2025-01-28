using ESCHOOL.Models;
using System;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentPaymentRepository
    {
        IEnumerable<StudentPayment> GetStudentPaymentAll(int schoolID, string period);
        IEnumerable<StudentPayment> GetStudentPayment(int schoolID, string period, int studentID);
        IEnumerable<StudentPayment> GetPaymentOrder(int schoolID, string period, int studentPaymentID);
        IEnumerable<StudentPayment> GetStudentPaymentIE(int schoolID, int studentID, string period, string installmentNo);
        
        StudentPayment GetStudentAccountReceiptBySchool(int schoolID, DateTime paymentDate, int studentID, string period, int accountReceipt);
        StudentPayment GetStudentPaymentID(int studentPaymentID);
        StudentPayment GetStudentAccountReceipt(int schoolID, string period, int studentID, int accountReceipt);
        bool CreateStudentPayment(StudentPayment studentPayment);
        bool UpdateStudentPayment(StudentPayment studentPayment);
        bool DeleteStudentPayment(StudentPayment studentPayment);
        bool ExistStudentPayment(int studentPaymentID);

        bool Save();
    }
}
