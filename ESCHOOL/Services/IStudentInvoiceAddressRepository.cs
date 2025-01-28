using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentInvoiceAddressRepository
    {
        IEnumerable<StudentInvoiceAddress> GetStudentInvoiceAddressAll();
        StudentInvoiceAddress GetStudentInvoiceAddress(int studentInvoiceAddressID);
        StudentInvoiceAddress GetStudentInvoiceAddressID(int studentID);
        StudentInvoiceAddress GetStudentInvoiceAddressTitle(string invoiceTitle, string invoiceTaxNumber);
        bool CreateStudentInvoiceAddress(StudentInvoiceAddress studentInvoiceAddress);
        bool UpdateStudentInvoiceAddress(StudentInvoiceAddress studentInvoiceAddress);
        bool DeleteStudentInvoiceAddress(StudentInvoiceAddress studentInvoiceAddress);
        bool ExistStudentInvoiceAddress(int studentInvoiceAdressID);

        bool Save();
    }
}
