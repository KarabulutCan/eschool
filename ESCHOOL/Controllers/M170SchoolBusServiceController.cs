using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Controllers
{
    public class M170SchoolBusServiceController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        ISchoolBusServicesRepository _schoolBusServicesRepository;
        IUsersRepository _usersRepository;
        IWebHostEnvironment _hostEnvironment;
        public M170SchoolBusServiceController(
             ISchoolBusServicesRepository schoolBusServicesRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IUsersRepository usersRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;
            _usersRepository = usersRepository;
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

            var schoolViewModel = new SchoolViewModel
            {
                StudentID = studentID,
                UserID = userID,
                SchoolID = user.SchoolID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(schoolViewModel);
        }

        #region read
        [Route("M170SchoolBusService/SchoolBusServiceDataRead/{userID}/{period}")]
        public IActionResult SchoolBusServiceDataRead(int userID, string period)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolBusServices = _schoolBusServicesRepository.GetSchoolBusServicesAll(user.SchoolID, period);
            return Json(schoolBusServices);
        }

        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }

        #endregion
        #region create, update, delete
        [HttpPost]
        public IActionResult SchoolBusServiceDataCreate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolBusServices>>(strResult);
            List<SchoolBusServices> list = new List<SchoolBusServices>();

            var i = 0;
            foreach (var item in json)
            {
                var sortNo = 0;
                var lastNo = _schoolBusServicesRepository.GetSchoolBusServicesAll(json[i].SchoolID, json[i].Period);

                if (lastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)lastNo.Max(a => a.SortOrder);
                    sortNo++;
                }

                var schoolBusServices = new SchoolBusServices();
                schoolBusServices.SchoolBusServicesID = 0;
                schoolBusServices.SchoolID = json[i].SchoolID;
                schoolBusServices.Period = json[i].Period;
                schoolBusServices.PlateNo = json[i].PlateNo;
                schoolBusServices.DriverName = json[i].DriverName;
                schoolBusServices.BusPhone = json[i].BusPhone;
                schoolBusServices.BusRoute = json[i].BusRoute;
                schoolBusServices.BusTeacher = json[i].BusTeacher;
                schoolBusServices.BusTeacherPhone = json[i].BusTeacherPhone;
                schoolBusServices.Explanation = json[i].Explanation;
                schoolBusServices.SortOrder = json[i].SortOrder;
                if (schoolBusServices.SortOrder == 0)
                {
                    schoolBusServices.SortOrder = sortNo;
                }
                schoolBusServices.IsActive = json[i].IsActive;
                list.Add(schoolBusServices);

                if (ModelState.IsValid)
                {
                    _schoolBusServicesRepository.CreateSchoolBusServices(schoolBusServices);
                }
                i = i + 1;
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult SchoolBusServiceDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolBusServices>>(strResult);

            SchoolBusServices schoolBusServices = new SchoolBusServices();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _schoolBusServicesRepository.GetSchoolBusServices(json[i].SchoolBusServicesID);
                getCode.SchoolID = json[i].SchoolID;
                getCode.SchoolBusServicesID = json[i].SchoolBusServicesID;
                getCode.PlateNo = json[i].PlateNo;
                getCode.DriverName = json[i].DriverName;
                getCode.BusPhone = json[i].BusPhone;
                getCode.BusRoute = json[i].BusRoute;
                getCode.BusTeacher = json[i].BusTeacher;
                getCode.BusTeacherPhone = json[i].BusTeacherPhone;
                getCode.Explanation = json[i].Explanation;
                getCode.SortOrder = json[i].SortOrder;
                getCode.IsActive = json[i].IsActive;
                if (ModelState.IsValid)
                {
                    _schoolBusServicesRepository.UpdateSchoolBusServices(getCode);
                }
                i = i + 1;
            }
            return Json(schoolBusServices);
        }

        [HttpPost]
        public IActionResult SchoolBusServiceDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolBusServices>>(strResult);
            List<SchoolBusServices> list = new List<SchoolBusServices>();

            var i = 0;
            foreach (var item in json)
            {
                var schoolBusServices = new SchoolBusServices();
                schoolBusServices.SchoolBusServicesID = json[i].SchoolBusServicesID;
                schoolBusServices.SchoolID = json[i].SchoolID;
                schoolBusServices.PlateNo = json[i].PlateNo;
                schoolBusServices.DriverName = json[i].DriverName;
                schoolBusServices.BusPhone = json[i].BusPhone;
                schoolBusServices.BusRoute = json[i].BusRoute;
                schoolBusServices.BusTeacher = json[i].BusTeacher;
                schoolBusServices.BusTeacherPhone = json[i].BusTeacherPhone;
                schoolBusServices.Explanation = json[i].Explanation;
                schoolBusServices.SortOrder = json[i].SortOrder;
                schoolBusServices.IsActive = json[i].IsActive;
                list.Add(schoolBusServices);
                if (ModelState.IsValid)
                {
                    _schoolBusServicesRepository.DeleteSchoolBusServices(schoolBusServices);
                }
                i = i + 1;
            }
            return Json(list);
        }
        #endregion
    }
}