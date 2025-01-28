using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ESCHOOL.Controllers
{
    public class M900UsersController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentDebtDetailTableRepository _studentDebtDetailTableRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IStudentNoteRepository _studentNoteRepository;
        IStudentTempRepository _studentTempRepository;
        ITempDataRepository _tempDataRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IBankRepository _bankRepository;
        IClassroomRepository _classroomRepository;
        IDiscountTableRepository _discountTableRepository;
        IStudentDiscountRepository _studentDiscountRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IParameterRepository _parameterRepository;
        ISchoolBusServicesRepository _schoolBusServicesRepository;

        IAccountCodesRepository _accountCodesRepository;
        IAccountingRepository _accountingRepository;

        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IUsersLogRepository _usersLogRepository;
        IUsersTaskDataSourceRepository _usersTaskDataSourceRepository;
        IWebHostEnvironment _hostEnvironment;
        public M900UsersController(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDebtDetailTableRepository studentDebtDetailTableRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            IStudentPaymentRepository studentPaymentRepository,

            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,
            IStudentNoteRepository studentNoteRepository,
            IStudentTempRepository studentTempRepository,
            ITempDataRepository tempDataRepository,
            IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
            IBankRepository bankRepository,
            IClassroomRepository classroomRepository,
            IDiscountTableRepository discountTableRepository,
            IStudentDiscountRepository studentDiscountRepository,
            IPSerialNumberRepository pSerialNumberRepository,
            ISchoolFeeRepository schoolFeeRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IParameterRepository parameterRepository,
            ISchoolBusServicesRepository schoolBusServicesRepository,

            IAccountCodesRepository accountCodesRepository,
            IAccountingRepository accountingRepository,

            IUsersRepository usersRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            IUsersTaskDataSourceRepository usersTaskDataSourceRepository,
            IUsersLogRepository usersLogRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentDebtDetailTableRepository = studentDebtDetailTableRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _discountTableRepository = discountTableRepository;
            _studentDiscountRepository = studentDiscountRepository;
            _studentPaymentRepository = studentPaymentRepository;

            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _studentNoteRepository = studentNoteRepository;
            _studentTempRepository = studentTempRepository;
            _tempDataRepository = tempDataRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _bankRepository = bankRepository;
            _classroomRepository = classroomRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _parameterRepository = parameterRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;

            _accountCodesRepository = accountCodesRepository;
            _accountingRepository = accountingRepository;

            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _usersLogRepository = usersLogRepository;
            _usersTaskDataSourceRepository = usersTaskDataSourceRepository;
            _hostEnvironment = hostEnvironment;
        }

        #region index Students
        public IActionResult index(int prgUserID)
        {
            var user = _usersRepository.GetUser(prgUserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);
            TempData["isSuperVisor"] = user.IsSupervisor;

            var studentIndexViewModel = new StudentIndexViewModel
            {
                ViewModelID = prgUserID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                SchoolName = school.CompanyName,
                IsActive = true,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(studentIndexViewModel);
        }

        #endregion

        #region Users
        [HttpGet]
        public IActionResult AddOrEditUsers(int id, int prgUserID)
        {
            var user = new Users();
            if (id > 0)
                user = _usersRepository.GetUser(id);

            var prgUser = _usersRepository.GetUser(prgUserID);
            var users = _usersRepository.GetUsersAll();

            var period = prgUser.UserPeriod;
            string date = string.Format("{0:dd/MM/yyyy}", prgUser.UserDate);
            var school = _schoolInfoRepository.GetSchoolInfo(prgUser.SchoolID);
            if (id == 0)
            {
                user.UserPicture = "male.jpg";
                user.UserViewPicture = prgUser.UserViewPicture;
                user.SchoolID = user.SchoolID;
                user.GenderTypeCategoryID = prgUser.GenderTypeCategoryID;
                user.IsActive = true;
                user.IsSupervisor = false;

                TimeSpan ts1 = new TimeSpan(08, 30, 00);
                user.UserShiftFrom = ts1;
                TimeSpan ts2 = new TimeSpan(18, 00, 00);
                user.UserShiftTo = ts2;

                user.SchoolID = prgUser.SchoolID;
                user.UserDate = DateTime.Now;
                user.IsLogin = false;
                user.LoginDate = DateTime.Now;
                user.UserPeriod = prgUser.UserPeriod;
                user.SelectedTheme = prgUser.SelectedTheme;
                user.SelectedSchoolCode = prgUser.SelectedSchoolCode;
                user.SelectedCulture = "tr-TR";
                user.SelectedLanguage = prgUser.SelectedLanguage;
                user.SortOrder = users.Max(a => a.SortOrder) + 1;
                _usersRepository.CreateUsers(user);

                TempData["userID"] = user.UserID;
                id = user.UserID;

            }

            if (user.UserPicture == null || user.UserPicture == "")
                user.UserPicture = "male.jpg";
            if (user.UserViewPicture == null || user.UserViewPicture == "")
                user.UserViewPicture = "default.jpg";

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(prgUser.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(prgUser.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(prgUser.SelectedTheme);
            TempData["isSuperVisor"] = prgUser.IsSupervisor;

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var userViewModel = new UserViewModel
            {
                UserID = id,
                ViewModelID = prgUserID,
                UserPasswordTmp = user.UserPassword,
                Users = user,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName,
            };
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditUsers(UserViewModel userViewModel, IFormFile imgfileUser, IFormFile imgfileUserTopView, IFormFile imgfileUserBottomView)
        {
            if (userViewModel.Users.UserID == 0)
            {
                var gender = _parameterRepository.GetParameter(userViewModel.Users.GenderTypeCategoryID);
                userViewModel.Users.GenderTypeCategoryID = gender.CategoryID;
                if (gender.CategoryName == "Erkek" || gender.CategoryName == "Male")
                    userViewModel.Users.UserPicture = "male.jpg";
                else userViewModel.Users.UserPicture = "female.jpg";
            }

            //Önceki dosya siliniyor
            var pathUsers = Path.Combine("Upload", "Users");
            if (imgfileUser != null && ModelState.IsValid)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, pathUsers, userViewModel.Users.UserPicture);
                if (System.IO.File.Exists(imagePath))
                    if (userViewModel.Users.UserPicture != "male.jpg" && userViewModel.Users.UserPicture != "female.jpg")
                        System.IO.File.Delete(imagePath);
            }

            if (imgfileUser != null && ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imgfileUser.FileName);
                string extension = Path.GetExtension(imgfileUser.FileName);

                var uniqFileName = Guid.NewGuid().ToString();
                userViewModel.Users.UserPicture = fileName = Path.GetFileName(uniqFileName + "-" + fileName.ToLower() + extension);
                int lenght = userViewModel.Users.UserPicture.Length;
                if (lenght > 100) userViewModel.Users.UserPicture = fileName = Path.GetFileName(uniqFileName + extension);

                pathUsers = Path.Combine(wwwRootPath + "/Upload/Users", fileName);
                using (var fileStream = new FileStream(pathUsers, FileMode.Create))
                {
                    await imgfileUser.CopyToAsync(fileStream);
                }
            }

            //imgfileUserTopView
            var pathUsersTopView = Path.Combine("Upload", "Users");
            if (imgfileUserTopView != null && ModelState.IsValid)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, pathUsersTopView, userViewModel.Users.UserViewPicture);
                if (System.IO.File.Exists(imagePath))
                    if (userViewModel.Users.UserPicture != "default.jpg")
                        System.IO.File.Delete(imagePath);
            }

            if (imgfileUserTopView != null && ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imgfileUserTopView.FileName);
                string extension = Path.GetExtension(imgfileUserTopView.FileName);

                var uniqFileName = Guid.NewGuid().ToString();
                userViewModel.Users.UserViewPicture = fileName = Path.GetFileName(uniqFileName + "-" + fileName.ToLower() + extension);
                int lenght = userViewModel.Users.UserViewPicture.Length;
                if (lenght > 100) userViewModel.Users.UserViewPicture = fileName = Path.GetFileName(uniqFileName + extension);

                pathUsersTopView = Path.Combine(wwwRootPath + "/Upload/Users", fileName);
                using (var fileStream = new FileStream(pathUsersTopView, FileMode.Create))
                {
                    await imgfileUserTopView.CopyToAsync(fileStream);
                }
            }
            if (userViewModel.Users.UserPicture == null || userViewModel.Users.UserPicture == "")
                userViewModel.Users.UserPicture = "default.jpg";

            if (userViewModel.Users.UserName == null)
            {
                userViewModel.Users.UserName = userViewModel.Users.FirstName;
                userViewModel.Users.UserPassword = "123";
            }
            if (userViewModel.Users.UserPassword == null && userViewModel.UserPasswordTmp != null)
            {
                userViewModel.Users.UserPassword = userViewModel.UserPasswordTmp;
            }

            var categoryName = _parameterRepository.GetParameter(userViewModel.Users.SelectedLanguage).CategoryName;
            userViewModel.Users.SelectedCulture = "tr-TR";
            if (categoryName == "İngilizce") userViewModel.Users.SelectedCulture = "en-US";

            _usersRepository.UpdateUsers(userViewModel.Users);

            //////Users Log//////////////////
            int transactionID = 0;
            string userLogDescription = "";
            string culture = "tr-TR";
            if (userViewModel.Users != null)
            {
                culture = userViewModel.Users.SelectedCulture.Trim();
                transactionID = _parameterRepository.GetParameterCategoryName("Kullanıcı Bilgileri").CategoryID;
                if (culture == "tr-TR")
                {
                    userLogDescription = "Kullanıcı bilgilerinde değişiklik yapıldı.";
                }
                if (culture == "en-US")
                {
                    userLogDescription = "User information has been changed.";
                }
            }

            var log = new UsersLog();
            log.SchoolID = userViewModel.Users.SchoolID;
            log.Period = userViewModel.Users.UserPeriod;
            log.UserID = userViewModel.Users.UserID;
            log.TransactionID = transactionID;
            log.UserLogDate = DateTime.Now;
            log.UserLogDescription = userLogDescription;
            _usersLogRepository.CreateUsersLog(log);
            ////////////////////////////////


            string url = "/M900Users/index?prgUserID=" + userViewModel.ViewModelID;
            return Redirect(url);
        }
        #endregion

        #region Grid

        [Route("M900Users/GridUsersDataRead/{prgUserID}")]
        public IActionResult GridUsersDataRead(int prgUserID)
        {
            var users = _usersRepository.GetUsersAllSV().Where(b => b.UserName != "ncs").ToList();
            List<UserIndexViewModel> list = new List<UserIndexViewModel>();
            foreach (var item in users)
            {
                if (item.FirstName == null && item.LastName == null)
                {
                    _usersRepository.DeleteUsers(item);
                }
                else
                {
                    var userViewModel = new UserIndexViewModel();
                    {
                        userViewModel.ViewModelID = item.UserID;
                        userViewModel.UserID = item.UserID;
                        userViewModel.Name = item.FirstName + " " + item.LastName;
                        userViewModel.UserPicture = item.UserPicture;
                        userViewModel.DateOfBird = item.DateOfBird;
                        userViewModel.HomePhone = item.HomePhone;
                        userViewModel.MobilePhone = item.MobilePhone;
                        userViewModel.IsActive = item.IsActive;
                        userViewModel.IsSupervisor = item.IsSupervisor;
                    };
                    list.Add(userViewModel);
                }
            }
            return Json(list);
        }

        [Route("M900Users/UserPermissionsDataRead/{userID}/{isSupervisor}")]
        public IActionResult UserPermissionsDataRead(int userID, bool isSupervisor)
        {
            var user = _usersRepository.GetUser(userID);
            string culture = user.SelectedCulture.Trim();

            var categoryID = _parameterRepository.GetParameterCategoryName("Kullanıcı İzinleri").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);
            var userParameter = _usersWorkAreasRepository.GetUserWorkAreasID(userID).Where(b => b.IsSchool == false);

            List<UsersWorkAreaModel> list = new List<UsersWorkAreaModel>();
            if (userParameter.Count() == 0)
            {
                foreach (var item1 in parameter)
                {
                    var param = parameter.FirstOrDefault(p => p.CategoryID == item1.CategoryID);
                    if (parameter == null)
                    {
                        param = parameter.FirstOrDefault(p => p.CategorySubID == categoryID);
                    }

                    var usersWorkAreas = new UsersWorkAreas();
                    usersWorkAreas.UsersWorkAreaID = 0;
                    usersWorkAreas.UserID = userID;
                    usersWorkAreas.CategoryID = item1.CategoryID;
                    usersWorkAreas.IsSelect = true;
                    usersWorkAreas.IsDirtySelect = false;
                    var usersWorkAreaID = usersWorkAreas.UsersWorkAreaID;
                    var usersWorkAreaModel = new UsersWorkAreaModel
                    {
                        ViewModelID = item1.CategoryID,
                        UsersWorkAreaID = usersWorkAreaID,
                        UserID = userID,
                        CategoryID = item1.CategoryID,
                        IsSelect = true,
                        IsDirtySelect = false,
                        Parameter = param,
                    };
                    list.Add(usersWorkAreaModel);
                }
            }
            else
            {
                if (isSupervisor)
                {
                    foreach (var item2 in userParameter)
                    {
                        var param = parameter.FirstOrDefault(p => p.CategoryID == item2.CategoryID);
                        if (parameter == null)
                        {
                            param = parameter.FirstOrDefault(p => p.CategorySubID == categoryID);
                        }
                        var usersWorkAreaModel = new UsersWorkAreaModel
                        {
                            ViewModelID = item2.CategoryID,
                            UsersWorkAreaID = item2.UsersWorkAreaID,
                            UserID = userID,
                            CategoryID = item2.CategoryID,
                            IsSelect = item2.IsSelect,

                            Parameter = param,
                        };
                        list.Add(usersWorkAreaModel);
                    }
                }
                else
                {
                    foreach (var item2 in userParameter)
                    {
                        if (item2.IsSelect)
                        {
                            var param = parameter.FirstOrDefault(p => p.CategoryID == item2.CategoryID);
                            if (parameter == null)
                            {
                                param = parameter.FirstOrDefault(p => p.CategorySubID == categoryID);
                            }
                            var usersWorkAreaModel = new UsersWorkAreaModel
                            {
                                ViewModelID = item2.CategoryID,
                                UsersWorkAreaID = item2.UsersWorkAreaID,
                                UserID = userID,
                                CategoryID = item2.CategoryID,
                                IsSelect = item2.IsSelect,

                                Parameter = param,
                            };
                            list.Add(usersWorkAreaModel);
                        }
                    }
                }

            }
            return Json(list);
        }

        [Route("M900Users/UserShoolPermissionsDataRead/{userID}/{isSupervisor}")]
        public IActionResult UserShoolPermissionsDataRead(int userID, bool isSupervisor)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfoAllTrue();
            string culture = user.SelectedCulture.Trim();

            var categoryID = _parameterRepository.GetParameterCategoryName("Kullanıcı İzinleri").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);
            var userParameter = _usersWorkAreasRepository.GetUserWorkAreasID(userID).Where(b => b.IsSchool == true);
            bool isRead = false;
            foreach (var item in userParameter)
            {
                if (item.IsSchool)
                {
                    var sc = _schoolInfoRepository.GetSchoolInfo(item.CategoryID);
                    if (sc == null)
                    {
                        _usersWorkAreasRepository.DeleteUsersWorkAreas(item);
                        isRead = true;
                    }
                }
                if (isRead) userParameter = _usersWorkAreasRepository.GetUserWorkAreasID(userID).Where(b => b.IsSchool == true);
            }

            List<UsersWorkAreaModel> list = new List<UsersWorkAreaModel>();
            if (userParameter.Count() == 0)
            {
                foreach (var item1 in school)
                {
                    var usersWorkAreas = new UsersWorkAreas();
                    usersWorkAreas.UsersWorkAreaID = 0;
                    usersWorkAreas.UserID = userID;
                    usersWorkAreas.CategoryID = item1.SchoolID;
                    usersWorkAreas.IsSelect = true;
                    usersWorkAreas.IsDirtySelect = false;
                    var usersWorkAreaID = usersWorkAreas.UsersWorkAreaID;
                    var usersWorkAreaModel = new UsersWorkAreaModel
                    {
                        ViewModelID = item1.SchoolID,
                        UsersWorkAreaID = usersWorkAreaID,
                        UserID = userID,
                        CategoryID = item1.SchoolID,
                        CategoryName = item1.CompanyName,
                        IsSelect = true,
                        IsDirtySelect = false,
                    };
                    list.Add(usersWorkAreaModel);
                }
            }
            else
            {
                if (isSupervisor)
                {
                    foreach (var item2 in userParameter)
                    {
                        var usersWorkAreaModel = new UsersWorkAreaModel
                        {
                            ViewModelID = item2.CategoryID,
                            UsersWorkAreaID = item2.UsersWorkAreaID,
                            CategoryName = _schoolInfoRepository.GetSchoolInfo(item2.CategoryID).CompanyName,
                            UserID = userID,
                            CategoryID = item2.CategoryID,
                            IsSelect = item2.IsSelect,
                        };
                        list.Add(usersWorkAreaModel);
                    }
                }
                else
                {
                    foreach (var item2 in userParameter)
                    {
                        var usersWorkAreaModel = new UsersWorkAreaModel
                        {
                            ViewModelID = item2.CategoryID,
                            UsersWorkAreaID = item2.UsersWorkAreaID,
                            CategoryName = _schoolInfoRepository.GetSchoolInfo(item2.CategoryID).CompanyName,
                            UserID = userID,
                            CategoryID = item2.CategoryID,
                            IsSelect = item2.IsSelect,
                        };
                        list.Add(usersWorkAreaModel);
                    }
                }
            }
            return Json(list);
        }

        [Route("M900Users/UserPermissionsUpdate/{strResult}/{userID}")]
        public IActionResult UserPermissionsUpdate([Bind(Prefix = "models")] string strResult, int userID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<UsersWorkAreaModel>>(strResult);
            List<UsersWorkAreas> list = new List<UsersWorkAreas>();

            var i = 0;
            foreach (var item in json)
            {
                var usersWorkAreas = new UsersWorkAreas();
                if (item.UsersWorkAreaID == 0)
                {
                    usersWorkAreas.UsersWorkAreaID = 0;
                    usersWorkAreas.UserID = userID;
                    usersWorkAreas.CategoryID = item.CategoryID;
                    usersWorkAreas.IsSchool = false;
                    usersWorkAreas.IsSelect = true;

                    list.Add(usersWorkAreas);

                    if (ModelState.IsValid)
                    {
                        _usersWorkAreasRepository.CreateUsersWorkAreas(usersWorkAreas);
                    }
                }
                else
                {
                    usersWorkAreas.UsersWorkAreaID = item.UsersWorkAreaID;
                    usersWorkAreas.UserID = item.UserID;
                    usersWorkAreas.CategoryID = item.CategoryID;
                    usersWorkAreas.IsSchool = false;
                    usersWorkAreas.IsSelect = item.IsSelect;

                    list.Add(usersWorkAreas);

                    if (ModelState.IsValid)
                    {
                        _usersWorkAreasRepository.UpdateUsersWorkAreas(usersWorkAreas);
                    }
                }

                i += 1;
            }
            return Json(list);
        }


        [Route("M900Users/UserSchoolPermissionsUpdate/{strResult}/{userID}")]
        public IActionResult UserSchoolPermissionsUpdate([Bind(Prefix = "models")] string strResult, int userID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<UsersWorkAreaModel>>(strResult);
            List<UsersWorkAreas> list = new List<UsersWorkAreas>();

            var i = 0;
            foreach (var item in json)
            {
                var usersWorkAreas = new UsersWorkAreas();
                if (item.UsersWorkAreaID == 0)
                {
                    usersWorkAreas.UsersWorkAreaID = 0;
                    usersWorkAreas.UserID = userID;
                    usersWorkAreas.CategoryID = item.CategoryID;
                    usersWorkAreas.IsSchool = true;
                    usersWorkAreas.IsSelect = true;

                    list.Add(usersWorkAreas);

                    if (ModelState.IsValid)
                    {
                        _usersWorkAreasRepository.CreateUsersWorkAreas(usersWorkAreas);
                    }
                }
                else
                {
                    usersWorkAreas.UsersWorkAreaID = item.UsersWorkAreaID;
                    usersWorkAreas.UserID = item.UserID;
                    usersWorkAreas.CategoryID = item.CategoryID;
                    usersWorkAreas.IsSchool = true;
                    usersWorkAreas.IsSelect = item.IsSelect;

                    list.Add(usersWorkAreas);

                    if (ModelState.IsValid)
                    {
                        _usersWorkAreasRepository.UpdateUsersWorkAreas(usersWorkAreas);
                    }
                }

                i += 1;
            }
            return Json(list);
        }

        //Delete in Index
        public IActionResult GridUserDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Users>>(strResult);

            var id = json[0].UserID;
            var user = _usersRepository.GetUser(id);

            if (user == null)
                return Json(true);

            //Önceki dosya siliniyor
            var pathUsers = Path.Combine("Upload", "Users");

            var imagePath1 = Path.Combine(_hostEnvironment.WebRootPath, pathUsers, user.UserPicture);
            if (System.IO.File.Exists(imagePath1))
                if (user.UserPicture != "male.jpg" && user.UserPicture != "female.jpg")
                    System.IO.File.Delete(imagePath1);


            var path = Path.Combine("Upload", "Images"); // Bir alt klasör için 
                                                         //var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "/Upload/Images", schoolInfo.LogoName);
            var imagePath2 = Path.Combine(_hostEnvironment.WebRootPath, path, user.UserViewPicture);
            if (System.IO.File.Exists(imagePath2))
                if (user.UserViewPicture != "default.jpg")
                    System.IO.File.Delete(imagePath2);

            _usersRepository.DeleteUsers(user);

            var work = _usersWorkAreasRepository.GetUserWorkAreasID(id);
            foreach (var item in work)
            {
                _usersWorkAreasRepository.DeleteUsersWorkAreas(item);
            }

            return Json(true);
        }

        [Route("M900Users/GridDataDeleteNew/{userID}")]
        public IActionResult GridDataDeleteNew(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            _usersRepository.DeleteUsers(user);

            return Json(true);
        }

        #endregion

        #region Combo

        public IActionResult WorkAreaDropDown()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Kullanıcı İzinleri").CategoryID;
            var permissions = _parameterRepository.GetParameterSubID(categoryID);
            return Json(permissions);
        }
        public IActionResult BloodGroupTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Kan Grubu").CategoryID;
            var bloodGroupType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(bloodGroupType);
        }
        public IActionResult NationalityTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Uyruğu").CategoryID;
            var nationalityType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(nationalityType);
        }
        public IActionResult ReligiousTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Dini").CategoryID;
            var religiousType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(religiousType);
        }
        public IActionResult CityCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("İl ve İlçeler").CategoryID;
            var city = _parameterRepository.GetParameterSubID(categoryID);
            return Json(city);
        }

        public IActionResult TownCombo(int id)
        {
            var town = _parameterRepository.GetParameterSubID(id);
            return Json(town);
        }

        public IActionResult LanguageTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Dil").CategoryID;
            var languageType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(languageType);
        }
        #endregion

        #region Scheduler
        public IActionResult UsersTask(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var schoolName = "";
            if (user.SchoolID > 0)
                schoolName = _schoolInfoRepository.GetSchoolInfo(user.SchoolID).CompanyName;
            else
            {
                user.SchoolID = 1;
                schoolName = _schoolInfoRepository.GetSchoolInfo(user.SchoolID).CompanyName;
            }
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            TempData["SchoolName"] = schoolName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            ViewBag.ClassroomEmpty = false;
            ViewBag.FeeEmpty = false;
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            if (classroom.Count() > 0) ViewBag.ClassroomEmpty = true;
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTableAll(user.SchoolID, user.UserPeriod);
            if (schoolFeeTable.Count() > 0) ViewBag.FeeEmpty = true;

            var categoryID = _parameterRepository.GetParameterCategoryName("Kullanıcı İşaretleyicileri").CategoryID;
            var color = _parameterRepository.GetParameterSubID(categoryID);
            int inx = _parameterRepository.GetParameterSubID(categoryID).First().CategoryID;

            var taskViewModel = new TaskViewModel
            {
                SchoolID = user.SchoolID,
                UserID = userID,
                StudentID = 0,
                //Period = user.UserPeriod,
                //SchoolName = schoolName,
                SelectedCulture = user.SelectedCulture.Trim(),
                ResourceDefaultValue = inx,
            };
            int i = 0;
            foreach (var item in color)
            {
                i++;
                if (i == 01) { taskViewModel.Color01Name = item.CategoryName; taskViewModel.Color01Value = item.CategoryID; taskViewModel.Color01 = item.Color; };
                if (i == 02) { taskViewModel.Color02Name = item.CategoryName; taskViewModel.Color02Value = item.CategoryID; taskViewModel.Color02 = item.Color; };
                if (i == 03) { taskViewModel.Color03Name = item.CategoryName; taskViewModel.Color03Value = item.CategoryID; taskViewModel.Color03 = item.Color; };
                if (i == 04) { taskViewModel.Color04Name = item.CategoryName; taskViewModel.Color04Value = item.CategoryID; taskViewModel.Color04 = item.Color; };
                if (i == 05) { taskViewModel.Color05Name = item.CategoryName; taskViewModel.Color05Value = item.CategoryID; taskViewModel.Color05 = item.Color; };
                if (i == 06) { taskViewModel.Color06Name = item.CategoryName; taskViewModel.Color06Value = item.CategoryID; taskViewModel.Color06 = item.Color; };
                if (i == 07) { taskViewModel.Color07Name = item.CategoryName; taskViewModel.Color07Value = item.CategoryID; taskViewModel.Color07 = item.Color; };
                if (i == 08) { taskViewModel.Color08Name = item.CategoryName; taskViewModel.Color08Value = item.CategoryID; taskViewModel.Color08 = item.Color; };
                if (i == 09) { taskViewModel.Color09Name = item.CategoryName; taskViewModel.Color09Value = item.CategoryID; taskViewModel.Color09 = item.Color; };
                if (i == 10) { taskViewModel.Color10Name = item.CategoryName; taskViewModel.Color10Value = item.CategoryID; taskViewModel.Color10 = item.Color; };

                if (i == 11) { taskViewModel.Color11Name = item.CategoryName; taskViewModel.Color11Value = item.CategoryID; taskViewModel.Color11 = item.Color; };
                if (i == 12) { taskViewModel.Color12Name = item.CategoryName; taskViewModel.Color12Value = item.CategoryID; taskViewModel.Color12 = item.Color; };
                if (i == 13) { taskViewModel.Color13Name = item.CategoryName; taskViewModel.Color13Value = item.CategoryID; taskViewModel.Color13 = item.Color; };
                if (i == 14) { taskViewModel.Color14Name = item.CategoryName; taskViewModel.Color14Value = item.CategoryID; taskViewModel.Color14 = item.Color; };
                if (i == 15) { taskViewModel.Color15Name = item.CategoryName; taskViewModel.Color15Value = item.CategoryID; taskViewModel.Color15 = item.Color; };
                if (i == 16) { taskViewModel.Color16Name = item.CategoryName; taskViewModel.Color16Value = item.CategoryID; taskViewModel.Color16 = item.Color; };
                if (i == 17) { taskViewModel.Color17Name = item.CategoryName; taskViewModel.Color17Value = item.CategoryID; taskViewModel.Color17 = item.Color; };
                if (i == 18) { taskViewModel.Color18Name = item.CategoryName; taskViewModel.Color18Value = item.CategoryID; taskViewModel.Color18 = item.Color; };
                if (i == 19) { taskViewModel.Color19Name = item.CategoryName; taskViewModel.Color19Value = item.CategoryID; taskViewModel.Color19 = item.Color; };
                if (i == 20) { taskViewModel.Color20Name = item.CategoryName; taskViewModel.Color20Value = item.CategoryID; taskViewModel.Color20 = item.Color; };
            }

            if (taskViewModel.Color11Name != null) taskViewModel.Type11 = "checkbox"; else taskViewModel.Type11 = "hidden";
            if (taskViewModel.Color12Name != null) taskViewModel.Type12 = "checkbox"; else taskViewModel.Type12 = "hidden";
            if (taskViewModel.Color13Name != null) taskViewModel.Type13 = "checkbox"; else taskViewModel.Type13 = "hidden";
            if (taskViewModel.Color14Name != null) taskViewModel.Type14 = "checkbox"; else taskViewModel.Type14 = "hidden";
            if (taskViewModel.Color15Name != null) taskViewModel.Type15 = "checkbox"; else taskViewModel.Type15 = "hidden";
            if (taskViewModel.Color16Name != null) taskViewModel.Type16 = "checkbox"; else taskViewModel.Type16 = "hidden";
            if (taskViewModel.Color17Name != null) taskViewModel.Type17 = "checkbox"; else taskViewModel.Type17 = "hidden";
            if (taskViewModel.Color18Name != null) taskViewModel.Type18 = "checkbox"; else taskViewModel.Type18 = "hidden";
            if (taskViewModel.Color19Name != null) taskViewModel.Type19 = "checkbox"; else taskViewModel.Type19 = "hidden";
            if (taskViewModel.Color20Name != null) taskViewModel.Type20 = "checkbox"; else taskViewModel.Type20 = "hidden";

            return View(taskViewModel);
        }

        [Route("M900Users/UsersTaskDataRead/{schoolID}/{userID}")]
        public IActionResult UsersTaskDataRead(int schoolID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var task = _usersTaskDataSourceRepository.GetUsersTaskAll(schoolID, userID);
            List<TaskViewModel> list = new List<TaskViewModel>();

            foreach (var item in task)
            {
                var taskViewModel = new TaskViewModel
                {
                    ViewModelID = item.TaskID,
                    TaskID = item.TaskID,
                    SchoolID = item.SchoolID,
                    UserID = item.UserID,
                    Title = item.Title,
                    Start = item.Start.ToLocalTime(),
                    End = item.End.ToLocalTime(),

                    Description = item.Description,
                    RecurrenceId = item.RecurrenceId,
                    RecurrenceRule = item.RecurrenceRule,
                    RecurrenceException = item.RecurrenceException,
                    IsAllDay = item.IsAllDay,
                    OwnerID = item.OwnerID,
                    SelectedCulture = user.SelectedCulture.Trim(),
                };
                list.Add(taskViewModel);
            }
            return Json(list);
        }


        [Route("M900Users/UsersTaskDataCreate/{strResult}/{schoolID}/{userID}")]
        public IActionResult UsersTaskDataCreate([Bind(Prefix = "models")] string strResult, int schoolID, int userID)
        {
            //int schoolID = 1;
            //int studentID = 2;
            var json = new JavaScriptSerializer().Deserialize<List<UsersTaskDataSource>>(strResult);
            List<UsersTaskDataSource> list = new List<UsersTaskDataSource>();
            var task = new UsersTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                task = new UsersTaskDataSource();
                task.TaskID = 0;
                task.SchoolID = schoolID;
                task.UserID = userID;
                task.Title = json[i].Title;
                task.Start = json[i].Start;
                task.End = json[i].End;

                task.Description = json[i].Description;
                task.RecurrenceId = json[i].RecurrenceId;
                task.RecurrenceRule = json[i].RecurrenceRule;
                task.RecurrenceException = json[i].RecurrenceException;
                task.IsAllDay = json[i].IsAllDay;
                task.OwnerID = json[i].OwnerID;

                if (ModelState.IsValid)
                {
                    _usersTaskDataSourceRepository.CreateUsersTask(task);
                }
                i = i + 1;
            }
            return Json(task);
        }
        public IActionResult UsersTaskDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<UsersTaskDataSource>>(strResult);

            UsersTaskDataSource list = new UsersTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                var task = _usersTaskDataSourceRepository.GetUsersTask(json[i].SchoolID, json[i].UserID, json[i].TaskID);

                task.TaskID = json[i].TaskID;
                task.SchoolID = json[i].SchoolID;
                task.UserID = json[i].UserID;
                task.Title = json[i].Title;
                task.Start = json[i].Start;
                task.End = json[i].End;

                task.Description = json[i].Description;
                task.RecurrenceId = json[i].RecurrenceId;
                task.RecurrenceRule = json[i].RecurrenceRule;
                task.RecurrenceException = json[i].RecurrenceException;
                task.IsAllDay = json[i].IsAllDay;
                task.OwnerID = json[i].OwnerID;
                if (ModelState.IsValid)
                {
                    _usersTaskDataSourceRepository.UpdateUsersTask(task);
                }
                i = i + 1;
            }

            return Json(list);
        }
        [HttpPost]
        public IActionResult UsersTaskDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<UsersTaskDataSource>>(strResult);

            UsersTaskDataSource list = new UsersTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                var task = _usersTaskDataSourceRepository.GetUsersTask(json[i].SchoolID, json[i].UserID, json[i].TaskID);

                task.TaskID = json[i].TaskID;
                task.SchoolID = json[i].SchoolID;
                task.UserID = json[i].UserID;
                task.Title = json[i].Title;
                task.Start = json[i].Start;
                task.End = json[i].End;

                task.Description = json[i].Description;
                task.RecurrenceId = json[i].RecurrenceId;
                task.RecurrenceRule = json[i].RecurrenceRule;
                task.RecurrenceException = json[i].RecurrenceException;
                task.IsAllDay = json[i].IsAllDay;
                task.OwnerID = json[i].OwnerID;
                if (ModelState.IsValid)
                {
                    _usersTaskDataSourceRepository.DeleteUsersTask(task);
                }
                i = i + 1;
            }

            return Json(list);
        }

        public IActionResult ResourceTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Kullanıcı İşaretleyicileri").CategoryID;
            var user = _parameterRepository.GetParameterSubID(categoryID);
            return Json(user);
        }
        #endregion
    }
}