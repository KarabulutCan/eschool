using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentPaymentRepository : IStudentPaymentRepository
    {
        private SchoolDbContext _studentPaymentContext;
        public StudentPaymentRepository(SchoolDbContext studentPaymentContext)
        {
            _studentPaymentContext = studentPaymentContext;
        }

        public IEnumerable<StudentPayment> GetStudentPaymentAll(int schoolID, string period)
        {
            return _studentPaymentContext.StudentPayment.Where(b => b.SchoolID == schoolID && b.Period == period).ToList();
        }
        public IEnumerable<StudentPayment> GetStudentPayment(int schoolID, string period, int studentID)
        {
            return _studentPaymentContext.StudentPayment.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).ToList();
        }
        public IEnumerable<StudentPayment> GetPaymentOrder(int schoolID, string period, int studentID)
        {
            return _studentPaymentContext.StudentPayment.OrderByDescending(c => c.AccountReceipt).Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).ToList();
        }

        public IEnumerable<StudentPayment> GetStudentPaymentIE(int schoolID, int studentID, string period, string installmentNo)
        {
            return _studentPaymentContext.StudentPayment.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period && b.InstallmentNumbers == installmentNo).ToList();
        }

        public StudentPayment GetStudentAccountReceiptBySchool(int schoolID, DateTime paymentDate, int studentID, string period, int accountReceipt)
        {
            return _studentPaymentContext.StudentPayment.Where(b => b.SchoolID == schoolID && b.PaymentDate == paymentDate && b.StudentID == studentID && b.Period == period && b.AccountReceipt == accountReceipt).SingleOrDefault();
        }

        public StudentPayment GetStudentPaymentID(int studentPaymentID)
        {
            return _studentPaymentContext.StudentPayment.Where(b => b.StudentPaymentID == studentPaymentID).SingleOrDefault();
        }
        public StudentPayment GetStudentAccountReceipt(int schoolID, string period, int studentID, int accountReceipt)
        {
            return _studentPaymentContext.StudentPayment.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID && b.AccountReceipt == accountReceipt).SingleOrDefault();
        }

        public bool CreateStudentPayment(StudentPayment studentPayment)
        {
            _studentPaymentContext.Add(studentPayment);
            return Save();
        }
        public bool UpdateStudentPayment(StudentPayment studentPayment)
        {
            _studentPaymentContext.Update(studentPayment);
            return Save();
        }
        public bool DeleteStudentPayment(StudentPayment studentPayment)
        {
            _studentPaymentContext.Remove(studentPayment);
            return Save();
        }

        public bool ExistStudentPayment(int studentPaymentID)
        {
            return _studentPaymentContext.StudentPayment.Any(c => c.StudentPaymentID == studentPaymentID);
        }

        public bool Save()
        {
            var saved = _studentPaymentContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }

}
