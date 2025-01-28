using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentDiscountRepository : IStudentDiscountRepository
    {
        private SchoolDbContext _studentDiscountContext;
        public StudentDiscountRepository(SchoolDbContext studentDiscountContext)
        {
            _studentDiscountContext = studentDiscountContext;
        }

        IEnumerable<StudentDiscount> IStudentDiscountRepository.GetDiscountAll()
        {

            return _studentDiscountContext.StudentDiscount.ToList();
        }
        IEnumerable<StudentDiscount> IStudentDiscountRepository.GetDiscountID(int studentDebtID)
        {

            return _studentDiscountContext.StudentDiscount.Where(c => c.StudentDebtID == studentDebtID).ToList();
        }

        IEnumerable<StudentDiscount> IStudentDiscountRepository.GetDiscount2(int schoolID, string period, int studentID)
        {
            return _studentDiscountContext.StudentDiscount.Where(c => c.SchoolID == schoolID && c.Period == period && c.StudentID == studentID ).ToList();
        }

        IEnumerable<StudentDiscount> IStudentDiscountRepository.GetDiscount4(int ID, string period, int? schoolID, int discountTableID)
        {

            return _studentDiscountContext.StudentDiscount.Where(c => c.StudentID == ID && c.Period == period && c.SchoolID == schoolID && c.DiscountTableID == discountTableID).ToList();
        }
        public StudentDiscount GetDiscount(int ID, string period, int? schoolID, int discountTableID, int studentDebtID)
        {
            return _studentDiscountContext.StudentDiscount.Where(c => c.StudentID == ID && c.Period == period && c.SchoolID == schoolID && c.DiscountTableID == discountTableID && c.StudentDebtID == studentDebtID).FirstOrDefault();
        }

        public bool CreateStudentDiscount(StudentDiscount studentDiscount)
        {
            _studentDiscountContext.Add(studentDiscount);
            return Save();
        }

        public bool UpdateStudentDiscount(StudentDiscount studentDiscount)
        {
            _studentDiscountContext.Update(studentDiscount);
            return Save();
        }
        public bool DeleteStudentDiscount(StudentDiscount studentDiscount)
        {
            _studentDiscountContext.Remove(studentDiscount);
            return Save();
        }

        public bool Save()
        {
            var saved = _studentDiscountContext.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}
