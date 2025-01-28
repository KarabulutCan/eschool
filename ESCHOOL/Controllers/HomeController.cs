using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Spreadsheet;
using Parameter = ESCHOOL.Models.Parameter;

namespace ESCHOOL.Controllers
{
    public class HomeController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IUsersRepository _usersRepository;
        IUsersChatRepository _usersChatRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IWebHostEnvironment _hostEnvironment;
        public HomeController(
            ISchoolInfoRepository schoolInfoRepository,
            IUsersRepository usersRepository,
            IUsersChatRepository usersChatRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _usersRepository = usersRepository;
            _usersChatRepository = usersChatRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }

        //[HttpPost]
        //[Route("Home/index/{userID}")]
        public IActionResult index(int userID, string userName, string password)
        {
            //Memory clearing
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            if (userID == 0)
                // while logging in
                userID = _usersRepository.GetUser(userName, password).UserID;
            
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            if (school.NewPeriod == null)
            {
                school.NewPeriod = user.UserPeriod;
                _schoolInfoRepository.UpdateSchoolInfo(school);
            }

            var schoolName = "";
            if (user.SchoolID > 0)
                schoolName = school.CompanyName;
            else
            {
                user.SchoolID = 1;
                schoolName = school.CompanyName;
            }

            if (user == null)
            {
                user = new Models.Users();
                user.SchoolID = user.SchoolID;
            }

            // Manin menu / Change 

            if (user.SelectedCulture.Trim() == "tr-TR")
            {
                TempData["eSchool"] = "E-School ";
                TempData["LastVersion"] = "Yeni Versiyon : 09.01.2025";
            }
            else
            {
                TempData["eSchool"] = "E-School: ";
                TempData["LastVersion"] = "New Version : 09.01.2025";
            }

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["SchoolName"] = schoolName + " - (" + user.SelectedSchoolCode + ")";
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;
            TempData["culture"] = user.SelectedCulture;

            TempData["theme"] = "../telerik/2021.2.616/styles/kendo." + user.SelectedTheme + ".min.css";
            TempData["themeMobile"] = "../telerik/2021.2.616/styles/kendo." + user.SelectedTheme + ".mobile.min.css";
            TempData["color"] = "#ffffff}";
            if (user.SelectedTheme == "black" || user.SelectedTheme == "metroblack" || user.SelectedTheme == "highcontrast" || user.SelectedTheme == "moonlight") TempData["color"] = "#222";

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            password = connectionString.Password;
            string cs = $"Data Source={dataSource};Initial Catalog={user.SelectedSchoolCode};User Id=sa;Password={password};";

            int messageCount = _usersChatRepository.GetUsersMessagesReceive(user.SelectedSchoolCode, userID, false).Count();

            string messageCountTxt = "";
            string icon = "share";
            if (messageCount > 0)
            {
                messageCountTxt = messageCount.ToString();
                icon = "comment";
            }
            var schoolInfo = new SchoolInfo();
            var schoolViewModel = new SchoolViewModel
            {
                
                UserID = user.UserID,
                SchoolID = user.SchoolID,
                StudentID = 5,
                SchoolName = schoolName,
                UserDate = user.UserDate,
                UserPeriod = user.UserPeriod,
                NewPeriod = school.NewPeriod,

                UserPicture = user.UserPicture,
                UserName = user.UserName + " ( " + user.FirstName + " " + user.LastName + " - #" + user.UserID + " )",
                UserViewPicture = user.UserViewPicture,
                SelectedTheme = user.SelectedTheme,
                SelectedSchoolCode = user.SelectedSchoolCode,
                ConnectionString = cs,

                SelectedCulture = user.SelectedCulture.Trim(),

                RegistrationTypeSubID = 3,
                StatuCategorySubID = 4,

                StatuCategoryID = 1391,

                SchoolInfo = schoolInfo,
                MessageCount = messageCountTxt,
                Icon = icon,

                StartDate = school.SchoolYearStart,
                EndDate = school.SchoolYearEnd,
                wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/"), //Picture Path

            };
            return View(schoolViewModel);
        }

