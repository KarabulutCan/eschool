using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ESCHOOL.Controllers
{
    public class M950AbsenteeismController : Controller
    {
        IAbsenteeismRepository _absenteeismRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IClassroomRepository _classroomRepository;
        IUsersLogRepository _usersLogRepository;
        IUsersRepository _usersRepository;
        ISchoolInfoRepository _schoolInfoRepository;
        IParameterRepository _parameterRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IWebHostEnvironment _hostEnvironment;
        public M950AbsenteeismController(
            IAbsenteeismRepository absenteeismRepository,
            IStudentRepository studentRepository,
            IClassroomRepository classroomRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IUsersLogRepository usersLogRepository,
            IUsersRepository usersRepository,
            ISchoolInfoRepository schoolInfoRepository,
            IParameterRepository parameterRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            IWebHostEnvironment hostEnvironment)
        {
            _absenteeismRepository = absenteeismRepository;
            _studentRepository = studentRepository;
            _classroomRepository = classroomRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _usersLogRepository = usersLogRepository;
            _usersRepository = usersRepository;
            _schoolInfoRepository = schoolInfoRepository;
            _parameterRepository = parameterRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Denetim İzi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            int classroomInx = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod).First().ClassroomID;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            var absenteeismViewModel = new AbsenteeismViewModel
            {
                UserID = user.UserID,
                Period = user.UserPeriod,
                SchoolID = user.SchoolID,
                Date = DateTime.Now,
                ClassroomID = classroomInx,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(absenteeismViewModel);
        }
        public IActionResult Absenteeism(int userID, int classroomID, string dateString)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            DateTime date = DateTime.Parse(dateString);
            int prgyear = date.Year;
            int prgmonth = date.Month;
            int days = DateTime.DaysInMonth(prgyear, prgmonth);
            string hday01 = null; string hday02 = null; string hday03 = null; string hday04 = null; string hday05 = null; string hday06 = null; string hday07 = null; string hday08 = null; string hday09 = null; string hday10 = null;
            string hday11 = null; string hday12 = null; string hday13 = null; string hday14 = null; string hday15 = null; string hday16 = null; string hday17 = null; string hday18 = null; string hday19 = null; string hday20 = null;
            string hday21 = null; string hday22 = null; string hday23 = null; string hday24 = null; string hday25 = null; string hday26 = null; string hday27 = null; string hday28 = null; string hday29 = null; string hday30 = null; string hday31 = null;
            for (int i = 1; i < days + 1; i++)
            {
                DateTime newDate = new DateTime(prgyear, prgmonth, i);

                if (i == 01) hday01 = newDate.ToString("ddd, dd");
                if (i == 02) hday02 = newDate.ToString("ddd, dd");
                if (i == 03) hday03 = newDate.ToString("ddd, dd");
                if (i == 04) hday04 = newDate.ToString("ddd, dd");
                if (i == 05) hday05 = newDate.ToString("ddd, dd");
                if (i == 06) hday06 = newDate.ToString("ddd, dd");
                if (i == 07) hday07 = newDate.ToString("ddd, dd");
                if (i == 08) hday08 = newDate.ToString("ddd, dd");
                if (i == 09) hday09 = newDate.ToString("ddd, dd");
                if (i == 10) hday10 = newDate.ToString("ddd, dd");
                if (i == 11) hday11 = newDate.ToString("ddd, dd");
                if (i == 12) hday12 = newDate.ToString("ddd, dd");
                if (i == 13) hday13 = newDate.ToString("ddd, dd");
                if (i == 14) hday14 = newDate.ToString("ddd, dd");
                if (i == 15) hday15 = newDate.ToString("ddd, dd");
                if (i == 16) hday16 = newDate.ToString("ddd, dd");
                if (i == 17) hday17 = newDate.ToString("ddd, dd");
                if (i == 18) hday18 = newDate.ToString("ddd, dd");
                if (i == 19) hday19 = newDate.ToString("ddd, dd");
                if (i == 20) hday20 = newDate.ToString("ddd, dd");
                if (i == 21) hday21 = newDate.ToString("ddd, dd");
                if (i == 22) hday22 = newDate.ToString("ddd, dd");
                if (i == 23) hday23 = newDate.ToString("ddd, dd");
                if (i == 24) hday24 = newDate.ToString("ddd, dd");
                if (i == 25) hday25 = newDate.ToString("ddd, dd");
                if (i == 26) hday26 = newDate.ToString("ddd, dd");
                if (i == 27) hday27 = newDate.ToString("ddd, dd");
                if (i == 28) hday28 = newDate.ToString("ddd, dd");
                if (i == 29) hday29 = newDate.ToString("ddd, dd");
                if (i == 30) hday30 = newDate.ToString("ddd, dd");
                if (i == 31) hday31 = newDate.ToString("ddd, dd");
            }
            TempData["hday29IsTrue"] = true;
            TempData["hday30IsTrue"] = true;
            TempData["hday31IsTrue"] = true;
            if (hday29 == null) TempData["hday29IsTrue"] = false;
            else hday29.Replace('Ç', 'C');
            if (hday30 == null) TempData["hday30IsTrue"] = false;
            else hday30.Replace('Ç', 'C');
            if (hday31 == null) TempData["hday31IsTrue"] = false;
            else hday31.Replace('Ç', 'C');

            string monthTxt = date.ToString("MMMM' / 'yyyy", new CultureInfo(user.SelectedCulture.Trim()));

            string htotal1 = ESCHOOL.Resources.Resource.Excused;
            string htotal2 = ESCHOOL.Resources.Resource.Unexcused;

            DateTime? dateStart = school.SchoolYearStart;
            DateTime? dateEnd = school.SchoolYearEnd;

            var year = dateStart.Value.Year;
            var month = dateStart.Value.Month;
            string hmonth01 = null; string hmonth02 = null; string hmonth03 = null; string hmonth04 = null; string hmonth05 = null; string hmonth06 = null; string hmonth07 = null; string hmonth08 = null; string hmonth09 = null; string hmonth10 = null; string hmonth11 = null; string hmonth12 = null; ;
            var inx = 0;
            var absList = new AbsenteeismViewModel();
            {
                for (int i = 1; i < 13; i++)
                {
                    if (i >= month)
                    {
                        DateTime newDate = new DateTime(year, i, 1);
                        inx++;
                        if (inx == 01) hmonth01 = newDate.ToString("MMM, yyyy");
                        if (inx == 02) hmonth02 = newDate.ToString("MMM, yyyy");
                        if (inx == 03) hmonth03 = newDate.ToString("MMM, yyyy");
                        if (inx == 04) hmonth04 = newDate.ToString("MMM, yyyy");
                        if (inx == 05) hmonth05 = newDate.ToString("MMM, yyyy");
                        if (inx == 06) hmonth06 = newDate.ToString("MMM, yyyy");
                        if (inx == 07) hmonth07 = newDate.ToString("MMM, yyyy");
                        if (inx == 08) hmonth08 = newDate.ToString("MMM, yyyy");
                        if (inx == 09) hmonth09 = newDate.ToString("MMM, yyyy");
                        if (inx == 10) hmonth10 = newDate.ToString("MMM, yyyy");
                        if (inx == 11) hmonth11 = newDate.ToString("MMM, yyyy");
                        if (inx == 12) hmonth12 = newDate.ToString("MMM, yyyy");
                    }
                }

                year = dateEnd.Value.Year;
                month = dateEnd.Value.Month;
                for (int i = 1; i < 13; i++)
                {
                    if (i <= month)
                    {
                        DateTime newDate = new DateTime(year, i, 1);
                        inx++;
                        if (inx == 01) hmonth01 = newDate.ToString("MMM, yyyy");
                        if (inx == 02) hmonth02 = newDate.ToString("MMM, yyyy");
                        if (inx == 03) hmonth03 = newDate.ToString("MMM, yyyy");
                        if (inx == 04) hmonth04 = newDate.ToString("MMM, yyyy");
                        if (inx == 05) hmonth05 = newDate.ToString("MMM, yyyy");
                        if (inx == 06) hmonth06 = newDate.ToString("MMM, yyyy");
                        if (inx == 07) hmonth07 = newDate.ToString("MMM, yyyy");
                        if (inx == 08) hmonth08 = newDate.ToString("MMM, yyyy");
                        if (inx == 09) hmonth09 = newDate.ToString("MMM, yyyy");
                        if (inx == 10) hmonth10 = newDate.ToString("MMM, yyyy");
                        if (inx == 11) hmonth11 = newDate.ToString("MMM, yyyy");
                        if (inx == 12) hmonth12 = newDate.ToString("MMM, yyyy");
                    }
                }
                TempData["hmonth10IsTrue"] = true;
                TempData["hmonth11IsTrue"] = true;
                TempData["hmonth12IsTrue"] = true;
                if (hmonth10 == null) TempData["hmonth10IsTrue"] = false;
                else hmonth10.Replace('Ş', 'S');
                if (hmonth11 == null) TempData["hmonth11IsTrue"] = false;
                else hmonth11.Replace('Ş', 'S');
                if (hmonth12 == null) TempData["hmonth12IsTrue"] = false;
                else hmonth12.Replace('Ş', 'S');

                var absenteeismViewModel = new AbsenteeismViewModel
                {
                    UserID = user.UserID,
                    Period = user.UserPeriod,
                    SchoolID = user.SchoolID,
                    Date = date,

                    MonthText = monthTxt,

                    ClassroomID = classroomID,
                    ClassroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName,

                    HDay01 = hday01.Replace('Ç', 'C'),
                    HDay02 = hday02.Replace('Ç', 'C'),
                    HDay03 = hday03.Replace('Ç', 'C'),
                    HDay04 = hday04.Replace('Ç', 'C'),
                    HDay05 = hday05.Replace('Ç', 'C'),
                    HDay06 = hday06.Replace('Ç', 'C'),
                    HDay07 = hday07.Replace('Ç', 'C'),
                    HDay08 = hday08.Replace('Ç', 'C'),
                    HDay09 = hday09.Replace('Ç', 'C'),
                    HDay10 = hday10.Replace('Ç', 'C'),
                    HDay11 = hday11.Replace('Ç', 'C'),
                    HDay12 = hday12.Replace('Ç', 'C'),
                    HDay13 = hday13.Replace('Ç', 'C'),
                    HDay14 = hday14.Replace('Ç', 'C'),
                    HDay15 = hday15.Replace('Ç', 'C'),
                    HDay16 = hday16.Replace('Ç', 'C'),
                    HDay17 = hday17.Replace('Ç', 'C'),
                    HDay18 = hday18.Replace('Ç', 'C'),
                    HDay19 = hday19.Replace('Ç', 'C'),
                    HDay20 = hday20.Replace('Ç', 'C'),
                    HDay21 = hday21.Replace('Ç', 'C'),
                    HDay22 = hday22.Replace('Ç', 'C'),
                    HDay23 = hday23.Replace('Ç', 'C'),
                    HDay24 = hday24.Replace('Ç', 'C'),
                    HDay25 = hday25.Replace('Ç', 'C'),
                    HDay26 = hday26.Replace('Ç', 'C'),
                    HDay27 = hday27.Replace('Ç', 'C'),
                    HDay28 = hday28.Replace('Ç', 'C'),
                    HDay29 = hday29,
                    HDay30 = hday30,
                    HDay31 = hday31,

                    HTotal1 = htotal1,
                    HTotal2 = htotal2,

                    HMonth01 = hmonth01.Replace('Ş', 'S'),
                    HMonth02 = hmonth02.Replace('Ş', 'S'),
                    HMonth03 = hmonth03.Replace('Ş', 'S'),
                    HMonth04 = hmonth04.Replace('Ş', 'S'),
                    HMonth05 = hmonth05.Replace('Ş', 'S'),
                    HMonth06 = hmonth06.Replace('Ş', 'S'),
                    HMonth07 = hmonth07.Replace('Ş', 'S'),
                    HMonth08 = hmonth08.Replace('Ş', 'S'),
                    HMonth09 = hmonth09.Replace('Ş', 'S'),
                    HMonth10 = hmonth10.Replace('Ş', 'S'),
                    HMonth11 = hmonth11,
                    HMonth12 = hmonth12,

                    Char01 = school.Char01,
                    Char02 = school.Char02,
                    Char03 = school.Char03,
                    Char04 = school.Char04,
                    Char05 = school.Char05,
                    Char06 = school.Char06,
                    IsChar01 = (bool)school.IsChar01,
                    IsChar02 = (bool)school.IsChar02,
                    IsChar03 = (bool)school.IsChar03,
                    IsChar04 = (bool)school.IsChar04,
                    IsChar05 = (bool)school.IsChar05,
                    IsChar06 = (bool)school.IsChar06,
                    Char01Explanation = school.Char01Explanation,
                    Char02Explanation = school.Char02Explanation,
                    Char03Explanation = school.Char03Explanation,
                    Char04Explanation = school.Char04Explanation,
                    Char05Explanation = school.Char05Explanation,
                    Char06Explanation = school.Char06Explanation,

                    Char01Max = school.Char01Max,
                    Char02Max = school.Char02Max,
                    Char03Max = school.Char03Max,
                    Char04Max = school.Char04Max,
                    Char05Max = school.Char05Max,
                    Char06Max = school.Char06Max,

                    SelectedCulture = user.SelectedCulture.Trim()
                };
                return View(absenteeismViewModel);
            }
        }
        public IActionResult AbsenteeismSettings(AbsenteeismViewModel absenteeismViewModel)
        {
            var user = _usersRepository.GetUser(absenteeismViewModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(absenteeismViewModel.SchoolID);

            school.Char01 = absenteeismViewModel.Char01;
            school.Char02 = absenteeismViewModel.Char02;
            school.Char03 = absenteeismViewModel.Char03;
            school.Char04 = absenteeismViewModel.Char04;
            school.Char05 = absenteeismViewModel.Char05;
            school.Char06 = absenteeismViewModel.Char06;

            school.IsChar01 = absenteeismViewModel.IsChar01;
            school.IsChar02 = absenteeismViewModel.IsChar02;
            school.IsChar03 = absenteeismViewModel.IsChar03;
            school.IsChar04 = absenteeismViewModel.IsChar04;
            school.IsChar05 = absenteeismViewModel.IsChar05;
            school.IsChar06 = absenteeismViewModel.IsChar06;

            school.Char01Explanation = absenteeismViewModel.Char01Explanation;
            school.Char02Explanation = absenteeismViewModel.Char02Explanation;
            school.Char03Explanation = absenteeismViewModel.Char03Explanation;
            school.Char04Explanation = absenteeismViewModel.Char04Explanation;
            school.Char05Explanation = absenteeismViewModel.Char05Explanation;
            school.Char06Explanation = absenteeismViewModel.Char06Explanation;

            school.Char01Max = absenteeismViewModel.Char01Max;
            school.Char02Max = absenteeismViewModel.Char02Max;
            school.Char03Max = absenteeismViewModel.Char03Max;
            school.Char04Max = absenteeismViewModel.Char04Max;
            school.Char05Max = absenteeismViewModel.Char05Max;
            school.Char06Max = absenteeismViewModel.Char06Max;

            _schoolInfoRepository.UpdateSchoolInfo(school);

            string url = "/M950Absenteeism/index/?userID=" + user.UserID;
            return Redirect(url);
        }

        #region read
        [Route("M950Absenteeism/AbsenteeismDataRead/{userID}/{classroomID}/{dateString}")]
        public IActionResult AbsenteeismDataRead(int userID, int classroomID, string dateString)
        {
            // DateTime date = DateTime.Now;
            DateTime date = DateTime.Parse(dateString);
            //int year = (DateTime.Now.Year);
            //int month = (DateTime.Now.Month);

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> student = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            int classroomIDCont = 0;
            string classroomName = "";
            List<AbsenteeismViewModel> list = new List<AbsenteeismViewModel>();
            foreach (var s in student)
            {
                var isExist = false;
                bool isExist2 = false;

                if (school.NewPeriod == user.UserPeriod)
                    classroomIDCont = s.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(s.SchoolID, s.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(s.SchoolID, s.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomIDCont = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomIDCont == classroomID) { isExist = true; } else { isExist = false; };

                if (isExist)
                {
                    var abs = _absenteeismRepository.GetAbsenteeismStudent(user.SchoolID, user.UserPeriod, s.StudentID, date.Year, date.Month);
                    if (abs == null) abs = new Absenteeism();

                    var absList = new AbsenteeismViewModel();
                    {
                        absList.AbsenteeismID = abs.AbsenteeismID;
                        absList.Period = user.UserPeriod;
                        absList.SchoolID = user.SchoolID;
                        absList.StudentID = s.StudentID;
                        absList.StudentName = _studentRepository.GetStudent(s.StudentID).FirstName + " " + _studentRepository.GetStudent(s.StudentID).LastName;
                        absList.Date = date;
                        

                        if (abs.Day01 == null) abs.Day01 = "";
                        if (abs.Day02 == null) abs.Day02 = "";
                        if (abs.Day03 == null) abs.Day03 = "";
                        if (abs.Day04 == null) abs.Day04 = "";
                        if (abs.Day05 == null) abs.Day05 = "";
                        if (abs.Day06 == null) abs.Day06 = "";
                        if (abs.Day07 == null) abs.Day07 = "";
                        if (abs.Day08 == null) abs.Day08 = "";
                        if (abs.Day09 == null) abs.Day09 = "";
                        if (abs.Day10 == null) abs.Day10 = "";
                        if (abs.Day11 == null) abs.Day11 = "";
                        if (abs.Day12 == null) abs.Day12 = "";
                        if (abs.Day13 == null) abs.Day13 = "";
                        if (abs.Day14 == null) abs.Day14 = "";
                        if (abs.Day15 == null) abs.Day15 = "";
                        if (abs.Day16 == null) abs.Day16 = "";
                        if (abs.Day17 == null) abs.Day17 = "";
                        if (abs.Day18 == null) abs.Day18 = "";
                        if (abs.Day19 == null) abs.Day19 = "";
                        if (abs.Day20 == null) abs.Day20 = "";
                        if (abs.Day21 == null) abs.Day21 = "";
                        if (abs.Day22 == null) abs.Day22 = "";
                        if (abs.Day23 == null) abs.Day23 = "";
                        if (abs.Day24 == null) abs.Day24 = "";
                        if (abs.Day25 == null) abs.Day25 = "";
                        if (abs.Day26 == null) abs.Day26 = "";
                        if (abs.Day27 == null) abs.Day27 = "";
                        if (abs.Day28 == null) abs.Day28 = "";
                        if (abs.Day29 == null) abs.Day29 = "";
                        if (abs.Day30 == null) abs.Day30 = "";
                        if (abs.Day31 == null) abs.Day31 = "";

                        absList.Day01 = abs.Day01;
                        absList.Day02 = abs.Day02;
                        absList.Day03 = abs.Day03;
                        absList.Day04 = abs.Day04;
                        absList.Day05 = abs.Day05;
                        absList.Day06 = abs.Day06;
                        absList.Day07 = abs.Day07;
                        absList.Day08 = abs.Day08;
                        absList.Day09 = abs.Day09;
                        absList.Day10 = abs.Day10;

                        absList.Day11 = abs.Day11;
                        absList.Day12 = abs.Day12;
                        absList.Day13 = abs.Day13;
                        absList.Day14 = abs.Day14;
                        absList.Day15 = abs.Day15;
                        absList.Day16 = abs.Day16;
                        absList.Day17 = abs.Day17;
                        absList.Day18 = abs.Day18;
                        absList.Day19 = abs.Day19;
                        absList.Day20 = abs.Day20;

                        absList.Day21 = abs.Day21;
                        absList.Day22 = abs.Day22;
                        absList.Day23 = abs.Day23;
                        absList.Day24 = abs.Day24;
                        absList.Day25 = abs.Day25;
                        absList.Day26 = abs.Day26;
                        absList.Day27 = abs.Day27;
                        absList.Day28 = abs.Day28;
                        absList.Day29 = abs.Day29;
                        absList.Day30 = abs.Day30;
                        absList.Day31 = abs.Day31;


                        absList.Char01 = school.Char01;
                        absList.Char02 = school.Char02;
                        absList.Char03 = school.Char03;
                        absList.Char04 = school.Char04;
                        absList.Char05 = school.Char05;
                        absList.Char06 = school.Char06;
                        absList.IsChar01 = (bool)school.IsChar01;
                        absList.IsChar02 = (bool)school.IsChar02;
                        absList.IsChar03 = (bool)school.IsChar03;
                        absList.IsChar04 = (bool)school.IsChar04;
                        absList.IsChar05 = (bool)school.IsChar05;
                        absList.IsChar06 = (bool)school.IsChar06;
                        absList.Char01Explanation = school.Char01Explanation;
                        absList.Char02Explanation = school.Char02Explanation;
                        absList.Char03Explanation = school.Char03Explanation;
                        absList.Char04Explanation = school.Char04Explanation;
                        absList.Char05Explanation = school.Char05Explanation;
                        absList.Char06Explanation = school.Char06Explanation;

                        absList.Char01Max = school.Char01Max;
                        absList.Char02Max = school.Char02Max;
                        absList.Char03Max = school.Char03Max;
                        absList.Char04Max = school.Char04Max;
                        absList.Char05Max = school.Char05Max;
                        absList.Char06Max = school.Char06Max;

                    }
                    list.Add(absList);
                }
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult AbsenteeismDataCreate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<AbsenteeismViewModel>>(strResult);

            foreach (var abs in json)
            {
                var absList = _absenteeismRepository.GetAbsenteeismStudent(abs.SchoolID, abs.Period, abs.StudentID, abs.Date.Value.Year, abs.Date.Value.Month);
                if (absList == null) absList = new Absenteeism();

                if (ModelState.IsValid)
                {
                    absList.AbsenteeismID = 0;
                    absList.SchoolID = abs.SchoolID;
                    absList.Period = abs.Period;
                    absList.StudentID = abs.StudentID;
                    absList.Date = abs.Date;

                    absList.Day01 = abs.Day01;
                    absList.Day02 = abs.Day02;
                    absList.Day03 = abs.Day03;
                    absList.Day04 = abs.Day04;
                    absList.Day05 = abs.Day05;
                    absList.Day06 = abs.Day06;
                    absList.Day07 = abs.Day07;
                    absList.Day08 = abs.Day08;
                    absList.Day09 = abs.Day09;
                    absList.Day10 = abs.Day10;

                    absList.Day11 = abs.Day11;
                    absList.Day12 = abs.Day12;
                    absList.Day13 = abs.Day13;
                    absList.Day14 = abs.Day14;
                    absList.Day15 = abs.Day15;
                    absList.Day16 = abs.Day16;
                    absList.Day17 = abs.Day17;
                    absList.Day18 = abs.Day18;
                    absList.Day19 = abs.Day19;
                    absList.Day20 = abs.Day20;

                    absList.Day21 = abs.Day21;
                    absList.Day22 = abs.Day22;
                    absList.Day23 = abs.Day23;
                    absList.Day24 = abs.Day24;
                    absList.Day25 = abs.Day25;
                    absList.Day26 = abs.Day26;
                    absList.Day27 = abs.Day27;
                    absList.Day28 = abs.Day28;
                    absList.Day29 = abs.Day29;
                    absList.Day30 = abs.Day30;
                    absList.Day31 = abs.Day31;

                    _absenteeismRepository.CreateAbsenteeism(absList);
                }
            }
            return Json(true);
        }

        [HttpPost]
        //[Route("M950Absenteeism/AbsenteeismDataCreate/{strResult}/{dateString}")]
        public IActionResult AbsenteeismDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            //DateTime date = DateTime.Parse(dateString);

            var json = new JavaScriptSerializer().Deserialize<List<AbsenteeismViewModel>>(strResult);

            foreach (var abs in json)
            {
                var absList = _absenteeismRepository.GetAbsenteeismStudent(abs.SchoolID, abs.Period, abs.StudentID, abs.Date.Value.Year, abs.Date.Value.Month);

                if (ModelState.IsValid)
                {
                    absList.AbsenteeismID = abs.AbsenteeismID;
                    absList.Day01 = abs.Day01;
                    absList.Day02 = abs.Day02;
                    absList.Day03 = abs.Day03;
                    absList.Day04 = abs.Day04;
                    absList.Day05 = abs.Day05;
                    absList.Day06 = abs.Day06;
                    absList.Day07 = abs.Day07;
                    absList.Day08 = abs.Day08;
                    absList.Day09 = abs.Day09;
                    absList.Day10 = abs.Day10;

                    absList.Day11 = abs.Day11;
                    absList.Day12 = abs.Day12;
                    absList.Day13 = abs.Day13;
                    absList.Day14 = abs.Day14;
                    absList.Day15 = abs.Day15;
                    absList.Day16 = abs.Day16;
                    absList.Day17 = abs.Day17;
                    absList.Day18 = abs.Day18;
                    absList.Day19 = abs.Day19;
                    absList.Day20 = abs.Day20;

                    absList.Day21 = abs.Day21;
                    absList.Day22 = abs.Day22;
                    absList.Day23 = abs.Day23;
                    absList.Day24 = abs.Day24;
                    absList.Day25 = abs.Day25;
                    absList.Day26 = abs.Day26;
                    absList.Day27 = abs.Day27;
                    absList.Day28 = abs.Day28;
                    absList.Day29 = abs.Day29;
                    absList.Day30 = abs.Day30;
                    absList.Day31 = abs.Day31;

                    _absenteeismRepository.UpdateAbsenteeism(absList);
                }
            }
            return Json(true);
        }

        [Route("M950Absenteeism/TotalAbsenteeismDataRead/{userID}/{classroomID}")]
        public IActionResult TotalAbsenteeismDataRead(int userID, int classroomID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            DateTime date = DateTime.Now;

            List<Student> student = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            List<AbsenteeismViewModel> list = new List<AbsenteeismViewModel>();
            double d = 0.5;
            decimal half = Convert.ToDecimal(d);
            DateTime? dateStart = school.SchoolYearStart;
            DateTime? dateEnd = school.SchoolYearEnd;
            decimal total011 = 0;
            decimal total021 = 0;
            decimal total031 = 0;
            decimal total041 = 0;
            decimal total051 = 0;
            decimal total061 = 0;
            decimal total071 = 0;
            decimal total081 = 0;
            decimal total091 = 0;
            decimal total101 = 0;
            decimal total111 = 0;
            decimal total121 = 0;

            decimal total012 = 0;
            decimal total022 = 0;
            decimal total032 = 0;
            decimal total042 = 0;
            decimal total052 = 0;
            decimal total062 = 0;
            decimal total072 = 0;
            decimal total082 = 0;
            decimal total092 = 0;
            decimal total102 = 0;
            decimal total112 = 0;
            decimal total122 = 0;

            int classroomIDCont = 0;
            string classroomName = "";

            foreach (var s in student)
            {
                var isExist = false;
                bool isExist2 = false;

                if (school.NewPeriod == user.UserPeriod)
                    classroomIDCont = s.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(s.SchoolID, s.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(s.SchoolID, s.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomIDCont = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomIDCont == classroomID) { isExist = true; } else { isExist = false; };

                if (isExist)
                {
                    var abs = _absenteeismRepository.GetAbsenteeismTotalStudent(user.SchoolID, user.UserPeriod, s.StudentID);
                    var absList = new AbsenteeismViewModel();

                    foreach (var item in abs)
                    {
                        decimal total1G = 0;
                        decimal total2G = 0;

                        int year = dateStart.Value.Year;
                        int month = dateStart.Value.Month;

                        if (school.Char01 != null && school.Char01 != "" && school.Char01 != " ")
                        {
                            if (item.Day01 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day01 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day02 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day02 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day03 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day03 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day04 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day04 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day05 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day05 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day06 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day06 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day07 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day07 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day08 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day08 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day09 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day09 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day10 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day10 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day11 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day11 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day12 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day12 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day13 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day13 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day14 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day14 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day15 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day15 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day16 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day16 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day17 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day17 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day18 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day18 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day19 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day19 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day20 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day20 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day21 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day21 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day22 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day22 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day23 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day23 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day24 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day24 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day25 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day25 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day26 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day26 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day27 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day27 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day28 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day28 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day29 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day29 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day30 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day30 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                            if (item.Day31 == school.Char01) { if (school.IsChar01 == true) total2G++; else total1G++; }
                            if (item.Day31 == school.Char01.ToLower()) { if (school.IsChar01 == true) total2G += half; else total1G += half; }
                        }

                        if (school.Char02 != null && school.Char02 != "" && school.Char02 != " ")
                        {
                            if (item.Day01 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day01 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day02 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day02 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day03 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day03 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day04 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day04 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day05 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day05 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day06 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day06 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day07 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day07 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day08 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day08 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day09 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day09 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day10 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day10 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day11 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day11 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day12 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day12 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day13 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day13 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day14 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day14 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day15 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day15 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day16 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day16 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day17 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day17 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day18 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day18 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day19 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day19 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day20 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day20 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day21 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day21 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day22 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day22 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day23 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day23 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day24 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day24 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day25 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day25 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day26 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day26 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day27 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day27 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day28 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day28 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day29 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day29 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day30 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day30 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                            if (item.Day31 == school.Char02) { if (school.IsChar02 == true) total2G++; else total1G++; }
                            if (item.Day31 == school.Char02.ToLower()) { if (school.IsChar02 == true) total2G += half; else total1G += half; }
                        }

                        if (school.Char03 != null && school.Char03 != "" && school.Char03 != " ")
                        {
                            if (item.Day01 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day01 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day02 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day02 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day03 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day03 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day04 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day04 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day05 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day05 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day06 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day06 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day07 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day07 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day08 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day08 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day09 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day09 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day10 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day10 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day11 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day11 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day12 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day12 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day13 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day13 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day14 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day14 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day15 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day15 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day16 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day16 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day17 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day17 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day18 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day18 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day19 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day19 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day20 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day20 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day21 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day21 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day22 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day22 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day23 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day23 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day24 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day24 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day25 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day25 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day26 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day26 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day27 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day27 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day28 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day28 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day29 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day29 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day30 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day30 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                            if (item.Day31 == school.Char03) { if (school.IsChar03 == true) total2G++; else total1G++; }
                            if (item.Day31 == school.Char03.ToLower()) { if (school.IsChar03 == true) total2G += half; else total1G += half; }
                        }

                        if (school.Char04 != null && school.Char04 != "" && school.Char04 != " ")
                        {
                            if (item.Day01 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day01 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day02 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day02 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day03 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day03 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day04 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day04 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day05 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day05 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day06 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day06 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day07 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day07 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day08 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day08 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day09 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day09 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day10 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day10 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day11 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day11 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day12 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day12 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day13 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day13 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day14 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day14 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day15 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day15 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day16 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day16 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day17 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day17 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day18 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day18 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day19 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day19 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day20 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day20 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day21 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day21 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day22 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day22 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day23 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day23 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day24 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day24 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day25 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day25 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day26 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day26 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day27 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day27 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day28 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day28 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day29 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day29 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day30 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day30 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                            if (item.Day31 == school.Char04) { if (school.IsChar04 == true) total2G++; else total1G++; }
                            if (item.Day31 == school.Char04.ToLower()) { if (school.IsChar04 == true) total2G += half; else total1G += half; }
                        }

                        if (school.Char05 != null && school.Char05 != "" && school.Char05 != " ")
                        {
                            if (item.Day01 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day01 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day02 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day02 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day03 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day03 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day04 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day04 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day05 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day05 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day06 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day06 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day07 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day07 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day08 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day08 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day09 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day09 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day10 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day10 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day11 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day11 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day12 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day12 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day13 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day13 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day14 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day14 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day15 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day15 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day16 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day16 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day17 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day17 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day18 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day18 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day19 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day19 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day20 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day20 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day21 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day21 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day22 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day22 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day23 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day23 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day24 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day24 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day25 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day25 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day26 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day26 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day27 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day27 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day28 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day28 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day29 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day29 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day30 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day30 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                            if (item.Day31 == school.Char05) { if (school.IsChar05 == true) total2G++; else total1G++; }
                            if (item.Day31 == school.Char05.ToLower()) { if (school.IsChar05 == true) total2G += half; else total1G += half; }
                        }

                        if (school.Char06 != null && school.Char06 != "" && school.Char06 != " ")
                        {
                            if (item.Day01 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day01 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day02 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day02 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day03 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day03 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day04 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day04 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day05 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day05 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day06 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day06 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day07 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day07 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day08 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day08 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day09 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day09 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day10 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day10 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day11 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day11 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day12 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day12 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day13 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day13 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day14 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day14 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day15 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day15 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day16 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day16 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day17 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day17 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day18 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day18 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day19 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day19 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day20 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day20 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day21 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day21 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day22 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day22 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day23 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day23 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day24 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day24 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day25 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day25 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day26 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day26 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day27 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day27 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day28 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day28 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day29 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day29 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day30 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day30 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                            if (item.Day31 == school.Char06) { if (school.IsChar06 == true) total2G++; else total1G++; }
                            if (item.Day31 == school.Char06.ToLower()) { if (school.IsChar06 == true) total2G += half; else total1G += half; }
                        }

                        if (item.Date.Value.Month == 01) { total011 = total1G; total012 = total2G; };
                        if (item.Date.Value.Month == 02) { total021 = total1G; total022 = total2G; };
                        if (item.Date.Value.Month == 03) { total031 = total1G; total032 = total2G; };
                        if (item.Date.Value.Month == 04) { total041 = total1G; total042 = total2G; };
                        if (item.Date.Value.Month == 05) { total051 = total1G; total052 = total2G; };
                        if (item.Date.Value.Month == 06) { total061 = total1G; total062 = total2G; };
                        if (item.Date.Value.Month == 07) { total071 = total1G; total072 = total2G; };
                        if (item.Date.Value.Month == 08) { total081 = total1G; total082 = total2G; };
                        if (item.Date.Value.Month == 09) { total091 = total1G; total092 = total2G; };
                        if (item.Date.Value.Month == 10) { total101 = total1G; total102 = total2G; };
                        if (item.Date.Value.Month == 11) { total111 = total1G; total112 = total2G; };
                        if (item.Date.Value.Month == 12) { total121 = total1G; total122 = total2G; };

                        string hmonth01 = null; string hmonth02 = null; string hmonth03 = null; string hmonth04 = null; string hmonth05 = null; string hmonth06 = null; string hmonth07 = null; string hmonth08 = null; string hmonth09 = null; string hmonth10 = null; string hmonth11 = null; string hmonth12 = null; ;

                        var inx = 0;
                        for (int i = 1; i < 13; i++)
                        {
                            if (i >= month)
                            {
                                DateTime newDate = new DateTime(year, i, 1);
                                inx++;
                                if (inx == 01) { hmonth01 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total101 = total.Item1; absList.Total201 = total.Item2; }
                                if (inx == 02) { hmonth02 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total102 = total.Item1; absList.Total202 = total.Item2; }
                                if (inx == 03) { hmonth03 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total103 = total.Item1; absList.Total203 = total.Item2; }
                                if (inx == 04) { hmonth04 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total104 = total.Item1; absList.Total204 = total.Item2; }
                                if (inx == 05) { hmonth05 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total105 = total.Item1; absList.Total205 = total.Item2; }
                                if (inx == 06) { hmonth06 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total106 = total.Item1; absList.Total206 = total.Item2; }
                                if (inx == 07) { hmonth07 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total107 = total.Item1; absList.Total207 = total.Item2; }
                                if (inx == 08) { hmonth08 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total108 = total.Item1; absList.Total208 = total.Item2; }
                                if (inx == 09) { hmonth09 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total109 = total.Item1; absList.Total209 = total.Item2; }
                                if (inx == 10) { hmonth10 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total110 = total.Item1; absList.Total210 = total.Item2; }
                                if (inx == 11) { hmonth11 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total111 = total.Item1; absList.Total211 = total.Item2; }
                                if (inx == 12) { hmonth12 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total112 = total.Item1; absList.Total212 = total.Item2; }
                            }
                        }

                        year = dateEnd.Value.Year;
                        month = dateEnd.Value.Month;
                        for (int i = 1; i < 13; i++)
                        {
                            if (i <= month)
                            {
                                DateTime newDate = new DateTime(year, i, 1);
                                inx++;
                                if (inx == 01) { hmonth01 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total101 = total.Item1; absList.Total201 = total.Item2; }
                                if (inx == 02) { hmonth02 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total102 = total.Item1; absList.Total202 = total.Item2; }
                                if (inx == 03) { hmonth03 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total103 = total.Item1; absList.Total203 = total.Item2; }
                                if (inx == 04) { hmonth04 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total104 = total.Item1; absList.Total204 = total.Item2; }
                                if (inx == 05) { hmonth05 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total105 = total.Item1; absList.Total205 = total.Item2; }
                                if (inx == 06) { hmonth06 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total106 = total.Item1; absList.Total206 = total.Item2; }
                                if (inx == 07) { hmonth07 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total107 = total.Item1; absList.Total207 = total.Item2; }
                                if (inx == 08) { hmonth08 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total108 = total.Item1; absList.Total208 = total.Item2; }
                                if (inx == 09) { hmonth09 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total109 = total.Item1; absList.Total209 = total.Item2; }
                                if (inx == 10) { hmonth10 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total110 = total.Item1; absList.Total210 = total.Item2; }
                                if (inx == 11) { hmonth11 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total111 = total.Item1; absList.Total211 = total.Item2; }
                                if (inx == 12) { hmonth12 = newDate.ToString("MMM, yyyy"); var total = ConvertToTotals(i, total011, total012, total021, total022, total031, total032, total041, total042, total051, total052, total061, total062, total071, total072, total081, total082, total091, total092, total101, total102, total111, total112, total121, total122); absList.Total112 = total.Item1; absList.Total212 = total.Item2; }
                            }
                        }

                    }

                    absList.GrandTotal1 = absList.Total101 + absList.Total102 + absList.Total103 + absList.Total104 + absList.Total105 + absList.Total106 + absList.Total107 + absList.Total108 + absList.Total109 + absList.Total110 + absList.Total111 + absList.Total112;
                    absList.GrandTotal2 = absList.Total201 + absList.Total202 + absList.Total203 + absList.Total204 + absList.Total205 + absList.Total206 + absList.Total207 + absList.Total208 + absList.Total209 + absList.Total210 + absList.Total211 + absList.Total212;

                    //absList.AbsenteeismID = item.AbsenteeismID;
                    absList.Period = user.UserPeriod;
                    absList.SchoolID = user.SchoolID;
                    absList.StudentID = s.StudentID;
                    absList.StudentName = _studentRepository.GetStudent(s.StudentID).FirstName + " " + _studentRepository.GetStudent(s.StudentID).LastName;
                    //absList.Date = date;
                    absList.WarningSW = 0;
                    if (absList.GrandTotal2 > school.Char01Max) absList.WarningSW = 1;
                    list.Add(absList);
                }
            }

            return Json(list);
        }

        public (decimal, decimal) ConvertToTotals(int month, decimal total011, decimal total012, decimal total021, decimal total022, decimal total031, decimal total032, decimal total041, decimal total042, decimal total051, decimal total052, decimal total061, decimal total062, decimal total071, decimal total072, decimal total081, decimal total082, decimal total091, decimal total092, decimal total101, decimal total102, decimal total111, decimal total112, decimal total121, decimal total122)
        {
            decimal rtotal1 = 0;
            decimal rtotal2 = 0;

            if (month == 01) { rtotal1 = total011; rtotal2 = total012; }
            if (month == 02) { rtotal1 = total021; rtotal2 = total022; }
            if (month == 03) { rtotal1 = total031; rtotal2 = total032; }
            if (month == 04) { rtotal1 = total041; rtotal2 = total042; }
            if (month == 05) { rtotal1 = total051; rtotal2 = total052; }
            if (month == 06) { rtotal1 = total061; rtotal2 = total062; }
            if (month == 07) { rtotal1 = total071; rtotal2 = total072; }
            if (month == 08) { rtotal1 = total081; rtotal2 = total082; }
            if (month == 09) { rtotal1 = total091; rtotal2 = total092; }
            if (month == 10) { rtotal1 = total101; rtotal2 = total102; }
            if (month == 11) { rtotal1 = total111; rtotal2 = total112; }
            if (month == 12) { rtotal1 = total121; rtotal2 = total122; }

            return (rtotal1, rtotal2);
        }
    }

    #endregion
}

