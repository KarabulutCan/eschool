using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ISchoolsTaskDataSourceRepository
    {
        IEnumerable<SchoolsTaskDataSource> GetSchoolsTaskAll(int schoolID, int userID, int taskTypeID, int classroomID);
        SchoolsTaskDataSource GetSchoolsTask(int schoolID, int userID, int taskTypeID, int taskID, int classroomID);
        bool CreateSchoolsTask(SchoolsTaskDataSource SchoolsTaskDataSource);
        bool UpdateSchoolsTask(SchoolsTaskDataSource SchoolsTaskDataSource);
        bool DeleteSchoolsTask(SchoolsTaskDataSource SchoolsTaskDataSource);
        bool ExistSchoolsTask(int schoolID, int userID, int taskTypeID, int taskID, int classroomID);

        bool Save();
    }
}
