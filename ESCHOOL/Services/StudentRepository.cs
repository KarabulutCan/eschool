using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentRepository : IStudentRepository
    {
        private SchoolDbContext _studentContext;
        public StudentRepository(SchoolDbContext studentContext)
        {
            _studentContext = studentContext;
        }
        public Student GetStudent(int studentID)
        {
            return _studentContext.Student.Where(b => b.StudentID == studentID).FirstOrDefault();
        }
        public Student GetStudent2(int schoolID, int studentID)
        {
            return _studentContext.Student.Where(b => b.SchoolID == schoolID && b.StudentID == studentID).FirstOrDefault();
        }

        public Student GetStudentIdNumber(string idNumber)
        {
            return _studentContext.Student.Where(b => b.IdNumber == idNumber).FirstOrDefault();
        }

        public IEnumerable<Student> GetStudentAllPeriod(int schoolID)
        {
            return _studentContext.Student.OrderByDescending(c => c.StudentID).Where(b => b.SchoolID == schoolID && b.IsActive == true).ToList();
        }
        public IEnumerable<Student> GetStudentAllWithClassroom(int schoolID)
        {
            return _studentContext.Student.OrderByDescending(c => c.StudentID).Where(b => b.SchoolID == schoolID && b.IsActive == true && b.ClassroomID != 0).ToList();
        }
        public IEnumerable<Student> GetStudentAllWithClassroomCount(int schoolID, int classroomID)
        {
            return _studentContext.Student.OrderByDescending(c => c.StudentID).Where(b => b.SchoolID == schoolID && b.IsActive == true && b.ClassroomID == classroomID).ToList();
        }
        public IEnumerable<Student> GetStudentAllWithClassroomTrueFalse(int schoolID)
        {
            return _studentContext.Student.OrderByDescending(c => c.StudentID).Where(b => b.SchoolID == schoolID && b.ClassroomID != 0).ToList();
        }

        public IEnumerable<Student> GetOldStudent(int schoolID)
        {
            return _studentContext.Student.OrderByDescending(c => c.StudentID).Where(b => b.SchoolID == schoolID && b.IsActive == true & b.ClassroomID == 0).ToList();
        }

        public IEnumerable<Student> GetStudentClassroom(int schoolID, int classroomID)
        {
            return _studentContext.Student.Where(b => b.SchoolID == schoolID && b.ClassroomID == classroomID).ToList();
        }

        public IEnumerable<Student> GetStudentSibling(int schoolID)
        {
            return _studentContext.Student.OrderByDescending(c => c.SiblingID).Where(n => n.SiblingID > 0 && n.StudentID == n.SiblingID && n.IsActive == true).ToList();
        }
        public IEnumerable<Student> GetStudentSibling1(int schoolID, int studentID)
        {
            return _studentContext.Student.OrderByDescending(c => c.SiblingID).Where(n => n.SiblingID > 0 && studentID == n.SiblingID && n.IsActive == true).ToList();
        }
        public IEnumerable<Student> GetStudentSibling2(int schoolID, int studentID)
        {
            return _studentContext.Student.OrderByDescending(c => c.SiblingID).Where(n => n.SchoolID == schoolID && n.StudentID != studentID && n.IsActive == true).ToList();
        }

        public IEnumerable<Student> GetStudentStatusCategory(int schoolID, int categoryID)
        {
            return _studentContext.Student.Where(c => c.SchoolID == schoolID && c.StatuCategoryID == categoryID && c.ClassroomID != 0).ToList();
            //return _studentContext.Student.Where(c => c.SchoolID == schoolID && c.StatuCategoryID == categoryID ).ToList();
        }
        public IEnumerable<Student> GetStudentRegisterCategory(int schoolID, int categoryID)
        {
            return _studentContext.Student.Where(c => c.SchoolID == schoolID && c.RegistrationTypeCategoryID == categoryID && c.ClassroomID != 0).ToList();
            //return _studentContext.Student.Where(c => c.SchoolID == schoolID && c.RegistrationTypeCategoryID == categoryID).ToList();
        }
        public bool CreateStudent(Student student)
        {
            _studentContext.Add(student);
            return Save();
        }
        public bool UpdateStudent(Student student)
        {
            _studentContext.Update(student);
            return Save();
        }
        public bool DeleteStudent(Student student)
        {
            _studentContext.Remove(student);
            return Save();
        }
        public bool ExistStudent(int studentID)
        {
            return _studentContext.Student.Any(c => c.StudentID == studentID);
        }
        public bool ExistClassroom(int classroomID)
        {
            return _studentContext.Student.Any(c => c.ClassroomID == classroomID);
        }
        public bool ExistStudentIdNumber(string idNumber)
        {
            return _studentContext.Student.Any(c => c.IdNumber == idNumber);
        }
        public bool Save()
        {
            var saved = _studentContext.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}
