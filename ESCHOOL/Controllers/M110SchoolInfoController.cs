using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using System.Globalization;
using System.Threading;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ESCHOOL.Controllers
{
    public class M110SchoolInfoController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IClassroomRepository _classroomRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IParameterRepository _parameterRepository;
        IBankRepository _bankRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IUsersTaskDataSourceRepository _usersTaskDataSourceRepository;
        ISchoolsTaskDataSourceRepository _schoolsTaskDataSourceRepository;
        IWebHostEnvironment _hostEnvironment;

        public M110SchoolInfoController(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IClassroomRepository classroomRepository,
            IPSerialNumberRepository pSerialNumberRepository,
            ISchoolFeeRepository schoolFeeRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IParameterRepository parameterRepository,
            IBankRepository bankRepository,
            IUsersRepository usersRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            IUsersTaskDataSourceRepository usersTaskDataSourceRepository,
            ISchoolsTaskDataSourceRepository schoolsTaskDataSourceRepository,
            IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _classroomRepository = classroomRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _parameterRepository = parameterRepository;
            _bankRepository = bankRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _usersTaskDataSourceRepository = usersTaskDataSourceRepository;
            _schoolsTaskDataSourceRepository = schoolsTaskDataSourceRepository;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult index(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            //var categoryID = _parameterRepository.GetParameterCategoryName("Kurum Bilgileri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.ExistUsersWorkAreas(user.SchoolID);


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

            var schoolViewModel = new SchoolViewModel
            {
                IsPermission = isPermission,
                UserID = userID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),
                SelectedTheme = user.SelectedTheme,
            };
            return View(schoolViewModel);
        }

        [Route("M110SchoolInfo/GridSchoolDataRead/{userID}")]
        public IActionResult GridSchoolDataRead(int userID)
        {
            var usersWork = _usersWorkAreasRepository.GetUserWorkAreasID(userID).Where(b => b.IsSchool == true && b.IsSelect == true);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfoAll();
            var selectedSchool = schoolInfo.Where(s => usersWork.Where(p => p.CategoryID == s.SchoolID).Count() > 0).ToList();

            bool isProtected = false;
            List<SchoolViewModel> list = new List<SchoolViewModel>();
            foreach (var item in selectedSchool)
            {
                var students = _studentRepository.GetStudentAllPeriod(item.SchoolID).FirstOrDefault();
                if (students != null) isProtected = true;
                var schoolViewModel = new SchoolViewModel
                {
                    SchoolID = item.SchoolID,
                    LogoName = item.LogoName,
                    SchoolName = item.CompanyName,
                    SchoolShortName = item.CompanyShortName,
                    StartDate = item.SchoolYearStart,
                    EndDate = item.SchoolYearEnd,
                    IsActive = item.IsActive,

                    IsProtected = isProtected,
                };

                list.Add(schoolViewModel);
            }
            return Json(list);
        }

        [HttpGet]
        public IActionResult AddorEditSchoolInfo(string period, int schoolID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);
            SchoolInfo schoolInfo = new SchoolInfo();
            PSerialNumber pSerialNumber = new PSerialNumber();

            if (schoolID != 0)
            {
                schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
                pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(schoolID);
                var schoolFee = _schoolFeeRepository.GetSchoolFeeAll(schoolID, "L1");
            }
            if (schoolInfo.CompanyShortCode == null) schoolInfo.CompanyShortCode = schoolID.ToString();
            if (schoolInfo.LogoName == null) schoolInfo.LogoName = "default.png";
            if (schoolInfo.NameOnReceipt == null) schoolInfo.NameOnReceipt = true;
            if (schoolInfo.EmailSMTPSsl == null) schoolInfo.EmailSMTPSsl = false;
            if (schoolInfo.EIIsActive == null) schoolInfo.EIIsActive = false;
            if (schoolInfo.IsActive == null) schoolInfo.IsActive = true;
            if (schoolInfo.DefaultInstallment == 0) schoolInfo.DefaultInstallment = 10;
            if (schoolInfo.CurrencyDecimalPlaces == 0) schoolInfo.CurrencyDecimalPlaces = 2;
            if (schoolInfo.EIInvoiceSerialCode1 == null) schoolInfo.EIInvoiceSerialCode1 = "NCS";
            if (schoolInfo.EIInvoiceSerialCode2 == null) schoolInfo.EIInvoiceSerialCode2 = "NCS";

            DateTime s = DateTime.Now;
            s = new DateTime(s.Year, 09, 01);

            DateTime e = DateTime.Now;
            e = new DateTime(s.Year + 1, 06, 01);

            if (schoolInfo.SchoolYearStart == null)
            {
                schoolInfo.SchoolYearStart = s;
                schoolInfo.FinancialYearStart = s;
            }
            if (schoolInfo.SchoolYearEnd == null)
            {
                schoolInfo.SchoolYearEnd = e;
                schoolInfo.FinancialYearEnd = e;
            }

            if (schoolInfo.SchoolID == 0)
            {
                schoolInfo.NewPeriod = user.UserPeriod;
                schoolInfo.Char01 = "D";
                schoolInfo.Char02 = "F";
                schoolInfo.Char03 = "I";
                schoolInfo.IsChar01 = true;
                schoolInfo.IsChar02 = false;
                schoolInfo.IsChar03 = false;
                schoolInfo.IsChar04 = false;
                schoolInfo.IsChar06 = false;
                schoolInfo.Char01Explanation = "Devamsız";
                schoolInfo.Char02Explanation = "Faaliyet";
                schoolInfo.Char03Explanation = "İzin";
                schoolInfo.Char01Max = 20;
                schoolInfo.Char02Max = 20;
                schoolInfo.Char03Max = 20;
                schoolInfo.CopiesOfForm = 2;
                schoolInfo.CopiesOfForm = 2;
                schoolInfo.PrintQuantity = 1;
                _schoolInfoRepository.CreateSchoolInfo(schoolInfo);
                TempData["schoolID"] = schoolInfo.SchoolID;
                schoolID = schoolInfo.SchoolID;

                bool isPermission = _usersWorkAreasRepository.ExistUsersWorkAreas(schoolID);
                if (!isPermission)
                {
                    var usersWorkAreas = new UsersWorkAreas();
                    usersWorkAreas.UsersWorkAreaID = 0;
                    usersWorkAreas.UserID = userID;
                    usersWorkAreas.CategoryID = schoolInfo.SchoolID;
                    usersWorkAreas.IsSchool = true;
                    usersWorkAreas.IsSelect = true;
                    _usersWorkAreasRepository.CreateUsersWorkAreas(usersWorkAreas);
                }

                var sortNo = 0;
                var schools = _schoolInfoRepository.GetSchoolInfoAll();
                if (schools.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)schools.Max(a => a.SortOrder);
                    sortNo++;
                }
                schoolInfo.SortOrder = sortNo;
            }

            if (schoolInfo.AccountNoID01 == null || schoolInfo.AccountNoID02 == null)
            {
                schoolInfo.AccountNoID01 = "100";
                schoolInfo.AccountNoID02 = "340";
                schoolInfo.AccountNoID03 = "600";
                schoolInfo.AccountNoID04 = "101";
                schoolInfo.AccountNoID05 = "121";
                schoolInfo.AccountNoID06 = "120";
                schoolInfo.AccountNoID07 = "102";
                schoolInfo.AccountNoID08 = "108";
                schoolInfo.AccountNoID09 = "108 01";
                schoolInfo.AccountNoID10 = "600";
                schoolInfo.AccountNoID11 = "102";

                schoolInfo.RefundDebtAccountID = "610";
                schoolInfo.RefundAccountNoID1 = "100";
                schoolInfo.RefundAccountNoID2 = "100";
                schoolInfo.RefundAccountNoID3 = "100";

                schoolInfo.CancelCreditNoID = "121";
                schoolInfo.CancelDebtNoID = "340";
            }

            if (pSerialNumber.PSerialNumberID == 0)
            {
                pSerialNumber.PSerialNumberID = schoolInfo.SchoolID;
                pSerialNumber.InvoiceName1 = Resources.Resource.Invoice + "-1";
                pSerialNumber.InvoiceName2 = Resources.Resource.Invoice + "-2";
                pSerialNumber.InvoiceName3 = Resources.Resource.Invoice + "-3";
                pSerialNumber.InvoiceName4 = Resources.Resource.Invoice + "-4";
                _pSerialNumberRepository.CreatePSerialNumber(pSerialNumber);
            }

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["SchoolName"] = schoolInfo.CompanyName;
            TempData["Period"] = period;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryNameSub = "name";
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryNameSub = "language1";
                categoryName = "language1";
            }

            var schoolViewModel = new SchoolViewModel
            {
                UserPeriod = period,
                UserID = userID,
                SchoolID = schoolID,
                SchoolInfo = schoolInfo,
                PSerialNumber = pSerialNumber,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName,
                CategoryNameSub = categoryNameSub,

            };
            return View(schoolViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditSchoolInfo(SchoolViewModel schoolViewModel, IFormFile imgfile)
        {
            if (!ModelState.IsValid)
                ViewBag.IsSuccess = true;

            //Önceki image dosyası siliniyor
            var deletepath = Path.Combine("Upload", "Images");
            if (imgfile != null && ModelState.IsValid)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, deletepath, schoolViewModel.SchoolInfo.LogoName);
                if (System.IO.File.Exists(imagePath))
                    if (schoolViewModel.SchoolInfo.LogoName != "default.png")
                        System.IO.File.Delete(imagePath);
            }

            if (imgfile != null && ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imgfile.FileName);
                string extension = Path.GetExtension(imgfile.FileName);

                var uniqFileName = Guid.NewGuid().ToString();
                schoolViewModel.SchoolInfo.LogoName = fileName = Path.GetFileName(uniqFileName + "-" + fileName.ToLower() + extension);
                int lenght = schoolViewModel.SchoolInfo.LogoName.Length;
                if (lenght > 100) schoolViewModel.SchoolInfo.LogoName = fileName = Path.GetFileName(uniqFileName + extension);

                string path = Path.Combine(wwwRootPath + "/Upload/Images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
            }


            if (ModelState.IsValid)
            {
                if (schoolViewModel.SchoolInfo.SchoolID != 0)
                {
                    if (schoolViewModel.SchoolInfo.CopiesOfForm == null)
                    {
                        schoolViewModel.SchoolInfo.CopiesOfForm = 2;
                    }
                    if (schoolViewModel.SchoolInfo.PrintQuantity == null)
                    {
                        schoolViewModel.SchoolInfo.PrintQuantity = 1;
                    }
                    _schoolInfoRepository.UpdateSchoolInfo(schoolViewModel.SchoolInfo);
                }

                if (schoolViewModel.PSerialNumber.PSerialNumberID != 0)
                {
                    _pSerialNumberRepository.UpdatePSerialNumber(schoolViewModel.PSerialNumber);
                }

                ViewBag.IsSuccess = false;
                _schoolInfoRepository.Save();
                _pSerialNumberRepository.Save();
            }

            string url = "/M110SchoolInfo/index?userID=" + schoolViewModel.UserID;
            return Redirect(url);
        }

        #region Others
        public IActionResult GridDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolInfo>>(strResult);

            var id = json[0].SchoolID;
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(id);

            if (schoolInfo == null)
                return Json(true);

            var path = Path.Combine("Upload", "Images"); // Bir alt klasör için 
                                                         //var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "/Upload/Images", schoolInfo.LogoName);
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, path, json[0].LogoName);
            if (System.IO.File.Exists(imagePath))
                if (schoolInfo.LogoName != "default.png")
                    System.IO.File.Delete(imagePath);


            var pserialNumber = _pSerialNumberRepository.GetPSerialNumber(id);
            if (pserialNumber != null)
            {
                _pSerialNumberRepository.DeletePSerialNumber(pserialNumber);
            }

            var schoolFee = _schoolFeeRepository.GetSchoolFeeAll(id, "L1");
            foreach (var item in schoolFee)
            {
                _schoolFeeRepository.DeleteSchoolFee(item);
            }

            _schoolInfoRepository.DeleteSchoolInfo(schoolInfo);

            return Json(true);
        }

        [Route("M110SchoolInfo/GridDataDeleteNew/{schoolID}")]
        public IActionResult GridDataDeleteNew(int schoolID)
        {
            var schoolInfo = _schoolInfoRepository.GetSchoolInfoAll().ToList();
            foreach (var item in schoolInfo)
            {
                if (item.CompanyName == null && item.CompanyNameForBond == null)
                {
                    _schoolInfoRepository.DeleteSchoolInfo(item);
                    var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(schoolID);
                    _pSerialNumberRepository.DeletePSerialNumber(pSerialNumber);
                }
            }

            var pserialNumber = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            if (pserialNumber != null)
            {
                _pSerialNumberRepository.DeletePSerialNumber(pserialNumber);
            }
            return Json(true);
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

        public IActionResult IntegratorCombo(int id)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var integrator = _parameterRepository.GetParameterSubID(categoryID);
            return Json(integrator);
        }
        public IActionResult PaymentTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(registrationType);
        }
        public IActionResult BankNameCombo(int id)
        {
            var bankNameType = _bankRepository.GetBankAll(id);
            return Json(bankNameType);
        }

        public IActionResult TimeZone()
        {
            ReadOnlyCollection<TimeZoneInfo> tzCollection;
            tzCollection = TimeZoneInfo.GetSystemTimeZones();

            return Json(tzCollection);
        }

        [Route("M110SchoolInfo/SchoolFeeDataRead/{schoolID}/{userID}")]
        public IActionResult SchoolFeeDataRead(int schoolID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            //Turkish
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
            string schoolFee01 = Resources.Resource.SchoolFee01;
            string schoolFee02 = Resources.Resource.SchoolFee02;
            string schoolFee03 = Resources.Resource.SchoolFee03;
            string schoolFee04 = Resources.Resource.SchoolFee04;
            string schoolFee05 = Resources.Resource.SchoolFee05;
            //By Cullture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            string schoolFee011 = Resources.Resource.SchoolFee01;
            string schoolFee022 = Resources.Resource.SchoolFee02;
            string schoolFee033 = Resources.Resource.SchoolFee03;
            string schoolFee044 = Resources.Resource.SchoolFee04;
            string schoolFee055 = Resources.Resource.SchoolFee05;

            List<SchoolFee> list = new List<SchoolFee>();
            var schoolFee = _schoolFeeRepository.GetSchoolFeeAll(schoolID, "L1");

            bool tr = false;
            bool en = false;
            foreach (var item in schoolFee)
            {
                if (item.Name != null) tr = true;
                if (item.Language1 != null) en = true;
            }

            if (schoolFee.Count() == 0 && (tr == false || en == false))
            {
                for (int i = 0; i < 5; i++)
                {
                    var fee = new SchoolFee();
                    fee.SchoolFeeID = 0;
                    fee.SchoolID = schoolID;
                    fee.FeeCategory = 1;

                    if (i == 0) { fee.Name = schoolFee01; fee.Language1 = schoolFee011; fee.Tax = 08; }
                    if (i == 1) { fee.Name = schoolFee02; fee.Language1 = schoolFee022; fee.Tax = 18; }
                    if (i == 2) { fee.Name = schoolFee03; fee.Language1 = schoolFee033; fee.Tax = 18; }
                    if (i == 3) { fee.Name = schoolFee04; fee.Language1 = schoolFee044; fee.Tax = 18; }
                    if (i == 4) { fee.Name = schoolFee05; fee.Language1 = schoolFee055; fee.Tax = 18; }

                    fee.CategoryLevel = "L1";
                    fee.SortOrder = i + 1;
                    fee.IsActive = true;
                    fee.IsSelect = false;
                    list.Add(fee);
                }
                return Json(list);
            }
            else
            {
                return Json(schoolFee);
            }
        }

        [Route("M110SchoolInfo/SchoolServiceFeeDataRead/{schoolID}/{userID}")]
        public IActionResult SchoolServiceFeeDataRead(int schoolID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            //Turkish
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
            string schoolFee01 = Resources.Resource.SchoolFee011;
            string schoolFee02 = Resources.Resource.SchoolFee022;
            string schoolFee03 = Resources.Resource.SchoolFee033;
            string schoolFee04 = Resources.Resource.SchoolFee044;
            string schoolFee05 = Resources.Resource.SchoolFee055;
            //By Cullture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            string schoolFee011 = Resources.Resource.SchoolFee011;
            string schoolFee022 = Resources.Resource.SchoolFee022;
            string schoolFee033 = Resources.Resource.SchoolFee033;
            string schoolFee044 = Resources.Resource.SchoolFee044;
            string schoolFee055 = Resources.Resource.SchoolFee055;

            List<SchoolFee> list = new List<SchoolFee>();
            var schoolFee = _schoolFeeRepository.GetSchoolServiceFeeAll(schoolID, "L1");

            bool tr = false;
            bool en = false;
            foreach (var item in schoolFee)
            {
                if (item.Name != null) tr = true;
                if (item.Language1 != null) en = true;
            }

            if (schoolFee.Count() == 0 && (tr == false || en == false))
            {
                for (int i = 0; i < 5; i++)
                {
                    var fee = new SchoolFee();
                    fee.SchoolFeeID = 0;
                    fee.SchoolID = schoolID;
                    fee.FeeCategory = 2;

                    if (i == 0) { fee.Name = schoolFee01; fee.Language1 = schoolFee011; fee.Tax = 08; }
                    if (i == 1) { fee.Name = schoolFee02; fee.Language1 = schoolFee022; fee.Tax = 18; }
                    if (i == 2) { fee.Name = schoolFee03; fee.Language1 = schoolFee033; fee.Tax = 18; }
                    if (i == 3) { fee.Name = schoolFee04; fee.Language1 = schoolFee044; fee.Tax = 18; }
                    if (i == 4) { fee.Name = schoolFee05; fee.Language1 = schoolFee055; fee.Tax = 18; }

                    fee.CategoryLevel = "L1";
                    fee.Tax = 8;
                    fee.SortOrder = i + 1;
                    fee.IsActive = true;
                    fee.IsSelect = false;
                    list.Add(fee);
                }
                return Json(list);
            }
            else
            {
                return Json(schoolFee);
            }
        }

        [HttpPost]
        [Route("M110SchoolInfo/SchoolFeeDataUpdate/{strResult}/{schoolID}")]
        public IActionResult SchoolFeeDataUpdate([Bind(Prefix = "models")] string strResult, int schoolID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);

            List<SchoolFee> list = new List<SchoolFee>();
            var i = 0;
            foreach (var item in json)
            {
                var schoolFee = _schoolFeeRepository.GetSchoolFee2(json[i].SchoolFeeID);

                if (ModelState.IsValid)
                {
                    schoolFee.SchoolFeeID = json[i].SchoolFeeID;
                    schoolFee.SchoolID = schoolID;
                    schoolFee.Name = json[i].Name;
                    schoolFee.Language1 = json[i].Language1;
                    schoolFee.Tax = json[i].Tax;
                    schoolFee.SortOrder = json[i].SortOrder;
                    schoolFee.IsActive = json[i].IsActive;
                    schoolFee.IsSelect = json[i].IsSelect;
                    _schoolFeeRepository.UpdateSchoolFee(schoolFee);
                }
                i++;
            }
            return Json(list);
        }
        [HttpPost]
        [Route("M110SchoolInfo/SchoolFeeDataDelete/{strResult}/{userID}/{schoolID}")]
        public IActionResult SchoolFeeDataDelete([Bind(Prefix = "models")] string strResult, int userID, int schoolID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);
            var i = 0;
            foreach (var item in json)
            {
                var schoolFee = _schoolFeeRepository.GetSchoolFee2(json[i].SchoolFeeID);
                _schoolFeeRepository.DeleteSchoolFee(schoolFee);
                i++;
            }
            DeleteSchoolFeeTable(schoolID, userID, json[0].SchoolFeeID);
            return Json(true);
        }
        public void DeleteSchoolFeeTable(int schoolID, int userID, int schoolFeeID)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(schoolID, user.UserPeriod, schoolFeeID);
            foreach (var df in schoolFeeTable)
            {
                _schoolFeeTableRepository.DeleteSchoolFeeTable(df);
            }
        }

        [HttpPost]
        [Route("M110SchoolInfo/SchoolServiceFeeDataUpdate/{strResult}/{schoolID}")]
        public IActionResult SchoolServiceFeeDataUpdate([Bind(Prefix = "models")] string strResult, int schoolID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);

            List<SchoolFee> list = new List<SchoolFee>();
            var i = 0;
            foreach (var item in json)
            {
                var schoolFee = _schoolFeeRepository.GetSchoolFee(json[i].SchoolFeeID);
                if (ModelState.IsValid)
                {
                    schoolFee.SchoolFeeID = json[i].SchoolFeeID;
                    schoolFee.SchoolID = json[i].SchoolID;
                    schoolFee.Name = json[i].Name;
                    schoolFee.Language1 = json[i].Language1;
                    schoolFee.Tax = json[i].Tax;
                    schoolFee.SortOrder = json[i].SortOrder;
                    schoolFee.IsActive = json[i].IsActive;
                    schoolFee.IsSelect = json[i].IsSelect;
                    _schoolFeeRepository.UpdateSchoolFee(schoolFee);
                }

                i++;
            }
            return Json(list);
        }


        [HttpPost]
        [Route("M110SchoolInfo/SchoolFeeDataCreate/{strResult}/{schoolID}")]
        public IActionResult SchoolFeeDataCreate([Bind(Prefix = "models")] string strResult, int schoolID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);
            List<SchoolFee> list = new List<SchoolFee>();

            var i = 0;
            foreach (var item in json)
            {
                var schoolFee = _schoolFeeRepository.GetSchoolFee(json[i].SchoolFeeID);

                var sortNo = 0;
                var schoolFeeLastNo = _schoolFeeRepository.GetSchoolFeeAll(schoolID, "L1");
                if (schoolFeeLastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)schoolFeeLastNo.Max(a => a.SortOrder);
                    sortNo++;
                }

                if (ModelState.IsValid)
                {
                    schoolFee = new SchoolFee();
                    schoolFee.FeeCategory = 1;
                    schoolFee.SchoolID = schoolID;
                    schoolFee.Name = json[i].Name;
                    schoolFee.Language1 = json[i].Language1;
                    schoolFee.SchoolFeeSubID = json[i].SchoolFeeSubID;
                    schoolFee.CategoryLevel = json[i].CategoryLevel;
                    schoolFee.Tax = json[i].Tax;
                    schoolFee.SortOrder = json[i].SortOrder;
                    if (schoolFee.SortOrder == 0)
                    {
                        schoolFee.SortOrder = sortNo;
                    }
                    schoolFee.IsActive = true;
                    schoolFee.IsSelect = true;
                    _schoolFeeRepository.CreateSchoolFee(schoolFee);
                }
                i = i + 1;
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M110SchoolInfo/SchoolServiceFeeDataCreate/{strResult}/{schoolID}")]
        public IActionResult SchoolServiceFeeDataCreate([Bind(Prefix = "models")] string strResult, int schoolID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);

            List<SchoolFee> list = new List<SchoolFee>();
            var i = 0;
            foreach (var item in json)
            {
                var schoolFee = _schoolFeeRepository.GetSchoolFee(json[i].SchoolFeeID);

                var sortNo = 0;
                var schoolFeeLastNo = _schoolFeeRepository.GetSchoolServiceFeeAll(schoolID, "L1");
                if (schoolFeeLastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)schoolFeeLastNo.Max(a => a.SortOrder);
                    sortNo++;
                }

                if (ModelState.IsValid)
                {
                    schoolFee = new SchoolFee();
                    schoolFee.SchoolFeeID = json[i].SchoolFeeID;
                    schoolFee.FeeCategory = 2;
                    schoolFee.SchoolID = schoolID;
                    schoolFee.Name = json[i].Name;
                    schoolFee.Language1 = json[i].Language1;
                    schoolFee.SchoolFeeSubID = json[i].SchoolFeeSubID;
                    schoolFee.CategoryLevel = json[i].CategoryLevel;
                    schoolFee.Tax = json[i].Tax;
                    schoolFee.SortOrder = json[i].SortOrder;
                    if (schoolFee.SortOrder == 0)
                    {
                        schoolFee.SortOrder = sortNo;
                    }
                    schoolFee.IsActive = true;
                    schoolFee.IsSelect = true;
                    _schoolFeeRepository.CreateSchoolFee(schoolFee);
                }
                i = i + 1;
            }
            return Json(list);
        }

        [Route("M110SchoolInfo/SchoolServiceFeeDataDelete/{strResult}")]
        public IActionResult SchoolServiceFeeDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);
            var i = 0;
            foreach (var item in json)
            {
                var schoolFee = _schoolFeeRepository.GetSchoolFee2(json[i].SchoolFeeID);
                _schoolFeeRepository.DeleteSchoolFee(schoolFee);
                i++;
            }
            return Json(true);
        }

        #endregion


        #region Scheduler
        public IActionResult Guidance(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            var taskViewModel = new TaskViewModel
            {
                UserID = userID,
                SchoolID = user.SchoolID,
                SelectedCulture = user.SelectedCulture.Trim(),
            };
            return View(taskViewModel);
        }
        [Route("M110SchoolInfo/GridStudentDataRead/{userID}")]
        public IActionResult GridStudentDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());

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

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;
            var statuCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationTypeCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Cinsiyeti").CategoryID;
            var gender = _parameterRepository.GetParameterSubID(categoryID);

            bool isExist = false;
            string classroomName = "";
            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in student)
            {
                if (item.FirstName == null && item.LastName == null)
                {
                    _studentRepository.DeleteStudent(item);
                }
                else
                {
                    if (school.NewPeriod == user.UserPeriod)
                    {
                        if (item.ClassroomID > 0)
                        {
                            classroomName = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                        }
                    }
                    else
                    {
                        isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                        if (isExist)
                        {
                            classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        }
                    }

                    var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                    var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);

                    string gendertxt = gender.FirstOrDefault(p => p.CategoryID == item.GenderTypeCategoryID).CategoryName;


                    var studentViewModel = new StudentViewModel();
                    {
                        studentViewModel.Period = user.UserPeriod;
                        studentViewModel.SchoolID = user.SchoolID;
                        studentViewModel.ViewModelID = item.StudentID;
                        studentViewModel.StudentID = item.StudentID;
                        studentViewModel.StudentPicture = item.StudentPicture;
                        studentViewModel.Gender = gendertxt;

                        studentViewModel.Name = item.FirstName + " " + item.LastName;

                        studentViewModel.StudentClassroom = classroomName;

                        studentViewModel.StatuCategory = statuCategory.CategoryName;

                        studentViewModel.RegistrationTypeCategory = registrationTypeCategory.CategoryName;
                        studentViewModel.DateOfRegistration = item.DateOfRegistration;

                        studentViewModel.StudentNumber = item.StudentNumber;
                        studentViewModel.IdNumber = item.IdNumber;
                        studentViewModel.DateOfRegistration = item.DateOfRegistration;
                        studentViewModel.ParentName = item.ParentName;
                        studentViewModel.IsActive = item.IsActive;
                        studentViewModel.SiblingID = item.SiblingID;
                    };

                    list.Add(studentViewModel);

                }
            }
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        public IActionResult SchoolTask(int userID, int taskTypeID)
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

            var categoryID = 0;
            if (taskTypeID == 3)
                categoryID = _parameterRepository.GetParameterCategoryName("Ders İşaretleyicileri").CategoryID;
            else
                categoryID = _parameterRepository.GetParameterCategoryName("Okul İşaretleyicileri").CategoryID;

            var color = _parameterRepository.GetParameterSubID(categoryID);
            int inx = _parameterRepository.GetParameterSubID(categoryID).First().CategoryID;
            int classroomInx = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod).First().ClassroomID;
            var taskViewModel = new TaskViewModel
            {
                SchoolID = user.SchoolID,
                UserID = userID,
                StudentID = 0,
                TaskTypeID = taskTypeID,
                ClassroomID = classroomInx,
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
        [Route("M110SchoolInfo/SchoolsTaskDataRead/{schoolID}/{userID}/{taskTypeID}/{classroomID}")]
        public IActionResult SchoolsTaskDataRead(int schoolID, int userID, int taskTypeID, int classroomID)
        {
            var user = _usersRepository.GetUser(userID);
            var task = _schoolsTaskDataSourceRepository.GetSchoolsTaskAll(schoolID, userID, taskTypeID, classroomID);
            List<TaskViewModel> list = new List<TaskViewModel>();

            foreach (var item in task)
            {
                var taskViewModel = new TaskViewModel
                {
                    ViewModelID = item.TaskID,
                    TaskID = item.TaskID,
                    TaskTypeID = taskTypeID,
                    ClassroomID = item.ClassroomID,
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

        [Route("M110SchoolInfo/SchoolsTaskDataCreate/{strResult}/{schoolID}/{userID}/{taskTypeID}/{classroomID}")]
        public IActionResult SchoolsTaskDataCreate([Bind(Prefix = "models")] string strResult, int schoolID, int userID, int taskTypeID, int classroomID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolsTaskDataSource>>(strResult);
            List<SchoolsTaskDataSource> list = new List<SchoolsTaskDataSource>();
            var task = new SchoolsTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                task = new SchoolsTaskDataSource();
                task.TaskID = 0;
                task.TaskTypeID = taskTypeID;
                task.ClassroomID = classroomID;
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
                    _schoolsTaskDataSourceRepository.CreateSchoolsTask(task);
                }
                i = i + 1;
            }
            return Json(task);
        }
        public IActionResult SchoolsTaskDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolsTaskDataSource>>(strResult);

            SchoolsTaskDataSource list = new SchoolsTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                var task = _schoolsTaskDataSourceRepository.GetSchoolsTask(json[i].SchoolID, json[i].UserID, json[i].TaskTypeID, json[i].TaskID, json[i].ClassroomID);

                task.TaskID = json[i].TaskID;
                task.TaskTypeID = json[i].TaskTypeID;
                task.ClassroomID = item.ClassroomID;
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
                    _schoolsTaskDataSourceRepository.UpdateSchoolsTask(task);
                }
                i = i + 1;
            }

            return Json(list);
        }
        [HttpPost]
        public IActionResult SchoolsTaskDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolsTaskDataSource>>(strResult);

            SchoolsTaskDataSource list = new SchoolsTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                var task = _schoolsTaskDataSourceRepository.GetSchoolsTask(json[i].SchoolID, json[i].UserID, json[i].TaskTypeID, json[i].TaskID, json[i].ClassroomID);

                task.TaskID = json[i].TaskID;
                task.SchoolID = json[i].SchoolID;
                task.ClassroomID = item.ClassroomID;
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
                    _schoolsTaskDataSourceRepository.DeleteSchoolsTask(task);
                }
                i = i + 1;
            }

            return Json(list);
        }

        [Route("M110SchoolInfo/ResourceTypeCombo/{taskTypeID}")]
        public IActionResult ResourceTypeCombo(int taskTypeID)
        {
            var categoryID = 0;
            if (taskTypeID == 3)
                categoryID = _parameterRepository.GetParameterCategoryName("Ders İşaretleyicileri").CategoryID;
            else
                categoryID = _parameterRepository.GetParameterCategoryName("Okul İşaretleyicileri").CategoryID;

            var user = _parameterRepository.GetParameterSubID(categoryID);
            return Json(user);
        }
        #endregion
    }
}

