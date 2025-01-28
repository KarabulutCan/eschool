using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class AbsenteeismRepository : IAbsenteeismRepository
    {
        private SchoolDbContext _absenteeismContext;
        public AbsenteeismRepository(SchoolDbContext absenteeismContext)
        {
            _absenteeismContext = absenteeismContext;
        }

        IEnumerable<Absenteeism> IAbsenteeismRepository.GetAbsenteeismAll(int schoolID, string  period)
        {
            return _absenteeismContext.Absenteeism.Where(b => b.SchoolID == schoolID && b.Period == period).OrderByDescending(b=> b.Date.Value.Month).ToList();
        }
        IEnumerable<Absenteeism> IAbsenteeismRepository.GetAbsenteeismTotalStudent(int schoolID, string period, int studentID)
        {
            return _absenteeismContext.Absenteeism.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID).ToList();
        }

        public Absenteeism GetAbsenteeismStudent(int schoolID, string period, int studentID, int year, int month)
        {
            return _absenteeismContext.Absenteeism.Where(b => b.SchoolID == schoolID && b.Period == period && b.StudentID == studentID && b.Date.Value.Year == year && b.Date.Value.Month == month).FirstOrDefault();
        }


        public Absenteeism GetAbsenteeism(int absenteeismID)
        {
            return _absenteeismContext.Absenteeism.Where(b => b.AbsenteeismID == absenteeismID).FirstOrDefault();
        }
        public bool CreateAbsenteeism(Absenteeism absenteeism)
        {
            _absenteeismContext.Add(absenteeism);
            return Save();
        }

        public bool UpdateAbsenteeism(Absenteeism absenteeism)
        {
            _absenteeismContext.Update(absenteeism);
            return Save();
        }

        public bool DeleteAbsenteeism(Absenteeism absenteeism)
        {
            _absenteeismContext.Remove(absenteeism);
            return Save();
        }

        public bool ExistAbsenteeism(int absenteeismID)
        {
            return _absenteeismContext.Absenteeism.Any(c => c.AbsenteeismID == absenteeismID);
        }
        public bool Save()
        {
            var saved = _absenteeismContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
