using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class SchoolsTaskDataSourceRepository : ISchoolsTaskDataSourceRepository
    {
        private SchoolDbContext _schoolsTaskDataSourceContext;
        public SchoolsTaskDataSourceRepository(SchoolDbContext schoolsTaskDataSourceContext)
        {
            _schoolsTaskDataSourceContext = schoolsTaskDataSourceContext;
        }

        IEnumerable<SchoolsTaskDataSource> ISchoolsTaskDataSourceRepository.GetSchoolsTaskAll(int schoolID, int userID, int taskTypeID, int classroomID)
        {
            return _schoolsTaskDataSourceContext.SchoolsTaskDataSource.OrderBy(c => c.Start).Where(c => c.SchoolID == schoolID && c.UserID == userID && c.TaskTypeID == taskTypeID && c.ClassroomID == classroomID).ToList();
        }
        public SchoolsTaskDataSource GetSchoolsTask(int schoolID, int userID, int taskTypeID, int taskID, int classroomID)
        {
            return _schoolsTaskDataSourceContext.SchoolsTaskDataSource.Where(c => c.SchoolID == schoolID && c.UserID == userID && c.TaskTypeID == taskTypeID && c.TaskID == taskID && c.ClassroomID == classroomID).FirstOrDefault();
        }
       
        public bool CreateSchoolsTask(SchoolsTaskDataSource SchoolsTaskDataSource)
        {
            _schoolsTaskDataSourceContext.Add(SchoolsTaskDataSource);
            return Save();
        }

        public bool UpdateSchoolsTask(SchoolsTaskDataSource SchoolsTaskDataSource)
        {
            _schoolsTaskDataSourceContext.Update(SchoolsTaskDataSource);
            return Save();
        }

        public bool DeleteSchoolsTask(SchoolsTaskDataSource SchoolsTaskDataSource)
        {
            _schoolsTaskDataSourceContext.Remove(SchoolsTaskDataSource);
            return Save();
        }

        public bool ExistSchoolsTask(int schoolID, int UserID, int taskTypeID, int taskID, int classroomID)
        {
            return _schoolsTaskDataSourceContext.SchoolsTaskDataSource.Any(c => c.SchoolID == schoolID && c.UserID == UserID && c.TaskTypeID == taskTypeID && c.TaskID == taskID && c.ClassroomID == classroomID);
        }
        public bool Save()
        {
            var saved = _schoolsTaskDataSourceContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
