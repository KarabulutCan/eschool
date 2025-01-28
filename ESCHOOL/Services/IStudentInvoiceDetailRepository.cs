using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentInvoiceDetailRepository
    {
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetailAll();
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail(int studentID, int studentInvoiceDetailID);
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail1(string period, int schoolID, int studentID, int feeID);
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail2(string period, int schoolID, int studentID, int feeID);
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail3(int schoolID, string period, int studentID);
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceID(int studentInvoiceDetailID);
        IEnumerable<StudentInvoiceDetail> GetStudentInvoiceSerialNo(int invoiceSerialNo);
        StudentInvoiceDetail GetStudentInvoiceDetailIDSingle(int studentInvoiceDetailID);

        bool CreateStudentInvoiceDetail(StudentInvoiceDetail studentInvoiceDetail);
        bool UpdateStudentInvoiceDetail(StudentInvoiceDetail studentInvoiceDetail);
        bool DeleteStudentInvoiceDetail(StudentInvoiceDetail studentInvoiceDetail);
        bool ExistStudentInvoiceDetail(int studentInvoiceDetailID);

        bool Save();
    }
}
