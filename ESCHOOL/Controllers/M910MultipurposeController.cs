﻿using ClosedXML.Excel;
using ESCHOOL.Models;
using ESCHOOL.Resources;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace ESCHOOL.Controllers
{
    public class M910MultipurposeController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IClassroomRepository _classroomRepository;
        IParameterRepository _parameterRepository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        IMultipurposeListRepository _multipurposeListRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        ISchoolBusServicesRepository _schoolBusServicesRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        IStudentRepository _studentRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IStudentTempRepository _studentTempRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IStudentNoteRepository _studentNoteRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;

        IExcelDataRepository _excelDataRepository;
        IWebHostEnvironment _hostEnvironment;
        public M910MultipurposeController(
             IClassroomRepository classroomRepository,
             IStudentPeriodsRepository studentPeriodsRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IParameterRepository parameterRepository,
             IAccountingRepository accountingRepository,
             IAccountCodesRepository accountCodesRepository,
             IMultipurposeListRepository multipurposeListRepository,
             IPSerialNumberRepository pSerialNumberRepository,
             IUsersRepository usersRepository,
             IUsersWorkAreasRepository usersWorkAreasRepository,
             ISchoolBusServicesRepository schoolBusServicesRepository,
             ISchoolFeeRepository schoolFeeRepository,
             IStudentAddressRepository studentAddressRepository,
             IStudentParentAddressRepository studentParentAddressRepository,
             IStudentFamilyAddressRepository studentFamilyAddressRepository,
             IStudentTempRepository studentTempRepository,
             IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
             IStudentNoteRepository studentNoteRepository,
             IStudentDebtRepository studentDebtRepository,
             IStudentPaymentRepository studentPaymentRepository,
             IStudentInvoiceRepository studentInvoiceRepository,
             IStudentInstallmentRepository studentInstallmentRepository,
             IStudentRepository studentRepository,
             IExcelDataRepository excelDataRepository,

        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _classroomRepository = classroomRepository;
            _parameterRepository = parameterRepository;
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountCodesRepository;
            _multipurposeListRepository = multipurposeListRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _studentRepository = studentRepository;
            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _studentTempRepository = studentTempRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _studentNoteRepository = studentNoteRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentPaymentRepository = studentPaymentRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInstallmentRepository = studentInstallmentRepository;

            _excelDataRepository = excelDataRepository;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult MultipurposeList(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Girişi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            ViewBag.date = user.UserDate;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

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
                UserID = userID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName1 = categoryName1,
                CategoryName2 = categoryName2,
            };
            return View(studentViewModel);
        }

        [Route("M910Multipurpose/MultipurposeListRead/{userID}")]
        public IActionResult MultipurposeListRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());

            int ID = userID;
            var multipurposeList = _multipurposeListRepository.GetMultipurposeListAll();
            if (multipurposeList.Count() == 0)
            {
                MultipurposeListDefaultCreate(ID);
                multipurposeList = _multipurposeListRepository.GetMultipurposeListAll();
            }
            return Json(multipurposeList);
        }

        [HttpPost]
        [Route("M910Multipurpose/MultipurposeListUpdate/{strResult}/{userID}")]
        public IActionResult MultipurposeListUpdate([Bind(Prefix = "models")] string strResult, int userID)
        {
            var json = new JavaScriptSerializer().Deserialize<List<MultipurposeList>>(strResult);

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            List<ExcelData> list = new List<ExcelData>();

            var i = 0;
            foreach (var item in json)
            {
                var getCode = _multipurposeListRepository.GetMultipurposeListID(json[i].MultipurposeListID);

                getCode.MultipurposeListID = json[i].MultipurposeListID;
                getCode.Name = json[i].Name;
                getCode.Conditions1 = json[i].Conditions1;
                getCode.Conditions2 = json[i].Conditions2;
                getCode.IsSelect = json[i].IsSelect;

                if (ModelState.IsValid)
                {
                    _multipurposeListRepository.UpdateMultipurposeList(getCode);
                    _multipurposeListRepository.Save();
                }
                i = i + 1;
            }
            ExcelCreateData(userID);

            return Json(list);
        }

        public void ExcelCreateData(int userID)
        {

            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(c => c.FirstName).OrderBy(b => b.ClassroomID).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(c => c.FirstName).OrderBy(b => b.ClassroomID).ToList();
            }

            var mp = _multipurposeListRepository.GetMultipurposeListAll();

            var excelRemove = _excelDataRepository.GetExcelData();
            foreach (var item in excelRemove)
            {
                _excelDataRepository.DeleteExcelData(item);
            }

            var studensAddressAll = _studentAddressRepository.GetStudentAddressAll();
            var studensParentAddressAll = _studentParentAddressRepository.GetStudentParentAddressAll();
            var studensFamilyAddressAll = _studentFamilyAddressRepository.GetStudentFamilyAddressAll();
            var studentTempAll = _studentTempRepository.GetStudentTempAll(user.SchoolID, user.UserPeriod);
            var studentInvoiceAddressAll = _studentInvoiceAddressRepository.GetStudentInvoiceAddressAll();
            var studentNoteAll = _studentNoteRepository.GetStudentNoteAll();
            var feesAll = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1");
            var studentPaymentsAll = _studentPaymentRepository.GetStudentPaymentAll(user.SchoolID, user.UserPeriod);
            var studentInvoiceAll = _studentInvoiceRepository.GetInvoiceAll(user.SchoolID, user.UserPeriod);

            decimal bank = 0, handPayment = 0, check = 0, kmh = 0, creditCard = 0, mailOrder = 0, ots1 = 0, ots2 = 0;
            var categoryID = _parameterRepository.GetParameterCategoryName("Banka").CategoryID;
            var installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            bank += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("Elden").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            handPayment += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("Çek").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            check += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("KMH").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            kmh += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("Kredi kartı").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            creditCard += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("Mail order").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            mailOrder += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("OTS_1").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            ots1 += installment.Sum(p => p.InstallmentAmount);

            categoryID = _parameterRepository.GetParameterCategoryName("OTS_2").CategoryID;
            installment = _studentInstallmentRepository.GetStudentInstallmentAll(user.SchoolID, user.UserPeriod, categoryID);
            ots2 += installment.Sum(p => p.InstallmentAmount);

            var p = _parameterRepository.GetParameterAll();



            List<ExcelData> list = new List<ExcelData>();
            int conditions1 = 0;
            int conditions2 = 0;
            int conditions11 = 0;
            int conditions22 = 0;

            string studentNumber = "";
            int studentSerialNumber = 0, scholarshipRate = 0;
            var inx = 0;
            foreach (var s in students)
            {
                var statuName = _parameterRepository.GetParameter(s.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var isExist = false;
                inx = 0;
                var evm = new ExcelData();

                var studensAddress = studensAddressAll.Where(b => b.StudentID == s.StudentID).FirstOrDefault();
                var studensParentAddress = studensParentAddressAll.Where(b => b.StudentID == s.StudentID).FirstOrDefault();
                var studensFamilyAddress = studensFamilyAddressAll.Where(b => b.StudentID == s.StudentID).FirstOrDefault();
                var studentTemp = studentTempAll.Where(b => b.SchoolID == user.SchoolID && b.Period == user.UserPeriod && b.StudentID == s.StudentID).FirstOrDefault();
                var studentInvoiceAddress = studentInvoiceAddressAll.Where(b => b.StudentID == s.StudentID).FirstOrDefault();
                var studentNote = studentNoteAll.Where(b => b.StudentID == s.StudentID).FirstOrDefault();
                var fees = feesAll.Where(b => b.SchoolID == s.SchoolID && b.CategoryLevel == "L1");

                decimal payment = 0, balance = 0, cash = 0, cutInvoice = 0, uncutInvoice = 0, totalRefund = 0;
                var studentPayments = studentPaymentsAll.Where(b => b.SchoolID == s.SchoolID && b.Period == user.UserPeriod && b.StudentID == s.StudentID);

                var studentDebtAll = _studentDebtRepository.GetDebtAll(user.SchoolID, user.UserPeriod);
                var classroomNameAll = _studentPeriodsRepository.GetStudentAll(s.SchoolID, user.UserPeriod);
                cash = 0;
                totalRefund = 0;
                if (studentTemp != null)
                {
                    cash = studentTemp.CashPayment;
                    totalRefund = studentTemp.RefundAmount1 + studentTemp.RefundAmount2 + studentTemp.RefundAmount3;
                }
                payment += studentPayments.Sum(p => p.PaymentAmount) + cash;

                var studentInvoice1 = studentInvoiceAll.Where(b => b.Period == user.UserPeriod && b.SchoolID == user.SchoolID && b.StudentID == s.StudentID && b.InvoiceStatus == true);
                cutInvoice += Convert.ToDecimal(studentInvoice1.Sum(p => p.DAmount));
                var studentInvoice2 = studentInvoiceAll.Where(b => b.Period == user.UserPeriod && b.SchoolID == user.SchoolID && b.StudentID == s.StudentID && b.InvoiceStatus == false);
                uncutInvoice += Convert.ToDecimal(studentInvoice2.Sum(p => p.DAmount));

                foreach (var l in mp)
                {
                    if (l.Conditions1 == null || l.Conditions1 == "Seç" || l.Conditions1 == "Select") l.Conditions1 = "0";
                    if (l.Conditions2 == null || l.Conditions2 == "Seç" || l.Conditions1 == "Select") l.Conditions2 = "0";

                    inx++;
                    evm.ExcelNameSurname = s.FirstName + " " + s.LastName;
                    isExist = true;

                    if (inx == 001 && l.IsSelect == true)
                    {
                        if (s.StudentNumber != null && s.StudentNumber != "")
                            //studentNumber = Int32.Parse(s.StudentNumber);
                            studentNumber = s.StudentNumber;
                        //if (studentNumber != "")
                        //{
                        //    evm.Excel001 = s.StudentNumber;
                        //    isExist = true;
                        //}
                        //else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    #region conditions
                    if (inx == 002 && l.IsSelect == true)
                    {
                        bool isExist2 = false;
                        string classroomName = "";
                        int classroom = 0;
                        var classrooms = _classroomRepository.GetClassroomIDPeriod(user.UserPeriod, s.ClassroomID);
                        if (school.NewPeriod == user.UserPeriod)
                        {
                            if (s.ClassroomID > 0)
                            {
                                if (classrooms != null)
                                    classroomName = classrooms.ClassroomName;
                            }
                        }
                        else
                        {
                            isExist2 = _studentPeriodsRepository.ExistStudentPeriods(s.SchoolID, s.StudentID, user.UserPeriod);
                            if (isExist2)
                            {
                                classroomName = classroomNameAll.Where(b => b.SchoolID == s.SchoolID && b.StudentID == s.StudentID && b.Period == user.UserPeriod).FirstOrDefault().ClassroomName;
                                if (classroom == 0) classroom = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).SortOrder;
                            }
                        }


                        if (classrooms != null)
                            classroom = classrooms.SortOrder;
                        if (l.Conditions1 != "0")
                            conditions11 = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, l.Conditions1).SortOrder;
                        if (l.Conditions2 != "0")
                            conditions22 = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, l.Conditions2).SortOrder;

                        if ((classroom >= conditions11 && classroom <= conditions22) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel002 = classroomName;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 003 && l.IsSelect == true)
                    {
                        var gender = "";
                        var chk = p.Where(b => b.CategoryID == s.GenderTypeCategoryID);
                        if (chk != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && s.GenderTypeCategoryID > 0) gender = p.Where(b => b.CategoryID == s.GenderTypeCategoryID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && s.GenderTypeCategoryID > 0) gender = p.Where(b => b.CategoryID == s.GenderTypeCategoryID).SingleOrDefault().CategoryName;
                        }
                        if ((String.Compare(gender, l.Conditions1) == 0 || String.Compare(gender, l.Conditions1) > 0) &&
                            (String.Compare(gender, l.Conditions2) == 0 || String.Compare(gender, l.Conditions2) < 0)
                             || (l.Conditions1 == "0" && l.Conditions2 == "0"))
                        {
                            evm.Excel003 = gender;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 004 && l.IsSelect == true)
                    {
                        evm.Excel004 = s.IdNumber;
                        isExist = true;
                    }
                    if (inx == 005 && l.IsSelect == true)
                    {
                        var statu = "";
                        var chk = _parameterRepository.GetParameter(s.StatuCategoryID);
                        if (chk != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && s.StatuCategoryID > 0) statu = p.Where(b => b.CategoryID == s.StatuCategoryID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && s.StatuCategoryID > 0) statu = p.Where(b => b.CategoryID == s.StatuCategoryID).SingleOrDefault().Language1;
                        }
                        if ((String.Compare(statu, l.Conditions1) == 0 || String.Compare(statu, l.Conditions1) > 0) &&
                            (String.Compare(statu, l.Conditions2) == 0 || String.Compare(statu, l.Conditions2) < 0)
                             || (l.Conditions1 == "0" && l.Conditions2 == "0"))
                        {
                            evm.Excel005 = statu;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 006 && l.IsSelect == true)
                    {
                        var registerType = "";
                        var chk = _parameterRepository.GetParameter(s.RegistrationTypeCategoryID);
                        if (chk != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && s.RegistrationTypeCategoryID > 0) registerType = p.Where(b => b.CategoryID == s.RegistrationTypeCategoryID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && s.RegistrationTypeCategoryID > 0) registerType = p.Where(b => b.CategoryID == s.RegistrationTypeCategoryID).SingleOrDefault().Language1;
                        }
                        if ((String.Compare(registerType, l.Conditions1) == 0 || String.Compare(registerType, l.Conditions1) > 0) &&
                             (String.Compare(registerType, l.Conditions2) == 0 || String.Compare(registerType, l.Conditions2) < 0)
                              || (l.Conditions1 == "0" && l.Conditions2 == "0"))
                        {
                            evm.Excel006 = registerType;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 007 && l.IsSelect == true)
                    {
                        studentSerialNumber = s.StudentSerialNumber;
                        if ((studentSerialNumber >= conditions1 && studentSerialNumber <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel007 = s.StudentSerialNumber.ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 008 && l.IsSelect == true)
                    {
                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel008 = s.FirstDateOfRegistration.ToString();
                            isExist = true;
                        }
                        else
                        {
                            DateTime date1 = Convert.ToDateTime(l.Conditions1);
                            DateTime date2 = Convert.ToDateTime(l.Conditions2);
                            DateTime date = Convert.ToDateTime(s.FirstDateOfRegistration);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0) ||
                                (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel008 = date.ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                    }
                    if (inx == 009 && l.IsSelect == true)
                    {
                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel009 = s.DateOfRegistration.ToString();
                            isExist = true;
                        }
                        else
                        {
                            DateTime date1 = Convert.ToDateTime(l.Conditions1);
                            DateTime date2 = Convert.ToDateTime(l.Conditions2);
                            DateTime date = Convert.ToDateTime(s.DateOfRegistration);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                 (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel009 = date.ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                    }
                    if (inx == 010 && l.IsSelect == true)
                    {
                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel010 = s.DateOfBird.ToString();
                            isExist = true;
                        }
                        else
                        {
                            DateTime date1 = Convert.ToDateTime(l.Conditions1);
                            DateTime date2 = Convert.ToDateTime(l.Conditions2);
                            DateTime date = Convert.ToDateTime(s.DateOfBird);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                 (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel010 = date.ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                    }
                    if (inx == 011 && l.IsSelect == true)
                    {
                        evm.Excel011 = "";
                        if (studensAddress != null)
                            evm.Excel011 = studensAddress.Address1;
                        isExist = true;
                    }
                    if (inx == 012 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensAddress != null && studensAddress.TownParameterID1 != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensAddress.TownParameterID1 > 0) town = p.Where(b => b.CategoryID == studensAddress.TownParameterID1).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensAddress.TownParameterID1 > 0) town = p.Where(b => b.CategoryID == studensAddress.TownParameterID1).SingleOrDefault().Language1;
                        }
                        evm.Excel012 = town;
                        isExist = true;
                    }
                    if (inx == 013 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensAddress.CityParameterID1 > 0) city = p.Where(b => b.CategoryID == studensAddress.CityParameterID1).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensAddress.CityParameterID1 > 0) city = p.Where(b => b.CategoryID == studensAddress.CityParameterID1).SingleOrDefault().Language1;
                        }
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0) ||
                            (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel013 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 014 && l.IsSelect == true)
                    {
                        evm.Excel014 = "";
                        if (studensAddress != null)
                            evm.Excel014 = studensAddress.ZipCode1;
                        isExist = true;
                    }
                    if (inx == 015 && l.IsSelect == true)
                    {
                        evm.Excel015 = "";
                        if (studensAddress != null)
                            evm.Excel015 = studensAddress.MobilePhone;
                        isExist = true;
                    }
                    if (inx == 016 && l.IsSelect == true)
                    {
                        evm.Excel016 = "";
                        if (studensAddress != null)
                            evm.Excel016 = studensAddress.EMail;
                        isExist = true;
                    }
                    if (inx == 017 && l.IsSelect == true)
                    {
                        evm.Excel017 = "";
                        if (studensAddress != null)
                            evm.Excel017 = studensAddress.Address2;
                        isExist = true;
                    }
                    if (inx == 018 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensAddress != null && studensAddress.TownParameterID2 != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensAddress.TownParameterID2 > 0) town = p.Where(b => b.CategoryID == studensAddress.TownParameterID2).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensAddress.TownParameterID2 > 0) town = p.Where(b => b.CategoryID == studensAddress.TownParameterID2).SingleOrDefault().Language1;
                        }
                        evm.Excel018 = town;
                        isExist = true;
                    }
                    if (inx == 019 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensAddress.CityParameterID2 > 0) city = p.Where(b => b.CategoryID == studensAddress.CityParameterID2).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensAddress.CityParameterID2 > 0) city = p.Where(b => b.CategoryID == studensAddress.CityParameterID2).SingleOrDefault().Language1;
                        }
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0) ||
                            (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel019 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 020 && l.IsSelect == true)
                    {
                        evm.Excel020 = "";
                        if (studensAddress != null)
                            evm.Excel020 = studensAddress.ZipCode2;
                        isExist = true;
                    }
                    if (inx == 021 && l.IsSelect == true)
                    {
                        var busDeparture = "";
                        if (s.SchoolBusDepartureID != 0)
                            busDeparture = _schoolBusServicesRepository.GetSchoolBusServices(s.SchoolBusDepartureID).BusRoute;

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel021 = busDeparture;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(busDeparture, l.Conditions1) == 0 || String.Compare(busDeparture, l.Conditions1) > 0) &&
                            (String.Compare(busDeparture, l.Conditions2) == 0 || String.Compare(busDeparture, l.Conditions2) < 0))
                        {
                            evm.Excel021 = busDeparture;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 022 && l.IsSelect == true)
                    {
                        var busReturn = "";
                        if (s.SchoolBusReturnID != 0)
                            busReturn = _schoolBusServicesRepository.GetSchoolBusServices(s.SchoolBusReturnID).BusRoute;

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel022 = busReturn;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(busReturn, l.Conditions1) == 0 || String.Compare(busReturn, l.Conditions1) > 0) &&
                            (String.Compare(busReturn, l.Conditions2) == 0 || String.Compare(busReturn, l.Conditions2) < 0))
                        {
                            evm.Excel022 = busReturn;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 023 && l.IsSelect == true)
                    {
                        var busStatus = "";
                        if (s.SchoolBusStatuID != 0)
                        {
                            var chk = _parameterRepository.GetParameter(s.SchoolBusStatuID);
                            if (chk != null)
                            {
                                if (user.SelectedCulture.Trim() == "tr-TR" && s.SchoolBusStatuID > 0) busStatus = p.Where(b => b.CategoryID == s.SchoolBusStatuID).SingleOrDefault().CategoryName;
                                if (user.SelectedCulture.Trim() == "en-US" && s.SchoolBusStatuID > 0) busStatus = p.Where(b => b.CategoryID == s.SchoolBusStatuID).SingleOrDefault().Language1;
                            }
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel023 = busStatus;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(busStatus, l.Conditions1) == 0 || String.Compare(busStatus, l.Conditions1) > 0) &&
                            (String.Compare(busStatus, l.Conditions2) == 0 || String.Compare(busStatus, l.Conditions2) < 0))
                        {
                            evm.Excel023 = busStatus;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 024 && l.IsSelect == true)
                    {
                        evm.Excel024 = "";
                        if (studentInvoiceAddress != null)
                            evm.Excel024 = studentInvoiceAddress.InvoiceAddress;
                        isExist = true;
                    }
                    if (inx == 025 && l.IsSelect == true)
                    {
                        evm.Excel025 = "";
                        if (studentInvoiceAddress != null)
                            evm.Excel025 = studentInvoiceAddress.InvoiceTaxOffice;
                        isExist = true;
                    }
                    if (inx == 026 && l.IsSelect == true)
                    {
                        evm.Excel026 = "";
                        if (studentInvoiceAddress != null)
                            evm.Excel026 = studentInvoiceAddress.InvoiceTaxNumber;
                        isExist = true;
                    }
                    if (inx == 027 && l.IsSelect == true)
                    {
                        evm.Excel027 = s.ParentName;
                        isExist = true;
                    }
                    if (inx == 028 && l.IsSelect == true)
                    {
                        var kinship = "";
                        var kinshipID = _studentParentAddressRepository.GetStudentParentAddress(s.StudentID);
                        if (kinshipID != null)
                        {
                            var chk = _parameterRepository.GetParameter(kinshipID.KinshipCategoryID);
                            if (chk != null)
                            {
                                if (user.SelectedCulture.Trim() == "tr-TR" && kinshipID.KinshipCategoryID > 0) kinship = p.Where(b => b.CategoryID == kinshipID.KinshipCategoryID).SingleOrDefault().CategoryName;
                                if (user.SelectedCulture.Trim() == "en-US" && kinshipID.KinshipCategoryID > 0) kinship = p.Where(b => b.CategoryID == kinshipID.KinshipCategoryID).SingleOrDefault().Language1;
                            }
                        }

                        if ((String.Compare(kinship, l.Conditions1) == 0 || String.Compare(kinship, l.Conditions1) > 0) &&
                            (String.Compare(kinship, l.Conditions2) == 0 || String.Compare(kinship, l.Conditions2) < 0)
                             || (l.Conditions1 == "0" && l.Conditions2 == "0"))
                        {
                            evm.Excel028 = kinship;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 029 && l.IsSelect == true)
                    {
                        var profession = "";
                        var professionID = _studentParentAddressRepository.GetStudentParentAddress(s.StudentID);
                        if (professionID != null)
                        {
                            var chk = _parameterRepository.GetParameter(professionID.ProfessionCategoryID);
                            if (chk != null)
                            {
                                if (user.SelectedCulture.Trim() == "tr-TR" && professionID.ProfessionCategoryID > 0) profession = p.Where(b => b.CategoryID == professionID.ProfessionCategoryID).SingleOrDefault().CategoryName;
                                if (user.SelectedCulture.Trim() == "en-US" && professionID.ProfessionCategoryID > 0) profession = p.Where(b => b.CategoryID == professionID.ProfessionCategoryID).SingleOrDefault().Language1;
                            }
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel029 = profession;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(profession, l.Conditions1) == 0 || String.Compare(profession, l.Conditions1) > 0) &&
                            (String.Compare(profession, l.Conditions2) == 0 || String.Compare(profession, l.Conditions2) < 0))
                        {
                            evm.Excel029 = profession;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 030 && l.IsSelect == true)
                    {
                        evm.Excel030 = "";
                        if (studensParentAddress != null)
                            evm.Excel030 = studensParentAddress.IdNumber;
                        isExist = true;
                    }
                    if (inx == 031 && l.IsSelect == true)
                    {
                        evm.Excel031 = "";
                        if (studensParentAddress != null)
                            evm.Excel031 = studensParentAddress.MobilePhone;
                        isExist = true;
                    }
                    if (inx == 032 && l.IsSelect == true)
                    {
                        evm.Excel032 = "";
                        if (studensParentAddress != null)
                            evm.Excel032 = studensParentAddress.EMail;
                        isExist = true;
                    }
                    if (inx == 033 && l.IsSelect == true)
                    {
                        evm.Excel033 = "";
                        if (studensParentAddress != null)
                            evm.Excel033 = studensParentAddress.HomeAddress;
                        isExist = true;
                    }
                    if (inx == 034 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensParentAddress != null && studensParentAddress.HomeTownParameterID != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensParentAddress.HomeTownParameterID > 0) town = _parameterRepository.GetParameter(studensParentAddress.HomeTownParameterID).CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensParentAddress.HomeTownParameterID > 0) town = _parameterRepository.GetParameter(studensParentAddress.HomeTownParameterID).Language1;
                        }
                        evm.Excel034 = town;
                        isExist = true;
                    }
                    if (inx == 035 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensParentAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensParentAddress.HomeCityParameterID > 0) city = p.Where(b => b.CategoryID == studensParentAddress.HomeCityParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensParentAddress.HomeCityParameterID > 0) city = p.Where(b => b.CategoryID == studensParentAddress.HomeCityParameterID).SingleOrDefault().Language1;
                        }
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0) ||
                            (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel035 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 036 && l.IsSelect == true)
                    {
                        evm.Excel036 = "";
                        if (studensParentAddress != null)
                            evm.Excel036 = studensParentAddress.HomeZipCode;
                        isExist = true;
                    }
                    if (inx == 037 && l.IsSelect == true)
                    {
                        evm.Excel037 = "";
                        if (studensParentAddress != null)
                            evm.Excel037 = studensParentAddress.WorkAddress;
                        isExist = true;
                    }
                    if (inx == 038 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensParentAddress != null && studensParentAddress.WorkTownParameterID != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensParentAddress.WorkTownParameterID > 0) town = p.Where(b => b.CategoryID == studensParentAddress.WorkTownParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensParentAddress.WorkTownParameterID > 0) town = p.Where(b => b.CategoryID == studensParentAddress.WorkTownParameterID).SingleOrDefault().Language1;
                        }
                        evm.Excel038 = town;
                        isExist = true;
                    }
                    if (inx == 039 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensParentAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensParentAddress.WorkCityParameterID > 0) city = p.Where(b => b.CategoryID == studensParentAddress.WorkCityParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensParentAddress.WorkCityParameterID > 0) city = p.Where(b => b.CategoryID == studensParentAddress.WorkCityParameterID).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel039 = city;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0))
                        {
                            evm.Excel039 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 040 && l.IsSelect == true)
                    {
                        evm.Excel040 = "";
                        if (studensParentAddress != null)
                            evm.Excel040 = studensParentAddress.WorkZipCode;
                        isExist = true;
                    }
                    if (inx == 041 && l.IsSelect == true)
                    {
                        evm.Excel041 = "";
                        if (studensParentAddress != null)
                            evm.Excel041 = studensParentAddress.WorkPhone;
                        isExist = true;
                    }
                    if (inx == 042 && l.IsSelect == true)
                    {
                        evm.Excel042 = "";
                        if (studensParentAddress != null)
                            evm.Excel042 = studensParentAddress.Name3;
                        isExist = true;
                    }
                    if (inx == 043 && l.IsSelect == true)
                    {
                        evm.Excel043 = "";
                        if (studensParentAddress != null)
                            evm.Excel043 = studensParentAddress.MobilePhone3;
                        isExist = true;
                    }
                    if (inx == 044 && l.IsSelect == true)
                    {
                        evm.Excel044 = "";
                        if (studensParentAddress != null)
                            evm.Excel044 = studensParentAddress.HomePhone3;
                        isExist = true;
                    }
                    if (inx == 045 && l.IsSelect == true)
                    {
                        evm.Excel045 = "";
                        if (studensParentAddress != null)
                            evm.Excel045 = studensParentAddress.WorkPhone3;
                        isExist = true;
                    }
                    if (inx == 046 && l.IsSelect == true)
                    {
                        evm.Excel046 = "";
                        if (studensParentAddress != null)
                            evm.Excel046 = studensParentAddress.Address3;
                        isExist = true;
                    }
                    if (inx == 047 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensParentAddress != null && studensParentAddress.TownParameterID3 != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensParentAddress.TownParameterID3 > 0) town = _parameterRepository.GetParameter(studensParentAddress.TownParameterID3).CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensParentAddress.TownParameterID3 > 0) town = _parameterRepository.GetParameter(studensParentAddress.TownParameterID3).Language1;
                        }
                        evm.Excel047 = town;
                        isExist = true;
                    }
                    if (inx == 048 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensParentAddress != null && studensParentAddress.CityParameterID3 != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensParentAddress.CityParameterID3 > 0) city = p.Where(b => b.CategoryID == studensParentAddress.CityParameterID3).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensParentAddress.CityParameterID3 > 0) city = p.Where(b => b.CategoryID == studensParentAddress.CityParameterID3).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel048 = city;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0))
                        {
                            evm.Excel048 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 049 && l.IsSelect == true)
                    {
                        evm.Excel049 = "";
                        if (studensParentAddress != null)
                            evm.Excel049 = studensParentAddress.ZipCode3;
                        isExist = true;
                    }
                    if (inx == 50 && l.IsSelect == true)
                    {
                        evm.Excel050 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel050 = studensFamilyAddress.FatherName;
                        isExist = true;
                    }
                    if (inx == 51 && l.IsSelect == true)
                    {
                        evm.Excel051 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel051 = studensFamilyAddress.FatherIdNumber;
                        isExist = true;
                    }
                    if (inx == 052 && l.IsSelect == true)
                    {
                        var profession = "";
                        var professionID = _studentFamilyAddressRepository.GetStudentFamilyAddress(s.StudentID);
                        if (professionID != null)
                        {
                            var chk = _parameterRepository.GetParameter(professionID.FatherProfessionCategoryID);
                            if (chk != null)
                            {
                                if (user.SelectedCulture.Trim() == "tr-TR" && professionID.FatherProfessionCategoryID > 0) profession = p.Where(b => b.CategoryID == professionID.FatherProfessionCategoryID).SingleOrDefault().CategoryName;
                                if (user.SelectedCulture.Trim() == "en-US" && professionID.FatherProfessionCategoryID > 0) profession = p.Where(b => b.CategoryID == professionID.FatherProfessionCategoryID).SingleOrDefault().Language1;
                            }
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel052 = profession;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(profession, l.Conditions1) == 0 || String.Compare(profession, l.Conditions1) > 0) &&
                            (String.Compare(profession, l.Conditions2) == 0 || String.Compare(profession, l.Conditions2) < 0))
                        {
                            evm.Excel052 = profession;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 053 && l.IsSelect == true)
                    {
                        evm.Excel053 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel053 = studensFamilyAddress.FatherEMail;
                        isExist = true;
                    }
                    if (inx == 054 && l.IsSelect == true)
                    {
                        evm.Excel054 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel054 = studensFamilyAddress.FatherMobilePhone;
                        isExist = true;
                    }
                    if (inx == 055 && l.IsSelect == true)
                    {
                        evm.Excel055 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel055 = studensFamilyAddress.FatherHomePhone;
                        isExist = true;
                    }
                    if (inx == 056 && l.IsSelect == true)
                    {
                        evm.Excel056 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel056 = studensFamilyAddress.FatherHomeAddress;
                        isExist = true;
                    }
                    if (inx == 057 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensFamilyAddress != null && studensFamilyAddress.FatherHomeTownParameterID != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.FatherHomeTownParameterID > 0) town = _parameterRepository.GetParameter(studensFamilyAddress.FatherHomeTownParameterID).CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.FatherHomeTownParameterID > 0) town = _parameterRepository.GetParameter(studensFamilyAddress.FatherHomeTownParameterID).Language1;
                        }

                        evm.Excel057 = town;
                        isExist = true;
                    }
                    if (inx == 058 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensFamilyAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.FatherHomeCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.FatherHomeCityParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.FatherHomeCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.FatherHomeCityParameterID).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel058 = city;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0))
                        {
                            evm.Excel058 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 059 && l.IsSelect == true)
                    {
                        evm.Excel059 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel014 = studensFamilyAddress.FatherHomeZipCode;
                        isExist = true;
                    }
                    if (inx == 060 && l.IsSelect == true)
                    {
                        evm.Excel060 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel060 = studensFamilyAddress.FatherWorkPhone;
                        isExist = true;
                    }
                    if (inx == 061 && l.IsSelect == true)
                    {
                        evm.Excel061 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel061 = studensFamilyAddress.FatherWorkAddress;
                        isExist = true;
                    }
                    if (inx == 062 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensFamilyAddress != null && studensFamilyAddress.FatherWorkTownParameterID != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.FatherWorkTownParameterID > 0) town = p.Where(b => b.CategoryID == studensFamilyAddress.FatherWorkTownParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.FatherWorkTownParameterID > 0) town = p.Where(b => b.CategoryID == studensFamilyAddress.FatherWorkTownParameterID).SingleOrDefault().Language1;
                        }
                        evm.Excel062 = town;
                        isExist = true;
                    }
                    if (inx == 063 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensFamilyAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.FatherWorkCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.FatherWorkCityParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.FatherWorkCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.FatherWorkCityParameterID).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel063 = city;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0))
                        {
                            evm.Excel063 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 064 && l.IsSelect == true)
                    {
                        evm.Excel064 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel064 = studensFamilyAddress.FatherWorkZipCode;
                        isExist = true;
                    }
                    if (inx == 065 && l.IsSelect == true)
                    {
                        evm.Excel065 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel065 = studensFamilyAddress.MotherName;
                        isExist = true;
                    }
                    if (inx == 066 && l.IsSelect == true)
                    {
                        evm.Excel066 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel066 = studensFamilyAddress.MotherIdNumber;
                        isExist = true;
                    }
                    if (inx == 067 && l.IsSelect == true)
                    {
                        var profession = "";
                        var professionID = _studentFamilyAddressRepository.GetStudentFamilyAddress(s.StudentID);
                        if (professionID != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && professionID.MotherProfessionCategoryID > 0) profession = p.Where(b => b.CategoryID == professionID.MotherProfessionCategoryID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && professionID.MotherProfessionCategoryID > 0) profession = p.Where(b => b.CategoryID == professionID.MotherProfessionCategoryID).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel067 = profession;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(profession, l.Conditions1) == 0 || String.Compare(profession, l.Conditions1) > 0) &&
                            (String.Compare(profession, l.Conditions2) == 0 || String.Compare(profession, l.Conditions2) < 0))
                        {
                            evm.Excel067 = profession;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 068 && l.IsSelect == true)
                    {
                        evm.Excel068 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel068 = studensFamilyAddress.MotherEMail;
                        isExist = true;
                    }
                    if (inx == 069 && l.IsSelect == true)
                    {
                        evm.Excel069 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel069 = studensFamilyAddress.MotherMobilePhone;
                        isExist = true;
                    }
                    if (inx == 070 && l.IsSelect == true)
                    {
                        evm.Excel070 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel070 = studensFamilyAddress.MotherHomePhone;
                        isExist = true;
                    }
                    if (inx == 071 && l.IsSelect == true)
                    {
                        evm.Excel071 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel071 = studensFamilyAddress.MotherHomeAddress;
                        isExist = true;
                    }
                    if (inx == 072 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensFamilyAddress != null && studensFamilyAddress.MotherHomeTownParameterID != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.MotherHomeTownParameterID > 0) town = p.Where(b => b.CategoryID == studensFamilyAddress.MotherHomeTownParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.MotherHomeTownParameterID > 0) town = p.Where(b => b.CategoryID == studensFamilyAddress.MotherHomeTownParameterID).SingleOrDefault().Language1;
                        }

                        evm.Excel072 = town;
                        isExist = true;
                    }
                    if (inx == 073 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensFamilyAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.MotherHomeCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.MotherHomeCityParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.MotherHomeCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.MotherHomeCityParameterID).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel073 = city;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0))
                        {
                            evm.Excel073 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 074 && l.IsSelect == true)
                    {
                        evm.Excel074 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel074 = studensFamilyAddress.MotherHomeZipCode;
                        isExist = true;
                    }
                    if (inx == 075 && l.IsSelect == true)
                    {
                        evm.Excel075 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel075 = studensFamilyAddress.MotherWorkPhone;
                        isExist = true;
                    }
                    if (inx == 076 && l.IsSelect == true)
                    {
                        evm.Excel076 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel076 = studensFamilyAddress.MotherWorkAddress;
                        isExist = true;
                    }
                    if (inx == 077 && l.IsSelect == true)
                    {
                        var town = "";
                        if (studensFamilyAddress != null && studensFamilyAddress.MotherWorkTownParameterID != 0)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.MotherWorkTownParameterID > 0) town = p.Where(b => b.CategoryID == studensFamilyAddress.MotherWorkTownParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.MotherWorkTownParameterID > 0) town = p.Where(b => b.CategoryID == studensFamilyAddress.MotherWorkTownParameterID).SingleOrDefault().Language1;
                        }
                        evm.Excel077 = town;
                        isExist = true;
                    }
                    if (inx == 078 && l.IsSelect == true)
                    {
                        var city = "";
                        if (studensFamilyAddress != null)
                        {
                            if (user.SelectedCulture.Trim() == "tr-TR" && studensFamilyAddress.MotherWorkCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.MotherWorkCityParameterID).SingleOrDefault().CategoryName;
                            if (user.SelectedCulture.Trim() == "en-US" && studensFamilyAddress.MotherWorkCityParameterID > 0) city = p.Where(b => b.CategoryID == studensFamilyAddress.MotherWorkCityParameterID).SingleOrDefault().Language1;
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel078 = city;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(city, l.Conditions1) == 0 || String.Compare(city, l.Conditions1) > 0) &&
                            (String.Compare(city, l.Conditions2) == 0 || String.Compare(city, l.Conditions2) < 0))
                        {
                            evm.Excel078 = city;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 079 && l.IsSelect == true)
                    {
                        evm.Excel079 = "";
                        if (studensFamilyAddress != null)
                            evm.Excel079 = studensFamilyAddress.MotherEMail;
                        isExist = true;
                    }
                    if (inx == 080 && l.IsSelect == true)
                    {
                        evm.Excel080 = "";
                        if (studentNote != null)
                            evm.Excel080 = studentNote.Note1;
                        isExist = true;
                    }
                    if (inx == 081 && l.IsSelect == true)
                    {
                        evm.Excel081 = "";
                        if (studentNote != null)
                            evm.Excel081 = studentNote.Note2;
                        isExist = true;
                    }
                    if (inx == 082 && l.IsSelect == true)
                    {
                        evm.Excel082 = "";
                        if (studentNote != null)
                            evm.Excel082 = studentNote.Note3;
                        isExist = true;
                    }
                    if (inx == 083 && l.IsSelect == true)
                    {
                        var previousSchool = "";
                        if (s.PreviousSchoolID != 0)
                        {
                            var chk = _parameterRepository.GetParameter(s.PreviousSchoolID);
                            if (chk != null)
                            {
                                if (user.SelectedCulture.Trim() == "tr-TR" && s.PreviousSchoolID > 0) previousSchool = p.Where(b => b.CategoryID == s.PreviousSchoolID).SingleOrDefault().CategoryName;
                                if (user.SelectedCulture.Trim() == "en-US" && s.PreviousSchoolID > 0) previousSchool = p.Where(b => b.CategoryID == s.PreviousSchoolID).SingleOrDefault().Language1;
                            }
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel083 = previousSchool;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(previousSchool, l.Conditions1) == 0 || String.Compare(previousSchool, l.Conditions1) > 0) &&
                            (String.Compare(previousSchool, l.Conditions2) == 0 || String.Compare(previousSchool, l.Conditions2) < 0))
                        {
                            evm.Excel083 = previousSchool;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 084 && l.IsSelect == true)
                    {
                        var previousBranch = "";
                        if (s.PreviousBranchID != 0)
                        {
                            var chk = _parameterRepository.GetParameter(s.PreviousBranchID);
                            if (chk != null)
                            {
                                if (user.SelectedCulture.Trim() == "tr-TR" && s.PreviousBranchID > 0) previousBranch = p.Where(b => b.CategoryID == s.PreviousBranchID).SingleOrDefault().CategoryName;
                                if (user.SelectedCulture.Trim() == "en-US" && s.PreviousBranchID > 0) previousBranch = p.Where(b => b.CategoryID == s.PreviousBranchID).SingleOrDefault().Language1;
                            }
                        }

                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel084 = previousBranch;
                            isExist = true;
                        }
                        else
                        if ((String.Compare(previousBranch, l.Conditions1) == 0 || String.Compare(previousBranch, l.Conditions1) > 0) &&
                            (String.Compare(previousBranch, l.Conditions2) == 0 || String.Compare(previousBranch, l.Conditions2) < 0))
                        {
                            evm.Excel084 = previousBranch;
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 085 && l.IsSelect == true)
                    {
                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel085 = s.ScholarshipStartDate.ToString();
                            isExist = true;
                        }
                        else
                        {
                            DateTime date1 = Convert.ToDateTime(l.Conditions1);
                            DateTime date2 = Convert.ToDateTime(l.Conditions2);
                            DateTime date = Convert.ToDateTime(s.ScholarshipStartDate);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                 (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                            {
                                evm.Excel085 = date.ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                    }
                    if (inx == 086 && l.IsSelect == true)
                    {
                        if (l.Conditions1 == "0" || l.Conditions2 == "0")
                        {
                            evm.Excel086 = s.ScholarshipEndDate.ToString();
                            isExist = true;
                        }
                        else
                        {
                            DateTime date1 = Convert.ToDateTime(l.Conditions1);
                            DateTime date2 = Convert.ToDateTime(l.Conditions2);
                            DateTime date = Convert.ToDateTime(s.ScholarshipEndDate);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                 (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                            {
                                evm.Excel086 = date.ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                    }
                    if (inx == 087 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);
                        scholarshipRate = Convert.ToInt16(s.ScholarshipRate);

                        if ((scholarshipRate >= conditions1 && scholarshipRate <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel087 = s.ScholarshipRate.ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 088 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);
                        decimal cashPayment = studentTemp.CashPayment;

                        if ((cashPayment >= conditions1 && cashPayment <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel088 =  Math.Round(studentTemp.CashPayment, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 089 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((installment.Count() >= conditions1 && installment.Count() <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel089 = studentTemp.Installment.ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }

                    int i = 0;
                    decimal fee01 = 0, fee02 = 0, fee03 = 0, fee04 = 0, fee05 = 0, fee06 = 0, fee07 = 0, fee08 = 0, fee09 = 0, fee10 = 0, debt = 0, totalDiscount = 0, totalFee = 0;
                    if (inx == 090 || inx == 091 || inx == 092 || inx == 093 || inx == 094 || inx == 095 || inx == 096 || inx == 097 || inx == 098 || inx == 099)
                    {
                        foreach (var f in fees)
                        {
                            i++;
                            var studentDebt = studentDebtAll.Where(b => b.SchoolID == user.SchoolID && b.Period == user.UserPeriod && b.StudentID == s.StudentID && b.SchoolFeeID == f.SchoolFeeID).SingleOrDefault();
                            if (studentDebt != null)
                            {
                                debt += studentDebt.Amount;
                                totalDiscount += studentDebt.Discount;
                                totalFee += studentDebt.Amount;

                                if (i == 01) { fee01 = studentDebt.Amount; }
                                if (i == 02) { fee02 = studentDebt.Amount; }
                                if (i == 03) { fee03 = studentDebt.Amount; }
                                if (i == 04) { fee04 = studentDebt.Amount; }
                                if (i == 05) { fee05 = studentDebt.Amount; }
                                if (i == 06) { fee06 = studentDebt.Amount; }
                                if (i == 07) { fee07 = studentDebt.Amount; }
                                if (i == 08) { fee08 = studentDebt.Amount; }
                                if (i == 09) { fee09 = studentDebt.Amount; }
                                if (i == 10) { fee10 = studentDebt.Amount; }
                            }
                        }

                        if (inx == 090 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee01 >= conditions1 && fee01 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel090 = Math.Round(fee01, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 091 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee02 >= conditions1 && fee02 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel091 = Math.Round(fee02, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 092 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee03 >= conditions1 && fee03 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel092 = Math.Round(fee03, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 093 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee04 >= conditions1 && fee04 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel093 = Math.Round(fee04, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 094 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee05 >= conditions1 && fee05 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel094 = Math.Round(fee05, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 095 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee06 >= conditions1 && fee06 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel095 = Math.Round(fee06, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 096 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee07 >= conditions1 && fee07 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel096 = Math.Round(fee07, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 097 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee08 >= conditions1 && fee08 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel097 = Math.Round(fee08, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 098 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee09 >= conditions1 && fee09 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel098 = Math.Round(fee09, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                        if (inx == 099 && l.IsSelect == true)
                        {
                            conditions1 = Int32.Parse(l.Conditions1);
                            conditions2 = Int32.Parse(l.Conditions2);

                            if ((fee10 >= conditions1 && fee10 <= conditions2) ||
                                 (l.Conditions1 == "0" || l.Conditions2 == "0"))
                            {
                                evm.Excel099 = Math.Round(fee10, school.CurrencyDecimalPlaces).ToString();
                                isExist = true;
                            }
                            else if (l.IsSelect == true) { isExist = false; break; }
                        }
                    }

                    if (inx == 100 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((payment >= conditions1 && payment <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel100 = Math.Round(payment, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    balance = payment - debt;
                    if (inx == 101 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((balance >= conditions1 && balance <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel101 = Math.Round(balance, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 102 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((cutInvoice >= conditions1 && cutInvoice <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel102 = Math.Round(cutInvoice, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 103 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((uncutInvoice >= conditions1 && uncutInvoice <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel103 = Math.Round(uncutInvoice, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 104 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((bank >= conditions1 && bank <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel104 = Math.Round(bank, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 105 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((handPayment >= conditions1 && handPayment <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel105 = Math.Round(handPayment, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 106 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((check >= conditions1 && check <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel106 = Math.Round(check, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 107 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((ots1 >= conditions1 && ots1 <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel107 = Math.Round(ots1, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 108 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((ots2 >= conditions1 && ots2 <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel108 = Math.Round(ots2, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 109 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((mailOrder >= conditions1 && mailOrder <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel109 = Math.Round(mailOrder, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 110 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((creditCard >= conditions1 && creditCard <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel110 = Math.Round(creditCard, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 111 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((kmh >= conditions1 && kmh <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel111 = Math.Round(kmh, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 112 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((debt >= conditions1 && debt <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel112 = Math.Round(debt, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 113 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((totalRefund >= conditions1 && totalRefund <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel113 = Math.Round(totalRefund, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 114 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((totalDiscount >= conditions1 && totalDiscount <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel114 = Math.Round(totalDiscount, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    if (inx == 115 && l.IsSelect == true)
                    {
                        conditions1 = Int32.Parse(l.Conditions1);
                        conditions2 = Int32.Parse(l.Conditions2);

                        if ((totalFee >= conditions1 && totalFee <= conditions2) ||
                             (l.Conditions1 == "0" || l.Conditions2 == "0"))
                        {
                            evm.Excel115 = Math.Round(totalFee, school.CurrencyDecimalPlaces).ToString();
                            isExist = true;
                        }
                        else if (l.IsSelect == true) { isExist = false; break; }
                    }
                    #endregion
                }

                if (isExist)
                {
                    evm.ExcelId = 0;
                    _excelDataRepository.CreateExcelData(evm);
                }
            }
        }

        [Route("M910Multipurpose/ExportExcel/{userID}")]
        public IActionResult ExportExcel(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            using (var workbook = new XLWorkbook())
            {
                string title = "";
                if (user.SelectedCulture.Trim() == "en-US") title = "Multi-Purpose List";
                else title = "Çok Amaçlı Liste";

                var worksheet = workbook.Worksheets.Add(title);

                var mp = _multipurposeListRepository.GetMultipurposeListAll();
                var excelList = _excelDataRepository.GetExcelData();

                var s = 1;
                foreach (var m in mp)
                {
                    int rowCount = 2;
                    if (m.MultipurposeListID == 001) { worksheet.Cell(1, 1).Value = Resources.Resource.StudentName; }

                    if (m.IsSelect == true || rowCount == 2)
                    {
                        if (user.SelectedCulture.Trim() == "en-US") m.Name = m.Language1;

                        if (m.MultipurposeListID == 001 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 002 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 003 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 004 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 005 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 006 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 007 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 008 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 009 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 010 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 011 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 012 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 013 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 014 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 015 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 016 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 017 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 018 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 019 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 020 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 021 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 022 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 023 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 024 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 025 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 026 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 027 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 028 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 029 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 030 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 031 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 032 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 033 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 034 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 035 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 036 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 037 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 038 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 039 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 040 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 041 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 042 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 043 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 044 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 045 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 046 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 047 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 048 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 049 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 050 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }

                        if (m.MultipurposeListID == 051 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 052 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 053 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 054 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 055 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 056 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 057 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 058 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 059 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 060 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 061 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 062 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 063 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 064 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 065 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 066 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 067 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 068 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 069 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 070 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 071 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 072 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 073 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 074 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 075 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 076 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 077 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 078 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 079 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 080 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 081 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 082 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 083 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 084 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 085 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 086 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 087 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 088 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 089 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 090 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 091 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 092 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 093 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 094 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 095 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 096 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 097 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 098 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 099 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 100 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }

                        if (m.MultipurposeListID == 101 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 102 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 103 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 104 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 105 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 106 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 107 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 108 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 109 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 110 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 111 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 112 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 113 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 114 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }
                        if (m.MultipurposeListID == 115 && m.IsSelect == true) { s++; worksheet.Cell(1, s).Value = m.Name; }


                        foreach (var item in excelList)
                        {
                            if (m.MultipurposeListID == 001) { worksheet.Cell(rowCount, 1).Value = item.ExcelNameSurname; }
                            if (m.MultipurposeListID == 001 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel001; }
                            if (m.MultipurposeListID == 002 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel002; }
                            if (m.MultipurposeListID == 003 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel003; }
                            if (m.MultipurposeListID == 004 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel004; }
                            if (m.MultipurposeListID == 005 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel005; }
                            if (m.MultipurposeListID == 006 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel006; }
                            if (m.MultipurposeListID == 007 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel007; }
                            if (m.MultipurposeListID == 008 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel008; }
                            if (m.MultipurposeListID == 009 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel009; }
                            if (m.MultipurposeListID == 010 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel010; }
                            if (m.MultipurposeListID == 011 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel011; }
                            if (m.MultipurposeListID == 012 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel012; }
                            if (m.MultipurposeListID == 013 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel013; }
                            if (m.MultipurposeListID == 014 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel014; }
                            if (m.MultipurposeListID == 015 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel015; }
                            if (m.MultipurposeListID == 016 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel016; }
                            if (m.MultipurposeListID == 017 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel017; }
                            if (m.MultipurposeListID == 018 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel018; }
                            if (m.MultipurposeListID == 019 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel019; }
                            if (m.MultipurposeListID == 020 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel020; }
                            if (m.MultipurposeListID == 021 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel021; }
                            if (m.MultipurposeListID == 022 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel022; }
                            if (m.MultipurposeListID == 023 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel023; }
                            if (m.MultipurposeListID == 024 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel024; }
                            if (m.MultipurposeListID == 025 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel025; }
                            if (m.MultipurposeListID == 026 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel026; }
                            if (m.MultipurposeListID == 027 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel027; }
                            if (m.MultipurposeListID == 028 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel028; }
                            if (m.MultipurposeListID == 029 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel029; }
                            if (m.MultipurposeListID == 030 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel030; }
                            if (m.MultipurposeListID == 031 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel031; }
                            if (m.MultipurposeListID == 032 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel032; }
                            if (m.MultipurposeListID == 033 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel033; }
                            if (m.MultipurposeListID == 034 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel034; }
                            if (m.MultipurposeListID == 035 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel035; }
                            if (m.MultipurposeListID == 036 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel036; }
                            if (m.MultipurposeListID == 037 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel037; }
                            if (m.MultipurposeListID == 038 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel038; }
                            if (m.MultipurposeListID == 039 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel039; }
                            if (m.MultipurposeListID == 040 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel040; }
                            if (m.MultipurposeListID == 041 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel041; }
                            if (m.MultipurposeListID == 042 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel042; }
                            if (m.MultipurposeListID == 043 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel043; }
                            if (m.MultipurposeListID == 044 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel044; }
                            if (m.MultipurposeListID == 045 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel045; }
                            if (m.MultipurposeListID == 046 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel046; }
                            if (m.MultipurposeListID == 047 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel047; }
                            if (m.MultipurposeListID == 048 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel048; }
                            if (m.MultipurposeListID == 049 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel049; }
                            if (m.MultipurposeListID == 050 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel050; }

                            if (m.MultipurposeListID == 051 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel051; }
                            if (m.MultipurposeListID == 052 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel052; }
                            if (m.MultipurposeListID == 053 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel053; }
                            if (m.MultipurposeListID == 054 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel054; }
                            if (m.MultipurposeListID == 055 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel055; }
                            if (m.MultipurposeListID == 056 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel056; }
                            if (m.MultipurposeListID == 057 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel057; }
                            if (m.MultipurposeListID == 058 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel058; }
                            if (m.MultipurposeListID == 059 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel059; }
                            if (m.MultipurposeListID == 060 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel060; }
                            if (m.MultipurposeListID == 061 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel061; }
                            if (m.MultipurposeListID == 062 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel062; }
                            if (m.MultipurposeListID == 063 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel063; }
                            if (m.MultipurposeListID == 064 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel064; }
                            if (m.MultipurposeListID == 065 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel065; }
                            if (m.MultipurposeListID == 066 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel066; }
                            if (m.MultipurposeListID == 067 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel067; }
                            if (m.MultipurposeListID == 068 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel068; }
                            if (m.MultipurposeListID == 069 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel069; }
                            if (m.MultipurposeListID == 070 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel070; }
                            if (m.MultipurposeListID == 071 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel071; }
                            if (m.MultipurposeListID == 072 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel072; }
                            if (m.MultipurposeListID == 073 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel073; }
                            if (m.MultipurposeListID == 074 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel074; }
                            if (m.MultipurposeListID == 075 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel075; }
                            if (m.MultipurposeListID == 076 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel076; }
                            if (m.MultipurposeListID == 077 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel077; }
                            if (m.MultipurposeListID == 078 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel078; }
                            if (m.MultipurposeListID == 079 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel079; }
                            if (m.MultipurposeListID == 080 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel080; }
                            if (m.MultipurposeListID == 081 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel081; }
                            if (m.MultipurposeListID == 082 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel082; }
                            if (m.MultipurposeListID == 083 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel083; }
                            if (m.MultipurposeListID == 084 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel084; }
                            if (m.MultipurposeListID == 085 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel085; }
                            if (m.MultipurposeListID == 086 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel086; }
                            if (m.MultipurposeListID == 087 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel087; }
                            if (m.MultipurposeListID == 088 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel088; }
                            if (m.MultipurposeListID == 089 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel089; }
                            if (m.MultipurposeListID == 090 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel090; }
                            if (m.MultipurposeListID == 091 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel091; }
                            if (m.MultipurposeListID == 092 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel092; }
                            if (m.MultipurposeListID == 093 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel093; }
                            if (m.MultipurposeListID == 094 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel094; }
                            if (m.MultipurposeListID == 095 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel095; }
                            if (m.MultipurposeListID == 096 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel096; }
                            if (m.MultipurposeListID == 097 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel097; }
                            if (m.MultipurposeListID == 098 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel098; }
                            if (m.MultipurposeListID == 099 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel099; }
                            if (m.MultipurposeListID == 100 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel100; }

                            if (m.MultipurposeListID == 101 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel101; }
                            if (m.MultipurposeListID == 102 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel102; }
                            if (m.MultipurposeListID == 103 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel103; }
                            if (m.MultipurposeListID == 104 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel104; }
                            if (m.MultipurposeListID == 105 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel105; }
                            if (m.MultipurposeListID == 106 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel106; }
                            if (m.MultipurposeListID == 107 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel107; }
                            if (m.MultipurposeListID == 108 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel108; }
                            if (m.MultipurposeListID == 109 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel109; }
                            if (m.MultipurposeListID == 110 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel110; }
                            if (m.MultipurposeListID == 111 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel111; }
                            if (m.MultipurposeListID == 112 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel112; }
                            if (m.MultipurposeListID == 113 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel113; }
                            if (m.MultipurposeListID == 114 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel114; }
                            if (m.MultipurposeListID == 115 && m.IsSelect == true) { worksheet.Cell(rowCount, s).Value = item.Excel115; }
                            rowCount++;
                        }

                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    DateTime d = DateTime.Now;
                    string dateString = d.ToString("yyyy/MM/dd HH:mm");
                    string excelName = "Excel-" + dateString + ".xlsx";
                    return File(content, "application/vnd.opemxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }
        }

        [HttpPost]
        public IActionResult MultipurposeListRefresh()
        {
            var getCode = _multipurposeListRepository.GetMultipurposeListAll();
            foreach (var item in getCode)
            {
                item.IsSelect = false;
                item.Conditions1 = " ";
                item.Conditions2 = " ";

                if (ModelState.IsValid)
                {
                    _multipurposeListRepository.UpdateMultipurposeList(item);
                    _multipurposeListRepository.Save();
                }
            }
            return Json(getCode);
        }

        #region DefaultCreate
        public void MultipurposeListDefaultCreate(int ID)
        {
            var mp = new MultipurposeList();

            var user = _usersRepository.GetUser(ID);
            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            for (int i = 1; i < 116; i++)
            {
                mp.UserID = ID;
                mp.Condition = false;
                mp.Type = null;
                if (i == 001) { mp.Lenght = 010; mp.Name = Resources.Resource.StudentNumber1; mp.Condition = true; mp.Type = "string"; }
                if (i == 002) { mp.Lenght = 015; mp.Name = Resources.Resource.Classroom; mp.Condition = true; mp.Type = "dropdown1"; }
                if (i == 003) { mp.Lenght = 006; mp.Name = Resources.Resource.GenderType; mp.Condition = true; mp.Type = "dropdown2"; }
                if (i == 004) { mp.Lenght = 011; mp.Name = Resources.Resource.IdNumber; mp.Type = "string"; }
                if (i == 005) { mp.Lenght = 015; mp.Name = Resources.Resource.Status; mp.Condition = true; mp.Type = "dropdown3"; }
                if (i == 006) { mp.Lenght = 015; mp.Name = Resources.Resource.RegistrationType; mp.Condition = true; mp.Type = "dropdown4"; }
                if (i == 007) { mp.Lenght = 015; mp.Name = Resources.Resource.RegistrationSerialNumbers; mp.Condition = true; mp.Type = "string"; }
                if (i == 008) { mp.Lenght = 010; mp.Name = Resources.Resource.FirstDateOfRegistration; mp.Condition = true; mp.Type = "date"; }
                if (i == 009) { mp.Lenght = 010; mp.Name = Resources.Resource.DateOfRegistration; mp.Condition = true; mp.Type = "date"; }
                if (i == 010) { mp.Lenght = 010; mp.Name = Resources.Resource.DateOfBird; mp.Condition = true; mp.Type = "date"; }

                if (i == 011) { mp.Lenght = 100; mp.Name = Resources.Resource.Address11; }
                if (i == 012) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 013) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown5"; }
                if (i == 014) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }
                if (i == 015) { mp.Lenght = 015; mp.Name = Resources.Resource.MobilePhone; }
                if (i == 016) { mp.Lenght = 050; mp.Name = Resources.Resource.EMail; }

                if (i == 017) { mp.Lenght = 100; mp.Name = Resources.Resource.Address21; }
                if (i == 018) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 019) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown6"; }
                if (i == 020) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }

                if (i == 021) { mp.Lenght = 050; mp.Name = Resources.Resource.SchoolBusDeparture; mp.Condition = true; mp.Type = "dropdown14"; }
                if (i == 022) { mp.Lenght = 050; mp.Name = Resources.Resource.SchoolBusReturn; mp.Condition = true; mp.Type = "dropdown15"; }
                if (i == 023) { mp.Lenght = 050; mp.Name = Resources.Resource.SchoolBusStatus; mp.Condition = true; mp.Type = "dropdown16"; }

                if (i == 024) { mp.Lenght = 100; mp.Name = Resources.Resource.InvoiceAddress; }
                if (i == 025) { mp.Lenght = 030; mp.Name = Resources.Resource.TaxOffice; }
                if (i == 026) { mp.Lenght = 020; mp.Name = Resources.Resource.TaxNumber; }

                if (i == 027) { mp.Lenght = 050; mp.Name = Resources.Resource.ParentName; }
                if (i == 028) { mp.Lenght = 030; mp.Name = Resources.Resource.Kinship; mp.Condition = true; mp.Type = "dropdown17"; }
                if (i == 029) { mp.Lenght = 030; mp.Name = Resources.Resource.Profession; mp.Condition = true; mp.Type = "dropdown18"; }
                if (i == 030) { mp.Lenght = 015; mp.Name = Resources.Resource.IdNumber; }
                if (i == 031) { mp.Lenght = 015; mp.Name = Resources.Resource.MobilePhone; }
                if (i == 032) { mp.Lenght = 050; mp.Name = Resources.Resource.EMail; }
                if (i == 033) { mp.Lenght = 100; mp.Name = Resources.Resource.ParentHomeAddress; }
                if (i == 034) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 035) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown7"; }
                if (i == 036) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }
                if (i == 037) { mp.Lenght = 100; mp.Name = Resources.Resource.ParentWorkAddress; }
                if (i == 038) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 039) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown8"; }
                if (i == 040) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }
                if (i == 041) { mp.Lenght = 015; mp.Name = Resources.Resource.WorkPhone; }

                if (i == 042) { mp.Lenght = 030; mp.Name = Resources.Resource.Name3; }
                if (i == 043) { mp.Lenght = 015; mp.Name = Resources.Resource.MobilePhone; }
                if (i == 044) { mp.Lenght = 015; mp.Name = Resources.Resource.HomePhone; }
                if (i == 045) { mp.Lenght = 015; mp.Name = Resources.Resource.WorkPhone; }
                if (i == 046) { mp.Lenght = 100; mp.Name = Resources.Resource.HomeAddress; }
                if (i == 047) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 048) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown9"; }
                if (i == 049) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }

                if (i == 050) { mp.Lenght = 050; mp.Name = Resources.Resource.FatherName; }
                if (i == 051) { mp.Lenght = 015; mp.Name = Resources.Resource.IdNumber; }
                if (i == 052) { mp.Lenght = 030; mp.Name = Resources.Resource.Profession; mp.Condition = true; mp.Type = "dropdown19"; }
                if (i == 053) { mp.Lenght = 050; mp.Name = Resources.Resource.EMail; }
                if (i == 054) { mp.Lenght = 015; mp.Name = Resources.Resource.MobilePhone; }
                if (i == 055) { mp.Lenght = 015; mp.Name = Resources.Resource.HomePhone; }
                if (i == 056) { mp.Lenght = 100; mp.Name = Resources.Resource.HomeAddress; }
                if (i == 057) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 058) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown10"; }
                if (i == 059) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }
                if (i == 060) { mp.Lenght = 015; mp.Name = Resources.Resource.WorkPhone; }
                if (i == 061) { mp.Lenght = 100; mp.Name = Resources.Resource.WorkAddress; }
                if (i == 062) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 063) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown11"; }
                if (i == 064) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }

                if (i == 065) { mp.Lenght = 050; mp.Name = Resources.Resource.MotherName; }
                if (i == 066) { mp.Lenght = 015; mp.Name = Resources.Resource.IdNumber; }
                if (i == 067) { mp.Lenght = 030; mp.Name = Resources.Resource.Profession; mp.Condition = true; mp.Type = "dropdown20"; }
                if (i == 068) { mp.Lenght = 050; mp.Name = Resources.Resource.EMail; }
                if (i == 069) { mp.Lenght = 015; mp.Name = Resources.Resource.MobilePhone; }
                if (i == 070) { mp.Lenght = 015; mp.Name = Resources.Resource.HomePhone; }
                if (i == 071) { mp.Lenght = 100; mp.Name = Resources.Resource.HomeAddress; }
                if (i == 072) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 073) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown12"; }
                if (i == 074) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }
                if (i == 075) { mp.Lenght = 015; mp.Name = Resources.Resource.WorkPhone; }
                if (i == 076) { mp.Lenght = 100; mp.Name = Resources.Resource.WorkAddress; }
                if (i == 077) { mp.Lenght = 030; mp.Name = Resources.Resource.Town; }
                if (i == 078) { mp.Lenght = 030; mp.Name = Resources.Resource.City; mp.Condition = true; mp.Type = "dropdown13"; }
                if (i == 079) { mp.Lenght = 010; mp.Name = Resources.Resource.ZipCode; }

                if (i == 080) { mp.Lenght = 200; mp.Name = Resources.Resource.Note1; }
                if (i == 081) { mp.Lenght = 300; mp.Name = Resources.Resource.Note2; }
                if (i == 082) { mp.Lenght = 300; mp.Name = Resources.Resource.Note3; }
                if (i == 083) { mp.Lenght = 030; mp.Name = Resources.Resource.PreviousSchool; mp.Condition = true; mp.Type = "dropdown21"; }
                if (i == 084) { mp.Lenght = 030; mp.Name = Resources.Resource.PreviousBranch; mp.Condition = true; mp.Type = "dropdown22"; }
                if (i == 085) { mp.Lenght = 010; mp.Name = Resources.Resource.StartDate1; mp.Condition = true; mp.Type = "date"; }
                if (i == 086) { mp.Lenght = 010; mp.Name = Resources.Resource.EndDate1; mp.Condition = true; mp.Type = "date"; }
                if (i == 087) { mp.Lenght = 010; mp.Name = Resources.Resource.ScholarshipRate; mp.Condition = true; mp.Type = "string"; }

                if (i == 088) { mp.Lenght = 015; mp.Name = Resources.Resource.CashPayment; mp.Condition = true; }
                if (i == 089) { mp.Lenght = 015; mp.Name = Resources.Resource.Installment; mp.Condition = true; }

                if (i == 090 || i == 091 || i == 092 || i == 093 || i == 094 || i == 095 || i == 096 || i == 097 || i == 098 || i == 099)
                {
                    if (i == 090) mp.Name = Resources.Resource.Fees + " - " + 1;
                    if (i == 091) mp.Name = Resources.Resource.Fees + " - " + 2;
                    if (i == 092) mp.Name = Resources.Resource.Fees + " - " + 3;
                    if (i == 093) mp.Name = Resources.Resource.Fees + " - " + 4;
                    if (i == 094) mp.Name = Resources.Resource.Fees + " - " + 5;
                    if (i == 095) mp.Name = Resources.Resource.Fees + " - " + 6;
                    if (i == 096) mp.Name = Resources.Resource.Fees + " - " + 7;
                    if (i == 097) mp.Name = Resources.Resource.Fees + " - " + 8;
                    if (i == 098) mp.Name = Resources.Resource.Fees + " - " + 9;
                    if (i == 099) mp.Name = Resources.Resource.Fees + " - " + 10;

                    var inx = 0;
                    foreach (var item in fee)
                    {
                        inx++;

                        if (i == 090 && inx == 01) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 091 && inx == 02) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 092 && inx == 03) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 093 && inx == 04) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 0941 && inx == 05) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 095 && inx == 06) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 096 && inx == 07) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 097 && inx == 08) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 098 && inx == 09) { mp.Lenght = 015; mp.Name = item.Name; break; }
                        if (i == 099 && inx == 10) { mp.Lenght = 015; mp.Name = item.Name; break; }
                    }
                }

                if (i == 100) { mp.Lenght = 015; mp.Name = Resources.Resource.TotalCollection; }
                if (i == 101) { mp.Lenght = 015; mp.Name = Resources.Resource.TotalDebt; }
                if (i == 102) { mp.Lenght = 015; mp.Name = Resources.Resource.TotalInvoiced; }
                if (i == 103) { mp.Lenght = 015; mp.Name = Resources.Resource.TotalUnissuedlnvoiced; }
                if (i == 104) { mp.Lenght = 015; mp.Name = Resources.Resource.Bank; }
                if (i == 105) { mp.Lenght = 015; mp.Name = Resources.Resource.PromissoryNote; }
                if (i == 106) { mp.Lenght = 015; mp.Name = Resources.Resource.Check; }
                if (i == 107) { mp.Lenght = 015; mp.Name = Resources.Resource.Ots1; }
                if (i == 108) { mp.Lenght = 015; mp.Name = Resources.Resource.Ots2; }
                if (i == 109) { mp.Lenght = 015; mp.Name = Resources.Resource.MailOrder; }
                if (i == 110) { mp.Lenght = 015; mp.Name = Resources.Resource.CreditCard; }
                if (i == 111) { mp.Lenght = 015; mp.Name = Resources.Resource.KMH; }

                if (i == 112) { mp.Lenght = 015; mp.Name = Resources.Resource.TotalFee; }
                if (i == 113) { mp.Lenght = 015; mp.Name = Resources.Resource.Refund; }
                if (i == 114) { mp.Lenght = 015; mp.Name = Resources.Resource.TotalDiscount; }
                if (i == 115) { mp.Lenght = 015; mp.Name = Resources.Resource.Amount; }

                if (mp.Name != "")
                {
                    mp.MultipurposeListID = 0;
                    mp.IsSelect = false;
                    _multipurposeListRepository.CreateMultipurposeList(mp);
                }
            }
        }
        #endregion

        #region Combo
        [Route("M910Multipurpose/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }

        public IActionResult GenderTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Cinsiyeti").CategoryID;
            var gender = _parameterRepository.GetParameterSubID(categoryID);
            return Json(gender);
        }

        public IActionResult StatuCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Durumu").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            return Json(statu);
        }
        public IActionResult RegistrationTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Şekli").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(registrationType);
        }
        public IActionResult CityCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("İl ve İlçeler").CategoryID;
            var city = _parameterRepository.GetParameterSubID(categoryID);
            return Json(city);
        }

        [Route("M910Multipurpose/SchoolBusServiceDataRead/{userID}/")]
        public IActionResult SchoolBusServiceDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolBusServices = _schoolBusServicesRepository.GetSchoolBusServicesAll(user.SchoolID, user.UserPeriod);
            return Json(schoolBusServices);
        }

        public IActionResult ServicesStatusCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Servis Durumu").CategoryID;
            var statu = _parameterRepository.GetParameterSubID(categoryID);
            return Json(statu);
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

        #endregion
    }
}