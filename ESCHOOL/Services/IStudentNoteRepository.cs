using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IStudentNoteRepository
    {

        StudentNote GetStudentNote(int studentID);

        IEnumerable<StudentNote> GetStudentNoteAll();
        bool CreateStudentNote(StudentNote studentNote);
        bool UpdateStudentNote(StudentNote studentNote);
        bool DeleteStudentNote(StudentNote studentNote);

        bool Save();
    }
}
