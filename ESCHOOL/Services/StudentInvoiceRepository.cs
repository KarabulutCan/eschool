using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentInvoiceRepository : IStudentInvoiceRepository
    {
        private SchoolDbContext _studentInvoiceContext;
        public StudentInvoiceRepository(SchoolDbContext studentInvoiceContext)
        {
            _studentInvoiceContext = studentInvoiceContext;
        }

        public IEnumerable<StudentInvoice> GetInvoiceAll(int schoolID, string period)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID).ToList();
        }
        public IEnumerable<StudentInvoice> GetStudentInvoiceAll(int schoolID, string period, int studentID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID).ToList(); 
        }

        public IEnumerable<StudentInvoice> GetStudentInvoice(string period, int schoolID, int studentID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID && b.StudentID > 0);
        }

        public IEnumerable<StudentInvoice> GetStudentInvoiceFalse(string period, int schoolID, int studentID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID && b.InvoiceStatus == false);
        }
        public IEnumerable<StudentInvoice> GetStudentInvoiceTrue(string period, int schoolID, int studentID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID && b.InvoiceStatus == true);
        }
        public IEnumerable<StudentInvoice> GetStudentInvoiceAllTrue(string period, int schoolID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.InvoiceStatus == true).ToList();
        }
        public IEnumerable<StudentInvoice> GetStudentInvoiceAllFalse(string period, int schoolID, DateTime date)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.InvoiceStatus == false && b.InvoiceDate == date).ToList();
        }
        public IEnumerable<StudentInvoice> GetStudentInvoiceUpdate(int schoolID, int studentID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.SchoolID == schoolID && b.StudentID == studentID);
        }
        public StudentInvoice GetStudentInvoiceID(string period, int schoolID, int studentInvoiceID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentInvoiceID == studentInvoiceID).FirstOrDefault();
        }
        public StudentInvoice GetStudentInvoiceAddressID(string period, int schoolID, int studentInvoiceID, int studentInvoiceAddressID)
        {
            return _studentInvoiceContext.StudentInvoice.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentInvoiceID == studentInvoiceID && b.StudentInvoiceAddressID == studentInvoiceAddressID).FirstOrDefault();

        }
        public StudentInvoice GetStudentInvoiceControl(string period, int schoolID, int studentID, DateTime date)
        {
            return _studentInvoiceContext.StudentInvoice.Where(c => c.Period == period && c.SchoolID == schoolID && c.StudentID == studentID && c.InvoiceDate == date).FirstOrDefault();
        }
        public StudentInvoice GetStudentInvoiceSerialNo(string period, int schoolID, int invoiceSerialNo)
        {
            return _studentInvoiceContext.StudentInvoice.Where(c => c.Period == period && c.SchoolID == schoolID && c.InvoiceSerialNo == invoiceSerialNo).FirstOrDefault();
        }
        public bool CreateStudentInvoice(StudentInvoice studentInvoice)
        {
            _studentInvoiceContext.Add(studentInvoice);
            return Save();
        }
        public bool UpdateStudentInvoice(StudentInvoice studentInvoice)
        {
            _studentInvoiceContext.Update(studentInvoice);
            return Save();
        }
        public bool DeleteStudentInvoice(StudentInvoice studentInvoice)
        {
            _studentInvoiceContext.Remove(studentInvoice);
            return Save();
        }


        public bool Save()
        {
            var saved = _studentInvoiceContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
