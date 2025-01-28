using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ESCHOOL.Controllers
{
    public class M310PositionChanges : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;

        IStudentDebtRepository _studentDebtRepository;

        IStudentInstallmentRepository _studentInstallmentRepository;
        ITempDataRepository _tempDataRepository;
        ITempPlanRepository _tempPlanRepository;

        IStudentTempRepository _studentTempRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInvoiceDetailRepository _studentInvoiceDetailRepository;
        IBankRepository _bankRepository;
        IClassroomRepository _classroomRepository;

        IStudentDiscountRepository _studentDiscountRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IParameterRepository _parameterRepository;
        ISchoolBusServicesRepository _schoolBusServicesRepository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        ITempM101Repository _tempM101Repository;

        IUsersRepository _usersRepository;
        IUsersLogRepository _usersLogRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;

        IWebHostEnvironment _hostEnvironment;
        public M310PositionChanges(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            ITempDataRepository tempDataRepository,
            ITempPlanRepository tempPlanRepository,

            IStudentTempRepository studentTempRepository,
            IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
            IStudentInvoiceRepository studentInvoiceRepository,
            IStudentInvoiceDetailRepository studentInvoiceDetailRepository,
            IBankRepository bankRepository,
            IClassroomRepository classroomRepository,
            IStudentDiscountRepository studentDiscountRepository,
            IPSerialNumberRepository pSerialNumberRepository,
            ISchoolFeeRepository schoolFeeRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IParameterRepository parameterRepository,
            ISchoolBusServicesRepository schoolBusServicesRepository,

            IAccountingRepository accountingRepository,
            IAccountCodesRepository accountingCodeRepository,
            ITempM101Repository tempM101Repository,

            IUsersRepository usersRepository,
            IUsersLogRepository usersLogRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,

        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            
            _studentDebtRepository = studentDebtRepository;

            _studentInstallmentRepository = studentInstallmentRepository;
            _tempDataRepository = tempDataRepository;
            _tempPlanRepository = tempPlanRepository;

            _studentDiscountRepository = studentDiscountRepository;

            _studentTempRepository = studentTempRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInvoiceDetailRepository = studentInvoiceDetailRepository;
            _bankRepository = bankRepository;
            _classroomRepository = classroomRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _parameterRepository = parameterRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;

            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountingCodeRepository;
            _tempM101Repository = tempM101Repository;

            _usersRepository = usersRepository;
            _usersLogRepository = usersLogRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;

            _hostEnvironment = hostEnvironment;
        }

        #region Students 
        [Route("M310PositionChanges/GridStudentDataRead/{userID}")]
        public IActionResult GridStudentDataRead(int userID)
        {
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

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;
            var statuCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationTypeCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Cinsiyeti").CategoryID;
            var gender = _parameterRepository.GetParameterSubID(categoryID);

            string classroomName = "";
            bool isExist = false;
            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                if (item.FirstName == null && item.LastName == null)
                {
                    _studentRepository.DeleteStudent(item);
                }
                else
                {
                    var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                    var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);

                    string gendertxt = "";
                    if (user.SelectedCulture.Trim() == "en-US")
                        gendertxt = gender.FirstOrDefault(p => p.CategoryID == item.GenderTypeCategoryID).Language1;
                    else gendertxt = gender.FirstOrDefault(p => p.CategoryID == item.GenderTypeCategoryID).CategoryName;

                    var installments = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);

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

                    var studentViewModel = new StudentViewModel();
                    {
                        studentViewModel.Period = user.UserPeriod;
                        studentViewModel.SchoolID = user.SchoolID;
                        studentViewModel.UserID = user.UserID;
                        studentViewModel.ViewModelID = item.StudentID;
                        studentViewModel.StudentID = item.StudentID;
                        studentViewModel.StudentPicture = item.StudentPicture;
                        studentViewModel.Gender = gendertxt;

                        studentViewModel.Name = item.FirstName + " " + item.LastName;

                        studentViewModel.StudentClassroom = classroomName;
                        if (user.SelectedCulture.Trim() == "en-US")
                        {
                            studentViewModel.StatuCategory = statuCategory.Language1;
                            studentViewModel.RegistrationTypeCategory = registrationTypeCategory.Language1;
                        }
                        else
                        {
                            studentViewModel.StatuCategory = statuCategory.CategoryName;
                            studentViewModel.RegistrationTypeCategory = registrationTypeCategory.CategoryName;
                        }

                        studentViewModel.DateOfRegistration = item.DateOfRegistration;
                        studentViewModel.StudentNumber = item.StudentNumber;
                        studentViewModel.IdNumber = item.IdNumber;
                        studentViewModel.DateOfRegistration = item.DateOfRegistration;
                        studentViewModel.ParentName = item.ParentName;
                        studentViewModel.IsActive = item.IsActive;
                        studentViewModel.SiblingID = item.SiblingID;

                        var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceTrue(user.UserPeriod, user.SchoolID, item.StudentID).ToList();
                        studentViewModel.CutInvoice = Convert.ToDecimal(cutinvoice.Sum(p => p.DAmount));

                        var uncutinvoice = _studentInvoiceRepository.GetStudentInvoiceFalse(user.UserPeriod, user.SchoolID, item.StudentID).ToList();
                        studentViewModel.UncutInvoice = Convert.ToDecimal(uncutinvoice.Sum(p => p.DAmount));

                    };
                    decimal total = 0;
                    foreach (var inst in installments)
                    {
                        total += inst.InstallmentAmount;
                    }
                    studentViewModel.Total = total;

                    list.Add(studentViewModel);
                }
            }
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        [Route("M310PositionChanges/GridStudentDataReadFilter/{userID}/{paymentTypeID}")]
        public IActionResult GridStudentDataReadFilter(int userID, int paymentTypeID)
        {
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

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;
            var statuCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationTypeCategories = _parameterRepository.GetParameterSubID(categoryID);

            bool isExist = false;
            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);
                if (paymentTypeID != 0)
                {
                    isExist = _studentInstallmentRepository.ExistStudentInstallment2(item.SchoolID, user.UserPeriod, item.StudentID, paymentTypeID);
                }
                if (paymentTypeID == 0) isExist = true;

                if (isExist)
                {
                    var studentViewModel = new StudentViewModel();
                    {
                        studentViewModel.ViewModelID = item.StudentID;
                        studentViewModel.StudentID = item.StudentID;
                        studentViewModel.StudentPicture = item.StudentPicture;
                        studentViewModel.Name = item.FirstName + " " + item.LastName;

                        if (school.NewPeriod == user.UserPeriod)
                        {
                            if (item.ClassroomID > 0)
                            {
                                studentViewModel.StudentClassroom = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                            }
                        }
                        else
                        {
                            isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                            if (isExist)
                            {
                                studentViewModel.StudentClassroom = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                            }
                        }

                        if (user.SelectedCulture.Trim() == "en-US")
                        {
                            studentViewModel.StatuCategory = statuCategory.Language1;
                            studentViewModel.RegistrationTypeCategory = registrationTypeCategory.Language1;
                        }
                        else
                        {
                            studentViewModel.StatuCategory = statuCategory.CategoryName;
                            studentViewModel.RegistrationTypeCategory = registrationTypeCategory.CategoryName;
                        }

                        var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceTrue(user.UserPeriod, user.SchoolID, item.StudentID);
                        studentViewModel.CutInvoice = Convert.ToDecimal(cutinvoice.Sum(p => p.DAmount));

                        var uncutinvoice = _studentInvoiceRepository.GetStudentInvoiceFalse(user.UserPeriod, user.SchoolID, item.StudentID).FirstOrDefault();
                        studentViewModel.UncutInvoice = Convert.ToDecimal(cutinvoice.Sum(p => p.DAmount));

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
            return Json(list);
        }

        #endregion

        #region PositionChanges

        [HttpGet]
        public IActionResult PositionChanges(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Fatura İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var isSuccess = false;
            if (school.EIIsActive == true)
                if (school.EIUserPassword == null || school.EIUserName == null || school.EIInvoiceSerialCode1 == null || school.EIInvoiceSerialCode2 == null || school.EIIntegratorNameID == 0)
                    isSuccess = true;

            var student = _studentRepository.GetStudent(studentID);

            ViewBag.date = user.UserDate;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["studentID"] = studentID;

            var period = user.UserPeriod;

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID);

            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (pSerialNumber == null)
                pSerialNumber = new PSerialNumber();

            categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(school.SchoolID, studentID, period);
            // Clean Data
            var tempDataClean = _tempDataRepository.GetTempData(studentID);
            if (tempDataClean != null)
            {
                foreach (var item in tempDataClean)
                {
                    _tempDataRepository.DeleteTempData(item);
                }
            }
            // Old StudentInstallment Data
            foreach (var item in studentInstallment)
            {
                var tempData = new TempData();
                tempData.TempDataID = 0;
                tempData.StudentID = item.StudentID;
                tempData.InstallmentDate = item.InstallmentDate;
                tempData.InstallmentNo = item.InstallmentNo;
                tempData.CategoryID = item.CategoryID;
                tempData.InstallmentAmount = item.InstallmentAmount;
                tempData.BankID = item.BankID;
                tempData.PreviousPayment = item.PreviousPayment;
                tempData.CashPayment = studentTemp.CashPayment;

                _tempDataRepository.CreateTempData(tempData);
            }

            categoryID = _parameterRepository.GetParameterCategoryName("Cinsiyeti").CategoryID;
            var gender = _parameterRepository.GetParameterSubID(categoryID);

            string gendertxt = "";
            if (student != null)
            {
                if (user.SelectedCulture.Trim() == "en-US")
                    gendertxt = gender.FirstOrDefault(p => p.CategoryID == student.GenderTypeCategoryID).Language1;
                else gendertxt = gender.FirstOrDefault(p => p.CategoryID == student.GenderTypeCategoryID).CategoryName;
            }

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;

            var statu = _parameterRepository.GetParameterSubID(categoryID);
            string statutxt = "";
            if (student != null)
            {
                if (user.SelectedCulture.Trim() == "en-US")
                    statutxt = gender.FirstOrDefault(p => p.CategoryID == student.GenderTypeCategoryID).Language1;
                else statutxt = gender.FirstOrDefault(p => p.CategoryID == student.GenderTypeCategoryID).CategoryName;
            }

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName1 = "feeName";
            string categoryName2 = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName1 = "language1";
                categoryName2 = "language1";
            }

            var studentViewModel = new StudentViewModel
            {
                IsPermission = isPermission,
                Gender = gendertxt,
                StatuCategory = statutxt,

                UserID = userID,
                Period = user.UserPeriod,
                SchoolID = user.SchoolID,
                StudentID = studentID,
                Student = student,
                StudentInstallment2 = studentInstallment,
                StudentTemp = studentTemp,
                PSerialNumber = pSerialNumber,
                Parameter = parameter,
                SchoolInfo = school,
                SelectedCulture = user.SelectedCulture.Trim(),
                IsSuccess = isSuccess,
                CategoryName1 = categoryName1,
                CategoryName2 = categoryName2
            };
            return View(studentViewModel);
        }

        [Route("M310PositionChanges/StudentDebtDataRead/{userID}/{studentid}")]
        public IActionResult StudentDebtDataRead(int userID, int studentid)
        {
            List<StudentDebtViewModel> list = new List<StudentDebtViewModel>();
            if (studentid != 0)
            {
                var user = _usersRepository.GetUser(userID);
                var period = user.UserPeriod;
                var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

                var student = _studentRepository.GetStudent(studentid);
                var typeId = 0;
                if (student.ClassroomID > 0)
                    typeId = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomTypeID;

                Boolean isExist = _studentDebtRepository.ExistStudentDebt(user.UserPeriod, studentid);
                if (!isExist)
                    foreach (var item in fee)
                    {
                        var getCode = new StudentDebt();
                        getCode.StudentID = studentid;
                        getCode.SchoolID = item.SchoolID;
                        getCode.SchoolFeeID = item.SchoolFeeID;
                        getCode.ClassroomTypeID = typeId;
                        getCode.Period = period;
                        getCode.UnitPrice = 0;
                        getCode.Discount = 0;
                        getCode.Amount = 0;
                        getCode.IsList = true;
                        _studentDebtRepository.CreateStudentDebt(getCode);
                    }

                foreach (var item in fee)
                {
                    var studentDebtViewModel = new StudentDebtViewModel();
                    var debt = _studentDebtRepository.GetStudentDebt22(period, user.SchoolID, studentid, item.SchoolFeeID);

                    studentDebtViewModel.ViewModelId = studentid;
                    studentDebtViewModel.StudentID = studentid;
                    if (user.SelectedCulture.Trim() == "en-US") studentDebtViewModel.FeeName = item.Language1;
                    else studentDebtViewModel.FeeName = item.Name;
                    studentDebtViewModel.SchoolFeeID = item.SchoolFeeID;
                    if (debt != null)
                    {
                        studentDebtViewModel.StudentDebtID = debt.StudentDebtID;
                        studentDebtViewModel.SchoolID = debt.SchoolID;
                        studentDebtViewModel.Period = debt.Period;
                        studentDebtViewModel.SelectFee = false;
                        studentDebtViewModel.SelectDiscount = false;
                        studentDebtViewModel.UnitPrice = debt.UnitPrice;
                        studentDebtViewModel.Discount = debt.Discount;
                        studentDebtViewModel.Amount = debt.Amount;
                        studentDebtViewModel.IsList = debt.IsList;
                    }
                    list.Add(studentDebtViewModel);
                }
            }
            return Json(list);
        }

        [Route("M310Collections/SchoolDebtDataRead/{userID}/{period}/{studentid}")]
        public IActionResult SchoolDebtDataRead(int userID, string period, int studentid)
        {
            List<StudentDebtViewModel> list = new List<StudentDebtViewModel>();

            if (studentid != 0)
            {
                var user = _usersRepository.GetUser(userID);
                var student = _studentRepository.GetStudent(studentid);

                var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

                foreach (var item in fee)
                {
                    var studentDebtViewModel = new StudentDebtViewModel();
                    var debt = _studentDebtRepository.GetStudentDebt22(period, user.SchoolID, studentid, item.SchoolFeeID);

                    studentDebtViewModel.ViewModelId = studentid;
                    studentDebtViewModel.StudentID = studentid;
                    if (user.SelectedCulture.Trim() == "en-US") studentDebtViewModel.FeeName = item.Language1;
                    else studentDebtViewModel.FeeName = item.Name;
                    studentDebtViewModel.SchoolFeeID = item.SchoolFeeID;
                    if (debt != null)
                    {
                        studentDebtViewModel.StudentDebtID = debt.StudentDebtID;
                        studentDebtViewModel.SchoolID = debt.SchoolID;
                        studentDebtViewModel.Period = debt.Period;
                        studentDebtViewModel.SelectFee = false;
                        studentDebtViewModel.SelectDiscount = false;
                        studentDebtViewModel.UnitPrice = debt.UnitPrice;
                        studentDebtViewModel.Discount = debt.Discount;
                        studentDebtViewModel.Amount = debt.Amount;
                        studentDebtViewModel.IsList = debt.IsList;
                    }
                    list.Add(studentDebtViewModel);
                }
            }
            return Json(list);
        }


        [Route("M310PositionChanges/InstallmentDataRead1/{userID}/{studentid}")]
        public IActionResult InstallmentDataRead1(int userID, int studentid)
        {
            List<StudentInstallmentViewModel> list = new List<StudentInstallmentViewModel>();
            if (studentid != 0)
            {
                var user = _usersRepository.GetUser(userID);
                var period = _usersRepository.GetUser(userID).UserPeriod;

                var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
                var parameterList = _parameterRepository.GetParameterSubID(categoryID);
                var bankList = _bankRepository.GetBankAll(user.SchoolID);
                var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentid, period);

                foreach (var item in studentinstallment)
                {
                    decimal balance = 0;
                    if (item.PreviousPayment > 0 && item.InstallmentAmount > 0)
                    { balance = item.InstallmentAmount - item.PreviousPayment; };
                    if (item.PreviousPayment == 0 || item.InstallmentAmount == item.PreviousPayment)
                        balance = 0;

                    var bank = "";
                    if (item.BankID > 0)
                        bank = bankList.FirstOrDefault(p => p.BankID == item.BankID).BankName;

                    string category = "";
                    string status = "";
                    if (user.SelectedCulture.Trim() == "en-US")
                    {
                        category = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID).Language1;
                        status = _parameterRepository.GetParameter(item.StatusCategoryID).Language1;
                    }
                    else
                    {
                        category = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID).CategoryName;
                        status = _parameterRepository.GetParameter(item.StatusCategoryID).CategoryName;
                    }

                    var studentInstallmentViewModel = new StudentInstallmentViewModel
                    {
                        StudentInstallmentID = item.StudentInstallmentID,
                        SchoolID = item.SchoolID,
                        StudentID = item.StudentID,
                        InstallmentDate = item.InstallmentDate,
                        InstallmentNo = item.InstallmentNo,
                        Category = category,

                        InstallmentAmount = item.InstallmentAmount,
                        PreviousPayment = item.PreviousPayment,
                        Balance = balance,
                        Status = status,
                        Bank = bank,
                        Explanation = item.Explanation,
                        AccountReceiptNo = item.AccountReceiptNo,
                        PaymentDate = item.PaymentDate,
                    };
                    list.Add(studentInstallmentViewModel);
                }
            }

            return Json(list);
        }


        [Route("M310PositionChanges/InstallmentDataRead2/{userID}/{studentid}")]
        public IActionResult InstallmentDataRead2(int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;
            var student = _studentRepository.GetStudent(studentid);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var statusCategoryID = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;

            var parameterList2 = _parameterRepository.GetParameterSubID(categoryID);
            var parameterList3 = _parameterRepository.GetParameterSubID(statusCategoryID);
            var bankList = _bankRepository.GetBankAll(user.SchoolID);

            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentid, period).Where(b => b.StatusCategoryID != statusID);

            string parentName = "";
            if (student != null)
                if (student.ParentName != null) parentName = student.ParentName;

            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in studentinstallment)
            {
                var parameter2 = parameterList2.FirstOrDefault(p => p.CategoryID == item.CategoryID);
                var parameter3 = parameterList3.FirstOrDefault(p => p.CategoryID == item.StatusCategoryID);
                var bank = bankList.FirstOrDefault(p => p.BankID == item.BankID);
                if (parameter2 == null)
                {
                    parameter2 = parameterList2.FirstOrDefault(p => p.CategorySubID == categoryID);
                }
                if (parameter3 == null)
                {
                    parameter3 = parameterList3.FirstOrDefault(p => p.CategorySubID == statusCategoryID);
                }
                var studentViewModel = new StudentViewModel
                {
                    StudentName = student.FirstName + " " + student.LastName,
                    ParentName = parentName,
                    UserID = userID,
                    ViewModelID = item.StudentInstallmentID,
                    StudentInstallment = item,
                    Parameter2 = parameter2,
                    Parameter3 = parameter3,
                    Bank = bank,
                };

                list.Add(studentViewModel);
            }

            return Json(list);
        }

        [HttpPost]
        [Route("M310PositionChanges/InstallmentDataUpdate/{strResult}")]
        public IActionResult InstallmentDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentViewModel>>(strResult);
            var installment = new StudentInstallment();
            var i = 0;
            foreach (var item in json)
            {
                installment = _studentInstallmentRepository.GetStudentInstallmentID(json[i].StudentInstallment.SchoolID, json[i].StudentInstallment.StudentInstallmentID);

                installment.StudentInstallmentID = json[i].StudentInstallment.StudentInstallmentID;
                installment.SchoolID = json[i].StudentInstallment.SchoolID;
                installment.StudentID = json[i].StudentInstallment.StudentID;
                installment.Period = json[i].StudentInstallment.Period;
                installment.InstallmentDate = json[i].StudentInstallment.InstallmentDate;
                installment.InstallmentNo = json[i].StudentInstallment.InstallmentNo;
                installment.CategoryID = json[i].StudentInstallment.CategoryID;
                installment.InstallmentAmount = json[i].StudentInstallment.InstallmentAmount;
                installment.BankID = json[i].StudentInstallment.BankID;
                installment.CheckCardNo = json[i].StudentInstallment.CheckCardNo;
                installment.PreviousPayment = json[i].StudentInstallment.PreviousPayment;
                installment.FeeName = json[i].StudentInstallment.FeeName;
                installment.IsPrint = json[i].StudentInstallment.IsPrint;
                installment.StatusCategoryID = json[i].StudentInstallment.StatusCategoryID;
                installment.AccountReceiptNo = json[i].StudentInstallment.AccountReceiptNo;
                installment.PaymentDate = json[i].StudentInstallment.PaymentDate;
                installment.Explanation = json[i].StudentInstallment.Explanation;
                installment.CheckBankName = json[i].StudentInstallment.CheckBankName;
                installment.CheckNo = json[i].StudentInstallment.CheckNo;
                installment.Drawer = json[i].StudentInstallment.Drawer;
                installment.Endorser = json[i].StudentInstallment.Endorser;

                if (ModelState.IsValid)
                {
                    _studentInstallmentRepository.UpdateStudentInstallment(installment);
                }
                i++;
            }
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameterList2 = _parameterRepository.GetParameterSubID(categoryID);
            var parameter2 = parameterList2.FirstOrDefault(p => p.CategoryID == json[0].StudentInstallment.CategoryID);

            var statusCategoryID = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var parameterList3 = _parameterRepository.GetParameterSubID(statusCategoryID);
            var parameter3 = parameterList3.FirstOrDefault(p => p.CategoryID == json[0].StudentInstallment.StatusCategoryID);

            var bankList = _bankRepository.GetBankAll(json[0].StudentInstallment.SchoolID);
            var bank = bankList.FirstOrDefault(p => p.BankID == json[0].StudentInstallment.BankID);

            var studentViewModel = new StudentViewModel
            {
                SchoolID = installment.StudentInstallmentID,
                StudentInstallment = installment,
                Parameter2 = parameter2,
                Parameter3 = parameter3,
                Bank = bank,
            };

            //////Users Log//////////////////
            var user = _usersRepository.GetUser(json[0].UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(json[0].StudentInstallment.StudentID);
            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = user.UserID;
            log.TransactionID = _parameterRepository.GetParameterCategoryName("Pozisyon Değişikliği").CategoryID;
            log.UserLogDate = DateTime.Now;
            decimal amount = Math.Round(json[0].StudentInstallment.InstallmentAmount, school.CurrencyDecimalPlaces);
            string dateString = string.Format("{0:dd/MM/yyyy}", json[0].StudentInstallment.InstallmentDate);
            string status = _parameterRepository.GetParameter(json[0].StudentInstallment.StatusCategoryID).CategoryName;

            log.UserLogDescription = student.FirstName + " " + student.LastName + " Taksit Pozisyon Değişikliği, " + dateString + " Tarihli Taksit Ödemesi, " + "Tutarı:" + amount + ", Pozisyon Değişikliği:" + status;
            if (user.SelectedCulture.Trim() == "en-US")
            {
                status = _parameterRepository.GetParameter(json[0].StudentInstallment.StatusCategoryID).Language1;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Installment Position Change, " + dateString + " Dated Installment Payment, " + "Amount:" + amount + ", Position Change:" + status;
            }
            else
            {
                status = _parameterRepository.GetParameter(json[0].StudentInstallment.StatusCategoryID).CategoryName;
            }

            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return Json(studentViewModel);
        }

        #endregion

        #region Combo
        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }
        public IActionResult PaymentTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);

            return Json(registrationType);
        }
        [Route("M310PositionChanges/StatusTypeDataRead/{userID}")]
        public IActionResult StatusTypeDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;

            var statusType = _parameterRepository.GetParameterSubID(categoryID).Where(b => b.CategoryName != "Tahsil");
            if (user.SelectedCulture.Trim() == "en-US") statusType = _parameterRepository.GetParameterSubID(categoryID).Where(b => b.Language1 != "Collection");

            return Json(statusType);
        }

        [Route("M310PositionChanges/BankTypeDataRead/{userID}")]
        public IActionResult BankTypeDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var bankNameType = _bankRepository.GetBankAll(user.SchoolID);
            return Json(bankNameType);
        }

        #endregion
    }
}
