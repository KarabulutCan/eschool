using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentTaskDataSourceRepository : IStudentTaskDataSourceRepository
    {
        private SchoolDbContext _studentTaskDataSourceContext;
        public StudentTaskDataSourceRepository(SchoolDbContext studentTaskDataSourceContext)
        {
            _studentTaskDataSourceContext = studentTaskDataSourceContext;
        }

        IEnumerable<StudentTaskDataSource> IStudentTaskDataSourceRepository.GetStudentTaskAll(int schoolID, int studentID, int taskTypeID)
        {
            return _studentTaskDataSourceContext.StudentTaskDataSource.OrderBy(c => c.Start).Where(c => c.SchoolID == schoolID && c.StudentID == studentID && c.TaskTypeID == taskTypeID).ToList();
        }
        IEnumerable<StudentTaskDataSource> IStudentTaskDataSourceRepository.GetStudentTaskAll2(int schoolID, int studentID)
        {
            return _studentTaskDataSourceContext.StudentTaskDataSource.OrderBy(c => c.Start).Where(c => c.SchoolID == schoolID && c.StudentID == studentID).ToList();
        }
        public StudentTaskDataSource GetStudentTask(int schoolID, int studentID, int taskID, int taskTypeID)
        {
            return _studentTaskDataSourceContext.StudentTaskDataSource.Where(c => c.SchoolID == schoolID && c.StudentID == studentID && c.TaskID == taskID && c.TaskTypeID == taskTypeID).FirstOrDefault();
        }
       
        public bool CreateStudentTask(StudentTaskDataSource studentTaskDataSource)
        {
            _studentTaskDataSourceContext.Add(studentTaskDataSource);
            return Save();
        }

        public bool UpdateStudentTask(StudentTaskDataSource studentTaskDataSource)
        {
            _studentTaskDataSourceContext.Update(studentTaskDataSource);
            return Save();
        }

        public bool DeleteStudentTask(StudentTaskDataSource studentTaskDataSource)
        {
            _studentTaskDataSourceContext.Remove(studentTaskDataSource);
            return Save();
        }

        public bool ExistStudentTask(int schoolID, int studentID, int taskID, int taskTypeID)
        {
            return _studentTaskDataSourceContext.StudentTaskDataSource.Any(c => c.SchoolID == schoolID && c.StudentID == studentID && c.TaskID == taskID && c.TaskTypeID == taskTypeID);
        }
        public bool Save()
        {
            var saved = _studentTaskDataSourceContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
