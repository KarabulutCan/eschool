using ESCHOOL.Models;
using System;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IAbsenteeismRepository
    {
        IEnumerable<Absenteeism> GetAbsenteeismAll(int schoolID, string period);

        IEnumerable<Absenteeism> GetAbsenteeismTotalStudent(int schoolID, string period, int studentID);
        Absenteeism GetAbsenteeismStudent(int schoolID, string period, int studentID, int year, int month);
        Absenteeism GetAbsenteeism(int absenteeismID);

        bool CreateAbsenteeism(Absenteeism absenteeism);
        bool UpdateAbsenteeism(Absenteeism absenteeism);
        bool DeleteAbsenteeism(Absenteeism absenteeism);
        bool ExistAbsenteeism(int absenteeismID);

        bool Save();
    }
}
