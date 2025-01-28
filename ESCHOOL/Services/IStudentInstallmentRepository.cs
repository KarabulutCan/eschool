using ESCHOOL.Models;
using System;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentInstallmentRepository
    {
        IEnumerable<StudentInstallment> GetStudentInstallmentAll(int schoolID, string period, int categoryID);
        IEnumerable<StudentInstallment> GetStudentInstallment(int schoolID, int studentInstallmentID, string period);
        IEnumerable<StudentInstallment> GetStudentInstallmentPeriod(int schoolID, string period);
        IEnumerable<StudentInstallment> GetStudentInstallmentByCategory(int schoolID, int studentID, string period, int categoryID);
        IEnumerable<StudentInstallment> GetStudentInstallmentByPeriod(int schoolID, string period, int studentID);
        IEnumerable<StudentInstallment> GetStudentInstallmentNoIE(int schoolID, string period, int studentID, int installmentNo);

        StudentInstallment GetStudentInstallmentID(int schoolID, int studentInstallmentID);
        StudentInstallment GetStudentInstallmentNoAccountNo(int schoolID, string period, int studentInstallmentNo, int studentID, int accountReceiptNo);
        
        StudentInstallment GetStudentInstallmentNo(int schoolID, int studentInstallmentID, string period, int installmentNo);
        StudentInstallment GetStudentInstallment2(int schoolID, int studentID, string period, int studentInstallmentID);
        StudentInstallment GetStudentInstallment3(int schoolID, int studentID, string period, DateTime? installmentDate);

        bool CreateStudentInstallment(StudentInstallment studentInstallment);
        bool UpdateStudentInstallment(StudentInstallment studentInstallment);
        bool DeleteStudentInstallment(StudentInstallment studentInstallment);
        bool ExistStudentInstallment(int studentID);
        bool ExistStudentInstallment2(int schoolID, string period, int studentID, int categoryID);
       
        bool Save();
    }
}
