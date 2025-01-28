using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IClassroomRepository
    {
        IEnumerable<Classroom> GetClassroomPeriods(int schoolID, string period);
        IEnumerable<Classroom> GetClassroom(string period);
        Classroom GetClassroomIDPeriod(string period, int classroomID);
        Classroom GetClassroomID(int classroomID);

        Classroom GetClassroomNamePeriod(string period, string classroomName);
        bool CreateClassroom(Classroom classroom);
        bool UpdateClassroom(Classroom classroom);
        bool DeleteClassroom(Classroom classroom);
        bool ExistClassroomName(string period, string classroomNameD);

        bool Save();
    }
}
