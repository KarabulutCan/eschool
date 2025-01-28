using DocumentFormat.OpenXml.Drawing.Charts;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using Unity;
using Parameter = ESCHOOL.Models.Parameter;

namespace ESCHOOL.Controllers
{
    public class M400Invoice : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentAddressRepository _studentAddressRepository;
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
        IAccountCodesDetailRepository _accountCodesDetailRepository;
        IAccountCodesRepository _accountCodesRepository;

        ITempM101Repository _tempM101Repository;

        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;

        IWebHostEnvironment _hostEnvironment;
        public M400Invoice(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentAddressRepository studentAddressRepository,
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
            IAccountCodesDetailRepository accountCodesDetailRepository,
            ITempM101Repository tempM101Repository,

            IUsersRepository usersRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,

        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentAddressRepository = studentAddressRepository;
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
            _accountCodesDetailRepository = accountCodesDetailRepository;
            _tempM101Repository = tempM101Repository;

            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;

            _hostEnvironment = hostEnvironment;
        }

        #region Students 
        [Route("M400Invoice/GridStudentDataReadPlan/{userID}")]
        public IActionResult GridStudentDataReadPlan(int userID)
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
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

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

                    var installments = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);

                    if (installments.Count() > 0)
                    {
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

                            var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceTrue(user.UserPeriod, user.SchoolID, item.StudentID).ToList();
                            studentViewModel.CutInvoice = Convert.ToDecimal(cutinvoice.Sum(p => p.DAmount));

                            var uncutinvoice = _studentInvoiceRepository.GetStudentInvoiceFalse(user.UserPeriod, user.SchoolID, item.StudentID).ToList();
                            studentViewModel.UncutInvoice = Convert.ToDecimal(uncutinvoice.Sum(p => p.DAmount));

                            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID);
                            if (studentViewModel.InvoiceTaxNumber == null)
                            {
                                if (studentInvoiceAddress != null)
                                {
                                    studentViewModel.InvoiceTaxNumber = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID).InvoiceTaxNumber;
                                }
                                if (studentViewModel.InvoiceTaxNumber == null) studentViewModel.InvoiceTaxNumber = item.IdNumber;
                            }
                            var studentAdress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID);
                            if (studentAdress != null)
                            {
                                if (studentViewModel.InvoiceTaxNumber == null) studentViewModel.StudentClassroom = Resources.Resource.NoTaxId;
                                if (studentAdress.EMail == null) studentViewModel.StudentClassroom = Resources.Resource.NoMail;
                                if (studentAdress.InvoiceTitle == null) studentViewModel.StudentClassroom = Resources.Resource.NoTitle;
                                if (studentAdress.InvoiceAddress == null) studentViewModel.StudentClassroom = Resources.Resource.NoAddress;
                                if (studentAdress.InvoiceCityParameterID == 0) studentViewModel.StudentClassroom = Resources.Resource.NoCity;
                                if (studentAdress.InvoiceTownParameterID == 0) studentViewModel.StudentClassroom = Resources.Resource.NoTown;
                            }
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
            }
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        [Route("M400Invoice/GridStudentDataReadBatch/{userID}/{classroomID}")]
        public IActionResult GridStudentDataReadBatch(int userID, int classroomID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());

            IEnumerable<Student> allStudents = null;
            List<Student> students = new List<Student>();
            List<StudentViewModel> list = new List<StudentViewModel>();
            List<StudentPeriods> studentPeriod = new List<StudentPeriods>();
            string classroomName = "";

            if (classroomID != 0)
                classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;

            if (classroomID == 0)
                allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
            else
            {
                allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID).Where(b => b.ClassroomID == classroomID);
                if (allStudents.Count() == 0)
                {
                    allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                    studentPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod).Where(s => s.ClassroomName == classroomName).ToList();
                    students = allStudents.Where(s => studentPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                    goto CONTINUE;
                }
            }
            studentPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod).ToList();
            students = allStudents.Where(s => studentPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
        CONTINUE:;

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;
            var statuCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationTypeCategories = _parameterRepository.GetParameterSubID(categoryID);

            categoryID = _parameterRepository.GetParameterCategoryName("Cinsiyeti").CategoryID;
            var gender = _parameterRepository.GetParameterSubID(categoryID);

            bool isExist = false;
            var studentViewModel = new StudentViewModel();
            bool isSuccess2 = true;
            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

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

                    var installments = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);
                    var uncutinvoice = _studentInvoiceRepository.GetStudentInvoiceFalse(user.UserPeriod, user.SchoolID, item.StudentID).ToList();
                    if (installments.Count() > 0 && uncutinvoice.Count() > 0)
                    {
                        studentViewModel = new StudentViewModel();
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

                            var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceTrue(user.UserPeriod, user.SchoolID, item.StudentID).ToList();
                            studentViewModel.CutInvoice = Convert.ToDecimal(cutinvoice.Sum(p => p.DAmount));

                            studentViewModel.UncutInvoice = Convert.ToDecimal(uncutinvoice.Sum(p => p.DAmount));

                            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID);
                            if (studentViewModel.InvoiceTaxNumber == null)
                            {
                                if (studentInvoiceAddress != null)
                                {
                                    studentViewModel.InvoiceTaxNumber = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID).InvoiceTaxNumber;
                                }
                                if (studentViewModel.InvoiceTaxNumber == null) studentViewModel.InvoiceTaxNumber = item.IdNumber;
                            }

                            var studentAdress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID);
                            if (studentAdress != null)
                            {
                                if (studentAdress.InvoiceTaxNumber == null) { studentViewModel.StudentClassroom = Resources.Resource.NoTaxId; isSuccess2 = false; }
                                if (studentAdress.EMail == null) { studentViewModel.StudentClassroom = Resources.Resource.NoMail; isSuccess2 = false; }
                                if (studentAdress.InvoiceTitle == null) { studentViewModel.StudentClassroom = Resources.Resource.NoTitle; isSuccess2 = false; }
                                if (studentAdress.InvoiceAddress == null) { studentViewModel.StudentClassroom = Resources.Resource.NoAddress; isSuccess2 = false; }
                                if (studentAdress.InvoiceCityParameterID == 0) { studentViewModel.StudentClassroom = Resources.Resource.NoCity; isSuccess2 = false; }
                                if (studentAdress.InvoiceTownParameterID == 0) { studentViewModel.StudentClassroom = Resources.Resource.NoTown; }
                            }
                            else isSuccess2 = false;
                        };
                        decimal total = 0;
                        foreach (var inst in installments)
                        {
                            total += inst.InstallmentAmount;
                        }

                        studentViewModel.Total = total;
                        studentViewModel.IsSuccess2 = isSuccess2;
                        list.Add(studentViewModel);
                    }
                }
            }
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }


        [Route("M400Invoice/GridStudentDataReadFilter/{userID}/{paymentTypeID}")]
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
            string classroomName = "";
            bool isExist = false;
            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in student)
            {
                var classroom = _classroomRepository.GetClassroomID(item.ClassroomID);

                var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);
                if (paymentTypeID != 0)
                {
                    isExist = _studentInstallmentRepository.ExistStudentInstallment2(item.SchoolID, user.UserPeriod, item.StudentID, paymentTypeID);
                }
                if (paymentTypeID == 0) isExist = true;

                if (isExist)
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

                    var uncutinvoice = _studentInvoiceRepository.GetStudentInvoiceFalse(user.UserPeriod, user.SchoolID, item.StudentID).FirstOrDefault();
                    if (uncutinvoice != null)
                    {
                        var studentViewModel = new StudentViewModel();
                        {
                            studentViewModel.ViewModelID = item.StudentID;
                            studentViewModel.StudentID = item.StudentID;
                            studentViewModel.StudentPicture = item.StudentPicture;
                            studentViewModel.Name = item.FirstName + " " + item.LastName;
                            studentViewModel.StudentClassroom = classroomName;

                            if (statuCategory != null)
                                studentViewModel.StatuCategory = statuCategory.CategoryName;
                            if (registrationTypeCategory != null)
                                studentViewModel.RegistrationTypeCategory = registrationTypeCategory.CategoryName;

                            var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceTrue(user.UserPeriod, user.SchoolID, item.StudentID);
                            studentViewModel.CutInvoice = Convert.ToDecimal(cutinvoice.Sum(p => p.DAmount));


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
            }
            return Json(list);
        }

        #endregion

        #region Invoice_Plan

        [HttpGet]
        public IActionResult InvoicePlan(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Fatura İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var isSuccess = false;
            if (school.EIIsActive == true)
                if (school.EIUserPassword == null || school.EIUserName == null || school.EIInvoiceSerialCode1 == null || school.EIInvoiceSerialCode2 == null || school.EIIntegratorNameID == 0)
                    isSuccess = true;

            var student = _studentRepository.GetStudent(user.SchoolID);

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
                gendertxt = gender.FirstOrDefault(p => p.CategoryID == student.GenderTypeCategoryID).CategoryName;

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            string statutxt = "";
            if (student != null)
                statutxt = statu.FirstOrDefault(p => p.CategoryID == student.RegistrationTypeCategoryID).CategoryName;

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
                CategoryName2 = categoryName2,
            };
            return View(studentViewModel);
        }

        [Route("M400Invoice/InvoiceDataRead/{period}/{userID}/{studentID}")]
        public IActionResult InvoiceDataRead(string period, int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);

            var studentinvoice = _studentInvoiceRepository.GetStudentInvoice(period, user.SchoolID, studentID);
            return Json(studentinvoice);
        }

        [Route("M400Invoice/PlanDataRead/{count}/{dateString}/{percent}/{isPercent}/{isRefresh}")]
        public IActionResult PlanDataRead(int count, string dateString, int percent, bool isPercent, bool isRefresh)
        {
            DateTime transactiondate = DateTime.Parse(dateString);

            var plan = _tempPlanRepository.GetTempPlan();
            if (isRefresh == true || plan.Count() == 0)
            {
                foreach (var item in plan)
                {
                    _tempPlanRepository.DeleteTempPlan(item);
                    _tempPlanRepository.Save();
                }

                List<TempPlan> list = new List<TempPlan>();
                var maxPercent = 100;
                if (!isPercent)
                {
                    percent = 0;
                    maxPercent = 0;
                };
                var max = count * percent;
                if (max > 100) percent = 0;

                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                    {
                        percent = maxPercent;
                    }

                    var tempPlan = new TempPlan
                    {
                        PlanID = 0,
                        PlanPercent = percent,
                        PlanDate = transactiondate.AddMonths(i),
                        IsActive = true,
                    };
                    _tempPlanRepository.CreateTempPlan(tempPlan);

                    maxPercent -= percent;
                    list.Add(tempPlan);
                }
                return Json(list);
            }
            else
                return Json(plan);
        }

        [HttpPost]
        public IActionResult PlanDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<TempPlan>>(strResult);
            List<TempPlan> list = new List<TempPlan>();

            var i = 0;
            foreach (var item in json)
            {
                var getCode = _tempPlanRepository.GetTempPlanID(item.PlanID);
                getCode.PlanID = json[i].PlanID;
                getCode.PlanPercent = json[i].PlanPercent;
                getCode.PlanDate = json[i].PlanDate;
                getCode.IsActive = json[i].IsActive;
                if (ModelState.IsValid)
                {
                    _tempPlanRepository.UpdateTempPlan(getCode);
                }
                i += 1;
                list.Add(getCode);
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M400Invoice/PlanDataStart/{userID}/{isDetailed}/{isDiscount}/{isSelectStudent}/{studentID}")]
        public IActionResult PlanDataStart(int userID, bool isDetailed, bool isDiscount, bool isSelectStudent, int studentID)
        {
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);


            if (schoolInfo.NewPeriod != user.UserPeriod)
            {
                students = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            var selectFee = _schoolFeeRepository.GetSchoolFeeSelect(user.SchoolID, "L1");
            bool isSingle = false;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());

            foreach (var studentitem in students)
            {
                var statuName = _parameterRepository.GetParameter(studentitem.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                if (isSelectStudent == true)
                {
                    if (isSingle) break;
                    var student = _studentRepository.GetStudent(studentID);
                    studentID = student.StudentID;
                    isSingle = true;
                }
                else studentID = studentitem.StudentID;

                decimal cutUnitPrice = 0;
                decimal cutDiscount = 0;
                decimal cutAmount = 0;
                int cutCount = 0;

                decimal studentDebt = 0;
                decimal unitPrice = 0;
                decimal discount = 0;
                decimal amount = 0;

                decimal unitPriceT = 0;
                decimal discountT = 0;
                decimal amountT = 0;
                decimal taxT = 0;

                var selectPlan = _tempPlanRepository.GetTempPlanSelect();
                int count = selectPlan.Count();

                var invoiceTrue = _studentInvoiceRepository.GetStudentInvoiceTrue(user.UserPeriod, user.SchoolID, studentID).ToList();
                foreach (var itemTrue in invoiceTrue)
                {
                    cutCount += 1;
                    cutUnitPrice += Convert.ToDecimal(itemTrue.DUnitPrice);
                    cutDiscount += Convert.ToDecimal(itemTrue.DDiscount);
                    cutAmount += Convert.ToDecimal(itemTrue.DAmount);
                }

                //Deleted Amounts
                var invoiceDelete = _studentInvoiceRepository.GetStudentInvoiceFalse(user.UserPeriod, user.SchoolID, studentID).ToList();

                foreach (var item in invoiceDelete)
                {
                    _studentInvoiceRepository.DeleteStudentInvoice(item);

                    var invoiceDetailDelete = _studentInvoiceDetailRepository.GetStudentInvoiceDetail(studentID, item.StudentInvoiceID).ToList();
                    foreach (var itemDetail in invoiceDetailDelete)
                    {
                        if (!itemDetail.InvoiceStatus)
                        {
                            _studentInvoiceDetailRepository.DeleteStudentInvoiceDetail(itemDetail);
                        }
                    }
                }

                //Total Amounts
                foreach (var feeitem in selectFee)
                {
                    if (feeitem.IsActive == true)
                    {
                        var unitPriceControl = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeitem.SchoolFeeID);
                        if (unitPriceControl != null && unitPriceControl.UnitPrice > 0)
                        {
                            unitPrice = unitPriceControl.UnitPrice;
                            discount = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeitem.SchoolFeeID).Discount;
                            amount = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeitem.SchoolFeeID).Amount;
                            studentDebt += amount;
                            unitPriceT += unitPrice;
                            discountT += discount;
                            amountT += amount;
                        }
                    }
                }

                unitPriceT -= cutUnitPrice;
                discountT -= cutDiscount;
                amountT -= cutAmount;
                count -= cutCount;

                foreach (var planitem in selectPlan)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
                    DateTime dateOnly = planitem.PlanDate.Date;
                    var isExist = _studentInvoiceRepository.GetStudentInvoiceControl(user.UserPeriod, user.SchoolID, studentID, dateOnly);
                    if (isExist == null)
                    {
                        if (planitem.IsActive == true)
                        {
                            var invoice = new StudentInvoice();
                            taxT = 0;
                            InvoiceInitialize(invoice, user.UserPeriod, user.UserID);
                            _studentInvoiceRepository.CreateStudentInvoice(invoice);

                            var studentInvoiceID = invoice.StudentInvoiceID;
                            double taxPercent = 0;
                            byte taxPercent2 = 0;
                            //Detail Invoice
                            foreach (var feeitem in selectFee)
                            {
                                if (feeitem.IsActive == true)
                                {
                                    var detail = new StudentInvoiceDetail();

                                    var unitPriceControl = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeitem.SchoolFeeID);
                                    if (unitPriceControl != null && unitPriceControl.UnitPrice > 0)
                                    {
                                        unitPrice = unitPriceControl.UnitPrice;
                                        discount = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeitem.SchoolFeeID).Discount;
                                        amount = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, feeitem.SchoolFeeID).Amount;

                                        detail.StudentInvoiceDetailID = 0;
                                        detail.StudentInvoiceID = studentInvoiceID;
                                        detail.SchoolID = user.SchoolID;
                                        detail.StudentID = studentID;
                                        detail.SchoolFeeID = feeitem.SchoolFeeID;
                                        detail.Period = user.UserPeriod;

                                        string strDate = planitem.PlanDate.ToString("dd-MM-yyyy", new CultureInfo(user.SelectedCulture.Trim()));
                                        string txtDate = Resources.Resource.Dated;
                                        if (schoolInfo.InvoiceOnDate) detail.Explanation = strDate + " " + txtDate + " " + feeitem.Name;
                                        else detail.Explanation = feeitem.Name;
                                        if (user.SelectedCulture.Trim() == "en-US")
                                        {
                                            if (schoolInfo.InvoiceOnDate) detail.Explanation = strDate + " " + txtDate + " " + feeitem.Language1;
                                            else detail.Explanation = feeitem.Language1;
                                        }

                                        detail.UnitPrice = unitPrice / (count + cutCount);
                                        detail.Quantity = 1;
                                        detail.Discount = discount / (count + cutCount);
                                        detail.TaxPercent = feeitem.Tax;
                                        detail.Amount = detail.UnitPrice - detail.Discount;

                                        var total = detail.UnitPrice - detail.Discount;
                                        taxPercent = (1 + (Convert.ToDouble(feeitem.Tax) / 100));
                                        taxPercent2 = (byte)feeitem.Tax;

                                        decimal tax1 = Math.Round(total - (total / Convert.ToDecimal(taxPercent)), schoolInfo.CurrencyDecimalPlaces);
                                        detail.Tax = tax1;
                                        taxT += tax1;

                                        if (isDiscount == false)
                                        {
                                            detail.UnitPrice = total - tax1;
                                            detail.Discount = 0;
                                        }

                                        detail.InvoiceStatus = false;
                                        if (ModelState.IsValid)
                                        {
                                            _studentInvoiceDetailRepository.CreateStudentInvoiceDetail(detail);
                                        }
                                    }
                                }
                            }

                            //Invoice
                            invoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);

                            invoice.SchoolID = user.SchoolID;
                            invoice.StudentID = studentID;
                            invoice.Period = user.UserPeriod;
                            invoice.InvoiceSerialNo = 0;
                            invoice.InvoiceDate = planitem.PlanDate;
                            invoice.InvoiceCutDate = null;
                            invoice.DUnitPrice = unitPriceT / count;
                            invoice.DQuantity = 1;
                            invoice.DDiscount = discountT / count;
                            invoice.DTaxPercent = taxPercent2;
                            invoice.DTax = taxT;
                            invoice.DAmount = invoice.DUnitPrice - invoice.DDiscount;

                            if (isDiscount == false)
                            {
                                invoice.DUnitPrice = invoice.DAmount - invoice.DTax;
                                invoice.DDiscount = 0;
                            }

                            invoice.WithholdingPercent1 = 0;
                            invoice.WithholdingPercent2 = 0;
                            invoice.WithholdingCode = "";
                            invoice.WithholdingExplanation = "";
                            invoice.WithholdingTax = 0;
                            invoice.WithholdingTotal = 0;

                            invoice.IsPlanned = true;
                            invoice.InvoiceType = true;
                            invoice.InvoiceStatus = false;
                            invoice.IsActive = true;

                            // Invoice Detail Option Update
                            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);
                            if (invoiceAddress == null)
                            {
                                invoiceAddress = new StudentInvoiceAddress();
                                invoiceAddress.StudentID = studentitem.StudentID;
                                if (studentitem.ParentName != null)
                                    invoiceAddress.InvoiceTitle = studentitem.ParentName;
                                else invoiceAddress.InvoiceTitle = studentitem.FirstName + " " + studentitem.LastName;
                                invoiceAddress.IsInvoiceDetailed = isDetailed;
                                invoiceAddress.IsInvoiceDiscount = isDiscount;
                                _studentInvoiceAddressRepository.CreateStudentInvoiceAddress(invoiceAddress);
                            }
                            else
                            {
                                invoiceAddress.IsInvoiceDetailed = isDetailed;
                                invoiceAddress.IsInvoiceDiscount = isDiscount;
                                _studentInvoiceAddressRepository.UpdateStudentInvoiceAddress(invoiceAddress);
                            }

                            invoice.StudentInvoiceAddressID = invoiceAddress.StudentInvoiceAddressID;

                            if (ModelState.IsValid)
                            {
                                _studentInvoiceRepository.UpdateStudentInvoice(invoice);
                            }
                        }
                    }
                }
            }
            return Json(true);
        }
        public void InvoiceInitialize(StudentInvoice invoice, string period, int userID)
        {
            var user = _usersRepository.GetUser(userID);

            invoice.StudentInvoiceID = 0;

            invoice.SchoolID = user.SchoolID;
            invoice.StudentID = 0;
            invoice.Period = period;
            invoice.InvoiceSerialNo = 0;
            invoice.InvoiceDate = null;
            invoice.InvoiceCutDate = null;
            invoice.DUnitPrice = 0;
            invoice.DQuantity = 0;
            invoice.DAmount = 0;
            invoice.DDiscount = 0;
            invoice.DAmount = 0;
            invoice.DTaxPercent = 0;
            invoice.DTax = 0;

            invoice.WithholdingPercent1 = 0;
            invoice.WithholdingPercent2 = 0;
            invoice.WithholdingCode = "";
            invoice.WithholdingExplanation = "";
            invoice.WithholdingTax = 0;
            invoice.WithholdingTotal = 0;

            invoice.IsPlanned = false;
            invoice.InvoiceType = false;
            invoice.InvoiceStatus = false;
            invoice.IsActive = false;
        }

        [Route("M400Collections/SchoolDebtDataRead2/{userID}/{period}/{studentid}")]
        public IActionResult SchoolDebtDataRead2(int userID, string period, int studentid)
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

        [Route("M400Invoice/SchoolDebtDataRead/{userID}")]
        public IActionResult SchoolDebtDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            List<SchoolFee> list = new List<SchoolFee>();
            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            foreach (var item in fee)
            {
                var getCode = new SchoolFee();
                getCode.SchoolFeeID = item.SchoolFeeID;
                getCode.SchoolID = item.SchoolID;
                if (user.SelectedCulture.Trim() == "en-US")
                    getCode.Name = item.Language1;
                else getCode.Name = item.Name;
                getCode.Tax = item.Tax;
                getCode.IsSelect = true;

                list.Add(getCode);
            }
            return Json(list);
        }
        [HttpPost]
        public IActionResult SchoolDebtDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);
            List<SchoolFee> list = new List<SchoolFee>();

            var i = 0;
            foreach (var item in json)
            {
                var getCode = _schoolFeeRepository.GetSchoolFee((int)item.SchoolFeeID);

                getCode.SchoolFeeID = json[i].SchoolFeeID;
                getCode.IsSelect = json[i].IsSelect;
                getCode.Name = json[i].Name;
                if (ModelState.IsValid)
                {
                    _schoolFeeRepository.UpdateSchoolFee(getCode);
                }
                i += 1;
                list.Add(getCode);
            }
            return Json(list);
        }

        [Route("M400Invoice/InvoiceDetailDataRead/{userID}/{studentID}")]
        public IActionResult InvoiceDetailDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var invoice = _studentInvoiceRepository.GetStudentInvoice(user.UserPeriod, user.SchoolID, studentID);
            return Json(invoice);
        }

        [Route("M400Invoice/InvoiceDetailDataReadSub/{studentID}/{studentInvoiceID}")]
        public IActionResult InvoiceDetailDataReadSub(int studentID, int studentInvoiceID)
        {
            var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetail(studentID, studentInvoiceID);

            return Json(invoiceDetail);
        }

        [Route("M400Invoice/StudentUnplannedDataRead/{userID}/{studentid}")]
        public IActionResult StudentUnplannedDataRead(int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;
            List<StudentUnplanViewModel> list = new List<StudentUnplanViewModel>();

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");
            foreach (var item in fee)
            {
                var studentUnplanViewModel = new StudentUnplanViewModel();
                var debt = _studentDebtRepository.GetStudentDebt22(period, user.SchoolID, studentid, item.SchoolFeeID);
                studentUnplanViewModel.UserID = userID;
                studentUnplanViewModel.ViewModelId = studentid;
                studentUnplanViewModel.StudentID = studentid;
                if (user.SelectedCulture.Trim() == "en-US")
                    studentUnplanViewModel.FeeName = item.Language1;
                else studentUnplanViewModel.FeeName = item.Name;

                studentUnplanViewModel.SchoolFeeID = item.SchoolFeeID;
                if (debt != null && debt.Amount > 0)
                {
                    studentUnplanViewModel.SchoolID = (int)debt.SchoolID;
                    studentUnplanViewModel.Period = debt.Period;

                    //studentUnplanViewModel.Amount = debt.Amount - debt.Discount;
                    studentUnplanViewModel.Amount = debt.Amount;
                    //if (debt.Amount == 0) break;

                    studentUnplanViewModel.Tax = Convert.ToByte(item.Tax);

                    var cutinvoice = _studentInvoiceDetailRepository.GetStudentInvoiceDetail2(period, user.SchoolID, studentid, item.SchoolFeeID);

                    //studentUnplanViewModel.CutInvoiceAmount = cutinvoice.Sum(p => p.Amount) - cutinvoice.Sum(p => p.Discount);
                    studentUnplanViewModel.CutInvoiceAmount = cutinvoice.Sum(p => p.Amount);

                    var uncutinvoice = studentUnplanViewModel.Amount - studentUnplanViewModel.CutInvoiceAmount;
                    studentUnplanViewModel.UncutInvoiceAmount = uncutinvoice;

                    studentUnplanViewModel.NewInvoiceAmount = 0;

                    list.Add(studentUnplanViewModel);
                }
            }

            return Json(list);
        }

        [HttpPost]
        [Route("M400Invoice/StudentUnplannedDataCreate/{strResult}/{dateString}")]
        public IActionResult StudentUnplannedDataCreate([Bind(Prefix = "models")] string strResult, string dateString)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentUnplanViewModel>>(strResult);

            DateTime transactiondate = DateTime.Parse(dateString);

            decimal unitPriceT = 0;
            decimal discountT = 0;
            decimal taxT = 0;

            var invoice = new StudentInvoice();
            taxT = 0;
            int count = 0;

            InvoiceInitialize(invoice, json[0].Period, (int)json[0].UserID);
            _studentInvoiceRepository.CreateStudentInvoice(invoice);

            var studentInvoiceID = invoice.StudentInvoiceID;
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(invoice.SchoolID);
            double taxPercent = 0;
            var i = 0;
            foreach (var item in json)
            {
                if (item.NewInvoiceAmount > 0)
                {
                    var getCode = _studentInvoiceRepository.GetStudentInvoiceID(json[i].Period, json[i].SchoolID, json[i].StudentInvoiceID);

                    var detail = new StudentInvoiceDetail();

                    count += 1;
                    detail.StudentInvoiceDetailID = 0;
                    detail.StudentInvoiceID = studentInvoiceID;
                    detail.SchoolID = json[i].SchoolID;
                    detail.StudentID = json[i].StudentID;
                    detail.Period = json[i].Period;
                    detail.Explanation = json[i].FeeName;
                    detail.SchoolFeeID = json[i].SchoolFeeID;
                    detail.Quantity = 1;
                    detail.InvoiceStatus = false;
                    detail.UnitPrice = json[i].NewInvoiceAmount;
                    detail.Amount = json[i].NewInvoiceAmount;
                    detail.Discount = 0;
                    detail.TaxPercent = json[i].Tax;

                    var total = detail.Amount - detail.Discount;
                    taxPercent = (1 + (Convert.ToDouble(detail.TaxPercent) / 100));
                    decimal tax1 = Math.Round(total - (total / Convert.ToDecimal(taxPercent)), schoolInfo.CurrencyDecimalPlaces);
                    unitPriceT += detail.UnitPrice;
                    detail.Tax = tax1;
                    taxT += tax1;

                    if (ModelState.IsValid)
                    {
                        _studentInvoiceDetailRepository.CreateStudentInvoiceDetail(detail);
                    }
                    i += 1;
                }
            }

            invoice = _studentInvoiceRepository.GetStudentInvoiceID(json[0].Period, json[0].SchoolID, studentInvoiceID);

            invoice.SchoolID = json[0].SchoolID;
            invoice.StudentID = json[0].StudentID;
            invoice.Period = json[0].Period;
            invoice.InvoiceSerialNo = 0;
            invoice.InvoiceDate = transactiondate;
            invoice.InvoiceCutDate = null;
            invoice.DUnitPrice = unitPriceT;
            invoice.DDiscount = discountT;
            invoice.DTaxPercent = Convert.ToByte(taxPercent);
            invoice.DTax = taxT;
            invoice.DAmount = (invoice.DUnitPrice - invoice.DDiscount);

            invoice.WithholdingPercent1 = 0;
            invoice.WithholdingPercent2 = 0;
            invoice.WithholdingCode = "";
            invoice.WithholdingExplanation = "";
            invoice.WithholdingTax = 0;
            invoice.WithholdingTotal = 0;

            invoice.IsPlanned = false;
            invoice.InvoiceType = true;
            invoice.InvoiceStatus = false;
            invoice.IsActive = true;

            if (ModelState.IsValid)
            {
                _studentInvoiceRepository.UpdateStudentInvoice(invoice);
            }
            return Json(true);
        }

        #endregion

        #region Invoice program
        public IActionResult InvoiceManually(int userID, int studentInvoiceID)
        {
            return Invoice(userID, 0, studentInvoiceID);
        }
        public IActionResult Invoice(int userID, int studentID, int studentInvoiceID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Fatura İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var isSuccess = false;
            if (school.EIIsActive == true)
                if (school.EIUserPassword == null || school.EIUserName == null || school.EIInvoiceSerialCode1 == null || school.EIInvoiceSerialCode2 == null || school.EIIntegratorNameID == 0)
                    isSuccess = true;

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            ViewBag.date = user.UserDate;
            TempData["studentID"] = studentID;

            var studentInvoiceAddress = new StudentInvoiceAddress();
            var student = new Student();
            string classroomName = "";
            if (studentID != 0)
            {
                student = _studentRepository.GetStudent(studentID);
                studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);
                studentInvoiceAddress.Notes = "";
                if (student.ClassroomID > 0)
                {
                    classroomName = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomName;
                }
            }

            var serialname = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            var serial = 0;

            if (studentInvoiceAddress == null)
                studentInvoiceAddress = new StudentInvoiceAddress();

            if (studentInvoiceAddress.InvoiceProfile == true)
            {
                serial = serialname.InvoiceSerialNo1 + 1;
            }
            else
            {
                serial = serialname.InvoiceSerialNo11 + 1;
            }

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);


            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;
            string cs = $"Data Source={dataSource};Initial Catalog={user.SelectedSchoolCode};User Id=sa;Password={password};";

            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString(), "earsiv.xslt");
            bool isSuccess2 = false;
            if (System.IO.File.Exists(filePath)) isSuccess2 = true;
            if (!(bool)school.EIIsActive) isSuccess2 = true;

            bool isActive = (bool)school.EIIsActive;
            if (isSuccess2 == false) isActive = false;

            var invoiceViewModel = new InvoiceViewModel
            {
                IsPermission = isPermission,
                UserID = userID,
                SchoolID = user.SchoolID,
                SelectedSchoolCode = user.SelectedSchoolCode,
                StudentID = studentID,
                StudentInvoiceID = studentInvoiceID,
                Period = user.UserPeriod,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ClassroomName = classroomName,
                StudentInvoiceAddress = studentInvoiceAddress,
                InvoiceSerialNo = serial,

                InvoiceName1 = serialname.InvoiceName1,
                InvoiceSerialNo1 = serialname.InvoiceSerialNo1,
                InvoiceSerialNo11 = serialname.InvoiceSerialNo11,
                InvoiceName2 = serialname.InvoiceName2,
                InvoiceSerialNo2 = serialname.InvoiceSerialNo2,
                InvoiceSerialNo22 = serialname.InvoiceSerialNo22,
                InvoiceName3 = serialname.InvoiceName3,
                InvoiceSerialNo3 = serialname.InvoiceSerialNo3,
                InvoiceSerialNo33 = serialname.InvoiceSerialNo33,

                InvoiceProfile = school.InvoiceProfile,
                InvoiceTypeParameter = school.InvoiceTypeParameter,
                ParameterExceptionCode = school.ParameterExceptionCode,

                SelectedCulture = user.SelectedCulture.Trim(),
                IsSuccess = isSuccess,
                IsSuccess2 = isSuccess2,
                SchoolCode = user.SelectedSchoolCode,
                SchoolEInvoiceIsActive = isActive,
                ConnectionString = cs,
            };
            return View(invoiceViewModel);
        }

        public IActionResult InvoiceCurrentCard(int userID, string accountCode, int accountingID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Fatura İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var isSuccess = false;
            if (school.EIIsActive == true)
                if (school.EIUserPassword == null || school.EIUserName == null || school.EIInvoiceSerialCode1 == null || school.EIInvoiceSerialCode2 == null || school.EIIntegratorNameID == 0)
                    isSuccess = true;

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            ViewBag.date = user.UserDate;
            TempData["accountCode"] = accountCode;

            var accountCodesDetail = new AccountCodesDetail();
            string classroomName = "";
            var code = _accountCodesRepository.GetAccountCode2(accountCode, user.UserPeriod);
            if (accountCode != null)
            {
                if (code == null)
                {
                    string period1 = user.UserPeriod.Substring(0, 4);
                    string period2 = user.UserPeriod.Substring(5, 4);
                    int year1 = Convert.ToInt32(period1);
                    int year2 = Convert.ToInt32(period2);
                    year1 -= 1;
                    year2 -= 1;
                    string oldPeriod = year1 + "-" + year2;

                    code = new AccountCodes();
                    var oldCode = _accountCodesRepository.GetAccountCode2(accountCode, oldPeriod);
                    code.AccountCodeID = 0;
                    code.Period = user.UserPeriod;
                    code.AccountCode = oldCode.AccountCode;
                    code.AccountCodeName = oldCode.AccountCodeName;
                    code.Language1 = oldCode.Language1;
                    code.Language2 = oldCode.Language2;
                    code.Language3 = oldCode.Language3;
                    code.Language4 = oldCode.Language4;
                    code.IsCurrentCard = oldCode.IsCurrentCard;
                    code.IsActive = oldCode.IsActive;
                    _accountCodesRepository.CreateAccountCode(code);
                }
            }
            var accountCodeID = _accountCodesRepository.GetAccountCode2(accountCode, user.UserPeriod).AccountCodeID;
            accountCodesDetail = _accountCodesDetailRepository.GetAccountCodesDetailID1(accountCodeID);

            var serialname = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            var serial = 0;

            if (accountCodesDetail == null)
                accountCodesDetail = new AccountCodesDetail();

            if (accountCodesDetail.InvoiceProfile == true)
            {
                serial = serialname.InvoiceSerialNo1 + 1;
            }
            else
            {
                serial = serialname.InvoiceSerialNo11 + 1;
            }

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);


            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;
            string cs = $"Data Source={dataSource};Initial Catalog={user.SelectedSchoolCode};User Id=sa;Password={password};";

            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString(), "earsiv.xslt");
            bool isSuccess2 = false;
            if (System.IO.File.Exists(filePath)) isSuccess2 = true;
            if (!(bool)school.EIIsActive) isSuccess2 = true;

            bool isActive = (bool)school.EIIsActive;
            if (isSuccess2 == false) isActive = false;

            if (accountCodesDetail.InvoiceProfile != true)
                accountCodesDetail.InvoiceProfile = false;
            if (accountCodesDetail.InvoiceTypeParameter != 0)
                accountCodesDetail.InvoiceTypeParameter = 0;

            var invoiceViewModel = new InvoiceViewModel
            {
                IsPermission = isPermission,
                UserID = userID,
                SchoolID = user.SchoolID,
                SelectedSchoolCode = user.SelectedSchoolCode,
                StudentID = 0,
                AccountCode = accountCode,
                AccountingID = accountingID,
                Period = user.UserPeriod,
                //FirstName = student.FirstName,
                //LastName = student.LastName,
                ClassroomName = classroomName,

                StudentInvoiceAddress = null,
                InvoiceSerialNo = serial,

                InvoiceName1 = serialname.InvoiceName1,
                InvoiceSerialNo1 = serialname.InvoiceSerialNo1,
                InvoiceSerialNo11 = serialname.InvoiceSerialNo11,
                InvoiceName2 = serialname.InvoiceName2,
                InvoiceSerialNo2 = serialname.InvoiceSerialNo2,
                InvoiceSerialNo22 = serialname.InvoiceSerialNo22,
                InvoiceName3 = serialname.InvoiceName3,
                InvoiceSerialNo3 = serialname.InvoiceSerialNo3,
                InvoiceSerialNo33 = serialname.InvoiceSerialNo33,

                SelectedCulture = user.SelectedCulture.Trim(),
                IsSuccess = isSuccess,
                IsSuccess2 = isSuccess2,
                SchoolCode = user.SelectedSchoolCode,
                SchoolEInvoiceIsActive = isActive,
                ConnectionString = cs,
            };
            return View(invoiceViewModel);

        }
        [Route("M400Invoice/InvoiceProgramDataRead1/{userID}/{StudentInvoiceID}")]
        public IActionResult InvoiceProgramDataRead1(int userID, int studentInvoiceID)
        {
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            var user = _usersRepository.GetUser(userID);
            var invoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);
            if (invoice != null)
            {
                var invoicedetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(studentInvoiceID);

                var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(invoice.StudentInvoiceAddressID);
                if (studentInvoiceAddress == null)
                    studentInvoiceAddress = new StudentInvoiceAddress();

                foreach (var detail in invoicedetail)
                {
                    var invoiceViewModel = new InvoiceViewModel();
                    {
                        invoiceViewModel.StudentInvoiceAddress = studentInvoiceAddress;
                        if (detail.UnitPrice > 0)
                        {
                            invoiceViewModel.ViewModelID = invoice.StudentID;
                            invoiceViewModel.StudentID = invoice.StudentID;
                            invoiceViewModel.Period = user.UserPeriod;
                            invoiceViewModel.SchoolID = user.SchoolID;

                            invoiceViewModel.InvoiceSerialNo = invoice.InvoiceSerialNo;
                            invoiceViewModel.InvoiceDate = invoice.InvoiceDate;

                            invoiceViewModel.StudentInvoiceID = detail.StudentInvoiceID;
                            invoiceViewModel.StudentInvoiceDetailID = detail.StudentInvoiceDetailID;
                            invoiceViewModel.Explanation = detail.Explanation;
                            invoiceViewModel.Quantity = detail.Quantity;
                            invoiceViewModel.UnitPrice = detail.UnitPrice;
                            invoiceViewModel.Discount = detail.Discount;
                            invoiceViewModel.TaxPercent = detail.TaxPercent;
                            invoiceViewModel.Tax = detail.Tax;
                            invoiceViewModel.Amount = detail.Amount;

                            invoiceViewModel.WithholdingPercent1 = (int)invoice.WithholdingPercent1;
                            invoiceViewModel.WithholdingPercent2 = (int)invoice.WithholdingPercent2;
                            invoiceViewModel.WithholdingExplanation = invoice.WithholdingExplanation;
                            invoiceViewModel.WithholdingTax = invoice.WithholdingTax;
                            invoiceViewModel.WithholdingTotal = invoice.WithholdingTotal;
                            invoiceViewModel.DirtyField = "0";

                            list.Add(invoiceViewModel);
                        }
                    }
                }
            };

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        [Route("M400Invoice/InvoiceProgramDataRead2/{userID}/{StudentInvoiceID}")]
        public IActionResult InvoiceProgramDataRead2(int userID, int studentInvoiceID)
        {
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            var user = _usersRepository.GetUser(userID);
            var invoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);
            if (invoice != null)
            {
                var invoicedetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(studentInvoiceID);

                var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(invoice.StudentInvoiceAddressID);
                if (studentInvoiceAddress == null)
                    studentInvoiceAddress = new StudentInvoiceAddress();

                foreach (var detail in invoicedetail)
                {
                    var invoiceViewModel = new InvoiceViewModel();
                    {
                        invoiceViewModel.StudentInvoiceAddress = studentInvoiceAddress;
                        if (detail.UnitPrice > 0)
                        {
                            invoiceViewModel.ViewModelID = invoice.StudentID;
                            invoiceViewModel.StudentID = invoice.StudentID;
                            invoiceViewModel.Period = user.UserPeriod;
                            invoiceViewModel.SchoolID = user.SchoolID;

                            invoiceViewModel.InvoiceSerialNo = invoice.InvoiceSerialNo;
                            invoiceViewModel.InvoiceDate = invoice.InvoiceDate;

                            invoiceViewModel.StudentInvoiceID = detail.StudentInvoiceID;
                            invoiceViewModel.StudentInvoiceDetailID = detail.StudentInvoiceDetailID;
                            invoiceViewModel.Explanation = detail.Explanation;
                            invoiceViewModel.Quantity = detail.Quantity;
                            invoiceViewModel.UnitPrice = detail.UnitPrice;
                            invoiceViewModel.Discount = detail.Discount;
                            invoiceViewModel.TaxPercent = detail.TaxPercent;
                            invoiceViewModel.Tax = detail.Tax;
                            invoiceViewModel.Amount = detail.Amount;

                            invoiceViewModel.WithholdingPercent1 = (int)invoice.WithholdingPercent1;
                            invoiceViewModel.WithholdingPercent2 = (int)invoice.WithholdingPercent2;
                            invoiceViewModel.WithholdingExplanation = invoice.WithholdingExplanation;
                            invoiceViewModel.WithholdingTax = invoice.WithholdingTax;
                            invoiceViewModel.WithholdingTotal = invoice.WithholdingTotal;
                            invoiceViewModel.DirtyField = "0";

                            list.Add(invoiceViewModel);
                        }
                    }
                }
            };

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        [Route("M400Invoice/InvoiceProgramDataRead3/{userID}/{studentInvoiceID}/{accountingID}")]
        public IActionResult InvoiceProgramDataRead3(int userID, int studentInvoiceID, int accountingID)
        {
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            var user = _usersRepository.GetUser(userID);
            if (accountingID != 0)
            {

                var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
                byte tp = (byte)_schoolFeeRepository.GetSchoolFeeSelect(school.SchoolID, "L1").Where(b => b.Tax > 0).LastOrDefault().Tax;
                var account = _accountingRepository.GetAccountingID(accountingID);

                var studentInvoiceDetail = new StudentInvoiceDetail();
                studentInvoiceDetail.StudentInvoiceDetailID = 0;
                studentInvoiceDetail.StudentInvoiceID = 0;
                studentInvoiceDetail.SchoolID = user.SchoolID;
                studentInvoiceDetail.StudentID = 0;
                studentInvoiceDetail.SchoolFeeID = 0;
                studentInvoiceDetail.Period = user.UserPeriod;
                studentInvoiceDetail.InvoiceSerialNo = 0;
                studentInvoiceDetail.Explanation = account.Explanation;
                decimal unitPrice = 0;
                if (account.Debt > 0) unitPrice = (decimal)account.Debt;
                else unitPrice = (decimal)account.Credit;
                studentInvoiceDetail.UnitPrice = unitPrice;
                decimal total = unitPrice;

                studentInvoiceDetail.Quantity = 1;
                studentInvoiceDetail.Discount = 0;
                studentInvoiceDetail.Amount = total;

                double taxPercent = (1 + (Convert.ToDouble(tp) / 100));
                decimal tax1 = Math.Round(total - (total / Convert.ToDecimal(taxPercent)), school.CurrencyDecimalPlaces);
                studentInvoiceDetail.TaxPercent = tp;
                studentInvoiceDetail.Tax = tax1;

                studentInvoiceDetail.InvoiceStatus = true;

                var invoice = new StudentInvoice();
                invoice.StudentInvoiceID = 0;
                invoice.SchoolID = user.SchoolID;
                invoice.StudentID = 0;
                invoice.StudentInvoiceAddressID = 0;
                invoice.Period = user.UserPeriod;
                invoice.InvoiceSerialNo = 0;
                invoice.InvoiceDate = DateTime.Now;
                invoice.InvoiceCutDate = null;
                invoice.DExplanation = null;
                invoice.DUnitPrice = unitPrice;
                invoice.DQuantity = 1;
                invoice.DDiscount = 0;
                invoice.DTaxPercent = (byte)taxPercent;
                invoice.DTax = tax1;
                invoice.DAmount = total;
                //invoice.WithholdingPercent1 = 5;
                //invoice.WithholdingPercent2 = 10;
                //invoice.WithholdingCode = "0650";
                //invoice.WithholdingExplanation = "";
                //invoice.WithholdingTax = 0;
                //invoice.WithholdingTotal = 0;
                invoice.IsPlanned = false;
                invoice.InvoiceType = true;
                invoice.IsActive = true;
                invoice.IsBatchPrint = false;

                var invoiceViewModel = new InvoiceViewModel();
                {
                    //invoiceViewModel.StudentInvoiceAddress = studentInvoiceAddress;

                    invoiceViewModel.ViewModelID = invoice.StudentID;
                    invoiceViewModel.StudentID = invoice.StudentID;
                    invoiceViewModel.Period = user.UserPeriod;
                    invoiceViewModel.SchoolID = user.SchoolID;

                    invoiceViewModel.InvoiceSerialNo = invoice.InvoiceSerialNo;
                    invoiceViewModel.InvoiceDate = invoice.InvoiceDate;

                    invoiceViewModel.StudentInvoiceID = studentInvoiceDetail.StudentInvoiceID;
                    invoiceViewModel.StudentInvoiceDetailID = studentInvoiceDetail.StudentInvoiceDetailID;
                    invoiceViewModel.Explanation = studentInvoiceDetail.Explanation;
                    invoiceViewModel.Quantity = studentInvoiceDetail.Quantity;
                    invoiceViewModel.UnitPrice = studentInvoiceDetail.UnitPrice;
                    invoiceViewModel.Discount = studentInvoiceDetail.Discount;
                    invoiceViewModel.TaxPercent = studentInvoiceDetail.TaxPercent;
                    invoiceViewModel.Tax = studentInvoiceDetail.Tax;
                    invoiceViewModel.Amount = studentInvoiceDetail.Amount;

                    //invoiceViewModel.WithholdingPercent1 = (int)invoice.WithholdingPercent1;
                    //invoiceViewModel.WithholdingPercent2 = (int)invoice.WithholdingPercent2;
                    //invoiceViewModel.WithholdingExplanation = invoice.WithholdingExplanation;
                    //invoiceViewModel.WithholdingTax = invoice.WithholdingTax;
                    //invoiceViewModel.WithholdingTotal = invoice.WithholdingTotal;
                    invoiceViewModel.DirtyField = "0";

                    list.Add(invoiceViewModel);
                }
            }
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> Invoice(StudentInvoiceAddress studentInvoiceAddress, InvoiceViewModel invoiceViewModel)
        {
            await Task.Delay(200);
            var user = _usersRepository.GetUser(invoiceViewModel.UserID);
            if (invoiceViewModel.AccountCode == null)
            {
                if (!studentInvoiceAddress.isEmpty)
                {
                    studentInvoiceAddress.StudentID = invoiceViewModel.StudentID;
                    if (studentInvoiceAddress.StudentInvoiceAddressID == 0)
                    {
                        _studentInvoiceAddressRepository.CreateStudentInvoiceAddress(studentInvoiceAddress);
                    }
                    else
                    {
                        _studentInvoiceAddressRepository.UpdateStudentInvoiceAddress(studentInvoiceAddress);
                    }
                }
            }
            else
            {
                var accountCodeID = _accountCodesRepository.GetAccountCode2(invoiceViewModel.AccountCode, user.UserPeriod).AccountCodeID;
                var accountCodesDetail = _accountCodesDetailRepository.GetAccountCodesDetailID1(accountCodeID);
                accountCodesDetail.InvoiceProfile = invoiceViewModel.StudentInvoiceAddress.InvoiceProfile;
                accountCodesDetail.InvoiceTypeParameter = invoiceViewModel.StudentInvoiceAddress.InvoiceTypeParameter;
                accountCodesDetail.ParameterExceptionCode = invoiceViewModel.StudentInvoiceAddress.ParameterExceptionCode;
                accountCodesDetail.Notes = invoiceViewModel.StudentInvoiceAddress.Notes;
                _accountCodesDetailRepository.UpdateAccountCodesDetail(accountCodesDetail);
            }
            var serialname = _pSerialNumberRepository.GetPSerialNumber(invoiceViewModel.SchoolID);
            if (studentInvoiceAddress.InvoiceProfile == true)
            {
                serialname.InvoiceSerialNo1 = invoiceViewModel.InvoiceSerialNo;
            }
            else
            {
                serialname.InvoiceSerialNo11 = invoiceViewModel.InvoiceSerialNo;
            }
            _pSerialNumberRepository.UpdatePSerialNumber(serialname);

            if (invoiceViewModel.StudentInvoiceID != 0)
            {
                var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceID(invoiceViewModel.Period, invoiceViewModel.SchoolID, invoiceViewModel.StudentInvoiceID);
                cutinvoice.InvoiceSerialNo = invoiceViewModel.InvoiceSerialNo;
                cutinvoice.InvoiceCutDate = DateTime.Now;
                cutinvoice.InvoiceCutTime = DateTime.Now.TimeOfDay;
                cutinvoice.InvoiceStatus = true;
                cutinvoice.StudentInvoiceAddressID = studentInvoiceAddress.StudentInvoiceAddressID;
                _studentInvoiceRepository.UpdateStudentInvoice(cutinvoice);

            }

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            school.InvoiceProfile = invoiceViewModel.InvoiceProfile;
            school.InvoiceTypeParameter = invoiceViewModel.InvoiceTypeParameter;
            school.ParameterExceptionCode = invoiceViewModel.ParameterExceptionCode;
            _schoolInfoRepository.UpdateSchoolInfo(school);

            var tempM101 = _tempM101Repository.GetTempM101All(invoiceViewModel.SchoolID, invoiceViewModel.UserID);
            foreach (var item in tempM101)
            {
                _tempM101Repository.DeleteTempM101(item);
            }

            var temp = new TempM101();
            temp.ID = 0;
            temp.UserID = invoiceViewModel.UserID;
            temp.SchoolID = invoiceViewModel.SchoolID;
            temp.StudentID = invoiceViewModel.StudentID;

            decimal unitPrice = 0;
            decimal discount = 0;
            decimal subTotal = 0;
            decimal tax = 0;
            decimal total = 0;

            var inv = _studentInvoiceRepository.GetStudentInvoiceSerialNo(invoiceViewModel.Period, invoiceViewModel.SchoolID, invoiceViewModel.InvoiceSerialNo);
            if (invoiceViewModel.StudentInvoiceID == 0 && inv != null)
            {
                invoiceViewModel.StudentInvoiceID = inv.StudentInvoiceID;
                inv.StudentInvoiceAddressID = studentInvoiceAddress.StudentInvoiceAddressID;
                _studentInvoiceRepository.UpdateStudentInvoice(inv);
            }

            var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(invoiceViewModel.StudentInvoiceID);

            unitPrice = invoiceDetail.Sum(b => b.UnitPrice);
            discount = invoiceDetail.Sum(b => b.Discount);
            tax = invoiceDetail.Sum(b => b.Tax);
            subTotal = unitPrice - tax - discount;
            total = invoiceDetail.Sum(b => b.Amount);
            foreach (var item in invoiceDetail)
            {
                item.InvoiceSerialNo = invoiceViewModel.InvoiceSerialNo;
                item.InvoiceStatus = true;
                _studentInvoiceDetailRepository.UpdateStudentInvoiceDetail(item);
            }

            MoneyToText txt = new MoneyToText();
            temp.InWriting = txt.ConvertToText(total);

            temp.Fee01 = discount;
            temp.Fee02 = subTotal;
            temp.Fee03 = tax;
            temp.Fee04 = total;
            temp.StudentName = null;
            temp.ReceiptNo = invoiceViewModel.InvoiceSerialNo;
            string classroomName = "";
            if (invoiceViewModel.StudentID > 0)
            {
                var student = _studentRepository.GetStudent(invoiceViewModel.StudentID);

                if (student.ClassroomID > 0)
                {
                    classroomName = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomName;
                }

                temp.StudentName = student.FirstName + " " + student.LastName + " " + classroomName;
            }

            _tempM101Repository.CreateTempM101(temp);

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            TempData["studentInvoiceAddressID"] = studentInvoiceAddress.StudentInvoiceAddressID;
            return Json(new { studentInvoiceAddressID = studentInvoiceAddress.StudentInvoiceAddressID, studentInvoiceID = invoiceViewModel.StudentInvoiceID });
        }

        [Route("M400Invoice/InvoiceProgramDataAddressRead/{userID}/{studentInvoiceID}/{studentInvoiceAddressID}")]
        public IActionResult InvoiceProgramDataAddressRead(int userID, int studentInvoiceID, int studentInvoiceAddressID)
        {
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            var user = _usersRepository.GetUser(userID);
            var invoice = _studentInvoiceRepository.GetStudentInvoiceAddressID(user.UserPeriod, user.SchoolID, studentInvoiceID, studentInvoiceAddressID);
            if (invoice != null)
            {
                var invoicedetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(invoice.StudentInvoiceID);

                var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(invoice.StudentInvoiceAddressID);
                if (studentInvoiceAddress == null)
                    studentInvoiceAddress = new StudentInvoiceAddress();

                foreach (var detail in invoicedetail)
                {
                    var invoiceViewModel = new InvoiceViewModel();
                    {
                        invoiceViewModel.StudentInvoiceAddress = studentInvoiceAddress;
                        if (detail.Amount > 0)
                        {
                            invoiceViewModel.ViewModelID = invoice.StudentID;
                            invoiceViewModel.StudentID = invoice.StudentID;
                            invoiceViewModel.Period = user.UserPeriod;
                            invoiceViewModel.SchoolID = user.SchoolID;

                            invoiceViewModel.InvoiceSerialNo = invoice.InvoiceSerialNo;
                            invoiceViewModel.InvoiceDate = invoice.InvoiceDate;

                            invoiceViewModel.StudentInvoiceID = detail.StudentInvoiceID;
                            invoiceViewModel.StudentInvoiceDetailID = detail.StudentInvoiceDetailID;
                            invoiceViewModel.Explanation = detail.Explanation;
                            invoiceViewModel.Quantity = detail.Quantity;
                            invoiceViewModel.UnitPrice = detail.UnitPrice;
                            invoiceViewModel.Discount = detail.Discount;
                            invoiceViewModel.TaxPercent = detail.TaxPercent;
                            invoiceViewModel.Tax = detail.Tax;
                            invoiceViewModel.Amount = detail.Amount;

                            invoiceViewModel.WithholdingPercent1 = (int)invoice.WithholdingPercent1;
                            invoiceViewModel.WithholdingPercent2 = (int)invoice.WithholdingPercent2;
                            invoiceViewModel.WithholdingExplanation = invoice.WithholdingExplanation;
                            invoiceViewModel.WithholdingTax = invoice.WithholdingTax;
                            invoiceViewModel.WithholdingTotal = invoice.WithholdingTotal;
                            invoiceViewModel.DirtyField = "0";

                            list.Add(invoiceViewModel);
                        }
                    }
                }
            };
            return Json(list);
        }


        [Route("M400Invoice/InvoiceProgramDataCreate/{strResult}/{userID}/{studentID}/{dateString}/{percent1}/{percent2}/{code}/{explanation}/{withholdingTax}/{withholdingTotal}/{studentInvoiceAddressID}/{invoiceNameId}/{serialNo}")]
        public IActionResult InvoiceProgramDataCreate([Bind(Prefix = "models")] string strResult, int userID, int studentID, string dateString, int percent1, int percent2, string code, string explanation, decimal withholdingTax, decimal withholdingTotal, int studentInvoiceAddressID, int invoiceNameId, int serialNo)
        {
            DateTime transactiondate = DateTime.Parse(dateString);
            var user = _usersRepository.GetUser(userID);
            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoiceDetail>>(strResult);

            var invoice = new StudentInvoice();
            InvoiceInitialize(invoice, user.UserPeriod, user.UserID);
            _studentInvoiceRepository.CreateStudentInvoice(invoice);

            var studentInvoiceID = invoice.StudentInvoiceID;
            //Invoice Detail Create
            var i = 0;
            decimal unitPriceT = 0;
            decimal discountT = 0;
            decimal taxT = 0;
            decimal taxPercent = 0;
            foreach (var detail in json)
            {
                detail.StudentInvoiceDetailID = 0;
                detail.StudentInvoiceID = studentInvoiceID;
                detail.SchoolID = user.SchoolID;
                detail.StudentID = studentID;
                detail.Period = user.UserPeriod;
                detail.Explanation = json[i].Explanation;
                detail.SchoolFeeID = json[i].SchoolFeeID;
                detail.Quantity = json[i].Quantity;
                detail.InvoiceStatus = false;
                detail.UnitPrice = json[i].UnitPrice;
                detail.Amount = json[i].Amount;
                detail.Discount = json[i].Discount;
                //detail.InvoiceSerialNo = serialNo;

                if (detail.TaxPercent < 0) detail.TaxPercent = 0;
                else detail.TaxPercent = json[i].TaxPercent;

                if (detail.Amount < 0) detail.Amount = 0;
                else detail.Amount = json[i].Amount;

                detail.Tax = json[i].Tax;

                unitPriceT += detail.UnitPrice;
                discountT += detail.Discount;
                taxT += detail.Tax;
                taxPercent = Convert.ToByte(detail.TaxPercent);

                if (ModelState.IsValid)
                {
                    _studentInvoiceDetailRepository.CreateStudentInvoiceDetail(detail);
                }
                i += 1;
            }

            //Invoice Create
            invoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);

            invoice.SchoolID = user.SchoolID;
            invoice.StudentID = studentID;
            //invoice.StudentInvoiceAddressID = studentInvoiceAddressID;
            invoice.Period = user.UserPeriod;
            invoice.InvoiceSerialNo = serialNo;
            invoice.InvoiceDate = transactiondate;
            invoice.InvoiceCutDate = transactiondate;
            invoice.InvoiceCutTime = DateTime.Now.TimeOfDay;
            invoice.DQuantity = 1;
            invoice.DUnitPrice = unitPriceT;
            invoice.DDiscount = discountT;
            invoice.DTaxPercent = Convert.ToByte(taxPercent);
            invoice.DTax = taxT;
            invoice.DAmount = (invoice.DUnitPrice - invoice.DDiscount);

            invoice.WithholdingPercent1 = percent1;
            invoice.WithholdingPercent2 = percent2;
            invoice.WithholdingCode = code;
            invoice.WithholdingExplanation = explanation;
            invoice.WithholdingTax = withholdingTax;
            invoice.WithholdingTotal = withholdingTotal;

            invoice.IsPlanned = false;
            invoice.InvoiceType = true;
            invoice.InvoiceStatus = true;
            invoice.IsActive = true;

            if (ModelState.IsValid)
            {
                _studentInvoiceRepository.UpdateStudentInvoice(invoice);
            }

            return Json(new { studentInvoiceID = studentInvoiceID });
        }

        [HttpPost]
        [Route("M400Invoice/InvoiceProgramDataUpdate/{strResult}/{percent1}/{percent2}/{code}/{explanation}/{withholdingTax}/{withholdingTotal}")]
        public IActionResult InvoiceProgramDataUpdate([Bind(Prefix = "models")] string strResult, int percent1, int percent2, string code, string explanation, int withholdingTax, int withholdingTotal)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoiceDetail>>(strResult);

            decimal cutUnitPrice = 0;
            decimal cutDiscount = 0;
            decimal cutAmount = 0;

            var invoice = _studentInvoiceRepository.GetStudentInvoiceID(json[0].Period, json[0].SchoolID, json[0].StudentInvoiceID);
            if (invoice != null)
            {
                var detail = _studentInvoiceDetailRepository.GetStudentInvoiceDetailIDSingle(json[0].StudentInvoiceDetailID);

                detail.Explanation = json[0].Explanation;
                detail.Quantity = json[0].Quantity;
                detail.TaxPercent = json[0].TaxPercent;
                detail.UnitPrice = json[0].UnitPrice;
                detail.Discount = json[0].Discount;
                detail.Tax = json[0].Tax;
                detail.Amount = json[0].Amount;
                detail.InvoiceStatus = json[0].InvoiceStatus;

                _studentInvoiceDetailRepository.UpdateStudentInvoiceDetail(detail);
                _studentInvoiceDetailRepository.Save();

                var invoicedetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(json[0].StudentInvoiceID);
                foreach (var item in invoicedetail)
                {
                    cutUnitPrice += item.UnitPrice;
                    cutDiscount += item.Discount;
                    cutAmount += item.Amount;
                }
                invoice.DUnitPrice = cutUnitPrice;
                invoice.DDiscount = cutDiscount;
                invoice.DAmount = cutAmount;

                invoice.WithholdingPercent1 = percent1;
                invoice.WithholdingPercent2 = percent2;
                invoice.WithholdingCode = code;
                invoice.WithholdingExplanation = explanation;
                invoice.WithholdingTax = withholdingTax;
                invoice.WithholdingTotal = withholdingTotal;

                _studentInvoiceRepository.UpdateStudentInvoice(invoice);
            }

            return Json(true);
        }
        public IActionResult InvoiceProgramDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoiceDetail>>(strResult);

            var invoice = _studentInvoiceRepository.GetStudentInvoiceID(json[0].Period, json[0].SchoolID, json[0].StudentInvoiceID);
            if (invoice != null)
            {
                var invoicedetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(json[0].StudentInvoiceID);
                foreach (var detail in invoicedetail)
                {
                    _studentInvoiceDetailRepository.DeleteStudentInvoiceDetail(detail);
                }

                _studentInvoiceRepository.DeleteStudentInvoice(invoice);
                _studentInvoiceRepository.Save();
            }

            return Json(true);
        }

        #endregion

        #region Invoice Archive
        [Route("M400Invoice/InvoiceArchiveDataRead/{userID}")]
        public IActionResult InvoiceArchiveDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var invoice = _studentInvoiceRepository.GetStudentInvoiceAllTrue(user.UserPeriod, user.SchoolID);

            List<InvoiceArchiveViewModel> list = new List<InvoiceArchiveViewModel>();
            if (invoice != null)
            {
                foreach (var detail in invoice)
                {
                    var isExist = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(detail.StudentInvoiceAddressID);
                    if (isExist != null)
                    {
                        if (isExist.StudentID == 0)
                        {
                            var invoiceArchiveViewModel = new InvoiceArchiveViewModel();
                            {
                                invoiceArchiveViewModel.StudentInvoiceID = detail.StudentInvoiceID;
                                invoiceArchiveViewModel.SchoolID = detail.SchoolID;
                                invoiceArchiveViewModel.StudentID = detail.StudentID;
                                invoiceArchiveViewModel.StudentInvoiceAddressID = detail.StudentInvoiceAddressID;
                                invoiceArchiveViewModel.Period = detail.Period;
                                invoiceArchiveViewModel.StudentID = detail.StudentID;

                                invoiceArchiveViewModel.InvoiceTitle = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(detail.StudentInvoiceAddressID).InvoiceTitle;
                                invoiceArchiveViewModel.InvoiceSerialNo = detail.InvoiceSerialNo;
                                invoiceArchiveViewModel.InvoiceDate = detail.InvoiceDate;
                                invoiceArchiveViewModel.InvoiceCutDate = detail.InvoiceCutDate;

                                invoiceArchiveViewModel.UnitPrice = Convert.ToDecimal(detail.DUnitPrice);
                                invoiceArchiveViewModel.Discount = Convert.ToDecimal(detail.DDiscount);
                                invoiceArchiveViewModel.Tax = Convert.ToDecimal(detail.DTax);
                                invoiceArchiveViewModel.Amount = Convert.ToDecimal(invoiceArchiveViewModel.UnitPrice - invoiceArchiveViewModel.Discount);
                                invoiceArchiveViewModel.IsPlanned = detail.IsPlanned;

                                list.Add(invoiceArchiveViewModel);
                            }
                        }
                    }
                }
            }
            return Json(list);
        }

        [Route("M400Invoice/GetAddress/{userID}/{studentInvoiceID}")]
        public IActionResult GetAddress(int userID, int studentInvoiceID)
        {
            var user = _usersRepository.GetUser(userID);

            var invoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);
            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(invoice.StudentInvoiceAddressID);

            var studentInvoiceAddressID = invoiceAddress.StudentInvoiceAddressID;
            var getTitle = invoiceAddress.InvoiceTitle;
            var getAddress = invoiceAddress.InvoiceAddress;
            var getCityID = invoiceAddress.InvoiceCityParameterID;
            var getTownID = invoiceAddress.InvoiceTownParameterID;
            var getCountry = invoiceAddress.InvoiceCountry;
            var getZip = invoiceAddress.InvoiceZipCode;

            var getTaxOffice = invoiceAddress.InvoiceTaxOffice;
            var getTaxNumber = invoiceAddress.InvoiceTaxNumber;
            var getEMail = invoiceAddress.EMail;
            var getNotes = invoiceAddress.Notes;
            var getProfile = invoiceAddress.InvoiceProfile;
            var getTypeParameter = invoiceAddress.InvoiceTypeParameter;
            var getParameterExceptionCode = invoiceAddress.ParameterExceptionCode;
            var getDetailed = invoiceAddress.IsInvoiceDetailed;
            var getDiscount = invoiceAddress.IsInvoiceDiscount;

            return Json(new
            {
                studentInvoiceAddressID = studentInvoiceAddressID,
                title = getTitle,
                address = getAddress,
                city = getCityID,
                town = getTownID,
                country = getCountry,
                zip = getZip,
                taxOffice = getTaxOffice,
                taxNumber = getTaxNumber,
                eMail = getEMail,
                notes = getNotes,
                profile = getProfile,
                typeParameter = getTypeParameter,
                parameterExceptionCode = getParameterExceptionCode,
                detailed = getDetailed,
                discount = getDiscount,
                serialNo = invoice.InvoiceSerialNo,
                invoiceDate = invoice.InvoiceDate,

                withholdingPercent1 = invoice.WithholdingPercent1,
                withholdingPercent2 = invoice.WithholdingPercent2,
                withholdingCode = invoice.WithholdingCode,
                withholdingExplanation = invoice.WithholdingExplanation,
                withholdingTax = invoice.WithholdingTax,
                withholdingTotal = invoice.WithholdingTotal,

            });
        }

        public IActionResult GetAddressQuery()
        {
            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressAll();

            return Json(invoiceAddress);
        }

        [Route("M400Invoice/CurrentCardGetAddress/{userID}/{studentInvoiceID}/{accountingID}")]
        public IActionResult CurrentCardGetAddress(int userID, int studentInvoiceID, int accountingID)
        {
            var user = _usersRepository.GetUser(userID);

            var accounting = _accountingRepository.GetAccountingID(accountingID);
            var accountCode = _accountCodesRepository.GetAccountCode(accounting.AccountCode, user.UserPeriod);
            var invoiceAddress = _accountCodesDetailRepository.GetAccountCodesDetailID1(accountCode.AccountCodeID);


            //accountCodesDetail = _accountCodesDetailRepository.GetAccountCodesDetailID1(accountCodeID);

            //var invoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);
            //var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(invoice.StudentInvoiceAddressID);

            var studentInvoiceAddressID = invoiceAddress.AccountCodeDetailID;
            var getTitle = invoiceAddress.InvoiceTitle;
            var getAddress = invoiceAddress.InvoiceAddress;
            var getCityID = invoiceAddress.InvoiceCityParameterID;
            var getTownID = invoiceAddress.InvoiceTownParameterID;
            var getCountry = invoiceAddress.InvoiceCountry;
            var getZip = invoiceAddress.InvoiceZipCode;

            var getTaxOffice = invoiceAddress.InvoiceTaxOffice;
            var getTaxNumber = invoiceAddress.InvoiceTaxNumber;
            var getEMail = invoiceAddress.EMail;
            var getNotes = invoiceAddress.Notes;
            var getProfile = invoiceAddress.InvoiceProfile;
            var getTypeParameter = invoiceAddress.InvoiceTypeParameter;
            var getParameterExceptionCode = invoiceAddress.ParameterExceptionCode;

            //var getDetailed = invoiceAddress.IsInvoiceDetailed;
            //var getDiscount = invoiceAddress.IsInvoiceDiscount;

            return Json(new
            {
                studentInvoiceAddressID = studentInvoiceAddressID,
                title = getTitle,
                address = getAddress,
                city = getCityID,
                town = getTownID,
                country = getCountry,
                zip = getZip,
                taxOffice = getTaxOffice,
                taxNumber = getTaxNumber,
                eMail = getEMail,
                notes = "",
                profile = getProfile,
                typeParameter = getTypeParameter,
                parameterExceptionCode = getParameterExceptionCode,
                //detailed = getDetailed,
                //discount = getDiscount,
                //serialNo = invoice.InvoiceSerialNo,
                //invoiceDate = invoice.InvoiceDate,

                //withholdingPercent1 = invoice.WithholdingPercent1,
                //withholdingPercent2 = invoice.WithholdingPercent2,
                //withholdingCode = invoice.WithholdingCode,
                //withholdingExplanation = invoice.WithholdingExplanation,
                //withholdingTax = invoice.WithholdingTax,
                //withholdingTotal = invoice.WithholdingTotal,

            });
        }

        #endregion

        #region invoice Batch
        [HttpGet]
        public IActionResult InvoiceBatch(int userID, int studentID, string dateString, bool isLoop)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Fatura İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            DateTime invoiceDate = DateTime.Now;
            if (dateString != "null" && dateString != null) invoiceDate = DateTime.Parse(dateString);

            var isSuccess = false;
            if (school.EIIsActive == true)
                if (school.EIUserPassword == null || school.EIUserName == null || school.EIInvoiceSerialCode1 == null || school.EIInvoiceSerialCode2 == null || school.EIIntegratorNameID == 0)
                    isSuccess = true;
            var student = _studentRepository.GetStudent(studentID);

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            ViewBag.date = user.UserDate;
            TempData["studentID"] = studentID;

            var period = user.UserPeriod;

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            if (studentTemp != null && studentTemp.Installment == 0)
            {
                studentTemp.Installment = school.DefaultInstallment;
                _studentTempRepository.UpdateStudentTemp(studentTemp);
            }

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
                gendertxt = gender.FirstOrDefault(p => p.CategoryID == student.GenderTypeCategoryID).CategoryName;

            categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            string statutxt = "";
            if (student != null)
                statutxt = statu.FirstOrDefault(p => p.CategoryID == student.RegistrationTypeCategoryID).CategoryName;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            NCSIni ini;
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            ini = new NCSIni(filePath + "\\" + "\\INVOICEDATE.ini");
            string maxPrint = ini.IniReadValue("Invoice", "MaxPrint").Trim();
            if (maxPrint == null || maxPrint == "" || maxPrint == "0") maxPrint = "10";
            string iniRemainStr = ini.IniReadValue("Invoice", "RemainInvoice").Trim();
            int iniRemain = 0;
            if (iniRemainStr != "") iniRemain = Convert.ToInt32(iniRemainStr);

            ini.IniWriteValue("Invoice", "InvoiceDate", "");
            ini.IniWriteValue("Invoice", "PrintedInvoice", "");
            ini.IniWriteValue("Invoice", "RemainInvoice", "");
            ini.IniWriteValue("Invoice", "MaxPrint", "");
            isLoop = false;

            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressAll();
            var isSuccess2 = true;
            if (studentInvoiceAddress == null) isSuccess2 = false;
            else
            {
                foreach (var item in studentInvoiceAddress)
                {
                    if (item.InvoiceTaxNumber == null || item.InvoiceTaxNumber == "" && item.StudentID > 0)
                    {
                        student = _studentRepository.GetStudent2(user.SchoolID, item.StudentID);
                        if (student != null)
                        {
                            var statuName = _parameterRepository.GetParameter(student.StatuCategoryID).CategoryName;
                            if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;
                            if (student.IdNumber == null || item.EMail == null || item.InvoiceTitle == null || item.InvoiceAddress == null && item.InvoiceCityParameterID == 0 && item.InvoiceTownParameterID == 0)
                            {
                                isSuccess2 = false;
                                break;
                            }
                        }
                    }
                }
            }

            filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString(), "earsiv.xslt");
            bool isSuccess3 = false;
            if (System.IO.File.Exists(filePath)) isSuccess3 = true;
            if (!(bool)school.EIIsActive) isSuccess3 = true;

            if (student == null) { student = new Student(); }
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
                SelectedSchoolCode = user.SelectedSchoolCode,
                SelectedCulture = user.SelectedCulture.Trim(),
                IsSuccess = isSuccess,
                IsSuccess2 = isSuccess2,
                IsSuccess3 = isSuccess3,
                StartDate = invoiceDate,
                MaxPrint = maxPrint,
                IsLoop = isLoop,
            };
            return View(studentViewModel);
        }

        public IActionResult InvoiceBatchReset(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            NCSIni ini;
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            ini = new NCSIni(filePath + "\\" + "\\INVOICEDATE.ini");
            ini.IniWriteValue("Invoice", "InvoiceDate", "");
            ini.IniWriteValue("Invoice", "PrintedInvoice", "");
            ini.IniWriteValue("Invoice", "RemainInvoice", "");
            ini.IniWriteValue("Invoice", "MaxPrint", "");

            return View(true);
        }

        [Route("M400Invoice/InvoiceBatchDataRead/{userID}/{studentID}")]
        public IActionResult InvoiceBatchDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var studentinvoice = _studentInvoiceRepository.GetStudentInvoice(user.UserPeriod, user.SchoolID, studentID);
            return Json(studentinvoice);
        }

        public IActionResult InvoiceBatchConfirm(int userID, int classroomID, int studentID, string dateString)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            IEnumerable<Student> allStudents = null;
            List<Student> students = new List<Student>();
            List<StudentViewModel> list = new List<StudentViewModel>();
            List<StudentPeriods> studentPeriod = new List<StudentPeriods>();
            string classroomName = "";

            if (classroomID != 0)
                classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;

            if (classroomID == 0)
                allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
            else
            {
                allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID).Where(b => b.ClassroomID == classroomID);
                if (allStudents.Count() == 0)
                {
                    allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                    studentPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod).Where(s => s.ClassroomName == classroomName).ToList();
                    students = allStudents.Where(s => studentPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                    goto CONTINUE;
                }
            }
            studentPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod).ToList();
            students = allStudents.Where(s => studentPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
        CONTINUE:;

            NCSIni ini;
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            ini = new NCSIni(filePath + "\\" + "\\INVOICEDATE.ini");
            string iniDate = ini.IniReadValue("Invoice", "InvoiceDate").Trim();
            string iniPrintedStr = ini.IniReadValue("Invoice", "PrintedInvoice").Trim();
            string iniRemainStr = ini.IniReadValue("Invoice", "RemainInvoice").Trim();
            string inimaxPrintStr = ini.IniReadValue("Invoice", "MaxPrint").Trim();

            int iniPrinted = 0;
            int iniRemain = 0;
            int iniMaxPrint = 0;
            if (iniRemainStr != "") iniRemain = Convert.ToInt32(iniRemainStr);
            if (iniPrintedStr != "") iniPrinted = Convert.ToInt32(iniPrintedStr);
            if (inimaxPrintStr != "") iniMaxPrint = Convert.ToInt32(inimaxPrintStr);
            if (iniMaxPrint == 0) iniMaxPrint = 10;

            DateTime date = DateTime.Now;
            if (dateString != null)
                date = DateTime.Parse(dateString);

            var invoicex = _studentInvoiceRepository.GetStudentInvoiceAllFalse(user.UserPeriod, user.SchoolID, date);
            var invoice = students.Where(s => invoicex.Where(p => s.StudentID == p.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();

            if (iniDate == "")
            {
                //var invoicex = _studentInvoiceRepository.GetStudentInvoiceAllFalse(user.UserPeriod, user.SchoolID, date);
                //var invoice = students.Where(s => invoicex.Where(p => s.ClassroomID == classroomID && p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();

                if (invoice.Count() > 0) iniPrinted = invoice.Count();
                if (iniPrinted < 10) iniMaxPrint = invoice.Count();
                iniRemain = iniPrinted;

                ini.IniWriteValue("Invoice", "InvoiceDate", dateString);
                ini.IniWriteValue("Invoice", "PrintedInvoice", iniPrinted.ToString());
                ini.IniWriteValue("Invoice", "MaxPrint", iniMaxPrint.ToString());
            }
            if (iniDate != "")
            {
                dateString = iniDate;
                studentID = 0;
                if (iniRemain > 0) iniRemain--;
                else iniRemain = 0;
                ini.IniWriteValue("Invoice", "RemainInvoice", iniRemain.ToString());
            }

            DateTime invoiceDate = DateTime.Parse(dateString);

            var studentName = "";
            var parentName = "";
            foreach (var item in invoice)
            {
                if (item.StudentID == studentID || studentID == 0)
                {
                    var inv = _studentInvoiceRepository.GetStudentInvoiceControl(user.UserPeriod, user.SchoolID, item.StudentID, invoiceDate);
                    if (inv != null && inv.IsBatchPrint == false)
                    {
                        if (studentName == "") studentName = item.FirstName + " " + item.LastName;
                        if (parentName == "") parentName = item.ParentName;
                        studentID = item.StudentID;
                        break;
                    }
                }
            }

            string EIIsActive = "Pasif";
            if (school.EIIsActive == true) EIIsActive = "Etkin";
            var invoiceShowViewModel = new InvoiceConfirmViewModel
            {
                UserID = userID,
                ClassroomID = classroomID,
                Period = user.UserPeriod,
                SchoolID = user.SchoolID,
                SelectedCulture = user.SelectedCulture.Trim(),
                InvoiceBatchDate = invoiceDate,
                MaxPrint = iniMaxPrint,
                TotalInvoice = iniPrinted,
                RemainingInvoice = iniRemain,
                StudentName = studentName,
                StudentID = studentID,
                ParentName = parentName,
                EIIsActive = EIIsActive,
            };

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(invoiceShowViewModel);
        }
        public async Task<IActionResult> InvoiceBatchStartPrint(int userID, int classroomID, int studentID, string dateString, int remainingInvoice, int printOpt, int maxPrint)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            string selectedLanguage = user.SelectedCulture.Trim();

            DateTime invoiceDate = DateTime.Parse(dateString);
            IEnumerable<Student> allStudents = null;
            List<Student> students = new List<Student>();
            List<StudentViewModel> list = new List<StudentViewModel>();
            List<StudentPeriods> studentPeriod = new List<StudentPeriods>();
            string classroomName = "";

            if (classroomID != 0)
                classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;

            if (classroomID == 0)
                allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
            else
            {
                allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID).Where(b => b.ClassroomID == classroomID);
                if (allStudents.Count() == 0)
                {
                    allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                    studentPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod).Where(s => s.ClassroomName == classroomName).ToList();
                    students = allStudents.Where(s => studentPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                    goto CONTINUE;
                }
            }
            studentPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod).ToList();
            students = allStudents.Where(s => studentPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
        CONTINUE:;

            var tempM101 = _tempM101Repository.GetTempM101All(user.SchoolID, userID);
            foreach (var itemx in tempM101)
            {
                _tempM101Repository.DeleteTempM101(itemx);
            }
            var serialname = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;
            string cs = $"Data Source={dataSource};Initial Catalog={user.SelectedSchoolCode};User Id=sa;Password={password};";

            bool isLoop = true;
            string url = url = "/M400Invoice/InvoiceBatch?userID=" + userID + "&studentID=0" + "&dateString=" + dateString + "&isLoop=" + isLoop;

            NCSIni ini;
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            ini = new NCSIni(filePath + "\\" + "\\INVOICEDATE.ini");
            string iniDate = ini.IniReadValue("Invoice", "InvoiceDate").Trim();
            string iniPrintedStr = ini.IniReadValue("Invoice", "PrintedInvoice").Trim();
            string iniRemainStr = ini.IniReadValue("Invoice", "RemainInvoice").Trim();
            string iniMaxPrintStr = ini.IniReadValue("Invoice", "MaxPrint").Trim();
            int inimaxPrint = Convert.ToInt32(iniMaxPrintStr);

            if (inimaxPrint != maxPrint)
                ini.IniWriteValue("Invoice", "MaxPrint", maxPrint.ToString());

            int iniPrinted = 0;
            int iniRemain = 0;
            if (iniRemainStr != "") iniRemain = Convert.ToInt32(iniRemainStr);
            if (iniPrintedStr != "") iniPrinted = Convert.ToInt32(iniPrintedStr);

            int count = 0;
            bool isContinue = false;
            foreach (var item in students)
            {
                if (item.StudentID == studentID || studentID == 0)
                {
                    var invoice = _studentInvoiceRepository.GetStudentInvoiceControl(user.UserPeriod, user.SchoolID, item.StudentID, invoiceDate);
                    if (invoice != null && invoice.IsBatchPrint == false)
                    {
                        var temp = new TempM101();
                        temp.ID = 0;
                        temp.UserID = userID;
                        temp.SchoolID = user.SchoolID;
                        temp.StudentID = item.StudentID;

                        decimal unitPrice = 0;
                        decimal discount = 0;
                        decimal subTotal = 0;
                        decimal tax = 0;
                        decimal total = 0;

                        var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID);
                        if (studentInvoiceAddress != null)
                        {
                            if (studentInvoiceAddress.InvoiceProfile == true)
                            {
                                serialname.InvoiceSerialNo1 += 1;
                                invoice.InvoiceSerialNo = serialname.InvoiceSerialNo1;
                            }
                            else
                            {
                                serialname.InvoiceSerialNo11 += 1;
                                invoice.InvoiceSerialNo = serialname.InvoiceSerialNo11;
                            }
                            invoice.InvoiceCutDate = DateTime.Now;
                            invoice.InvoiceCutTime = DateTime.Now.TimeOfDay;
                            invoice.InvoiceStatus = true;
                        }
                        else
                        {
                            serialname.InvoiceSerialNo11 += 1;
                            invoice.InvoiceSerialNo = serialname.InvoiceSerialNo11;

                            invoice.InvoiceCutDate = DateTime.Now;
                            invoice.InvoiceCutTime = DateTime.Now.TimeOfDay;
                            invoice.InvoiceStatus = true;
                        }

                        var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(invoice.StudentInvoiceID);
                        unitPrice = invoiceDetail.Sum(b => b.UnitPrice);
                        discount = invoiceDetail.Sum(b => b.Discount);
                        tax = invoiceDetail.Sum(b => b.Tax);
                        subTotal = unitPrice - tax - discount;
                        total = invoiceDetail.Sum(b => b.Amount);
                        foreach (var inv in invoiceDetail)
                        {
                            inv.InvoiceSerialNo = invoice.InvoiceSerialNo;
                            _studentInvoiceDetailRepository.UpdateStudentInvoiceDetail(inv);
                        }

                        MoneyToText txt = new MoneyToText();
                        temp.InWriting = txt.ConvertToText(total);

                        temp.StudentSerialNumber = invoice.StudentInvoiceID;
                        temp.ReceiptNo = invoice.InvoiceSerialNo;
                        temp.Fee01 = discount;
                        temp.Fee02 = subTotal;
                        temp.Fee03 = tax;
                        temp.Fee04 = total;
                        temp.StudentName = null;

                        if (item.ClassroomID > 0)
                        {
                            classroomName = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                        }

                        temp.StudentName = item.FirstName + " " + item.LastName + " " + classroomName;

                        count++;
                        _pSerialNumberRepository.UpdatePSerialNumber(serialname);
                        _tempM101Repository.CreateTempM101(temp);

                        studentID = 0;

                        //var invoiceCount = _studentInvoiceRepository.GetStudentInvoiceAllFalse(user.UserPeriod, user.SchoolID, invoiceDate);

                        var invoicex = _studentInvoiceRepository.GetStudentInvoiceAllFalse(user.UserPeriod, user.SchoolID, invoiceDate);
                        var invoiceCount = students.Where(s => invoicex.Where(p => s.ClassroomID == classroomID && p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();


                        if (invoiceCount.Count() > 0) iniPrinted = invoiceCount.Count();
                        iniRemain = iniPrinted;

                        ini.IniWriteValue("Invoice", "RemainInvoice", iniRemain.ToString());

                        if (school.EIIsActive == true)
                        {
                            Invoices = string.Empty;
                            XmlCreateLoop(user, item, studentInvoiceAddress, invoice, school);
                            AddCsvNewRow();
                        }

                        invoice.IsBatchPrint = true;
                        _studentInvoiceRepository.UpdateStudentInvoice(invoice);
                    }
                    if (printOpt == 1 && count == 1)
                    {
                        isLoop = false;
                        break;
                    }

                    if (printOpt == 2 && count == maxPrint)
                    {
                        isLoop = false;
                        break;
                    }
                }
            }

            if (printOpt == 2 && count < maxPrint + 1) isContinue = false;
            if (printOpt == 2 && count == maxPrint) isContinue = true;
            if (printOpt == 3) isContinue = false;

            if (isContinue == false) isLoop = false;

            if (school.EIIsActive == false)
            {
                string reportName = "Invoices" + user.SelectedSchoolCode + "-" + user.SchoolID;
                url = "/reporting/index/" + reportName + "/" + "0" + "/" + "6" + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&studentID=0" + "&period=" + '"' + user.UserPeriod + '"' + "&studentInvoiceID=0" + "&cs=" + '"' + cs + '"';
            }
            else
            {
                EInvoiceSendingLoop(userID);
            }
            ViewBag.DateString = dateString;
            ViewBag.IsLoop = isLoop;

            return Redirect(url);
        }


        [Route("M400Invoice/InstallmentDataRead/{userID}/{studentid}")]
        public IActionResult InstallmentDataRead(int userID, int studentid)
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

                    string categoryName = "";
                    string status = "";
                    if (user.SelectedCulture.Trim() == "en-US")
                    {
                        if (item.CategoryID > 0)
                            categoryName = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID).Language1;
                        if (item.StatusCategoryID > 0)
                            status = _parameterRepository.GetParameter(item.StatusCategoryID).Language1;
                    }
                    else
                    {
                        if (item.CategoryID > 0)
                            categoryName = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID).CategoryName;
                        if (item.StatusCategoryID > 0)
                            status = _parameterRepository.GetParameter(item.StatusCategoryID).CategoryName;
                    }

                    var studentInstallmentViewModel = new StudentInstallmentViewModel
                    {
                        StudentInstallmentID = item.StudentInstallmentID,
                        SchoolID = item.SchoolID,
                        StudentID = item.StudentID,
                        InstallmentDate = item.InstallmentDate,
                        InstallmentNo = item.InstallmentNo,
                        Category = categoryName,

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

        public IActionResult InvoiceDataCreate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoice>>(strResult);
            List<StudentInvoice> list = new List<StudentInvoice>();

            var i = 0;
            foreach (var item in json)
            {
                var invoice = new StudentInvoice();
                invoice.StudentInvoiceID = 0;
                invoice.SchoolID = json[i].SchoolID;
                invoice.StudentID = json[i].StudentID;
                invoice.Period = json[i].Period;
                invoice.InvoiceSerialNo = json[i].InvoiceSerialNo;
                invoice.InvoiceDate = json[i].InvoiceDate;
                invoice.InvoiceCutDate = json[i].InvoiceCutDate;
                invoice.InvoiceCutTime = json[i].InvoiceCutTime;
                invoice.DUnitPrice = json[i].DUnitPrice;
                invoice.DQuantity = json[i].DQuantity;
                invoice.DDiscount = json[i].DDiscount;
                invoice.DTax = json[i].DTax;
                invoice.DTaxPercent = json[i].DTaxPercent;
                invoice.DAmount = json[i].DAmount;

                invoice.WithholdingPercent1 = json[i].WithholdingPercent1;
                invoice.WithholdingPercent2 = json[i].WithholdingPercent2;
                invoice.WithholdingCode = json[i].WithholdingCode;
                invoice.WithholdingExplanation = json[i].WithholdingExplanation;
                invoice.WithholdingTax = json[i].WithholdingTax;
                invoice.WithholdingTotal = json[i].WithholdingTotal;

                invoice.IsPlanned = json[i].IsPlanned;
                invoice.InvoiceType = json[i].InvoiceType;
                invoice.InvoiceStatus = json[i].InvoiceStatus;
                invoice.IsActive = true;

                list.Add(invoice);
                if (ModelState.IsValid)
                {
                    _studentInvoiceRepository.CreateStudentInvoice(invoice);
                }
                i += 1;
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult InvoiceDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoice>>(strResult);

            StudentInvoice invoice = new StudentInvoice();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _studentInvoiceRepository.GetStudentInvoiceID(json[i].Period, json[i].SchoolID, json[i].StudentInvoiceID);

                getCode.StudentInvoiceID = json[i].StudentInvoiceID;
                getCode.InvoiceSerialNo = json[i].InvoiceSerialNo;
                getCode.InvoiceDate = json[i].InvoiceDate;
                getCode.DUnitPrice = json[i].DUnitPrice;
                getCode.DQuantity = json[i].DQuantity;
                getCode.DDiscount = json[i].DDiscount;
                getCode.DTaxPercent = json[i].DTaxPercent;
                getCode.DTax = json[i].DTax;
                getCode.DAmount = json[i].DAmount;

                getCode.IsPlanned = json[i].IsPlanned;
                getCode.InvoiceType = json[i].InvoiceType;
                getCode.InvoiceStatus = json[i].InvoiceStatus;
                if (getCode.InvoiceStatus == false)
                {
                    getCode.IsBatchPrint = false;
                    getCode.InvoiceSerialNo = 0;
                }
                getCode.IsActive = json[i].IsActive;
                if (ModelState.IsValid)
                {
                    _studentInvoiceRepository.UpdateStudentInvoice(getCode);
                }
                i += 1;
            }

            return Json(invoice);
        }

        [HttpPost]
        public IActionResult InvoiceDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoice>>(strResult);
            List<StudentInvoice> list = new List<StudentInvoice>();
            var getCode = _studentInvoiceRepository.GetStudentInvoiceID(json[0].Period, json[0].SchoolID, json[0].StudentInvoiceID);

            getCode.StudentInvoiceID = json[0].StudentInvoiceID;
            if (ModelState.IsValid)
            {
                _studentInvoiceRepository.DeleteStudentInvoice(getCode);
                _studentInvoiceRepository.Save();
            }
            return Json(list);
        }
        #endregion

        #region InvoiceSerial
        [Route("M400Invoice/InvoiceSerialNameCombo/{userID}")]
        public IActionResult InvoiceSerialNameCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var serialname = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);

            var id = 0;
            string name = null;
            List<ComboBoxViewModel> list = new List<ComboBoxViewModel>();
            for (int i = 0; i < 3; i++)
            {
                if (i == 0) { id = i; name = serialname.InvoiceName1; };
                if (i == 1) { id = i; name = serialname.InvoiceName2; };
                if (i == 2) { id = i; name = serialname.InvoiceName3; };

                var comboBoxViewModel = new ComboBoxViewModel
                {
                    Id = id,
                    Name = name,
                };
                list.Add(comboBoxViewModel);
            }

            return Json(list);
        }

        [Route("M400Invoice/GetInvoiceSerialNameCombo/{studentID}/{userID}/{value}")]
        public IActionResult GetInvoiceSerialNameCombo(int studentID, int userID, int value)
        {
            var user = _usersRepository.GetUser(userID);

            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);
            var serialname = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            var serial = 0;

            if (invoiceAddress != null)
            {
                if (invoiceAddress.InvoiceProfile == true)
                {
                    if (value == 0) { serial = serialname.InvoiceSerialNo1; };
                    if (value == 1) { serial = serialname.InvoiceSerialNo2; };
                    if (value == 2) { serial = serialname.InvoiceSerialNo3; };
                }
                else
                {
                    if (value == 0) { serial = serialname.InvoiceSerialNo11; };
                    if (value == 1) { serial = serialname.InvoiceSerialNo22; };
                    if (value == 2) { serial = serialname.InvoiceSerialNo33; };
                }
            }
            else
            {
                if (value == 0) { serial = serialname.InvoiceSerialNo1; };
                if (value == 1) { serial = serialname.InvoiceSerialNo2; };
                if (value == 2) { serial = serialname.InvoiceSerialNo3; };
            }
            return Json(new { serialno = serial });
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
        [Route("M400Invoice/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }
        #endregion

        #region Div1
        [Route("M400Invoice/Div1Update/{userID}/{invoiceName1}/{invoiceName2}/{invoiceName3}/{invoiceSerialNo1}/{invoiceSerialNo11}/{invoiceSerialNo2}/{invoiceSerialNo22}/{invoiceSerialNo3}/{invoiceSerialNo33}")]
        public IActionResult Div1Update(int userID, string invoiceName1, string invoiceName2, string invoiceName3, int invoiceSerialNo1, int invoiceSerialNo11, int invoiceSerialNo2, int invoiceSerialNo22, int invoiceSerialNo3, int invoiceSerialNo33)
        {
            var user = _usersRepository.GetUser(userID);

            var serialname = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            serialname.InvoiceName1 = invoiceName1;
            serialname.InvoiceName2 = invoiceName2;
            serialname.InvoiceName3 = invoiceName3;

            serialname.InvoiceSerialNo1 = invoiceSerialNo1;
            serialname.InvoiceSerialNo11 = invoiceSerialNo11;
            serialname.InvoiceSerialNo2 = invoiceSerialNo2;
            serialname.InvoiceSerialNo22 = invoiceSerialNo22;
            serialname.InvoiceSerialNo3 = invoiceSerialNo3;
            serialname.InvoiceSerialNo33 = invoiceSerialNo33;

            _pSerialNumberRepository.UpdatePSerialNumber(serialname);

            return Json(true);
        }

        #endregion

        #region Result 
        [Route("M400Invoice/GridResultDataRead/{userID}")]
        public IActionResult GridResultDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            string file = "FATGIDEN.CSV";
            string[] readText = null;
            string filePath = Path.Combine(projectDir, file);

            List<EInvoiceViewModel> list = new List<EInvoiceViewModel>();
            if (System.IO.File.Exists(filePath))
            {
                readText = System.IO.File.ReadAllLines(filePath);

                int inx = 0;
                foreach (var csv in readText)
                {
                    string[] words = csv.Split(';');

                    var eInvoiceViewModel = new EInvoiceViewModel();
                    {
                        inx++;
                        eInvoiceViewModel.ID = inx;
                        eInvoiceViewModel.Name = words[3];
                        eInvoiceViewModel.TaxOffice = words[8];
                        eInvoiceViewModel.TaxNumber = words[9];
                        eInvoiceViewModel.InvoiceType = words[2];
                        eInvoiceViewModel.InvoiceNumber = words[12];
                        if (words[13] != null && words[13] != "")
                        {
                            eInvoiceViewModel.Result = Resources.Resource.Successfull;
                            if (words[13] == "0")
                                eInvoiceViewModel.Result = Resources.Resource.Unsuccessfull;

                            eInvoiceViewModel.Explanation = words[15];
                            if (words[15] == "DOKUMAN KUYRUYA EKLENDI")
                                eInvoiceViewModel.Explanation = Resources.Resource.DocumentAddedtoQueue;
                        }
                    }
                    list.Add(eInvoiceViewModel);
                }
            }
            return Json(list);
        }

        #endregion

        #region XmlCreateLoop
        string Invoices = string.Empty;
        public IActionResult XmlCreateLoop(Models.Users user, Student student, StudentInvoiceAddress studentInvoiceAddress, StudentInvoice studentInvoice, SchoolInfo school)
        {
            var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetail(student.StudentID, studentInvoice.StudentInvoiceID);

            var schoolTownName = "";
            var schoolCityName = "";
            var studentTownName = "";
            var studentCityName = "";
            if (school.EITownID != 0)
                schoolTownName = _parameterRepository.GetParameter(school.EITownID).CategoryName;
            if (school.EICityID != 0)
                schoolCityName = _parameterRepository.GetParameter(school.EICityID).CategoryName;
            if (studentInvoiceAddress.InvoiceTownParameterID != 0)
                studentTownName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceTownParameterID).CategoryName;
            if (studentInvoiceAddress.InvoiceCityParameterID != 0)
                studentCityName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceCityParameterID).CategoryName;

            decimal? subTotal = (studentInvoice.DUnitPrice - studentInvoice.DDiscount) - studentInvoice.DTax;

            string invCode = "";
            if (studentInvoiceAddress.InvoiceProfile) invCode = school.EIInvoiceSerialCode1;
            else invCode = school.EIInvoiceSerialCode2;

            string invoiceSerialID = invCode.Substring(0, 3) + DateTime.Now.Year + studentInvoice.InvoiceSerialNo.ToString("000000000");
            //FATGIDEN.CSV
            CsvCreate(user, student, studentInvoice, studentInvoiceAddress, school, invoiceSerialID);

            //XML 10 IF
            decimal unitPrice = Math.Round(Convert.ToDecimal(studentInvoice.DUnitPrice), school.CurrencyDecimalPlaces);
            decimal exclusiveAmount = Math.Round(Convert.ToDecimal(subTotal), school.CurrencyDecimalPlaces);
            decimal inclusiveAmount = unitPrice + Math.Round(Convert.ToDecimal(studentInvoice.DTax), school.CurrencyDecimalPlaces);
            decimal totalDiscount = Math.Round(Convert.ToDecimal(studentInvoice.DDiscount), school.CurrencyDecimalPlaces);
            decimal payableAmount = inclusiveAmount;

            decimal amount = Convert.ToDecimal(payableAmount);
            MoneyToText text = new MoneyToText();
            string inWrite = "YALNIZ:" + text.ConvertToText(amount);

            int row = 1;
            AddInv("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            //XML 02
            AddInv("<Invoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            AddInv("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"");
            AddInv("xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"");
            AddInv("xmlns:qdt=\"urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2\"");
            AddInv("xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"");
            AddInv("xmlns:udt=\"urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2\"");
            AddInv("xmlns:xades=\"http://uri.etsi.org/01903/v1.3.2#\"");
            AddInv("xmlns:ubltr=\"urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents\"");
            AddInv("xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"");
            AddInv("xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"");
            AddInv("xmlns:ccts=\"urn:un:unece:uncefact:documentation:2\"");
            AddInv("xsi:schemaLocation=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 UBL-Invoice-2.1.xsd\"");
            AddInv("xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\">");

            //XML 03
            AddInv("<cbc:UBLVersionID>2.1</cbc:UBLVersionID>");
            AddInv("<cbc:CustomizationID>TR1.2</cbc:CustomizationID>");
            AddInv("<cbc:ProfileID></cbc:ProfileID>");
            AddInv("<cbc:ID>" + invoiceSerialID + "</cbc:ID>");
            AddInv("<cbc:CopyIndicator>false</cbc:CopyIndicator>");
            AddInv("<cbc:UUID></cbc:UUID>");
            AddInv("<cbc:IssueDate>" + string.Format("{0:yyyy-MM-dd}", studentInvoice.InvoiceCutDate) + "</cbc:IssueDate>");
            AddInv("<cbc:IssueTime>" + DateTime.Now.ToString("HH:mm:ss") + "</cbc:IssueTime>");
            if (studentInvoice.WithholdingTotal > 0)
                AddInv("<cbc:InvoiceTypeCode>TEVKIFAT</cbc:InvoiceTypeCode>");
            else AddInv("<cbc:InvoiceTypeCode>SATIS</cbc:InvoiceTypeCode>");
            AddInv("<cbc:Note>" + studentInvoiceAddress.Notes + "</cbc:Note>");
            AddInv("<cbc:Note>" + inWrite + "</cbc:Note>");
            AddInv("<cbc:DocumentCurrencyCode listVersionID=\"2001\" listName=\"Currency\" listID=\"ISO 4217 Alpha\" listAgencyName=\"United Nations Economic Commission for Europe\">" + "TRY" + "</cbc:DocumentCurrencyCode>");
            AddInv("<cbc:LineCountNumeric>" + row + "</cbc:LineCountNumeric>");

            //XML 04 lOGO
            AddInv("<cac:AdditionalDocumentReference>");
            AddInv("<cbc:ID>invoiceSchmetronControl</cbc:ID>");
            AddInv("<cbc:IssueDate>" + string.Format("{0:yyyy-MM-dd}", studentInvoice.InvoiceCutDate) + "</cbc:IssueDate>");
            AddInv("<cac:Attachment>");
            AddInv("<cbc:EmbeddedDocumentBinaryObject characterSetCode=\"UTF-8\" encodingCode=\"Base64\" mimeCode=\"application/xml\" filename=\"efatura.xslt\">" + "</cbc:EmbeddedDocumentBinaryObject>");
            AddInv("</cac:Attachment>");
            AddInv("</cac:AdditionalDocumentReference>");

            //XML 06 
            AddInv("<cac:AccountingSupplierParty>");
            AddInv("<cac:Party>");
            AddInv("<cbc:WebsiteURI>" + school.EIWebAddress + "</cbc:WebsiteURI>");
            AddInv("<cac:PartyIdentification>");
            AddInv("<cbc:ID schemeID=\"VKN\">" + school.EITaxNo + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");
            //XML 06 Trade Register No
            AddInv("<cac:PartyIdentification>");
            AddInv("<cbc:ID schemeID=\"TICARETSICILNO\">" + school.EITaxNo + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");

            //XML Mersis
            AddInv("<cac:PartyIdentification>");
            AddInv("<cbc:ID schemeID=\"MERSISNO\">" + school.EIMersisNo + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");

            //XML Mersis Ex
            AddInv("<cac:PartyName>");
            AddInv("<cbc:Name>" + school.EITitle + "</cbc:Name>");
            AddInv("</cac:PartyName>");
            AddInv("<cac:PostalAddress>");
            AddInv("<cbc:StreetName>" + school.EIAddress + "</cbc:StreetName>");
            AddInv("<cbc:BuildingNumber />");
            AddInv("<cbc:CitySubdivisionName>" + schoolTownName + "</cbc:CitySubdivisionName>");
            AddInv("<cbc:CityName>" + schoolCityName + "</cbc:CityName>");
            AddInv("<cbc:PostalZone />");
            AddInv("<cac:Country>");
            AddInv("<cbc:Name>" + school.EICountry + "</cbc:Name>");
            AddInv("</cac:Country>");
            AddInv("</cac:PostalAddress>");
            AddInv("<cac:PartyTaxScheme>");
            AddInv("<cac:TaxScheme>");
            AddInv("<cbc:Name>" + school.EITaxOffice + "</cbc:Name>");
            AddInv("</cac:TaxScheme>");
            AddInv("</cac:PartyTaxScheme>");
            AddInv("<cac:Contact>");

            //XML Phone
            AddInv("<cbc:Telephone>" + school.EIPhone + "</cbc:Telephone>");
            //XML Fax
            AddInv("<cbc:Telefax>" + school.EIFax + "</cbc:Telefax>");
            //XML Email
            AddInv("<cbc:ElectronicMail>" + school.EIEMail + "</cbc:ElectronicMail>");
            //XML Email Ex
            AddInv("</cac:Contact>");
            AddInv("</cac:Party>");
            AddInv("</cac:AccountingSupplierParty>");
            //XML 06 END

            //XML 07
            AddInv("<cac:AccountingCustomerParty>");
            AddInv("<cac:Party>");
            AddInv("<cac:PartyIdentification>");
            if (studentInvoiceAddress.InvoiceTaxOffice != null)
                AddInv("<cbc:ID schemeID=\"VKN\">" + studentInvoiceAddress.InvoiceTaxNumber + "</cbc:ID>");
            else
                if (student != null) AddInv("<cbc:ID schemeID=\"TCKN\">" + student.IdNumber + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");

            //XML Company
            if (studentInvoiceAddress.InvoiceTaxOffice != null)
            {
                AddInv("<cac:PartyName>");
                AddInv("<cbc:Name>" + studentInvoiceAddress.InvoiceTitle + "</cbc:Name>");
                AddInv("</cac:PartyName>");
            }

            AddInv("<cac:PostalAddress>");
            AddInv("<cbc:StreetName>" + studentInvoiceAddress.InvoiceAddress + "</cbc:StreetName>");
            AddInv("<cbc:BuildingNumber />");
            AddInv("<cbc:CitySubdivisionName>" + studentTownName + "</cbc:CitySubdivisionName>");
            AddInv("<cbc:CityName>" + studentCityName + "</cbc:CityName>");
            AddInv("<cbc:PostalZone />");
            AddInv("<cac:Country>");
            AddInv("<cbc:Name>" + studentInvoiceAddress.InvoiceCountry + "</cbc:Name>");
            AddInv("</cac:Country>");
            AddInv("</cac:PostalAddress>");
            AddInv("<cac:PartyTaxScheme>");
            AddInv("<cac:TaxScheme>");
            AddInv("<cbc:Name>" + studentInvoiceAddress.InvoiceTaxOffice + "</cbc:Name>");
            AddInv("</cac:TaxScheme>");
            AddInv("</cac:PartyTaxScheme>");
            AddInv("<cac:Contact>");
            AddInv("<cbc:ElectronicMail>" + studentInvoiceAddress.EMail + "</cbc:ElectronicMail>");
            AddInv("</cac:Contact>");

            //PERSON XML
            if (studentInvoiceAddress.InvoiceTaxOffice == null)
            {
                string title = studentInvoiceAddress.InvoiceTitle + " " + " " + " ";
                string[] name = title.Split(' ');
                string firstName = "";
                string lastName = "";
                if (name[2] == null || name[2] == "")
                {
                    firstName = name[0];
                    lastName = name[1];
                }
                else
                {
                    firstName = name[0] + " " + name[1];
                    lastName = name[2];
                }

                AddInv("<cac:Person>");
                AddInv("<cbc:FirstName>" + firstName + "</cbc:FirstName>");
                AddInv("<cbc:FamilyName>" + lastName + "</cbc:FamilyName>");
                AddInv("</cac:Person>");
            }

            AddInv("</cac:Party>");
            AddInv("</cac:AccountingCustomerParty>");
            //PERSON 07 ex

            //XML IBAN
            AddInv("<cac:PaymentMeans>");
            AddInv("<cbc:PaymentMeansCode>ZZZ</cbc:PaymentMeansCode>");
            AddInv("<cac:PayeeFinancialAccount>");
            AddInv("<cbc:ID>" + school.EIIban + "</cbc:ID>");
            AddInv("<cbc:CurrencyCode>\"TRY\"</cbc:CurrencyCode>");
            AddInv("<cbc:PaymentNote />");
            AddInv("</cac:PayeeFinancialAccount>");
            AddInv("</cac:PaymentMeans>");

            //XML 08
            if (studentInvoice.DDiscount > 0)
            {
                decimal discount = Math.Round(Convert.ToDecimal(studentInvoice.DDiscount), school.CurrencyDecimalPlaces);

                string discountStr = discount.ToString().Replace(',', '.');
                AddInv("<cac:AllowanceCharge>");
                AddInv("<cbc:ChargeIndicator>true</cbc:ChargeIndicator>");
                AddInv("<cbc:Amount currencyID=\"TRY\">" + discountStr + "</cbc:Amount>");
                AddInv("<cbc:BaseAmount currencyID=\"TRY\">" + discountStr + "</cbc:BaseAmount>");
                AddInv("</cac:AllowanceCharge>");
            }

            //XML 09
            AddInv("<cac:TaxTotal>");
            decimal tax = Math.Round(Convert.ToDecimal(studentInvoice.DTax), school.CurrencyDecimalPlaces);
            decimal sTotal = Math.Round(Convert.ToDecimal(subTotal), school.CurrencyDecimalPlaces);

            string taxStr = tax.ToString().Replace(',', '.');
            string sTotalStr = sTotal.ToString().Replace(',', '.');
            string dTaxPercentStr = studentInvoice.DTaxPercent.ToString().Replace(',', '.');

            AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + sTotalStr + "</cbc:TaxAmount>");
            AddInv("<cac:TaxSubtotal>");
            AddInv("<cbc:TaxableAmount currencyID=\"TRY\">" + sTotalStr + "</cbc:TaxableAmount>");
            AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + taxStr + "</cbc:TaxAmount>");
            AddInv("<cbc:CalculationSequenceNumeric>" + "1" + "</cbc:CalculationSequenceNumeric>");
            AddInv("<cbc:Percent>" + dTaxPercentStr + "</cbc:Percent>");
            AddInv("<cac:TaxCategory>");
            AddInv("<cac:TaxScheme>");
            AddInv("<cbc:Name>KDV</cbc:Name>");
            AddInv("<cbc:TaxTypeCode>0015</cbc:TaxTypeCode>");
            AddInv("</cac:TaxScheme>");
            AddInv("</cac:TaxCategory>");
            AddInv("</cac:TaxSubtotal>");
            AddInv("</cac:TaxTotal>");

            //XML Withholding (TEVKİFAT)
            if (studentInvoice.WithholdingTax > 0)
            {

                string withholdingTaxStr = studentInvoice.WithholdingTax.ToString().Replace(',', '.');

                AddInv("<cac:WithholdingTaxTotal>");
                AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + withholdingTaxStr + "</cbc:TaxAmount>");
                AddInv("<cac:TaxSubtotal>");
                AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + withholdingTaxStr + "</cbc:TaxAmount>");
                AddInv("<cbc:CalculationSequenceNumeric>" + "2" + "</cbc:CalculationSequenceNumeric>");
                AddInv("<cbc:Percent>" + "50" + "</cbc:Percent>");
                AddInv("<cac:TaxCategory>");
                AddInv("<cac:TaxScheme>");
                AddInv("<cbc:Name>" + studentInvoice.WithholdingExplanation + "</cbc:Name>");
                AddInv("<cbc:TaxTypeCode>604</cbc:TaxTypeCode>");
                AddInv("</cac:TaxScheme>");
                AddInv("/cac:TaxCategory>");
                AddInv("</cac:TaxSubtotal>");
                AddInv("</cac:WithholdingTaxTotal>");
            }

            string unitPriceStr = unitPrice.ToString().Replace(',', '.');
            string exclusiveAmountStr = exclusiveAmount.ToString().Replace(',', '.');
            string inclusiveAmountStr = inclusiveAmount.ToString().Replace(',', '.');
            string totalDiscountStr = totalDiscount.ToString().Replace(',', '.');
            string payableAmountStr = payableAmount.ToString().Replace(',', '.');

            AddInv("<cac:LegalMonetaryTotal>");
            AddInv("<cbc:LineExtensionAmount currencyID=\"TRY\">" + unitPriceStr + "</cbc:LineExtensionAmount>");
            AddInv("<cbc:TaxExclusiveAmount currencyID=\"TRY\">" + exclusiveAmountStr + "</cbc:TaxExclusiveAmount>");
            AddInv("<cbc:TaxInclusiveAmount currencyID=\"TRY\">" + inclusiveAmountStr + "</cbc:TaxInclusiveAmount>");
            AddInv("<cbc:AllowanceTotalAmount currencyID=\"TRY\">" + totalDiscountStr + "</cbc:AllowanceTotalAmount>");
            AddInv("<cbc:PayableAmount currencyID=\"TRY\">" + payableAmountStr + "</cbc:PayableAmount>");
            AddInv("</cac:LegalMonetaryTotal>");

            //XML 11
            foreach (var item in invoiceDetail)
            {
                if (item.Amount > 0)
                {
                    decimal amount2 = Math.Round(Convert.ToDecimal(item.Amount), school.CurrencyDecimalPlaces);
                    decimal tax2 = Math.Round(Convert.ToDecimal(item.Tax), school.CurrencyDecimalPlaces);
                    row++;
                    AddInv("<cac:InvoiceLine>");
                    AddInv("<cbc:ID>" + row + "</cbc:ID>");
                    AddInv("<cbc:Note />");
                    AddInv("<cbc:InvoicedQuantity unitCode=\"C62\">" + item.Quantity + "</cbc:InvoicedQuantity>");

                    string amount2Str = amount2.ToString().Replace(',', '.');
                    AddInv("<cbc:LineExtensionAmount currencyID=\"TRY\">" + amount2Str + "</cbc:LineExtensionAmount>");
                    AddInv("<cac:TaxTotal>");
                    AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + "0.00" + "</cbc:TaxAmount>");
                    AddInv("<cac:TaxSubtotal>");

                    string tax2Str = tax2.ToString().Replace(',', '.');
                    string taxPercentStr = item.TaxPercent.ToString().Replace(',', '.');

                    AddInv("<cbc:TaxableAmount currencyID=\"TRY\">" + amount2Str + "</cbc:TaxableAmount>");
                    AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + tax2Str + "</cbc:TaxAmount>");
                    AddInv("<cbc:Percent>" + taxPercentStr + "</cbc:Percent>");
                    AddInv("<cac:TaxCategory>");

                    if (item.TaxPercent == 0)
                    {
                        AddInv("<cbc:TaxExemptionReason>" + "Kdv.Kanununun 13.M bendi gereği istisna uygulanmıştır" + "</cbc:TaxExemptionReason>");
                        AddInv("<cbc:TaxExemptionReasonCode>" + "350" + "</cbc:TaxExemptionReasonCode>");
                    }

                    AddInv("<cac:TaxScheme>");
                    AddInv("<cbc:Name>" + "KDV" + "</cbc:Name>");
                    AddInv("<cbc:TaxTypeCode>" + "0015" + "</cbc:TaxTypeCode>");
                    AddInv("</cac:TaxScheme>");
                    AddInv("</cac:TaxCategory>");
                    AddInv("</cac:TaxSubtotal>");

                    //XML Withholding 
                    if (studentInvoice.WithholdingTax > 0)
                    {
                        decimal tax3 = Math.Round(Convert.ToDecimal(studentInvoice.DTax), school.CurrencyDecimalPlaces);
                        decimal tax33 = Math.Round(Convert.ToDecimal(studentInvoice.WithholdingTax), school.CurrencyDecimalPlaces);

                        string tax3Str = tax3.ToString().Replace(',', '.');
                        string tax33Str = tax33.ToString().Replace(',', '.');
                        int percent = (int)studentInvoice.WithholdingPercent1 / (int)studentInvoice.WithholdingPercent2;
                        string percentStr = tax33.ToString().Replace(',', '.');

                        AddInv("<cac:TaxSubtotal>");
                        AddInv("<cbc:TaxableAmount currencyID=\"TRY\">" + tax3Str + "</cbc:TaxableAmount>");
                        AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + tax33Str + "</cbc:TaxAmount>");
                        AddInv("<cbc:CalculationSequenceNumeric>" + "2" + "</cbc:CalculationSequenceNumeric>");
                        AddInv("<cbc:Percent>" + percentStr + "</cbc:Percent>");
                        AddInv("<cac:TaxCategory>");
                        AddInv("<cac:TaxScheme>");
                        AddInv("<cbc:Name>KDV TEVKİFAT</cbc:Name>");
                        AddInv("<cbc:TaxTypeCode>9015</cbc:TaxTypeCode>");
                        AddInv("</cac:TaxScheme");
                        AddInv("</cac:TaxCategory>");
                        AddInv("</cac:TaxSubtotal");
                    }
                    AddInv("</cac:TaxTotal>");

                    //XML 12
                    decimal unitPrice2 = Math.Round(Convert.ToDecimal(item.UnitPrice), school.CurrencyDecimalPlaces);

                    AddInv("<cac:Item>");
                    AddInv("<cbc:Description />");
                    AddInv("<cbc:Name>" + item.Explanation + "</cbc:Name>");
                    AddInv("</cac:Item>");
                    AddInv("<cac:Price>");

                    string unitPrice2Str = unitPrice2.ToString().Replace(',', '.');
                    AddInv("<cbc:PriceAmount currencyID=\"TRY\">" + unitPrice2Str + "</cbc:PriceAmount>");
                    AddInv("</cac:Price>");
                    AddInv("</cac:InvoiceLine>");
                }
            }
            //XML 13
            AddInv("</Invoice>");

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            if (!Directory.Exists(projectDir))
            {
                Directory.CreateDirectory(projectDir);
            }
            string file = "FAT" + invoiceSerialID + ".xml";
            string filePath = Path.Combine(projectDir, file);

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        w.WriteLine(Invoices);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception) { }
            var url = "";

            if (student != null)
                url = "/M400Invoice/InvoicePlan?userID=" + user.UserID + "&studentID=" + student.StudentID;
            else url = "/M400Invoice/InvoiceManually?userID=" + user.UserID + "&studentID=0";
            return Redirect(url);
        }

        private void AddInv(string InvInfo)
        {
            Invoices += InvInfo + "\n";
        }

        string CsvInvoices = string.Empty;
        public void CsvCreate(Models.Users user, Student student, StudentInvoice studentInvoice, StudentInvoiceAddress studentInvoiceAddress, SchoolInfo school, string invoiceSerialID)
        {
            string profile = "TEMELFATURA";
            if (studentInvoiceAddress.InvoiceTypeParameter == 1) profile = "TICARIFATURA";
            if (studentInvoiceAddress.InvoiceTypeParameter == 2) profile = "ISTISNAFATURASI";
            if (studentInvoiceAddress.InvoiceTypeParameter == 3) profile = "SATISFATURASI";

            var studentTownName = "";
            var studentCityName = "";
            if (studentInvoiceAddress.InvoiceTownParameterID != 0)
                studentTownName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceTownParameterID).CategoryName;
            if (studentInvoiceAddress.InvoiceCityParameterID != 0)
                studentCityName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceCityParameterID).CategoryName;

            if (school.EIUserName == null) school.EIUserName = "TESTER@VRBN";
            if (school.EIUserPassword == null) school.EIUserPassword = "Vtest*2020*";

            AddCsv(school.EIUserName);
            AddCsv(school.EIUserPassword);
            AddCsv(profile);
            AddCsv(studentInvoiceAddress.InvoiceTitle);
            AddCsv(studentInvoiceAddress.InvoiceAddress);
            AddCsv(studentTownName);
            AddCsv(studentCityName);
            AddCsv(studentInvoiceAddress.InvoiceCountry);
            AddCsv(studentInvoiceAddress.InvoiceTaxOffice);
            AddCsv(studentInvoiceAddress.InvoiceTaxNumber);
            AddCsv(string.Format("{0:yyyyMMdd}", studentInvoice.InvoiceCutDate));
            AddCsv(studentInvoiceAddress.EMail);
            AddCsv(invoiceSerialID);

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", user.SelectedSchoolCode.ToString());
            string file = "FATGIDEN.CSV";
            string filePath = Path.Combine(projectDir, file);

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        w.WriteLine(CsvInvoices);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception) { }
        }

        private void AddCsv(string CsvInfo)
        {
            CsvInvoices += CsvInfo + ";";
        }
        private void AddCsvNewRow()
        {
            CsvInvoices += "\n";
        }

        #endregion

        #region EInvoiceSendingLoop
        public void EInvoiceSendingLoop(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string entegrator = _parameterRepository.GetParameter(school.EIIntegratorNameID).CategoryName;
            var schoolCode = user.SelectedSchoolCode;

            EInvoiceExecuteCommand(entegrator, schoolCode);
        }
        public void EInvoiceExecuteCommand(string entegrator, int schoolCode)
        {
            try
            {
                string exepath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES");
                string dataPath = exepath + "\\" + schoolCode.ToString();
                Directory.SetCurrentDirectory(exepath);

                NCSEFatura.NCSEFatura ef = new NCSEFatura.NCSEFatura();
                ef.Execute(entegrator, dataPath);
            }

            catch (Exception) { }
        }
        #endregion

        #region InvoiceCurrentCard
        [HttpPost]
        public async Task<IActionResult> InvoiceCurrentCard(StudentInvoiceAddress studentInvoiceAddress, InvoiceViewModel invoiceViewModel)
        {
            await Task.Delay(200);
            var user = _usersRepository.GetUser(invoiceViewModel.UserID);
            if (!studentInvoiceAddress.isEmpty)
            {
                studentInvoiceAddress.StudentID = invoiceViewModel.StudentID;
                if (studentInvoiceAddress.StudentInvoiceAddressID == 0)
                {
                    _studentInvoiceAddressRepository.CreateStudentInvoiceAddress(studentInvoiceAddress);
                }
                else
                {
                    _studentInvoiceAddressRepository.UpdateStudentInvoiceAddress(studentInvoiceAddress);
                }
            }

            var tempM101 = _tempM101Repository.GetTempM101All(invoiceViewModel.SchoolID, invoiceViewModel.UserID);
            foreach (var item in tempM101)
            {
                _tempM101Repository.DeleteTempM101(item);
            }

            var temp = new TempM101();
            temp.ID = 0;
            temp.UserID = invoiceViewModel.UserID;
            temp.SchoolID = invoiceViewModel.SchoolID;
            temp.StudentID = invoiceViewModel.StudentID;

            decimal unitPrice = 0;
            decimal discount = 0;
            decimal subTotal = 0;
            decimal tax = 0;
            decimal total = 0;

            var inv = _studentInvoiceRepository.GetStudentInvoiceSerialNo(invoiceViewModel.Period, invoiceViewModel.SchoolID, invoiceViewModel.InvoiceSerialNo);
            if (invoiceViewModel.StudentInvoiceID == 0 && inv != null)
            {
                invoiceViewModel.StudentInvoiceID = inv.StudentInvoiceID;
                inv.StudentInvoiceAddressID = studentInvoiceAddress.StudentInvoiceAddressID;
                _studentInvoiceRepository.UpdateStudentInvoice(inv);
            }

            var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceID(invoiceViewModel.StudentInvoiceID);

            unitPrice = invoiceDetail.Sum(b => b.UnitPrice);
            discount = invoiceDetail.Sum(b => b.Discount);
            tax = invoiceDetail.Sum(b => b.Tax);
            subTotal = unitPrice - tax - discount;
            total = invoiceDetail.Sum(b => b.Amount);
            foreach (var item in invoiceDetail)
            {
                item.InvoiceSerialNo = invoiceViewModel.InvoiceSerialNo;
                item.InvoiceStatus = true;
                _studentInvoiceDetailRepository.UpdateStudentInvoiceDetail(item);
            }

            MoneyToText txt = new MoneyToText();
            temp.InWriting = txt.ConvertToText(total);

            temp.Fee01 = discount;
            temp.Fee02 = subTotal;
            temp.Fee03 = tax;
            temp.Fee04 = total;
            temp.StudentName = null;
            temp.ReceiptNo = invoiceViewModel.InvoiceSerialNo;
            string classroomName = "";
            if (invoiceViewModel.StudentID > 0)
            {
                var student = _studentRepository.GetStudent(invoiceViewModel.StudentID);

                if (student.ClassroomID > 0)
                {
                    classroomName = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomName;
                }

                temp.StudentName = student.FirstName + " " + student.LastName + " " + classroomName;
            }

            _tempM101Repository.CreateTempM101(temp);

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            TempData["studentInvoiceAddressID"] = studentInvoiceAddress.StudentInvoiceAddressID;
            return Json(new { studentInvoiceAddressID = studentInvoiceAddress.StudentInvoiceAddressID, studentInvoiceID = invoiceViewModel.StudentInvoiceID });
        }


        [Route("M400Invoice/InvoiceProgramDataRead/{userID}/{studentInvoiceID}/{accountingID}")]
        public IActionResult InvoiceProgramDataRead(int userID, int studentInvoiceID, int accountingID)
        {
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            byte tp = (byte)_schoolFeeRepository.GetSchoolFeeSelect(school.SchoolID, "L1").Where(b => b.Tax > 0).LastOrDefault().Tax;
            var account = _accountingRepository.GetAccountingID(accountingID);

            var studentInvoiceDetail = new StudentInvoiceDetail();
            studentInvoiceDetail.StudentInvoiceDetailID = 0;
            studentInvoiceDetail.StudentInvoiceID = 0;
            studentInvoiceDetail.SchoolID = user.SchoolID;
            studentInvoiceDetail.StudentID = 0;
            studentInvoiceDetail.SchoolFeeID = 0;
            studentInvoiceDetail.Period = user.UserPeriod;
            studentInvoiceDetail.InvoiceSerialNo = 0;
            studentInvoiceDetail.Explanation = account.Explanation;
            decimal unitPrice = 0;
            if (account.Debt > 0) unitPrice = (decimal)account.Debt;
            else unitPrice = (decimal)account.Credit;
            studentInvoiceDetail.UnitPrice = unitPrice;
            decimal total = unitPrice;

            studentInvoiceDetail.Quantity = 1;
            studentInvoiceDetail.Discount = 0;
            studentInvoiceDetail.Amount = total;

            double taxPercent = (1 + (Convert.ToDouble(tp) / 100));
            decimal tax1 = Math.Round(total - (total / Convert.ToDecimal(taxPercent)), school.CurrencyDecimalPlaces);
            studentInvoiceDetail.TaxPercent = tp;
            studentInvoiceDetail.Tax = tax1;

            studentInvoiceDetail.InvoiceStatus = true;

            var invoice = new StudentInvoice();
            invoice.StudentInvoiceID = 0;
            invoice.SchoolID = user.SchoolID;
            invoice.StudentID = 0;
            invoice.StudentInvoiceAddressID = 0;
            invoice.Period = user.UserPeriod;
            invoice.InvoiceSerialNo = 0;
            invoice.InvoiceDate = DateTime.Now;
            invoice.InvoiceCutDate = null;
            invoice.DExplanation = null;
            invoice.DUnitPrice = unitPrice;
            invoice.DQuantity = 1;
            invoice.DDiscount = 0;
            invoice.DTaxPercent = (byte)taxPercent;
            invoice.DTax = tax1;
            invoice.DAmount = total;
            invoice.WithholdingPercent1 = 5;
            invoice.WithholdingPercent2 = 10;
            invoice.WithholdingCode = "0650";
            invoice.WithholdingExplanation = "";
            invoice.WithholdingTax = 0;
            invoice.WithholdingTotal = 0;
            invoice.IsPlanned = false;
            invoice.InvoiceType = true;
            invoice.IsActive = true;
            invoice.IsBatchPrint = false;

            var invoiceViewModel = new InvoiceViewModel();
            {
                //invoiceViewModel.StudentInvoiceAddress = studentInvoiceAddress;

                invoiceViewModel.ViewModelID = invoice.StudentID;
                invoiceViewModel.StudentID = invoice.StudentID;
                invoiceViewModel.Period = user.UserPeriod;
                invoiceViewModel.SchoolID = user.SchoolID;

                invoiceViewModel.InvoiceSerialNo = invoice.InvoiceSerialNo;
                invoiceViewModel.InvoiceDate = invoice.InvoiceDate;

                invoiceViewModel.StudentInvoiceID = studentInvoiceDetail.StudentInvoiceID;
                invoiceViewModel.StudentInvoiceDetailID = studentInvoiceDetail.StudentInvoiceDetailID;
                invoiceViewModel.Explanation = studentInvoiceDetail.Explanation;
                invoiceViewModel.Quantity = studentInvoiceDetail.Quantity;
                invoiceViewModel.UnitPrice = studentInvoiceDetail.UnitPrice;
                invoiceViewModel.Discount = studentInvoiceDetail.Discount;
                invoiceViewModel.TaxPercent = studentInvoiceDetail.TaxPercent;
                invoiceViewModel.Tax = studentInvoiceDetail.Tax;
                invoiceViewModel.Amount = studentInvoiceDetail.Amount;

                invoiceViewModel.WithholdingPercent1 = (int)invoice.WithholdingPercent1;
                invoiceViewModel.WithholdingPercent2 = (int)invoice.WithholdingPercent2;
                invoiceViewModel.WithholdingExplanation = invoice.WithholdingExplanation;
                invoiceViewModel.WithholdingTax = invoice.WithholdingTax;
                invoiceViewModel.WithholdingTotal = invoice.WithholdingTotal;
                invoiceViewModel.DirtyField = "0";

                list.Add(invoiceViewModel);
            }

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }


        [Route("M400Invoice/InvoiceProgramCurrentCardDataUpdate/{strResult}/{title}/{taxNo}/{invoiceSerialNumber}/{dateString}")]
        public async Task<IActionResult> InvoiceProgramCurrentCardDataUpdate([Bind(Prefix = "models")] string strResult, string title, string taxNo, int invoiceSerialNumber, string dateString)
        {
            await Task.Delay(100);
            DateTime invoiceDate = DateTime.Parse(dateString);

            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressTitle(title, taxNo);

            var json = new JavaScriptSerializer().Deserialize<List<StudentInvoiceDetail>>(strResult);

            if (invoiceAddress == null)
                invoiceAddress = new StudentInvoiceAddress();

            var i = 0;
            var invoice = new StudentInvoice();
            invoice.StudentInvoiceID = 0;
            invoice.StudentInvoiceAddressID = invoiceAddress.StudentInvoiceAddressID;
            invoice.SchoolID = json[i].SchoolID;
            invoice.StudentID = json[i].StudentID;
            invoice.Period = json[i].Period;
            invoice.InvoiceSerialNo = invoiceSerialNumber;
            invoice.InvoiceDate = invoiceDate;
            invoice.InvoiceCutDate = DateTime.Now;
            invoice.InvoiceCutTime = DateTime.Now.TimeOfDay;

            invoice.DUnitPrice = json[i].UnitPrice;
            invoice.DQuantity = json[i].Quantity;
            invoice.DDiscount = json[i].Discount;
            invoice.DTax = json[i].Tax;
            invoice.DTaxPercent = json[i].TaxPercent;
            invoice.DAmount = json[i].Amount;

            invoice.WithholdingPercent1 = 5;
            invoice.WithholdingPercent2 = 10;
            invoice.WithholdingCode = "";
            invoice.WithholdingExplanation = "";
            invoice.WithholdingTax = 0;
            invoice.WithholdingTotal = 0;

            invoice.IsPlanned = false;
            invoice.InvoiceType = true;
            invoice.InvoiceStatus = true;
            invoice.IsActive = true;

            if (ModelState.IsValid)
            {
                _studentInvoiceRepository.CreateStudentInvoice(invoice);
            }

            i = 0;
            foreach (var item in json)
            {
                var detail = new StudentInvoiceDetail();

                detail.StudentInvoiceDetailID = 0;
                detail.StudentInvoiceID = invoice.StudentInvoiceID;
                detail.SchoolID = json[i].SchoolID;
                detail.SchoolFeeID = json[i].SchoolFeeID;
                detail.StudentID = json[i].StudentID;
                detail.Period = json[i].Period;
                detail.InvoiceSerialNo = invoiceSerialNumber;
                detail.Explanation = json[i].Explanation;
                detail.Quantity = 1;
                detail.InvoiceStatus = false;
                detail.UnitPrice = json[i].UnitPrice;
                detail.Amount = json[i].Amount;
                detail.Discount = 0;
                detail.TaxPercent = json[i].TaxPercent;
                detail.Tax = json[i].Tax;

                if (ModelState.IsValid)
                {
                    _studentInvoiceDetailRepository.CreateStudentInvoiceDetail(detail);
                }
                i += 1;
            }

            //if (i == 0)
            //{ 
            //string url = "/Home/Index?userID=" + 2 + "&msg=0";
            //return Redirect(url);
            //}
            //else
            ////return Json(new { redirectToUrl = Url.Action("Index", "Home") });

            ////return RedirectToAction("Index", "Home");

            //return RedirectToAction("Index", "Home", new { userID = 2, msg = 0 });

            return Json(true);
        }

        #endregion
    }
}
