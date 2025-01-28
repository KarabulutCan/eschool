using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetStudentAllPeriod(int schoolID);
        IEnumerable<Student> GetStudentAllWithClassroom(int schoolID);
        IEnumerable<Student> GetStudentAllWithClassroomCount(int schoolID, int classsroomID);
        IEnumerable<Student> GetStudentAllWithClassroomTrueFalse(int schoolID);
        IEnumerable<Student> GetOldStudent(int schoolID);

        IEnumerable<Student> GetStudentClassroom(int schoolID, int classroomID);
        IEnumerable<Student> GetStudentSibling(int schoolID);
        IEnumerable<Student> GetStudentSibling1(int schoolID, int studentID);
        IEnumerable<Student> GetStudentSibling2(int schoolID, int studentID);
        IEnumerable<Student> GetStudentStatusCategory(int schoolID, int categoryID);
        IEnumerable<Student> GetStudentRegisterCategory(int schoolID, int categoryID);

        Student GetStudent(int studentID);
        Student GetStudent2(int schoolID, int studentID);
        Student GetStudentIdNumber(string idNumber);

        bool CreateStudent(Student student);
        bool UpdateStudent(Student student);
        bool DeleteStudent(Student student);
        bool ExistClassroom(int classroomID);
        bool ExistStudent(int studentID);
        bool ExistStudentIdNumber(string idNumber);
        bool Save();
    }
}
