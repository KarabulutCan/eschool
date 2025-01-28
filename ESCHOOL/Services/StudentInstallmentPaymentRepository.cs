using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentInstallmentPaymentRepository : IStudentInstallmentPaymentRepository
    {
        private SchoolDbContext _studentInstallmentPaymentContext;

        public IEnumerable<StudentInstallmentPayment> GetStudentInstallmentPayment1(string period, int studentID)
        {
            return _studentInstallmentPaymentContext.StudentInstallmentPayment.Where(b => b.Period == period && b.StudentID == studentID).ToList();
        }
        public IEnumerable<StudentInstallmentPayment> GetStudentInstallmentPayment2(string period, int studentID, int studentPaymentID)
        {
            return _studentInstallmentPaymentContext.StudentInstallmentPayment.Where(b => b.Period == period && b.StudentID == studentID && b.StudentPaymentID == studentPaymentID).ToList();
        }
        public IEnumerable<StudentInstallmentPayment> GetStudentInstallmentPaymentByPeriod(int schoolID, string period, int studentID)
        {
            return _studentInstallmentPaymentContext.StudentInstallmentPayment.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID).ToList();
        }

        public StudentInstallmentPayment GetStudentInstallmentNo(string period, int studentID, int installmentID)
        {
            return _studentInstallmentPaymentContext.StudentInstallmentPayment.Where(b => b.Period == period && b.StudentID == studentID && b.StudentInstallmentID == installmentID).SingleOrDefault();
        }

        public StudentInstallmentPaymentRepository(SchoolDbContext studentInstallmentPaymentContext)
        {
            _studentInstallmentPaymentContext = studentInstallmentPaymentContext;
        }
        public bool CreateStudentInstallmentPayment(StudentInstallmentPayment studentInstallmentPayment)
        {
            _studentInstallmentPaymentContext.Add(studentInstallmentPayment);
            return Save();
        }
        public bool UpdateStudentInstallmentPayment(StudentInstallmentPayment studentInstallmentPayment)
        {
            _studentInstallmentPaymentContext.Update(studentInstallmentPayment);
            return Save();
        }
        public bool DeleteStudentInstallmentPayment(StudentInstallmentPayment studentInstallmentPayment)
        {
            _studentInstallmentPaymentContext.Remove(studentInstallmentPayment);
            return Save();
        }
        public bool Save()
        {
            var saved = _studentInstallmentPaymentContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