        [Route("Home/PeriodDataRead/{plusYear}")]
        public IActionResult PeriodDataRead(int plusYear)
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, plusYear);
            return Json(mylist);
        }

        [Route("Home/SchoolDataRead/{userID}")]
        public IActionResult SchoolDataRead(int userID)
        {
            var usersWork = _usersWorkAreasRepository.GetUserWorkAreasID(userID).Where(b => b.IsSchool == true && b.IsSelect == true);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfoAll();
            var selectedSchool = schoolInfo.Where(s => usersWork.Where(p => p.CategoryID == s.SchoolID).Count() > 0).ToList();

            return Json(selectedSchool);
        }

        [HttpPost]
        [Route("Home/UserSaveChanges/{userID}/{period}/{schoolCode}/{dateString}/{themeChooser}")]
        public IActionResult UserSaveChanges(int userID, string period, int schoolCode, string dateString, string themeChooser)
        {
            DateTime userDate = DateTime.Now;
            if (dateString != null)
                userDate = DateTime.Parse(dateString);
            var user = _usersRepository.GetUser(userID);

            user.UserPeriod = period;
            user.SchoolID = schoolCode;
            user.UserDate = userDate;
            user.SelectedTheme = themeChooser;
            _usersRepository.UpdateUsers(user);

            return Json(true);
        }

        [HttpPost]
        [Route("Home/ExitProgram/{userID}")]
        public IActionResult ExitProgram(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            user.IsLogin = false;
            _usersRepository.UpdateUsers(user);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            return Json(true);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Home/GridLoginUsersDataRead/{prgUserID}")]
        public IActionResult GridLoginUsersDataRead(int prgUserID)
        {
            DateTime loginDate = DateTime.Now;
            string dateString1 = loginDate.ToString("yyyy/MM/dd");
            //DateTime date = Convert.ToDateTime(dateString);

            var users = _usersRepository.GetUsersAllSV().Where(b => b.UserName != "ncs").Where(b =>  b.IsActive == true).OrderByDescending(c => c.LoginDate).OrderByDescending(o => o.IsLogin).ToList();
            List<UserIndexViewModel> list = new List<UserIndexViewModel>();

            foreach (var item in users)
            {
                var userViewModel = new UserIndexViewModel();
                {
                    userViewModel.ViewModelID = item.UserID;
                    userViewModel.UserID = item.UserID;
                    userViewModel.UserName = item.UserName;
                    userViewModel.Name = item.FirstName + " " + item.LastName;
                    userViewModel.UserPicture = item.UserPicture;
                    userViewModel.LoginDate = item.LoginDate;

                    string dateString2 = item.LoginDate?.ToString("yyyy/MM/dd");
                    userViewModel.IsLoginToday = "";
                    if (item.IsLogin == true)
                        userViewModel.IsLoginToday = "✔";
                };
                list.Add(userViewModel);
            }
            return Json(list);
        }

        [Route("Home/GridLoginUsersDataRead1/{userID}/{isArchive}")]
        public IActionResult GridLoginUsersDataRead1(int userID, bool isArchive)
        {
            var users = _usersRepository.GetUsersAllSV().Where(b => b.UserName != "ncs" && b.UserID != userID).Where(c => c.IsActive == true).OrderByDescending( o=> o.LoginDate).ToList();
            List<UserIndexViewModel> list = new List<UserIndexViewModel>();

            foreach (var item in users)
            {
                var userViewModel = new UserIndexViewModel();
                {
                    userViewModel.ViewModelID = item.UserID;
                    userViewModel.UserID = item.UserID;
                    userViewModel.UserName = item.UserName;
                    userViewModel.Name = item.FirstName + " " + item.LastName;
                    userViewModel.UserPicture = item.UserPicture;
                    userViewModel.DateOfBird = item.DateOfBird;

                    int counter = _usersChatRepository.GetUsersMessages(item.SelectedSchoolCode, item.UserID, userID, isArchive).Count();
                    userViewModel.UserMessageCounter = "";
                    if (counter > 0) userViewModel.UserMessageCounter = counter + "+";

                    int send = _usersChatRepository.GetUsersMessages(item.SelectedSchoolCode, userID, item.UserID, isArchive).Count();
                    userViewModel.userSendMessage = "";
                    if (send > 0) userViewModel.userSendMessage = "✔";
                };
                list.Add(userViewModel);
            }
            return Json(list);
        }

        [Route("Home/GridChatDataRead2/{userID}/{chatUserID}/{isArchive}")]
        public IActionResult GridChatDataRead2(int userID, int chatUserID, bool isArchive)
        {
            var user1 = _usersRepository.GetUser(userID);
            var user2 = _usersRepository.GetUser(chatUserID);
            var usersChat = _usersChatRepository.GetUsersChatAll(user1.SelectedSchoolCode, isArchive).Where(b => b.ChatUserID1 == userID && b.ChatUserID2 == chatUserID || b.ChatUserID1 == chatUserID && b.ChatUserID2 == userID).OrderBy(c => c.ChatDate).ToList();

            List<SchoolViewModel> list = new List<SchoolViewModel>();
            foreach (var item in usersChat)
            {
                var schoolViewModel = new SchoolViewModel();
                {
                    schoolViewModel.ChatID = item.ChatID;
                    schoolViewModel.UserID = item.UserID;
                    schoolViewModel.UserName = user1.UserName;
                    schoolViewModel.ChatUserID1 = item.ChatUserID1;
                    schoolViewModel.ChatUserID2 = item.ChatUserID2;

                    schoolViewModel.Messages1 = item.Messages;
                    schoolViewModel.Messages2 = item.Messages;
                    schoolViewModel.ChatDate = item.ChatDate;
                    schoolViewModel.ChatDate1 = item.ChatDate;
                    schoolViewModel.ChatDate2 = item.ChatDate;
                    schoolViewModel.UserPicture1 = user1.UserPicture;
                    schoolViewModel.UserPicture2 = user2.UserPicture;

                    schoolViewModel.IsRead = item.IsArchive;
                };
                list.Add(schoolViewModel);
            }
            return Json(list);
        }

        [Route("Home/SendMessage/{schoolID}/{userID}/{chatUserID}/{messageText}")]
        public IActionResult SendMessage(int schoolID, int userID, int chatUserID, string messageText)
        {
            var user = _usersRepository.GetUser(userID);

            UsersChat chat = new UsersChat();
            chat.ChatID = 0;
            chat.SchoolCode = user.SelectedSchoolCode;
            chat.UserID = userID;
            chat.ChatUserID1 = userID;
            chat.Messages = messageText;
            chat.ChatUserID2 = chatUserID;
            chat.ChatDate = DateTime.Now;
            chat.IsArchive = false;

            _usersChatRepository.CreateUsersChat(chat);

            return Json(chat);
        }

        [Route("Home/PostArchive/{userID}/{chatUserID}")]
        public IActionResult PostArchive(int userID, int chatUserID)
        {
            var user = _usersRepository.GetUser(userID);
            var usersChat = _usersChatRepository.GetUsersChatAll(user.SelectedSchoolCode, false).Where(b => b.SchoolCode == user.SelectedSchoolCode && b.ChatUserID1 == userID && b.ChatUserID2 == chatUserID || b.ChatUserID1 == chatUserID && b.ChatUserID2 == userID).OrderBy(c => c.ChatDate).ToList();

            foreach (var item in usersChat)
            {
                item.IsArchive = true;
                _usersChatRepository.UpdateUsersChat(item);
            }
            return Json(usersChat);
        }

        [Route("Home/DeleteArchive/{userID}/{chatUserID}")]
        public IActionResult DeleteArchive(int userID, int chatUserID)
        {
            var user = _usersRepository.GetUser(userID);
            var usersChat = _usersChatRepository.GetUsersChatAll(user.SelectedSchoolCode, true).Where(b => b.ChatUserID1 == userID && b.ChatUserID2 == chatUserID || b.ChatUserID1 == chatUserID && b.ChatUserID2 == userID).OrderBy(c => c.ChatDate).ToList();

            foreach (var item in usersChat)
            {
                _usersChatRepository.DeleteUsersChat(item);
            }
            return Json(usersChat);
        }

        [Route("Home/DeleteMessages/{userID}/{chatUserID}")]
        public IActionResult DeleteMessages(int userID, int chatUserID)
        {
            var user = _usersRepository.GetUser(userID);
            var usersChat = _usersChatRepository.GetUsersChatAll(user.SelectedSchoolCode, false).Where(b => b.ChatUserID1 == userID && b.ChatUserID2 == chatUserID || b.ChatUserID1 == chatUserID && b.ChatUserID2 == userID).OrderBy(c => c.ChatDate).ToList();

            foreach (var item in usersChat)
            {
                _usersChatRepository.DeleteUsersChat(item);
            }
            return Json(usersChat);
        }
    }
}
