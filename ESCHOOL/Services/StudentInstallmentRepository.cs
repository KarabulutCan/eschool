using ESCHOOL.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentInstallmentRepository : IStudentInstallmentRepository
    {
        private SchoolDbContext _studentInstallmentContext;
        public StudentInstallmentRepository(SchoolDbContext studentInstallmentContext)
        {
            _studentInstallmentContext = studentInstallmentContext;
        }
        public IEnumerable<StudentInstallment> GetStudentInstallmentAll(int schoolID, string period, int categoryID)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.Period == period && b.CategoryID == categoryID).ToList();
        }
        public IEnumerable<StudentInstallment> GetStudentInstallmentPeriod(int schoolID, string period)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.Period == period).ToList();
        }
        public IEnumerable<StudentInstallment> GetStudentInstallmentByCategory(int schoolID, int studentID, string period, int categoryID)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period && b.CategoryID == categoryID).ToList();
        }
        public IEnumerable<StudentInstallment> GetStudentInstallmentByPeriod(int schoolID, string period, int studentID)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID &&b.Period == period && b.StudentID == studentID).ToList();
        }
        public IEnumerable<StudentInstallment> GetStudentInstallment(int schoolID, int studentID, string period)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.StudentID == studentID && b.Period == period).ToList();
        }
        
        public IEnumerable<StudentInstallment> GetStudentInstallmentNoIE(int schoolID, string period, int studentID, int installmentNo)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID && b.InstallmentNo == installmentNo).ToList();
        }
        public StudentInstallment GetStudentInstallmentID(int schoolID, int studentInstallmentID)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.StudentInstallmentID == studentInstallmentID).SingleOrDefault();
        }
        public StudentInstallment GetStudentInstallmentNoAccountNo(int schoolID, string period, int studentInstallmentNo, int studentID, int accountReceiptNo)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.SchoolID == schoolID && b.Period == period && b.InstallmentNo == studentInstallmentNo && b.StudentID == studentID && b.AccountReceiptNo == accountReceiptNo).SingleOrDefault();
        }
        public StudentInstallment GetStudentInstallmentNo(int studentID, int studentInstallmentID, string period,  int installmentNo)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.StudentID == studentID && b.StudentInstallmentID == studentInstallmentID && b.Period == period && b.InstallmentNo == installmentNo).SingleOrDefault();
        }

        public StudentInstallment GetStudentInstallment2(int schoolID, int studentID, string period, int studentInstallmentID)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.StudentID == studentID && b.Period == period && b.StudentInstallmentID == studentInstallmentID).SingleOrDefault();
        }

        public StudentInstallment GetStudentInstallment3(int schoolID, int studentID, string period, DateTime? installmentDate)
        {
            return _studentInstallmentContext.StudentInstallment.Where(b => b.StudentID == studentID && b.Period == period && b.InstallmentDate == installmentDate).SingleOrDefault();
        }


        public bool CreateStudentInstallment(StudentInstallment studentInstallment)
        {
            _studentInstallmentContext.Add(studentInstallment);
            return Save();
        }
        public bool UpdateStudentInstallment(StudentInstallment studentInstallment)
        {
            _studentInstallmentContext.Update(studentInstallment);
            return Save();
        }
        public bool DeleteStudentInstallment(StudentInstallment studentInstallment)
        {
            _studentInstallmentContext.Remove(studentInstallment);
            return Save();
        }

        public bool ExistStudentInstallment(int studentID)
        {
            return _studentInstallmentContext.StudentInstallment.Any(c => c.StudentID == studentID);
        }

        public bool ExistStudentInstallment2(int schoolID, string period, int studentID, int categoryID)
        {
            return _studentInstallmentContext.StudentInstallment.Any(c => c.SchoolID == schoolID && c.Period == period && c.StudentID == studentID && c.CategoryID == categoryID);
        }


        public bool Save()
        {
            var saved = _studentInstallmentContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
