using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentInvoiceAddressRepository : IStudentInvoiceAddressRepository
    {
        private SchoolDbContext _studentInvoiceAddressContext;
        public StudentInvoiceAddressRepository(SchoolDbContext studentInvoiceAddressContext)
        {
            _studentInvoiceAddressContext = studentInvoiceAddressContext;
        }

        public StudentInvoiceAddress GetStudentInvoiceAddress(int studentInvoiceAddressID)
        {
            return _studentInvoiceAddressContext.StudentInvoiceAddress.Where(b => b.StudentInvoiceAddressID == studentInvoiceAddressID).FirstOrDefault();
        }
        public StudentInvoiceAddress GetStudentInvoiceAddressID(int studentID)
        {
            return _studentInvoiceAddressContext.StudentInvoiceAddress.Where(b => b.StudentID == studentID).FirstOrDefault();
        }

        public StudentInvoiceAddress GetStudentInvoiceAddressTitle(string invoiceTitle, string invoiceTaxNumber)
        {
            return _studentInvoiceAddressContext.StudentInvoiceAddress.Where(b => b.InvoiceTitle == invoiceTitle && b.InvoiceTaxNumber == invoiceTaxNumber).FirstOrDefault();
        }
        public IEnumerable<StudentInvoiceAddress> GetStudentInvoiceAddressAll()
        {
            return _studentInvoiceAddressContext.StudentInvoiceAddress.OrderBy(c => c.StudentInvoiceAddressID);
        }

        public bool CreateStudentInvoiceAddress(StudentInvoiceAddress studentInvoiceAddress)
        {
            _studentInvoiceAddressContext.Add(studentInvoiceAddress);
            return Save();
        }
        public bool UpdateStudentInvoiceAddress(StudentInvoiceAddress studentInvoiceAddress)
        {
            _studentInvoiceAddressContext.Update(studentInvoiceAddress);
            return Save();
        }
        public bool DeleteStudentInvoiceAddress(StudentInvoiceAddress studentInvoiceAddress)
        {
            _studentInvoiceAddressContext.Remove(studentInvoiceAddress);
            return Save();
        }

        public bool ExistStudentInvoiceAddress(int studentInvoiceAdressID)
        {
            return _studentInvoiceAddressContext.StudentInvoiceAddress.Any(c => c.StudentInvoiceAddressID == studentInvoiceAdressID);
        }

        public bool Save()
        {
            var saved = _studentInvoiceAddressContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
