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
    public class M140DiscountTableController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IClassroomRepository _classroomRepository;
        IParameterRepository _parameterRepository;
        IDiscountTableRepository _discountTableRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IWebHostEnvironment _hostEnvironment;
        public M140DiscountTableController(
             IClassroomRepository classroomRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IParameterRepository parameterRepository,
             IDiscountTableRepository discountTableRepository,
             IUsersRepository usersRepository,
             IUsersWorkAreasRepository usersWorkAreasRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _classroomRepository = classroomRepository;
            _parameterRepository = parameterRepository;
            _discountTableRepository = discountTableRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult index(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("İndirim Tablosu").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName = "discountName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var schoolViewModel = new SchoolViewModel
            {
                IsPermission = isPermission,
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

        [Route("M140DiscountTable/DiscountTableDataRead/{userID}/{period}")]
        public IActionResult DiscountTableDataRead(int userID, string period)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("İndirim Dilekçesi").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);
            var discountTable = _discountTableRepository.GetDiscountTablePeriod(period, user.SchoolID);
            List<DiscountTable> list = new List<DiscountTable>();

            if ( (discountTable == null || discountTable.Count() == 0) && user.UserPeriod == period )
            {
                int s = 0;
                foreach (var item in parameter)
                {
                    s = s + 1;
                    var discount = new DiscountTable();
                    discount.DiscountName = item.CategoryName;
                    discount.SchoolID = user.SchoolID;
                    discount.DiscountPercent = 0;
                    discount.DiscountAmount = 0;
                    discount.SortOrder = s;
                    discount.Period = user.UserPeriod;
                    discount.IsActive = true;
                    discount.IsSelect = true;
                    discount.IsPasswordRequired = false;
                    list.Add(discount);

                    _discountTableRepository.CreateDiscountTable(discount);
                }
                discountTable = list;
            }
            return Json(discountTable);
        }


        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }
        #endregion

        #region discount update, create, delete
        [HttpPost]
        public IActionResult DiscountTableDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<DiscountTable>>(strResult);

            List<DiscountTable> list = new List<DiscountTable>();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _discountTableRepository.GetDiscountTable(json[i].DiscountTableID);

                getCode.DiscountTableID = json[i].DiscountTableID;
                getCode.SchoolID = json[i].SchoolID;
                getCode.Period = json[i].Period;
                getCode.DiscountName = json[i].DiscountName;
                getCode.DiscountPercent = json[i].DiscountPercent;
                getCode.DiscountAmount = json[i].DiscountAmount;
                getCode.IsPasswordRequired = json[i].IsPasswordRequired;
                getCode.SortOrder = json[i].SortOrder;

                getCode.IsActive = json[i].IsActive;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _discountTableRepository.UpdateDiscountTable(getCode);
                }
                i++;
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult DiscountTableDataCreate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<DiscountTable>>(strResult);
            List<DiscountTable> list = new List<DiscountTable>();

            var i = 0;
            foreach (var item in json)
            {
                var sortNo = 0;
                var lastNo = _discountTableRepository.GetDiscountTableAll();

                if (lastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)lastNo.Max(a => a.SortOrder);
                    sortNo++;
                }
                var discountTable = new DiscountTable();
                discountTable.DiscountTableID = 0;
                discountTable.SchoolID = json[i].SchoolID;
                discountTable.Period = json[i].Period;
                discountTable.DiscountName = json[i].DiscountName;
                discountTable.DiscountPercent = json[i].DiscountPercent;
                discountTable.DiscountAmount = json[i].DiscountAmount;
                discountTable.IsPasswordRequired = json[i].IsPasswordRequired;
                discountTable.SortOrder = json[i].SortOrder;
                if (discountTable.SortOrder == 0)
                {
                    discountTable.SortOrder = sortNo;
                }
                discountTable.IsActive = json[i].IsActive;
                discountTable.IsSelect = true;
                list.Add(discountTable);

                if (ModelState.IsValid)
                {
                    _discountTableRepository.CreateDiscountTable(discountTable);
                }
                i = i + 1;
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult DiscountTableDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<DiscountTable>>(strResult);
            List<DiscountTable> list = new List<DiscountTable>();

            var i = 0;
            foreach (var item in json)
            {
                var discountTable = new DiscountTable();
                discountTable.DiscountTableID = json[i].DiscountTableID;
                discountTable.SchoolID = json[i].SchoolID;
                discountTable.Period = json[i].Period;
                discountTable.DiscountName = json[i].DiscountName;
                discountTable.DiscountAmount = json[i].DiscountAmount;
                discountTable.DiscountPercent = json[i].DiscountPercent;
                discountTable.IsPasswordRequired = json[i].IsPasswordRequired;
                discountTable.SortOrder = json[i].SortOrder;
                discountTable.IsActive = json[i].IsActive;
                list.Add(discountTable);

                if (ModelState.IsValid)
                {
                    _discountTableRepository.DeleteDiscountTable(discountTable);
                }
                i++;
            }
            return Json(list);
        }
        #endregion
    }
}