using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ESCHOOL.Controllers
{
    public class M300Collections : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentDebtDetailTableRepository _studentDebtDetailTableRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentInstallmentPaymentRepository _studentInstallmentPaymentRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        ITempDataRepository _tempDataRepository;
        IStudentTempRepository _studentTempRepository;
        IBankRepository _bankRepository;
        IClassroomRepository _classroomRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IParameterRepository _parameterRepository;

        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;

        IUsersRepository _usersRepository;
        IUsersLogRepository _usersLogRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;

        IWebHostEnvironment _hostEnvironment;
        public M300Collections(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDebtDetailTableRepository studentDebtDetailTableRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            IStudentInstallmentPaymentRepository studentInstallmentPaymentRepository,
            IStudentPaymentRepository studentPaymentRepository,
            ITempDataRepository tempDataRepository,
             IStudentTempRepository studentTempRepository,
            IBankRepository bankRepository,
            IClassroomRepository classroomRepository,
            IPSerialNumberRepository pSerialNumberRepository,
            ISchoolFeeRepository schoolFeeRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IParameterRepository parameterRepository,
            ISchoolBusServicesRepository schoolBusServicesRepository,

            IAccountingRepository accountingRepository,
            IAccountCodesRepository accountingCodeRepository,

            IUsersRepository usersRepository,
            IUsersLogRepository usersLogRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,

        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentDebtDetailTableRepository = studentDebtDetailTableRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _studentInstallmentPaymentRepository = studentInstallmentPaymentRepository;
            _studentPaymentRepository = studentPaymentRepository;
            _tempDataRepository = tempDataRepository;
            _bankRepository = bankRepository;
            _classroomRepository = classroomRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _parameterRepository = parameterRepository;
            _studentTempRepository = studentTempRepository;
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountingCodeRepository;

            _usersRepository = usersRepository;
            _usersLogRepository = usersLogRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }
        #region collections
        [HttpGet]

        public IActionResult Collections(int userID, int studentID, int isMenu)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Tahsilat İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            categoryID = _parameterRepository.GetParameterCategoryName("Öğrenci veya Tahsilat İptali").CategoryID;
            bool isPermissionCancel = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            categoryID = _parameterRepository.GetParameterCategoryName("Ücret Bilgilerini Görme").CategoryID;
            bool isPermissionFee = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var student = _studentRepository.GetStudent(studentID);

            ViewBag.date = user.UserDate;
            TempData["studentID"] = studentID;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID);
            if (studentTemp != null)
            {
                if (studentTemp.AccountCode != school.AccountNoID01)
                {
                    studentTemp.AccountCode = school.AccountNoID01;
                    _studentTempRepository.UpdateStudentTemp(studentTemp);
                }
            }

            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (pSerialNumber == null)
                pSerialNumber = new PSerialNumber();

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, period);
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
                    statutxt = statu.FirstOrDefault(p => p.CategoryID == student.RegistrationTypeCategoryID).Language1;
                else statutxt = statu.FirstOrDefault(p => p.CategoryID == student.RegistrationTypeCategoryID).CategoryName;
            }

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName1 = "name";
            string categoryName2 = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName1 = "language1";
                categoryName2 = "language1";
            }

            if (student == null) { student = new Student(); }
            var studentViewModel = new StudentViewModel
            {
                IsPermission = isPermission,
                IsPermissionFee = isPermissionFee,
                IsPermissionCancel = isPermissionCancel,
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
                SchoolInfo = school,
                IsMenu = isMenu,
                SelectedCulture = user.SelectedCulture.Trim(),

                SelectedSchoolCode = user.SelectedSchoolCode,
                RegistrationTypeSubID = 3,
                StatuCategorySubID = 4,
                StatuCategoryID = 10,
                StartDate = school.SchoolYearStart,
                EndDate = school.SchoolYearEnd,
                wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/"), //Picture Path
                CategoryName1 = categoryName1,
                CategoryName2 = categoryName2
            };
            return View(studentViewModel);
        }

        [Route("M300Collections/GridStudentDataRead/{userID}/{check}/{isSibling}/{isTotal}")]
        public IActionResult GridStudentDataRead(int userID, bool check, bool isSibling, bool isTotal)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            int MM = dt.Month;
            int YYYY = dt.Year;

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> student = new List<Student>();
            if (isSibling == true)
            {
                student = _studentRepository.GetStudentSibling(user.SchoolID).ToList();
            }
            else
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                if (check == true)
                    student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
                else student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
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
                    if (installments.Count() > 0)
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
                            studentViewModel.StudentSerialNumber = item.StudentSerialNumber.ToString();
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

                            studentViewModel.StudentNumber = item.StudentNumber;
                            studentViewModel.IdNumber = item.IdNumber;
                            studentViewModel.DateOfRegistration = item.DateOfRegistration;
                            studentViewModel.ParentName = item.ParentName;
                            studentViewModel.IsActive = item.IsActive;
                            studentViewModel.SiblingID = item.SiblingID;
                        };
                        decimal total = 0, totalDebt = 0;
                        var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                        if (studentTemp != null)
                        {
                            total = studentTemp.CashPayment;
                            totalDebt = studentTemp.SubTotal;
                        }
                        totalDebt -= total;
                        foreach (var inst in installments)
                        {
                            total += inst.InstallmentAmount;
                            totalDebt -= inst.PreviousPayment;
                            studentViewModel.Balance = 0;
                            if (isTotal == false)
                            {
                                DateTime d = Convert.ToDateTime(inst.InstallmentDate);
                                int M = d.Month;
                                int Y = d.Year;

                                if (MM == M && YYYY == Y)
                                {
                                    studentViewModel.Balance = inst.InstallmentAmount - inst.PreviousPayment;
                                }
                            }
                            else
                            {
                                studentViewModel.Balance = totalDebt;
                            }
                        }
                        studentViewModel.Total = total;
                        studentViewModel.TotalDebt = totalDebt;

                        list.Add(studentViewModel);
                    }
                }
            }
            return Json(list);
        }

        [Route("M300Collections/GridStudentDataReadFilter/{userID}/{paymentTypeID}")]
        public IActionResult GridStudentDataReadFilter(int userID, int paymentTypeID)
        {
            List<Student> student = new List<Student>();
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;
            var statuCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationTypeCategories = _parameterRepository.GetParameterSubID(categoryID);

            string classroomName = "";
            bool isExist = false;
            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in student)
            {
                var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);
                if (paymentTypeID != 0)
                {
                    isExist = _studentInstallmentRepository.ExistStudentInstallment2(item.SchoolID, user.UserPeriod, item.StudentID, paymentTypeID);
                }
                if (paymentTypeID == 0) isExist = true;

                if (isExist)
                {
                    if (item.ClassroomID > 0)
                        classroomName = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
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
                        studentViewModel.ViewModelID = item.StudentID;
                        studentViewModel.StudentID = item.StudentID;
                        studentViewModel.StudentPicture = item.StudentPicture;
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

        [Route("M300Collections/GridSiblingDataRead1/{userID}/{studentID}/{isTotal}")]
        public IActionResult GridSiblingDataRead1(int userID, int studentID, bool isTotal)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            int MM = dt.Month;

            var user = _usersRepository.GetUser(userID);
            List<Student> students = new List<Student>();

            int studentIDTmp = studentID;
            var student = _studentRepository.GetStudent(studentID);
            if (student.SiblingID != student.StudentID)
            {
                student = _studentRepository.GetStudent(studentID);
                studentIDTmp = _studentRepository.GetStudent(studentID).SiblingID;
            }

            students = _studentRepository.GetStudentSibling1(user.SchoolID, studentIDTmp).ToList();
            bool isExist = false;
            string classroomName = "";
            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in students)
            {
                var studentViewModel = new StudentViewModel();
                {
                    if (item.ClassroomID > 0)
                        classroomName = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                    else
                    {
                        isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                        if (isExist)
                        {
                            classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID = user.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        }
                    }

                    var installments = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);

                    decimal total = 0, totalDebt = 0;
                    var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                    if (studentTemp != null)
                    {
                        total = studentTemp.CashPayment;
                        totalDebt = studentTemp.SubTotal;
                    }
                    totalDebt -= total;
                    foreach (var inst in installments)
                    {
                        total += inst.InstallmentAmount;
                        totalDebt -= inst.PreviousPayment;

                        if (isTotal == false)
                        {
                            DateTime d = Convert.ToDateTime(inst.InstallmentDate);
                            int M = d.Month;
                            if (MM == M)
                            {
                                studentViewModel.Balance = totalDebt;
                            }
                        }
                        else
                        {
                            studentViewModel.Balance = totalDebt;
                        }
                    }
                    studentViewModel.Total = total;
                    studentViewModel.TotalDebt = totalDebt;

                    studentViewModel.ViewModelID = item.StudentID;
                    studentViewModel.StudentID = item.StudentID;
                    studentViewModel.StudentClassroom = classroomName;
                    studentViewModel.Name = item.FirstName + " " + item.LastName;
                    studentViewModel.StudentNumber = item.StudentNumber;
                    studentViewModel.IdNumber = item.IdNumber;
                    studentViewModel.ParentName = item.ParentName;
                    studentViewModel.SiblingID = item.SiblingID;
                    studentViewModel.StudentPicture = item.StudentPicture;

                };
                list.Add(studentViewModel);
            }
            return Json(list);
        }


        #endregion

        #region StudentPeriods
        [Route("M300Collections/StudentPeriodsDataRead/{userID}/{studentID}")]
        public IActionResult StudentPeriodsDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var studentPeriod = _studentPeriodsRepository.GetStudent(user.SchoolID, studentID).OrderByDescending(b => b.Period);

            List<StudentPeriodsViewModel> list = new List<StudentPeriodsViewModel>();
            foreach (var item in studentPeriod)
            {
                decimal payment = 0, debt = 0, balance = 0;
                var installment = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, studentID, item.Period);
                var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, item.Period, studentID);
                if (studentTemp != null)
                {
                    payment = studentTemp.CashPayment;
                    debt = studentTemp.CashPayment;
                }
                if (installment != null)
                {
                    payment += installment.Sum(p => p.PreviousPayment);
                    debt += installment.Sum(p => p.InstallmentAmount);
                    balance = debt - payment;
                }

                var sP = new StudentPeriodsViewModel();
                {
                    sP.StudentPeriodID = item.StudentPeriodID;
                    sP.SchoolID = item.SchoolID;
                    sP.StudentID = item.StudentID;
                    sP.Period = item.Period;
                    sP.ClassroomName = item.ClassroomName;
                    sP.Debt = debt;
                    sP.Payment = payment;
                    sP.Balance = balance;
                }
                list.Add(sP);
            }
            return Json(list);
        }
        #endregion

        #region Others
        [Route("M300Collections/StudentDebtDataRead/{userID}/{studentid}")]
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
                        studentDebtViewModel.IsList = true;
                        studentDebtViewModel.IsList = debt.IsList;
                    }
                    list.Add(studentDebtViewModel);
                }
            }
            return Json(list);
        }

        [Route("M300Collections/SchoolDebtDataRead/{userID}/{period}/{studentid}")]
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

        //Installment
        [Route("M300Collections/InstallmentDataRead/{userID}/{period}/{studentid}")]
        public IActionResult InstallmentDataRead(int userID, string period, int studentid)
        {
            List<StudentInstallmentViewModel> list = new List<StudentInstallmentViewModel>();
            //var user = _usersRepository.GetUser(userID);
            //var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            //var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            //var statusID2 = _parameterRepository.GetParameterCategoryName("Borçlu").CategoryID;

            if (studentid != 0)
            {
                var user = _usersRepository.GetUser(userID);
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
                    bool isExist = false;
                    if (item.BankID > 0)
                    {
                        isExist = _bankRepository.ExistBank(item.SchoolID, (int)item.BankID);
                        if (isExist)
                        {
                            bank = bankList.FirstOrDefault(p => p.BankID == item.BankID).BankName;
                        }
                    }

                    var category = "";
                    if (item.CategoryID > 0)
                        if (user.SelectedCulture.Trim() == "en-US") category = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID).Language1;
                        else category = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID).CategoryName;

                    var status = "";
                    if (item.StatusCategoryID > 0)
                        if (user.SelectedCulture.Trim() == "en-US") status = _parameterRepository.GetParameter(item.StatusCategoryID).Language1;
                        else status = _parameterRepository.GetParameter(item.StatusCategoryID).CategoryName;


                    //// Bu bölüm 4 digit küsürat düzeltemesi için ilave edildi.
                    //decimal installmentAmount = Math.Round(item.InstallmentAmount, school.CurrencyDecimalPlaces,MidpointRounding.ToZero);
                    //decimal previousPayment = Math.Round(item.PreviousPayment, school.CurrencyDecimalPlaces, MidpointRounding.ToZero);
                    //if (installmentAmount == previousPayment && previousPayment > 0)
                    //{
                    //    status = "";
                    //    if (statusID > 0)
                    //        if (user.SelectedCulture.Trim() == "en-US") status = _parameterRepository.GetParameter(statusID).Language1;
                    //        else status = _parameterRepository.GetParameter(statusID).CategoryName;

                    //    if (item.StatusCategoryID != statusID)
                    //    {
                    //        item.StatusCategoryID = statusID;
                    //        item.InstallmentAmount = installmentAmount;
                    //        item.PreviousPayment = previousPayment;
                    //        _studentInstallmentRepository.UpdateStudentInstallment(item);
                    //    }
                    //    else 
                    //    {
                    //        item.InstallmentAmount = installmentAmount;
                    //        item.PreviousPayment = previousPayment;
                    //        _studentInstallmentRepository.UpdateStudentInstallment(item);
                    //    }
                    //}
                    //else
                    //{
                    //    if (installmentAmount != previousPayment && previousPayment > 0 && item.StatusCategoryID == statusID2)
                    //    {
                    //        item.PreviousPayment = 0;
                    //        item.PaymentDate = null;
                    //        item.AccountReceiptNo = 0;
                    //        _studentInstallmentRepository.UpdateStudentInstallment(item);
                    //    }
                    //}

                    //////////////////////////////////////////////////////////////////////////
                    var studentInstallmentViewModel = new StudentInstallmentViewModel
                    {
                        StudentInstallmentID = item.StudentInstallmentID,
                        SchoolID = item.SchoolID,
                        StudentID = item.StudentID,
                        InstallmentDate = item.InstallmentDate,
                        FeeName = item.FeeName,
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

        //Payments
        [Route("M300Collections/PaymentsDataRead/{userID}/{period}/{studentid}")]
        public IActionResult PaymentsDataRead(int userID, string period, int studentid)
        {
            List<StudentPayment> list = new List<StudentPayment>();

            if (studentid != 0)
            {
                var user = _usersRepository.GetUser(userID);
                var cash = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentid);
                decimal totaldebt = 0;
              
                if (cash != null && cash.CashPayment > 0)
                {
                    var payments = new StudentPayment();
                    payments.StudentPaymentID =
                    payments.StudentID = studentid;
                    payments.SchoolID = user.SchoolID;
                    payments.Period = user.UserPeriod;
                    payments.PaymentDate = cash.TransactionDate;
                    payments.ReceiptNo = cash.ReceiptNo;
                    payments.PaymentAmount = cash.CashPayment;
                    payments.BalanceAmount = cash.SubTotal - cash.CashPayment;
                    payments.AccountReceipt = cash.AccountReceipt;
                    payments.InstallmentNumbers = "";
                    payments.InstallmentIdentities = "";
                    payments.Explanation = "Peşin";
                    if (user.SelectedCulture.Trim() == "en-US")
                    {
                        payments.Explanation = "Cash";
                    }
                    list.Add(payments);
                }

                totaldebt = 0;
                var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentid, period);
                totaldebt = studentinstallment.Sum(p => p.InstallmentAmount);
                //if (cash != null && totaldebt > cash.CashPayment) totaldebt = totaldebt - cash.CashPayment;

                var studentpayments = _studentPaymentRepository.GetStudentPayment(user.SchoolID, period, studentid);
                foreach (var item in studentpayments)
                {
                    if (totaldebt >= item.PaymentAmount) totaldebt -= item.PaymentAmount;
                    item.BalanceAmount = totaldebt;
                    list.Add(item);
                }
            }
            return Json(list);
        }

        [Route("M300Collections/PaymentsDataPeriodRead/{period}/{userID}/{studentid}")]
        public IActionResult PaymentsDataPeriodRead(string period, int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var studentpayments = _studentPaymentRepository.GetStudentPayment(user.SchoolID, period, studentid);
            return Json(studentpayments);
        }

        [HttpPost]
        [Route("M300Collections/PaymentsDataCreate/{strResult}/{userID}")]
        public IActionResult PaymentsDataCreate([Bind(Prefix = "models")] string strResult, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            string userName = UserAccountWrite(userID);

            var json = new JavaScriptSerializer().Deserialize<List<StudentPayment>>(strResult);
            List<StudentPayment> list = new List<StudentPayment>();
            var i = 0;
            var sortOrder = 1;
            bool isFirst = true;
            var studentID = json[i].StudentID;
            var period = json[i].Period;
            string installmentNumbers = json[i].InstallmentNumbers;
            string installmentIdentities = json[i].InstallmentIdentities;

            decimal total = json[i].PaymentAmount;
            var date = json[i].PaymentDate;
            decimal payment = 0;
            decimal accountPayment = 0;

            var lastNumber = 0;
            var paymentReceiptNo = 0;
            string dateString = "";
            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            foreach (var item in json)
            {
                paymentReceiptNo = (int)json[i].ReceiptNo;
                var getCode = _studentPaymentRepository.GetStudentPaymentID(json[i].StudentPaymentID);

                if (ModelState.IsValid)
                {
                    var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
                    if (pserialNumbers == null)
                        pserialNumbers = new PSerialNumber();
                    lastNumber = pserialNumbers.AccountSerialNo += 1;
                    pserialNumbers.AccountSerialNo = lastNumber;

                    getCode = new StudentPayment();
                    getCode.StudentPaymentID = 0;
                    getCode.StudentID = json[i].StudentID;
                    getCode.SchoolID = user.SchoolID;
                    getCode.Period = json[i].Period;
                    getCode.PaymentDate = json[i].PaymentDate;
                    getCode.ReceiptNo = paymentReceiptNo;
                    getCode.PaymentAmount = json[i].PaymentAmount;
                    getCode.BalanceAmount = json[i].BalanceAmount;
                    getCode.AccountReceipt = lastNumber;
                    getCode.InstallmentNumbers = json[i].InstallmentNumbers;
                    getCode.InstallmentIdentities = json[i].InstallmentIdentities;
                    getCode.Explanation = json[i].Explanation;

                    list.Add(getCode);

                    _studentPaymentRepository.CreateStudentPayment(getCode);
                    _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

                    decimal totalPayment = json[i].PaymentAmount;

                    string identities = json[i].InstallmentIdentities;
                    string[] installmentIdentitiesDim = identities.Split(',');
                    var insID = 0;

                    foreach (var numberStr in installmentIdentitiesDim)
                    {
                        insID = Int32.Parse(numberStr);
                        var installment = _studentInstallmentRepository.GetStudentInstallmentID(user.SchoolID, insID);

                        payment = installment.InstallmentAmount - installment.PreviousPayment;
                        accountPayment = payment;
                        if (isFirst)
                        {
                            AccountCodeCreate(user.UserID, user.SchoolID, studentID, period, (int)installment.InstallmentNo);
                            AccountingCreate102(userID, userName, user.SchoolID, studentID, installment.StudentInstallmentID, period, (int)installment.InstallmentNo, total, lastNumber, paymentReceiptNo);
                            isFirst = false;
                        }

                        if (totalPayment >= payment && installment.StatusCategoryID != statusID)
                        {
                            installment.PreviousPayment += payment;
                            installment.StatusCategoryID = statusID;
                            totalPayment -= payment;
                            accountPayment = payment;
                        }
                        else if (totalPayment > 0)
                        {
                            installment.PreviousPayment += totalPayment;
                            accountPayment = totalPayment;
                            totalPayment -= totalPayment;
                        }

                        if (installment.AccountReceiptNo == 0)
                        {
                            installment.PaymentDate = date;
                            installment.AccountReceiptNo = lastNumber;
                        }
                        _studentInstallmentRepository.UpdateStudentInstallment(installment);

                        //StudentInstallmentPayment
                        var IP = new StudentInstallmentPayment();
                        IP.ID = 0;
                        IP.SchoolID = user.SchoolID;
                        IP.StudentID = studentID;
                        IP.Period = period;
                        IP.StudentInstallmentID = installment.StudentInstallmentID;
                        IP.StudentPaymentID = getCode.StudentPaymentID;
                        _studentInstallmentPaymentRepository.CreateStudentInstallmentPayment(IP);
                        ////////////////////////////

                        //121 ACOOUNT RECORDS 
                        sortOrder += 1;
                        AccountingCreate121(user.UserID, userName, user.SchoolID, studentID, period, installment.StudentInstallmentID, accountPayment, sortOrder, lastNumber, paymentReceiptNo);

                        string datex = string.Format("{0:dd/MM/yyyy}", installment.InstallmentDate);
                        dateString += datex + ",";
                    }
                }

                i = i + 1;
            }
            sortOrder += 1;
            AccountingCreate(user.UserID, userName, user.SchoolID, studentID, period, total, sortOrder, lastNumber, paymentReceiptNo);

            // Account number save for Collection
            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID);
            if (studentTemp != null)
            {
                studentTemp.StudentID = studentID;
                studentTemp.CollectionReceipt = lastNumber;
                _studentTempRepository.UpdateStudentTemp(studentTemp);
            }

            //////Users Log//////////////////
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(studentID);
            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = user.UserID;
            log.UserLogDate = DateTime.Now;

            log.TransactionID = _parameterRepository.GetParameterCategoryName("Tahsil İşlemi").CategoryID;
            log.UserLogDescription = student.FirstName + " " + student.LastName + " Taksit Tahsili, " + paymentReceiptNo + " Nolu Fiş, " + dateString + " Tarihli, " + "Toplam:" + total + ", Tahsil edildi.";

            if (user.SelectedCulture.Trim() == "en-US")
            {
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Installment Collection, " + paymentReceiptNo + " Receipt No, " + dateString + " Dated, " + "Total:" + total + ", Collected.";
            }
            else
            {
            }
            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return Json(list);
        }

        [Route("M300Collections/PaymentsRead/{userID}/{studentID}")]
        public IActionResult PaymentsRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            var period = user.UserPeriod;

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, period);

            decimal unbilled = 0; // Hesaplanacak
            decimal partial = 0;
            decimal payment = studentTemp.CashPayment;
            decimal debt = 0;
            decimal refund = studentTemp.RefundAmount1 + studentTemp.RefundAmount2 + studentTemp.RefundAmount3;
            decimal collections = 0;
            decimal balance = 0;

            foreach (var item in studentInstallment)
            {
                if (item.PreviousPayment > 0 && item.InstallmentAmount == item.PreviousPayment) payment += item.PreviousPayment;
                if (item.PreviousPayment > 0 && item.InstallmentAmount != item.PreviousPayment) partial += item.PreviousPayment;
                balance += item.InstallmentAmount;
            }
            balance += studentTemp.CashPayment;
            string[] dim1;
            if (user.SelectedCulture.Trim() == "tr-TR")
            {
                dim1 = new string[] { "Kesilmeyen Fatura", "Toplam Borç", "İade Edilen Tutar", "Toplam Tahsil", "Toplam Bakiye" };
            }
            else
            {
                dim1 = new string[] { "Unbilled ", "Total Debt", "Refund Amount", "Total Collections", "Total Balance" };
            }

            collections = (payment + partial);

            decimal[] dim2 = new decimal[] { unbilled, debt, refund, collections, balance };

            List<StudentPaymentInfo> list = new List<StudentPaymentInfo>();

            var inx = 0;
            for (int i = 0; i < 5; i++)
            {
                var studentPaymentInfo = new StudentPaymentInfo
                {
                    ViewModelId = inx,
                    Name = dim1[inx],
                    Payment = dim2[inx],
                };
                inx += 1;
                list.Add(studentPaymentInfo);
            }
            return Json(list);
        }

        [Route("M300Collections/PeriodDataRead/{plusYear}")]
        public IActionResult PeriodDataRead(int plusYear)
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, plusYear);
            return Json(mylist);
        }
        public IActionResult PaymentTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);

            return Json(registrationType);
        }

        [Route("M300Collections/TempDataDelete/{studentID}")]
        public IActionResult TempDataDelete(int studentID)
        {
            var tempData = _tempDataRepository.GetTempData(studentID);
            foreach (var item in tempData)
            {
                _tempDataRepository.DeleteTempData(item);
            }

            return Json(true);
        }

        [Route("M300Collections/PaymentDelete/{userID}/{studentID}/{accountReceiptNo}")]
        public IActionResult PaymentDelete(int userID, int studentID, int accountReceiptNo)
        {
            var user = _usersRepository.GetUser(userID);
            var payment = _studentPaymentRepository.GetStudentAccountReceipt(user.SchoolID, user.UserPeriod, studentID, accountReceiptNo);
            decimal totalPayment = 0;
            if (payment != null)
            {
                totalPayment = payment.PaymentAmount;
                _studentPaymentRepository.DeleteStudentPayment(payment);

                var IP = _studentInstallmentPaymentRepository.GetStudentInstallmentPayment2(user.UserPeriod, studentID, payment.StudentPaymentID);
                foreach (var ip in IP)
                {
                    _studentInstallmentPaymentRepository.DeleteStudentInstallmentPayment(ip);
                }
            }

            var remainderPayment = _studentPaymentRepository.GetPaymentOrder(user.SchoolID, user.UserPeriod, studentID);
            int accountReceipt = 0;
            if (remainderPayment.Count() > 0)
            {
                accountReceipt = (int)remainderPayment.Max(a => a.AccountReceipt);
            }
            var studentInstallments = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, user.UserPeriod);

            var statusID = _parameterRepository.GetParameterCategoryName("Borçlu").CategoryID;
            foreach (var item in studentInstallments)
            {
                if (item.PreviousPayment > 0)
                {
                    if (item.AccountReceiptNo == accountReceiptNo)
                    {
                        var studentInstallment = _studentInstallmentRepository.GetStudentInstallmentID(item.SchoolID, item.StudentInstallmentID);
                        if (totalPayment >= item.PreviousPayment)
                        {
                            item.PaymentDate = null;
                            totalPayment -= item.PreviousPayment;
                            item.PreviousPayment -= item.PreviousPayment;
                            item.StatusCategoryID = statusID;
                            if (totalPayment > 0)
                                item.AccountReceiptNo = accountReceipt;
                            else item.AccountReceiptNo = 0;
                            _studentInstallmentRepository.UpdateStudentInstallment(studentInstallment);
                        }
                        else
                        {
                            if (totalPayment > 0)
                                item.AccountReceiptNo = accountReceipt;
                            else item.AccountReceiptNo = 0;
                            item.PreviousPayment -= totalPayment;
                            _studentInstallmentRepository.UpdateStudentInstallment(studentInstallment);
                        }
                    }
                }

                var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, accountReceiptNo);
                foreach (var acc in accounting)
                {
                    var account = _accountingRepository.GetAccountingID(acc.AccountingID);
                    _accountingRepository.DeleteAccounting(account);
                }
            }


            //////Users Log//////////////////
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(studentID);
            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = user.UserID;

            log.UserLogDate = DateTime.Now;
            string date = string.Format("{0:dd/MM/yyyy}", payment.PaymentDate);
            decimal unitPricex = Math.Round(payment.PaymentAmount, school.CurrencyDecimalPlaces);

            if (user.SelectedCulture.Trim() == "en-US")
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Collection Cancellation").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Cancellation of a Charged Registration, " + date + " Dated, " + "Amount:" + unitPricex + ", Collection Transaction CANCELED.";
            }
            else
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Tahsilat İptali").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Tahsil Edilen Bir Kaydın İptali, " + date + " Tarihli, " + "Tutarı:" + unitPricex + ", Tahsil İşlemi İPTAL Edildi.";
            }

            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return Json(true);
        }

        [HttpPost]
        [Route("M300Collections/ReceiptNoRead/{userID}/{schoolID}/{categoryName}")]
        public IActionResult ReceiptNoRead(int userID, int schoolID, string categoryName)
        {
            var user = _usersRepository.GetUser(userID);

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();
            var collectionNo = pserialNumbers.CollectionNo += 1;
            pserialNumbers.CollectionNo = collectionNo;
            pserialNumbers.PaymentReceiptNo = collectionNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            string check = Resources.Resource.Check;
            string bond = Resources.Resource.FromHand;
            var checkOrFromHand = false;
            if (categoryName != null)
            {
                string[] categoryNameDim = categoryName.Split(',');
                foreach (var category in categoryNameDim)
                {
                    if (category == check || category == bond) { checkOrFromHand = true; };
                }
            }

            return Json(new { ReceiptNo = collectionNo, CheckOrFromHand = checkOrFromHand });
        }

        public IActionResult ReceiptNoUpdate(int schoolID, int studentID, string period, int receiptNo)
        {
            var studentTemp = _studentTempRepository.GetStudentTemp(schoolID, period, studentID);
            if (studentTemp != null)
            {
                studentTemp.CollectionReceipt = receiptNo;
                _studentTempRepository.UpdateStudentTemp(studentTemp);
            }

            return Json(true);
        }

        public IActionResult StudentTempUpdate(int schoolID, int studentID, string period, string accountCode, string accountExplanation, bool isFormPrint)
        {
            var studentTemp = _studentTempRepository.GetStudentTemp(schoolID, period, studentID);
            if (studentTemp != null)
            {
                if (studentTemp.AccountCode != accountCode && studentTemp.AccountExplanation != accountExplanation)
                {
                    studentTemp.AccountCode = accountCode;
                    studentTemp.AccountExplanation = accountExplanation;
                    if (accountCode != null && accountExplanation != null)
                        _studentTempRepository.UpdateStudentTemp(studentTemp);
                }
            }

            var school = _schoolInfoRepository.GetSchoolInfo(schoolID);
            if (school.IsFormPrint != isFormPrint)
            {
                school.IsFormPrint = isFormPrint;
                _schoolInfoRepository.UpdateSchoolInfo(school);
            }

            return Json(true);
        }
        public IActionResult StudentTempRead(int studentID, string period, int schoolID)
        {
            var school = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var studentTemp = _studentTempRepository.GetStudentTemp(schoolID, period, studentID);
            var accountCode = school.AccountNoID01;
            var explanation = "";
            if (studentTemp != null)
            {
                if (studentTemp.AccountCode == null) explanation = studentTemp.AccountExplanation;
            }
            return Json(new { accountCode = accountCode, accountExplanation = explanation, isFormPrint = school.IsFormPrint });
        }


        [Route("M300Collections/AccountCodesRead/{period}")]
        public IActionResult AccountCodesRead(string period)
        {
            var accountingCode = _accountCodesRepository.GetAccountCodeAllTrue(period);
            return Json(accountingCode);
        }

        #endregion

        #region Accounting
        public void AccountCodeCreate(int userID, int schoolID, int studentID, string period, int installmentNo)
        {
            var user = _usersRepository.GetUser(userID);

            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var student = _studentRepository.GetStudent(studentID);
            int bankID = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID).BankID;

            string bankName = "", accountdeptCode = "", accountDecimation = "", accountNo = "";
            if (bankID > 0)
            {
                bankName = _bankRepository.GetBank(bankID).BankName;
                accountdeptCode = _bankRepository.GetBank(bankID).BankBranchCode;
                accountDecimation = _bankRepository.GetBank(bankID).AccountDecimation;
                accountNo = _bankRepository.GetBank(bankID).AccountNo;
            }

            string studentNo = student.StudentSerialNumber.ToString();
            string studentName = student.FirstName + " " + student.LastName + "'ün ";

            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);

            string periodTxt = " Dönemi, ";
            string yearTxt = " Yılı ";
            string monthTxt = " Ayı ";

            string deptCode = schoolInfo.CompanyShortCode;
            string deptName = schoolInfo.CompanyShortName + " Şb. ";
            string deptText = " Nolu Şb.";
            string bankAccountText = "Nolu Hesap ";
            string accountSubText = "Hesap ";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                periodTxt = " Dated, ";
                yearTxt = " Year ";
                monthTxt = " Month ";

                deptName = schoolInfo.CompanyShortName + " Branch. ";
                deptText = " Branch No.";
                bankAccountText = "Account No. ";
                accountSubText = "Account ";
            }


            string controlCode = schoolInfo.AccountNoID07;
            if (controlCode == null) controlCode = "102";

            string code = controlCode;

            controlCode = controlCode + " " + accountdeptCode;
            string codeName = bankName + " " + accountdeptCode + deptText;
            bool isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            controlCode = code + " " + accountdeptCode + " " + accountDecimation;
            codeName = bankName + " " + accountdeptCode + deptText + ", " + accountDecimation + " " + bankAccountText;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            controlCode = code + " " + accountdeptCode + " " + accountDecimation + " " + deptCode;
            codeName = bankName + " " + accountdeptCode + deptText + ", " + accountDecimation + " " + bankAccountText + ", " + deptName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            controlCode = code + " " + accountdeptCode + " " + accountDecimation + " " + deptCode + " " + accountNo;
            codeName = bankName + " " + accountdeptCode + deptText + ", " + accountDecimation + " " + bankAccountText + ", " + deptName + " " + accountNo + " " + accountSubText;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            ///////////////////////////  121 ////////////////////////////////////////
            controlCode = schoolInfo.AccountNoID05;
            if (controlCode == null) controlCode = "121";
            string code121 = controlCode;
            codeName = periodCode;
            string mainCodeName = codeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
            else
            {
                controlCode = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCodeName;
            }

            //121 2122 Dönemi, ALACAK SENETLERİ
            controlCode = code121 + " " + periodCode;
            codeName = periodCode + periodTxt + ", " + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            //101, 121, 120, 102, 108, 108.01, 600 ,102 ACCOUNT CODE CREATE 
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, period);
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            foreach (var item in studentinstallment)
            {
                DateTime dt = Convert.ToDateTime(item.InstallmentDate);
                int MM = dt.Month;
                string YY = dt.ToString("yy");

                //121 2122 22 Yılı, ALACAK SENETLERİ
                controlCode = code121 + " " + periodCode + " " + YY;
                codeName = periodCode + periodTxt + YY + yearTxt + ", " + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                var param = parameters.FirstOrDefault(p => p.CategoryID == item.CategoryID);
                string categoryName = "";
                if (user.SelectedCulture.Trim() == "en-US")
                    categoryName = param.Language1;
                else categoryName = param.CategoryName;

                if (categoryName == "Banka" || categoryName == "BANKA") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Çek" || categoryName == "ÇEK") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
                if (categoryName == "Elden" || categoryName == "ELDEN") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Kmh" || categoryName == "KMH") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
                if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
                if (categoryName == "Ots_1" || categoryName == "OTS_1") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Ots_2" || categoryName == "OTS_2") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Teşvik" || categoryName == "TEŞVİK") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };

                if (user.SelectedCulture.Trim() == "en-US")
                {
                    if (categoryName == "Bank" || categoryName == "BANK") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                    if (categoryName == "Check" || categoryName == "CHECK") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
                    if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                    if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
                    if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
                    if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
                    if (categoryName == "Ots_1" || categoryName == "OTS_1") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                    if (categoryName == "Ots_2" || categoryName == "OTS_2") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                    if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") { code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
                }

                if (code == "101") mainCodeName = "ALINAN ÇEKLER HESABI";
                if (code == "121") mainCodeName = "ALACAK SENETLERİ";
                if (code == "120") mainCodeName = "ALICILAR HESABI";
                if (code == "102") mainCodeName = "BANKALAR";
                if (code == "108") mainCodeName = "DİĞER HAZIR DEĞERLER";
                if (code == "108 01") mainCodeName = "KREDİ KARTI";

                //121 2122 22/03 Ayı, ALACAK SENETLERİ
                controlCode = code + " " + periodCode;
                codeName = periodCode + periodTxt + ", " + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                controlCode = code + " " + periodCode + " " + YY;
                codeName = periodCode + periodTxt + YY + yearTxt + ", " + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                controlCode = code + " " + periodCode + " " + YY + " " + MM;
                codeName = periodCode + periodTxt + YY + "/" + MM + monthTxt + ", " + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                //121 2122 22/03 Ayı, NCS Şube, ALACAK SENETLERİ
                controlCode = code + " " + periodCode + " " + YY + " " + MM + " " + deptCode;
                codeName = periodCode + periodTxt + YY + "/" + MM + monthTxt + ", " + deptName + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

            }

            ///////////////////////////  340 ////////////////////////////////////////
            controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";
            string code340 = controlCode;
            codeName = "ALINAN SİPARİŞ AVANSLARI";
            mainCodeName = codeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
            else
            {
                controlCode = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID02, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID02, period).AccountCodeName;
            }

            //340 2021
            controlCode = code340 + " " + periodCode;
            codeName = periodCode + periodTxt + ", " + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            //340 2021 02
            controlCode = code340 + " " + periodCode + " " + deptCode;
            codeName = periodCode + periodTxt + deptName + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            //340 2021 02 1092
            controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;
            codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            ///////////////////////////  600 ////////////////////////////////////////
            controlCode = schoolInfo.AccountNoID03;
            if (controlCode == null) controlCode = "600";
            string code600 = controlCode;
            codeName = "YURT İÇİ SATIŞLAR";
            mainCodeName = codeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
            else
            {
                controlCode = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID03, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID03, period).AccountCodeName;
            }

            //600 2021
            controlCode = code600 + " " + periodCode;
            codeName = periodCode + periodTxt + ", " + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            //600 2021 02
            controlCode = code600 + " " + periodCode + " " + deptCode;
            codeName = periodCode + periodTxt + deptName + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            //600 2021 02 01
            string depttxt = "01 Nolu, ";
            controlCode = code600 + " " + periodCode + " " + deptCode + " " + "01";
            codeName = periodCode + periodTxt + deptName + depttxt + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
        }
        public void AccountingCreate102(int userID, string userName, int schoolID, int studentID, int studentInstallmentID, string period, int installmentNo, decimal total, int lastNumber, int paymentReceiptNo)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var student = _studentRepository.GetStudent(studentID);
            var installment = _studentInstallmentRepository.GetStudentInstallmentNo(studentID, studentInstallmentID, period, installmentNo);

            var checkOrFromHand = "";
            if (user.SelectedCulture.Trim() == "en-US") checkOrFromHand = _parameterRepository.GetParameter(installment.CategoryID).Language1;
            else checkOrFromHand = _parameterRepository.GetParameter(installment.CategoryID).CategoryName;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            string check = Resources.Resource.Check;
            string bond = Resources.Resource.FromHand;
            var process100or102 = false;
            if (checkOrFromHand == check || checkOrFromHand == bond)
            {
                process100or102 = true;
            }
            /////////////////////////////////////////

            int bankID = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID).BankID;
            string bankName = "", accountNo = "", accountdeptCode = "", accountDecimation = "", deptCode = "";

            if (bankID > 0)
            {
                bankName = _bankRepository.GetBank(bankID).BankName;
                accountNo = _bankRepository.GetBank(bankID).AccountNo;
                accountdeptCode = _bankRepository.GetBank(bankID).BankBranchCode;
                accountDecimation = _bankRepository.GetBank(bankID).AccountDecimation;
            }

            deptCode = schoolInfo.CompanyShortCode;

            var sortOrder = 1;
            var accounting = new Accounting();
            string controlCode = schoolInfo.AccountNoID07;
            if (controlCode == null) controlCode = "102";
            string deptText = " Nolu Şb.";
            var exp1 = " Nolu ";
            var exp2 = " Tahsil Dekontu ";
            var exp22 = " Nakit Tahsili ";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                deptText = " Branch No.";
                exp1 = " Numbered ";
                exp2 = " Collection Receipt ";
                exp22 = " Cash Collection ";
            }

            controlCode = controlCode + " " + accountdeptCode + " " + accountDecimation + " " + deptCode + " " + accountNo;
            string codeName = bankName + " " + accountdeptCode + deptText;

            if (process100or102)
            {
                var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID);
                controlCode = studentTemp.AccountCode;
                if (controlCode == null) { controlCode = schoolInfo.AccountNoID01; }
                if (controlCode == null) { controlCode = "100"; }

                exp2 = exp22;
                controlCode = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;

                installment.Explanation = studentTemp.AccountExplanation;
                _studentInstallmentRepository.UpdateStudentInstallment(installment);
            }

            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            if (schoolInfo.NewPeriod == user.UserPeriod)
                classroomID = student.ClassroomID;
            else
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(user.SchoolID, studentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(user.SchoolID, studentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                        classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            //100 or 102 ACOOUNT RECORDS 
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = userName;
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
            accounting.Debt = total;
            accounting.Credit = 0;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);
        }
        public void AccountingCreate(int userID, string userName, int schoolID, int studentID, string period, decimal total, int sortOrder, int lastNumber, int paymentReceiptNo)
        {
            var user = _usersRepository.GetUser(userID);

            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var student = _studentRepository.GetStudent(studentID);
            int bankID = _studentTempRepository.GetStudentTemp(user.SchoolID, period, studentID).BankID;

            string bankName = "";
            if (bankID > 0)
            {
                bankName = _bankRepository.GetBank(bankID).BankName;
            }

            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string deptCode = schoolInfo.CompanyShortCode;
            string studentNo = student.StudentSerialNumber.ToString();

            var accounting = new Accounting();

            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            if (schoolInfo.NewPeriod == user.UserPeriod)
                classroomID = student.ClassroomID;
            else
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(user.SchoolID, studentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(user.SchoolID, studentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                        classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            var exp1 = " ";
            var exp2 = " Tarihli Tahsilat ";

            if (user.SelectedCulture.Trim() == "en-US")
            {
                exp2 = " Dated Collection ";
            }

            //340 ACOOUNT RECORDS 
            string controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";
            string code340 = controlCode;

            controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = userName;
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
            accounting.Debt = total;
            accounting.Credit = 0;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

            //600 ACOOUNT RECORDS 
            controlCode = schoolInfo.AccountNoID03;
            if (controlCode == null) controlCode = "600";
            string code600 = controlCode;
            controlCode = code600 + " " + periodCode + " " + deptCode + " " + "01";

            sortOrder += 1;
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = userName;
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
            accounting.Debt = 0;
            accounting.Credit = total;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);
        }
        public void AccountingCreate121(int userID, string userName, int schoolID, int studentID, string period, int installmentID, decimal payment, int sortOrder, int lastNumber, int paymentReceiptNo)
        {
            var user = _usersRepository.GetUser(userID);

            ////121 ACOOUNT RECORDS 
            var installment = _studentInstallmentRepository.GetStudentInstallmentID(user.SchoolID, installmentID);
            DateTime dt = Convert.ToDateTime(installment.InstallmentDate);
            int MM = dt.Month;
            string YY = dt.ToString("yy");
            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);

            var accountingCode = new AccountCodes();
            var accounting = new Accounting();
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var student = _studentRepository.GetStudent(studentID);
            string deptCode = schoolInfo.CompanyShortCode;

            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            if (schoolInfo.NewPeriod == user.UserPeriod)
                classroomID = student.ClassroomID;
            else
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(user.SchoolID, studentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(user.SchoolID, studentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                        classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            string bankName = "";
            if (installment.BankID > 0)
            {
                bankName = _bankRepository.GetBank((int)installment.BankID).BankName;
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string code = null;

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;

            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.CodeTypeName = userName;
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;

            var paymentTypetxt = "";
            var param = parameters.FirstOrDefault(p => p.CategoryID == installment.CategoryID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = param.Language1;
            else categoryName = param.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") { paymentTypetxt += "Banka Senet"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
            if (categoryName == "Çek" || categoryName == "ÇEK") { paymentTypetxt += "Çek"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
            if (categoryName == "Elden" || categoryName == "ELDEN") { paymentTypetxt += "Elden Senet"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
            if (categoryName == "Kmh" || categoryName == "KMH") { paymentTypetxt += "KMH"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") { paymentTypetxt += "Kredi kartı"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { paymentTypetxt += "Mail order"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
            if (categoryName == "Ots_1" || categoryName == "OTS_1") { paymentTypetxt += "OTS1"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
            if (categoryName == "Ots_2" || categoryName == "OTS_2") { paymentTypetxt += "OTS2"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") { paymentTypetxt += "Teşvik"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") { paymentTypetxt += "Bank Bond"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Check" || categoryName == "CHECK") { paymentTypetxt += "Check"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") { paymentTypetxt += "Bond by Hand"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") { paymentTypetxt += "Overdraft Account"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") { paymentTypetxt += "Credit Card"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { paymentTypetxt += "Mail order"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
                if (categoryName == "Ots_1" || categoryName == "OTS_1") { paymentTypetxt += "OTS1"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Ots_2" || categoryName == "OTS_2") { paymentTypetxt += "OTS2"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") { paymentTypetxt += "Gov.Support"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
            }
            var exp3 = " Nolu Makbuz, ";
            var exp4 = " Tarihli " + paymentTypetxt + " Tahsili ";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                exp3 = "Receipt No, ";
                exp4 = " Dated " + paymentTypetxt + " Collection ";
            }

            string controlCode = code;
            if (controlCode == null) controlCode = "121";
            controlCode = code + " " + periodCode + " " + YY + " " + MM + " " + deptCode;

            accounting.AccountCode = controlCode;

            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;


            int DD = dt.Day;
            if (categoryName == "Elden" || categoryName == "Çek")
                accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp3 + installment.InstallmentNo + " Nolu ve " + DD + "/" + MM + "/" + YY + exp4;
            else
                accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp3 + installment.InstallmentNo + " Nolu ve " + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bond by Hand" || categoryName == "Check")
                    accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp3 + installment.InstallmentNo + " No. and " + DD + "/" + MM + "/" + YY + exp4;
                else
                    accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp3 + installment.InstallmentNo + " No. and " + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

            }
            accounting.Debt = 0;
            accounting.Credit = payment;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

        }
        private void CodeCreate(AccountCodes accountingCode, string controlCode, string codeName)
        {
            accountingCode.AccountCodeID = 0;
            accountingCode.Period = accountingCode.Period;
            accountingCode.AccountCode = controlCode;
            if (codeName.Length > 100)
                accountingCode.AccountCodeName = codeName.Substring(0, 100);
            else accountingCode.AccountCodeName = codeName;
            accountingCode.IsActive = true;
            _accountCodesRepository.CreateAccountCode(accountingCode);
        }
        #endregion

        #region Add/Change/Installment

        [Route("M300Collections/FeeTypeCombo/{userID}")]
        public IActionResult FeeTypeCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolFee = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1");
            return Json(schoolFee);
        }

        public IActionResult BankNameCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var bankNameType = _bankRepository.GetBankAll(user.SchoolID);
            return Json(bankNameType);
        }

        [Route("M300Collections/Div1Update/{userID}/{studentID}/{feeNameID}/{unitPrice}/{dateString}/{paymentNameID}/{bankNameID}")]
        public IActionResult Div1Update(int userID, int studentID, int feeNameID, decimal unitPrice, string dateString, int paymentNameID, int bankNameID)
        {
            DateTime transactiondate = DateTime.Parse(dateString);
            var user = _usersRepository.GetUser(userID);
            //Add Record
            var student = _studentRepository.GetStudent(studentID);
            var typeId = 0;
            if (student.ClassroomID > 0)
                typeId = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomTypeID;

            var debt = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeNameID);
            debt.UnitPrice += unitPrice;
            debt.Amount += unitPrice;
            debt.ClassroomTypeID = typeId;
            _studentDebtRepository.UpdateStudentDebt(debt);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            //New Record
            var inst = new StudentInstallment();

            var installmentNo = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            var param = parameters.FirstOrDefault(p => p.CategoryID == paymentNameID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = param.Language1;
            else categoryName = param.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") inst.InstallmentNo = installmentNo.BondNo += 1;
            if (categoryName == "Çek" || categoryName == "ÇEK") inst.InstallmentNo = installmentNo.CheckNo += 1;
            if (categoryName == "Elden" || categoryName == "ELDEN") inst.InstallmentNo = installmentNo.BondNo += 1;
            if (categoryName == "Kmh" || categoryName == "KMH") inst.InstallmentNo = installmentNo.KmhNo += 1;
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") inst.InstallmentNo = installmentNo.CreditCardNo += 1;
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") inst.InstallmentNo = installmentNo.MailOrderNo += 1;
            if (categoryName == "Ots_1" || categoryName == "OTS_1") inst.InstallmentNo = installmentNo.OtsNo1 += 1;
            if (categoryName == "Ots_2" || categoryName == "OTS_2") inst.InstallmentNo = installmentNo.OtsNo2 += 1;
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") inst.InstallmentNo = installmentNo.GovernmentPromotionNo += 1;

            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") inst.InstallmentNo = installmentNo.BondNo += 1;
                if (categoryName == "Check" || categoryName == "CHECK") inst.InstallmentNo = installmentNo.CheckNo += 1;
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") inst.InstallmentNo = installmentNo.BondNo += 1;
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") inst.InstallmentNo = installmentNo.KmhNo += 1;
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") inst.InstallmentNo = installmentNo.CreditCardNo += 1;
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") inst.InstallmentNo = installmentNo.MailOrderNo += 1;
                if (categoryName == "Ots_1" || categoryName == "OTS_1") inst.InstallmentNo = installmentNo.OtsNo1 += 1;
                if (categoryName == "Ots_2" || categoryName == "OTS_2") inst.InstallmentNo = installmentNo.OtsNo2 += 1;
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") inst.InstallmentNo = installmentNo.GovernmentPromotionNo += 1;
            }

            _pSerialNumberRepository.UpdatePSerialNumber(installmentNo);

            var statusID = _parameterRepository.GetParameterCategoryName("Borçlu").CategoryID;
            inst.StudentInstallmentID = 0;
            inst.SchoolID = user.SchoolID;
            inst.StudentID = studentID;
            inst.Period = user.UserPeriod;
            inst.InstallmentDate = transactiondate;
            inst.CategoryID = paymentNameID;
            inst.InstallmentAmount = unitPrice;
            inst.PreviousPayment = 0;
            inst.BankID = bankNameID;
            inst.CheckCardNo = "";
            inst.FeeName = _schoolFeeRepository.GetSchoolFee(feeNameID).Name;
            inst.IsPrint = false;
            inst.StatusCategoryID = statusID;
            inst.AccountReceiptNo = 0;
            inst.PaymentDate = null;
            inst.Explanation = "";
            inst.CheckBankName = "";
            inst.CheckNo = "";
            inst.Drawer = "";
            inst.Endorser = "";
            _studentInstallmentRepository.CreateStudentInstallment(inst);

            //121 ACOOUNT RECORDS 
            var lastNumber = 0;
            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();
            lastNumber = pserialNumbers.AccountSerialNo += 1;
            pserialNumbers.AccountSerialNo = lastNumber;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            Div1AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, unitPrice, dateString, paymentNameID, bankNameID, lastNumber);

            //////Users Log//////////////////
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = user.UserID;
            log.UserLogDate = DateTime.Now;
            decimal unitPricex = Math.Round(unitPrice, school.CurrencyDecimalPlaces);
            if (user.SelectedCulture.Trim() == "en-US")
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Creating a New Installment").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " New Installment Entry Process, " + dateString + " Dated, " + "Amount:" + unitPricex + ", A New Debt Entry Has Been Made.";
            }
            else
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Yeni Taksit Oluşturma").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Yeni Taksit Giriş İşlemi, " + dateString + " Tarihli, " + "Tutarı:" + unitPricex + ", Yeni bir Borç Girişi Yapıldı.";
            }

            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return Json(true);
        }

        [Route("M300Collections/Div2Update/{userID}/{studentID}/{amount}/{dateString}/{paymentNameID}/{bankNameID}/{studentInstallmentID}/{isSingle}")]
        public IActionResult Div2Update(int userID, int studentID, decimal amount, string dateString, int paymentNameID, int bankNameID, int studentInstallmentID, bool isSingle)
        {
            DateTime transactiondate = DateTime.Parse(dateString);
            var user = _usersRepository.GetUser(userID);
            //Subtraction process record
            var ins = _studentInstallmentRepository.GetStudentInstallment2(user.SchoolID, studentID, user.UserPeriod, studentInstallmentID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            if (!isSingle)
            {
                ins.InstallmentAmount -= amount;
                _studentInstallmentRepository.UpdateStudentInstallment(ins);
            }

            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            //Other Installments will Deleted
            if (isSingle)
            {
                var lastNumber = 0;
                var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
                int sortOrder = 1;
                int loop = 0;
                var insSingle = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, user.UserPeriod);

                foreach (var item in insSingle)
                {
                    if (item.StatusCategoryID != statusID)
                    {
                        if (item.PreviousPayment > 0)
                        {
                            item.InstallmentAmount = item.PreviousPayment;
                            item.StatusCategoryID = statusID;
                            _studentInstallmentRepository.UpdateStudentInstallment(ins);
                        }
                        else
                        {
                            _studentInstallmentRepository.DeleteStudentInstallment(item);
                        }

                        //Account121 and 340
                        decimal amount1 = item.InstallmentAmount;

                        if (pserialNumbers == null)
                            pserialNumbers = new PSerialNumber();
                        lastNumber = pserialNumbers.AccountSerialNo += 1;
                        pserialNumbers.AccountSerialNo = lastNumber;

                        string dateStringOld = item.InstallmentDate.ToString();
                        loop = 1;
                        Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount1, dateStringOld, item.CategoryID, (int)item.BankID, lastNumber, sortOrder, loop);
                    }
                }

                //Account121 and 340
                if (pserialNumbers == null)
                    pserialNumbers = new PSerialNumber();
                lastNumber = pserialNumbers.AccountSerialNo += 1;
                pserialNumbers.AccountSerialNo = lastNumber;
                _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);
                loop = 1;
                sortOrder += 1;
                if (isSingle)
                {
                    loop = 2;
                    Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateString, paymentNameID, bankNameID, lastNumber, sortOrder, loop);
                }
                else
                {
                    Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateString, paymentNameID, bankNameID, lastNumber, sortOrder, loop);
                    loop = 2;
                    sortOrder += 1;
                    Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateString, paymentNameID, bankNameID, lastNumber, sortOrder, loop);
                }
            }

            //New Record
            var inst = new StudentInstallment();
            var installmentNo = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);

            var param = parameters.FirstOrDefault(p => p.CategoryID == paymentNameID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = param.Language1;
            else categoryName = param.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") inst.InstallmentNo = installmentNo.BondNo += 1;
            if (categoryName == "Çek" || categoryName == "ÇEK") inst.InstallmentNo = installmentNo.CheckNo += 1;
            if (categoryName == "Elden" || categoryName == "ELDEN") inst.InstallmentNo = installmentNo.BondNo += 1;
            if (categoryName == "Kmh" || categoryName == "KMH") inst.InstallmentNo = installmentNo.KmhNo += 1;
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") inst.InstallmentNo = installmentNo.CreditCardNo += 1;
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") inst.InstallmentNo = installmentNo.MailOrderNo += 1;
            if (categoryName == "Ots_1" || categoryName == "OTS_1") inst.InstallmentNo = installmentNo.OtsNo1 += 1;
            if (categoryName == "Ots_2" || categoryName == "OTS_2") inst.InstallmentNo = installmentNo.OtsNo2 += 1;
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") inst.InstallmentNo = installmentNo.GovernmentPromotionNo += 1;
            statusID = _parameterRepository.GetParameterCategoryName("Borçlu").CategoryID;

            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") inst.InstallmentNo = installmentNo.BondNo += 1;
                if (categoryName == "Check" || categoryName == "CHECK") inst.InstallmentNo = installmentNo.CheckNo += 1;
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") inst.InstallmentNo = installmentNo.BondNo += 1;
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") inst.InstallmentNo = installmentNo.KmhNo += 1;
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") inst.InstallmentNo = installmentNo.CreditCardNo += 1;
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") inst.InstallmentNo = installmentNo.MailOrderNo += 1;
                if (categoryName == "Ots_1" || categoryName == "OTS_1") inst.InstallmentNo = installmentNo.OtsNo1 += 1;
                if (categoryName == "Ots_2" || categoryName == "OTS_2") inst.InstallmentNo = installmentNo.OtsNo2 += 1;
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") inst.InstallmentNo = installmentNo.GovernmentPromotionNo += 1;
                statusID = _parameterRepository.GetParameterCategoryName("Debtor").CategoryID;
            }

            _pSerialNumberRepository.UpdatePSerialNumber(installmentNo);

            inst.StudentInstallmentID = 0;
            inst.SchoolID = user.SchoolID;
            inst.StudentID = studentID;
            inst.Period = user.UserPeriod;
            inst.InstallmentDate = transactiondate;
            inst.CategoryID = paymentNameID;
            inst.InstallmentAmount = amount;
            inst.PreviousPayment = 0;
            inst.BankID = bankNameID;
            inst.CheckCardNo = "";
            inst.FeeName = ins.FeeName;
            inst.IsPrint = false;
            inst.StatusCategoryID = statusID;
            inst.AccountReceiptNo = 0;
            inst.PaymentDate = null;
            inst.Explanation = "";
            inst.CheckBankName = "";
            inst.CheckNo = "";
            inst.Drawer = "";
            inst.Endorser = "";
            _studentInstallmentRepository.CreateStudentInstallment(inst);

            //121 ACOOUNT RECORDS 
            if (!isSingle)
            {
                var lastNumber = 0;
                var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
                if (pserialNumbers == null)
                    pserialNumbers = new PSerialNumber();
                lastNumber = pserialNumbers.AccountSerialNo += 1;
                pserialNumbers.AccountSerialNo = lastNumber;
                int sortOrder = 1;
                int loop = 1;

                ins = _studentInstallmentRepository.GetStudentInstallment2(user.SchoolID, studentID, user.UserPeriod, studentInstallmentID);
                string dateStringOld = ins.InstallmentDate.ToString();
                Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateStringOld, ins.CategoryID, (int)ins.BankID, lastNumber, sortOrder, loop);
                loop = 2;
                sortOrder = 3;
                Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateString, paymentNameID, bankNameID, lastNumber, sortOrder, loop);
            }

            //////Users Log//////////////////
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(studentID);
            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = user.UserID;
            log.UserLogDate = DateTime.Now;
            decimal unitPricex = Math.Round(amount, school.CurrencyDecimalPlaces);

            if (user.SelectedCulture.Trim() == "en-US")
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Installment Divide").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Installment Amount Divided, " + dateString + " New Dated, " + "Divided Amount:" + unitPricex + ", Registered as a New Debt Entry.";
            }
            else
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Taksit Bölme").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Taksit Tutarı Bölündü, " + dateString + " Yeni Tarihli, " + "Bölünen Tutar:" + unitPricex + ", Yeni bir Borç Girişi olarak kayıt Yapıldı.";
            }

            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return Json(true);
        }

        [Route("M300Collections/Div3Update/{userID}/{studentID}/{dateString}/{paymentNameID}/{bankNameID}/{studentInstallmentID}/{isNewNumber}")]
        public IActionResult Div3Update(int userID, int studentID, string dateString, int paymentNameID, int bankNameID, int studentInstallmentID, bool isNewNumber)
        {
            DateTime transactiondate = DateTime.Parse(dateString);
            var user = _usersRepository.GetUser(userID);
            //Subtraction process record
            var inst = _studentInstallmentRepository.GetStudentInstallment2(user.SchoolID, studentID, user.UserPeriod, studentInstallmentID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            var param = parameters.FirstOrDefault(p => p.CategoryID == paymentNameID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = param.Language1;
            else categoryName = param.CategoryName;

            var installmentNo = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (categoryName == "Banka" || categoryName == "BANKA") inst.InstallmentNo = installmentNo.BondNo += 1;
            if (categoryName == "Çek" || categoryName == "ÇEK") inst.InstallmentNo = installmentNo.CheckNo += 1;
            if (categoryName == "Elden" || categoryName == "ELDEN") inst.InstallmentNo = installmentNo.BondNo += 1;
            if (categoryName == "Kmh" || categoryName == "KMH") inst.InstallmentNo = installmentNo.KmhNo += 1;
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") inst.InstallmentNo = installmentNo.CreditCardNo += 1;
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") inst.InstallmentNo = installmentNo.MailOrderNo += 1;
            if (categoryName == "Ots_1" || categoryName == "OTS_1") inst.InstallmentNo = installmentNo.OtsNo1 += 1;
            if (categoryName == "Ots_2" || categoryName == "OTS_2") inst.InstallmentNo = installmentNo.OtsNo2 += 1;
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") inst.InstallmentNo = installmentNo.GovernmentPromotionNo += 1;

            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") inst.InstallmentNo = installmentNo.BondNo += 1;
                if (categoryName == "Check" || categoryName == "CHECK") inst.InstallmentNo = installmentNo.CheckNo += 1;
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") inst.InstallmentNo = installmentNo.BondNo += 1;
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") inst.InstallmentNo = installmentNo.KmhNo += 1;
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") inst.InstallmentNo = installmentNo.CreditCardNo += 1;
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") inst.InstallmentNo = installmentNo.MailOrderNo += 1;
                if (categoryName == "Ots_1" || categoryName == "OTS_1") inst.InstallmentNo = installmentNo.OtsNo1 += 1;
                if (categoryName == "Ots_2" || categoryName == "OTS_2") inst.InstallmentNo = installmentNo.OtsNo2 += 1;
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") inst.InstallmentNo = installmentNo.GovernmentPromotionNo += 1;
            }
            _pSerialNumberRepository.UpdatePSerialNumber(installmentNo);

            //121 ACOOUNT RECORDS 
            var lastNumber = 0;
            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();
            lastNumber = pserialNumbers.AccountSerialNo += 1;
            pserialNumbers.AccountSerialNo = lastNumber;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            decimal amount = inst.InstallmentAmount;

            //Old
            var loop = 1;
            var sortOrder = 1;
            string dateStringOld = inst.InstallmentDate.ToString();
            Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateStringOld, inst.CategoryID, (int)inst.BankID, lastNumber, sortOrder, loop);

            //New
            inst.InstallmentDate = transactiondate;
            inst.CategoryID = paymentNameID;
            inst.BankID = bankNameID;

            loop = 2;
            sortOrder += 1;
            Div2AccountingCreate(user.UserID, user.SchoolID, studentID, user.UserPeriod, amount, dateString, paymentNameID, bankNameID, lastNumber, sortOrder, loop);

            _studentInstallmentRepository.UpdateStudentInstallment(inst);

            //////Users Log//////////////////
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(studentID);
            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = user.UserID;
            log.UserLogDate = DateTime.Now;
            decimal unitPricex = Math.Round(amount, school.CurrencyDecimalPlaces);
            if (user.SelectedCulture.Trim() == "en-US")
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Installment Change").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Installment Details Changed, " + dateString + " Dated, " + "Amount:" + unitPricex;
            }
            else
            {
                log.TransactionID = _parameterRepository.GetParameterCategoryName("Taksit Değişiklik").CategoryID;
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Taksit Detayları Değiştirildi, " + dateString + " Tarihli, " + "Tutar:" + unitPricex;
            }

            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return Json(true);
        }
        public void Div1AccountingCreate(int userID, int schoolID, int studentID, string period, decimal amount, string dateString, int paymentNameID, int bankNameID, int lastNumber)
        {
            //121 ACOOUNT RECORDS 
            int sortOrder = 1;
            DateTime transactiondate = DateTime.Parse(dateString);
            var user = _usersRepository.GetUser(userID);

            var accountingCode = new AccountCodes();
            var accounting = new Accounting();
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var student = _studentRepository.GetStudent(studentID);

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;

            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            if (schoolInfo.NewPeriod == user.UserPeriod)
                classroomID = student.ClassroomID;
            else
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(user.SchoolID, studentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(user.SchoolID, studentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                        classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            string bankName = "";
            if (bankNameID > 0)
            {
                bankName = _bankRepository.GetBank(bankNameID).BankName;
            }
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string code = null;

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            //accounting.AccountCodeID = _accountCodesRepository.GetAccountCode("121").AccountCodeID;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;

            var paymentTypetxt = "";
            var param = parameters.FirstOrDefault(p => p.CategoryID == paymentNameID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = param.Language1;
            else categoryName = param.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") { paymentTypetxt += "Banka Senet"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
            if (categoryName == "Çek" || categoryName == "ÇEK") { paymentTypetxt += "Çek"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
            if (categoryName == "Elden" || categoryName == "ELDEN") { paymentTypetxt += "Elden Senet"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
            if (categoryName == "Kmh" || categoryName == "KMH") { paymentTypetxt += "KMH"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") { paymentTypetxt += "Kredi kartı"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { paymentTypetxt += "Mail order"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
            if (categoryName == "Ots_1" || categoryName == "OTS_1") { paymentTypetxt += "OTS1"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
            if (categoryName == "Ots_2" || categoryName == "OTS_2") { paymentTypetxt += "OTS2"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") { paymentTypetxt += "Teşvik"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") { paymentTypetxt += "Bank Bond"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Check" || categoryName == "CHECK") { paymentTypetxt += "Check"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") { paymentTypetxt += "Bond by Hand"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") { paymentTypetxt += "Overdraft Account"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") { paymentTypetxt += "Credit Card"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { paymentTypetxt += "Mail order"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
                if (categoryName == "Ots_1" || categoryName == "OTS_1") { paymentTypetxt += "OTS1"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Ots_2" || categoryName == "OTS_2") { paymentTypetxt += "OTS2"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") { paymentTypetxt += "Gov.Support"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
            }
            var exp3 = " Nolu Makbuz, ";
            var exp2 = " Tarihli öğrenciden alınan ";
            var exp4 = " Tarihli Yeni " + paymentTypetxt + " Tahsili ";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                exp2 = " Taken from a student ";
                exp3 = " Receipt No, ";
                exp4 = " Dated " + paymentTypetxt + " Collection ";
            }

            string controlCode = code;
            if (controlCode == null) controlCode = "121";
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;

            DateTime dt = Convert.ToDateTime(transactiondate);
            int DD = dt.Day;
            int MM = dt.Month;
            int YY = dt.Year;

            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

            accounting.Debt = amount;
            accounting.Credit = 0;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

            //340 ACOOUNT RECORDS 
            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;
            var exp1 = " ";

            controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;

            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
            accounting.Debt = 0;
            accounting.Credit = amount;
            accounting.SortOrder = sortOrder + 1;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

        }
        public void Div2AccountingCreate(int userID, int schoolID, int studentID, string period, decimal amount, string dateString, int paymentNameID, int bankNameID, int lastNumber, int sortOrder, int loop)
        {
            //121 ACOOUNT RECORDS 
            var user = _usersRepository.GetUser(userID);

            DateTime transactiondate = DateTime.Parse(dateString);

            var accountingCode = new AccountCodes();
            var accounting = new Accounting();
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(schoolID);
            var student = _studentRepository.GetStudent(studentID);

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;

            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            if (schoolInfo.NewPeriod == user.UserPeriod)
                classroomID = student.ClassroomID;
            else
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(user.SchoolID, studentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(user.SchoolID, studentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                        classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            string bankName = "";
            if (bankNameID > 0)
            {
                bankName = _bankRepository.GetBank(bankNameID).BankName;
            }
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string code = null;

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            //accounting.AccountCodeID = _accountCodesRepository.GetAccountCode("121").AccountCodeID;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            var exp3 = "";
            var exp33 = "";
            var paymentTypetxt = "";

            var param = parameters.FirstOrDefault(p => p.CategoryID == paymentNameID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = param.Language1;
            else categoryName = param.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") { paymentTypetxt += "Banka Senet"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
            if (categoryName == "Çek" || categoryName == "ÇEK") { paymentTypetxt += "Çek"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
            if (categoryName == "Elden" || categoryName == "ELDEN") { paymentTypetxt += "Elden Senet"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
            if (categoryName == "Kmh" || categoryName == "KMH") { paymentTypetxt += "KMH"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") { paymentTypetxt += "Kredi kartı"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { paymentTypetxt += "Mail order"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
            if (categoryName == "Ots_1" || categoryName == "OTS_1") { paymentTypetxt += "OTS1"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
            if (categoryName == "Ots_2" || categoryName == "OTS_2") { paymentTypetxt += "OTS2"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") { paymentTypetxt += "Teşvik"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") { paymentTypetxt += "Bank Bond"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Check" || categoryName == "CHECK") { paymentTypetxt += "Check"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID04, period).AccountCode; };
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") { paymentTypetxt += "Bond by Hand"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID05, period).AccountCode; };
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") { paymentTypetxt += "Overdraft Account"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID11, period).AccountCode; };
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") { paymentTypetxt += "Credit Card"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID09, period).AccountCode; };
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") { paymentTypetxt += "Mail order"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID08, period).AccountCode; };
                if (categoryName == "Ots_1" || categoryName == "OTS_1") { paymentTypetxt += "OTS1"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Ots_2" || categoryName == "OTS_2") { paymentTypetxt += "OTS2"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID06, period).AccountCode; };
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") { paymentTypetxt += "Gov.Support"; code = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID10, period).AccountCode; };
            }
            exp3 = " Nolu Dekontu, ";
            exp33 = " Dekontu ";
            var exp4 = " Tarihli " + paymentTypetxt + exp33;
            if (user.SelectedCulture.Trim() == "en-US")
            {
                exp3 = " Receipt No., ";
                exp33 = " Receipt ";
                exp4 = " Dated " + paymentTypetxt + exp33;
            }

            string controlCode = code;
            if (controlCode == null) controlCode = "121";
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;

            DateTime dt = Convert.ToDateTime(transactiondate);
            int DD = dt.Day;
            int MM = dt.Month;
            int YY = dt.Year;
            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;
            if (loop == 1)
            {
                accounting.Debt = 0;
                accounting.Credit = amount;
            }
            else
            {
                accounting.Debt = amount;
                accounting.Credit = 0;
            }

            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

            //340 ACOOUNT RECORDS 
            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;
            var exp1 = " ";
            var exp2 = " ";

            if (loop == 1)
                exp2 = " Tarihli öğrenciden alınanların İptali ";
            else exp2 = " Tarihli öğrenciden alınan";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (loop == 1)
                    exp2 = " Cancellation of those taken from the student with the date ";
                else exp2 = " Taken from a student";
            }

            controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;

            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            accounting.Explanation = classroomName + " " + student.FirstName + " " + student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
            accounting.Debt = amount;
            accounting.Credit = 0;

            if (loop == 1)
            {
                accounting.Debt = amount;
                accounting.Credit = 0;
            }
            else
            {
                accounting.Debt = 0;
                accounting.Credit = amount;
            }

            accounting.SortOrder = sortOrder + 1;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

        }
        public string UserAccountWrite(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Tahsilat Yapan Kul. Muhasebeye Yaz").CategoryID;

            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;
            var username = "";
            if (isPermission)
            {
                username = user.UserName;
            }
            return (username);
        }
        #endregion
    }
}
