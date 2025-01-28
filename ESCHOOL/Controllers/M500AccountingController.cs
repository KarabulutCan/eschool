using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ESCHOOL.Controllers
{
    public class M500AccountingController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IClassroomRepository _classroomRepository;
        IParameterRepository _parameterRepository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        IAccountCodesDetailRepository _accountCodesDetailRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInvoiceDetailRepository _studentInvoiceDetailRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IWebHostEnvironment _hostEnvironment;
        public M500AccountingController(
             IClassroomRepository classroomRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IParameterRepository parameterRepository,
             IAccountingRepository accountingRepository,
             IAccountCodesRepository accountCodesRepository,
             IAccountCodesDetailRepository accountCodesDetailRepository,
             IPSerialNumberRepository pSerialNumberRepository,
             IUsersRepository usersRepository,
             IUsersWorkAreasRepository usersWorkAreasRepository,
             IStudentInvoiceRepository studentInvoiceRepository,
             IStudentInvoiceDetailRepository studentInvoiceDetailRepository,
             IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
             IStudentRepository studentRepository,
             IStudentPeriodsRepository studentPeriodsRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _classroomRepository = classroomRepository;
            _parameterRepository = parameterRepository;
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountCodesRepository;
            _accountCodesDetailRepository = accountCodesDetailRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInvoiceDetailRepository = studentInvoiceDetailRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _hostEnvironment = hostEnvironment;

        }

        #region currentCard
        public IActionResult index(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Girişi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            var categoryNameID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var categories = _parameterRepository.GetParameterSubID(categoryNameID);
            var voucherTypeID = categories.FirstOrDefault(p => p.CategoryName == "Mahsup").CategoryID;

            int lastNumber = 0;

            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            lastNumber = serialNumbers.AccountSerialNo;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName = "language1";
            }

            var accountingViewModel = new AccountingViewModel
            {
                IsPermission = isPermission,
                StudentID = studentID,
                UserID = user.UserID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                VoucherTypeID = (int)voucherTypeID,
                VoucherNo = lastNumber,
                SchoolName = _schoolInfoRepository.GetSchoolInfo(user.SchoolID).CompanyName,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName,
            };
            return View(accountingViewModel);
        }

        public IActionResult currentCard(int userID, int accountCodeDetailID, int accountCodeID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Girişi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            var categoryNameID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var categories = _parameterRepository.GetParameterSubID(categoryNameID);
            var voucherTypeID = categories.FirstOrDefault(p => p.CategoryName == "Mahsup").CategoryID;

            var currentCard = _accountCodesDetailRepository.GetAccountCodesDetailID2(accountCodeDetailID, accountCodeID);

            if (currentCard == null)
            {
                currentCard = new AccountCodesDetail();
            }

            int lastNumber = 0;

            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            lastNumber = serialNumbers.AccountSerialNo;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName = "language1";
            }

            var accountingViewModel = new AccountingViewModel
            {
                IsPermission = isPermission,
                UserID = user.UserID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                VoucherTypeID = (int)voucherTypeID,
                VoucherNo = lastNumber,
                SchoolName = _schoolInfoRepository.GetSchoolInfo(user.SchoolID).CompanyName,
                SelectedCulture = user.SelectedCulture.Trim(),
                AccountCodesDetail = currentCard,
                CategoryName = categoryName,
            };
            return View(accountingViewModel);
        }

        public IActionResult currentCardRead(int userID, int accountCodeDetailID, int accountCodeID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Girişi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            var categoryNameID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var categories = _parameterRepository.GetParameterSubID(categoryNameID);
            var voucherTypeID = categories.FirstOrDefault(p => p.CategoryName == "Mahsup").CategoryID;

            var currentCard = _accountCodesDetailRepository.GetAccountCodesDetailID2(accountCodeDetailID, accountCodeID);

            if (currentCard == null)
            {
                currentCard = new AccountCodesDetail();
            }

            int lastNumber = 0;

            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            lastNumber = serialNumbers.AccountSerialNo;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName = "language1";
            }

            var accountingViewModel = new AccountingViewModel
            {
                IsPermission = isPermission,
                UserID = user.UserID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                VoucherTypeID = (int)voucherTypeID,
                VoucherNo = lastNumber,
                SchoolName = _schoolInfoRepository.GetSchoolInfo(user.SchoolID).CompanyName,
                SelectedCulture = user.SelectedCulture.Trim(),
                AccountCodesDetail = currentCard,
                CategoryName = categoryName,
            };

            return Json(new
            {
                accountCodeDetailId = currentCard.AccountCodeDetailID,
                accountCodeId = currentCard.AccountCodeID,
                authorizedPersonName = currentCard.AuthorizedPersonName,
                mobile = currentCard.Mobile,
                phone1 = currentCard.Phone1,

                bankName1 = currentCard.BankName1,
                bankIBAN1 = currentCard.BankIBAN1,
                bankName2 = currentCard.BankName2,
                bankIBAN2 = currentCard.BankIBAN2,
                explanation = currentCard.Explanation,

                invoiceTitle = currentCard.InvoiceTitle,
                invoiceAddress = currentCard.InvoiceAddress,
                invoiceTaxOffice = currentCard.InvoiceTaxOffice,
                invoiceTaxNumber = currentCard.InvoiceTaxNumber,
                invoiceCityDropDown = currentCard.InvoiceCityParameterID,
                invoiceTownDropDown = currentCard.InvoiceTownParameterID,
                invoiceCountry = currentCard.InvoiceCountry,
                invoiceZip = currentCard.InvoiceZipCode,
                phone2 = currentCard.Phone2,
                fax = currentCard.Fax,
                eMail = currentCard.EMail,
                webAddress = currentCard.WebAddress,
                notes = currentCard.Notes,
                invoiceProfile = currentCard.InvoiceProfile,
                invoiceTypeParameter = currentCard.InvoiceTypeParameter,
            });
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

        [Route("M500Accounting/GridAccountCodetDataRead/{userID}")]
        public IActionResult GridAccountCodetDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var accountCodes = _accountCodesRepository.GetCurrentCard(user.UserPeriod);

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            int ID = 0;
            List<AccountingViewModel> list = new List<AccountingViewModel>();
            foreach (var item in accountCodes)
            {
                var accountCodesDetail = _accountCodesDetailRepository.GetAccountCodesDetailID1(item.AccountCodeID);
                if (accountCodesDetail == null) { ID = 0;}
                else { ID = accountCodesDetail.AccountCodeDetailID; }
                var accountingViewModel = new AccountingViewModel
                {
                    AccountCode = item.AccountCode,
                    AccountCodeName = item.AccountCodeName,

                    AccountCodeDetailID = ID,
                    AccountCodeID = item.AccountCodeID,

                };
                list.Add(accountingViewModel);
            }
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> CurrentCard(AccountingViewModel accountingViewModel, int userID)
        {
            await Task.Delay(100);
            var accountDetail = _accountCodesDetailRepository.GetAccountCodesDetailID2(accountingViewModel.AccountCodesDetail.AccountCodeDetailID, accountingViewModel.AccountCodesDetail.AccountCodeID);
            if (accountDetail == null)
            {
                accountDetail = new AccountCodesDetail();
            }
            accountDetail.AccountCodeID = accountingViewModel.AccountCodesDetail.AccountCodeID;
            accountDetail.AuthorizedPersonName = accountingViewModel.AccountCodesDetail.AuthorizedPersonName;
            accountDetail.Mobile = accountingViewModel.AccountCodesDetail.Mobile;
            accountDetail.Phone1 = accountingViewModel.AccountCodesDetail.Phone1;

            accountDetail.BankName1 = accountingViewModel.AccountCodesDetail.BankName1;
            accountDetail.BankIBAN1 = accountingViewModel.AccountCodesDetail.BankIBAN1;
            accountDetail.BankName2 = accountingViewModel.AccountCodesDetail.BankName2;
            accountDetail.BankIBAN2 = accountingViewModel.AccountCodesDetail.BankIBAN2;
            accountDetail.Explanation = accountingViewModel.AccountCodesDetail.Explanation;

            accountDetail.InvoiceTitle = accountingViewModel.AccountCodesDetail.InvoiceTitle;
            accountDetail.InvoiceAddress = accountingViewModel.AccountCodesDetail.InvoiceAddress;
            accountDetail.InvoiceCityParameterID = accountingViewModel.AccountCodesDetail.InvoiceCityParameterID;
            accountDetail.InvoiceTownParameterID = accountingViewModel.AccountCodesDetail.InvoiceTownParameterID;
            accountDetail.InvoiceCountry = accountingViewModel.AccountCodesDetail.InvoiceCountry;
            accountDetail.InvoiceZipCode = accountingViewModel.AccountCodesDetail.InvoiceZipCode;
            accountDetail.InvoiceTaxOffice = accountingViewModel.AccountCodesDetail.InvoiceTaxOffice;
            accountDetail.InvoiceTaxNumber = accountingViewModel.AccountCodesDetail.InvoiceTaxNumber;
            accountDetail.EMail = accountingViewModel.AccountCodesDetail.EMail;
            accountDetail.WebAddress = accountingViewModel.AccountCodesDetail.WebAddress;
            accountDetail.Phone2 = accountingViewModel.AccountCodesDetail.Phone2;
            accountDetail.Fax = accountingViewModel.AccountCodesDetail.Fax;
            accountDetail.Notes = accountingViewModel.AccountCodesDetail.Notes;
            accountDetail.InvoiceProfile = accountingViewModel.AccountCodesDetail.InvoiceProfile;
            accountDetail.InvoiceTypeParameter = accountingViewModel.AccountCodesDetail.InvoiceTypeParameter;

            if (accountDetail == null)
            {
                accountDetail.AccountCodeDetailID = 0;
                _accountCodesDetailRepository.CreateAccountCodesDetail(accountDetail);
            }
            else
            {
                accountDetail.AccountCodeDetailID = accountingViewModel.AccountCodesDetail.AccountCodeDetailID; ;
                _accountCodesDetailRepository.UpdateAccountCodesDetail(accountDetail);
            }

            return View(accountingViewModel);
        }
        #endregion

        #region read

        [Route("M500Accounting/AccountingDataRead1/{userID}/{voucherNo}")]
        public IActionResult AccountingDataRead1(int userID, int voucherNo)
        {
            List<AccountingViewModel> list = new List<AccountingViewModel>();

            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Girişinde Kullanılan Kod").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID);
            var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, period, voucherNo);

            decimal totalDebit = 0, totalCredit = 0;
            foreach (var item in accounting)
            {
                var parameter = parameterList.FirstOrDefault(p => p.CategoryName == item.CodeTypeName);
                if (parameter == null)
                {
                    parameter = parameterList.FirstOrDefault(p => p.CategorySubID == categoryID);
                }

                string categoryName = "";
                if (user.SelectedCulture.Trim() == "en-US")
                    categoryName = parameter.Language1;
                else categoryName = parameter.CategoryName;

                totalDebit += (decimal)item.Debt;
                totalCredit += (decimal)item.Credit;
                var accountingViewModel = new AccountingViewModel
                {
                    SchoolID = item.SchoolID,
                    AccountingID = item.AccountingID,
                    Period = item.Period,
                    VoucherTypeID = item.VoucherTypeID,
                    VoucherNo = item.VoucherNo,
                    ProcessTypeName = item.ProcessTypeName,
                    AccountDate = item.AccountDate,
                    AccountCode = item.AccountCode,
                    AccountCodeName = item.AccountCodeName,
                    CodeTypeName = categoryName,
                    DocumentNumber = item.DocumentNumber,
                    DocumentDate = item.DocumentDate,
                    TaxNoOrId = item.TaxNoOrId,
                    Explanation = item.Explanation,
                    Debt = item.Debt,
                    Credit = item.Credit,
                    Balance = totalDebit - totalCredit,

                    SortOrder = item.SortOrder,
                    IsTransaction = item.IsTransaction,

                    ViewModelID = item.AccountingID,
                    Parameter = parameter,
                };

                list.Add(accountingViewModel);
            }
            return Json(list);
        }

        [Route("M500Accounting/AccountingDataRead2/{userID}/{accountCode}")]
        public IActionResult AccountingDataRead2(int userID, string accountCode)
        {
            List<AccountingViewModel> list = new List<AccountingViewModel>();

            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Girişinde Kullanılan Kod").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID);
            var accounting = _accountingRepository.GetAccountingCode(user.SchoolID, period, accountCode);

            decimal totalDebit = 0, totalCredit = 0;
            foreach (var item in accounting)
            {
                var parameter = parameterList.FirstOrDefault(p => p.CategoryName == item.CodeTypeName);
                if (parameter == null)
                {
                    parameter = parameterList.FirstOrDefault(p => p.CategorySubID == categoryID);
                }

                string categoryName = "";
                if (user.SelectedCulture.Trim() == "en-US")
                    categoryName = parameter.Language1;
                else categoryName = parameter.CategoryName;

                totalDebit += (decimal)item.Debt;
                totalCredit += (decimal)item.Credit;
                var accountingViewModel = new AccountingViewModel
                {
                    SchoolID = item.SchoolID,
                    AccountingID = item.AccountingID,
                    Period = item.Period,
                    VoucherTypeID = item.VoucherTypeID,
                    VoucherNo = item.VoucherNo,
                    ProcessTypeName = item.ProcessTypeName,
                    AccountDate = item.AccountDate,
                    AccountCode = item.AccountCode,
                    AccountCodeName = item.AccountCodeName,
                    CodeTypeName = categoryName,
                    DocumentNumber = item.DocumentNumber,
                    DocumentDate = item.DocumentDate,
                    TaxNoOrId = item.TaxNoOrId,
                    Explanation = item.Explanation,
                    Debt = item.Debt,
                    Credit = item.Credit,
                    Balance = totalDebit - totalCredit,

                    SortOrder = item.SortOrder,
                    IsTransaction = item.IsTransaction,

                    ViewModelID = item.AccountingID,
                    Parameter = parameter,
                };

                list.Add(accountingViewModel);
            }
            return Json(list);
        }

        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 1);
            return Json(mylist);
        }

        public IActionResult AccountTypeCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);

            return Json(registrationType);
        }

        [Route("M500Accounting/AccountingCodeDataRead/{period}")]
        public IActionResult AccountingCodeDataRead(string period)
        {
            var accountingCode = _accountCodesRepository.GetAccountCodeAllTrue(period);
            return Json(accountingCode);
        }

        public IActionResult CodeTypeDataRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Girişinde Kullanılan Kod").CategoryID;
            var codeType = _parameterRepository.GetParameterSubID(categoryID);

            return Json(codeType);
        }

        public IActionResult ProcessTypeDataRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe İşlem Tipi").CategoryID;
            var codeType = _parameterRepository.GetParameterSubID(categoryID);

            return Json(codeType);
        }

        #endregion

        #region update, create, delete
        [HttpPost]
        [Route("M500Accounting/AccountingDataUpdate/{strResult}")]
        public IActionResult AccountingDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Accounting>>(strResult);

            List<Accounting> list = new List<Accounting>();
            var i = 0;
            foreach (var item in json)
            {
                var accounting = new Accounting();
                accounting.AccountingID = json[i].AccountingID;
                accounting.SchoolID = json[i].SchoolID;
                accounting.Period = json[i].Period;
                accounting.VoucherNo = json[i].VoucherNo;
                accounting.AccountDate = json[i].AccountDate;
                accounting.AccountCode = json[i].AccountCode;
                accounting.AccountCodeName = json[i].AccountCodeName;
                accounting.CodeTypeName = json[i].CodeTypeName;
                accounting.ProcessTypeName = json[i].ProcessTypeName;
                accounting.TaxNoOrId = json[i].TaxNoOrId;
                accounting.DocumentNumber = json[i].DocumentNumber;
                accounting.DocumentDate = json[i].DocumentDate;
                accounting.Explanation = json[i].Explanation;
                if (json[i].Debt == null) json[i].Debt = 0;
                if (json[i].Credit == null) json[i].Credit = 0;
                accounting.Debt = json[i].Debt;
                accounting.Credit = json[i].Credit;
                accounting.SortOrder = json[i].SortOrder;
                accounting.IsTransaction = json[i].IsTransaction;
                list.Add(accounting);

                if (ModelState.IsValid)
                {
                    _accountingRepository.UpdateAccounting(accounting);
                }
                i = i + 1;
            }

            return Json(list);
        }

        [HttpPost]
        [Route("M500Accounting/AccountingDataCreate/{strResult}/{userID}/{voucherNo}/{voucherTypeID}")]
        public IActionResult AccountingDataCreate([Bind(Prefix = "models")] string strResult, int userID, int voucherNo, int voucherTypeID)
        {
            var user = _usersRepository.GetUser(userID);

            var json = new JavaScriptSerializer().Deserialize<List<Accounting>>(strResult);
            List<Accounting> list = new List<Accounting>();

            var acc = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, voucherNo);
            var sortno = acc.Max(a => a.SortOrder);
            if (sortno == null) sortno = 0;

            var i = 0;
            foreach (var item in json)
            {
                var accounting = new Accounting();

                accounting.AccountingID = 0;
                accounting.SchoolID = user.SchoolID;
                accounting.Period = user.UserPeriod;
                if (json[i].VoucherNo != voucherNo)
                    accounting.VoucherNo = json[i].VoucherNo;
                else
                    accounting.VoucherNo = voucherNo;

                accounting.VoucherTypeID = voucherTypeID;

                accounting.AccountDate = json[i].AccountDate;
                accounting.AccountCode = json[i].AccountCode;
                accounting.AccountCodeName = json[i].AccountCodeName;
                accounting.CodeTypeName = json[i].CodeTypeName;
                accounting.ProcessTypeName = json[i].ProcessTypeName;
                accounting.TaxNoOrId = json[i].TaxNoOrId;
                accounting.DocumentNumber = json[i].DocumentNumber;
                accounting.DocumentDate = json[i].DocumentDate;
                accounting.Explanation = json[i].Explanation;
                if (json[i].Debt == null) json[i].Debt = 0;
                if (json[i].Credit == null) json[i].Credit = 0;
                accounting.Debt = json[i].Debt;
                accounting.Credit = json[i].Credit;
                accounting.SortOrder = sortno + 1;
                accounting.IsTransaction = json[i].IsTransaction;
                list.Add(accounting);

                if (ModelState.IsValid)
                {
                    _accountingRepository.CreateAccounting(accounting);
                }
                i = i + 1;
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult AccountingDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Accounting>>(strResult);

            foreach (var item in json)
            {
                if (ModelState.IsValid)
                {
                    _accountingRepository.DeleteAccounting(item);
                }
            }
            return Json(true);
        }

        [HttpPost]
        [Route("M500Accounting/AccountingDataDeleteAll/{userID}/{voucherNo}")]
        public IActionResult AccountingDataDeleteAll(int userID, int voucherNo)
        {
            var user = _usersRepository.GetUser(userID);
            var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, voucherNo);
            foreach (var item in accounting)
            {
                if (ModelState.IsValid)
                {
                    _accountingRepository.DeleteAccounting(item);
                }
            }
            return Json(true);
        }

        [Route("M500Accounting/VoucherNoControl/{userID}/{voucherNo}")]
        public IActionResult VoucherNoControl(int userID, int voucherNo)
        {
            var user = _usersRepository.GetUser(userID);
            List<AccountingVoucherNo> list = new List<AccountingVoucherNo>();
            var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, voucherNo);

            int no = 0;
            for (int i = voucherNo; i > 1; i--)
            {
                if (no >= 10)
                    break;
                else
                {
                    bool isExist = _accountingRepository.ExistAccounting(i);
                    if (!isExist)
                    {
                        no += 1;
                        var accountingVoucherNo = new AccountingVoucherNo
                        {
                            ViewModelId = no,
                            VoucherNo = i,
                        };
                        list.Add(accountingVoucherNo);
                    }
                }
            }
            return Json(list);
        }

        [Route("M500Accounting/BalanceControl/{userID}/{voucherNo}")]
        public IActionResult BalanceControl(int userID, int voucherNo)
        {
            List<AccountingVoucherNo> list = new List<AccountingVoucherNo>();
            var user = _usersRepository.GetUser(userID);

            decimal? debt = 0;
            decimal? credit = 0;
            int no = 0;
            for (int i = voucherNo; i > 1; i--)
            {
                if (no >= 10)
                    break;
                else
                {
                    var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, i);
                    debt = accounting.Sum(a => a.Debt);
                    credit = accounting.Sum(a => a.Credit);
                    if (debt > 0 || credit > 0)
                    {
                        if (debt != credit)
                        {
                            no += 1;
                            var accountingVoucherNo = new AccountingVoucherNo
                            {
                                ViewModelId = no,
                                VoucherNo = i,
                                Debt = debt,
                                Credit = credit,
                            };
                            list.Add(accountingVoucherNo);
                        }
                    }
                }
            }
            return Json(list);
        }

        [Route("M500Accounting/VoucherNoTransaction/{userID}/{voucherNo}")]
        public IActionResult VoucherNoTransaction(int userID, int voucherNo)
        {
            var user = _usersRepository.GetUser(userID);

            List<AccountingVoucherNo> list = new List<AccountingVoucherNo>();
            var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, voucherNo);

            foreach (var item in accounting)
            {
                item.IsTransaction = true;
                _accountingRepository.UpdateAccounting(item);
            }

            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            var lastNumber = serialNumbers.AccountSerialNo += 1;
            serialNumbers.AccountSerialNo = lastNumber;
            _pSerialNumberRepository.UpdatePSerialNumber(serialNumbers);

            return Json(new { voucherNo = lastNumber });
        }

        [Route("M500Accounting/NewVoucherDataRead/{userID}")]
        public IActionResult NewVoucherDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            int lastNumber = serialNumbers.AccountSerialNo += 1;
            serialNumbers.AccountSerialNo = lastNumber;
            _pSerialNumberRepository.UpdatePSerialNumber(serialNumbers);

            return Json(new { voucherNo = lastNumber });
        }
        #endregion

        #region plan

        public IActionResult Plan(int userID)
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

            var studentViewModel = new StudentViewModel
            {
                IsPermission = isPermission,
                UserID = userID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(studentViewModel);
        }

        [Route("M500Accounting/PlanDataRead/{period}")]
        public IActionResult PlanDataRead(string period)
        {
            var accountCodes = _accountCodesRepository.GetAccountCodeAll(period);

            return Json(accountCodes);
        }

        [HttpPost]
        public IActionResult PlanDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<AccountCodes>>(strResult);

            AccountCodes code = new AccountCodes();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _accountCodesRepository.GetAccountCodeID(json[i].AccountCodeID, json[i].Period);

                getCode.AccountCodeID = json[i].AccountCodeID;
                getCode.AccountCode = json[i].AccountCode;
                getCode.AccountCodeName = json[i].AccountCodeName;
                getCode.IsCurrentCard = json[i].IsCurrentCard;
                getCode.IsActive = json[i].IsActive;

                if (ModelState.IsValid)
                {
                    _accountCodesRepository.UpdateAccountCode(getCode);
                }
                i = i + 1;
            }
            return Json(code);
        }

        public IActionResult PlanDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<AccountCodes>>(strResult);
            foreach (var item in json)
            {
                if (ModelState.IsValid)
                {
                    _accountCodesRepository.DeleteAccountCode(item);
                }
            }
            return Json(true);
        }

        public IActionResult PlanDataCreate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<AccountCodes>>(strResult);
            List<AccountCodes> list = new List<AccountCodes>();

            var i = 0;
            foreach (var item in json)
            {
                var code = new AccountCodes();
                code.AccountCodeID = 0;
                code.Period = json[i].Period;
                code.AccountCode = json[i].AccountCode;
                code.AccountCodeName = json[i].AccountCodeName;
                code.IsCurrentCard = json[i].IsCurrentCard;
                code.IsActive = json[i].IsActive;
                list.Add(code);
                if (ModelState.IsValid)
                {
                    _accountCodesRepository.CreateAccountCode(code);
                }
                i = i + 1;
            }
            return Json(list);
        }

        #endregion 

        #region AccountCodes
        [Route("M500Accounting/AccountControl/{accountCode}/{period}")]
        public IActionResult AccountControl(string accountCode, string period)
        {
            int count = 0;
            int selectcount = accountCode.Split(' ').Count();

            var code = _accountCodesRepository.GetAccountCodeAllTrue(period);
            var acclist = code.Where(p => p.AccountCode.StartsWith(accountCode));
            Boolean isTrue = true;
            foreach (var item in acclist)
            {
                count = item.AccountCode.Split(' ').Count();
                if (count > selectcount)
                {
                    isTrue = false;
                    break;
                }

            }
            return Json(new { istrue = isTrue });
        }

        #endregion

        #region ExportAccountingToExcel
        public IActionResult ExportAccountingToExcel(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Girişi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            var category = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var categoryName = _parameterRepository.GetParameterSubID(category);
            var voucherTypeID = categoryName.FirstOrDefault(p => p.CategoryName == "Mahsup").CategoryID;

            int lastNumber = 0;

            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            lastNumber = serialNumbers.AccountSerialNo;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            var accountingViewModel = new AccountingViewModel
            {
                IsPermission = isPermission,
                StudentID = studentID,
                UserID = user.UserID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                VoucherTypeID = (int)voucherTypeID,
                VoucherNo = lastNumber,
                SchoolName = _schoolInfoRepository.GetSchoolInfo(user.SchoolID).CompanyName,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now,
                StartCode = 100,
                EndCode = 100,
            };
            return View(accountingViewModel);
        }

        [Route("M500Accounting/AccountingExcelDataRead/{userID}/{voucherTypeID}/{transactiondate1}/{transactiondate2}/{startCode}/{endCode}/{startvoucherNo}/{endvoucherNo}")]
        public IActionResult AccountingExcelDataRead(int userID, int voucherTypeID, string transactiondate1, string transactiondate2, string startCode, string endCode, int startvoucherNo, int endvoucherNo)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            DateTime dateTime = new DateTime();
            dateTime = Convert.ToDateTime(DateTime.ParseExact(transactiondate1, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            DateTime date1 = DateTime.Today;
            date1 = DateTime.Parse(transactiondate1);
            DateTime date2 = DateTime.Today;
            date2 = DateTime.Parse(transactiondate2);

            List<AccountingViewModel> list = new List<AccountingViewModel>();

            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Girişinde Kullanılan Kod").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID);

            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, period);

            foreach (var item in accounting)
            {
                string code = item.AccountCode.Substring(0, 3);
                var isExist1 = false;
                var isExist2 = false;
                var isExist3 = false;
                if (item.AccountDate >= date1 && item.AccountDate <= date2)
                {
                    if (voucherTypeID == item.VoucherTypeID)
                    {
                        if (String.Compare(code, startCode) == 0 || String.Compare(code, startCode) > 0)
                        {
                            isExist1 = true;
                        }
                        if (String.Compare(code, endCode) == 0 || String.Compare(code, endCode) < 0)
                        {
                            isExist2 = true;
                        }
                        if (startvoucherNo == 0 && endvoucherNo == 0) isExist3 = true;
                        else
                        if (startvoucherNo > 0 && endvoucherNo > 0 && item.VoucherNo >= startvoucherNo && item.VoucherNo <= endvoucherNo)
                        {
                            isExist3 = true;
                        }

                        if (isExist1 == true && isExist2 == true && isExist3 == true)
                        {
                            var parameter = parameterList.FirstOrDefault(p => p.CategoryName == item.CodeTypeName);
                            if (parameter == null)
                            {
                                parameter = parameterList.FirstOrDefault(p => p.CategorySubID == categoryID);
                            }

                            var accountingViewModel = new AccountingViewModel
                            {
                                SchoolID = item.SchoolID,
                                AccountingID = item.AccountingID,
                                Period = item.Period,
                                VoucherTypeID = item.VoucherTypeID,
                                VoucherNo = item.VoucherNo,
                                AccountDate = item.AccountDate,
                                AccountCode = item.AccountCode,
                                AccountCodeName = item.AccountCodeName,
                                CodeTypeName = item.CodeTypeName,

                                ProcessTypeName = item.ProcessTypeName,
                                TaxNoOrId = item.TaxNoOrId,

                                DocumentNumber = item.DocumentNumber,
                                DocumentDate = item.DocumentDate,
                                Explanation = item.Explanation,
                                Debt = item.Debt,
                                Credit = item.Credit,
                                SortOrder = item.SortOrder,
                                IsTransaction = item.IsTransaction,

                                ViewModelID = item.AccountingID,
                                Parameter = parameter,
                            };

                            list.Add(accountingViewModel);
                        }
                    }
                }
            }
            return Json(list);
        }

        #endregion

        #region ExportInvoiceToExcel
        public IActionResult ExportInvoiceToExcel(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Girişi").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            var category = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var categoryName = _parameterRepository.GetParameterSubID(category);
            var voucherTypeID = categoryName.FirstOrDefault(p => p.CategoryName == "Mahsup").CategoryID;

            int lastNumber = 0;

            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();
            lastNumber = serialNumbers.AccountSerialNo;

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            var invoiceExcelViewModel = new InvoiceExcelViewModel
            {
                StudentID = studentID,
                UserID = user.UserID,
                SchoolID = user.SchoolID,
                Period = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now,
            };
            return View(invoiceExcelViewModel);
        }

        [Route("M500Accounting/InvoiceExcelDataRead/{userID}/{transactiondate1}/{transactiondate2}")]
        public IActionResult InvoiceExcelDataRead(int userID, string transactiondate1, string transactiondate2)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            DateTime dateTime = new DateTime();
            dateTime = Convert.ToDateTime(DateTime.ParseExact(transactiondate1, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            DateTime date1 = DateTime.Today;
            date1 = DateTime.Parse(transactiondate1);
            DateTime date2 = DateTime.Today;
            date2 = DateTime.Parse(transactiondate2);

            List<InvoiceExcelViewModel> list = new List<InvoiceExcelViewModel>();

            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Girişinde Kullanılan Kod").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID);

            var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetailAll().OrderByDescending(p => p.StudentInvoiceID).ToList();

            foreach (var item in invoiceDetail)
            {
                var invoice = _studentInvoiceRepository.GetStudentInvoiceID(period, user.SchoolID, item.StudentInvoiceID);
                if (invoice.InvoiceDate >= date1 && invoice.InvoiceDate <= date2)
                {
                    var student = _studentRepository.GetStudent(item.StudentID);
                    var studentAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(item.StudentID);
                    string serial = school.EIInvoiceSerialCode1;

                    string ID = "";
                    if (studentAddress != null && studentAddress.InvoiceTaxOffice != null) ID = studentAddress.InvoiceTaxNumber; else ID = student.IdNumber;
                    if (studentAddress != null && studentAddress.InvoiceProfile == true) { serial = school.EIInvoiceSerialCode2; }

                    string type = "SATIS";
                    if (school.EIIsActive == true) type = "E-Fatura";

                    string city = "";
                    string town = "";
                    if (studentAddress.InvoiceCityParameterID > 0)
                        city = _parameterRepository.GetParameter(studentAddress.InvoiceCityParameterID).CategoryName;
                    if (studentAddress.InvoiceTownParameterID > 0)
                        town = _parameterRepository.GetParameter(studentAddress.InvoiceTownParameterID).CategoryName;

                    var invoiceExcelViewModel = new InvoiceExcelViewModel
                    {
                        StudentInvoiceID = item.StudentInvoiceID,
                        SchoolID = item.SchoolID,
                        UserID = user.UserID,
                        Period = item.Period,
                        SelectedCulture = user.SelectedCulture.Trim(),
                        StartDate = DateTime.Now.AddMonths(-1),
                        EndDate = DateTime.Now,

                        StudentID = item.StudentID,
                        DocumentType = type,
                        InvoiceDate = invoice.InvoiceDate,
                        InvoiceTime = invoice.InvoiceCutTime,
                        InvoiceSerialNo = serial,
                        InvoiceSerialSequence = item.InvoiceSerialNo,

                        AccountCode = studentAddress.AccountCode,
                        AccountName = studentAddress.InvoiceTitle,
                        AccountRate = "",
                        CurrencyCode = "",
                        Rate = "",
                        StockCode = "",
                        StockName = "",
                        Unit = "",
                        GroupCode = "",
                        SpecialCode = "",
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = item.Amount,
                        DiscountPercent = 0,
                        Discount = item.Discount,
                        Amount1 = 0,
                        TaxPercent = item.TaxPercent,
                        Tax = item.Tax,
                        AmountPercent2 = 0,
                        Amount2 = 0,
                        Total = 0,
                        Explanation = item.Explanation,
                        Agreement = "",
                        Amount3 = 0,
                        Amount4 = 0,
                        Amount5 = 0,
                        Amount6 = 0,
                        InvoiceTaxNumber = "",
                        InvoiceTitle = studentAddress.InvoiceTitle,
                        InvoiceIdNumber = ID,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        EMail = studentAddress.EMail,
                        InvoiceCity = city,
                        InvoiceTown = town,
                        InvoiceNeighbourhood = "",
                        CSBMType = "",
                        CSBM = "",
                        ExteriorDoorNo = "",
                        InnerDoorNo = "",
                        EArchiveType = "",
                        WaybillDate = "",
                        WaybillSerial = "",
                        WaybillSequence = "",
                    };
                    list.Add(invoiceExcelViewModel);
                }
            }
            return Json(list);
        }

        #endregion
    }
}