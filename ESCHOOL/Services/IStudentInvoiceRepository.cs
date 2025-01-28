using ESCHOOL.Models;
using System;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentInvoiceRepository
    {
        IEnumerable<StudentInvoice> GetInvoiceAll(int schoolID, string period);
        IEnumerable<StudentInvoice> GetStudentInvoiceAll(int schoolID, string period, int studentID);
        IEnumerable<StudentInvoice> GetStudentInvoice(string period, int schoolID, int studentID);
        IEnumerable<StudentInvoice> GetStudentInvoiceFalse(string period, int schoolID, int studentID);
        IEnumerable<StudentInvoice> GetStudentInvoiceTrue(string period, int schoolID, int studentID);
        IEnumerable<StudentInvoice> GetStudentInvoiceAllTrue(string period, int schoolID);
        IEnumerable<StudentInvoice> GetStudentInvoiceAllFalse(string period, int schoolID, DateTime date);
        IEnumerable<StudentInvoice> GetStudentInvoiceUpdate(int schoolID, int studentID);
        
        StudentInvoice GetStudentInvoiceAddressID(string period, int schoolID, int studentInvoiceID, int studentInvoiceAddressID);
        StudentInvoice GetStudentInvoiceID(string period, int schoolID, int studentInvoiceID);
        StudentInvoice GetStudentInvoiceControl(string period, int schoolID, int studentID, DateTime date);
        StudentInvoice GetStudentInvoiceSerialNo(string period, int schoolID, int invoiceSerialNo);
        bool CreateStudentInvoice(StudentInvoice studentInvoice);
        bool UpdateStudentInvoice(StudentInvoice studentInvoice);
        bool DeleteStudentInvoice(StudentInvoice studentInvoice);

        bool Save();
    }
}
