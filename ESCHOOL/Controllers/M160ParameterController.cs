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
    public class M160ParameterController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IParameterRepository _parameterRepository;
        IUsersRepository _usersRepository;
        IWebHostEnvironment _hostEnvironment;
        public M160ParameterController(
             IParameterRepository parameterRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IUsersRepository usersRepository,
             IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _parameterRepository = parameterRepository;
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

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var schoolViewModel = new SchoolViewModel
            {
                StudentID = studentID,
                UserID = userID,
                SchoolID = user.SchoolID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName
            };
            return View(schoolViewModel);
        }
        public IActionResult indexMarkers(int userID, int studentID)
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
                SchoolID = user.SchoolID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName
            };
            return View(schoolViewModel);
        }
        #region read
        [Route("M160Parameter/ParameterDataReadMain/{L}")]
        public IActionResult ParameterDataReadMain(string L)
        {
            string item1 = "Kullanıcı İşaretleyicileri";
            string item2 = "Öğrenci İşaretleyicileri";
            string item3 = "Okul İşaretleyicileri";
            string item4 = "Ders İşaretleyicileri";

            var parameter = _parameterRepository.GetParameterMain(L).Where(c => c.CategoryName != item1 && c.CategoryName != item2 && c.CategoryName != item3 && c.CategoryName != item4);
            return Json(parameter);
        }

        [Route("M160Parameter/ParameterDataReadMarkersMain/{L}")]
        public IActionResult ParameterDataReadMarkersMain(string L)
        {
            string item1 = "Kullanıcı İşaretleyicileri";
            string item2 = "Öğrenci İşaretleyicileri";
            string item3 = "Okul İşaretleyicileri";
            string item4 = "Ders İşaretleyicileri";

            var parameter = _parameterRepository.GetParameterMain(L).Where(c=> c.CategoryName == item1 || c.CategoryName == item2 || c.CategoryName == item3 || c.CategoryName == item4);
            return Json(parameter);
        }

        [Route("M160Parameter/ParameterDataReadDetail/{ID}/{L}")]
        public IActionResult ParameterDataRead(int ID, string L)
        {
            var parameter = _parameterRepository.GetParameterDetail(ID, L);
            return Json(parameter);
        }
        #endregion
        #region create, update, delete
        [Route("M160Parameter/ParameterDataCreate/{strResult}/{id}/{L}")]
        public IActionResult ParameterDataCreate([Bind(Prefix = "models")] string strResult, int id, string L)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Parameter>>(strResult);
            List<Parameter> list = new List<Parameter>();
           
            foreach (var item in json.OrderBy(s=> s.CategoryName))
            {
                string nationalityCode = "";
                if (item.NationalityCode == "tr-TR") nationalityCode = "TR";
                if (item.NationalityCode == "en-US") nationalityCode = "EN";

                var getCode = new Parameter();
                getCode.CategoryID = 0;
                getCode.CategorySubID = id;
                getCode.CategoryName = item.CategoryName;
                getCode.CategoryLevel = L;
                getCode.SortOrder = item.SortOrder;
                getCode.Color = item.Color;

                var sortNo = 0;
                var lastNo = _parameterRepository.GetParameterSubID(id);
                if (lastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)lastNo.Max(a => a.SortOrder);
                    sortNo++;
                }

                if (getCode.SortOrder == 0)
                {
                    getCode.SortOrder = sortNo;
                }

                getCode.IsActive = item.IsActive;
                getCode.IsProtected = item.IsProtected;
                getCode.NationalityCode = nationalityCode;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _parameterRepository.CreateParameter(getCode);
                }
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M160Parameter/ParameterDataUpdate/{strResult}/{L}")]
        public IActionResult ParameterDataUpdate([Bind(Prefix = "models")] string strResult, string L)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Parameter>>(strResult);

            Parameter parameter = new Parameter();
            foreach (var item in json)
            {
                var getCode = _parameterRepository.GetParameter(item.CategoryID);

                getCode.CategoryID = item.CategoryID;
                getCode.CategorySubID = item.CategorySubID;
                getCode.CategoryName = item.CategoryName;
                getCode.CategoryLevel = item.CategoryLevel;
                getCode.SortOrder = item.SortOrder;
                getCode.IsActive = item.IsActive;
                getCode.IsProtected = item.IsProtected;
                getCode.Color = item.Color;
                if (ModelState.IsValid)
                {
                    _parameterRepository.UpdateParameter(getCode);
                }
            }
            return Json(parameter);
        }

        [HttpPost]
        public IActionResult ParameterDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Parameter>>(strResult);
            List<Parameter> list = new List<Parameter>();

            foreach (var item in json)
            {
                var getCode = new Parameter();
                getCode.CategoryID = item.CategoryID;
                getCode.CategorySubID = item.CategorySubID;
                getCode.CategoryName = item.CategoryName;
                getCode.CategoryLevel = item.CategoryLevel;
                getCode.SortOrder = item.SortOrder;
                getCode.IsActive = item.IsActive;
                getCode.IsProtected = item.IsProtected;
                getCode.Color = item.Color;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _parameterRepository.DeleteParameter(getCode);
                }
            }
            return Json(list);
        }
        #endregion
    }
}