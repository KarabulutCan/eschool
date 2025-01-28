using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class ClassroomRepository : IClassroomRepository
    {
        private SchoolDbContext _classroomContext;
        public ClassroomRepository(SchoolDbContext classroomContext)
        {
            _classroomContext = classroomContext;
        }

        IEnumerable<Classroom> IClassroomRepository.GetClassroomPeriods(int schoolID, string period)
        {
            return _classroomContext.Classroom.OrderBy(c => c.SortOrder).Where(c =>  c.SchoolID == schoolID && c.Period == period).ToList();
        }

        IEnumerable<Classroom> IClassroomRepository.GetClassroom(string period)
        {
            return _classroomContext.Classroom.OrderBy(c => c.SortOrder).Where(c => c.Period == period).ToList();
        }

        public Classroom GetClassroomIDPeriod(string period, int classroomID)
        {
            return _classroomContext.Classroom.Where(b => b.Period == period && b.ClassroomID == classroomID).FirstOrDefault();
        }
        public Classroom GetClassroomID(int classroomID)
        {
            return _classroomContext.Classroom.Where(b => b.ClassroomID == classroomID).FirstOrDefault();
        }
   
        public Classroom GetClassroomNamePeriod(string period, string classroomName)
        {
            return _classroomContext.Classroom.Where(b => b.Period == period && b.ClassroomName == classroomName).FirstOrDefault();
        }
        public bool CreateClassroom(Classroom classroom)
        {
            _classroomContext.Add(classroom);
            return Save();
        }

        public bool UpdateClassroom(Classroom classroom)
        {
            _classroomContext.Update(classroom);
            return Save();
        }

        public bool DeleteClassroom(Classroom classroom)
        {
            _classroomContext.Remove(classroom);
            return Save();
        }

        public bool ExistClassroomName(string period, string classroomName)
        {
            return _classroomContext.Classroom.Any(c => c.Period == period && c.ClassroomName == classroomName);
        }

        public bool Save()
        {
            var saved = _classroomContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
