using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ESCHOOL.Controllers
{
    public class M905UsersLogController : Controller
    {
        IUsersLogRepository _usersLogRepository;
        IUsersRepository _usersRepository;
        ISchoolInfoRepository _schoolInfoRepository;
        IParameterRepository _parameterRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IWebHostEnvironment _hostEnvironment;
        public M905UsersLogController(
            IUsersLogRepository usersLogRepository,
            IUsersRepository usersRepository,
            ISchoolInfoRepository schoolInfoRepository,
            IParameterRepository parameterRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            IWebHostEnvironment hostEnvironment)
        {
            _usersLogRepository = usersLogRepository;
            _usersRepository = usersRepository;
            _schoolInfoRepository = schoolInfoRepository;
            _parameterRepository = parameterRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult index(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Denetim İzi").CategoryID;
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

            var schoolViewModel = new SchoolViewModel
            {
                IsPermission = isPermission,
                UserID = userID,
                SchoolID = user.SchoolID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(schoolViewModel);
        }
        #region read

        [Route("M905UsersLog/UsersLogDataRead/{userID}/{period}")]
        public IActionResult UsersLogDataRead(int userID, string period)
        {
            var user = _usersRepository.GetUser(userID);
            var usersLog = _usersLogRepository.GetUsersLogAll(period);

            List<UsersLogViewModel> list = new List<UsersLogViewModel>();
            foreach (var item in usersLog)
            {
                string transaction = "";
                if (item.TransactionID > 0)
                   transaction = _parameterRepository.GetParameter(item.TransactionID).CategoryName;
                var log = new UsersLogViewModel();
                log.UsersLogID = item.UserLogID;
                log.SchoolID = user.SchoolID;
                log.UserName = user.UserName;
                log.Period = user.UserPeriod;
                log.Transaction = transaction;
                log.UserLogDate = item.UserLogDate;
                log.UserLogDescription = item.UserLogDescription;
                list.Add(log);
            }
            return Json(list);
        }

        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }
        #endregion

        public void UsersLogWrite(int userID, int transactionID, DateTime transactiondate, string description)
        {
            var user = _usersRepository.GetUser(userID);

            UsersLog usersLog = new UsersLog();
            usersLog.UserLogID = 0;
            usersLog.SchoolID = user.SchoolID;
            usersLog.UserID = userID;
            usersLog.Period = user.UserPeriod;
            usersLog.TransactionID = transactionID;
            usersLog.UserLogDate = transactiondate;
            usersLog.UserLogDescription = description;
            _usersLogRepository.CreateUsersLog(usersLog);
        }
    }
}
