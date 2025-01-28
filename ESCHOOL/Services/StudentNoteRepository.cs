using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class StudentNoteRepository : IStudentNoteRepository
    {
        private SchoolDbContext _studentNoteContext;
        public StudentNoteRepository(SchoolDbContext studentNoteContext)
        {
            _studentNoteContext = studentNoteContext;
        }
        public IEnumerable<StudentNote> GetStudentNoteAll()
        {
            return _studentNoteContext.StudentNote.ToList();
        }

        public StudentNote GetStudentNote(int studentID)
        {
            return _studentNoteContext.StudentNote.Where(b => b.StudentID == studentID).FirstOrDefault();
        }

        public bool CreateStudentNote(StudentNote studentNote)
        {
            _studentNoteContext.Add(studentNote);
            return Save();
        }
        public bool UpdateStudentNote(StudentNote studentNote)
        {
            _studentNoteContext.Update(studentNote);
            return Save();
        }
        public bool DeleteStudentNote(StudentNote studentNote)
        {
            _studentNoteContext.Remove(studentNote);
            return Save();
        }

        public bool Save()
        {
            var saved = _studentNoteContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
