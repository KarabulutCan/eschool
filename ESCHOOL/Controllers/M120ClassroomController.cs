using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Controllers
{
    public class M120ClassroomController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IClassroomRepository _classroomRepository;
        IParameterRepository _parameterRepository;
        IUsersRepository _usersRepository;
        IStudentRepository _studentRepository;
        IWebHostEnvironment _hostEnvironment;
        public M120ClassroomController(
             IClassroomRepository classroomRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IParameterRepository parameterRepository,
             IUsersRepository usersRepository,
             IStudentRepository studentRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _classroomRepository = classroomRepository;
            _parameterRepository = parameterRepository;
            _usersRepository = usersRepository;
            _studentRepository = studentRepository;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult index(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var schoolViewModel = new SchoolViewModel
            {
                StudentID = studentID,
                UserID = userID,
                UserPeriod = user.UserPeriod,
                SchoolID = user.SchoolID,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName,
            };
            return View(schoolViewModel);
        }
        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }

        [Route("M120Classroom/GridDataRead/{userID}/{period}")]
        public IActionResult GridDataRead(int userID, string period)
        {
            var user = _usersRepository.GetUser(userID);

            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID);

            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, period);

            List<ClassroomViewModel> list = new List<ClassroomViewModel>();
            foreach (var item in classroom)
            {
                var parameter = parameterList.FirstOrDefault(p => p.CategoryID == item.ClassroomTypeID);
                if (parameter == null)
                {
                    parameter = parameterList.FirstOrDefault(p => p.CategorySubID == categoryID);
                }

                var categoryName = parameter.CategoryName;
                if (user.SelectedCulture.Trim() == "en-US") categoryName = parameter.Language1;
                
                var classroomViewModel = new ClassroomViewModel
                {
                    UserPeriod = item.Period,

                    ViewModelID = item.ClassroomID,
                    SchoolInfo = schoolInfo,

                    ClassroomID = item.ClassroomID,
                    SchoolID = item.SchoolID,
                    Period = item.Period,
                    ClassroomName = item.ClassroomName,
                    ClassroomTypeID = item.ClassroomTypeID,
                    ClassroomTeacher = item.ClassroomTeacher,
                    RoomQuota = item.RoomQuota,
                    SortOrder = item.SortOrder,
                    IsActive = (bool)item.IsActive,
                    IsProtected = _studentRepository.ExistClassroom(item.ClassroomID),
                    CategoryID = parameter.CategoryID,
                    CategoryName = categoryName,
                };

                list.Add(classroomViewModel);
            }
            return Json(list);
        }

        #region Others
        public IActionResult ClassroomDataRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var room = _parameterRepository.GetParameterSubID(categoryID);
            return Json(room);
        }

        [HttpPost]
        [Route("M120Classroom/GridDataUpdate/{strResult}/{userID}")]
        public IActionResult GridDataUpdate([Bind(Prefix = "models")] string strResult, int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var json = new JavaScriptSerializer().Deserialize<List<ClassroomViewModel>>(strResult);
            Classroom classroom = new Classroom();

            classroom.ClassroomID = json[0].ClassroomID;
            classroom.SchoolID = json[0].SchoolID;
            classroom.Period = json[0].Period;
            classroom.ClassroomName = json[0].ClassroomName;
            classroom.ClassroomTypeID = json[0].ClassroomTypeID;
            classroom.ClassroomTeacher = json[0].ClassroomTeacher;
            classroom.SortOrder = json[0].SortOrder;
            classroom.RoomQuota = json[0].RoomQuota;
            classroom.IsActive = json[0].IsActive;
            if (ModelState.IsValid)
            {
                _classroomRepository.UpdateClassroom(classroom);
            }

            GridDataRead(userID, user.UserPeriod);
            return Json(true);
        }

        [HttpPost]
        [Route("M120Classroom/GridDataCreate/{strResult}/{userID}")]
        public IActionResult GridDataCreate([Bind(Prefix = "models")] string strResult, int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var sortNo = 0;
            var classroomLastNo = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            if (classroomLastNo.Count() == 0)
            {
                sortNo = 1;
            }
            else
            {
                sortNo = classroomLastNo.Max(a => a.SortOrder);
                sortNo++;
            }

            var json = new JavaScriptSerializer().Deserialize<List<ClassroomViewModel>>(strResult);
            Classroom classroom = new Classroom();
            classroom.SchoolID = user.SchoolID;
            classroom.Period = user.UserPeriod;
            classroom.ClassroomID = json[0].ClassroomID;
            classroom.ClassroomName = json[0].ClassroomName;
            classroom.ClassroomTypeID = json[0].ClassroomTypeID;
            classroom.ClassroomTeacher = json[0].ClassroomTeacher;
            classroom.SortOrder = json[0].SortOrder;
            if (classroom.SortOrder == 0)
            {
                classroom.SortOrder = sortNo;
            }
            classroom.RoomQuota = json[0].RoomQuota;
            classroom.IsActive = json[0].IsActive;
            if (ModelState.IsValid)
            {
                _classroomRepository.CreateClassroom(classroom);
            }

            GridDataRead(userID, user.UserPeriod);
            return Json(true);
        }

        [HttpPost]
        public IActionResult GridDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<ClassroomViewModel>>(strResult);
            int ID = json[0].ClassroomID;
            var classroom = _classroomRepository.GetClassroomID(ID);

            if (ID == 0)
                return Json(true);

            _classroomRepository.DeleteClassroom(classroom);
            _classroomRepository.Save();

            return Json(true);
        }
        #endregion
    }
}
