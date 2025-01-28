using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentInvoiceDetailRepository : IStudentInvoiceDetailRepository
    {
        private SchoolDbContext _studentInvoiceDetailContext;
        public StudentInvoiceDetailRepository(SchoolDbContext studentInvoiceDetailContext)
        {
            _studentInvoiceDetailContext = studentInvoiceDetailContext;
        }

        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail(int studentID, int studentInvoiceID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.StudentID == studentID && b.StudentInvoiceID == studentInvoiceID).ToList();
        }
        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail1(string period, int schoolID, int studentID, int feeID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID && b.SchoolFeeID == feeID).ToList();
        }
        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail2(string period, int schoolID, int studentID, int feeID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.Period == period && b.SchoolID == schoolID && b.StudentID == studentID && b.SchoolFeeID == feeID && b.InvoiceStatus == true);
        }
        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetail3(int schoolID, string period, int studentID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.SchoolID == schoolID && b.Period == period &&  b.StudentID == studentID).ToList();
        }
        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceID(int studentInvoiceID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.StudentInvoiceID == studentInvoiceID).ToList();
        }

        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceDetailAll()
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.OrderBy(c => c.StudentInvoiceDetailID);
        }
        public IEnumerable<StudentInvoiceDetail> GetStudentInvoiceSerialNo(int invoiceSerialNo)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.InvoiceSerialNo == invoiceSerialNo).ToList();
        }

        public StudentInvoiceDetail GetStudentInvoiceDetailIDSingle(int studentInvoiceDetailID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Where(b => b.StudentInvoiceDetailID == studentInvoiceDetailID).FirstOrDefault(); ;
        }
        public bool CreateStudentInvoiceDetail(StudentInvoiceDetail studentInvoiceDetail)
        {
            _studentInvoiceDetailContext.Add(studentInvoiceDetail);
            return Save();
        }
        public bool UpdateStudentInvoiceDetail(StudentInvoiceDetail studentInvoiceDetail)
        {
            _studentInvoiceDetailContext.Update(studentInvoiceDetail);
            return Save();
        }
        public bool DeleteStudentInvoiceDetail(StudentInvoiceDetail studentInvoiceDetail)
        {
            _studentInvoiceDetailContext.Remove(studentInvoiceDetail);
            return Save();
        }

        public bool ExistStudentInvoiceDetail(int studentInvoiceDetailID)
        {
            return _studentInvoiceDetailContext.StudentInvoiceDetail.Any(c => c.StudentInvoiceDetailID == studentInvoiceDetailID);
        }

        public bool Save()
        {
            var saved = _studentInvoiceDetailContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
