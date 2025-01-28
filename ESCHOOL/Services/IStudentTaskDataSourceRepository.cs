using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentTaskDataSourceRepository
    {
        IEnumerable<StudentTaskDataSource> GetStudentTaskAll(int schoolID, int studentID, int taskTypeID);
        IEnumerable<StudentTaskDataSource> GetStudentTaskAll2(int schoolID, int studentID);
        StudentTaskDataSource GetStudentTask(int schoolID, int studentID, int taskID, int taskTypeID);

        bool CreateStudentTask(StudentTaskDataSource studentTaskDataSource);
        bool UpdateStudentTask(StudentTaskDataSource studentTaskDataSource);
        bool DeleteStudentTask(StudentTaskDataSource studentTaskDataSource);
        bool ExistStudentTask(int schoolID, int studentID, int taskID, int taskTypeID);

        bool Save();
    }
}
