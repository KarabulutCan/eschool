using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;

using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Helpers;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESCHOOL.Controllers
{
    public class M210StudentController : Controller
    {
        ICustomersRepository _customersRepository;
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentDebtDetailRepository _studentDebtDetailRepository;
        IStudentDebtDetailTableRepository _studentDebtDetailTableRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentInstallmentPaymentRepository _studentInstallmentPaymentRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IStudentNoteRepository _studentNoteRepository;
        IStudentTempRepository _studentTempRepository;
        ITempDataRepository _tempDataRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInvoiceDetailRepository _studentInvoiceDetailRepository;
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
        IUsersLogRepository _usersLogRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IStudentTaskDataSourceRepository _studentTaskDataSourceRepository;

        IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public M210StudentController(
             ICustomersRepository customersRepository,
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDebtDetailRepository studentDebtDetailRepository,
            IStudentDebtDetailTableRepository studentDebtDetailTableRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            IStudentInstallmentPaymentRepository studentInstallmentPaymentRepository,
            IStudentPaymentRepository studentPaymentRepository,
            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,
            IStudentNoteRepository studentNoteRepository,
            IStudentTempRepository studentTempRepository,
            ITempDataRepository tempDataRepository,
            IStudentInvoiceRepository studentInvoiceRepository,
            IStudentInvoiceDetailRepository studentInvoiceDetailRepository,
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
            IUsersLogRepository usersLogRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            IStudentTaskDataSourceRepository studentTaskDataSourceRepository,
            IWebHostEnvironment hostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _customersRepository = customersRepository;
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentDebtDetailRepository = studentDebtDetailRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _studentDebtDetailTableRepository = studentDebtDetailTableRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _studentInstallmentPaymentRepository = studentInstallmentPaymentRepository;
            _discountTableRepository = discountTableRepository;
            _studentDiscountRepository = studentDiscountRepository;
            _studentPaymentRepository = studentPaymentRepository;
            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _studentNoteRepository = studentNoteRepository;
            _studentTempRepository = studentTempRepository;
            _tempDataRepository = tempDataRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInvoiceDetailRepository = studentInvoiceDetailRepository;
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
            _usersLogRepository = usersLogRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _studentTaskDataSourceRepository = studentTaskDataSourceRepository;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Index_Students
        public IActionResult index(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Öğrenci veya Tahsilat İptali").CategoryID;
            bool isPermissionCancel = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

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

            var studentIndexViewModel = new StudentIndexViewModel
            {
                UserID = userID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                SchoolName = schoolName,
                SelectedCulture = user.SelectedCulture.Trim(),
                IsPermissionCancel = isPermissionCancel,
            };

            //Grid Title
            var inx = 0;
            ViewBag.Title01 = "."; ViewBag.Title02 = "."; ViewBag.Title03 = "."; ViewBag.Title04 = "."; ViewBag.Title05 = ".";
            ViewBag.Title06 = "."; ViewBag.Title07 = "."; ViewBag.Title08 = "."; ViewBag.Title09 = "."; ViewBag.Title10 = ".";
            var fee = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1").OrderBy(a => a.SortOrder);
            foreach (var item in fee)
            {
                string demoText = item.Name;
                if (user.SelectedCulture.Trim() == "en-US" && item.Language1 != null) demoText = item.Language1;

                string cleanedText = ClearTurkishCharacter(demoText);

                inx++;
                if (inx == 01) { ViewBag.Title01 = cleanedText; }
                if (inx == 02) { ViewBag.Title02 = cleanedText; }
                if (inx == 03) { ViewBag.Title03 = cleanedText; }
                if (inx == 04) { ViewBag.Title04 = cleanedText; }
                if (inx == 05) { ViewBag.Title05 = cleanedText; }
                if (inx == 06) { ViewBag.Title06 = cleanedText; }
                if (inx == 07) { ViewBag.Title07 = cleanedText; }
                if (inx == 08) { ViewBag.Title08 = cleanedText; }
                if (inx == 09) { ViewBag.Title09 = cleanedText; }
                if (inx == 10) { ViewBag.Title10 = cleanedText; }
            }
            return View(studentIndexViewModel);
        }

        //For Excel Header
        public static string ClearTurkishCharacter(string _dirtyText)
        {
            var text = _dirtyText;
            var unaccentedText = String.Join("", text.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark));
            return unaccentedText.Replace("ı", "i");
        }

        [Route("M210Student/GridStudentDataRead/{userID}/{check}/{isSibling}")]
        public IActionResult GridStudentDataRead(int userID, bool check, bool isSibling)
        {
            List<Student> student = new List<Student>();
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            student = _studentRepository.GetStudentClassroom(user.SchoolID, 0).Where(b => b.FirstName == null && b.LastName == null).ToList();
            foreach (var item in student)
            {
                _studentRepository.DeleteStudent(item);
            }

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

            bool isExist = false;
            List<StudentIndexViewModel> list = new List<StudentIndexViewModel>();
            foreach (var item in student)
            {
                if (item.FirstName == null && item.LastName == null)
                {
                    _studentRepository.DeleteStudent(item);

                    var studentPeriods = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (studentPeriods != null) _studentPeriodsRepository.DeleteStudentPeriods(studentPeriods);
                }
                else
                {
                    var studentAddress = _studentAddressRepository.GetStudentAddress(item.StudentID);
                    var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);

                    var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                    var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);

                    var studentViewModel1 = new StudentIndexViewModel();
                    {
                        studentViewModel1.ViewModelID = item.StudentID;
                        studentViewModel1.StudentID = item.StudentID;
                        studentViewModel1.Period = user.UserPeriod;
                        studentViewModel1.StudentPicture = item.StudentPicture;
                        studentViewModel1.Name = item.FirstName + " " + item.LastName;

                        if (school.NewPeriod == user.UserPeriod)
                        {
                            if (item.ClassroomID > 0)
                            {
                                studentViewModel1.StudentClassroom = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                            }
                        }
                        else
                        {
                            isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                            if (isExist)
                            {
                                studentViewModel1.StudentClassroom = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                            }
                        }


                        if (statuCategory != null)
                        {
                            studentViewModel1.StatuCategory = statuCategory.CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US") studentViewModel1.StatuCategory = statuCategory.Language1;
                        }
                        if (registrationTypeCategory != null)
                        {
                            studentViewModel1.RegistrationTypeCategory = registrationTypeCategory.CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US") studentViewModel1.RegistrationTypeCategory = registrationTypeCategory.Language1;
                        }
                        studentViewModel1.StudentNumber = item.StudentNumber;
                        studentViewModel1.IdNumber = item.IdNumber;
                        studentViewModel1.DateOfRegistration = item.DateOfRegistration;
                        studentViewModel1.ParentName = item.ParentName;

                        if (studentAddress != null)
                            studentViewModel1.StudentMobilePhone = studentAddress.MobilePhone;
                        if (parentAddress != null)
                            studentViewModel1.ParentMobilePhone = parentAddress.MobilePhone;

                        studentViewModel1.IsActive = item.IsActive;
                        studentViewModel1.SiblingID = item.SiblingID;
                        studentViewModel1.IsWebRegistration = item.IsWebRegistration;
                    };
                    list.Add(studentViewModel1);
                }
            }

            return Json(list);
        }


        [Route("M210Student/GridDataDeleteNew/{studentID}/{userID}")]
        public IActionResult GridDataDeleteNew(int studentID, int userID)
        {
            StudentDeleteAllFiles(studentID, userID);

            return Json(true);
        }

        [Route("M210Student/GridStudentDataDelete/{strResult}/{userID}")]
        public IActionResult GridStudentDataDelete([Bind(Prefix = "models")] string strResult, int userID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Student>>(strResult);
            var studentID = json[0].StudentID;

            StudentDeleteAllFiles(studentID, userID);

            return Json(true);
        }

        #endregion

        #region Sibling 
        [Route("M210Student/GridSiblingDataRead1/{userID}/{studentID}")]
        public IActionResult GridSiblingDataRead1(int userID, int studentID)
        {
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
            List<StudentIndexViewModel> list = new List<StudentIndexViewModel>();
            foreach (var item in students)
            {
                var studentViewModel1 = new StudentIndexViewModel();
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

                    studentViewModel1.ViewModelID = studentIDTmp;
                    studentViewModel1.StudentID = studentIDTmp;
                    studentViewModel1.StudentClassroom = classroomName;
                    studentViewModel1.Name = item.FirstName + " " + item.LastName;
                    studentViewModel1.StudentNumber = item.StudentNumber;
                    studentViewModel1.IdNumber = item.IdNumber;
                    studentViewModel1.ParentName = item.ParentName;
                    studentViewModel1.SiblingID = item.StudentID;
                };
                list.Add(studentViewModel1);
            }
            return Json(list);
        }
        [Route("M210Student/GridSiblingDataRead2/{userID}/{studentID}")]
        public IActionResult GridSiblingDataRead2(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> student = new List<Student>();
            student = _studentRepository.GetStudentSibling2(user.SchoolID, studentID).ToList();

            bool isExist = false;
            string classroomName = "";

            List<StudentIndexViewModel> list = new List<StudentIndexViewModel>();
            foreach (var item in student)
            {
                if (item.SiblingID == 0 || (item.SiblingID > 0 && item.SiblingID == studentID))
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

                    var studentViewModel1 = new StudentIndexViewModel();
                    {
                        studentViewModel1.ViewModelID = item.StudentID;
                        studentViewModel1.StudentID = item.StudentID;
                        studentViewModel1.StudentClassroom = classroomName;
                        studentViewModel1.Name = item.FirstName + " " + item.LastName;
                        studentViewModel1.StudentNumber = item.StudentNumber;
                        studentViewModel1.IdNumber = item.IdNumber;
                        studentViewModel1.ParentName = item.ParentName;
                        studentViewModel1.SiblingID = item.StudentID;
                    };
                    list.Add(studentViewModel1);
                }
            }
            return Json(list);
        }

        [Route("M210Student/GridStudentsDataRead/{userID}/{studentID}")]
        public IActionResult GridStudentsDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            List<Student> students = new List<Student>();
            students = _studentRepository.GetStudentSibling2(user.SchoolID, studentID).ToList();

            bool isExist = false;
            List<StudentIndexViewModel> list = new List<StudentIndexViewModel>();
            foreach (var item in students)
            {
                var studentViewModel1 = new StudentIndexViewModel();
                {
                    studentViewModel1.ViewModelID = item.StudentID;
                    studentViewModel1.StudentID = item.StudentID;
                    if (item.ClassroomID > 0)
                        studentViewModel1.StudentClassroom = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                    else
                    {
                        isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                        if (isExist)
                        {
                            studentViewModel1.StudentClassroom = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID = user.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        }
                    }

                    studentViewModel1.Name = item.FirstName + " " + item.LastName;
                    studentViewModel1.StudentNumber = item.StudentNumber;
                    studentViewModel1.IdNumber = item.IdNumber;
                    studentViewModel1.ParentName = item.ParentName;
                    studentViewModel1.SiblingID = item.StudentID;
                };
                list.Add(studentViewModel1);
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M210Student/GridSiblingDataUpdate/{userID}/{studentID}/{selectedID}")]
        public IActionResult GridSiblingDataUpdate(int userID, int studentID, string selectedID)
        {
            var user = _usersRepository.GetUser(userID);
            List<StudentIndexViewModel> list = new List<StudentIndexViewModel>();
            string[] selectedDim = selectedID.Split(',');
            var ID = 0;

            var student = _studentRepository.GetStudent(studentID);
            student.SiblingID = studentID;

            _studentRepository.UpdateStudent(student);

            foreach (var siblingID in selectedDim)
            {
                ID = Int32.Parse(siblingID);
                var studentsb = _studentRepository.GetStudent(ID);
                if (studentsb.SiblingID > 0)
                    studentsb.SiblingID = 0;
                else studentsb.SiblingID = studentID;

                _studentRepository.UpdateStudent(studentsb);
            }

            var students = _studentRepository.GetStudentSibling1(user.SchoolID, studentID).ToList();
            int total = students.Count();
            if (total == 0)
            {
                student.SiblingID = 0;
                _studentRepository.UpdateStudent(student);
            }

            //return Json(true);
            return Json(new { total = total });
        }

        private void SiblingAddressMove(int studentID1, int studentID2)
        {
            //StudentAddress
            var studentAddress1 = _studentAddressRepository.GetStudentAddress(studentID1);
            var studentAddress2 = _studentAddressRepository.GetStudentAddress(studentID2);
            var isNew = false;
            if (studentAddress2 == null)
            {
                isNew = true;
                studentAddress2 = new StudentAddress();
                studentAddress2.StudentAddressID = 0;
            }
            if (studentAddress1 != null)
            {
                studentAddress2.StudentID = studentID2;
                studentAddress2.IsSms = studentAddress1.IsSms;
                studentAddress2.MobilePhone = studentAddress1.MobilePhone;
                studentAddress2.IsEMail = studentAddress1.IsEMail;
                studentAddress2.EMail = studentAddress1.EMail;
                studentAddress2.Address1 = studentAddress1.Address1;
                studentAddress2.CityParameterID1 = studentAddress1.CityParameterID1;
                studentAddress2.TownParameterID1 = studentAddress1.TownParameterID1;
                studentAddress2.ZipCode1 = studentAddress1.ZipCode1;
                studentAddress2.Address2 = studentAddress1.Address2;
                studentAddress2.CityParameterID2 = studentAddress1.CityParameterID2;
                studentAddress2.TownParameterID2 = studentAddress1.TownParameterID2;
                studentAddress2.ZipCode2 = studentAddress1.ZipCode2;

                if (isNew == true) _studentAddressRepository.CreateStudentAddress(studentAddress2);
                else _studentAddressRepository.UpdateStudentAddress(studentAddress2);
            }
            //StudentParentAddress
            isNew = false;
            var studentParentAddress1 = _studentParentAddressRepository.GetStudentParentAddress(studentID1);
            var studentParentAddress2 = _studentParentAddressRepository.GetStudentParentAddress(studentID2);
            if (studentParentAddress2 == null)
            {
                isNew = true;
                studentParentAddress2 = new StudentParentAddress();
                studentParentAddress2.StudentParentAddressID = 0;
            }

            if (studentParentAddress1 != null)
            {
                studentParentAddress2.StudentID = studentID2;
                studentParentAddress2.ParentPicture = studentParentAddress1.ParentPicture;
                studentParentAddress2.KinshipCategoryID = studentParentAddress1.KinshipCategoryID;
                studentParentAddress2.ProfessionCategoryID = studentParentAddress1.ProfessionCategoryID;
                studentParentAddress2.IdNumber = studentParentAddress1.IdNumber;
                studentParentAddress2.HomePhone = studentParentAddress1.HomePhone;
                studentParentAddress2.WorkPhone = studentParentAddress1.WorkPhone;
                studentParentAddress2.IsSMS = studentParentAddress1.IsSMS;
                studentParentAddress2.MobilePhone = studentParentAddress1.MobilePhone;
                studentParentAddress2.IsEmail = studentParentAddress1.IsEmail;
                studentParentAddress2.EMail = studentParentAddress1.EMail;
                studentParentAddress2.HomeAddress = studentParentAddress1.HomeAddress;
                studentParentAddress2.HomeCityParameterID = studentParentAddress1.HomeCityParameterID;
                studentParentAddress2.HomeTownParameterID = studentParentAddress1.HomeTownParameterID;
                studentParentAddress2.HomeZipCode = studentParentAddress1.HomeZipCode;
                studentParentAddress2.WorkAddress = studentParentAddress1.WorkAddress;
                studentParentAddress2.WorkCityParameterID = studentParentAddress1.WorkCityParameterID;
                studentParentAddress2.WorkTownParameterID = studentParentAddress1.WorkTownParameterID;
                studentParentAddress2.WorkZipCode = studentParentAddress1.WorkZipCode;
                studentParentAddress2.Name3 = studentParentAddress1.Name3;
                studentParentAddress2.IdNumber3 = studentParentAddress1.IdNumber3;
                studentParentAddress2.HomePhone3 = studentParentAddress1.HomePhone3;
                studentParentAddress2.WorkPhone3 = studentParentAddress1.WorkPhone3;
                studentParentAddress2.MobilePhone3 = studentParentAddress1.MobilePhone3;
                studentParentAddress2.Address3 = studentParentAddress1.Address3;
                studentParentAddress2.CityParameterID3 = studentParentAddress1.CityParameterID3;
                studentParentAddress2.TownParameterID3 = studentParentAddress1.TownParameterID3;
                studentParentAddress2.ZipCode3 = studentParentAddress1.ZipCode3;
                studentParentAddress2.Note = studentParentAddress1.Note;

                if (isNew == true) _studentParentAddressRepository.CreateStudentParentAddress(studentParentAddress2);
                else _studentParentAddressRepository.UpdateStudentParentAddress(studentParentAddress2);
            }

            //StudentFamilyAddress
            isNew = false;
            var studentFamilyAddress1 = _studentFamilyAddressRepository.GetStudentFamilyAddress(studentID1);
            var studentFamilyAddress2 = _studentFamilyAddressRepository.GetStudentFamilyAddress(studentID2);
            if (studentFamilyAddress2 == null)
            {
                isNew = true;
                studentFamilyAddress2 = new StudentFamilyAddress();
                studentFamilyAddress2.StudentFamilyAddressID = 0;

            }

            if (studentFamilyAddress1 != null)
            {
                studentFamilyAddress2.StudentID = studentID2;
                studentFamilyAddress2.FatherName = studentFamilyAddress1.FatherName;
                studentFamilyAddress2.FatherProfessionCategoryID = studentFamilyAddress1.FatherProfessionCategoryID;
                studentFamilyAddress2.FatherIdNumber = studentFamilyAddress1.FatherIdNumber;
                studentFamilyAddress2.FatherHomePhone = studentFamilyAddress1.FatherHomePhone;
                studentFamilyAddress2.FatherWorkPhone = studentFamilyAddress1.FatherWorkPhone;
                studentFamilyAddress2.FatherIsSMS = studentFamilyAddress1.FatherIsSMS;
                studentFamilyAddress2.FatherMobilePhone = studentFamilyAddress1.FatherMobilePhone;
                studentFamilyAddress2.FatherIsEmail = studentFamilyAddress1.FatherIsEmail;
                studentFamilyAddress2.FatherEMail = studentFamilyAddress1.FatherEMail;
                studentFamilyAddress2.FatherHomeAddress = studentFamilyAddress1.FatherHomeAddress;
                studentFamilyAddress2.FatherHomeCityParameterID = studentFamilyAddress1.FatherHomeCityParameterID;
                studentFamilyAddress2.FatherHomeTownParameterID = studentFamilyAddress1.FatherHomeTownParameterID;
                studentFamilyAddress2.FatherHomeZipCode = studentFamilyAddress1.FatherHomeZipCode;
                studentFamilyAddress2.FatherWorkAddress = studentFamilyAddress1.FatherWorkAddress;
                studentFamilyAddress2.FatherWorkCityParameterID = studentFamilyAddress1.FatherWorkCityParameterID;
                studentFamilyAddress2.FatherWorkTownParameterID = studentFamilyAddress1.FatherWorkTownParameterID;
                studentFamilyAddress2.FatherWorkZipCode = studentFamilyAddress1.FatherWorkZipCode;
                studentFamilyAddress2.MotherName = studentFamilyAddress1.MotherName;
                studentFamilyAddress2.MotherProfessionCategoryID = studentFamilyAddress1.MotherProfessionCategoryID;
                studentFamilyAddress2.MotherIdNumber = studentFamilyAddress1.MotherIdNumber;
                studentFamilyAddress2.MotherHomePhone = studentFamilyAddress1.MotherHomePhone;
                studentFamilyAddress2.MotherWorkPhone = studentFamilyAddress1.MotherWorkPhone;
                studentFamilyAddress2.MotherIsSMS = studentFamilyAddress1.MotherIsSMS;
                studentFamilyAddress2.MotherMobilePhone = studentFamilyAddress1.MotherMobilePhone;
                studentFamilyAddress2.MotherIsEmail = studentFamilyAddress1.MotherIsEmail;
                studentFamilyAddress2.MotherEMail = studentFamilyAddress1.MotherEMail;
                studentFamilyAddress2.MotherHomeAddress = studentFamilyAddress1.MotherHomeAddress;
                studentFamilyAddress2.MotherHomeCityParameterID = studentFamilyAddress1.MotherHomeCityParameterID;
                studentFamilyAddress2.MotherHomeTownParameterID = studentFamilyAddress1.MotherHomeTownParameterID;
                studentFamilyAddress2.MotherHomeZipCode = studentFamilyAddress1.MotherHomeZipCode;
                studentFamilyAddress2.MotherWorkAddress = studentFamilyAddress1.MotherWorkAddress;
                studentFamilyAddress2.MotherWorkCityParameterID = studentFamilyAddress1.MotherWorkCityParameterID;
                studentFamilyAddress2.MotherWorkTownParameterID = studentFamilyAddress1.MotherWorkTownParameterID;
                studentFamilyAddress2.MotherWorkZipCode = studentFamilyAddress1.MotherWorkZipCode;
                studentFamilyAddress2.Note = studentFamilyAddress1.Note;

                if (isNew == true) _studentFamilyAddressRepository.CreateStudentFamilyAddress(studentFamilyAddress2);
                else _studentFamilyAddressRepository.UpdateStudentFamilyAddress(studentFamilyAddress2);
            }
            //StudentInvoiceAddress
            isNew = false;
            var studentInvoiceAddress1 = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID1);
            var studentInvoiceAddress2 = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID2);
            if (studentInvoiceAddress2 == null)
            {
                isNew = true;
                studentInvoiceAddress2 = new StudentInvoiceAddress();
                studentInvoiceAddress2.StudentInvoiceAddressID = 0;
            }

            if (studentInvoiceAddress1 != null)
            {
                studentInvoiceAddress2.StudentID = studentID2;
                studentInvoiceAddress2.InvoiceTitle = studentInvoiceAddress1.InvoiceTitle;
                studentInvoiceAddress2.InvoiceAddress = studentInvoiceAddress1.InvoiceAddress;
                studentInvoiceAddress2.InvoiceCityParameterID = studentInvoiceAddress1.InvoiceCityParameterID;
                studentInvoiceAddress2.InvoiceTownParameterID = studentInvoiceAddress1.InvoiceTownParameterID;
                studentInvoiceAddress2.InvoiceCountry = studentInvoiceAddress1.InvoiceCountry;
                studentInvoiceAddress2.InvoiceZipCode = studentInvoiceAddress1.InvoiceZipCode;
                studentInvoiceAddress2.InvoiceTaxOffice = studentInvoiceAddress1.InvoiceTaxOffice;
                studentInvoiceAddress2.EMail = studentInvoiceAddress1.EMail;
                studentInvoiceAddress2.Notes = studentInvoiceAddress1.Notes;
                studentInvoiceAddress2.AccountCode = studentInvoiceAddress1.AccountCode;
                studentInvoiceAddress2.InvoiceProfile = studentInvoiceAddress1.InvoiceProfile;
                studentInvoiceAddress2.InvoiceTypeParameter = studentInvoiceAddress1.InvoiceTypeParameter;
                studentInvoiceAddress2.IsInvoiceDetailed = studentInvoiceAddress1.IsInvoiceDetailed;
                studentInvoiceAddress2.IsInvoiceDiscount = studentInvoiceAddress1.IsInvoiceDiscount;

                if (isNew == true) _studentInvoiceAddressRepository.CreateStudentInvoiceAddress(studentInvoiceAddress2);
                else _studentInvoiceAddressRepository.UpdateStudentInvoiceAddress(studentInvoiceAddress2);
            }
        }
        public IActionResult StudentDeleteAllFiles(int studentID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var student = _studentRepository.GetStudent(studentID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            if (student != null)
                _studentRepository.DeleteStudent(student);

            var studentPeriods = _studentPeriodsRepository.GetStudentPeriod(user.SchoolID, studentID, user.UserPeriod);
            if (studentPeriods != null)
                _studentPeriodsRepository.DeleteStudentPeriods(studentPeriods);

            var studentAddress = _studentAddressRepository.GetStudentAddress(studentID);
            if (studentAddress != null)
                _studentAddressRepository.DeleteStudentAddress(studentAddress);

            var studentFamilyAddress = _studentFamilyAddressRepository.GetStudentFamilyAddress(studentID);
            if (studentFamilyAddress != null)
                _studentFamilyAddressRepository.DeleteStudentFamilyAddress(studentFamilyAddress);

            var studentParentAddress = _studentParentAddressRepository.GetStudentParentAddress(studentID);
            if (studentParentAddress != null)
                _studentParentAddressRepository.DeleteStudentParentAddress(studentParentAddress);

            var studentNote = _studentNoteRepository.GetStudentNote(studentID);
            if (studentNote != null)
                _studentNoteRepository.DeleteStudentNote(studentNote);

            var studentDebt = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, studentID);
            foreach (var item in studentDebt)
            {
                _studentDebtRepository.DeleteStudentDebt(item);
            }

            var studentDebtDetail = _studentDebtDetailTableRepository.GetStudentDebtDetailTable1(studentID, user.UserPeriod);
            foreach (var item in studentDebtDetail)
            {
                _studentDebtDetailTableRepository.DeleteStudentDebtDetailTable(item);
            }

            var studentDiscount = _studentDiscountRepository.GetDiscount2(user.SchoolID, user.UserPeriod, studentID);
            foreach (var item in studentDiscount)
            {
                _studentDiscountRepository.DeleteStudentDiscount(item);
            }

            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(school.SchoolID, studentID, user.UserPeriod);
            foreach (var item in studentinstallment)
            {
                _studentInstallmentRepository.DeleteStudentInstallment(item);
            }

            var studentinstallmentPayment = _studentInstallmentPaymentRepository.GetStudentInstallmentPayment1(user.UserPeriod, studentID);
            foreach (var item in studentinstallmentPayment)
            {
                _studentInstallmentPaymentRepository.DeleteStudentInstallmentPayment(item);
            }

            var studentpayments = _studentPaymentRepository.GetStudentPayment(user.SchoolID, user.UserPeriod, studentID);
            foreach (var item in studentpayments)
            {
                _studentPaymentRepository.DeleteStudentPayment(item);
            }

            var studentInvoice = _studentInvoiceRepository.GetStudentInvoice(user.UserPeriod, user.SchoolID, studentID).ToList();
            foreach (var item in studentInvoice)
            {
                _studentInvoiceRepository.DeleteStudentInvoice(item);
            }

            var studentInvoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetail3(user.SchoolID, user.UserPeriod, studentID).ToList();
            foreach (var item in studentInvoiceDetail)
            {
                _studentInvoiceDetailRepository.DeleteStudentInvoiceDetail(item);
            }

            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);
            if (studentInvoiceAddress != null)
                _studentInvoiceAddressRepository.DeleteStudentInvoiceAddress(studentInvoiceAddress);

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            if (studentTemp != null)
                _studentTempRepository.DeleteStudentTemp(studentTemp);


            var tempData = _tempDataRepository.GetTempData(studentID);
            foreach (var item in tempData)
            {
                _tempDataRepository.DeleteTempData(item);
            }


            //////Users Log//////////////////

            var log = new UsersLog();
            log.SchoolID = user.SchoolID;
            log.Period = user.UserPeriod;
            log.UserID = userID;
            log.TransactionID = _parameterRepository.GetParameterCategoryName("İptal İşlemi").CategoryID;
            log.UserLogDate = DateTime.Now;
            string classroom = "";
            if (student.ClassroomID > 0)
                classroom = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomName;

            string status = "";
            string registrationType = "";
            if (student.StatuCategoryID > 0)
            {
                if (user.SelectedCulture.Trim() == "en-US")
                {
                    status = _parameterRepository.GetParameter(student.StatuCategoryID).Language1;
                    registrationType = _parameterRepository.GetParameter(student.RegistrationTypeCategoryID).Language1;
                }
                else
                {
                    status = _parameterRepository.GetParameter(student.StatuCategoryID).CategoryName;
                    registrationType = _parameterRepository.GetParameter(student.RegistrationTypeCategoryID).CategoryName;
                }
            }
            decimal cashPayment = 0;
            decimal subTotal = 0;
            int intstallment = 0;
            if (studentTemp != null)
            {
                cashPayment = Math.Round(studentTemp.CashPayment, school.CurrencyDecimalPlaces);
                subTotal = Math.Round(studentTemp.SubTotal, school.CurrencyDecimalPlaces);
                intstallment = studentTemp.Installment;
            }

            log.UserLogDescription = student.FirstName + " " + student.LastName + " Kayıt Silme İşlemi, " + "Sınıfı:" + classroom + ", Durumu:" + status + ", Kayıt Şekli:" + registrationType + ", Peşinat:" + cashPayment + "," + intstallment + " Ay Vadeli, " + "Toplam:" + subTotal;
            if (user.SelectedCulture.Trim() == "en-US")
            {
                log.UserLogDescription = student.FirstName + " " + student.LastName + " Registration deletion., " + "Classroom:" + classroom + ", Status:" + status + ",Registration Type:" + registrationType + ", Cash Payment:" + cashPayment + "," + intstallment + " Monthly Payment, " + "Total:" + subTotal;
            }
            _usersLogRepository.CreateUsersLog(log);
            ////////////////////////////////

            return Json(true);
        }
        #endregion

        #region Student
        [HttpGet]
        public IActionResult AddOrEditStudent(int studentID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("İndirim İşlemleri").CategoryID;
            bool isPermissionDiscount = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            categoryID = _parameterRepository.GetParameterCategoryName("Ücret Bilgilerini Görme").CategoryID;
            bool isPermissionFee = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            bool IsNew = false;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = new Student();

            if (studentID == 0)
            {
                student.StudentPicture = "male.jpg";
                student.SchoolID = user.SchoolID;
                student.IsWebRegistration = false;
                _studentRepository.CreateStudent(student);
                studentID = student.StudentID;
                TempData["studentID"] = student.StudentID;
                IsNew = true;
            }
            else
            {
                student = _studentRepository.GetStudent(studentID);
                if (student.ClassroomID == 0 && user.UserPeriod == school.NewPeriod) IsNew = true;
            }
            if (student.DateOfRegistration == null)
                student.DateOfRegistration = DateTime.Today;
            if (student.FirstDateOfRegistration == null)
                student.FirstDateOfRegistration = student.DateOfRegistration;

            var statuCategoryName = "";
            if (student != null && student.StatuCategoryID > 0)
            {
                if (user.SelectedCulture.Trim() == "en-US") statuCategoryName = _parameterRepository.GetParameter(student.StatuCategoryID).Language1;
                else statuCategoryName = _parameterRepository.GetParameter(student.StatuCategoryID).CategoryName;
            }

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            if (studentTemp == null || studentTemp.StudentTempID == 0)
            {
                studentTemp = new StudentTemp();
                studentTemp.SchoolID = user.SchoolID;
                studentTemp.StudentID = studentID;
                studentTemp.Period = user.UserPeriod;
                studentTemp.Installment = school.DefaultInstallment;
                studentTemp.TransactionDate = DateTime.Today.AddMonths(1);
                studentTemp.CashPayment = 0;
                studentTemp.StatuCategoryName = statuCategoryName;
                _studentTempRepository.CreateStudentTemp(studentTemp);
            }
            else
            {
                studentTemp.StatuCategoryName = statuCategoryName;
                _studentTempRepository.UpdateStudentTemp(studentTemp);
            }

            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (pSerialNumber == null)
                pSerialNumber = new PSerialNumber();

            categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);

            if (student.StudentPicture == null || student.StudentPicture == "")
                student.StudentPicture = "male.jpg";

            var studentAddress = _studentAddressRepository.GetStudentAddress(student.StudentID);
            if (studentAddress == null)
            {
                studentAddress = new StudentAddress();
                studentAddress.IsEMail = true;
                studentAddress.IsSms = true;
            }

            var studentParentAddress = _studentParentAddressRepository.GetStudentParentAddress(student.StudentID);
            if (studentParentAddress == null)
                studentParentAddress = new StudentParentAddress();

            if (studentParentAddress.ParentPicture == null || studentParentAddress.ParentPicture == "")
            {
                studentParentAddress.ParentPicture = "male.jpg";
                studentParentAddress.IsEmail = true;
                studentParentAddress.IsSMS = true;
            }
            var studentFamilyAddress = _studentFamilyAddressRepository.GetStudentFamilyAddress(student.StudentID);
            if (studentFamilyAddress == null)
            {
                studentFamilyAddress = new StudentFamilyAddress();
                studentFamilyAddress.FatherIsEmail = true;
                studentFamilyAddress.FatherIsSMS = true;
                studentFamilyAddress.MotherIsEmail = true;
                studentFamilyAddress.MotherIsSMS = true;
            }

            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(student.StudentID);
            if (studentInvoiceAddress == null)
                studentInvoiceAddress = new StudentInvoiceAddress();

            if (studentInvoiceAddress.InvoiceTaxNumber == null)
                studentInvoiceAddress.InvoiceTaxNumber = student.IdNumber;
            if (studentInvoiceAddress.Notes == null)
                studentInvoiceAddress.Notes = student.FirstName + " " + student.LastName;

            var studentNote = _studentNoteRepository.GetStudentNote(student.StudentID);
            if (studentNote == null)
                studentNote = new StudentNote();


            var period = user.UserPeriod;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["NewPeriod"] = school.NewPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);


            var studentDebt = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, studentID);
            decimal totalAmount = studentDebt.Sum(p => p.UnitPrice);
            TempData["newYear"] = 0;
            if (school.NewPeriod == user.UserPeriod && totalAmount == 0) TempData["newYear"] = 1;

            string sortIcon = "filter";
            if (school.SortOption == false) sortIcon = "filter-clear";

            bool isExist = false;
            string classroomName = "";

            if (school.NewPeriod == user.UserPeriod)
            {
                if (student.ClassroomID > 0)
                {
                    classroomName = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomName;
                }
            }
            else
            {
                isExist = _studentPeriodsRepository.ExistStudentPeriods(student.SchoolID, student.StudentID, user.UserPeriod);
                if (isExist)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(student.SchoolID, student.StudentID, user.UserPeriod).ClassroomName;
                    student.ClassroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            //Sibling Name on Window Title
            string siblingName = "";
            var siblingID = 0;
            var studentIDTmp = _studentRepository.GetStudent(studentID).SiblingID;
            if (studentIDTmp != 0)
            {
                var studentSibling = _studentRepository.GetStudent(studentIDTmp);
                siblingName = studentSibling.FirstName + " " + studentSibling.LastName;
                siblingID = studentIDTmp;
            }

            string categoryName1 = "dicountName";
            string categoryName2 = "categoryName";
            string categoryName3 = "name";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName1 = "language1";
                categoryName2 = "language1";
                categoryName3 = "language1";
            }

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallmentByPeriod(student.SchoolID, user.UserPeriod, student.StudentID);
            bool isFirst = true;
            if (studentInstallment.Count() > 0) isFirst = false;

            var studentViewModel = new StudentViewModel
            {
                IsPermissionDiscount = isPermissionDiscount,
                IsPermissionFee = isPermissionFee,
                Period = period,
                UserID = userID,
                SchoolID = user.SchoolID,
                StudentID = studentID,
                ViewIsNew = IsNew,
                ViewIsFirst = isFirst,

                Student = student,
                StudentName = student.FirstName + " " + student.LastName,
                StudentClassroom = classroomName,
                ClassroomID = student.ClassroomID,
                StudentTemp = studentTemp,
                StudentAddress = studentAddress,
                StudentParentAddress = studentParentAddress,
                StudentFamilyAddress = studentFamilyAddress,
                StudentInvoiceAddress = studentInvoiceAddress,
                StudentNote = studentNote,
                PSerialNumber = pSerialNumber,
                Parameter = parameter,
                SchoolInfo = school,
                SelectedCulture = user.SelectedCulture.Trim(),
                SortType = school.SortType,
                SortOption = school.SortOption,
                SortIcon = sortIcon,
                IsExplanationShow = student.IsExplanationShow,
                SelectedSchoolCode = user.SelectedSchoolCode,

                SiblingID = siblingID,
                SiblingName = siblingName,

                RegistrationTypeSubID = 3,
                StatuCategorySubID = 4,
                StatuCategoryID = 10,
                StartDate = school.SchoolYearStart,
                EndDate = school.SchoolYearEnd,
                wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/"), //Picture Path
                CategoryName1 = categoryName1,
                CategoryName2 = categoryName2,
                CategoryName3 = categoryName3

            };
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditStudent(StudentViewModel studentViewModel, SchoolFee schoolFee,
                                              IFormFile imgfileStudent, IFormFile imgfileParent)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(studentViewModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            bool isSchoolChange = false;
            if (studentViewModel.SchoolID != studentViewModel.Student.SchoolID) isSchoolChange = true;

            bool isNew = false;
            var gender = _parameterRepository.GetParameter(studentViewModel.Student.GenderTypeCategoryID);
            studentViewModel.Student.GenderTypeCategoryID = gender.CategoryID;
            if (studentViewModel.Student.StudentPicture == null || studentViewModel.Student.StudentPicture == "male.jpg" || studentViewModel.Student.StudentPicture == "female.jpg")
            {
                if (gender.CategoryName == "Erkek" || gender.Language1 == "Male")
                    studentViewModel.Student.StudentPicture = "male.jpg";
                else studentViewModel.Student.StudentPicture = "female.jpg";
            }
            if (!ModelState.IsValid)
                ViewBag.IsSuccess = true;

            if (ModelState.IsValid)
            {
                //go on as normal
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
            }

            //Önceki dosya siliniyor
            var pathStudent = Path.Combine("Upload", "Students");
            if (imgfileStudent != null && ModelState.IsValid)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, pathStudent, studentViewModel.Student.StudentPicture);
                if (System.IO.File.Exists(imagePath))
                    if (studentViewModel.Student.StudentPicture != "male.jpg" && studentViewModel.Student.StudentPicture != "female.jpg")
                        System.IO.File.Delete(imagePath);
            }
            var pathParent = Path.Combine("Upload", "Parents");
            if (imgfileParent != null && ModelState.IsValid)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, pathParent, studentViewModel.StudentParentAddress.ParentPicture);
                if (System.IO.File.Exists(imagePath))
                    if (studentViewModel.StudentParentAddress.ParentPicture != "male.jpg" && studentViewModel.StudentParentAddress.ParentPicture != "female.jpg")
                        System.IO.File.Delete(imagePath);
            }

            if (imgfileStudent != null && ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imgfileStudent.FileName);
                string extension = Path.GetExtension(imgfileStudent.FileName);

                var uniqFileName = Guid.NewGuid().ToString();
                studentViewModel.Student.StudentPicture = fileName = Path.GetFileName(uniqFileName + "-" + fileName.ToLower() + extension);
                int lenght = studentViewModel.Student.StudentPicture.Length;
                if (lenght > 100) studentViewModel.Student.StudentPicture = fileName = Path.GetFileName(uniqFileName + extension);

                pathStudent = Path.Combine(wwwRootPath + "/Upload/Students", fileName);
                using (var fileStream = new FileStream(pathStudent, FileMode.Create))
                {
                    await imgfileStudent.CopyToAsync(fileStream);
                }
            }
            if (imgfileParent != null && ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imgfileParent.FileName);
                string extension = Path.GetExtension(imgfileParent.FileName);

                var uniqFileName = Guid.NewGuid().ToString();
                studentViewModel.StudentParentAddress.ParentPicture = fileName = Path.GetFileName(uniqFileName + "-" + fileName.ToLower() + extension);
                int lenght = studentViewModel.StudentParentAddress.ParentPicture.Length;
                if (lenght > 100) studentViewModel.StudentParentAddress.ParentPicture = fileName = Path.GetFileName(uniqFileName + extension);

                pathParent = Path.Combine(wwwRootPath + "/Upload/Parents", fileName);
                using (var fileStream = new FileStream(pathParent, FileMode.Create))
                {
                    await imgfileParent.CopyToAsync(fileStream);
                }
            }

            //Parameter dosyasında "Pasif Kayıt" olarak kontrol ediliyor, ID si alınıyor
            var statuCategoryID = _parameterRepository.GetParameterCategoryName("Pasif Kayıt").CategoryID;
            if (studentViewModel.Student.StatuCategoryID == statuCategoryID) studentViewModel.Student.IsActive = false;
            else studentViewModel.Student.IsActive = true;

            if (ModelState.IsValid)
            {
                if (studentViewModel.ViewIsNew)
                {
                    isNew = true;
                    if (studentViewModel.SchoolID > 0 && studentViewModel.Student.SchoolID == 0) studentViewModel.Student.SchoolID = studentViewModel.SchoolID;

                    var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
                    if (serialNumbers == null)
                        serialNumbers = new PSerialNumber();

                    var lastNumber = serialNumbers.RegisterNo += 1;
                    studentViewModel.Student.StudentSerialNumber = lastNumber;

                    studentViewModel.Student.FirstDateOfRegistration = studentViewModel.Student.DateOfRegistration;

                    var studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
                    studentViewModel.Student.IsPension = false;
                    if (studentTemp.IsPension) studentViewModel.Student.IsPension = true;
                    _studentRepository.UpdateStudent(studentViewModel.Student);

                    //////Users Log//////////////////

                    var log = new UsersLog();
                    log.SchoolID = studentViewModel.SchoolID;
                    log.Period = studentViewModel.Period;
                    log.UserID = studentViewModel.UserID;
                    log.TransactionID = _parameterRepository.GetParameterCategoryName("Öğrenci Kayıt").CategoryID;
                    log.UserLogDate = DateTime.Now;
                    string classroom = "";
                    if (studentViewModel.Student.ClassroomID > 0)
                        classroom = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;

                    string status = "";
                    string registrationType = "";
                    if (user.SelectedCulture.Trim() == "en-US")
                    {
                        status = _parameterRepository.GetParameter(studentViewModel.Student.StatuCategoryID).Language1;
                        registrationType = _parameterRepository.GetParameter(studentViewModel.Student.RegistrationTypeCategoryID).Language1;
                    }
                    else
                    {
                        status = _parameterRepository.GetParameter(studentViewModel.Student.StatuCategoryID).CategoryName;
                        registrationType = _parameterRepository.GetParameter(studentViewModel.Student.RegistrationTypeCategoryID).CategoryName;
                    }

                    decimal cashPayment = Math.Round(studentTemp.CashPayment, school.CurrencyDecimalPlaces);
                    decimal subTotal = Math.Round(studentTemp.SubTotal, school.CurrencyDecimalPlaces);
                    int intstallment = studentTemp.Installment;
                    log.UserLogDescription = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " Yeni Öğrenci Kaydı Yapıldı, " + "Sınıfı:" + classroom + ", Durumu:" + status + ", Kayıt Şekli:" + registrationType + ", Peşinat:" + cashPayment + "," + intstallment + " Ay Vadeli, " + "Toplam:" + subTotal;
                    if (user.SelectedCulture.Trim() == "en-US")
                    {
                        log.UserLogDescription = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " New Student Registration Made, " + "Classroom:" + classroom + ", Status:" + status + ",Registration Type:" + registrationType + ", Cash Payment:" + cashPayment + "," + intstallment + " Monthly Payment, " + "Total:" + subTotal;
                    }

                    _usersLogRepository.CreateUsersLog(log);
                    ////////////////////////////////

                    var studentPeriods = _studentPeriodsRepository.GetStudentPeriod(studentViewModel.SchoolID, studentViewModel.StudentID, studentViewModel.Period);
                    if (studentPeriods == null)
                    {
                        studentPeriods = new StudentPeriods();
                        studentPeriods.StudentPeriodID = 0;
                        studentPeriods.SchoolID = studentViewModel.SchoolID;
                        studentPeriods.StudentID = studentViewModel.Student.StudentID;
                        studentPeriods.Period = studentViewModel.Period;
                        if (studentViewModel.Student.ClassroomID > 0)
                            studentPeriods.ClassroomName = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;
                        _studentPeriodsRepository.CreateStudentPeriods(studentPeriods);
                    }
                    else
                    {
                        if (studentViewModel.Student.ClassroomID > 0)
                        {
                            studentPeriods.ClassroomName = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;
                            _studentPeriodsRepository.UpdateStudentPeriods(studentPeriods);
                        }
                    }

                    if (serialNumbers.PSerialNumberID == 0 && studentViewModel.ViewIsNew == true)
                    {
                        serialNumbers.PSerialNumberID = studentViewModel.UserID;
                        serialNumbers.RegisterNo = lastNumber;
                        _pSerialNumberRepository.CreatePSerialNumber(serialNumbers);
                    }
                    else
                    {
                        serialNumbers.RegisterNo = lastNumber;
                        _pSerialNumberRepository.UpdatePSerialNumber(serialNumbers);
                    }

                }
                else
                {
                    if (studentViewModel.Student.FirstDateOfRegistration == null)
                        studentViewModel.Student.FirstDateOfRegistration = studentViewModel.Student.DateOfRegistration;
                    if (studentViewModel.SchoolID > 0 && studentViewModel.Student.SchoolID == 0) studentViewModel.Student.SchoolID = studentViewModel.SchoolID;
                    _studentRepository.UpdateStudent(studentViewModel.Student);

                    var studentPeriods = _studentPeriodsRepository.GetStudentPeriod(studentViewModel.SchoolID, studentViewModel.Student.StudentID, studentViewModel.Period);
                    if (studentPeriods == null)
                    {
                        studentPeriods = new StudentPeriods();
                        studentPeriods.StudentPeriodID = 0;
                        studentPeriods.SchoolID = studentViewModel.SchoolID;
                        studentPeriods.StudentID = studentViewModel.Student.StudentID;
                        studentPeriods.Period = studentViewModel.Period;
                        studentPeriods.ClassroomName = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;
                        _studentPeriodsRepository.CreateStudentPeriods(studentPeriods);
                    }
                    else
                    {
                        string studentClassroom = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;
                        if (studentPeriods.ClassroomName != studentClassroom)
                        {
                            studentPeriods.ClassroomName = studentClassroom;
                            _studentPeriodsRepository.UpdateStudentPeriods(studentPeriods);
                        }
                    }
                }

                if (!studentViewModel.StudentAddress.isEmpty)
                {
                    if (studentViewModel.StudentAddress.StudentAddressID == 0)
                    {
                        studentViewModel.StudentAddress.StudentID = studentViewModel.Student.StudentID;
                        _studentAddressRepository.CreateStudentAddress(studentViewModel.StudentAddress);
                    }
                    else
                    {
                        _studentAddressRepository.UpdateStudentAddress(studentViewModel.StudentAddress);
                    }
                }

                if (!studentViewModel.StudentFamilyAddress.isEmpty)
                {
                    if (studentViewModel.StudentFamilyAddress.StudentFamilyAddressID == 0)
                    {
                        studentViewModel.StudentFamilyAddress.StudentID = studentViewModel.Student.StudentID;
                        _studentFamilyAddressRepository.CreateStudentFamilyAddress(studentViewModel.StudentFamilyAddress);
                    }
                    else
                    {
                        _studentFamilyAddressRepository.UpdateStudentFamilyAddress(studentViewModel.StudentFamilyAddress);
                    }
                }

                if (!studentViewModel.StudentParentAddress.isEmpty)
                {
                    if (studentViewModel.StudentParentAddress.StudentParentAddressID == 0)
                    {
                        studentViewModel.StudentParentAddress.StudentID = studentViewModel.Student.StudentID;
                        _studentParentAddressRepository.CreateStudentParentAddress(studentViewModel.StudentParentAddress);
                    }
                    else
                    {
                        _studentParentAddressRepository.UpdateStudentParentAddress(studentViewModel.StudentParentAddress);
                    }
                }

                if (!studentViewModel.StudentInvoiceAddress.isEmpty)
                {
                    if (studentViewModel.StudentInvoiceAddress.StudentInvoiceAddressID == 0)
                    {
                        studentViewModel.StudentInvoiceAddress.StudentID = studentViewModel.Student.StudentID;
                        _studentInvoiceAddressRepository.CreateStudentInvoiceAddress(studentViewModel.StudentInvoiceAddress);
                    }
                    else
                    {
                        _studentInvoiceAddressRepository.UpdateStudentInvoiceAddress(studentViewModel.StudentInvoiceAddress);
                    }
                }

                //if (!studentViewModel.StudentNote.isEmpty)
                //{
                if (studentViewModel.StudentNote.StudentNoteID == 0)
                {
                    studentViewModel.StudentNote.StudentID = studentViewModel.Student.StudentID;
                    _studentNoteRepository.CreateStudentNote(studentViewModel.StudentNote);
                }
                else
                {
                    _studentNoteRepository.UpdateStudentNote(studentViewModel.StudentNote);
                }
                //}

                ViewBag.IsSuccess = false;

                var period = studentViewModel.Period;
                bool isExist = _studentInstallmentRepository.ExistStudentInstallment(studentViewModel.Student.StudentID);
                if (isExist == true && studentViewModel.ViewIsFirst == true)
                {
                    AccounCodeDefaultCreate(period);
                    AccountCodeCreate(studentViewModel);
                    AccountingCreate(studentViewModel);
                }
            }

            if (school.NewPeriod != user.UserPeriod)
            {
                //New classroom record must be zero in Old 'Student' Table
                studentViewModel.Student.ClassroomID = 0;
                _studentRepository.UpdateStudent(studentViewModel.Student);
            }

            var studentDebt = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, studentViewModel.Student.StudentID);
            decimal totalAmount = studentDebt.Sum(p => p.UnitPrice);

            if (isSchoolChange) SchoolChange(studentViewModel);

            string url = null;
            if (studentViewModel.IsMenu == 9)
                url = "/M210Student/index?userID=" + studentViewModel.UserID;
            else
            if (isNew && totalAmount > 0)
            {
                url = "/ListPanel/List1000?userID=" + studentViewModel.UserID + "&studentID=" + studentViewModel.Student.StudentID + "&msg=0" + "&exitID=12" + "&receiptNo=' '" + "&paymentSW=0" + "&formTypeSW=1";
            }
            else
            {
                url = "/M210Student/index?userID=" + studentViewModel.UserID;
            }
            return Redirect(url);
        }

        public void SchoolChange(StudentViewModel studentViewModel)
        {
            var studentDebt = _studentDebtRepository.GetStudentDebtAll(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentDebt)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentDebtRepository.UpdateStudentDebt(item);
            }

            var studentDebtDetail = _studentDebtDetailRepository.GetStudentDebtDetailAllSchool3(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentDebtDetail)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentDebtDetailRepository.UpdateStudentDebtDetail(item);
            }

            var studentDebtDetailTable = _studentDebtDetailTableRepository.GetStudentDebtDetailTable(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentDebtDetailTable)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentDebtDetailTableRepository.UpdateStudentDebtDetailTable(item);
            }

            var studentDiscount = _studentDiscountRepository.GetDiscount2(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentDiscount)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentDiscountRepository.UpdateStudentDiscount(item);
            }

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallmentByPeriod(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentInstallment)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentInstallmentRepository.UpdateStudentInstallment(item);
            }

            var studentInstallmentPayment = _studentInstallmentPaymentRepository.GetStudentInstallmentPaymentByPeriod(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentInstallmentPayment)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentInstallmentPaymentRepository.UpdateStudentInstallmentPayment(item);
            }

            var studentInvoice = _studentInvoiceRepository.GetStudentInvoiceAll(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentInvoice)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentInvoiceRepository.UpdateStudentInvoice(item);
            }

            var studentInvoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetail3(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentInvoiceDetail)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentInvoiceDetailRepository.UpdateStudentInvoiceDetail(item);
            }

            var studentPayment = _studentPaymentRepository.GetStudentPayment(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentPayment)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentPaymentRepository.UpdateStudentPayment(item);
            }

            var studentPeriods = _studentPeriodsRepository.GetStudentPeriod2(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentPeriods)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentPeriodsRepository.UpdateStudentPeriods(item);
            }

            var studentTaskDataSource = _studentTaskDataSourceRepository.GetStudentTaskAll2(studentViewModel.Student.SchoolID, studentViewModel.Student.StudentID);
            foreach (var item in studentTaskDataSource)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentTaskDataSourceRepository.UpdateStudentTask(item);
            }

            var studentTemp = _studentTempRepository.GetStudentTempByPeriod(studentViewModel.Student.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
            foreach (var item in studentTemp)
            {
                item.SchoolID = (studentViewModel.Student.SchoolID);
                _studentTempRepository.UpdateStudentTemp(item);
            }
        }


        [Route("M210Student/GetAddress/{studentID}/{addressID}")]
        public IActionResult GetAddress(int studentID, string addressID)
        {
            var getAddress = string.Empty;
            var getCityID = 0;
            var getTownID = 0;
            var getZip = string.Empty;

            var getParentName = string.Empty;
            var getParentID = string.Empty;
            var getParentHomePhone = string.Empty;
            var getParentWorkPhone = string.Empty;
            var getParentMobilePhone = string.Empty;
            var getParentGenderType = 0;
            var getParentKinshipDropDown = 0;
            var getParentProfessionDropDown = 0;
            var getParentEMail = string.Empty;

            var getDebtorName = string.Empty;
            var getDebtorPlaceOfBirth = string.Empty;
            //DateTime getDebtorDOB = DateTime.Now;
            var getDebtorFatherName = string.Empty;

            var getFatherName = string.Empty;
            var getFatherID = string.Empty;
            var getFatherHomePhone = string.Empty;
            var getFatherWorkPhone = string.Empty;
            var getFatherMobilePhone = string.Empty;
            var getFatherProfessionDropDown = 0;
            var getFatherEMail = string.Empty;

            var getMotherName = string.Empty;
            var getMotherID = string.Empty;
            var getMotherHomePhone = string.Empty;
            var getMotherWorkPhone = string.Empty;
            var getMotherMobilePhone = string.Empty;
            var getMotherProfessionDropDown = 0;
            var getMotherEMail = string.Empty;

            var getInvoiceTitle = string.Empty;
            var getInvoiceCountry = string.Empty;
            var getInvoiceTaxOffice = string.Empty;
            var getInvoiceTaxNumber = string.Empty;
            var getInvoicePhone = string.Empty;
            var getInvoiceFax = string.Empty;
            var getInvoiceEMail = string.Empty;
            var getInvoiceWebAddress = string.Empty;

            var student = _studentRepository.GetStudent(studentID);
            var studentAddress = _studentAddressRepository.GetStudentAddress(studentID);
            var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(studentID);
            var familyAddress = _studentFamilyAddressRepository.GetStudentFamilyAddress(studentID);
            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);


            //var getDebtorDOB = parentAddress.DebtorDOB.Value;
            if (addressID == "Student1" && studentAddress != null)
            {
                getAddress = studentAddress.Address1;
                getCityID = studentAddress.CityParameterID1;
                getTownID = studentAddress.TownParameterID1;
                getZip = studentAddress.ZipCode1;
            }
            if (addressID == "Student2" && studentAddress != null)
            {
                getAddress = studentAddress.Address2;
                getCityID = studentAddress.CityParameterID2;
                getTownID = studentAddress.TownParameterID2;
                getZip = studentAddress.ZipCode2;
            }

            if (addressID == "ParentHome" && parentAddress != null)
            {
                getAddress = parentAddress.HomeAddress;
                getCityID = parentAddress.HomeCityParameterID;
                getTownID = parentAddress.HomeTownParameterID;
                getZip = parentAddress.HomeZipCode;

                getParentName = student.ParentName;
                getParentID = parentAddress.IdNumber;
                getParentHomePhone = parentAddress.HomePhone;
                getParentWorkPhone = parentAddress.WorkPhone;
                getParentGenderType = parentAddress.ParentGenderTypeCategoryID;
                getParentMobilePhone = parentAddress.MobilePhone;
                getParentKinshipDropDown = parentAddress.KinshipCategoryID;
                getParentProfessionDropDown = parentAddress.ProfessionCategoryID;
                getParentEMail = parentAddress.EMail;
            }
            if (addressID == "ParentWork" && parentAddress != null)
            {
                getAddress = parentAddress.WorkAddress;
                getCityID = parentAddress.WorkCityParameterID;
                getTownID = parentAddress.WorkTownParameterID;
                getZip = parentAddress.WorkZipCode;
            }
            if (addressID == "Parent3Address" && parentAddress != null)
            {
                getAddress = parentAddress.HomeAddress;
                getCityID = parentAddress.HomeCityParameterID;
                getTownID = parentAddress.HomeTownParameterID;
                getZip = parentAddress.HomeZipCode;

                getParentName = parentAddress.Name3;
                getParentID = parentAddress.IdNumber3;
                getParentHomePhone = parentAddress.HomePhone3;
                getParentWorkPhone = parentAddress.WorkPhone3;
                getParentMobilePhone = parentAddress.MobilePhone3;
            }

            var getDebtorDOB = DateTime.Now;
            if (addressID == "DebtorHome" && parentAddress != null)
            {
                getAddress = parentAddress.DebtorAddress;
                getCityID = parentAddress.DebtorCityID;
                getTownID = parentAddress.DebtorTownID;
                getZip = parentAddress.DebtorZipCode;

                getDebtorName = parentAddress.DebtorName;
                getDebtorPlaceOfBirth = parentAddress.DebtorPlaceOfBirth;

                DateTime date = Convert.ToDateTime(parentAddress.DebtorDOB);
                getDebtorDOB = date;

                getDebtorDOB = (DateTime)parentAddress.DebtorDOB;

                getDebtorFatherName = parentAddress.DebtorFatherName;
                getParentHomePhone = parentAddress.DebtorHomePhone;
                getParentWorkPhone = parentAddress.DebtorWorkPhone;
                getParentMobilePhone = parentAddress.DebtorMobilePhone;
            }

            if (addressID == "FatherHome" && familyAddress != null)
            {
                getAddress = familyAddress.FatherHomeAddress;
                getCityID = familyAddress.FatherHomeCityParameterID;
                getTownID = familyAddress.FatherHomeTownParameterID;
                getZip = familyAddress.FatherHomeZipCode;

                getFatherName = familyAddress.FatherName;
                getFatherID = familyAddress.FatherIdNumber;
                getFatherHomePhone = familyAddress.FatherHomePhone;
                getFatherWorkPhone = familyAddress.FatherWorkPhone;
                getFatherMobilePhone = familyAddress.FatherMobilePhone;
                getFatherProfessionDropDown = familyAddress.FatherProfessionCategoryID;
                getFatherEMail = familyAddress.FatherEMail;
            }
            if (addressID == "FatherWork" && familyAddress != null)
            {
                getAddress = familyAddress.FatherWorkAddress;
                getCityID = familyAddress.FatherWorkCityParameterID;
                getTownID = familyAddress.FatherWorkTownParameterID;
                getZip = familyAddress.FatherWorkZipCode;
            }

            if (addressID == "MotherHome" && familyAddress != null)
            {
                getAddress = familyAddress.MotherHomeAddress;
                getCityID = familyAddress.MotherHomeCityParameterID;
                getTownID = familyAddress.MotherHomeTownParameterID;
                getZip = familyAddress.MotherHomeZipCode;

                getMotherName = familyAddress.MotherName;
                getMotherID = familyAddress.MotherIdNumber;
                getMotherHomePhone = familyAddress.MotherHomePhone;
                getMotherWorkPhone = familyAddress.MotherWorkPhone;
                getMotherMobilePhone = familyAddress.MotherMobilePhone;
                getMotherProfessionDropDown = familyAddress.MotherProfessionCategoryID;
                getMotherEMail = familyAddress.MotherEMail;
            }
            if (addressID == "MotherWork" && familyAddress != null)
            {
                getAddress = familyAddress.MotherWorkAddress;
                getCityID = familyAddress.MotherWorkCityParameterID;
                getTownID = familyAddress.MotherWorkTownParameterID;
                getZip = familyAddress.MotherWorkZipCode;
            }

            if (addressID == "invoiceAddress" && familyAddress != null)
            {
                getAddress = invoiceAddress.InvoiceAddress;
                getCityID = invoiceAddress.InvoiceCityParameterID;
                getTownID = invoiceAddress.InvoiceTownParameterID;
                getZip = invoiceAddress.InvoiceZipCode;

                getInvoiceTitle = invoiceAddress.InvoiceTitle;
                getInvoiceCountry = invoiceAddress.InvoiceCountry;
                getInvoiceTaxOffice = invoiceAddress.InvoiceTaxOffice;
                getInvoiceTaxNumber = invoiceAddress.InvoiceTaxNumber;
                getInvoicePhone = invoiceAddress.Phone;
                getInvoiceFax = invoiceAddress.Fax;
                getInvoiceEMail = invoiceAddress.EMail;
                getInvoiceWebAddress = invoiceAddress.WebAddress;
            }

            return Json(new
            {
                address = getAddress,
                city = getCityID,
                town = getTownID,
                zip = getZip,

                name = getParentName,
                id = getParentID,
                homePhone = getParentHomePhone,
                workPhone = getParentWorkPhone,
                mobilePhone = getParentMobilePhone,
                eMail = getParentEMail,
                genderType = getParentGenderType,
                kinship = getParentKinshipDropDown,
                profession = getParentProfessionDropDown,

                debtorName = getDebtorName,
                debtorPlaceOfBirth = getDebtorPlaceOfBirth,
                debtorDOB = getDebtorDOB,
                debtorFatherName = getDebtorFatherName,

                fatherName = getFatherName,
                fatherID = getFatherID,
                fatherHomePhone = getFatherHomePhone,
                fatherWorkPhone = getFatherWorkPhone,
                fatherMobilePhone = getFatherMobilePhone,
                fatherProfession = getFatherProfessionDropDown,
                fatherEMail = getFatherEMail,

                motherName = getMotherName,
                motherID = getMotherID,
                motherHomePhone = getMotherHomePhone,
                motherWorkPhone = getMotherWorkPhone,
                motherMobilePhone = getMotherMobilePhone,
                motherProfession = getMotherProfessionDropDown,
                motherEMail = getMotherEMail,

                invoiceTitle = getInvoiceTitle,
                invoiceCountry = getInvoiceCountry,
                invoiceTaxOffice = getInvoiceTaxOffice,
                invoiceTaxNumber = getInvoiceTaxNumber,
                invoicePhone = getInvoicePhone,
                invoiceFax = getInvoiceFax,
                invoiceEMail = getInvoiceEMail,
                invoiceWebAddress = getInvoiceWebAddress,
            });
        }


        [HttpPost]
        [Route("M210Student/SchoolFeeDataTableRead/{period}/{userID}/{classroomID}/{feeID}/{ID}/{unitPrice}")]
        public IActionResult SchoolFeeDataTableRead(string period, int userID, int classroomID, int feeID, int ID, decimal unitPrice)
        {
            var user = _usersRepository.GetUser(userID);

            var classroom = new Classroom();
            bool isExpand = false;
            if (classroomID != 0)
            {
                classroom = _classroomRepository.GetClassroomID(classroomID);

                var amount = _schoolFeeTableRepository.GetSchoolFees(period, classroom.ClassroomTypeID, feeID, 1); // (1) Fees category (2) Services category
                if (amount == null) return Json(new { SchoolFeeTypeAmount = 0, UnitPrice = unitPrice });

                var expandControl = _schoolFeeRepository.GetSchoolFeeSubControl(user.SchoolID, feeID, "L2");
                if (expandControl.Count() > 0) isExpand = true;

                return Json(new { ClassroomTypeID = classroom.ClassroomTypeID, SchoolFeeTypeAmount = amount.SchoolFeeTypeAmount, UnitPrice = unitPrice, IsExpand = isExpand });
            }
            else
                return Json(new { ClassroomTypeID = classroom.ClassroomTypeID, SchoolFeeTypeAmount = 0, UnitPrice = 0, IsExpand = isExpand });
        }


        #endregion

        #region Debt 

        [Route("M210Student/SchoolDebtDataRead/{period}/{userID}/{studentid}")]
        public IActionResult SchoolDebtDataRead(string period, int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);

            List<StudentDebtViewModel> list = new List<StudentDebtViewModel>();
            var student = _studentRepository.GetStudent(studentid);

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");
            var typeId = 0;
            if (student.ClassroomID > 0)
                typeId = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomTypeID;

            Boolean isExist = _studentDebtRepository.ExistStudentDebt(user.UserPeriod, studentid);
            if (isExist)
            {
                var debtControl = _studentDebtRepository.GetStudentDebtAll2(period, studentid);
                decimal debtTotal = debtControl.Sum(c => c.UnitPrice);
                if (debtTotal == 0)
                {
                    foreach (var item in debtControl)
                    {
                        _studentDebtRepository.DeleteStudentDebt(item);
                    }
                }
                isExist = _studentDebtRepository.ExistStudentDebt(user.UserPeriod, studentid);
            }
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

                studentDebtViewModel.FeeName = item.Name;
                if (user.SelectedCulture.Trim() == "en-US") studentDebtViewModel.FeeName = item.Language1;
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

            return Json(list);
        }


        [HttpPost]
        [Route("M210Student/SchoolDebtDataUpdate/{strResult}/{userID}/{studentID}/{classroomID}")]
        public IActionResult SchoolDebtDataUpdate([Bind(Prefix = "models")] string strResult, int userID, int studentID, int classroomID)
        {

            var json = new JavaScriptSerializer().Deserialize<List<StudentDebtViewModel>>(strResult);
            List<StudentDebt> list = new List<StudentDebt>();

            var user = _usersRepository.GetUser(userID);
            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);

            var typeId = 0;
            if (classroomID > 0)
                typeId = _classroomRepository.GetClassroomID(classroomID).ClassroomTypeID;

            var i = 0;
            foreach (var item in json)
            {
                if (json[i].StudentDebtID > 0)
                {
                    var getCode = _studentDebtRepository.GetStudentDebtID((int)json[i].SchoolID, json[i].StudentDebtID);

                    if (getCode == null)
                    {
                        if (ModelState.IsValid)
                        {
                            getCode = new StudentDebt();
                            getCode.StudentDebtID = json[i].StudentDebtID;
                            getCode.SchoolID = json[i].SchoolID;
                            getCode.StudentID = json[i].StudentID;
                            getCode.SchoolFeeID = json[i].SchoolFeeID;
                            getCode.ClassroomTypeID = typeId;
                            getCode.Period = json[i].Period;

                            getCode.UnitPrice = json[i].UnitPrice;
                            getCode.Discount = json[i].Discount;
                            getCode.Amount = json[i].Amount;
                            if (json[i].UnitPrice > 0 && json[i].Amount == 0)
                            {
                                getCode.Amount = getCode.UnitPrice - getCode.Discount;
                            }

                            getCode.IsList = json[i].IsList;
                            list.Add(getCode);
                            _studentDebtRepository.CreateStudentDebt(getCode);

                            studentTemp.IsPension = false;
                            if (json[i].FeeName == "Pansiyon Ücreti" || json[i].FeeName == "Hostel Fee")
                                if (json[i].Amount > 0) studentTemp.IsPension = true;
                        }
                    }
                    else
                    if (ModelState.IsValid)
                    {
                        getCode.StudentDebtID = json[i].StudentDebtID;
                        getCode.UnitPrice = json[i].UnitPrice;
                        getCode.Discount = json[i].Discount;
                        getCode.Amount = json[i].Amount;
                        getCode.ClassroomTypeID = typeId;
                        if (json[i].UnitPrice > 0 && json[i].Amount == 0)
                        {
                            getCode.Amount = getCode.UnitPrice - getCode.Discount;
                        }
                        else getCode.Amount = 0;

                        getCode.IsList = json[i].IsList;

                        _studentDebtRepository.UpdateStudentDebt(getCode);

                        studentTemp.IsPension = false;
                        if (json[i].FeeName == "Pansiyon Ücreti" || json[i].FeeName == "Hostel Fee")
                            if (json[i].Amount > 0) studentTemp.IsPension = true;
                    }
                    i = i + 1;
                }
            }

            _studentTempRepository.UpdateStudentTemp(studentTemp);
            return Json(list);
        }
        [HttpPost]
        [Route("M210Student/GridFeesUpdate/{userID}/{studentID}/{classroomID}/{schoolFeeID}/{unitPrice}/{discount}/{isList}/{isPriceUpdate}/{isListUpdate}")]
        public IActionResult GridFeesUpdate(int userID, int studentID, int classroomID, int schoolFeeID, decimal unitPrice, decimal discount, bool isList, bool isPriceUpdate, bool isListUpdate)
        {
            var user = _usersRepository.GetUser(userID);
            var fee = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, schoolFeeID);
            var typeId = 0;
            if (classroomID > 0)
                typeId = _classroomRepository.GetClassroomID(classroomID).ClassroomTypeID;

            fee.SchoolID = user.SchoolID;
            fee.StudentID = studentID;
            fee.ClassroomTypeID = typeId;
            fee.SchoolFeeID = schoolFeeID;
            fee.Period = user.UserPeriod;

            if (isPriceUpdate)
            {
                fee.UnitPrice = unitPrice;
                fee.Discount = discount;
                fee.Amount = unitPrice - discount;
            }
            if (isListUpdate)
            {
                fee.IsList = isList;
            }
            _studentDebtRepository.UpdateStudentDebt(fee);
            return Json(true);
        }

        [Route("M210Student/SchoolDebtDetailedDataRead/{userID}/{studentID}")]
        public async Task<IActionResult> SchoolDebtDetailedDataRead(int userID, int studentID)
        {
            await Task.Delay(200);

            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            List<StudentDebtDetailedViewModel> list = new List<StudentDebtDetailedViewModel>();

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            Boolean isExist = _studentDebtDetailTableRepository.ExistStudentDebtDetailTable(studentID);
            if (!isExist)
                foreach (var item in fee)
                {
                    var debt = _studentDebtRepository.GetStudentDebt22(period, user.SchoolID, studentID, item.SchoolFeeID);
                    if (debt == null)
                        debt = new StudentDebt();
                    else
                    if (debt.Amount > 0)
                    {
                        var getCode = new StudentDebtDetailTable();
                        getCode.StudentID = studentID;
                        getCode.SchoolID = user.SchoolID;
                        getCode.SchoolFeeID = item.SchoolFeeID;
                        getCode.Period = period;
                        getCode.AmountTable = debt.Amount;
                        getCode.InstallmentTable = 1;
                        getCode.PaymentStartDateTable = DateTime.Now;

                        _studentDebtDetailTableRepository.CreateStudentDebtDetailTable(getCode);
                    }
                }

            var detail = _studentDebtDetailTableRepository.GetStudentDebtDetailTable(user.SchoolID, period, studentID);
            foreach (var item in detail)
            {
                var studentDebtDetailedViewModel = new StudentDebtDetailedViewModel();

                var feeTable = _schoolFeeRepository.GetSchoolFee(item.SchoolFeeID);

                studentDebtDetailedViewModel.ViewModelId = studentID;
                studentDebtDetailedViewModel.StudentID = studentID;

                studentDebtDetailedViewModel.FeeName = feeTable.Name;
                if (user.SelectedCulture.Trim() == "en-US") studentDebtDetailedViewModel.FeeName = feeTable.Language1;

                studentDebtDetailedViewModel.SchoolFeeID = item.SchoolFeeID;
                studentDebtDetailedViewModel.AmountTable = item.AmountTable;
                studentDebtDetailedViewModel.InstallmentTable = 1;
                studentDebtDetailedViewModel.PaymentStartDateTable = DateTime.Now;
                if (detail != null)
                {
                    studentDebtDetailedViewModel.StudentDebtTableID = item.StudentDebtTableID;
                    studentDebtDetailedViewModel.SchoolID = item.SchoolID;
                    studentDebtDetailedViewModel.Period = item.Period;
                    studentDebtDetailedViewModel.AmountTable = item.AmountTable;
                    studentDebtDetailedViewModel.InstallmentTable = item.InstallmentTable;
                    studentDebtDetailedViewModel.PaymentStartDateTable = item.PaymentStartDateTable;
                }

                list.Add(studentDebtDetailedViewModel);
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult SchoolDebtDetailedDataUpdate([Bind(Prefix = "models")] string strResult)
        {

            var json = new JavaScriptSerializer().Deserialize<List<StudentDebtDetailedViewModel>>(strResult);
            List<StudentDebtDetailTable> list = new List<StudentDebtDetailTable>();

            var i = 0;
            foreach (var item in json)
            {
                var getCode = _studentDebtDetailTableRepository.GetStudentDebtDetailTableID(json[i].StudentDebtTableID);

                if (getCode == null)
                {
                    if (ModelState.IsValid)
                    {
                        getCode = new StudentDebtDetailTable();
                        getCode.StudentDebtTableID = json[i].StudentDebtTableID;
                        getCode.SchoolID = json[i].SchoolID;
                        getCode.StudentID = json[i].StudentID;
                        getCode.SchoolFeeID = json[i].SchoolFeeID;
                        getCode.Period = json[i].Period;

                        getCode.AmountTable = json[i].AmountTable;
                        getCode.InstallmentTable = json[i].InstallmentTable;
                        getCode.PaymentStartDateTable = json[i].PaymentStartDateTable;
                        list.Add(getCode);
                        _studentDebtDetailTableRepository.CreateStudentDebtDetailTable(getCode);
                    }
                }
                else
                if (ModelState.IsValid)
                {
                    getCode.StudentDebtTableID = json[i].StudentDebtTableID;
                    getCode.InstallmentTable = json[i].InstallmentTable;
                    getCode.PaymentStartDateTable = json[i].PaymentStartDateTable;

                    _studentDebtDetailTableRepository.UpdateStudentDebtDetailTable(getCode);
                }
                i = i + 1;
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M210Student/DetailedRefreshDataRead/{userID}/{studentID}")]
        public IActionResult DetailedRefreshDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<StudentDebtDetailedViewModel> list = new List<StudentDebtDetailedViewModel>();

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            var detailTable = _studentDebtDetailTableRepository.GetStudentDebtDetailTable(user.SchoolID, period, studentID);
            foreach (var item in detailTable)
            {
                _studentDebtDetailTableRepository.DeleteStudentDebtDetailTable(item);
            }

            foreach (var item in fee)
            {
                var debt = _studentDebtRepository.GetStudentDebt22(period, user.SchoolID, studentID, item.SchoolFeeID);
                if (debt.Amount > 0)
                {
                    var getCode = new StudentDebtDetailTable();
                    getCode.StudentID = studentID;
                    getCode.SchoolID = user.SchoolID;
                    getCode.SchoolFeeID = item.SchoolFeeID;
                    getCode.Period = period;
                    getCode.AmountTable = debt.Amount;
                    getCode.InstallmentTable = 1;
                    getCode.PaymentStartDateTable = DateTime.Now;

                    _studentDebtDetailTableRepository.CreateStudentDebtDetailTable(getCode);
                    _studentDebtDetailTableRepository.Save();
                }
            }

            foreach (var item in fee)
            {
                var studentDebtDetailedViewModel = new StudentDebtDetailedViewModel();
                var debt = _studentDebtRepository.GetStudentDebt22(period, user.SchoolID, studentID, item.SchoolFeeID);
                var detail = _studentDebtDetailTableRepository.GetStudentDebtDetailTable2(item.SchoolFeeID, studentID, period);
                if (debt.Amount > 0)
                {
                    studentDebtDetailedViewModel.ViewModelId = studentID;
                    studentDebtDetailedViewModel.StudentID = studentID;

                    studentDebtDetailedViewModel.FeeName = item.Name;
                    if (user.SelectedCulture.Trim() == "en-US") studentDebtDetailedViewModel.FeeName = item.Language1;

                    studentDebtDetailedViewModel.SchoolFeeID = item.SchoolFeeID;
                    studentDebtDetailedViewModel.AmountTable = debt.Amount;
                    studentDebtDetailedViewModel.InstallmentTable = school.DefaultInstallment;
                    studentDebtDetailedViewModel.PaymentStartDateTable = DateTime.Now;
                    if (detail != null)
                    {
                        studentDebtDetailedViewModel.StudentDebtTableID = detail.StudentDebtTableID;
                        studentDebtDetailedViewModel.SchoolID = detail.SchoolID;
                        studentDebtDetailedViewModel.Period = detail.Period;
                        studentDebtDetailedViewModel.AmountTable = detail.AmountTable;
                        studentDebtDetailedViewModel.InstallmentTable = detail.InstallmentTable;
                        studentDebtDetailedViewModel.PaymentStartDateTable = detail.PaymentStartDateTable;
                    }
                    list.Add(studentDebtDetailedViewModel);
                }
            }
            return Json(list);
        }

        #endregion

        #region DetailFee 

        [Route("M210Student/SchoolFeeDetailRead/{userID}/{schoolFeeID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeDetailRead(int userID, int schoolFeeID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(user.SchoolID, L).Where(b => b.FeeCategory == 1); ;
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID20(user.SchoolID, period, schoolFeeID, categoryID).Where(b => b.FeeCategory == 1);

            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            foreach (var item in schoolFeeTable)
            {
                var schoolFee = new SchoolFee();
                schoolFee = schoolFeeList.FirstOrDefault(p => p.SchoolFeeID == item.SchoolFeeID);
                string categoryName = schoolFee.Name;
                if (user.SelectedCulture.Trim() == "en-US")
                {
                    categoryName = schoolFee.Language1;
                }

                var schoolFeeViewModel = new SchoolFeeViewModel
                {
                    ViewModelID = item.SchoolFeeID,
                    SchoolFeeID = item.SchoolFeeID,
                    FeeCategory = item.FeeCategory,
                    CategorySubID = item.SchoolFeeID,
                    SchoolFeeName = categoryName,
                    SchoolFeeTypeAmount = (decimal)item.SchoolFeeTypeAmount,
                    StockQuantity = (int)item.StockQuantity,
                    StockCode = item.StockCode,
                    Tax = schoolFee.Tax,

                    SortOrder = (int)item.SortOrder,
                    IsActive = item.IsActive,

                };

                list.Add(schoolFeeViewModel);
            }
            return Json(list);
        }

        [Route("M210Student/SchoolFeeMoreDetailRead1/{userID}/{schoolFeeID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeMoreDetailRead1(int userID, int schoolFeeID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            //var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel3(user.SchoolID, schoolFeeID, L);
            //var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, period, schoolFeeID);
            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(user.SchoolID, L).Where(b => b.FeeCategory == 1); ;
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID20(user.SchoolID, period, schoolFeeID, categoryID).Where(b => b.FeeCategory == 1);

            //var schoolFeeTableDetail = schoolFeeTable.Where(b => schoolFeeList.Where(c => b.SchoolFeeID == schoolFeeID).Count() > 0).ToList();

            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            foreach (var item in schoolFeeTable)
            {
                var schoolFee = new SchoolFee();
                //if (L == "L2")
                //    schoolFee = schoolFeeList.FirstOrDefault(p => p.CategorySubID == item.SchoolFeeID);
                //else
                schoolFee = schoolFeeList.FirstOrDefault(p => p.SchoolFeeID == item.CategoryID);

                string categoryName = schoolFee.Name;
                if (user.SelectedCulture.Trim() == "en-US")
                {
                    categoryName = schoolFee.Language1;
                }
                var schoolFeeViewModel = new SchoolFeeViewModel
                {
                    ViewModelID = item.CategoryID,
                    SchoolFeeID = item.CategoryID,
                    //SchoolFeeID = item.SchoolFeeID,

                    //Create runnning
                    FeeCategory = (int)item.FeeCategory,
                    SchoolFeeName = categoryName,
                    SchoolFeeTypeAmount = (int)item.SchoolFeeTypeAmount,
                    StockQuantity = 1,
                    StockCode = item.StockCode,
                    Tax = schoolFee.Tax,
                    SchoolID = item.SchoolID,
                    Period = item.Period,
                    //StudentDebtID = _studentDebtRepository.GetStudentDebt4(user.UserPeriod, item.SchoolID, studentID, item.SchoolFeeID).StudentDebtID,
                    StudentDebtID = item.SchoolFeeID,

                    SortOrder = (int)item.SortOrder,
                    IsActive = item.IsActive,
                    IsSelect = false,
                    /////////////////

                    SchoolFeeTable = item,
                    SchoolFee = schoolFee,
                };

                list.Add(schoolFeeViewModel);
            }
            return Json(list);
        }

        [Route("M210Student/SchoolFeeMoreDetailRead2/{userID}/{schoolFeeID}/{studentDebtID}/{L}")]
        public IActionResult SchoolFeeMoreDetailRead2(int userID, int schoolFeeID, int studentDebtID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel2(user.SchoolID, schoolFeeID);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID3(user.SchoolID, period);

            var schoolFeeTableDetail = schoolFeeTable.Where(b => schoolFeeList.Where(c => c.SchoolFeeID == b.CategoryID).Count() > 0).ToList();

            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            foreach (var item in schoolFeeTableDetail)
            {
                var schoolFee = new SchoolFee();
                //if (L == "L2")
                //    schoolFee = schoolFeeList.FirstOrDefault(p => p.CategorySubID == item.SchoolFeeID);
                //else
                schoolFee = schoolFeeList.FirstOrDefault(p => p.SchoolFeeID == item.CategoryID);

                string categoryName = schoolFee.Name;
                if (user.SelectedCulture.Trim() == "en-US")
                {
                    categoryName = schoolFee.Language1;
                }

                var schoolFeeViewModel = new SchoolFeeViewModel
                {
                    ViewModelID = item.CategoryID,
                    SchoolFeeID = item.CategoryID,
                    //SchoolFeeID = item.SchoolFeeID,

                    //Create runnning
                    FeeCategory = (int)item.FeeCategory,
                    SchoolFeeName = categoryName,
                    SchoolFeeTypeAmount = (int)item.SchoolFeeTypeAmount,
                    StockQuantity = 1,
                    StockCode = item.StockCode,
                    Tax = schoolFee.Tax,
                    SchoolID = item.SchoolID,
                    Period = item.Period,
                    StudentDebtID = item.SchoolFeeID,

                    SortOrder = (int)item.SortOrder,
                    IsActive = item.IsActive,
                    IsSelect = false,
                    /////////////////

                    SchoolFeeTable = item,
                    SchoolFee = schoolFee,
                };

                list.Add(schoolFeeViewModel);
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M210Student/SchoolFeeMoreDetailUpdate/{strResult}/{userID}/{L}/{schoolFeeID}/{schoolFeeIDMore}/{categoryID}/{studentID}")]
        public IActionResult SchoolFeeMoreDetailUpdate([Bind(Prefix = "models")] string strResult, int userID, string L, int schoolFeeID, int schoolFeeIDMore, int categoryID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);

            decimal total = 0;
            var i = 0;
            foreach (var item in json)
            {
                var studentDebtDetail = _studentDebtDetailRepository.GetStudentDebtDetailID(json[i].SchoolID, studentID, json[i].SchoolFeeID, json[i].StudentDebtID);
                if (studentDebtDetail == null)
                {
                    studentDebtDetail = new StudentDebtDetail();
                    studentDebtDetail.StudentDebtDetailID = 0;
                    studentDebtDetail.Period = user.UserPeriod;
                    studentDebtDetail.SchoolID = user.SchoolID;
                    studentDebtDetail.StudentID = studentID;
                    studentDebtDetail.CategoryID = categoryID;
                    studentDebtDetail.SchoolFeeID = json[i].SchoolFeeID;
                    studentDebtDetail.StudentDebtID = json[i].StudentDebtID;
                    studentDebtDetail.Amount = json[i].SchoolFeeTypeAmount;
                    studentDebtDetail.StockQuantity = json[i].StockQuantity;
                    studentDebtDetail.StockCode = json[i].StockCode;
                    _studentDebtDetailRepository.CreateStudentDebtDetail(studentDebtDetail);
                }
                else
                {
                    studentDebtDetail.Period = user.UserPassword;
                    studentDebtDetail.SchoolID = user.SchoolID;
                    studentDebtDetail.StudentID = studentID;
                    studentDebtDetail.CategoryID = categoryID;
                    studentDebtDetail.SchoolFeeID = json[i].SchoolFeeID;
                    studentDebtDetail.StudentDebtID = json[i].StudentDebtID;
                    studentDebtDetail.Amount = json[i].SchoolFeeTypeAmount;
                    studentDebtDetail.StockQuantity = json[i].StockQuantity;
                    studentDebtDetail.StockCode = json[i].StockCode;
                    _studentDebtDetailRepository.UpdateStudentDebtDetail(studentDebtDetail);
                }

                total += json[i].SchoolFeeTypeAmount * json[i].StockQuantity;
                i = i + 1;
            }

            var ID = 0;
            if (L == "L2") ID = schoolFeeID;
            else ID = schoolFeeIDMore;
            var studentDebt = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, ID);
            if (studentDebt == null)
            {
                studentDebt = new StudentDebt();
                studentDebt.StudentDebtID = 0;
                studentDebt.SchoolID = json[0].SchoolID;
                studentDebt.StudentID = studentID;
                if (L == "L2")
                    studentDebt.SchoolFeeID = schoolFeeID;
                else studentDebt.SchoolFeeID = schoolFeeIDMore;
                studentDebt.ClassroomTypeID = categoryID;
                studentDebt.Period = json[0].Period;
                studentDebt.UnitPrice = total;
                studentDebt.Discount = 0;
                studentDebt.Amount = total;
                studentDebt.IsList = true;

                _studentDebtRepository.CreateStudentDebt(studentDebt);
            }
            else
            {
                studentDebt.StudentID = studentID;
                studentDebt.UnitPrice = total;
                studentDebt.Discount = 0;
                studentDebt.Amount = total;
                studentDebt.IsList = true;
                _studentDebtRepository.UpdateStudentDebt(studentDebt);
            }
            //mehmet
            return Json(true);
        }
        #endregion

        #region Discount
        [HttpPost]
        [Route("M210Student/DiscountDataRead/{strResult}/{userID}/{studentid}")]
        public IActionResult DiscountDataRead(int strResult, int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var discounttable = _discountTableRepository.GetDiscountTableIDPeriod(user.SchoolID, period);
            string feeName = "";
            if (strResult != 0)
            {
                var debt = _studentDebtRepository.GetStudentDebtID(user.SchoolID, strResult);
                var schoolFee = _schoolFeeRepository.GetSchoolFee(debt.SchoolFeeID);
                feeName = schoolFee.Name;
                if (user.SelectedCulture.Trim() == "en-US") feeName = schoolFee.Language1;
            }

            List<StudentDiscountViewModel> list = new List<StudentDiscountViewModel>();

            foreach (var item in discounttable)
            {
                var studentDiscountViewModel = new StudentDiscountViewModel();
                var dis = _studentDiscountRepository.GetDiscount(studentid, period, item.SchoolID, item.DiscountTableID, strResult);


                studentDiscountViewModel.StudentID = studentid;
                studentDiscountViewModel.SchoolID = item.SchoolID;
                studentDiscountViewModel.StudentDebtID = 0;
                studentDiscountViewModel.DiscountTableID = item.DiscountTableID;
                studentDiscountViewModel.SchoolID = item.SchoolID;

                studentDiscountViewModel.Period = item.Period;
                studentDiscountViewModel.DiscountName = item.DiscountName;
                if (user.SelectedCulture.Trim() == "en-US") studentDiscountViewModel.DiscountName = item.Language1;

                studentDiscountViewModel.DiscountPercent = item.DiscountPercent;
                studentDiscountViewModel.DiscountAmount = item.DiscountAmount;

                if (dis != null)
                {
                    studentDiscountViewModel.StudentDiscountID = dis.StudentDiscountID;
                    studentDiscountViewModel.DiscountTotal = dis.DiscountTotal;
                    studentDiscountViewModel.DiscountApplied = dis.DiscountApplied;
                }
                else
                {
                    studentDiscountViewModel.DiscountTotal = 0;
                    studentDiscountViewModel.DiscountApplied = 0;
                }

                studentDiscountViewModel.DiscountFeeName = feeName;
                list.Add(studentDiscountViewModel);
            }
            return Json(list);
        }

        [Route("M210Student/DiscountDataUpdate/{strResult}/{unitPrice}")]
        public IActionResult DiscountDataUpdate([Bind(Prefix = "models")] string strResult, decimal unitPrice)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentDiscountViewModel>>(strResult);
            List<StudentDiscount> list = new List<StudentDiscount>();

            var i = 0;
            decimal total = 0;
            var student = _studentRepository.GetStudent(json[i].StudentID);
            var typeId = 0;
            if (student.ClassroomID > 0)
                typeId = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomTypeID;
            foreach (var item in json)
            {
                var studentDiscount = new StudentDiscount();
                studentDiscount.StudentDiscountID = json[i].StudentDiscountID;
                studentDiscount.StudentID = json[i].StudentID;
                studentDiscount.SchoolID = json[i].SchoolID;
                studentDiscount.StudentDebtID = json[i].StudentDebtID;
                studentDiscount.DiscountTableID = json[i].DiscountTableID;
                studentDiscount.Period = json[i].Period;
                studentDiscount.DiscountTotal = json[i].DiscountTotal;
                studentDiscount.DiscountApplied = json[i].DiscountApplied;
                studentDiscount.DiscountPercent = json[i].DiscountPercent;

                if (studentDiscount.StudentDiscountID == 0)
                {
                    {
                        _studentDiscountRepository.CreateStudentDiscount(studentDiscount);
                        _studentDiscountRepository.Save();
                    }
                }
                else
                {
                    {
                        _studentDiscountRepository.UpdateStudentDiscount(studentDiscount);
                        _studentDiscountRepository.Save();
                    }
                }

                i = i + 1;
            }

            var discount = _studentDiscountRepository.GetDiscountID(json[0].StudentDebtID);
            foreach (var item in discount)
            {
                total += item.DiscountApplied;
                //total += Convert.ToInt32(item.DiscountApplied);
            }

            var getCode = _studentDebtRepository.GetStudentDebtID(json[0].SchoolID, json[0].StudentDebtID);
            if (getCode != null)
            {
                getCode.UnitPrice = unitPrice;
                getCode.Amount = unitPrice - total;
                getCode.Discount = total;
                getCode.ClassroomTypeID = typeId;
                _studentDebtRepository.UpdateStudentDebt(getCode);
            }
            return Json(list);
        }

        #endregion

        #region Installment
        [Route("M210Student/InstallmentDataRead/{period}/{userID}/{studentid}")]
        public IActionResult InstallmentDataRead(string period, int userID, int studentid)
        {

            var user = _usersRepository.GetUser(userID);

            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameterList2 = _parameterRepository.GetParameterSubID(categoryID2);
            var categoryID3 = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var parameterList3 = _parameterRepository.GetParameterSubID(categoryID3);


            var bankList = _bankRepository.GetBankAll(user.SchoolID);
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentid, period);

            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in studentinstallment)
            {
                if (item.BankID == 0) item.BankID = null;
                var parameter2 = parameterList2.FirstOrDefault(p => p.CategoryID == item.CategoryID);
                var parameter3 = parameterList3.FirstOrDefault(p => p.CategoryID == item.StatusCategoryID);
                var bank = bankList.FirstOrDefault(p => p.BankID == item.BankID);
                if (item.BankID > 0 && bank == null)
                {
                    //Kurumun Bankası Yanlış aktarımdan dolayı sorunun çözümlenmesi için eklendi
                    var bankName = _bankRepository.GetBank((int)item.BankID).BankName;
                    bank = _bankRepository.GetBankName(user.SchoolID, bankName);
                }
                if (parameter2 == null)
                {
                    parameter2 = parameterList2.FirstOrDefault(p => p.CategorySubID == categoryID2);
                }
                if (parameter3 == null)
                {
                    parameter3 = parameterList3.FirstOrDefault(p => p.CategorySubID == categoryID3);
                }
                var studentViewModel = new StudentViewModel
                {
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

        [Route("M210Student/CalculateInstallment/{installment}/{singlepaper}/{cashpayment}/{subtotal}/{dateString}/{paymenttypecomboBox}/{banknamecomboBox}/{studentid}/{userID}/{byAmount}/{pressbuttonindex}")]
        public IActionResult CalculateInstallment(int installment, Boolean singlepaper, decimal cashpayment, decimal subtotal, string dateString, int paymenttypecomboBox, int banknamecomboBox, int studentid, int userID, decimal byAmount, int pressbuttonindex)
        {
            int maxIns = 0;
            DateTime transactiondate = DateTime.Parse(dateString);

            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var bank = _bankRepository.GetBank(banknamecomboBox);
            bool usePeriodOfTime = false;
            var periodOfTime = 30;
            if (bank != null)
            {
                if (bank.PeriodOfTime > 0) usePeriodOfTime = true;
                periodOfTime = bank.PeriodOfTime;
            }

            var period = user.UserPeriod;

            var studentdebt = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, period, studentid);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            decimal nonDeleted = 0;
            decimal totalCalculate = 0;
            decimal totalInstallment = 0;
            decimal amount = 0;

            decimal totalDebt = subtotal;

            var installmentNo = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            var studentinstallments = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentid, period);
            if (studentinstallments != null)
            {
                foreach (var item in studentinstallments)
                {
                    if (item.PreviousPayment > 0)
                    {
                        nonDeleted += Convert.ToInt32(item.InstallmentAmount);
                    }
                    else
                    {
                        _studentInstallmentRepository.DeleteStudentInstallment(item);
                    }
                }
            }

            totalInstallment = (totalDebt - cashpayment) - nonDeleted;

            if (installment == 0) installment = 1;
            if (pressbuttonindex == 0 || pressbuttonindex == 2)
            {
                if (byAmount > 0)
                {
                    amount = byAmount;
                }
                else
                {
                    amount = Math.Round(totalInstallment / installment, schoolInfo.CurrencyDecimalPlaces);
                }
            }

            decimal remainingFee = 0;
            decimal remainingCashPayment = cashpayment;
            decimal cashpaymenttotal = cashpayment;

            bool installmentNoUpdate = true;
            List<StudentInstallment> list = new List<StudentInstallment>();
            var detailTable = _studentDebtDetailTableRepository.GetStudentDebtDetailTable(user.SchoolID, period, studentid);
            if (pressbuttonindex == 1)
            {
                installmentNoUpdate = true;
                remainingFee += cashpaymenttotal;
                foreach (var item in detailTable)
                {
                    if (item.AmountTable > cashpaymenttotal)
                    {
                        remainingFee = item.AmountTable - cashpaymenttotal;
                    }
                    else
                    {
                        remainingFee = 0;
                    }

                    if (remainingFee > 0)
                    {
                        var fee = _schoolFeeRepository.GetSchoolFee2(item.SchoolFeeID);
                        totalCalculate = 0;
                        installment = item.InstallmentTable;
                        for (int i = 0; i < installment; i++)
                        {
                            var inst = new StudentInstallment();

                            inst.SchoolID = user.SchoolID;
                            inst.StudentID = studentid;
                            inst.Period = period;
                            inst.InstallmentDate = item.PaymentStartDateTable.AddMonths(i);
                            inst.CategoryID = paymenttypecomboBox;

                            var installmentAmount = remainingFee / item.InstallmentTable;
                            inst.InstallmentAmount = Math.Round(installmentAmount, schoolInfo.CurrencyDecimalPlaces);

                            inst.BankID = banknamecomboBox;
                            inst.CheckCardNo = "";
                            inst.PreviousPayment = 0;

                            inst.FeeName = fee.Name;
                            if (user.SelectedCulture.Trim() == "en-US") inst.FeeName = fee.Language1;

                            inst.IsPrint = false;
                            inst.StatusCategoryID = _parameterRepository.GetParameterCategoryName("Borçlu").CategoryID;
                            inst.AccountReceiptNo = 0;
                            inst.PaymentDate = null;
                            inst.Explanation = "";
                            inst.CheckBankName = "";
                            inst.CheckNo = "";
                            inst.Drawer = "";
                            inst.Endorser = "";

                            ///////////////////////////////////// Parameter ID ye göre kontrol eklendi Ingilizce daha sonra ilave edilecek
                            var param = parameters.FirstOrDefault(p => p.CategoryID == inst.CategoryID);

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

                            totalCalculate += Math.Round(inst.InstallmentAmount, schoolInfo.CurrencyDecimalPlaces);

                            if (i + 1 == installment)
                            {
                                if (totalCalculate != remainingFee)
                                {
                                    inst.InstallmentAmount += (remainingFee - totalCalculate);
                                }
                            }
                            cashpaymenttotal = 0;

                            var previousInstallment = _studentInstallmentRepository.GetStudentInstallment3(user.SchoolID, studentid, period, DateTime.Parse(string.Format("{0:yyyy/MM/dd}", inst.InstallmentDate)));
                            if (previousInstallment != null)
                            {
                                installmentNoUpdate = false;
                                previousInstallment.StudentInstallmentID = previousInstallment.StudentInstallmentID;
                                previousInstallment.InstallmentAmount = previousInstallment.InstallmentAmount + Math.Round(installmentAmount, schoolInfo.CurrencyDecimalPlaces);
                                previousInstallment.FeeName = "";
                                _studentInstallmentRepository.UpdateStudentInstallment(previousInstallment);
                            }
                            else
                            {
                                list.Add(inst);
                                maxIns++;
                                if (maxIns < 101)
                                    _studentInstallmentRepository.CreateStudentInstallment(inst);
                            }
                        }
                    }

                    if (item.AmountTable > remainingFee)
                    {
                        if (cashpaymenttotal >= item.AmountTable)
                            cashpaymenttotal -= item.AmountTable;
                        else cashpaymenttotal = 0;

                        if (remainingFee >= item.AmountTable)
                            remainingFee -= item.AmountTable;
                        else remainingFee = 0;
                    }
                }
            }
            maxIns = 0;
            if (pressbuttonindex != 1)
            {
                for (int i = 0; i < installment; i++)
                {
                    var inst = new StudentInstallment();

                    inst.SchoolID = user.SchoolID;
                    inst.StudentID = studentid;
                    inst.Period = period;
                    inst.CategoryID = paymenttypecomboBox;

                    inst.InstallmentAmount = amount;
                    inst.BankID = banknamecomboBox;
                    inst.CheckCardNo = "";
                    inst.PreviousPayment = 0;
                    inst.FeeName = "";
                    inst.IsPrint = false;
                    inst.StatusCategoryID = _parameterRepository.GetParameterCategoryName("Borçlu").CategoryID;
                    inst.AccountReceiptNo = 0;
                    inst.PaymentDate = null;
                    inst.Explanation = "";
                    inst.CheckBankName = "";
                    inst.CheckNo = "";
                    inst.Drawer = "";
                    inst.Endorser = "";

                    ///////////////////////////////////// Parameter ID ye göre kontrol eklendi Ingilizce daha sonra ilave edilecek
                    var param = parameters.FirstOrDefault(p => p.CategoryID == inst.CategoryID);

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

                    if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI" || categoryName == "Credit Card" || categoryName == "CREDIT CARD" && usePeriodOfTime == true)
                    {
                        if (periodOfTime == 30 || periodOfTime == 31)
                        {
                            inst.InstallmentDate = transactiondate.AddMonths(i);
                        }
                        else
                        {
                            if (i > 0)
                            {
                                inst.InstallmentDate = transactiondate.AddDays(periodOfTime);
                                transactiondate = transactiondate.AddDays(periodOfTime);
                            }
                            else
                            {
                                inst.InstallmentDate = transactiondate;
                            }
                        }
                    }
                    else inst.InstallmentDate = transactiondate.AddMonths(i);

                    totalCalculate += (inst.InstallmentAmount);
                    if (i + 1 == installment)
                    {
                        if (totalCalculate != totalInstallment)
                        {
                            inst.InstallmentAmount += (totalInstallment - totalCalculate);
                        }
                    }

                    list.Add(inst);
                    maxIns++;
                    if (maxIns < 101)
                        _studentInstallmentRepository.CreateStudentInstallment(inst);
                }
            }

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentid);
            studentTemp.TransactionDate = transactiondate;
            studentTemp.PaymentTypeCategoryID = paymenttypecomboBox;
            studentTemp.BankID = banknamecomboBox;
            studentTemp.Installment = installment;
            studentTemp.IsSingleNamePaper = singlepaper;
            studentTemp.CashPayment = cashpayment;
            studentTemp.SubTotal = subtotal;
            _studentTempRepository.UpdateStudentTemp(studentTemp);
            if (installmentNoUpdate)
                _pSerialNumberRepository.UpdatePSerialNumber(installmentNo);
            return Json(list);
        }

        [HttpPost]
        [Route("M210Student/InstallmentDataUpdate/{strResult}")]
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
                    _studentInstallmentRepository.Save();
                }
                i = i + 1;
            }
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID);
            var parameter = parameterList.FirstOrDefault(p => p.CategoryID == json[0].StudentInstallment.CategoryID);
            var bankList = _bankRepository.GetBankAll(json[0].StudentInstallment.SchoolID);
            var bank = bankList.FirstOrDefault(p => p.BankID == json[0].StudentInstallment.BankID);

            var studentViewModel = new StudentViewModel
            {
                ViewModelID = installment.StudentInstallmentID,
                StudentInstallment = installment,
                Parameter2 = parameter,
                Bank = bank,
            };
            return Json(studentViewModel);
        }
        #endregion

        #region SerialNumbers
        [Route("M210Student/SerialNumbersDataRead/{userID}")]
        public IActionResult SerialNumbersDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();

            string[] dim1;
            if (user.SelectedCulture.Trim() == "tr-TR")
            {
                dim1 = new string[] { "Kayıt No", "Banka", "Çek No", "Kmh.No", "Kredi Kartı", "Mail Order", "Ots No-1", "Ots No-2", "Teşvik" };
            }
            else
            {
                dim1 = new string[] { "Register No", "Bank", "Check No", "Kmh.No", "Credit Card", "Mail Order", "Ots No-1", "Ots No-2", "Gov.Pro.No" };
            }

            int[] dim2 = new int[] { serialNumbers.RegisterNo, serialNumbers.BondNo, serialNumbers.CheckNo, serialNumbers.KmhNo, serialNumbers.CreditCardNo, serialNumbers.MailOrderNo, serialNumbers.OtsNo1, serialNumbers.OtsNo2, serialNumbers.GovernmentPromotionNo };

            List<SchoolProccessSerialNumbers> list = new List<SchoolProccessSerialNumbers>();

            var inx = 0;
            foreach (var item in parameters)
            {
                var schoolProccessSerialNumbers = new SchoolProccessSerialNumbers
                {
                    ViewModelId = inx,
                    Name = dim1[inx],
                    SerialNo = dim2[inx],
                };
                inx += 1;
                list.Add(schoolProccessSerialNumbers);
            }
            return Json(list);
        }

        [Route("M210Student/GetNumberDataRead/{userID}/{schoolID}/{categoryID}")]
        public IActionResult GetNumberDataRead(int userID, int schoolID, int categoryID)
        {
            var user = _usersRepository.GetUser(userID);

            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            var number = 0;

            var parameters = _parameterRepository.GetParameter(categoryID);

            string categoryName = "";
            if (user.SelectedCulture.Trim() == "en-US")
                categoryName = parameters.Language1;
            else categoryName = parameters.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") number = pSerialNumber.BondNo += 1;
            if (categoryName == "Çek" || categoryName == "ÇEK") number = pSerialNumber.CheckNo += 1;
            if (categoryName == "Elden" || categoryName == "ELDEN") number = pSerialNumber.BondNo += 1;
            if (categoryName == "Kmh" || categoryName == "KMH") number = pSerialNumber.KmhNo += 1;
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") number = pSerialNumber.CreditCardNo += 1;
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") number = pSerialNumber.MailOrderNo += 1;
            if (categoryName == "Ots_1" || categoryName == "OTS_1") number = pSerialNumber.OtsNo1 += 1;
            if (categoryName == "Ots_2" || categoryName == "OTS_2") number = pSerialNumber.OtsNo2 += 1;
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") number = pSerialNumber.GovernmentPromotionNo += 1;

            if (user.SelectedCulture.Trim() == "en-US")
            {
                if (categoryName == "Bank" || categoryName == "BANK") number = number = pSerialNumber.BondNo += 1;
                if (categoryName == "Check" || categoryName == "CHECK") number = number = pSerialNumber.CheckNo += 1;
                if (categoryName == "Bond by Hand" || categoryName == "BOND BY HAND") number = pSerialNumber.BondNo += 1;
                if (categoryName == "Overdraft Account" || categoryName == "OVERDRAFT ACCOUNT") number = pSerialNumber.KmhNo += 1;
                if (categoryName == "Credit Card" || categoryName == "CREDIT CARD") number = pSerialNumber.CreditCardNo += 1;
                if (categoryName == "Mail order" || categoryName == "MAİL ORDER") number = pSerialNumber.MailOrderNo += 1;
                if (categoryName == "Ots_1" || categoryName == "OTS_1") number = pSerialNumber.OtsNo1 += 1;
                if (categoryName == "Ots_2" || categoryName == "OTS_2") number = pSerialNumber.OtsNo2 += 1;
                if (categoryName == "Gov.Support" || categoryName == "GOV.SUPPORT") number = pSerialNumber.GovernmentPromotionNo += 1;
            }

            _pSerialNumberRepository.UpdatePSerialNumber(pSerialNumber);
            return Json(new { lastnumber = number });
        }
        #endregion

        #region StudentPeriods
        [Route("M210Student/StudentPeriodsDataRead/{userID}/{studentID}")]
        public IActionResult StudentPeriodsDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var studentPeriod = _studentPeriodsRepository.GetStudent(user.SchoolID, studentID).OrderByDescending(b => b.Period);

            return Json(studentPeriod);
        }
        #endregion

        #region Combo
        [HttpGet]
        [Route("M210Student/BankTypeDataRead/{userID}")]
        public IActionResult BankTypeDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var bankNameType = _bankRepository.GetBankAll(user.SchoolID);
            return Json(bankNameType);
        }

        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }

        [Route("M210Student/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }

        [HttpPost]
        [Route("M210Student/ClassromControl/{userID}/{classroomID}")]
        public IActionResult ClassromControl(int userID, int classroomID)
        {
            bool isExceeded = false;
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomID(classroomID);
            var classroooms = _studentRepository.GetStudentAllWithClassroomCount(user.SchoolID, classroomID);

            if (classroooms.Count() > classroom.RoomQuota && classroom.RoomQuota > 0)
                isExceeded = true;

            return Json(new { IsExceeded = isExceeded, Quota = classroom.RoomQuota, IsExisting = classroooms.Count() });
        }

        [Route("M210Student/StatuCombo/{isNew}/{userID}")]
        public IActionResult StatuCombo(bool isNew, int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;

            var statu = _parameterRepository.GetParameterSubID(categoryID).Where(b => b.CategoryName != "İptal" && b.CategoryName != "Takipte (İptal)");
            if (user.SelectedCulture.Trim() == "en-US") statu = _parameterRepository.GetParameterSubID(categoryID).Where(b => b.Language1 != "Cancel" && b.Language1 != "Following (Cancel)");

            if (!isNew)
                statu = _parameterRepository.GetParameterSubID(categoryID);

            return Json(statu);
        }
        public IActionResult GenderTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Cinsiyeti").CategoryID;
            var gender = _parameterRepository.GetParameterSubID(categoryID);
            return Json(gender);
        }

        public IActionResult RegistrationTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(registrationType);
        }

        public IActionResult BloodGroupTypeCombo(string culture)
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
        public IActionResult PaymentTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(registrationType);
        }
        public IActionResult PaymentTypeDataRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(paymentType);
        }
        public IActionResult BankNameCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var bankNameType = _bankRepository.GetBankAll(user.SchoolID);
            return Json(bankNameType);
        }
        public IActionResult CityCombo(string culture)
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
        public IActionResult KinshipCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Yakınlığı").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            return Json(statu);
        }
        public IActionResult ProfessionCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Mesleği").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            return Json(statu);
        }
        public IActionResult PreviousSchoolCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Geldiği Okul").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            return Json(statu);
        }
        public IActionResult PreviousBranchCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Geldiği Bölüm").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            return Json(statu);
        }

        [Route("M210Student/SchoolBusServiceDataRead/{userID}")]
        public IActionResult SchoolBusServiceDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolBusServices = _schoolBusServicesRepository.GetSchoolBusServicesAll(user.SchoolID, user.UserPeriod);
            return Json(schoolBusServices);
        }

        [Route("M210Student/ServicesStatusCombo/{schoolID}")]
        public IActionResult ServicesStatusCombo(int schoolID)
        {
            var fees = _schoolFeeRepository.GetSchoolServiceFeeAll(schoolID, "L1");
            return Json(fees);
        }
        public void AccountCodeCreate(StudentViewModel studentViewModel)
        {
            var period = studentViewModel.Period;
            var accountingCode = new AccountCodes();
            accountingCode.Period = studentViewModel.Period;
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(studentViewModel.SchoolID);

            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string periodTxt = " Dönemi, ";
            string yearTxt = " Yılı ";
            string monthTxt = " Ayı ";
            string txt = "'ün ";
            string txt2 = " Şb. ";
            string codeName = "KASA";

            if (studentViewModel.SelectedCulture.Trim() == "en-US")
            {
                periodTxt = " Period, ";
                yearTxt = " Year ";
                monthTxt = " Month ";
                txt = "'s ";
                txt2 = " Branch ";
                codeName = "Cash Counter";
            }
            string deptCode = schoolInfo.CompanyShortCode;
            string deptName = schoolInfo.CompanyShortName + txt2;

            string studentNo = studentViewModel.Student.StudentSerialNumber.ToString();
            string studentName = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + txt;

            string controlCode = schoolInfo.AccountNoID01;
            if (controlCode == null) controlCode = "100";

            bool isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);

            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
            else
            {
                controlCode = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID01, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(schoolInfo.AccountNoID01, period).AccountCodeName;
            }

            ///////////////////////////  121 ////////////////////////////////////////
            controlCode = schoolInfo.AccountNoID05;
            if (controlCode == null) controlCode = "121";
            string code121 = controlCode;
            codeName = "ALACAK SENETLERİ";
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
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(studentViewModel.SchoolID, studentViewModel.Student.StudentID, period);
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string code = null;

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
                if (studentViewModel.SelectedCulture.Trim() == "en-US")
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

                if (studentViewModel.SelectedCulture.Trim() == "en-US")
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
                //if (code == "102") mainCodeName = "BANKALAR";
                if (code == "108") mainCodeName = "DİĞER HAZIR DEĞERLER";
                if (code == "108 01") mainCodeName = "KREDİ KARTI";
                if (code == "102") mainCodeName = "KMH.HESABI";

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
        private void CodeCreate(AccountCodes accountingCode, string controlCode, string codeName)
        {
            accountingCode.AccountCodeID = 0;
            accountingCode.Period = accountingCode.Period;
            accountingCode.AccountCode = controlCode;
            accountingCode.AccountCodeName = codeName;
            accountingCode.IsActive = true;
            _accountCodesRepository.CreateAccountCode(accountingCode);
        }
        public void AccountingCreate(StudentViewModel studentViewModel)
        {
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(studentViewModel.SchoolID);
            string studentNo = studentViewModel.Student.StudentSerialNumber.ToString();
            var sortOrder = 1;
            var period = studentViewModel.Period;
            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);

            var accounting = new Accounting();
            string controlCode = schoolInfo.AccountNoID01;
            if (controlCode == null) controlCode = "100";

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            var studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, period, studentViewModel.Student.StudentID);
            var cashPayment = studentTemp.CashPayment;

            var lastNumber = pserialNumbers.AccountSerialNo += 1;
            pserialNumbers.AccountSerialNo = lastNumber;

            var collectionReceiptNo = pserialNumbers.CollectionReceiptNo += 1;
            pserialNumbers.CollectionReceiptNo = collectionReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var classroomName = "";
            if (studentViewModel.Student.ClassroomID > 0)
                classroomName = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;

            var bankName = "";
            if (studentViewModel.StudentTemp.BankID != 0)
            {
                bankName = _bankRepository.GetBank(studentViewModel.StudentTemp.BankID).BankName;
            }
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            //CollectionNo
            var collectionNo = pserialNumbers.CollectionNo += 1;
            pserialNumbers.CollectionNo = collectionNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var exp1 = " ";
            var exp2 = " ";
            //100 ACOOUNT RECORDS 
            if (cashPayment > 0)
            {
                accounting.AccountingID = 0;
                accounting.SchoolID = studentViewModel.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = collectionNo.ToString();
                accounting.DocumentDate = DateTime.Today;

                exp1 = " Nolu Makbuz ";
                exp2 = " Tarihli Nakit Tahsili ";
                if (studentViewModel.SelectedCulture.Trim() == "en-US")
                {
                    exp1 = " Receipt No. ";
                    exp2 = " Dated Cash Collection ";
                }
                accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
                accounting.Debt = cashPayment;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;

                _accountingRepository.CreateAccounting(accounting);
            }
            //121 ACOOUNT RECORDS 
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(studentViewModel.SchoolID, studentViewModel.Student.StudentID, period);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string code = null;
            string codeName = "ALACAK SENETLERİ";
            string mainCodeName = codeName;
            string periodTxt = " Dönemi, ";
            string deptCode = schoolInfo.CompanyShortCode;
            string deptName = schoolInfo.CompanyShortName + " Şb. ";
            if (studentViewModel.SelectedCulture.Trim() == "en-US")
            {
                periodTxt = " Period, ";
                deptName = schoolInfo.CompanyShortName + " Branch. ";
            }

            foreach (var item in studentinstallment)
            {
                sortOrder += 1;
                accounting.AccountingID = 0;
                accounting.SchoolID = studentViewModel.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = collectionReceiptNo.ToString();
                accounting.DocumentDate = DateTime.Today;

                var paymentTypetxt = "";

                var param = parameters.FirstOrDefault(p => p.CategoryID == item.CategoryID);

                string categoryName = "";
                if (studentViewModel.SelectedCulture.Trim() == "en-US")
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
                if (studentViewModel.SelectedCulture.Trim() == "en-US")
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
                var exp4 = " Tarihli " + paymentTypetxt + " Dekontu ";
                string monthTxt = " Ayı ";
                if (studentViewModel.SelectedCulture.Trim() == "en-US")
                {
                    exp3 = " Receipt No, ";
                    exp4 = " Dated " + paymentTypetxt + " Receipt ";
                    monthTxt = " Month ";
                }

                controlCode = code;
                if (controlCode == null) controlCode = "121";
                string code121 = controlCode;

                controlCode = code121 + " " + periodCode;
                codeName = periodCode + periodTxt + ", " + mainCodeName;

                DateTime dt = Convert.ToDateTime(item.InstallmentDate);
                int MM = dt.Month;

                string YY = dt.ToString("yy");
                //121 2122 22 Yılı, ALACAK SENETLERİ
                controlCode = code121 + " " + periodCode + " " + YY + " " + MM + " " + deptCode; ;
                codeName = periodCode + periodTxt + YY + "/" + MM + monthTxt + ", " + deptCode + mainCodeName;

                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;

                int DD = dt.Day;
                if (categoryName == "Elden" || categoryName == "Çek")
                    accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4;
                else
                    accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

                if (studentViewModel.SelectedCulture.Trim() == "en-US")
                {
                    if (categoryName == "Bond by Hand" || categoryName == "Check")
                        accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4;
                    else
                        accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;
                }

                accounting.Debt = item.InstallmentAmount;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }

            //340 ACOOUNT RECORDS 

            controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";
            string code340 = controlCode;
            controlCode = code340 + " " + periodCode;
            mainCodeName = codeName;

            controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;

            sortOrder += 1;
            accounting.AccountingID = 0;
            accounting.SchoolID = studentViewModel.SchoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = collectionReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            var exp5 = " Tarihinde Öğrenciden Alınan Toplam";
            if (studentViewModel.SelectedCulture.Trim() == "en-US")
            {
                exp5 = " Total Received from Student on";
            }
            accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + DateTime.Today.ToString("dd.MM.yyyy") + exp5;
            accounting.Debt = 0;
            accounting.Credit = studentTemp.SubTotal;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;

            _accountingRepository.CreateAccounting(accounting);

            //340 ACOOUNT RECORDS 
            if (cashPayment > 0)
            {
                sortOrder += 1;
                accounting.AccountingID = 0;
                accounting.SchoolID = studentViewModel.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = collectionReceiptNo.ToString();
                accounting.DocumentDate = DateTime.Today;
                accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
                accounting.Debt = cashPayment;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);

                //600 ACOOUNT RECORDS 
                controlCode = schoolInfo.AccountNoID03;
                if (controlCode == null) controlCode = "600";
                string code600 = controlCode;
                codeName = "YURT İÇİ SATIŞLAR";
                mainCodeName = codeName;

                controlCode = code600 + " " + periodCode + " " + deptCode + " " + "01";
                codeName = periodCode + periodTxt + ", " + mainCodeName;

                sortOrder += 1;
                accounting.AccountingID = 0;
                accounting.SchoolID = studentViewModel.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = collectionReceiptNo.ToString();
                accounting.DocumentDate = DateTime.Today;
                accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + collectionNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
                accounting.Debt = 0;
                accounting.Credit = cashPayment;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }

            studentTemp.ReceiptNo = collectionNo;
            studentTemp.AccountReceipt = lastNumber;
            _studentTempRepository.UpdateStudentTemp(studentTemp);
        }
        public void AccounCodeDefaultCreate(string period)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            string code = " ";
            string codeName = " ";

            for (int i = 0; i < 12; i++)
            {
                if (i == 0) { code = "100"; codeName = "KASA"; }
                if (i == 1) { code = "340"; codeName = "ALINAN SİPARİŞ AVANSLARI"; }
                if (i == 2) { code = "600"; codeName = "YURT İÇİ SATIŞLAR"; }
                if (i == 3) { code = "101"; codeName = "ALINAN ÇEKLER HESABI"; }
                if (i == 4) { code = "121"; codeName = "ALACAK SENETLERİ"; }
                if (i == 5) { code = "120"; codeName = "ALICILAR HESABI"; }
                //if (i == 6) { code = "102"; codeName = "BANKALAR"; }
                if (i == 7) { code = "108"; codeName = "DİĞER HAZIR DEĞERLER"; }
                if (i == 8) { code = "108 01"; codeName = "KREDİ KARTI"; }
                // if (i ==  9) { code = "600"; codeName = "TEŞVİK"; }
                if (i == 10) { code = "102"; codeName = "KMH"; }
                if (i == 11) { code = "610"; codeName = "KAYITTAN İNDİRİMLER HESABI"; }
                if (code != "")
                {
                    bool isExist = _accountCodesRepository.ExistAccountCode(period, code);
                    if (!isExist)
                    {
                        accountingCode.AccountCodeID = 0;
                        accountingCode.Period = period;
                        accountingCode.AccountCode = code;
                        accountingCode.AccountCodeName = codeName;
                        accountingCode.IsActive = true;
                        _accountCodesRepository.CreateAccountCode(accountingCode);
                    }
                }
            }
        }
        #endregion

        #region Chart
        [Route("M210Student/ChartStudent1/{userID}")]
        public IActionResult ChartStudent1(int userID)
        {
            List<Student> student = new List<Student>();

            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            //var student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();

            if (schoolInfo.NewPeriod != user.UserPeriod)
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

            List<ChartModel> list = new List<ChartModel>();
            decimal multiplier = 0;
            decimal count = student.Count();

            if (student.Count() != 0)
                multiplier = Math.Round(100 / count, schoolInfo.CurrencyDecimalPlaces);

            decimal per = 0;
            string name = "";
            Boolean Exp = false;
            foreach (var item in statuCategories)
            {
                Exp = false;
                if (item.CategoryName == "Kesin Kayıt" || item.Language1 == "Registration") Exp = true;

                name = item.CategoryName;
                if (user.SelectedCulture.Trim() == "en-US") name = item.Language1;
                var total = _studentRepository.GetStudentStatusCategory(user.SchoolID, item.CategoryID);

                per = total.Count() * multiplier;
                var chartModel = new ChartModel
                {
                    Source = name,
                    Explode = Exp,
                    Percentage = per,
                    Amount = total.Count(),
                };
                list.Add(chartModel);
            };
            return Json(list);
        }

        [Route("M210Student/ChartStudent2/{userID}")]
        public IActionResult ChartStudent2(int userID)
        {
            List<Student> student = new List<Student>();

            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            //var student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();

            if (schoolInfo.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registerCategories = _parameterRepository.GetParameterSubID(categoryID);

            List<ChartModel> list = new List<ChartModel>();
            decimal multiplier = 0;
            decimal count = student.Count();
            if (student.Count() != 0)
                multiplier = Math.Round(100 / count, schoolInfo.CurrencyDecimalPlaces);

            decimal per = 0;
            string name = "";
            Boolean Exp = false;

            foreach (var item in registerCategories.OrderByDescending(x => x.CategoryName))
            {
                Exp = false;
                if (item.CategoryName == "Yeni Kayıt" || item.Language1 == "New Registration") Exp = true;

                name = item.CategoryName;
                if (user.SelectedCulture.Trim() == "en-US") name = item.Language1;
                var total = _studentRepository.GetStudentRegisterCategory(user.SchoolID, item.CategoryID);

                per = total.Count() * multiplier;
                var chartModel = new ChartModel
                {
                    Source = name,
                    Explode = Exp,
                    Percentage = per,
                    Amount = total.Count(),
                };
                list.Add(chartModel);
            };
            return Json(list);
        }

        [Route("M210Student/SummaryDataRead1/{userID}")]
        public IActionResult SummaryDataRead1(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            string culture = user.SelectedCulture.Trim();

            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            decimal cash = 0;
            decimal bank = 0;
            decimal handPayment = 0;
            decimal check = 0;
            decimal kmh = 0;
            decimal creditCard = 0;
            decimal mailOrder = 0;
            decimal ots1 = 0;
            decimal ots2 = 0;

            decimal fee1 = 0;
            decimal fee2 = 0;
            decimal fee3 = 0;
            decimal fee4 = 0;
            decimal fee5 = 0;
            decimal fee6 = 0;
            decimal fee7 = 0;
            decimal fee8 = 0;
            decimal fee9 = 0;
            decimal fee10 = 0;

            int cashQ = 0;
            int bankQ = 0;
            int handPaymentQ = 0;
            int checkQ = 0;
            int kmhQ = 0;
            int creditCardQ = 0;
            int mailOrderQ = 0;
            int ots1Q = 0;
            int ots2Q = 0;

            var students = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);

            cash = _studentTempRepository.GetStudentTempAllCash(user.UserPeriod).GroupBy(a => a.StudentID).Select(a => a.FirstOrDefault()).Sum(c => c.CashPayment);
            cashQ = _studentTempRepository.GetStudentTempAllCash(user.UserPeriod).GroupBy(b => b.StudentID).Count();

            var categoryID = _parameterRepository.GetParameterCategoryName("Banka").CategoryID;
            var installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            var installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            bank += installment.Sum(p => p.InstallmentAmount);
            bankQ = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("Elden").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            handPayment += installment.Sum(p => p.InstallmentAmount);
            handPaymentQ = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("Çek").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            check += installment.Sum(p => p.InstallmentAmount);
            checkQ = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("KMH").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            kmh += installment.Sum(p => p.InstallmentAmount);
            kmhQ = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("Kredi kartı").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            creditCard += installment.Sum(p => p.InstallmentAmount);
            creditCardQ = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("Mail order").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            mailOrder += installment.Sum(p => p.InstallmentAmount);
            mailOrderQ = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("OTS_1").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            ots1 += installment.Sum(p => p.InstallmentAmount);
            ots1Q = installment.Count();

            categoryID = _parameterRepository.GetParameterCategoryName("OTS_2").CategoryID;
            installmentx = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            installment = installmentx.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            ots2 += installment.Sum(p => p.InstallmentAmount);
            ots2Q = installment.Count();

            string[] dim1;
            if (user.SelectedCulture.Trim() == "tr-TR")
            {
                dim1 = new string[] { "Peşin", "Banka", "Elden", "Çek No", "Kmh.No", "Kredi Kartı", "Mail Order", "Ots No-1", "Ots No-2" };
            }
            else
            {
                dim1 = new string[] { "Cash", "Bank", "Hand payment", "Check No", "Kmh.No", "Credit Card", "Mail Order", "Ots No-1", "Ots No-2" };
            }

            decimal[] dim2 = new decimal[] { cash, bank, handPayment, check, kmh, creditCard, mailOrder, ots1, ots2 };
            int[] dim3 = new int[] { cashQ, bankQ, handPaymentQ, checkQ, kmhQ, creditCardQ, mailOrderQ, ots1Q, ots2Q };

            int i = 0;
            int inx = 0;



            foreach (var item in fee)
            {
                i += 1;

                if (user.SelectedCulture.Trim() == "tr-TR") dim1 = dim1.Append(item.Name).ToArray();
                if (user.SelectedCulture.Trim() == "en-US") dim1 = dim1.Append(item.Language1).ToArray();

                var studentfeex = _studentDebtRepository.GetStudentDebtAllCategory2(user.UserPeriod, user.SchoolID, item.SchoolFeeID);
                var studentfee = studentfeex.Where(s => students.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();

                dim3 = dim3.Append(studentfee.Count()).ToArray();

                if (i == 1)
                {
                    fee1 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee1).ToArray(); inx = i; }
                }

                if (i == 2)
                {
                    fee2 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee2).ToArray(); inx = i; }
                }

                if (i == 3)
                {
                    fee3 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee3).ToArray(); inx = i; }
                }

                if (i == 4)
                {
                    fee4 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee4).ToArray(); inx = i; }
                }

                if (i == 5)
                {
                    fee5 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee5).ToArray(); inx = i; }
                }

                if (i == 6)
                {
                    fee6 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee6).ToArray(); inx = i; }
                }

                if (i == 7)
                {
                    fee7 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee7).ToArray(); inx = i; }
                }
                if (i == 8)
                {
                    fee8 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee8).ToArray(); inx = i; }
                }
                if (i == 9)
                {
                    fee9 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee9).ToArray(); inx = i; }
                }
                if (i == 10)
                {
                    fee10 += studentfee.Sum(p => p.Amount);
                    { dim2 = dim2.Append(fee10).ToArray(); inx = i; }
                }
            }

            inx += 9;
            List<SummaryModel> list = new List<SummaryModel>();
            for (int ii = 0; ii < inx; ii++)
            {
                if (ii > 9)
                {
                    if ((dim2[ii] > 0 || dim3[ii] > 0))
                    {
                        var summaryModel = new SummaryModel
                        {
                            ViewModelId = i,
                            Name = dim1[ii],
                            Amount = dim2[ii],
                            Quantity = dim3[ii],
                        };
                        list.Add(summaryModel);
                    }
                }
                else
                {
                    var summaryModel = new SummaryModel
                    {
                        ViewModelId = i,
                        Name = dim1[ii],
                        Amount = dim2[ii],
                        Quantity = dim3[ii],
                    };
                    list.Add(summaryModel);
                }
            }
            return Json(list);
        }


        [Route("M210Student/SummaryDataRead2/{userID}")]
        public IActionResult SummaryDataRead2(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<SummaryModel> list = new List<SummaryModel>();

            var categoryID = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var classrooomType = _parameterRepository.GetParameterSubID(categoryID);

            var students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID);
            var fee = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1").OrderBy(a => a.SortOrder);

            foreach (var item in classrooomType)
            {
                var inx = 0;
                var summaryModel = new SummaryModel();
                summaryModel.Amount01 = 0; summaryModel.Amount02 = 0; summaryModel.Amount03 = 0; summaryModel.Amount04 = 0; summaryModel.Amount05 = 0;
                summaryModel.Amount06 = 0; summaryModel.Amount07 = 0; summaryModel.Amount08 = 0; summaryModel.Amount09 = 0; summaryModel.Amount10 = 0;
                summaryModel.AvarageAmount01 = 0; summaryModel.AvarageAmount02 = 0; summaryModel.AvarageAmount03 = 0; summaryModel.AvarageAmount04 = 0; summaryModel.AvarageAmount05 = 0;
                summaryModel.AvarageAmount06 = 0; summaryModel.AvarageAmount07 = 0; summaryModel.AvarageAmount08 = 0; summaryModel.AvarageAmount09 = 0; summaryModel.AvarageAmount10 = 0;
                summaryModel.Quantity01 = 0; summaryModel.Quantity02 = 0; summaryModel.Quantity03 = 0; summaryModel.Quantity04 = 0; summaryModel.Quantity05 = 0;
                summaryModel.Quantity06 = 0; summaryModel.Quantity07 = 0; summaryModel.Quantity08 = 0; summaryModel.Quantity09 = 0; summaryModel.Quantity10 = 0;

                decimal amount = 0; int quantity = 0;
                if (item.CategoryName != "")
                {
                    foreach (var f in fee)
                    {
                        inx++;
                        amount = 0; quantity = 0;
                        var studentFees = _studentDebtRepository.GetStudentDebtAllCategory(user.UserPeriod, user.SchoolID, item.CategoryID, f.SchoolFeeID);
                        amount = studentFees.Sum(p => p.Amount);
                        quantity = studentFees.Count();
                        summaryModel.ViewModelId = (int)item.CategorySubID;

                        summaryModel.Name = item.CategoryName;
                        if (user.SelectedCulture.Trim() == "en-US") summaryModel.Name = item.Language1;
                        if (summaryModel.Name == "") summaryModel.Name = ".";

                        if (amount > 0)
                        {
                            if (inx == 01) { summaryModel.Amount01 = amount; Math.Round(summaryModel.AvarageAmount01 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity01 = quantity; }
                            if (inx == 02) { summaryModel.Amount02 = amount; Math.Round(summaryModel.AvarageAmount02 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity02 = quantity; }
                            if (inx == 03) { summaryModel.Amount03 = amount; Math.Round(summaryModel.AvarageAmount03 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity03 = quantity; }
                            if (inx == 04) { summaryModel.Amount04 = amount; Math.Round(summaryModel.AvarageAmount04 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity04 = quantity; }
                            if (inx == 05) { summaryModel.Amount05 = amount; Math.Round(summaryModel.AvarageAmount05 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity05 = quantity; }
                            if (inx == 06) { summaryModel.Amount06 = amount; Math.Round(summaryModel.AvarageAmount06 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity06 = quantity; }
                            if (inx == 07) { summaryModel.Amount07 = amount; Math.Round(summaryModel.AvarageAmount07 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity07 = quantity; }
                            if (inx == 08) { summaryModel.Amount08 = amount; Math.Round(summaryModel.AvarageAmount08 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity08 = quantity; }
                            if (inx == 09) { summaryModel.Amount09 = amount; Math.Round(summaryModel.AvarageAmount09 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity09 = quantity; }
                            if (inx == 10) { summaryModel.Amount10 = amount; Math.Round(summaryModel.AvarageAmount10 = amount / quantity, school.CurrencyDecimalPlaces); summaryModel.Quantity10 = quantity; }
                        }
                    }
                }
                list.Add(summaryModel);
            }
            return Json(list);
        }

        #endregion

        #region Navigate

        [Route("M210Student/Navigate/{navigate}/{classroomID}/{period}/{schoolID}/{studentID}/{firstName}/{lastName}")]
        public IActionResult Navigate(string navigate, int classroomID, string period, int schoolID, int studentID, string firstName, string lastName)
        {
            var school = _schoolInfoRepository.GetSchoolInfo(schoolID);
            int sortType = school.SortType;
            if (sortType < 1 || sortType > 3) sortType = 1;

            List<Student> students = new List<Student>();

            if (school.NewPeriod != period)
            {
                if (school.SortOption == false)
                {
                    var allStudents = _studentRepository.GetStudentAllPeriod(schoolID).ToList();
                    var studenPeriod = _studentPeriodsRepository.GetStudentAll(schoolID, period);
                    students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
                }
                else
                {
                    var allStudents = _studentRepository.GetStudentClassroom(schoolID, classroomID).ToList();
                    var studenPeriod = _studentPeriodsRepository.GetStudentAll(schoolID, period);
                    students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
                }
            }
            else
            {
                if (school.SortOption == false)
                    students = _studentRepository.GetStudentAllWithClassroom(schoolID).OrderBy(b => b.FirstName).ToList();
                else students = _studentRepository.GetStudentClassroom(schoolID, classroomID).ToList();
            }

            int ID = 0;
            if (navigate == "First")
            {
                if (sortType == 1)
                    ID = students.OrderBy(p => p.StudentID).FirstOrDefault().StudentID;
                if (sortType == 2)
                    ID = students.OrderBy(p => p.FirstName).FirstOrDefault().StudentID;
                if (sortType == 3)
                    ID = students.OrderBy(p => p.LastName).FirstOrDefault().StudentID;
            }
            else if (navigate == "Previous")
            {
                if (sortType == 1)
                {
                    var prv = students.Where(p => p.StudentID < studentID).OrderByDescending(i => i.StudentID).FirstOrDefault();
                    if (prv != null) ID = prv.StudentID;
                    else ID = studentID;
                }
                if (sortType == 2)
                {
                    string name = firstName + " " + lastName;
                    var next = students.Where(x => string.Compare(x.FirstName + " " + x.LastName, name, false) < 0).OrderByDescending(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                    if (next != null) ID = next.StudentID;
                    else ID = studentID;
                }
                if (sortType == 3)
                {
                    string name = lastName + " " + firstName;
                    var next = students.Where(x => string.Compare(x.LastName + " " + x.FirstName, name, false) < 0).OrderByDescending(x => x.LastName + " " + x.FirstName).FirstOrDefault();
                    if (next != null) ID = next.StudentID;
                    else ID = studentID;
                }
            }
            else if (navigate == "Next")
            {
                if (sortType == 1)
                {
                    var prv = students.Where(p => p.StudentID > studentID).OrderBy(i => i.StudentID).FirstOrDefault();
                    if (prv != null) ID = prv.StudentID;
                    else ID = studentID;
                }
                if (sortType == 2)
                {
                    string name = firstName + " " + lastName;
                    var next = students.Where(x => string.Compare(x.FirstName + " " + x.LastName, name, false) > 0).OrderBy(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                    if (next != null) ID = next.StudentID;
                    else ID = studentID;
                }

                if (sortType == 3)
                {
                    string name = lastName + " " + firstName;
                    var next = students.Where(x => string.Compare(x.LastName + " " + x.FirstName, name, false) > 0).OrderBy(x => x.LastName + " " + x.FirstName).FirstOrDefault();
                    if (next != null) ID = next.StudentID;
                    else ID = studentID;
                }
            }
            else if (navigate == "Last")
            {
                if (sortType == 1)
                    ID = students.OrderByDescending(p => p.StudentID).FirstOrDefault().StudentID;
                if (sortType == 2)
                    ID = students.OrderByDescending(p => p.FirstName).FirstOrDefault().StudentID;
                if (sortType == 3)
                    ID = students.OrderByDescending(p => p.LastName).FirstOrDefault().StudentID;
            }

            return Json(new { studentID = ID });
        }

        [Route("M210Student/SortTypeUpdate/{schoolID}/{period}/{userID}/{classroomID}/{studentID}/{e}")]
        public IActionResult SortTypeUpdate(int schoolID, string period, int userID, int classroomID, int studentID, string e)
        {
            var school = _schoolInfoRepository.GetSchoolInfo(schoolID);
            string classroomName = "";
            bool isExist = false;
            if (school.NewPeriod != period)
            {
                isExist = _studentPeriodsRepository.ExistStudentPeriods(schoolID, studentID, period);
                if (isExist)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(schoolID, studentID, period).ClassroomName;
                }
            }
            else
                if (classroomID > 0)
                classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;

            List<Student> students = new List<Student>();
            if (school.NewPeriod != period)
            {
                if (school.SortOption == true)
                {
                    var allStudents = _studentRepository.GetStudentAllPeriod(schoolID).ToList();
                    var studenPeriod = _studentPeriodsRepository.GetStudentAll(schoolID, period);
                    students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
                }
                else
                {
                    var allStudents = _studentRepository.GetStudentClassroom(schoolID, classroomID).ToList();
                    var studenPeriod = _studentPeriodsRepository.GetStudentAll(schoolID, period);
                    students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
                }
            }
            else
            {
                if (school.SortOption == true)
                    students = _studentRepository.GetStudentAllWithClassroom(schoolID).OrderBy(b => b.FirstName).ToList();
                else students = _studentRepository.GetStudentClassroom(schoolID, classroomID).ToList();
            }

            int count = students.Count();

            if (e == "RecordSequential") school.SortType = 1;
            if (e == "SortByName") school.SortType = 2;
            if (e == "SortBySurname") school.SortType = 3;

            if (e == "filter")
                if (school.SortOption == false) school.SortOption = true;
                else school.SortOption = false;

            _schoolInfoRepository.UpdateSchoolInfo(school);

            return Json(new { sortOption = school.SortOption, count = count, classroom = classroomName });
        }

        #endregion


        #region Preregistration
        //[Route("M210Student/Preregistration/{studentID}/{userID}")]
        [Route("M210Student/Preregistration")]
        public IActionResult Preregistration(int studentID)
        {

            int userID = 1;
            var user = _usersRepository.GetUser(userID);
            bool isPermissionDiscount = true;
            bool isPermissionFee = true;

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            bool IsNew = false;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = new Student();

            if (studentID == 0)
            {
                student.IsWebRegistration = true;
                IsNew = true;
            }
            else
            {
                student = _studentRepository.GetStudent(studentID);
                if (student.ClassroomID == 0 && user.UserPeriod == school.NewPeriod) IsNew = true;
            }
            if (student.DateOfRegistration == null)
                student.DateOfRegistration = DateTime.Today;
            if (student.FirstDateOfRegistration == null)
                student.FirstDateOfRegistration = student.DateOfRegistration;

            var statuCategoryName = "";
            if (student != null && student.StatuCategoryID > 0)
            {
                if (user.SelectedCulture.Trim() == "en-US") statuCategoryName = _parameterRepository.GetParameter(student.StatuCategoryID).Language1;
                else statuCategoryName = _parameterRepository.GetParameter(student.StatuCategoryID).CategoryName;
            }

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            if (studentTemp == null || studentTemp.StudentTempID == 0)
            {
                studentTemp = new StudentTemp();
                studentTemp.SchoolID = user.SchoolID;
                studentTemp.StudentID = studentID;
                studentTemp.Period = user.UserPeriod;
                studentTemp.Installment = school.DefaultInstallment;
                studentTemp.TransactionDate = DateTime.Today.AddMonths(1);
                studentTemp.CashPayment = 0;
                studentTemp.StatuCategoryName = statuCategoryName;
                _studentTempRepository.CreateStudentTemp(studentTemp);
            }
            else
            {
                studentTemp.StatuCategoryName = statuCategoryName;
                _studentTempRepository.UpdateStudentTemp(studentTemp);
            }

            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (pSerialNumber == null)
                pSerialNumber = new PSerialNumber();

            var categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);

            if (student.StudentPicture == null || student.StudentPicture == "")
                student.StudentPicture = "male.jpg";

            var studentAddress = _studentAddressRepository.GetStudentAddress(student.StudentID);
            if (studentAddress == null)
            {
                studentAddress = new StudentAddress();
                studentAddress.IsEMail = true;
                studentAddress.IsSms = true;
            }

            var studentParentAddress = _studentParentAddressRepository.GetStudentParentAddress(student.StudentID);
            if (studentParentAddress == null)
                studentParentAddress = new StudentParentAddress();

            if (studentParentAddress.ParentPicture == null || studentParentAddress.ParentPicture == "")
            {
                studentParentAddress.ParentPicture = "male.jpg";
                studentParentAddress.IsEmail = true;
                studentParentAddress.IsSMS = true;
            }
            var studentFamilyAddress = _studentFamilyAddressRepository.GetStudentFamilyAddress(student.StudentID);
            if (studentFamilyAddress == null)
            {
                studentFamilyAddress = new StudentFamilyAddress();
                studentFamilyAddress.FatherIsEmail = true;
                studentFamilyAddress.FatherIsSMS = true;
                studentFamilyAddress.MotherIsEmail = true;
                studentFamilyAddress.MotherIsSMS = true;
            }

            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(student.StudentID);
            if (studentInvoiceAddress == null)
                studentInvoiceAddress = new StudentInvoiceAddress();

            if (studentInvoiceAddress.InvoiceTaxNumber == null)
                studentInvoiceAddress.InvoiceTaxNumber = student.IdNumber;
            if (studentInvoiceAddress.Notes == null)
                studentInvoiceAddress.Notes = student.FirstName + " " + student.LastName;

            var studentNote = _studentNoteRepository.GetStudentNote(student.StudentID);
            if (studentNote == null)
                studentNote = new StudentNote();


            var period = user.UserPeriod;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);


            var studentDebt = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, studentID);
            decimal totalAmount = studentDebt.Sum(p => p.UnitPrice);
            TempData["newYear"] = 0;
            if (school.NewPeriod == user.UserPeriod && totalAmount == 0) TempData["newYear"] = 1;

            string sortIcon = "filter";
            if (school.SortOption == false) sortIcon = "filter-clear";

            bool isExist = false;
            string classroomName = "";

            if (school.NewPeriod == user.UserPeriod)
            {
                if (student.ClassroomID > 0)
                {
                    classroomName = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomName;
                }
            }
            else
            {
                isExist = _studentPeriodsRepository.ExistStudentPeriods(student.SchoolID, student.StudentID, user.UserPeriod);
                if (isExist)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(student.SchoolID, student.StudentID, user.UserPeriod).ClassroomName;
                    student.ClassroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            //Sibling Name on Window Title
            //string siblingName = "";
            //var siblingID = 0;
            //var studentIDTmp = _studentRepository.GetStudent(studentID).SiblingID;
            //if (studentIDTmp != 0)
            //{
            //    var studentSibling = _studentRepository.GetStudent(studentIDTmp);
            //    siblingName = studentSibling.FirstName + " " + studentSibling.LastName;
            //    siblingID = studentIDTmp;
            //}

            string categoryName1 = "dicountName";
            string categoryName2 = "categoryName";
            string categoryName3 = "name";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName1 = "language1";
                categoryName2 = "language1";
                categoryName3 = "language1";
            }

          /*  var studentInstallment = _studentInstallmentRepository.GetStudentInstallmentByPeriod(student.SchoolID, user.UserPeriod, student.StudentID)*/;
            bool isFirst = true;
            //if (studentInstallment.Count() > 0) isFirst = false;

            var studentViewModel = new StudentViewModel
            {
                IsPermissionDiscount = isPermissionDiscount,
                IsPermissionFee = isPermissionFee,
                Period = period,
                UserID = userID,
                SchoolID = user.SchoolID,
                StudentID = studentID,
                ViewIsNew = IsNew,
                ViewIsFirst = isFirst,

                Student = student,
                StudentName = student.FirstName + " " + student.LastName,
                StudentClassroom = classroomName,
                ClassroomID = student.ClassroomID,
                StudentTemp = studentTemp,
                StudentAddress = studentAddress,
                StudentParentAddress = studentParentAddress,
                StudentFamilyAddress = studentFamilyAddress,
                StudentInvoiceAddress = studentInvoiceAddress,
                StudentNote = studentNote,
                PSerialNumber = pSerialNumber,
                Parameter = parameter,
                SchoolInfo = school,
                SelectedCulture = user.SelectedCulture.Trim(),
                SortType = school.SortType,
                SortOption = school.SortOption,
                SortIcon = sortIcon,
                IsExplanationShow = student.IsExplanationShow,
                SelectedSchoolCode = user.SelectedSchoolCode,

                //SiblingID = siblingID,
                //SiblingName = siblingName,

                RegistrationTypeSubID = 3,
                StatuCategorySubID = 4,
                StatuCategoryID = 10,
                StartDate = school.SchoolYearStart,
                EndDate = school.SchoolYearEnd,
                wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/"), //Picture Path
                CategoryName1 = categoryName1,
                CategoryName2 = categoryName2,
                CategoryName3 = categoryName3
            };
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("M210Student/Preregistration/{studentViewModel}")]
        public async Task<IActionResult> PreregistrationUpdate(StudentViewModel studentViewModel)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(studentViewModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var gender = _parameterRepository.GetParameter(studentViewModel.Student.GenderTypeCategoryID);
            studentViewModel.Student.GenderTypeCategoryID = gender.CategoryID;
            if (studentViewModel.Student.StudentPicture == null || studentViewModel.Student.StudentPicture == "male.jpg" || studentViewModel.Student.StudentPicture == "female.jpg")
            {
                if (gender.CategoryName == "Erkek" || gender.Language1 == "Male")
                    studentViewModel.Student.StudentPicture = "male.jpg";
                else studentViewModel.Student.StudentPicture = "female.jpg";
            }
            if (!ModelState.IsValid)
                ViewBag.IsSuccess = true;

            if (ModelState.IsValid)
            {
                //go on as normal
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
            }

            var statuCategoryID = _parameterRepository.GetParameterCategoryName("Ön Kayıt").CategoryID;
            studentViewModel.Student.StatuCategoryID = statuCategoryID;

            statuCategoryID = _parameterRepository.GetParameterCategoryName("Yeni Kayıt").CategoryID;
            studentViewModel.Student.RegistrationTypeCategoryID = statuCategoryID;

            studentViewModel.Student.IsActive = true;

            if (ModelState.IsValid)
            {
                if (studentViewModel.ViewIsNew)
                {
                    if (studentViewModel.SchoolID > 0 && studentViewModel.Student.SchoolID == 0) studentViewModel.Student.SchoolID = studentViewModel.SchoolID;

                    var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
                    if (serialNumbers == null)
                        serialNumbers = new PSerialNumber();

                    var lastNumber = serialNumbers.RegisterNo += 1;
                    studentViewModel.Student.StudentSerialNumber = lastNumber;

                    studentViewModel.Student.FirstDateOfRegistration = studentViewModel.Student.DateOfRegistration;

                    var studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, studentViewModel.Period, studentViewModel.Student.StudentID);
                    studentViewModel.Student.IsPension = false;
                    if (studentTemp.IsPension) studentViewModel.Student.IsPension = true;
                    _studentRepository.UpdateStudent(studentViewModel.Student);

                    if (serialNumbers.PSerialNumberID == 0 && studentViewModel.ViewIsNew == true)
                    {
                        serialNumbers.PSerialNumberID = studentViewModel.UserID;
                        serialNumbers.RegisterNo = lastNumber;
                        _pSerialNumberRepository.CreatePSerialNumber(serialNumbers);
                    }
                    else
                    {
                        serialNumbers.RegisterNo = lastNumber;
                        _pSerialNumberRepository.UpdatePSerialNumber(serialNumbers);
                    }

                }
                else
                {
                    if (studentViewModel.Student.FirstDateOfRegistration == null)
                        studentViewModel.Student.FirstDateOfRegistration = studentViewModel.Student.DateOfRegistration;
                    if (studentViewModel.SchoolID > 0 && studentViewModel.Student.SchoolID == 0) studentViewModel.Student.SchoolID = studentViewModel.SchoolID;
                    _studentRepository.UpdateStudent(studentViewModel.Student);

                }

                if (!studentViewModel.StudentAddress.isEmpty)
                {
                    if (studentViewModel.StudentAddress.StudentAddressID == 0)
                    {
                        studentViewModel.StudentAddress.StudentID = studentViewModel.Student.StudentID;
                        _studentAddressRepository.CreateStudentAddress(studentViewModel.StudentAddress);
                    }
                    else
                    {
                        _studentAddressRepository.UpdateStudentAddress(studentViewModel.StudentAddress);
                    }
                }

                if (!studentViewModel.StudentParentAddress.isEmpty)
                {
                    if (studentViewModel.StudentParentAddress.StudentParentAddressID == 0)
                    {
                        studentViewModel.StudentParentAddress.StudentID = studentViewModel.Student.StudentID;
                        _studentParentAddressRepository.CreateStudentParentAddress(studentViewModel.StudentParentAddress);
                    }
                    else
                    {
                        _studentParentAddressRepository.UpdateStudentParentAddress(studentViewModel.StudentParentAddress);
                    }
                }

                ViewBag.IsSuccess = false;

                var period = studentViewModel.Period;
                bool isExist = _studentInstallmentRepository.ExistStudentInstallment(studentViewModel.Student.StudentID);
            }

            if (school.NewPeriod != user.UserPeriod)
            {
                //New classroom record must be zero in Old 'Student' Table
                studentViewModel.Student.ClassroomID = 0;
                _studentRepository.UpdateStudent(studentViewModel.Student);
            }

            var studentDebt = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, studentViewModel.Student.StudentID);
            decimal totalAmount = studentDebt.Sum(p => p.UnitPrice);

            string url = "/M210Student/Login";
            return Redirect(url);
        }

        public async Task<IActionResult> Login()
        {
            var cookieschoolCode = _httpContextAccessor.HttpContext.Request.Cookies["schoolCode"];
            var cookieculture = _httpContextAccessor.HttpContext.Request.Cookies["culture"];
            int schoolCode = Convert.ToInt32(cookieschoolCode);
            await Task.Delay(100);

            string culture = "";
            if (cookieculture == null)
                culture = System.Globalization.CultureInfo.CurrentUICulture.ToString();
            else culture = cookieculture;

            var schoolViewModel = new SchoolViewModel
            {
                SelectedCulture = culture,
            };
            return View(schoolViewModel);
        }

        [HttpPost]
        [Route("M210Student/LoginControlDataRead/{schoolCode}")]
        public async Task<IActionResult> LoginControlDataRead(int schoolCode)
        {
            await Task.Delay(100);
            bool isExist = false;

            var customer = _customersRepository.GetCustomer(schoolCode);

            if (customer != null)
            {
                if (ModelState.IsValid)
                {
                    if (customer.CustomerID == schoolCode) isExist = true;
                }
            }

            if (isExist == true)
            {
                var cookieschoolCode = "schoolCode";
                var cookie1 = _httpContextAccessor.HttpContext.Request.Cookies[cookieschoolCode];

                if (cookie1 != null)
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieschoolCode);

                CookieOptions cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(10)
                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieschoolCode, schoolCode.ToString(), cookieOptions);
            }

            return Json(new { IsExist = isExist });
        }

        [HttpPost]
        [Route("M210Student/PreregistrationRead/{idNumber}")]
        public IActionResult PreregistrationRead(string idNumber)
        {
            int studentID = 0;
            var student = new Student();
            if (idNumber != null)
            {
                student = _studentRepository.GetStudentIdNumber(idNumber);
                if (student != null)
                    studentID = student.StudentID;
            }
            return Json(new { StudentID = studentID });
        }
        #endregion

        #region Scheduler
        public IActionResult Guidance(int userID, int studentID, int taskTypeID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);


            var categoryID = _parameterRepository.GetParameterCategoryName("Öğrenci İşaretleyicileri").CategoryID;
            var color = _parameterRepository.GetParameterSubID(categoryID);
            int inx = _parameterRepository.GetParameterSubID(categoryID).First().CategoryID;

            var taskViewModel = new TaskViewModel
            {
                UserID = userID,
                SchoolID = user.SchoolID,
                SelectedCulture = user.SelectedCulture.Trim(),
                ResourceDefaultValue = inx,
                TaskTypeID = taskTypeID,
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

        [Route("M210Student/StudentTaskGridDataRead/{userID}/{classroomID}")]
        public IActionResult StudentTaskGridDataRead(int userID, int classroomID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());

            List<Student> student = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                if (classroomID == 0)
                    student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                else student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID && s.ClassroomID == classroomID).Count() > 0).ToList();
            }
            else
            {
                if (classroomID == 0)
                    student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
                else student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).Where(s => s.ClassroomID == classroomID).ToList();
            }

            bool isExist = false;
            string classroomName = "";
            List<TaskViewModel> list = new List<TaskViewModel>();
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

                    //var statuCategory = statuCategories.FirstOrDefault(p => p.CategoryID == item.StatuCategoryID);
                    //var registrationTypeCategory = registrationTypeCategories.FirstOrDefault(p => p.CategoryID == item.RegistrationTypeCategoryID);

                    //string gendertxt = gender.FirstOrDefault(p => p.CategoryID == item.GenderTypeCategoryID).CategoryName;

                    var taskViewModel = new TaskViewModel();
                    {
                        taskViewModel.SchoolID = user.SchoolID;
                        taskViewModel.ViewModelID = item.StudentID;
                        taskViewModel.StudentID = item.StudentID;
                        taskViewModel.StudentPicture = item.StudentPicture;
                        taskViewModel.Name = item.FirstName + " " + item.LastName;
                        taskViewModel.StudentClassroom = classroomName;
                        taskViewModel.StudentNumber = item.StudentNumber;
                    };
                    list.Add(taskViewModel);
                }
            }
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return Json(list);
        }

        public IActionResult StudentTask(int userID, int studentID, int taskTypeID)
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

            string demoText = _studentRepository.GetStudent(studentID).FirstName + "_" + _studentRepository.GetStudent(studentID).LastName;
            string cleanedText = ClearTurkishCharacter(demoText);

            var categoryID = _parameterRepository.GetParameterCategoryName("Öğrenci İşaretleyicileri").CategoryID;
            var color = _parameterRepository.GetParameterSubID(categoryID);
            int inx = _parameterRepository.GetParameterSubID(categoryID).First().CategoryID;

            var taskViewModel = new TaskViewModel
            {
                UserID = userID,
                SchoolID = user.SchoolID,
                StudentID = studentID,
                SelectedCulture = user.SelectedCulture.Trim(),
                PdfName = cleanedText,
                ResourceDefaultValue = inx,
                TaskTypeID = taskTypeID,
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

        [Route("M210Student/TaskDataRead/{schoolID}/{userID}/{studentID}/{taskTypeID}")]
        public IActionResult TaskDataRead(int schoolID, int userID, int studentID, int taskTypeID)
        {
            var user = _usersRepository.GetUser(userID);
            var task = _studentTaskDataSourceRepository.GetStudentTaskAll(schoolID, studentID, taskTypeID);
            List<TaskViewModel> list = new List<TaskViewModel>();

            foreach (var item in task)
            {
                var taskViewModel = new TaskViewModel
                {
                    ViewModelID = item.TaskID,
                    TaskID = item.TaskID,
                    UserID = userID,
                    TaskTypeID = taskTypeID,
                    SchoolID = item.SchoolID,
                    StudentID = studentID,

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
                    Name = "Abdullah",
                };
                list.Add(taskViewModel);
            }

            return Json(list);
        }

        [Route("M210Student/TaskDataCreate/{strResult}/{schoolID}/{studentID}/{taskTypeID}")]
        public IActionResult TaskDataCreate([Bind(Prefix = "models")] string strResult, int schoolID, int studentID, int taskTypeID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentTaskDataSource>>(strResult);
            List<StudentTaskDataSource> list = new List<StudentTaskDataSource>();
            var task = new StudentTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                task = new StudentTaskDataSource();
                task.TaskID = 0;
                task.TaskTypeID = taskTypeID;
                task.SchoolID = schoolID;
                task.StudentID = studentID;
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
                    _studentTaskDataSourceRepository.CreateStudentTask(task);
                }
                i = i + 1;
            }
            return Json(task);
        }
        public IActionResult TaskDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentTaskDataSource>>(strResult);

            StudentTaskDataSource list = new StudentTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                var task = _studentTaskDataSourceRepository.GetStudentTask(json[i].SchoolID, json[i].StudentID, json[i].TaskID, json[i].TaskTypeID);

                task.TaskID = json[i].TaskID;
                task.TaskTypeID = json[i].TaskTypeID;
                task.SchoolID = json[i].SchoolID;
                task.StudentID = json[i].StudentID;
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
                    _studentTaskDataSourceRepository.UpdateStudentTask(task);
                }
                i = i + 1;
            }

            return Json(list);
        }
        [HttpPost]
        public IActionResult TaskDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentTaskDataSource>>(strResult);

            StudentTaskDataSource list = new StudentTaskDataSource();
            var i = 0;
            foreach (var item in json)
            {
                var task = _studentTaskDataSourceRepository.GetStudentTask(json[i].SchoolID, json[i].StudentID, json[i].TaskID, json[i].TaskTypeID);

                task.TaskID = json[i].TaskID;
                task.SchoolID = json[i].SchoolID;
                task.StudentID = json[i].StudentID;
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
                    _studentTaskDataSourceRepository.DeleteStudentTask(task);
                }
                i = i + 1;
            }

            return Json(list);
        }
        public IActionResult ResourceTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Öğrenci İşaretleyicileri").CategoryID;
            var user = _parameterRepository.GetParameterSubID(categoryID);
            return Json(user);
        }

        #endregion

    }
}


