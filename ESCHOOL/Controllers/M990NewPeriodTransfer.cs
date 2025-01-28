using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Controllers
{
    public class M990NewPeriodTransfer : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        IUsersRepository _usersRepository;
        IStudentRepository _studentRepository;
        IStudentNoteRepository _studentNoteRepository;
        IClassroomRepository _classroomRepository;
        IDiscountTableRepository _discountTableRepository;
        ISchoolBusServicesRepository _schoolBusServicesRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IParameterRepository _parameterRepository;
        IAccountCodesRepository _accountCodesRepository;
        ITempM101HeaderRepository _tempM101HeaderRepository;
        ITempM101Repository _tempM101Repository;

        IWebHostEnvironment _hostEnvironment;
        public M990NewPeriodTransfer(
            ISchoolInfoRepository schoolInfoRepository,
            IPSerialNumberRepository pSerialNumberRepository,
            IUsersRepository usersRepository,
            IStudentRepository studentRepository,
            IStudentNoteRepository studentNoteRepository,
            IClassroomRepository classroomRepository,
            IDiscountTableRepository discountTableRepository,
            ISchoolBusServicesRepository schoolBusServicesRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IParameterRepository parameterRepository,
            IAccountCodesRepository accountCodesRepository,
            ITempM101HeaderRepository tempM101HeaderRepository,
            ITempM101Repository tempM101Repository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _usersRepository = usersRepository;
            _studentRepository = studentRepository;
            _studentNoteRepository = studentNoteRepository;
            _classroomRepository = classroomRepository;
            _discountTableRepository = discountTableRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _parameterRepository = parameterRepository;
            _accountCodesRepository = accountCodesRepository;
            _tempM101Repository = tempM101Repository;
            _tempM101HeaderRepository = tempM101HeaderRepository;

            _hostEnvironment = hostEnvironment;
        }

        #region NewPeriodTransfer

        public IActionResult NewPeriodTransfer(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            int year = (DateTime.Now.Year);
            DateTime sYear = new DateTime(year, 1, 1);
            string newperiod = (sYear.ToString("yyyy") + "-" + (sYear.AddYears(1).ToString("yyyy")));
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),
                Periods = user.UserPeriod,

                OldPeriod = period,
                NewPeriod = newperiod,

                List01Options0 = true,
                List01Options1 = false,
                List01Options2 = true,
                List01Options3 = true,
                List01Options4 = true,
                List01Options5 = true,

                SchoolYearStart = school.SchoolYearStart,
                SchoolYearEnd = school.SchoolYearEnd,
                FinancialYearStart = school.FinancialYearStart,
                FinancialYearEnd = school.FinancialYearEnd,
            };
            ViewBag.IsSuccess = 0;
            if (msg == 1) { ViewBag.IsSuccess = 1; };
            if (msg == 2) { ViewBag.IsSuccess = 2; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = userDate;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> NewPeriodTransferInfo(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var isTransaction = true;
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var studens = _studentRepository.GetStudentAllWithClassroom(user.SchoolID);
            var newTransaction = _classroomRepository.GetClassroomPeriods(user.SchoolID, listPanelModel.NewPeriod);
            foreach (var item in newTransaction)
            {
                if (item.Period == listPanelModel.NewPeriod) { isTransaction = false; break; };
            }

            if (isTransaction)
            {
                foreach (var item in studens)
                {
                    //Student
                    if (listPanelModel.List01Options0) item.DateOfRegistration = null;
                    var categoryID = _parameterRepository.GetParameterCategoryName("Kayıt Yenileme").CategoryID;
                    item.RegistrationTypeCategoryID = categoryID;
                    item.ClassroomID = 0;
                    _studentRepository.UpdateStudent(item);

                    //StudentNote
                    var studentNote = _studentNoteRepository.GetStudentNote(item.StudentID);
                    if (studentNote != null)
                    {
                        if (listPanelModel.List01Options1)
                        {
                            studentNote.Note1 = null;
                            studentNote.Note2 = null;
                            studentNote.Note3 = null;
                            _studentNoteRepository.UpdateStudentNote(studentNote);
                        }
                    }
                }

                //Classroom
                var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
                foreach (var item in classroom)
                {
                    item.ClassroomID = 0;
                    item.Period = listPanelModel.NewPeriod;
                    _classroomRepository.CreateClassroom(item);
                }

                //DiscountTable
                var discountTable = _discountTableRepository.GetDiscountTablePeriod(user.UserPeriod, user.SchoolID);
                var newDiscountTable = _discountTableRepository.GetDiscountTablePeriod(listPanelModel.NewPeriod, user.SchoolID);
                if (newDiscountTable == null)
                {
                    foreach (var item in discountTable)
                    {
                        item.DiscountTableID = 0;
                        item.Period = listPanelModel.NewPeriod;
                        _discountTableRepository.CreateDiscountTable(item);
                    }
                }

                //BusServices
                var schoolBusServices = _schoolBusServicesRepository.GetSchoolBusServicesAll(user.SchoolID, user.UserPeriod);
                var newSchoolBusServices = _schoolBusServicesRepository.GetSchoolBusServicesAll(user.SchoolID, listPanelModel.NewPeriod);
                if (newSchoolBusServices == null)
                {
                    foreach (var item in schoolBusServices)
                    {
                        item.SchoolBusServicesID = 0;
                        item.Period = listPanelModel.NewPeriod;
                        _schoolBusServicesRepository.CreateSchoolBusServices(item);
                    }
                }

                //AccountCodes
                var accountCodes = _accountCodesRepository.GetAccountCodeAll(user.UserPeriod);
                var newAccountCodes = _accountCodesRepository.GetAccountCodeAll(listPanelModel.NewPeriod);
                if (newAccountCodes == null)
                {
                    foreach (var item in accountCodes)
                    {
                        item.AccountCodeID = 0;
                        item.Period = listPanelModel.NewPeriod;
                        _accountCodesRepository.CreateAccountCode(item);
                    }
                }

                //SchoolFeeTable
                var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTableAll(user.SchoolID, user.UserPeriod);
                var newSchoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTableAll(user.SchoolID, listPanelModel.NewPeriod);
                if (newSchoolFeeTable == null)
                {
                    foreach (var item in schoolFeeTable)
                    {
                        item.SchoolFeeTableID = 0;
                        item.Period = listPanelModel.NewPeriod;
                        _schoolFeeTableRepository.CreateSchoolFeeTable(item);
                    }
                }

                //PSerialNumber
                var pserialNumber = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
                if (pserialNumber != null)
                {
                    if (listPanelModel.List01Options2)
                    {
                        pserialNumber.RegisterNo = 0;
                        pserialNumber.AccountSerialNo = 0;
                    }

                    if (listPanelModel.List01Options3)
                    {
                        pserialNumber.CollectionNo = 0;
                        pserialNumber.CollectionReceiptNo = 0;
                        pserialNumber.PaymentNo = 0;
                        pserialNumber.PaymentReceiptNo = 0;
                    }

                    if (listPanelModel.List01Options4)
                    {
                        pserialNumber.BondNo = 0;
                        pserialNumber.CheckNo = 0;
                        pserialNumber.MailOrderNo = 0;
                        pserialNumber.OtsNo1 = 0;
                        pserialNumber.OtsNo2 = 0;
                        pserialNumber.CreditCardNo = 0;
                        pserialNumber.KmhNo = 0;
                        pserialNumber.MailOrderNo = 0;
                        pserialNumber.OtsNo1 = 0;
                    }
                }
                _pSerialNumberRepository.UpdatePSerialNumber(pserialNumber);

                //SchoolInfo
                var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
                school.SchoolYearStart = listPanelModel.SchoolYearStart;
                school.SchoolYearEnd = listPanelModel.SchoolYearEnd;
                school.FinancialYearStart = listPanelModel.FinancialYearStart;
                school.FinancialYearEnd = listPanelModel.FinancialYearEnd;
                school.NewPeriod = listPanelModel.NewPeriod;
                _schoolInfoRepository.UpdateSchoolInfo(school);
            }

            if (listPanelModel.List01Options5)
            {
                //Delete SQL
                MyAppSettingControl appSettings = new MyAppSettingControl();
                appSettings.GetAppSettingAndSqlDeleteFiles2("DevConnection", user.SelectedSchoolCode, user.SchoolID);
            }

            string url = "/M990NewPeriodTransfer/NewPeriodTransfer?userID=" + user.UserID + "&msg=1";
            if (isTransaction == false)
            {
                url = "/M990NewPeriodTransfer/NewPeriodTransfer?userID=" + user.UserID + "&msg=2";
            }

            return Redirect(url);
        }

        [Route("M990NewPeriodTransfer/PeriodDataRead/{plusYear}")]
        public IActionResult PeriodDataRead(int plusYear)
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, plusYear);
            return Json(mylist);
        }
        #endregion


    }
}