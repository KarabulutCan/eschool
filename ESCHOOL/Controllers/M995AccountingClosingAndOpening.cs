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
    public class M995AccountingClosingAndOpening : Controller
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

        ITempM101HeaderRepository _tempM101HeaderRepository;
        ITempM101Repository _tempM101Repository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        IWebHostEnvironment _hostEnvironment;
        public M995AccountingClosingAndOpening(
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
            IAccountingRepository accountingRepository,
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
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountCodesRepository;
            _tempM101Repository = tempM101Repository;
            _tempM101HeaderRepository = tempM101HeaderRepository;

            _hostEnvironment = hostEnvironment;
        }

        #region AccountingClosingAndOpening

        public IActionResult AccountingClosingAndOpening(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);

            DecadeController periodList = new DecadeController();
            var period = periodList.Period();

            int year = (DateTime.Now.Year + 1);
            DateTime sYear = new DateTime(year, 1, 1);
            string newperiod = (sYear.ToString("yyyy") + "-" + (sYear.AddYears(1).ToString("yyyy")));
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),
                Periods = user.UserPeriod,

                OldPeriod = period,
                NewPeriod = newperiod,

                StartNumber = pserialNumbers.AccountSerialNo + 1,
                EndNumber = 1,

                List01Options0 = false,

                StartListDate = DateTime.Now,
                SchoolYearStart = school.SchoolYearStart,
                SchoolYearEnd = school.SchoolYearEnd,
                FinancialYearStart = school.FinancialYearStart,
                FinancialYearEnd = school.FinancialYearEnd,
            };
            ViewBag.IsSuccess = 0;
            if (msg == 1) { ViewBag.IsSuccess = 1; };
            if (msg == 2) { ViewBag.IsSuccess = 2; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> AccountingClosingAndOpeningInfo(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            string openingReceipt = Resources.Resource.OpeningReceipt;
            string closingReceipt = Resources.Resource.ClosingReceipt;

            var isTransaction = true;
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var pserialNumber = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            int documentNo1 = pserialNumber.PaymentNo + 1;
            int documentNo2 = pserialNumber.PaymentNo + 1;
            int sortorder1 = 0, sortorder2 = 0;
            decimal debt = 0, credit = 0;

            var accounCode = _accountCodesRepository.GetAccountCodeAllTrue(listPanelModel.Periods);
            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);

            if (listPanelModel.List01Options0 == true)
            {
                var accountingNew = _accountingRepository.GetAccountingAll(user.SchoolID, listPanelModel.NewPeriod);
                foreach (var item in accounting)
                {
                    if (item.Explanation == closingReceipt && item.Period == listPanelModel.OldPeriod) 
                        _accountingRepository.DeleteAccounting(item);
                }
                foreach (var item in accountingNew)
                {
                    if (item.Explanation == openingReceipt && item.Period == listPanelModel.NewPeriod)
                        _accountingRepository.DeleteAccounting(item);
                }
                accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);
            }
            else
            {
                foreach (var item in accounting)
                {
                    if (item.Explanation == closingReceipt && item.Period == listPanelModel.OldPeriod) { isTransaction = false; break; };
                    if (item.Explanation == openingReceipt && item.Period == listPanelModel.NewPeriod) { isTransaction = false; break; };
                }
            }

            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            if (isTransaction)
            {
                foreach (var item in accounCode)
                {
                    var codeTotal = accounting.Where(p => p.AccountCode == item.AccountCode);
                    debt = Convert.ToDecimal(codeTotal.Sum(p => p.Debt));
                    credit = Convert.ToDecimal(codeTotal.Sum(p => p.Credit));
                    if (debt >= credit) { debt -= credit; credit = 0; }
                    else { credit -= debt; debt = 0; }

                    if ((debt > 0 || credit > 0) && debt != credit)
                    {
                        var acc = new Accounting();

                        acc.AccountingID = 0;
                        acc.SchoolID = user.SchoolID;
                        acc.Period = listPanelModel.OldPeriod;
                        acc.VoucherTypeID = catID;
                        acc.VoucherNo = listPanelModel.StartNumber;
                        acc.AccountDate = listPanelModel.StartListDate;
                        acc.AccountCode = item.AccountCode;
                        acc.AccountCodeName = item.AccountCodeName;
                        acc.CodeTypeName = "";
                        acc.DocumentNumber = documentNo1.ToString();
                        acc.DocumentDate = listPanelModel.StartListDate;
                        acc.Explanation = closingReceipt;
                        acc.Debt = credit;
                        acc.Credit = debt;
                        sortorder1++;
                        acc.SortOrder = sortorder1;
                        acc.IsTransaction = true;
                        _accountingRepository.CreateAccounting(acc);

                        acc.AccountingID = 0;
                        acc.SchoolID = user.SchoolID;
                        acc.Period = listPanelModel.NewPeriod;
                        acc.VoucherTypeID = catID;
                        acc.VoucherNo = listPanelModel.EndNumber;
                        acc.AccountDate = listPanelModel.StartListDate;
                        acc.AccountCode = item.AccountCode;
                        acc.AccountCodeName = item.AccountCodeName;
                        acc.CodeTypeName = "";
                        acc.DocumentNumber = documentNo2.ToString();
                        acc.DocumentDate = listPanelModel.StartListDate;
                        acc.Explanation = openingReceipt;
                        acc.Debt = debt;
                        acc.Credit = credit;
                        sortorder2++;
                        acc.SortOrder = sortorder2;
                        acc.IsTransaction = true;
                        _accountingRepository.CreateAccounting(acc);
                    }
                }
                //PSerialNumber
                pserialNumber.AccountSerialNo = listPanelModel.EndNumber;
                pserialNumber.PaymentNo = documentNo2;
                _pSerialNumberRepository.UpdatePSerialNumber(pserialNumber);

                //SchoolInfo
                var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
                school.FinancialYearStart = listPanelModel.FinancialYearStart;
                school.FinancialYearEnd = listPanelModel.FinancialYearEnd;
                _schoolInfoRepository.UpdateSchoolInfo(school);
            }
            string url = "/M995AccountingClosingAndOpening/AccountingClosingAndOpening?userID=" + user.UserID + "&msg=1";
            if (isTransaction == false)
            {
                url = "/M995AccountingClosingAndOpening/AccountingClosingAndOpening?userID=" + user.UserID + "&msg=2";
            }

            return Redirect(url);
        }

        [Route("M995NewPeriodTransfer/PeriodDataRead/{plusYear}")]
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