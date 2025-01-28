using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Threading.Tasks;

using System.IO.Compression;
using System.Threading;

using System.Text;
using Uyumsoft;
using DocumentFormat.OpenXml.Drawing.Charts;
//using AspNetCore;

namespace ESCHOOL.Controllers
{
    public class ListPanelController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IClassroomRepository _classroomRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentRepository _studentRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        ITempM101HeaderRepository _tempM101HeaderRepository;
        ITempM101Repository _tempM101Repository;
        ISchoolFeeRepository _schoolFeeRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IStudentDebtRepository _studentDebtRepository;

        IStudentDebtDetailRepository _studentDebtDetailRepository;
        IParameterRepository _parameterRepository;
        IDiscountTableRepository _discountTableRepository;
        IStudentDiscountRepository _studentDiscountRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentInstallmentPaymentRepository _studentInstallmentPaymentRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        IStudentTempRepository _studentTempRepository;
        IBankRepository _bankRepository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        IAccountCodesDetailRepository _accountCodesDetailRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInvoiceDetailRepository _studentInvoiceDetailRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IWebHostEnvironment _hostEnvironment;
        public ListPanelController(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IUsersRepository usersRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            IClassroomRepository classroomRepository,
            IStudentRepository studentRepository,
            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,
            ITempM101HeaderRepository tempM101HeaderRepository,
            ITempM101Repository tempM101Repository,
            ISchoolFeeRepository schoolFeeRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDebtDetailRepository studentDebtDetailRepository,
            IParameterRepository parameterRepository,
            IDiscountTableRepository discountTableRepository,
            IStudentDiscountRepository studentDiscountRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            IStudentInstallmentPaymentRepository studentInstallmentPaymentRepository,
            IStudentPaymentRepository studentPaymentRepository,
            IStudentTempRepository studentTempRepository,
            IBankRepository bankRepository,
            IAccountingRepository accountingRepository,
            IAccountCodesRepository accountingCodeRepository,
            IAccountCodesDetailRepository accountCodesDetailRepository,
            IStudentInvoiceRepository studentInvoiceRepository,
            IStudentInvoiceDetailRepository studentInvoiceDetailRepository,
            IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _classroomRepository = classroomRepository;
            _studentRepository = studentRepository;
            _studentAddressRepository = studentAddressRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _tempM101Repository = tempM101Repository;
            _tempM101HeaderRepository = tempM101HeaderRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentDebtDetailRepository = studentDebtDetailRepository;
            _parameterRepository = parameterRepository;
            _discountTableRepository = discountTableRepository;
            _studentDiscountRepository = studentDiscountRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _studentInstallmentPaymentRepository = studentInstallmentPaymentRepository;
            _studentPaymentRepository = studentPaymentRepository;
            _studentTempRepository = studentTempRepository;
            _bankRepository = bankRepository;
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountingCodeRepository;
            _accountCodesDetailRepository = accountCodesDetailRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInvoiceDetailRepository = studentInvoiceDetailRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _hostEnvironment = hostEnvironment;
        }
        #region List1000
        public IActionResult List1()
        {
            return View(true);
        }
        public IActionResult List1000KvkkConfirm(int studentID, int userID)
        {
            var user = _usersRepository.GetUser(userID);

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;
            string cs = $"Data Source={dataSource};Initial Catalog={user.SelectedSchoolCode};User Id=sa;Password={password}";

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                StudentID = studentID,
                SelectedSchoolCode = user.SelectedSchoolCode,
                SelectedCulture = user.SelectedCulture.Trim(),
                ConnectionString = cs,
                wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/"), //Picture Path
            };

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            ViewBag.UserID = user.UserID;
            ViewBag.StudentID = studentID;

            return View(listPanelModel);
        }

        public IActionResult List1000(int userID, int studentID, int msg, int exitID, string receiptNo, int paymentSW, int formTypeSW)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var listPanelModel = new ListPanelModel();

            var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(studentID);
            var selectedCulture = user.SelectedCulture.Trim();
            var studentinstallments = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentID, user.UserPeriod);

            listPanelModel.List01Options0 = true;
            listPanelModel.List01Options1 = false;
            listPanelModel.List01Options2 = false;
            listPanelModel.List01Options3 = false;
            listPanelModel.List01Options4 = false;
            listPanelModel.List01Options5 = false;
            listPanelModel.List01Options8 = false;
            listPanelModel.List01Options6 = true;
            listPanelModel.List01Options7 = true;

            foreach (var item in studentinstallments)
            {
                var type = _parameterRepository.GetParameter(item.CategoryID).CategoryName;
                if (type == "Mail order") listPanelModel.List01Options1 = true;
                if (type == "Banka") listPanelModel.List01Options2 = true;
                if (type == "Elden") listPanelModel.List01Options3 = true;
                if (type == "OTS_1") listPanelModel.List01Options4 = true;
                if (type == "OTS_2") listPanelModel.List01Options4 = true;
                if (type == "Mail order") listPanelModel.List01Options5 = true;
                if (type == "Kredi kartı") listPanelModel.List01Options8 = true;

            }
            listPanelModel.Prg = 1;
            if (formTypeSW == 2) listPanelModel.Prg = 2;

            listPanelModel.StudentID = studentID;
            listPanelModel.SchoolID = user.SchoolID;
            listPanelModel.UserID = user.UserID;
            listPanelModel.SelectedCulture = selectedCulture;
            listPanelModel.SelectedSchoolCode = user.SelectedSchoolCode;
            listPanelModel.Periods = user.UserPeriod;
            listPanelModel.StudentID = studentID;
            listPanelModel.ExitID = exitID;
            listPanelModel.ReceiptNo = receiptNo;

            listPanelModel.PaymentSW = paymentSW;
            //Collection parameter
            if (msg == 9 || msg == 8 || formTypeSW == 2)
            {
                listPanelModel.List01Options0 = false;
                listPanelModel.List01Options1 = false;
                listPanelModel.List01Options2 = false;
                listPanelModel.List01Options3 = false;
                listPanelModel.List01Options4 = false;
                listPanelModel.List01Options5 = false;
                listPanelModel.List01Options8 = false;

                listPanelModel.List01Options6 = true;
                listPanelModel.List01Options7 = false;
            }

            var student = _studentRepository.GetStudent(studentID);
            listPanelModel.StudentName = student.FirstName + " " + student.LastName;

            listPanelModel.BondType = true;
            listPanelModel.StartListDate = DateTime.Now;
            listPanelModel.Interlocutor = 1;
            listPanelModel.WriteDate = true;
            listPanelModel.WriteStudentName = true;
            listPanelModel.FormTitle = schoolInfo.FormTitle;
            listPanelModel.FormOpt = schoolInfo.FormOpt;

            listPanelModel.GuarantorID = parentAddress.GuarantorId;
            listPanelModel.GuarantorName = parentAddress.GuarantorName;
            listPanelModel.GuarantorAddress = parentAddress.GuarantorAddress;
            listPanelModel.GuarantorZip = parentAddress.GuarantorZipCode;
            listPanelModel.GuarantorCity = parentAddress.GuarantorCityParameterID;
            listPanelModel.GuarantorTown = parentAddress.GuarantorTownParameterID;
            listPanelModel.GuarantorPhone = parentAddress.GuarantorPhone;
            listPanelModel.GuarantorOther = parentAddress.GuarantorOther;
            listPanelModel.wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/");

            ViewBag.Msg = 0;
            ViewBag.IsSuccess = false;
            if (msg == 9 || msg == 8) ViewBag.Msg = msg;
            if (msg > 0 && msg != 9 && msg != 8) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
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
            listPanelModel.ConnectionString = cs;

            return View(listPanelModel);

        }
        public async Task<IActionResult> ListPanelInfo1000(ListPanelModel listPanelModel)
        {
            await Task.Delay(1000);

            var isFormPrint = 1;
            var isMailOrder = 1;
            var isReceipt1 = 1;
            var isReceipt2 = 1;

            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string selectedLanguage = user.SelectedCulture.Trim();
            if (schoolInfo.FormTitle != listPanelModel.FormTitle || schoolInfo.FormOpt != listPanelModel.FormOpt)
            {
                schoolInfo.FormTitle = listPanelModel.FormTitle;
                schoolInfo.FormOpt = listPanelModel.FormOpt;
                _schoolInfoRepository.UpdateSchoolInfo(schoolInfo);
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            string url = null;
            var student = _studentRepository.GetStudent(listPanelModel.StudentID);

            var installments = new TempM101();

            //Receipt Control
            if (listPanelModel.List01Options6 == true || listPanelModel.List01Options7 == true)
            {
                ListPanelInfo1011(listPanelModel, installments);
            }

            var isExist = false;
            var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(student.StudentID);
            var familyAddress = _studentFamilyAddressRepository.GetStudentFamilyAddress(student.StudentID);
            var studentinstallments = _studentInstallmentRepository.GetStudentInstallment(student.SchoolID, student.StudentID, user.UserPeriod);

            //Guarantor
            parentAddress.GuarantorId = listPanelModel.GuarantorID;
            parentAddress.GuarantorName = listPanelModel.GuarantorName;
            parentAddress.GuarantorPhone = listPanelModel.GuarantorPhone;
            parentAddress.GuarantorAddress = listPanelModel.GuarantorAddress;
            parentAddress.GuarantorCityParameterID = listPanelModel.GuarantorCity;
            parentAddress.GuarantorTownParameterID = listPanelModel.GuarantorTown;
            parentAddress.GuarantorZipCode = listPanelModel.GuarantorZip;
            parentAddress.GuarantorOther = listPanelModel.GuarantorOther;

            if (parentAddress.StudentParentAddressID == 0)
            {
                parentAddress.StudentParentAddressID = 0;
                parentAddress.StudentID = student.StudentID;
                //parentAddress.ParentGenderTypeCategoryID = listPanelMode
                _studentParentAddressRepository.CreateStudentParentAddress(parentAddress);
            }
            else
            {
                _studentParentAddressRepository.UpdateStudentParentAddress(parentAddress);
            }

            var isSingle = false;
            decimal singleAmount = 0;

            decimal cashPayment = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, listPanelModel.StudentID).CashPayment;
            decimal total = cashPayment;

            string classroomName = "";
            int classroomID = 0;
            var isExist2 = false;
            if (schoolInfo.NewPeriod == user.UserPeriod)
            {
                if (student.ClassroomID > 0)
                {
                    classroomID = student.ClassroomID;
                    classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                }
            }
            else
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(student.SchoolID, student.StudentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(student.SchoolID, student.StudentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                        classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                }
            }

            foreach (var si in studentinstallments)
            {
                installments.ID = 0;
                installments.UserID = listPanelModel.UserID;
                installments.SchoolID = student.SchoolID;
                installments.StudentID = student.StudentID;
                installments.ClassroomID = student.ClassroomID;
                installments.IdNumber = student.IdNumber;
                installments.StudentName = student.FirstName + " " + student.LastName;
                installments.ClassroomName = classroomName;

                if (listPanelModel.Interlocutor == 1 && parentAddress != null)
                {
                    installments.ParentName = student.ParentName;
                    installments.ParentIdNumber = parentAddress.IdNumber;
                    installments.ParentAddress = parentAddress.HomeAddress;
                    if (parentAddress.HomeCityParameterID != 0)
                        installments.ParentCity = _parameterRepository.GetParameter(parentAddress.HomeCityParameterID).CategoryName;
                    if (parentAddress.HomeTownParameterID != 0)
                        installments.ParentTown = _parameterRepository.GetParameter(parentAddress.HomeTownParameterID).CategoryName;

                    installments.ParentZipCode = parentAddress.HomeZipCode;
                    installments.ParentMobilePhone = parentAddress.MobilePhone;
                    installments.HomePhone = parentAddress.HomePhone;
                    installments.WorkPhone = parentAddress.WorkPhone;
                }
                if (listPanelModel.Interlocutor == 2 && parentAddress != null)
                {
                    if (familyAddress != null)
                    {
                        installments.ParentName = familyAddress.FatherName;
                        installments.ParentIdNumber = familyAddress.FatherIdNumber;
                        installments.ParentAddress = familyAddress.FatherHomeAddress;
                        if (familyAddress.FatherHomeCityParameterID != 0)
                            installments.ParentCity = _parameterRepository.GetParameter(familyAddress.FatherHomeCityParameterID).CategoryName;
                        if (familyAddress.FatherHomeTownParameterID != 0)
                            installments.ParentTown = _parameterRepository.GetParameter(familyAddress.FatherHomeTownParameterID).CategoryName;

                        installments.ParentZipCode = familyAddress.FatherHomeZipCode;
                        installments.ParentMobilePhone = familyAddress.FatherMobilePhone;
                        installments.HomePhone = familyAddress.FatherHomePhone;
                        installments.WorkPhone = familyAddress.FatherWorkPhone;
                    }
                }
                if (listPanelModel.Interlocutor == 3 && parentAddress != null)
                {
                    if (familyAddress != null)
                    {
                        installments.ParentName = familyAddress.MotherName;
                        installments.ParentIdNumber = familyAddress.MotherIdNumber;
                        installments.ParentAddress = familyAddress.MotherHomeAddress;
                        if (familyAddress.MotherHomeCityParameterID != 0)
                            installments.ParentCity = _parameterRepository.GetParameter(familyAddress.MotherHomeCityParameterID).CategoryName;
                        if (familyAddress.MotherHomeTownParameterID != 0)
                            installments.ParentTown = _parameterRepository.GetParameter(familyAddress.MotherHomeTownParameterID).CategoryName;

                        installments.ParentZipCode = familyAddress.MotherHomeZipCode;
                        installments.ParentMobilePhone = familyAddress.MotherMobilePhone;
                        installments.HomePhone = familyAddress.MotherHomePhone;
                        installments.WorkPhone = familyAddress.MotherWorkPhone;
                    }
                }

                if (parentAddress != null)
                {
                    installments.GuarantorId = parentAddress.GuarantorId;
                    installments.GuarantorName = parentAddress.GuarantorName;
                    installments.GuarantorPhone = parentAddress.GuarantorPhone;
                    installments.GuarantorAddress = parentAddress.GuarantorAddress;
                    if (parentAddress.GuarantorCityParameterID != 0)
                        installments.GuarantorCity = _parameterRepository.GetParameter(parentAddress.GuarantorCityParameterID).CategoryName;
                    if (parentAddress.GuarantorCityParameterID != 0)
                        installments.GuarantorTown = _parameterRepository.GetParameter(parentAddress.GuarantorTownParameterID).CategoryName;

                    installments.GuarantorZipCode = parentAddress.GuarantorZipCode;
                    installments.GuarantorOther = parentAddress.GuarantorOther;

                }
                if (listPanelModel.BondType == true)
                    installments.BondTypeTitle = "İş bu emre muharrer senedim mukabilinde";
                else installments.BondTypeTitle = "İş bu nama yazılı senedim mukabilinde";

                installments.BondDate = listPanelModel.StartListDate;
                installments.WriteDate = listPanelModel.WriteDate;
                installments.DateOfRegistration = si.InstallmentDate;
                var date = installments.DateOfRegistration.Value;

                var strDate = date.ToString("dd' / 'MMMM' / 'yyyy", new CultureInfo(user.SelectedCulture.Trim()));
                installments.StrDate = strDate;

                installments.WriteStudentName = listPanelModel.WriteStudentName;
                installments.Fee01 = si.InstallmentAmount;
                installments.StudentSerialNumber = (int)si.InstallmentNo;

                var type = _parameterRepository.GetParameter(si.CategoryID).CategoryName;
                if (type == "Banka" || type == "Elden" || type == "Mail order" || type == "Kredi kartı" || type == "OTS_1" || type == "OTS_2" || type == "Teşvik") isExist = true;
                else isExist = false;

                var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
                if (si.StatusCategoryID == statusID) isExist = false;
                if (listPanelModel.List01Options2 == false && type == "Banka") isExist = false;
                if (listPanelModel.List01Options3 == false && type == "Elden") isExist = false;
                if (listPanelModel.List01Options4 == false && type == "OTS_1" || type == "OTS_2") isExist = false;
                if (listPanelModel.List01Options5 == false && type == "Mail order") isExist = false;
                if (listPanelModel.List01Options8 == false && type == "Kredi kartı") isExist = false;

                string typeFirstChar = _parameterRepository.GetParameter(si.CategoryID).CategoryName;

                installments.TypeAndNo = "- (" + typeFirstChar.Substring(0, 1) + ")";

                installments.Name = schoolInfo.CompanyNameForBond;
                installments.BondCity = "";
                if (schoolInfo.CityNameForBondID > 0)
                    installments.BondCity = _parameterRepository.GetParameter(schoolInfo.CityNameForBondID).CategoryName;

                MoneyToText text = new MoneyToText();
                installments.InWriting = text.ConvertToText(si.InstallmentAmount);
                total += si.InstallmentAmount;

                if (listPanelModel.BondSingle == false)
                {
                    if (isExist == true)
                    {
                        installments.Status = "";
                        _tempM101Repository.CreateTempM101(installments);
                    }
                }
                else
                {
                    if (isExist == true)
                    {
                        isSingle = true;
                        singleAmount += si.InstallmentAmount;
                        installments.InWriting = text.ConvertToText(singleAmount);
                    }
                }
            }

            var header = new TempM101Header();
            header.ID = 0;
            header.UserID = user.UserID;
            header.SchoolID = user.SchoolID;

            MoneyToText txt = new MoneyToText();
            header.InWriting = txt.ConvertToText(total);
            header.Total = total;
            header.CompanyName = schoolInfo.CompanyName;
            _tempM101HeaderRepository.CreateTempM101Header(header);

            if (isSingle == true)
            {
                installments.ClassroomID = 0;
                installments.Fee01 = singleAmount;
                _tempM101Repository.CreateTempM101(installments);
            }

            if (listPanelModel.List01Options0 == true) isFormPrint = 1; else isFormPrint = 0;
            if (listPanelModel.List01Options1 == true) isMailOrder = 1; else isMailOrder = 0;
            if (listPanelModel.List01Options6 == true && (cashPayment > 0 || listPanelModel.PaymentSW == 1)) isReceipt1 = 1; else isReceipt1 = 0;
            if (listPanelModel.List01Options7 == true && total > 0) isReceipt2 = 1; else isReceipt2 = 0;

            // Çift baskıyı önlemek için.
            if (studentinstallments.Count() == 0)
            {
                //listPanelModel.List01Options0 = false; isFormPrint = 0;
                isFormPrint = 0;
            }

            string formTitle = Resources.Resource.RegistrationRenewalForm;
            if (schoolInfo.FormTitle != null) formTitle = schoolInfo.FormTitle;

            string formNameHeader = Resources.Resource.FormHeaderParent;
            string formNameTitle = student.ParentName;

            if (!schoolInfo.FormOpt)
            {
                formNameHeader = Resources.Resource.FormHeaderStudent;
                formNameTitle = " ";
            }
            string cleanedText = ClearTurkishCharacter(formTitle);
            formTitle = cleanedText;

            cleanedText = ClearTurkishCharacter(formNameTitle);
            formNameTitle = cleanedText;

            if (listPanelModel.Prg == 1)
                url = "~/reporting/index/M1000/" + listPanelModel.StudentID + "/" + listPanelModel.ExitID + "?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&isFormPrint=" + isFormPrint + "&isMailOrder=" + isMailOrder + "&isReceipt1=" + isReceipt1 + "&isReceipt2=" + isReceipt2 + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&studentID=" + student.StudentID + "&wwwRootPath=" + '"' + listPanelModel.wwwRootPath + '"' + "&receiptNo=" + listPanelModel.ReceiptNo + "&formNameTitle=" + '"' + formNameTitle + '"' + "&formNameHeader=" + '"' + formNameHeader + '"' + "&formTitle=" + '"' + formTitle + '"';
            else
                url = "~/reporting/index/M1001/" + listPanelModel.StudentID + "/" + listPanelModel.ExitID + "?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&isFormPrint=" + isFormPrint + "&isMailOrder=" + isMailOrder + "&isReceipt1=" + isReceipt1 + "&isReceipt2=" + isReceipt2 + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&studentID=" + student.StudentID + "&wwwRootPath=" + '"' + listPanelModel.wwwRootPath + '"' + "&receiptNo=" + listPanelModel.ReceiptNo + "&formNameTitle=" + '"' + formNameTitle + '"' + "&formNameHeader=" + '"' + formNameHeader + '"' + "&formTitle=" + '"' + formTitle + '"';

            return Redirect(url);
        }
        public static string ClearTurkishCharacter(string _dirtyText)
        {
            var text = _dirtyText;
            if (text != null)
            {
                var unaccentedText = String.Join("", text.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark));
                text = unaccentedText.Replace("ı", "i").Replace("İ", "I").Replace("Ö", "O");
            }
            return text;
        }
        public IActionResult ListPanelInfo1011(ListPanelModel listPanelModel, TempM101 account1)
        {
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var student = _studentRepository.GetStudent(listPanelModel.StudentID);
            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, listPanelModel.StudentID);
            decimal cashPayment = studentTemp.CashPayment;
            var receiptNo = 0;
            var account100 = "";
            if (listPanelModel.Prg == 2)
            {
                receiptNo = studentTemp.CollectionReceipt;
            }
            else
            {
                receiptNo = studentTemp.AccountReceipt;
            }

            account100 = schoolInfo.AccountNoID01;
            var receiptNo1 = receiptNo;
            var receiptNo2 = receiptNo;

            var account340 = schoolInfo.AccountNoID02;
            var account600 = schoolInfo.AccountNoID03;

            var accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, receiptNo1);

            //100 - 102 TempM101
            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(student.SchoolID, student.StudentID, user.UserPeriod);
            decimal debt = 0;
            decimal payment = 0;
            decimal total = 0;

            foreach (var item in studentInstallment)
            {
                debt += item.InstallmentAmount;
                payment += item.PreviousPayment;
            }

            int inx = 0;
            if (listPanelModel.Prg == 1)
            {
                foreach (var item in accounting)
                {
                    account1.ID = 0;
                    account1.UserID = listPanelModel.UserID;
                    account1.SchoolID = schoolInfo.SchoolID;
                    account1.StudentID = student.StudentID;

                    string code = item.AccountCode.Substring(0, 3);
                    if (code == account100)
                    {
                        account1.BondTypeTitle = item.AccountCode;
                        account1.InWriting = item.Explanation;
                        account1.CashPayment = item.Debt;
                        account1.Status = "Receipt1";
                        _tempM101Repository.CreateTempM101(account1);
                        receiptNo1 = Convert.ToInt32(item.DocumentNumber);
                        total += Convert.ToDecimal(account1.CashPayment);
                        inx++;
                    }
                    else
                        receiptNo2 = Convert.ToInt32(item.DocumentNumber);
                }
            }
            else
            {

                foreach (var item in accounting)
                {
                    string documentNumber = item.DocumentNumber;
                    account1.ID = 0;
                    account1.UserID = listPanelModel.UserID;
                    account1.SchoolID = schoolInfo.SchoolID;
                    account1.StudentID = student.StudentID;

                    string code = item.AccountCode.Substring(0, 3);
                    int count = item.AccountCode.Count();
                    if (code != account100 && code != account340 && code != account600 && item.Credit > 0 && count > 3)
                    {
                        account1.BondTypeTitle = item.AccountCode;
                        account1.InWriting = item.Explanation;
                        account1.CashPayment = item.Credit;
                        account1.Status = "Receipt1";
                        total += Convert.ToDecimal(account1.CashPayment);
                        _tempM101Repository.CreateTempM101(account1);
                        inx++;
                    }
                }
            }

            for (int i = inx; i < 20; i++)
            {
                account1.ID = 0;
                account1.UserID = listPanelModel.UserID;
                account1.SchoolID = schoolInfo.SchoolID;
                account1.StudentID = student.StudentID;

                account1.BondTypeTitle = "";
                account1.InWriting = "";
                account1.CashPayment = 0;
                account1.Status = "Receipt1";
                _tempM101Repository.CreateTempM101(account1);
            }
            //100 TempM101Header
            var header = new TempM101Header();
            header.ID = 0;
            header.UserID = user.UserID;
            header.SchoolID = user.SchoolID;

            MoneyToText txt = new MoneyToText();
            header.InWriting = txt.ConvertToText(total);
            header.Total = debt - payment;
            if (schoolInfo.NameOnReceipt == true)
                header.HeaderFee01 = schoolInfo.AuthorizedPersonName3;
            else header.HeaderFee01 = user.UserName;
            header.EndNumber = 0;
            if (schoolInfo.DefaultShowDept == true)
                header.EndNumber = 1;

            header.HeaderFee12 = "Receipt1";
            header.StartNumber = receiptNo1;

            _tempM101HeaderRepository.CreateTempM101Header(header);

            //121 TempM101
            if (listPanelModel.Prg == 1)
            {
                inx = 0;
                total = 0;
                accounting = _accountingRepository.GetAccountingVoucherNo(user.SchoolID, user.UserPeriod, receiptNo);
                foreach (var item in accounting)
                {
                    account1.ID = 0;
                    account1.UserID = listPanelModel.UserID;
                    account1.SchoolID = schoolInfo.SchoolID;
                    account1.StudentID = student.StudentID;

                    string code = item.AccountCode.Substring(0, 3);
                    if (code != account100 && code != account340 && code != account600)
                    {
                        account1.BondTypeTitle = item.AccountCode;
                        account1.InWriting = item.Explanation;
                        account1.CashPayment = item.Debt;
                        account1.Status = "Receipt2";
                        total += Convert.ToDecimal(account1.CashPayment);
                        _tempM101Repository.CreateTempM101(account1);
                        inx++;
                    }
                }
                for (int i = inx; i < 20; i++)
                {
                    account1.ID = 0;
                    account1.UserID = listPanelModel.UserID;
                    account1.SchoolID = schoolInfo.SchoolID;
                    account1.StudentID = student.StudentID;

                    account1.BondTypeTitle = "";
                    account1.InWriting = "";
                    account1.CashPayment = 0;
                    account1.Status = "Receipt2";
                    _tempM101Repository.CreateTempM101(account1);
                }
                //121 M101Header
                header.ID = 0;
                header.UserID = user.UserID;
                header.SchoolID = user.SchoolID;
                decimal ltotal = total;
                header.InWriting = txt.ConvertToText(ltotal);
                header.Total = total;
                if (schoolInfo.NameOnReceipt == true)
                    header.HeaderFee01 = schoolInfo.AuthorizedPersonName3;
                else header.HeaderFee01 = user.UserName;
                header.HeaderFee12 = "Receipt2";
                header.StartNumber = receiptNo2;

                if (cashPayment == 0) listPanelModel.List01Options6 = false;
                if (total == 0) listPanelModel.List01Options7 = false;
                _tempM101HeaderRepository.CreateTempM101Header(header);
            }
            return Json(listPanelModel);
        }

        public async Task<IActionResult> Agreement1000(int userID, int studentID)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(studentID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var header = new TempM101Header();
            header.ID = 0;
            header.UserID = user.UserID;
            header.SchoolID = user.SchoolID;
            header.StartNumber = 0;
            header.EndNumber = 0;

            int inx = 0;
            //string text = "";
            var text = string.Empty;
            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(student.SchoolID, student.StudentID, user.UserPeriod);

            foreach (var item in studentInstallment)
            {
                inx++;
                text = inx.ToString() + '-' + String.Format("{0:0.00}", item.InstallmentAmount) + " ";
                header.InWriting += @text;
            }

            var temp = new TempM101();
            double taxPercent = 0;
            decimal taxExcluded = 0;
            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");
            foreach (var item in fee)
            {
                var studentDebt = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, studentID, item.SchoolFeeID);

                temp.ID = 0;
                temp.SchoolID = user.SchoolID;
                temp.SchoolNumber = schoolInfo.SchoolID;
                temp.StudentID = studentID;
                temp.UserID = user.UserID;
                temp.ClassroomID = 0;
                temp.ClassroomName = user.UserPeriod;
                if (studentDebt.UnitPrice > 0)
                {
                    taxPercent = (1 + (Convert.ToDouble(item.Tax) / 100));

                    temp.Name = item.Name;
                    taxExcluded = Math.Round(studentDebt.UnitPrice / Convert.ToDecimal(taxPercent), schoolInfo.CurrencyDecimalPlaces);
                    temp.Fee01 = taxExcluded;
                    temp.Fee02 = item.Tax;
                    temp.Fee03 = studentDebt.UnitPrice;

                    taxExcluded = Math.Round(studentDebt.Amount / Convert.ToDecimal(taxPercent), schoolInfo.CurrencyDecimalPlaces);
                    temp.Fee04 = taxExcluded;
                    temp.Fee05 = item.Tax;
                    temp.Fee06 = studentDebt.Amount;

                    _tempM101Repository.CreateTempM101(temp);
                }
            }

            var discount = _discountTableRepository.GetDiscountTablePeriodOnlyTrue(user.UserPeriod, user.SchoolID);
            string txt = "";
            decimal amount = 0;
            foreach (var item in discount)
            {
                temp.Fee01 = 0; temp.Fee02 = 0; temp.Fee03 = 0; temp.Fee04 = 0; temp.Fee05 = 0; temp.Fee06 = 0; temp.Fee07 = 0; temp.Fee08 = 0; temp.Fee09 = 0; temp.Fee10 = 0;

                var studentDiscounts = _studentDiscountRepository.GetDiscount4(studentID, user.UserPeriod, user.SchoolID, item.DiscountTableID);
                decimal discountAmount = studentDiscounts.Sum(p => p.DiscountTotal);

                temp.ID = 0;
                temp.SchoolID = user.SchoolID;
                temp.SchoolNumber = schoolInfo.SchoolID;
                temp.StudentID = studentID;
                temp.UserID = user.UserID;

                if (discountAmount > 0)
                {
                    if (item.DiscountName.Contains("Kardeş") || item.DiscountName.Contains("Personel Çocuğu") ||
                        item.DiscountName.Contains("Kurumsal İndirim") || item.DiscountName.Contains("Şehit/Gazi") ||
                        item.DiscountName.Contains("Öğretmen Çocuğu ") || item.DiscountName.Contains("Başarı") ||
                        item.DiscountName.Contains("Kayıtlı Öğrenci") || item.DiscountName.Contains("Korucu Çocuğu"))
                    {
                        inx = 0;
                        for (int i = 1; i < 9; i++)
                        {
                            if (i == 1 && item.DiscountName.Contains("Kardeş")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 2 && item.DiscountName.Contains("Personel Çocuğu")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 3 && item.DiscountName.Contains("Kurumsal İndirim")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 4 && item.DiscountName.Contains("Şehit/Gazi")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 5 && item.DiscountName.Contains("Öğretmen Çocuğu ")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 6 && item.DiscountName.Contains("Başarı")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 7 && item.DiscountName.Contains("Kayıtlı Öğrenci")) { amount = discountAmount; inx = i; break; } else amount = 0;
                            if (i == 8 && item.DiscountName.Contains("Korucu Çocuğu")) { amount = discountAmount; inx = i; break; } else amount = 0;
                        }

                        if (amount > 0)
                        {
                            if (inx == 1) temp.Fee01 = amount;
                            if (inx == 2) temp.Fee02 = amount;
                            if (inx == 3) temp.Fee03 = amount;
                            if (inx == 4) temp.Fee04 = amount;
                            if (inx == 5) temp.Fee05 = amount;
                            if (inx == 6) temp.Fee06 = amount;
                            if (inx == 7) temp.Fee07 = amount;
                            if (inx == 8) temp.Fee08 = amount;
                        }
                        temp.ClassroomID = inx;
                        _tempM101Repository.CreateTempM101(temp);
                    }
                    else
                    {
                        txt += item.DiscountName + ", ";
                    }
                }
            }
            int lenght = 300;
            lenght = txt.Length;
            if (lenght > 300) lenght = 300;

            if (txt != "")
            {
                temp.TypeAndNo = txt.Substring(0, lenght);
                temp.TypeAndNo = txt;
                temp.ClassroomID = 999999;
                _tempM101Repository.CreateTempM101(temp);
            }
            var studentFees = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, student.StudentID);

            var dis = _studentDiscountRepository.GetDiscount2(user.SchoolID, user.UserPeriod, student.StudentID);
            decimal Total = dis.Sum(p => p.DiscountTotal);
            header.Total = Total;

            _tempM101HeaderRepository.CreateTempM101Header(header);

            return Json(true);
        }
        public IActionResult PeriodDataRead()
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, 2);
            return Json(mylist);
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
        [Route("ListPanel/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }

        #endregion

        #region List101
        public IActionResult List101(int userID, int studentID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                StudentID = studentID,
                ExitID = exitID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options0 = false,
                List01Options1 = true,
                List01Options2 = true,
                List01Options3 = false
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = "#ffffff}";
            if (user.SelectedTheme == "black" || user.SelectedTheme == "metroblack" || user.SelectedTheme == "highcontrast" || user.SelectedTheme == "moonlight") TempData["color"] = "#222";

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo1(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> student = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.DateOfRegistration).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(b => b.DateOfRegistration).ToList();
            }

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            int inx = 1;
            foreach (var item in fee)
            {
                if (user.SelectedCulture.Trim() == "tr-TR")
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Name; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Name; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Name; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Name; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Name; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Name; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Name; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Name; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Name; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Name; }
                }
                else
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Language1; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Language1; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Language1; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Language1; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Language1; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Language1; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Language1; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Language1; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Language1; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Language1; }
                }
                inx++;
            }
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, total = 0;
            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var isExistStudent = false;
            foreach (var item in student)
            {
                var isExist = false;
                bool isExist2 = false;
                int classroomID = 0;
                string classroomName = "";
                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                var classroomSo = 0;
                if (classroomID > 0)
                    classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo)
                {
                    isExist = true;
                }
                else { isExist = false; };

                if (listPanelModel.List01Options0 == true && isExist == true)
                {
                    DateTime date = Convert.ToDateTime(item.DateOfRegistration);
                    if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                        (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                    {
                        isExist = true;
                    }
                    else { isExist = false; };
                };

                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (listPanelModel.List01Options1 == true && listPanelModel.List01Options3 == false && isExist == true)
                    if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Ön Kayıt")
                    { isExist = false; }
                    else { isExist = true; };


                if (listPanelModel.List01Options3 == true && isExist == true)
                    if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Ön Kayıt" || statuName == "Takipte(İptal")
                    { isExist = true; }
                    else { isExist = false; };

                studentFee.Fee01 = 0; studentFee.Fee02 = 0; studentFee.Fee03 = 0; studentFee.Fee04 = 0; studentFee.Fee05 = 0;
                studentFee.Fee06 = 0; studentFee.Fee07 = 0; studentFee.Fee08 = 0; studentFee.Fee09 = 0; studentFee.Fee10 = 0;
                if (isExist == true)
                {
                    inx = 1;
                    total = 0;
                    foreach (var f in fee)
                    {
                        var studentFees = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, item.StudentID, f.SchoolFeeID);
                        if (studentFees != null)
                        {
                            amount = studentFees.Amount;
                            if (amount == 0) amount = studentFees.UnitPrice;
                            if (inx == 01) { studentFee.Fee01 = amount; }
                            if (inx == 02) { studentFee.Fee02 = amount; }
                            if (inx == 03) { studentFee.Fee03 = amount; }
                            if (inx == 04) { studentFee.Fee04 = amount; }
                            if (inx == 05) { studentFee.Fee05 = amount; }
                            if (inx == 06) { studentFee.Fee06 = amount; }
                            if (inx == 07) { studentFee.Fee07 = amount; }
                            if (inx == 08) { studentFee.Fee08 = amount; }
                            if (inx == 09) { studentFee.Fee09 = amount; }
                            if (inx == 10) { studentFee.Fee10 = amount; }
                            total += amount;
                            inx++;
                        }

                        if (listPanelModel.List01Options2 == true && total == 0) { isExist = false; };


                        studentFee.ID = 0;
                        studentFee.UserID = listPanelModel.UserID;
                        studentFee.SchoolID = item.SchoolID;
                        studentFee.StudentID = item.StudentID;
                        studentFee.StudentNumber = item.StudentNumber;
                        studentFee.StudentSerialNumber = item.StudentSerialNumber;
                        studentFee.ClassroomID = classroomID;
                        studentFee.StudentName = item.FirstName + " " + item.LastName;
                        studentFee.DateOfRegistration = item.DateOfRegistration;
                    }
                    if (listPanelModel.List01Options2 == true && total == 0) { isExist = false; };
                    if (isExist == true) isExistStudent = true;
                    if (isExist)
                        _tempM101Repository.CreateTempM101(studentFee);
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();

            var exitID = 0;
            if (listPanelModel.ExitID == 1) exitID = 1;
            string url = "~/reporting/index/M101StudentFeeStatusLists/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List101?userID=" + user.UserID + "studentID = " + listPanelModel.StudentID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }

        [Route("ListPanel/PeriodCombo/{userID}")]
        public IActionResult PeriodCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }


        #endregion

        #region List102
        public IActionResult List102(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                Periods = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = school.SchoolYearStart,
                EndListDate = school.SchoolYearEnd,
                List01Options0 = false,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo102(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> student = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.DateOfRegistration).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(b => b.DateOfRegistration).ToList();
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var studentFee = new TempM101();
            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var isExist0 = false;
                var isExist1 = false;
                bool isExist2 = false;
                int classroomID = 0;
                string classroomName = "";
                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                var classroomSo = 0;
                if (classroomID > 0)
                    classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;


                if (listPanelModel.List01Options0 == true)
                {
                    DateTime date = Convert.ToDateTime(item.DateOfRegistration);
                    if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                        (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                    {
                        isExist0 = true;
                    }
                    else { isExist0 = false; };
                };

                studentFee.ID = 0;
                studentFee.StudentID = item.StudentID;
                studentFee.UserID = listPanelModel.UserID;
                studentFee.SchoolID = item.SchoolID;
                studentFee.StudentID = item.StudentID;
                studentFee.StudentNumber = item.StudentNumber;
                studentFee.StudentSerialNumber = item.StudentSerialNumber;
                studentFee.ClassroomID = classroomID;
                studentFee.StudentName = item.FirstName + " " + item.LastName;
                studentFee.DateOfRegistration = item.DateOfRegistration;
                studentFee.Name = "";

                if (classroomID > 0)
                {
                    var classrooms = _classroomRepository.GetClassroomID(classroomID);
                    studentFee.ClassroomName = classrooms.ClassroomName;
                    studentFee.Name = classrooms.ClassroomTeacher;
                    studentFee.ReceiptNo = classrooms.SortOrder;
                }

                var cashPayment = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                if (cashPayment != null)
                {
                    studentFee.CashPayment = cashPayment.CashPayment;
                    if (cashPayment.CashPayment > 0) isExist1 = true;
                }
                if (listPanelModel.List01Options1 == true)
                {
                    if (cashPayment.CashPayment == 0) isExist1 = true;
                }

                if ((listPanelModel.List01Options0 == true && isExist0 == true) || (listPanelModel.List01Options0 == false && isExist0 == false))
                    if ((listPanelModel.List01Options1 == true && isExist1 == true) || (listPanelModel.List01Options1 == false && isExist1 == true))
                        _tempM101Repository.CreateTempM101(studentFee);
            }

            var classroom = new TempM101Header();
            classroom.ID = 0;
            classroom.UserID = user.UserID;
            classroom.SchoolID = user.SchoolID;
            classroom.DateFrom = listPanelModel.StartListDate;
            classroom.DateTo = listPanelModel.EndListDate;
            classroom.StartNumber = 0;
            if (listPanelModel.List01Options0 == true) classroom.StartNumber = 1;
            classroom.HeaderFee01 = "3";
            classroom.HeaderFee02 = "4";
            classroom.HeaderFee03 = "10";

            _tempM101HeaderRepository.CreateTempM101Header(classroom);
            var isExistStudent = true;

            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "~/reporting/index/M102StudentCashPaymentLists/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&registrationTypeSubID=" + classroom.HeaderFee01 + "&statuCategorySubID=" + classroom.HeaderFee02 + "&dateControl=" + classroom.StartNumber;

            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List101?userID=" + user.UserID + "&studentID=" + listPanelModel.StudentID + "&msg=" + msg;
            }
            return Redirect(url);
        }

        #endregion

        #region List103
        public IActionResult List103(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);

            string categoryName = "discountName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                List01Options0 = false,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/DiscountTableDataRead/{userID}")]
        public IActionResult DiscountTableDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var discountTable = _discountTableRepository.GetDiscountTablePeriod(user.UserPeriod, user.SchoolID);
            int inx = 0;
            foreach (var item in discountTable)
            {
                inx++;
                item.IsDirtySelect = false;
                item.IsSelect = false;
                if (inx < 11) item.IsSelect = true;

                _discountTableRepository.UpdateDiscountTable(item);
            }
            return Json(discountTable);
        }

        [Route("ListPanel/DiscountTableDataUpdate/{strResult}")]
        public IActionResult DiscountTableDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<DiscountTable>>(strResult);

            List<DiscountTable> list = new List<DiscountTable>();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _discountTableRepository.GetDiscountTable(json[i].DiscountTableID);

                getCode.DiscountTableID = json[i].DiscountTableID;
                getCode.IsSelect = json[i].IsSelect;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _discountTableRepository.UpdateDiscountTable(getCode);
                }
                i++;
            }
            return Json(list);
        }
        public async Task<IActionResult> ListPanelInfo3(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> student = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(b => b.FirstName).ToList();
            }

            var discount = _discountTableRepository.GetDiscountTablePeriodOnlyTrue(user.UserPeriod, user.SchoolID);
            var isExistStudent = false;

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var discountNew = new TempM101Header();
            int inx = 0;
            foreach (var item in discount)
            {
                if (item.IsSelect == true)
                {
                    inx++;
                    if (user.SelectedCulture.Trim() == "tr-TR")
                    {
                        if (inx == 01) { discountNew.HeaderFee01 = item.DiscountName; }
                        if (inx == 02) { discountNew.HeaderFee02 = item.DiscountName; }
                        if (inx == 03) { discountNew.HeaderFee03 = item.DiscountName; }
                        if (inx == 04) { discountNew.HeaderFee04 = item.DiscountName; }
                        if (inx == 05) { discountNew.HeaderFee05 = item.DiscountName; }
                        if (inx == 06) { discountNew.HeaderFee06 = item.DiscountName; }
                        if (inx == 07) { discountNew.HeaderFee07 = item.DiscountName; }
                        if (inx == 08) { discountNew.HeaderFee08 = item.DiscountName; }
                        if (inx == 09) { discountNew.HeaderFee09 = item.DiscountName; }
                        if (inx == 10) { discountNew.HeaderFee10 = item.DiscountName; }
                    }
                    else
                    {
                        if (inx == 01) { discountNew.HeaderFee01 = item.Language1; }
                        if (inx == 02) { discountNew.HeaderFee02 = item.Language1; }
                        if (inx == 03) { discountNew.HeaderFee03 = item.Language1; }
                        if (inx == 04) { discountNew.HeaderFee04 = item.Language1; }
                        if (inx == 05) { discountNew.HeaderFee05 = item.Language1; }
                        if (inx == 06) { discountNew.HeaderFee06 = item.Language1; }
                        if (inx == 07) { discountNew.HeaderFee07 = item.Language1; }
                        if (inx == 08) { discountNew.HeaderFee08 = item.Language1; }
                        if (inx == 09) { discountNew.HeaderFee09 = item.Language1; }
                        if (inx == 10) { discountNew.HeaderFee10 = item.Language1; }
                    }

                }
            }
            discountNew.ID = 0;
            discountNew.UserID = user.UserID;
            discountNew.SchoolID = user.SchoolID;
            _tempM101HeaderRepository.CreateTempM101Header(discountNew);

            var studentDiscount = new TempM101();
            decimal applied = 0, totalFee = 0, totalDiscount = 0;
            //byte percent;

            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom == 0) listPanelModel.StartClassroom = listPanelModel.EndClassroom;
            if (listPanelModel.EndClassroom == 0) listPanelModel.EndClassroom = listPanelModel.StartClassroom;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;

                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");

            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var isExist = false;
                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                string classroom = "";
                var classroomSo = 0;
                if (classroomID > 0)
                {
                    classroom = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                    classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;
                }
                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo)
                {
                    isExist = true;
                }
                else { isExist = false; };

                if (isExist == true)
                {
                    totalFee = 0; totalDiscount = 0;
                    var studentFees = _studentDebtRepository.GetStudentDebtAll(user.SchoolID, user.UserPeriod, item.StudentID);
                    totalFee = studentFees.Sum(p => p.UnitPrice);

                    studentDiscount.Fee01 = 0;
                    studentDiscount.Fee02 = 0;
                    studentDiscount.Fee03 = 0;
                    studentDiscount.Fee04 = 0;
                    studentDiscount.Fee05 = 0;
                    studentDiscount.Fee06 = 0;
                    studentDiscount.Fee07 = 0;
                    studentDiscount.Fee08 = 0;
                    studentDiscount.Fee09 = 0;
                    studentDiscount.Fee10 = 0;

                    inx = 0;
                    bool isZero = false;
                    foreach (var f in discount)
                    {
                        applied = 0;
                        if (f.IsSelect == true)
                        {
                            var studentDiscountDetail = _studentDiscountRepository.GetDiscount4(item.StudentID, user.UserPeriod, user.SchoolID, f.DiscountTableID);
                            inx++;
                            if (studentDiscountDetail.Count() > 0)
                            {
                                applied += studentDiscountDetail.Sum(p => p.DiscountApplied);
                                if (applied > 0)
                                {
                                    isZero = true;
                                    if (inx == 01) { studentDiscount.Fee01 = applied; }
                                    if (inx == 02) { studentDiscount.Fee02 = applied; }
                                    if (inx == 03) { studentDiscount.Fee03 = applied; }
                                    if (inx == 04) { studentDiscount.Fee04 = applied; }
                                    if (inx == 05) { studentDiscount.Fee05 = applied; }
                                    if (inx == 06) { studentDiscount.Fee06 = applied; }
                                    if (inx == 07) { studentDiscount.Fee07 = applied; }
                                    if (inx == 08) { studentDiscount.Fee08 = applied; }
                                    if (inx == 09) { studentDiscount.Fee09 = applied; }
                                    if (inx == 10) { studentDiscount.Fee10 = applied; }
                                    totalDiscount += applied;
                                }
                            }
                        }
                    }
                    if (isExist == true)
                    {
                        if (listPanelModel.List01Options0 == true)
                        {
                            isExistStudent = true;
                            studentDiscount.ID = 0;
                            studentDiscount.UserID = listPanelModel.UserID;
                            studentDiscount.SchoolID = item.SchoolID;
                            studentDiscount.StudentID = item.StudentID;
                            studentDiscount.StudentNumber = item.StudentNumber;
                            studentDiscount.StudentSerialNumber = item.StudentSerialNumber;
                            studentDiscount.ClassroomID = classroomID;
                            studentDiscount.StudentName = item.FirstName + " " + item.LastName;
                            studentDiscount.TotalFee = totalFee;
                            studentDiscount.CashPayment = totalDiscount;
                            _tempM101Repository.CreateTempM101(studentDiscount);
                        }
                        else
                        {
                            if (isZero == true)
                            {
                                isExistStudent = true;
                                studentDiscount.ID = 0;
                                studentDiscount.UserID = listPanelModel.UserID;
                                studentDiscount.SchoolID = item.SchoolID;
                                studentDiscount.StudentID = item.StudentID;
                                studentDiscount.StudentNumber = item.StudentNumber;
                                studentDiscount.StudentSerialNumber = item.StudentSerialNumber;
                                studentDiscount.ClassroomID = classroomID;
                                studentDiscount.StudentName = item.FirstName + " " + item.LastName;
                                studentDiscount.TotalFee = totalFee;
                                studentDiscount.CashPayment = totalDiscount;
                                _tempM101Repository.CreateTempM101(studentDiscount);
                            }
                        }
                    }
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M103StudentDiscountLists/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;
            ViewBag.IsSuccess = false;
            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List103?userID=" + user.UserID + "&msg=" + msg;
            }
            return Redirect(url);

        }

        #endregion

        #region List104
        public IActionResult List104(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);

            string categoryName = "name";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                List01Options0 = true,
                List01Options1 = false,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/SchoolFeeDataRead/{SchoolID}")]
        public IActionResult SchoolFeeDataRead(int schoolID)
        {
            List<SchoolFee> list = new List<SchoolFee>();
            var schoolFee = _schoolFeeRepository.GetSchoolFeeAll(schoolID, "L1");

            return Json(schoolFee);
        }

        public IActionResult SchoolFeeDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFee>>(strResult);

            List<SchoolFee> list = new List<SchoolFee>();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _schoolFeeRepository.GetSchoolFee(item.SchoolFeeID);

                getCode.IsSelect = json[i].IsSelect;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _schoolFeeRepository.UpdateSchoolFee(getCode);
                }
                i++;
            }
            return Json(list);
        }
        public IActionResult ListPanelInfo4(ListPanelModel listPanelModel)
        {
            //await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
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

            var fee = _schoolFeeRepository.GetSchoolFeeOnlyTrue(user.SchoolID, "L1");

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var isExistStudent = false;
            var feeNew = new TempM101Header();
            int inx = 1;
            foreach (var item in fee)
            {
                if (user.SelectedCulture.Trim() == "tr-TR")
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Name; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Name; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Name; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Name; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Name; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Name; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Name; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Name; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Name; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Name; }
                }
                else
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Language1; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Language1; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Language1; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Language1; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Language1; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Language1; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Language1; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Language1; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Language1; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Language1; }
                }
                inx++;
            }
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, total = 0;

            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            foreach (var item in student)
            {
                var statu = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statu == "İptal" || statu == "Kayıt Dondurdu" || statu == "Pasif Kayıt") continue;

                inx = 1;
                var isExist = false;

                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var classroomSo = 0;
                if (classroomID > 0)
                    classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo)
                {
                    isExist = true;
                }
                else { isExist = false; };

                if (isExist == true)
                {
                    var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                    if (listPanelModel.List01Options0 == true)
                        if (statuName == "Kesin Kayıt")
                        { isExist = true; }
                        else { isExist = false; };

                    statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                    if (listPanelModel.List01Options1 == true && listPanelModel.List01Options0 == true)
                        if (statuName == "Kontenjan")
                        { isExist = true; }
                    if (listPanelModel.List01Options1 == true && listPanelModel.List01Options0 == false)
                        if (statuName == "Kontenjan")
                        { isExist = true; }
                        else { isExist = false; };
                    total = 0;
                    if (isExist == true)
                    {
                        foreach (var f in fee)
                        {
                            if (f.IsSelect == true)
                            {
                                var studentFees = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, item.StudentID, f.SchoolFeeID);
                                if (studentFees != null)
                                {
                                    amount = studentFees.Amount;
                                    if (inx == 01) { studentFee.Fee01 = amount; }
                                    if (inx == 02) { studentFee.Fee02 = amount; }
                                    if (inx == 03) { studentFee.Fee03 = amount; }
                                    if (inx == 04) { studentFee.Fee04 = amount; }
                                    if (inx == 05) { studentFee.Fee05 = amount; }
                                    if (inx == 06) { studentFee.Fee06 = amount; }
                                    if (inx == 07) { studentFee.Fee07 = amount; }
                                    if (inx == 08) { studentFee.Fee08 = amount; }
                                    if (inx == 09) { studentFee.Fee09 = amount; }
                                    if (inx == 10) { studentFee.Fee10 = amount; }
                                    total += amount;
                                }

                                studentFee.ID = 0;
                                studentFee.UserID = listPanelModel.UserID;
                                studentFee.SchoolID = item.SchoolID;
                                studentFee.StudentID = item.StudentID;
                                studentFee.StudentNumber = item.StudentNumber;
                                studentFee.StudentSerialNumber = item.StudentSerialNumber;
                                studentFee.ClassroomID = classroomID;
                                studentFee.StudentName = item.FirstName + " " + item.LastName;
                                studentFee.DateOfRegistration = item.DateOfRegistration;
                                studentFee.StudentNumber = item.StudentNumber;
                                studentFee.StudentSerialNumber = item.StudentSerialNumber;
                                studentFee.IsPension = item.IsPension;
                                studentFee.GenderTypeCategoryID = item.GenderTypeCategoryID;
                                isExistStudent = true;
                            }
                            inx++;
                        }

                    }
                    if (total > 0)
                        _tempM101Repository.CreateTempM101(studentFee);
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M104ListByFeesList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            ViewBag.IsSuccess = false;
            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List104?userID=" + user.UserID + "&msg=" + msg;
            }
            return Redirect(url);
        }
        #endregion

        #region List105
        public IActionResult List105(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);

            string categoryName = "name";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                List01Options0 = true,
                List01Options1 = false,
                List01Options2 = false,
                List01Options3 = false,
                StartListDate = DateTime.Now,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/SchoolFeeListDataRead/{schoolID}")]
        public IActionResult SchoolFeeListDataRead(int schoolID)
        {
            List<SchoolFee> list = new List<SchoolFee>();
            var schoolFee = _schoolFeeRepository.GetSchoolFeeAll(schoolID, "L1");
            return Json(schoolFee);
        }

        public async Task<IActionResult> ListPanelInfo5(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> student = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(b => b.FirstName).ToList();
            }

            var fee = _schoolFeeRepository.GetSchoolFee(listPanelModel.FeeID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();

            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            if (user.SelectedCulture.Trim() == "tr-TR")
                feeNew.HeaderFee01 = fee.Name;
            else
                feeNew.HeaderFee01 = fee.Language1;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0;

            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime selectedDate = Convert.ToDateTime(listPanelModel.StartListDate);

            var isExistStudent = false;
            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            foreach (var item in student)
            {
                var statu = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statu == "İptal" || statu == "Kayıt Dondurdu" || statu == "Pasif Kayıt") continue;

                var isExist = false;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var classroomSo = 0;
                if (classroomID > 0)
                    classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo)
                {
                    isExist = true;
                }
                else { isExist = false; };

                if (listPanelModel.ListOpt1 == 4 && isExist == true)
                {
                    isExist = false;
                    var studentinstallments = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);
                    foreach (var i in studentinstallments)
                    {
                        if (i.InstallmentDate == selectedDate) { isExist = true; break; }
                    }
                };

                if (isExist == true)
                {
                    var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;

                    if (isExist == true)
                    {
                        var studentFees = _studentDebtRepository.GetStudentDebt22(user.UserPeriod, user.SchoolID, item.StudentID, listPanelModel.FeeID);
                        if (studentFees != null)
                        {
                            amount = studentFees.Amount;
                            studentFee.Fee01 = amount;
                            if (amount == 0) { isExist = false; };
                            if (listPanelModel.ListOpt1 == 3 && studentFees.IsList == false) { isExist = false; }
                        }

                        studentFee.ID = 0;
                        studentFee.UserID = listPanelModel.UserID;
                        studentFee.SchoolID = item.SchoolID;
                        studentFee.StudentID = item.StudentID;
                        studentFee.IdNumber = item.IdNumber;
                        studentFee.StudentNumber = item.StudentNumber;
                        studentFee.StudentSerialNumber = item.StudentSerialNumber;
                        studentFee.ClassroomID = classroomID;
                        studentFee.StudentName = item.FirstName + " " + item.LastName;
                        studentFee.DateOfRegistration = item.DateOfRegistration;
                        studentFee.StudentNumber = item.StudentNumber;
                        studentFee.StudentSerialNumber = item.StudentSerialNumber;
                        studentFee.IsPension = item.IsPension;
                        studentFee.GenderTypeCategoryID = item.GenderTypeCategoryID;
                    }
                    if (isExist == true)
                    {
                        isExistStudent = true;
                        _tempM101Repository.CreateTempM101(studentFee);
                    }
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();
            string url = null;
            if (listPanelModel.ListOpt1 == 1 || listPanelModel.ListOpt1 == 3 || listPanelModel.ListOpt1 == 4)
                url = "~/reporting/index/M105ListByFeesList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;
            if (listPanelModel.ListOpt1 == 2)
                url = "~/reporting/index/M105ListByFeesFreeList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;
            ViewBag.IsSuccess = false;
            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List105?userID=" + user.UserID + "&msg=" + msg;
            }
            return Redirect(url);
        }

        #endregion

        #region List106
        public IActionResult List106(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options1 = true,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }
        public async Task<IActionResult> ListPanelInfo6(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            var isExist = false;
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var schools = _schoolInfoRepository.GetSchoolInfoAllTrue();
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            var feeNew = new TempM101Header();
            int inx = 1;
            foreach (var item in paymentType)
            {
                if (user.SelectedCulture.Trim() == "tr-TR")
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.CategoryName; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.CategoryName; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.CategoryName; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.CategoryName; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.CategoryName; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.CategoryName; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.CategoryName; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.CategoryName; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.CategoryName; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.CategoryName; }
                }
                else
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Language1; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Language1; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Language1; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Language1; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Language1; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Language1; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Language1; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Language1; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Language1; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Language1; }
                }
                inx++;
            }
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, collection = 0, balance = 0, total = 0;

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            string period1 = user.UserPeriod.Substring(0, 4);
            string period2 = user.UserPeriod.Substring(5, 4);
            int year1 = Convert.ToInt32(period1);
            int year2 = Convert.ToInt32(period2);

            var isExistStudent = false;
            foreach (var sc in schools)
            {
                if (sc.IsSelect == true)
                {
                    List<Student> students = new List<Student>();
                    if (school.NewPeriod != user.UserPeriod)
                    {
                        var allStudents = _studentRepository.GetStudentAllPeriod(sc.SchoolID);
                        var studenPeriod = _studentPeriodsRepository.GetStudentAll(sc.SchoolID, user.UserPeriod);
                        students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                    }
                    else
                    {
                        students = _studentRepository.GetStudentAllWithClassroom(sc.SchoolID).ToList();
                    }
                    var installments = _studentInstallmentRepository.GetStudentInstallmentPeriod(sc.SchoolID, user.UserPeriod);
                    var temps = _studentTempRepository.GetStudentTempAll(sc.SchoolID, user.UserPeriod);
                    foreach (var item in students)
                    {
                        var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                        if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                        inx = 1;
                        total = 0;

                        decimal cashPayment = 0, refundAmount = 0, cancelAmount = 0;
                        studentFee.CashPayment = 0; studentFee.RefundAmount = 0; studentFee.CancelAmount = 0;

                        var temp = temps.Where(b => b.StudentID == item.StudentID).FirstOrDefault();
                        if (temp != null)
                        {
                            cashPayment = temp.CashPayment;
                            refundAmount = temp.RefundAmount1 + temp.RefundAmount2 + temp.RefundAmount3;
                            cancelAmount = temp.CancelDebt;

                            studentFee.Fee01 = 0; studentFee.Fee02 = 0; studentFee.Fee03 = 0; studentFee.Fee04 = 0; studentFee.Fee05 = 0;
                            studentFee.Fee06 = 0; studentFee.Fee07 = 0; studentFee.Fee08 = 0; studentFee.Fee09 = 0; studentFee.Fee10 = 0;
                            studentFee.Fee01Collection = 0; studentFee.Fee02Collection = 0; studentFee.Fee03Collection = 0; studentFee.Fee04Collection = 0; studentFee.Fee05Collection = 0;
                            studentFee.Fee06Collection = 0; studentFee.Fee07Collection = 0; studentFee.Fee08Collection = 0; studentFee.Fee09Collection = 0; studentFee.Fee10Collection = 0;
                            studentFee.Fee01Balance = 0; studentFee.Fee02Balance = 0; studentFee.Fee03Balance = 0; studentFee.Fee04Balance = 0; studentFee.Fee05Balance = 0;
                            studentFee.Fee06Balance = 0; studentFee.Fee07Balance = 0; studentFee.Fee08Balance = 0; studentFee.Fee09Balance = 0; studentFee.Fee10Balance = 0;

                            studentFee.DateOfRegistration = temp.TransactionDate;
                            studentFee.CashPayment = cashPayment;
                            studentFee.RefundAmount = refundAmount;
                            studentFee.CancelAmount = cancelAmount;

                            studentFee.ID = 0;
                            studentFee.UserID = listPanelModel.UserID;
                            studentFee.SchoolID = user.SchoolID;
                            studentFee.StudentID = item.StudentID;

                            isExistStudent = true;
                            _tempM101Repository.CreateTempM101(studentFee);
                        }
                        studentFee = new TempM101();
                        cashPayment = 0; refundAmount = 0; cancelAmount = 0;
                        foreach (var f in paymentType)
                        {
                            var installment = installments.Where(b => b.StudentID == item.StudentID && b.CategoryID == f.CategoryID);

                            foreach (var i in installment)
                            {
                                amount = i.InstallmentAmount;
                                collection = i.PreviousPayment;
                                balance = amount - collection;

                                if (listPanelModel.List01Options1 == false)
                                {
                                    DateTime date = Convert.ToDateTime(i.InstallmentDate);
                                    if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                        (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                                    {
                                        isExist = true;
                                    }
                                    else { isExist = false; };
                                }
                                else { isExist = true; };

                                //Period Control
                                DateTime dt = Convert.ToDateTime(i.InstallmentDate);
                                int year = dt.Year;
                                if (year1 != year && year2 != year) isExist = false;

                                if (isExist == true)
                                {
                                    studentFee.CashPayment = 0;
                                    studentFee.Fee01 = 0; studentFee.Fee02 = 0; studentFee.Fee03 = 0; studentFee.Fee04 = 0; studentFee.Fee05 = 0;
                                    studentFee.Fee06 = 0; studentFee.Fee07 = 0; studentFee.Fee08 = 0; studentFee.Fee09 = 0; studentFee.Fee10 = 0;
                                    studentFee.Fee01Collection = 0; studentFee.Fee02Collection = 0; studentFee.Fee03Collection = 0; studentFee.Fee04Collection = 0; studentFee.Fee05Collection = 0;
                                    studentFee.Fee06Collection = 0; studentFee.Fee07Collection = 0; studentFee.Fee08Collection = 0; studentFee.Fee09Collection = 0; studentFee.Fee10Collection = 0;
                                    studentFee.Fee01Balance = 0; studentFee.Fee02Balance = 0; studentFee.Fee03Balance = 0; studentFee.Fee04Balance = 0; studentFee.Fee05Balance = 0;
                                    studentFee.Fee06Balance = 0; studentFee.Fee07Balance = 0; studentFee.Fee08Balance = 0; studentFee.Fee09Balance = 0; studentFee.Fee10Balance = 0;

                                    if (inx == 01) { studentFee.Fee01 = amount; studentFee.Fee01Collection = collection; studentFee.Fee01Balance = balance; }
                                    if (inx == 02) { studentFee.Fee02 = amount; studentFee.Fee02Collection = collection; studentFee.Fee02Balance = balance; }
                                    if (inx == 03) { studentFee.Fee03 = amount; studentFee.Fee03Collection = collection; studentFee.Fee03Balance = balance; }
                                    if (inx == 04) { studentFee.Fee04 = amount; studentFee.Fee04Collection = collection; studentFee.Fee04Balance = balance; }
                                    if (inx == 05) { studentFee.Fee05 = amount; studentFee.Fee05Collection = collection; studentFee.Fee05Balance = balance; }
                                    if (inx == 06) { studentFee.Fee06 = amount; studentFee.Fee06Collection = collection; studentFee.Fee06Balance = balance; }
                                    if (inx == 07) { studentFee.Fee07 = amount; studentFee.Fee07Collection = collection; studentFee.Fee07Balance = balance; }
                                    if (inx == 08) { studentFee.Fee08 = amount; studentFee.Fee08Collection = collection; studentFee.Fee08Balance = balance; }
                                    if (inx == 09) { studentFee.Fee09 = amount; studentFee.Fee09Collection = collection; studentFee.Fee09Balance = balance; }
                                    if (inx == 10) { studentFee.Fee10 = amount; studentFee.Fee10Collection = collection; studentFee.Fee10Balance = balance; }
                                    total += amount;

                                    if (i.InstallmentAmount == i.PreviousPayment)
                                        studentFee.DateOfRegistration = i.PaymentDate;
                                    else studentFee.DateOfRegistration = i.InstallmentDate;

                                    studentFee.ID = 0;
                                    studentFee.UserID = listPanelModel.UserID;
                                    studentFee.SchoolID = sc.SchoolID;
                                    studentFee.StudentID = item.StudentID;
                                    studentFee.StudentNumber = item.StudentNumber;
                                    studentFee.StudentSerialNumber = item.StudentSerialNumber;

                                    isExistStudent = true;
                                    _tempM101Repository.CreateTempM101(studentFee);
                                }
                            }
                            inx++;
                        }
                    }
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M106IncomeStatementLists/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List106?userID=" + user.UserID + "&msg=" + msg;
            }
            return Redirect(url);

        }

        [Route("ListPanel/SchoolSelectingDataRead/{userID}")]
        public IActionResult SchoolSelectingDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfoAllTrue().ToList();

            List<SchoolSelectingViewModel> list = new List<SchoolSelectingViewModel>();
            foreach (var item in schoolInfo)
            {
                bool isSelect = false;
                if (item.SchoolID == user.SchoolID) isSelect = true;

                var school = new SchoolSelectingViewModel
                {
                    Id = item.SchoolID,
                    Name = item.CompanyName,
                    IsSelect = isSelect,
                };
                var getCode = _schoolInfoRepository.GetSchoolInfo(school.Id);
                getCode.IsSelect = isSelect;
                _schoolInfoRepository.UpdateSchoolInfo(getCode);

                list.Add(school);
            }
            return Json(list);
        }

        [Route("ListPanel/SchoolSelectingDataUpdate/{strResult}")]
        public IActionResult SchoolSelectingDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolSelectingViewModel>>(strResult);

            foreach (var item in json)
            {
                var getCode = _schoolInfoRepository.GetSchoolInfo(item.Id);
                getCode.IsSelect = item.IsSelect;

                if (ModelState.IsValid)
                {
                    _schoolInfoRepository.UpdateSchoolInfo(getCode);
                }
            }
            return Json(true);
        }

        #endregion

        #region List107
        public IActionResult List107(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                //List01Options0 = true,
                //List01Options1 = false,
                //List01Options2 = false,
                List01Options3 = false,
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/SchoolPaymentTypeDataRead7/{schoolID}")]
        public IActionResult SchoolFeeListDataRead7(int schoolID)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            foreach (var item in paymentType)
            {
                item.IsSelect = true;
                item.IsDirtySelect = false;
                _parameterRepository.UpdateParameter(item);
            }
            return Json(paymentType);
        }
        public IActionResult SchoolPaymentTypeDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Parameter>>(strResult);

            List<Parameter> list = new List<Parameter>();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _parameterRepository.GetParameter(json[i].CategoryID);

                getCode.CategoryID = json[i].CategoryID;
                getCode.IsSelect = json[i].IsSelect;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _parameterRepository.UpdateParameter(getCode);
                }
                i++;
            }
            return Json(list);
        }
        public async Task<IActionResult> ListPanelInfo7(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var schools = _schoolInfoRepository.GetSchoolInfoAllTrue();

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubIDOnlyTrue(categoryID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var collectionHeader = new TempM101Header();

            collectionHeader.UserID = user.UserID;
            collectionHeader.SchoolID = user.SchoolID;

            var collection = new TempM101();
            decimal amount = 0;

            collectionHeader.ID = 0;
            collectionHeader.UserID = user.UserID;
            collectionHeader.SchoolID = user.SchoolID;
            collectionHeader.DateFrom = date1;
            collectionHeader.DateTo = date2;
            _tempM101HeaderRepository.CreateTempM101Header(collectionHeader);

            var isExistStudent = false;

            foreach (var sc in schools)
            {
                if (sc.IsSelect == true)
                {
                    if (school.NewPeriod != user.UserPeriod)
                    {
                        var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                        var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                        students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                    }
                    else
                    {
                        students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
                    }

                    foreach (var item in students)
                    {
                        var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                        if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                        var studentPayment = _studentPaymentRepository.GetPaymentOrder(user.SchoolID, user.UserPeriod, item.StudentID);
                        var isExist = false;
                        bool isExist2 = false;
                        string classroomName = "";
                        int classroomID = 0;
                        decimal refund = 0;
                        //if (listPanelModel.List01Options3 == true)
                        //{
                        if (school.NewPeriod == user.UserPeriod)
                            classroomID = item.ClassroomID;
                        else
                        {
                            isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                            if (isExist2)
                            {
                                classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                                isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                                if (isExist2)
                                    classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                            }
                        }

                        var cashPayment = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                        refund = (cashPayment.RefundAmount1 + cashPayment.RefundAmount2 + cashPayment.RefundAmount3);

                        if (cashPayment != null && cashPayment.CashPayment > 0 && listPanelModel.List01Options3 == true)
                        {
                            isExist = true;

                            collection.ID = 0;
                            collection.UserID = listPanelModel.UserID;
                            collection.SchoolID = user.SchoolID;
                            collection.SchoolNumber = sc.SchoolID;
                            collection.StudentID = item.StudentID;
                            collection.StudentNumber = item.StudentNumber;
                            collection.StudentSerialNumber = item.StudentSerialNumber;
                            collection.ClassroomID = classroomID;

                            collection.StudentName = item.FirstName + " " + item.LastName;
                            collection.ParentName = item.ParentName;
                            collection.DateOfRegistration = cashPayment.TransactionDate;
                            collection.ReceiptNo = cashPayment.ReceiptNo;
                            collection.AccountReceipt = cashPayment.AccountReceipt;
                            collection.Fee01 = cashPayment.CashPayment;
                            string bankName = "";
                            if (cashPayment.BankID > 0)
                            {
                                bankName = _bankRepository.GetBank(cashPayment.BankID).BankName;
                            }

                            collection.TypeAndNo = "Peşin" + "-" + bankName;
                            _tempM101Repository.CreateTempM101(collection);
                        }
                        //}

                        amount = 0;
                        foreach (var p in studentPayment)
                        {
                            DateTime date = Convert.ToDateTime(p.PaymentDate);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                            {
                                isExist = true;
                            }
                            else { isExist = false; };

                            if (isExist == true)
                            {
                                collection.ID = 0;
                                collection.UserID = listPanelModel.UserID;
                                collection.SchoolID = user.SchoolID;
                                collection.SchoolNumber = sc.SchoolID;
                                collection.StudentID = item.StudentID;
                                collection.ClassroomID = classroomID;
                                collection.StudentName = item.FirstName + " " + item.LastName;
                                collection.ParentName = item.ParentName;
                                collection.DateOfRegistration = p.PaymentDate;
                                collection.ReceiptNo = p.ReceiptNo;
                                collection.AccountReceipt = p.AccountReceipt;
                                collection.Fee01 = 0;
                                string typeAndNo = null;

                                var IP = _studentInstallmentPaymentRepository.GetStudentInstallmentPayment2(user.UserPeriod, item.StudentID, p.StudentPaymentID);
                                var inx = 0;

                                foreach (var ip in IP)
                                {
                                    var installment = _studentInstallmentRepository.GetStudentInstallmentID(ip.SchoolID, ip.StudentInstallmentID);
                                    if (installment != null)
                                    {
                                        foreach (var itemx in paymentType)
                                        {
                                            if (itemx.IsSelect == true && installment.CategoryID == itemx.CategoryID)
                                            {
                                                amount = p.PaymentAmount;

                                                var typeName = _parameterRepository.GetParameter(installment.CategoryID).CategoryName;

                                                string bankName = "";
                                                if (installment.BankID > 0)
                                                {
                                                    bankName = _bankRepository.GetBank((int)installment.BankID).BankName;
                                                }

                                                typeAndNo = typeName + "-" + installment.InstallmentNo + "-" + bankName;
                                                inx++;
                                            }
                                        }
                                    }
                                    collection.Fee01 = amount - refund;
                                    collection.Fee02 = refund;
                                    if (amount == 0) { isExist = false; };

                                }
                                collection.TypeAndNo = typeAndNo;

                            }
                            if (isExist == true)
                            {
                                isExistStudent = true;
                                _tempM101Repository.CreateTempM101(collection);
                            }
                        }
                    }

                }
            }

            var daily = new TempM101();
            //DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            //DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var accountCode = "";
            if (listPanelModel.StartClassroom > 0)
                accountCode = _parameterRepository.GetParameter(listPanelModel.StartClassroom).CategoryName;

            decimal? check01 = 0, bond01 = 0, ots01 = 0, mailOrder01 = 0, creditCard01 = 0, kmh01 = 0, cashBank01 = 0, cashCase01 = 0;
            decimal? check02 = 0, bond02 = 0, ots02 = 0, mailOrder02 = 0, creditCard02 = 0, kmh02 = 0, cashBank02 = 0, cashCase02 = 0;
            decimal? check03 = 0, bond03 = 0, ots03 = 0, mailOrder03 = 0, creditCard03 = 0, kmh03 = 0, cashBank03 = 0, cashCase03 = 0;
            decimal? check04 = 0, bond04 = 0, ots04 = 0, mailOrder04 = 0, creditCard04 = 0, kmh04 = 0, cashBank04 = 0, cashCase04 = 0;
            decimal? check05 = 0, bond05 = 0, ots05 = 0, mailOrder05 = 0, creditCard05 = 0, kmh05 = 0, cashBank05 = 0, cashCase05 = 0;
            int length1 = 0, length4 = 0, length5 = 0, length6 = 0, length7 = 0, length8 = 0, length9 = 0, length11 = 0;
            if (school.AccountNoID01 != null) length1 = school.AccountNoID01.Length;
            if (school.AccountNoID04 != null) length4 = school.AccountNoID04.Length;
            if (school.AccountNoID05 != null) length5 = school.AccountNoID05.Length;
            if (school.AccountNoID06 != null) length6 = school.AccountNoID06.Length;
            if (school.AccountNoID07 != null) length7 = school.AccountNoID07.Length;
            if (school.AccountNoID08 != null) length8 = school.AccountNoID08.Length;
            if (school.AccountNoID09 != null) length9 = school.AccountNoID09.Length;
            if (school.AccountNoID11 != null) length11 = school.AccountNoID11.Length;

            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);
            foreach (var item in accounting)
            {
                daily.ID = 0;
                daily.UserID = listPanelModel.UserID;
                daily.SchoolID = user.SchoolID;

                string code1 = "";
                string code4 = "";
                string code5 = "";
                string code6 = "";
                string code7 = "";
                string code8 = "";
                string code9 = "";
                string code11 = "";
                int lenght11 = item.AccountCode.Length;
                int lenght44 = item.AccountCode.Length;
                int lenght55 = item.AccountCode.Length;
                int lenght66 = item.AccountCode.Length;
                int lenght77 = item.AccountCode.Length;
                int lenght88 = item.AccountCode.Length;
                int lenght99 = item.AccountCode.Length;
                int lenght1111 = item.AccountCode.Length;

                if (lenght11 >= length1) code1 = item.AccountCode.Substring(0, length1);
                if (lenght44 >= length4) code4 = item.AccountCode.Substring(0, length4);
                if (lenght55 >= length5) code5 = item.AccountCode.Substring(0, length5);
                if (lenght66 >= length6) code6 = item.AccountCode.Substring(0, length6);
                if (lenght77 >= length7) code7 = item.AccountCode.Substring(0, length7);
                if (lenght88 >= length8) code8 = item.AccountCode.Substring(0, length8);
                if (lenght99 >= length9) code9 = item.AccountCode.Substring(0, length9);
                if (lenght1111 >= length11) code11 = item.AccountCode.Substring(0, length11);

                bool isExist = true;
                if (accountCode != "")
                    if (accountCode == item.CodeTypeName) isExist = true;
                    else isExist = false;

                if (item.AccountDate >= date1 && item.AccountDate <= date2)
                {
                    if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase02 += item.Debt;
                    if (code4 == school.AccountNoID04.Substring(0, length4)) check02 += item.Debt;
                    if (code5 == school.AccountNoID05.Substring(0, length5)) bond02 += item.Debt;
                    if (code6 == school.AccountNoID06.Substring(0, length6)) ots02 += item.Debt;
                    if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank02 += item.Debt;
                    if (code8 == school.AccountNoID08.Substring(0, length8))
                        if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard02 += item.Debt;
                        else mailOrder02 += item.Debt;
                    if (code11 == school.AccountNoID11.Substring(0, length11)) kmh02 += item.Debt;

                    if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase04 += item.Credit;
                    if (code4 == school.AccountNoID04.Substring(0, length4)) check04 += item.Credit;
                    if (code5 == school.AccountNoID05.Substring(0, length5)) bond04 += item.Credit;
                    if (code6 == school.AccountNoID06.Substring(0, length6)) ots04 += item.Credit;
                    if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank04 += item.Credit;
                    if (code8 == school.AccountNoID08.Substring(0, length8))
                        if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard04 += item.Credit;
                        else mailOrder04 += item.Credit;
                    if (code11 == school.AccountNoID11.Substring(0, length11)) kmh04 += item.Credit;

                    if (code1 == school.AccountNoID01.Substring(0, length1) && isExist == true)
                    {
                        daily.Status = null;
                        daily.BondTypeTitle = item.AccountCode;
                        daily.Name = item.AccountCodeName;
                        daily.DateOfRegistration = item.AccountDate;
                        daily.InWriting = item.Explanation;
                        daily.CashPayment = item.Debt;
                        daily.TotalFee = item.Credit;
                        _tempM101Repository.CreateTempM101(daily);
                    }
                }
                else
                {
                    if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase01 += item.Debt - item.Credit;
                    if (code4 == school.AccountNoID04.Substring(0, length4)) check01 += item.Debt - item.Credit;
                    if (code5 == school.AccountNoID05.Substring(0, length5)) bond01 += item.Debt - item.Credit;
                    if (code6 == school.AccountNoID06.Substring(0, length6)) ots01 += item.Debt - item.Credit;
                    if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank01 += item.Debt - item.Credit;
                    if (code8 == school.AccountNoID08.Substring(0, length8))
                        if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard01 += item.Debt - item.Credit;
                        else mailOrder01 += item.Debt - item.Credit;
                    if (code11 == school.AccountNoID11.Substring(0, length11)) kmh01 += item.Debt - item.Credit;
                }

                if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase03 = cashCase01 + cashCase02;
                if (code4 == school.AccountNoID04.Substring(0, length4)) check03 = check01 + check02;
                if (code5 == school.AccountNoID05.Substring(0, length5)) bond03 = bond01 + bond02;
                if (code6 == school.AccountNoID06.Substring(0, length6)) ots03 = ots01 + ots02;
                if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank03 = cashBank01 + cashBank02;
                if (code8 == school.AccountNoID08.Substring(0, length8))
                    if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard03 = creditCard01 + creditCard02;
                    else mailOrder03 = mailOrder01 + mailOrder02;
                if (code11 == school.AccountNoID11.Substring(0, length11)) kmh03 = kmh01 + kmh02;

                if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase05 = cashCase03 - cashCase04;
                if (code4 == school.AccountNoID04.Substring(0, length4)) check05 = check03 - check04;
                if (code5 == school.AccountNoID05.Substring(0, length5)) bond05 = bond03 - bond04;
                if (code6 == school.AccountNoID06.Substring(0, length6)) ots05 = ots03 - ots04;
                if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank05 = cashBank03 - cashBank04;
                if (code8 == school.AccountNoID08.Substring(0, length8))
                    if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard05 = creditCard03 - creditCard04;
                    else mailOrder05 = mailOrder03 - mailOrder04;
                if (code11 == school.AccountNoID11.Substring(0, length11)) kmh05 = kmh03 - kmh04;
            }

            //row1
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferPreviousH;
            daily.Status = "row1";

            daily.Fee01 = check01;
            daily.Fee02 = bond01;
            daily.Fee03 = kmh01;
            daily.Fee04 = ots01;
            daily.Fee05 = mailOrder01;
            daily.Fee06 = creditCard01;
            daily.Fee07 = cashBank01;
            daily.Fee08 = cashCase01;
            _tempM101Repository.CreateTempM101(daily);

            //row2
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TodayH;
            daily.Status = "row2";
            daily.Fee01 = check02;
            daily.Fee02 = bond02;
            daily.Fee03 = kmh02;
            daily.Fee04 = ots02;
            daily.Fee05 = mailOrder02;
            daily.Fee06 = creditCard02;
            daily.Fee07 = cashBank02;
            daily.Fee08 = cashCase02;
            _tempM101Repository.CreateTempM101(daily);

            //row3
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TotalH;
            daily.Status = "row3";
            daily.Fee01 = check03;
            daily.Fee02 = bond03;
            daily.Fee03 = kmh03;
            daily.Fee04 = ots03;
            daily.Fee05 = mailOrder03;
            daily.Fee06 = creditCard03;
            daily.Fee07 = cashBank03;
            daily.Fee08 = cashCase03;
            _tempM101Repository.CreateTempM101(daily);

            //row4
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.ExpenseH;
            daily.Status = "row4";
            daily.Fee01 = check04;
            daily.Fee02 = bond04;
            daily.Fee03 = kmh04;
            daily.Fee04 = ots04;
            daily.Fee05 = mailOrder04;
            daily.Fee06 = creditCard04;
            daily.Fee07 = cashBank04;
            daily.Fee08 = cashCase04;
            _tempM101Repository.CreateTempM101(daily);

            //row5
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferNextH;
            daily.Status = "row5";
            daily.Fee01 = check05;
            daily.Fee02 = bond05;
            daily.Fee03 = kmh05;
            daily.Fee04 = ots05;
            daily.Fee05 = mailOrder05;
            daily.Fee06 = creditCard05;
            daily.Fee07 = cashBank05;
            daily.Fee08 = cashCase05;
            _tempM101Repository.CreateTempM101(daily);


            string selectedLanguage = user.SelectedCulture.Trim();
            string url = null;
            url = "~/reporting/index/M107CollectionList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;
            ViewBag.IsSuccess = false;
            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List107?userID=" + user.UserID + "&msg=" + msg;
            }
            return Redirect(url);
        }

        #endregion

        #region List108
        public IActionResult List108(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options1 = true,
            };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }
        public async Task<IActionResult> ListPanelInfo8(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var isExist = false;
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            var feeNew = new TempM101Header();
            int inx = 1;
            foreach (var item in paymentType)
            {
                feeNew.ID = 0;
                feeNew.UserID = user.UserID;
                feeNew.SchoolID = user.SchoolID;
                if (user.SelectedCulture.Trim() == "tr-TR")
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.CategoryName; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.CategoryName; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.CategoryName; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.CategoryName; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.CategoryName; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.CategoryName; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.CategoryName; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.CategoryName; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.CategoryName; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.CategoryName; }
                }
                else
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Language1; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Language1; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Language1; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Language1; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Language1; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Language1; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Language1; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Language1; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Language1; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Language1; }
                }
                inx++;
            }
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, total = 0;

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            string period1 = user.UserPeriod.Substring(0, 4);
            string period2 = user.UserPeriod.Substring(5, 4);
            int year1 = Convert.ToInt32(period1);
            int year2 = Convert.ToInt32(period2);

            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                inx = 1;
                total = 0;

                decimal cashPayment = 0, refundAmount = 0, cancelAmount = 0;
                var temp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                if (temp != null)
                {
                    cashPayment = temp.CashPayment;
                    refundAmount = temp.RefundAmount1 + temp.RefundAmount2 + temp.RefundAmount3;
                    cancelAmount = temp.CancelDebt;
                }

                foreach (var f in paymentType)
                {
                    var installment = _studentInstallmentRepository.GetStudentInstallmentByCategory(item.SchoolID, item.StudentID, user.UserPeriod, f.CategoryID);

                    foreach (var i in installment)
                    {
                        if (listPanelModel.ListOpt1 == 1)
                            amount = i.PreviousPayment;
                        else amount = i.InstallmentAmount;

                        if (listPanelModel.List01Options1 == false)
                        {
                            DateTime date = Convert.ToDateTime(i.InstallmentDate);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                            {
                                isExist = true;
                            }
                            else { isExist = false; };
                        }
                        else { isExist = true; };

                        //Period Control
                        DateTime dt = Convert.ToDateTime(i.InstallmentDate);
                        int year = dt.Year;
                        if (year1 != year && year2 != year) isExist = false;

                        if (isExist == true)
                        {
                            if (f.CategoryID == i.CategoryID && inx == 01) { studentFee.Fee01 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 02) { studentFee.Fee02 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 03) { studentFee.Fee03 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 04) { studentFee.Fee04 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 05) { studentFee.Fee05 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 06) { studentFee.Fee06 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 07) { studentFee.Fee07 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 08) { studentFee.Fee08 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 09) { studentFee.Fee09 = amount; }
                            if (f.CategoryID == i.CategoryID && inx == 10) { studentFee.Fee10 = amount; }
                            total += amount;

                            if (i.InstallmentAmount == i.PreviousPayment)
                                studentFee.DateOfRegistration = i.PaymentDate;
                            else studentFee.DateOfRegistration = i.InstallmentDate;

                            studentFee.CashPayment = cashPayment;
                            studentFee.RefundAmount = refundAmount;
                            studentFee.CancelAmount = cancelAmount;

                            studentFee.ID = 0;
                            studentFee.UserID = listPanelModel.UserID;
                            studentFee.SchoolID = item.SchoolID;
                            studentFee.StudentID = item.StudentID;
                            studentFee.StudentNumber = item.StudentNumber;
                            studentFee.StudentSerialNumber = item.StudentSerialNumber;

                            _tempM101Repository.CreateTempM101(studentFee);
                            cashPayment = 0;
                            studentFee.Fee01 = 0; studentFee.Fee02 = 0; studentFee.Fee03 = 0; studentFee.Fee04 = 0; studentFee.Fee05 = 0;
                            studentFee.Fee06 = 0; studentFee.Fee07 = 0; studentFee.Fee08 = 0; studentFee.Fee09 = 0; studentFee.Fee10 = 0;

                        }
                    }
                    inx++;
                }

            }
            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M108CollectionListByMonth/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);

        }

        #endregion

        #region List109
        public IActionResult List109(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options0 = false,
                List01Options1 = false,
                List01Options2 = false,
            };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo9(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var schools = _schoolInfoRepository.GetSchoolInfoAllTrue();

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            if (listPanelModel.List01Options0 == false) feeNew.DateTo = DateTime.Now;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, collection = 0, balance = 0, cashPayment = 0, delayedPayment = 0;

            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }
            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            foreach (var sc in schools)
            {
                if (sc.IsSelect == true)
                {

                    if (school.NewPeriod != user.UserPeriod)
                    {
                        var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                        var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                        students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
                    }
                    else
                    {
                        students = _studentRepository.GetStudentAllWithClassroom(sc.SchoolID).ToList();
                    }
                    foreach (var item in students)
                    {
                        if (listPanelModel.List01Options1 == false)
                        {
                            var statu = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                            if (statu == "İptal" || statu == "Kayıt Dondurdu" || statu == "Pasif Kayıt") continue;
                        }
                        amount = 0; collection = 0; balance = 0; cashPayment = 0; delayedPayment = 0;
                        var isExist = true;
                        bool isExist2 = false;
                        string classroomName = "";
                        int classroomID = 0;

                        if (school.NewPeriod == user.UserPeriod)
                            classroomID = item.ClassroomID;
                        else
                        {
                            isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                            if (isExist2)
                            {
                                classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                                isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                                if (isExist2)
                                    classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                            }
                        }
                        var classroomSo = 0;
                        if (classroomID > 0)
                            classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                        if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo)
                        {
                            isExist = true;
                        }
                        else { isExist = false; };

                        var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                        if (listPanelModel.List01Options1 == true)
                            if (statuName == "İptal")
                            { isExist = true; }
                            else { isExist = false; };

                        if (isExist == true)
                        {
                            var installment = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);
                            var payments = _studentPaymentRepository.GetPaymentOrder(user.SchoolID, user.UserPeriod, item.StudentID);
                            var temp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                            if (temp != null) { cashPayment = temp.CashPayment; }
                            var parentAddress1 = _studentAddressRepository.GetStudentAddress(item.StudentID);
                            var parentAddress2 = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);

                            foreach (var ins in installment)
                            {
                                DateTime date = Convert.ToDateTime(ins.InstallmentDate);
                                if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                    (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                                {
                                    if (ins.StatusCategoryID != statusID) { delayedPayment += (ins.InstallmentAmount - ins.PreviousPayment); }
                                    if (listPanelModel.List01Options2 == true) { isExist = true; };

                                    if (delayedPayment > 0 && listPanelModel.List01Options4 == true)
                                    {
                                        amount = installment.Sum(p => p.InstallmentAmount);
                                        amount += cashPayment;
                                        collection = payments.Sum(p => p.PaymentAmount);
                                        collection += cashPayment;
                                        balance = amount - collection;

                                        if (listPanelModel.List01Options2 == true)
                                            if (balance == 0) isExist = true;
                                            else isExist = false;
                                        if (listPanelModel.List01Options2 == false)
                                            if (balance == 0) isExist = false;

                                        studentFee.ID = 0;
                                        studentFee.UserID = listPanelModel.UserID;
                                        studentFee.SchoolID = user.SchoolID;
                                        studentFee.SchoolNumber = sc.SchoolID;
                                        studentFee.StudentID = item.StudentID;
                                        studentFee.StudentSerialNumber = item.StudentSerialNumber;
                                        studentFee.ClassroomID = classroomID;
                                        studentFee.StudentName = item.FirstName + " " + item.LastName;
                                        studentFee.ParentName = item.ParentName;
                                        studentFee.IdNumber = item.IdNumber;
                                        if (parentAddress2 != null)
                                            studentFee.ParentMobilePhone = parentAddress2.MobilePhone;
                                        studentFee.DateOfRegistration = item.DateOfRegistration;
                                        studentFee.Fee01 = amount;
                                        studentFee.Fee02 = collection;
                                        studentFee.Fee03 = delayedPayment;
                                        studentFee.InWriting = string.Format("{0:dd/MM/yyyy}", ins.InstallmentDate) + "  " + String.Format("{0:0.00}", delayedPayment);
                                        studentFee.Fee04 = balance;
                                        studentFee.GenderTypeCategoryID = 0;
                                        if (isExist == true)
                                            if (listPanelModel.List01Options3 == true && delayedPayment == 0)
                                                continue;
                                            else
                                                _tempM101Repository.CreateTempM101(studentFee);
                                    }
                                }
                                else
                                    if (listPanelModel.List01Options2 == true) { isExist = false; };
                            }
                            if (listPanelModel.List01Options4 == false)
                            {
                                //var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);
                                amount = installment.Sum(p => p.InstallmentAmount);
                                amount += cashPayment;
                                collection = payments.Sum(p => p.PaymentAmount);
                                collection += cashPayment;
                                balance = amount - collection;

                                if (listPanelModel.List01Options2 == true)
                                    if (balance == 0) isExist = true;
                                    else isExist = false;
                                if (listPanelModel.List01Options2 == false)
                                    if (balance == 0) isExist = false;

                                studentFee.ID = 0;
                                studentFee.UserID = listPanelModel.UserID;
                                studentFee.SchoolID = user.SchoolID;
                                studentFee.SchoolNumber = sc.SchoolID;
                                studentFee.StudentID = item.StudentID;
                                studentFee.StudentNumber = item.StudentNumber;
                                studentFee.StudentSerialNumber = item.StudentSerialNumber;
                                studentFee.ClassroomID = classroomID;
                                studentFee.StudentName = item.FirstName + " " + item.LastName;
                                studentFee.ParentName = item.ParentName;
                                studentFee.IdNumber = item.IdNumber;
                                if (parentAddress2 != null)
                                    studentFee.ParentMobilePhone = parentAddress2.MobilePhone;
                                studentFee.DateOfRegistration = item.DateOfRegistration;
                                studentFee.Fee01 = amount;
                                studentFee.Fee02 = collection;
                                studentFee.Fee03 = delayedPayment;
                                studentFee.InWriting = String.Format("{0:0.00}", delayedPayment);
                                studentFee.Fee04 = balance;
                                studentFee.GenderTypeCategoryID = 0;
                                if (isExist == true)
                                    if (listPanelModel.List01Options3 == true && delayedPayment == 0)
                                        continue;
                                    else
                                        _tempM101Repository.CreateTempM101(studentFee);
                            }
                        }
                    }
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M109StudentTotalDebtList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }

        #endregion

        #region List110
        public IActionResult List110(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                //List01Options0 = true,
                //List01Options1 = false,
                //List01Options2 = false,
                List01Options3 = false,
                StartListDate = DateTime.Now,
                EndListDate = DateTime.Now.AddMonths(1),
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }
        public IActionResult BankNameCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var bankNameType = _bankRepository.GetBankAll(user.SchoolID);
            return Json(bankNameType);
        }

        [Route("ListPanel/SchoolPaymentTypeDataRead10/{schoolID}")]
        public IActionResult SchoolFeeListDataRead10(int schoolID)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            foreach (var item in paymentType)
            {
                item.IsSelect = true;
                item.IsDirtySelect = false;
                _parameterRepository.UpdateParameter(item);
            }
            return Json(paymentType);
        }
        public async Task<IActionResult> ListPanelInfo10(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var collectionHeader = new TempM101Header();

            collectionHeader.UserID = user.UserID;
            collectionHeader.SchoolID = user.SchoolID;

            var collection = new TempM101();

            collectionHeader.DateFrom = date1;
            collectionHeader.DateTo = date2;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            if (listPanelModel.List01Options3 == false)
                collectionHeader.HeaderFee01 = Resources.Resource.ParentID2;
            else collectionHeader.HeaderFee01 = Resources.Resource.StudentID2;

            collectionHeader.HeaderFee02 = null;
            if (listPanelModel.StartClassroom > 0)
                collectionHeader.HeaderFee02 = _bankRepository.GetBank(listPanelModel.StartClassroom).BankName;

            _tempM101HeaderRepository.CreateTempM101Header(collectionHeader);

            int bank = 0;
            if (listPanelModel.StartClassroom > 0)
                bank = _bankRepository.GetBank(listPanelModel.StartClassroom).BankID;

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubIDOnlyTrue(categoryID);

            var isExistStudent = false;

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.FirstName).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(b => b.FirstName).ToList();
            }
            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);
                var isExist = false;
                foreach (var f in paymentType)
                {
                    var installment = _studentInstallmentRepository.GetStudentInstallmentByCategory(item.SchoolID, item.StudentID, user.UserPeriod, f.CategoryID);

                    foreach (var i in installment)
                    {
                        DateTime date = Convert.ToDateTime(i.InstallmentDate);
                        if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                            (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                        {
                            isExist = true;
                        }
                        else { isExist = false; };

                        if (bank > 0 && isExist == true)
                        {
                            if ((String.Compare(i.BankID.ToString(), bank.ToString()) == 0))
                            {
                                isExist = true;
                            }
                            else { isExist = false; };
                        }

                        if (isExist == true)
                        {
                            collection.ID = 0;
                            collection.UserID = listPanelModel.UserID;
                            collection.SchoolID = item.SchoolID;
                            collection.StudentID = item.StudentID;
                            collection.StudentNumber = item.StudentNumber;
                            collection.StudentSerialNumber = item.StudentSerialNumber;
                            collection.ClassroomID = classroomID;
                            collection.StudentName = item.FirstName + " " + item.LastName;
                            collection.ParentName = item.ParentName;
                            collection.DateOfRegistration = i.InstallmentDate;

                            if (listPanelModel.List01Options3 == false)
                            {
                                if (parentAddress != null)
                                    collection.IdNumber = parentAddress.IdNumber;

                            }
                            else collection.IdNumber = item.IdNumber;
                            var typeName = "";
                            if (user.SelectedCulture.Trim() == "tr-TR")
                                typeName = _parameterRepository.GetParameter(i.CategoryID).CategoryName;
                            else typeName = _parameterRepository.GetParameter(i.CategoryID).Language1;
                            collection.TypeCategoryName = typeName;
                            collection.TypeAndNo = i.InstallmentNo.ToString();
                            collection.Fee01 = i.InstallmentAmount;
                            collection.Fee01Collection = i.PreviousPayment;
                            collection.Fee01Balance = i.InstallmentAmount - i.PreviousPayment;

                            isExistStudent = true;
                            _tempM101Repository.CreateTempM101(collection);
                        }
                    }
                }

            }
            string selectedLanguage = user.SelectedCulture.Trim();
            string url = null;
            url = "~/reporting/index/M110MonthlyPaymentList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;
            ViewBag.IsSuccess = false;
            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List110?userID=" + user.UserID + "&msg=" + msg;
            }
            return Redirect(url);
        }
        #endregion

        #region List111
        public IActionResult List111(int userID, int msg, int prg)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var title = "";
            if (prg == 1) title = Resources.Resource.PaymentListbyMonth;
            else title = Resources.Resource.PaymentStatusListbyMonth;

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,

                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = schoolInfo.FinancialYearStart,
                EndListDate = schoolInfo.FinancialYearEnd,

                Title = title,
                Prg = prg,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/SchoolPaymentTypeDataRead11/{schoolID}")]
        public IActionResult SchoolFeeListDataRead11(int schoolID)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            foreach (var item in paymentType)
            {
                item.IsSelect = true;
                _parameterRepository.UpdateParameter(item);
            }
            return Json(paymentType);
        }
        public IActionResult ListPanelInfo11(ListPanelModel listPanelModel)
        {
            //await Task.Delay(100);
            var isExist = false;
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            List<Student> students = new List<Student>();

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var startdate = listPanelModel.StartListDate;
            var feeNew = new TempM101Header();

            CultureInfo culture = new CultureInfo("tr-TR");

            for (int d = 0; d < 13; d++)
            {
                var date = startdate.Value.AddMonths(d);
                var strDate = date.ToString("MMMM yyyy", new CultureInfo(user.SelectedCulture.Trim()));

                if (d == 00) { feeNew.HeaderFee01 = strDate; }
                if (d == 01) { feeNew.HeaderFee02 = strDate; }
                if (d == 02) { feeNew.HeaderFee03 = strDate; }
                if (d == 03) { feeNew.HeaderFee04 = strDate; }
                if (d == 04) { feeNew.HeaderFee05 = strDate; }
                if (d == 05) { feeNew.HeaderFee06 = strDate; }
                if (d == 06) { feeNew.HeaderFee07 = strDate; }
                if (d == 07) { feeNew.HeaderFee08 = strDate; }
                if (d == 08) { feeNew.HeaderFee09 = strDate; }
                if (d == 09) { feeNew.HeaderFee10 = strDate; }
                if (d == 10) { feeNew.HeaderFee11 = strDate; }
                if (d == 11) { feeNew.HeaderFee12 = strDate; }
            }

            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;

            feeNew.BankName = null;
            if (listPanelModel.StartClassroom > 0)
                feeNew.BankName = _bankRepository.GetBank(listPanelModel.StartClassroom).BankName;

            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            int bank = 0;
            if (listPanelModel.StartClassroom > 0)
                bank = _bankRepository.GetBank(listPanelModel.StartClassroom).BankID;

            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            decimal amount = 0, collection = 0;

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var inx = 0;
            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var studentFee = new TempM101();
                studentFee.Fee01 = 0; studentFee.Fee02 = 0; studentFee.Fee03 = 0; studentFee.Fee04 = 0; studentFee.Fee05 = 0; studentFee.Fee06 = 0;
                studentFee.Fee07 = 0; studentFee.Fee08 = 0; studentFee.Fee09 = 0; studentFee.Fee10 = 0; studentFee.Fee11 = 0; studentFee.Fee12 = 0;
                studentFee.Fee01Collection = 0; studentFee.Fee02Collection = 0; studentFee.Fee03Collection = 0; studentFee.Fee04Collection = 0; studentFee.Fee05Collection = 0; studentFee.Fee06Collection = 0;
                studentFee.Fee07Collection = 0; studentFee.Fee08Collection = 0; studentFee.Fee09Collection = 0; studentFee.Fee10Collection = 0; studentFee.Fee11Collection = 0; studentFee.Fee12Collection = 0;
                studentFee.PreviousAmount = 0; studentFee.NextAmount = 0; studentFee.PreviousCollection = 0; studentFee.NextCollection = 0;
                inx = 1;
                isExist = false;
                decimal cashPayment = 0, refundAmount = 0, cancelAmount = 0;
                var temp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                if (temp != null)
                {
                    cashPayment = temp.CashPayment;
                    refundAmount = temp.RefundAmount1 + temp.RefundAmount2 + temp.RefundAmount3;
                    cancelAmount = temp.CancelDebt;
                }

                foreach (var f in paymentType)
                {
                    var installment = _studentInstallmentRepository.GetStudentInstallmentByCategory(item.SchoolID, item.StudentID, user.UserPeriod, f.CategoryID);

                    foreach (var i in installment)
                    {
                        DateTime date = Convert.ToDateTime(i.InstallmentDate);
                        if (listPanelModel.Prg == 1)
                        {
                            if (i.StatusCategoryID == statusID) { isExist = false; collection = i.PreviousPayment; }
                            else { isExist = true; }
                        }
                        else
                        {
                            collection = i.PreviousPayment;
                            isExist = true;
                        }

                        if (bank > 0 && isExist == true)
                        {
                            if ((String.Compare(i.BankID.ToString(), bank.ToString()) == 0))
                            {
                                isExist = true;
                            }
                            else { isExist = false; };
                        }
                        if (isExist == true)
                        {
                            amount = i.InstallmentAmount;

                            if (DateTime.Compare(date, date1) < 0) { studentFee.PreviousAmount += amount; studentFee.PreviousCollection += collection; }
                            if (DateTime.Compare(date, date2) >= 0) { studentFee.NextAmount += amount; studentFee.NextCollection += collection; }
                        }

                        if (isExist == true)
                        {
                            for (int d = 0; d < 13; d++)
                            {
                                var datec = startdate.Value.AddMonths(d);
                                DateTime dt = Convert.ToDateTime(datec);
                                int MM = dt.Month;
                                int YYYY = dt.Year;

                                DateTime dti = Convert.ToDateTime(i.InstallmentDate);
                                int MMi = dti.Month;
                                int YYYYi = dti.Year;

                                if (MM == MMi && YYYY == YYYYi && d == 00) { studentFee.Fee01 += amount; studentFee.Fee01Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 01) { studentFee.Fee02 += amount; studentFee.Fee02Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 02) { studentFee.Fee03 += amount; studentFee.Fee03Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 03) { studentFee.Fee04 += amount; studentFee.Fee04Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 04) { studentFee.Fee05 += amount; studentFee.Fee05Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 05) { studentFee.Fee06 += amount; studentFee.Fee06Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 06) { studentFee.Fee07 += amount; studentFee.Fee07Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 07) { studentFee.Fee08 += amount; studentFee.Fee08Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 08) { studentFee.Fee09 += amount; studentFee.Fee09Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 09) { studentFee.Fee10 += amount; studentFee.Fee10Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 10) { studentFee.Fee11 += amount; studentFee.Fee11Collection += collection; }
                                if (MM == MMi && YYYY == YYYYi && d == 11) { studentFee.Fee12 += amount; studentFee.Fee12Collection += collection; }

                                inx++;

                                studentFee.ID = 0;
                                studentFee.UserID = listPanelModel.UserID;
                                studentFee.SchoolID = item.SchoolID;
                                studentFee.StudentID = item.StudentID;
                                studentFee.StudentNumber = item.StudentNumber;
                                studentFee.StudentSerialNumber = item.StudentSerialNumber;
                                studentFee.StudentName = item.FirstName + " " + item.LastName;
                                studentFee.ParentName = item.ParentName;
                                decimal totalBalance = (decimal)(studentFee.Fee01 + studentFee.Fee02 + studentFee.Fee03 + studentFee.Fee04 + studentFee.Fee05 + studentFee.Fee06 +
                                                          studentFee.Fee07 + studentFee.Fee08 + studentFee.Fee09 + studentFee.Fee10 + studentFee.Fee11 + studentFee.Fee12 +
                                                          studentFee.PreviousAmount + studentFee.NextAmount);
                                studentFee.Fee01Balance = totalBalance;

                                decimal totalCollection = (decimal)(studentFee.Fee01Collection + studentFee.Fee02Collection + studentFee.Fee03Collection + studentFee.Fee04Collection + studentFee.Fee05Collection + studentFee.Fee06Collection +
                                                          studentFee.Fee07Collection + studentFee.Fee08Collection + studentFee.Fee09Collection + studentFee.Fee10Collection + studentFee.Fee11Collection + studentFee.Fee12Collection +
                                                          studentFee.PreviousCollection + studentFee.NextCollection);
                                studentFee.Fee02Balance = totalCollection;

                                studentFee.RefundAmount = refundAmount;
                                studentFee.CancelAmount = cancelAmount;
                            }
                        }
                    }
                }
                if (isExist == true)
                    _tempM101Repository.CreateTempM101(studentFee);
            }
            string url = "";
            string selectedLanguage = user.SelectedCulture.Trim();
            if (listPanelModel.Prg == 1)
                url = "~/reporting/index/M111PaymentListbyMonth/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode + "&bankID=" + listPanelModel.StartClassroom;
            else
                url = "~/reporting/index/M111PaymentStatusListbyMonth/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode + "&bankID=" + listPanelModel.StartClassroom;

            return Redirect(url);

        }
        #endregion

        #region List112
        public IActionResult List112(int userID, int msg, int prg)
        {
            var user = _usersRepository.GetUser(userID);

            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                Prg = prg,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            if (prg == 1) TempData["title"] = Resources.Resource.ChecksAndBondsByPositions;
            else TempData["title"] = Resources.Resource.ChecksAndBondsByPositionsDetailed;

            return View(listPanelModel);
        }

        [Route("ListPanel/SchoolPaymentTypeDataRead12/{schoolID}")]
        public IActionResult SchoolPaymentTypeDataRead12(int schoolID)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            foreach (var item in paymentType)
            {
                item.IsSelect = true;
                _parameterRepository.UpdateParameter(item);
            }
            return Json(paymentType);
        }
        [Route("ListPanel/PositionsTypeDataRead12/{schoolID}")]
        public IActionResult PositionsTypeDataRead12(int schoolID)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            foreach (var item in paymentType)
            {
                item.IsSelect = true;
                _parameterRepository.UpdateParameter(item);
            }
            return Json(paymentType);
        }
        public IActionResult PositionsTypeDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Parameter>>(strResult);

            List<Parameter> list = new List<Parameter>();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _parameterRepository.GetParameter(json[i].CategoryID);

                getCode.CategoryID = json[i].CategoryID;
                getCode.IsSelect = json[i].IsSelect;
                list.Add(getCode);
                if (ModelState.IsValid)
                {
                    _parameterRepository.UpdateParameter(getCode);
                }
                i++;
            }
            return Json(list);
        }
        public async Task<IActionResult> ListPanelInfo12(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var isExist = false;
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> students = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubIDOnlyTrue(categoryID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            CultureInfo culture = new CultureInfo("tr-TR");

            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;

            feeNew.BankName = null;
            if (listPanelModel.StartClassroom > 0)
                feeNew.BankName = _bankRepository.GetBank(listPanelModel.StartClassroom).BankName;

            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            int bank = 0;
            if (listPanelModel.StartClassroom > 0)
                bank = _bankRepository.GetBank(listPanelModel.StartClassroom).BankID;

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var pcategoryID = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var position = _parameterRepository.GetParameterSubIDOnlyTrue2(pcategoryID);
            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                decimal amount = 0, collection = 0;
                isExist = false;

                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var studentFee = new TempM101();

                foreach (var f in paymentType)
                {
                    var installment = _studentInstallmentRepository.GetStudentInstallmentByCategory(item.SchoolID, item.StudentID, user.UserPeriod, f.CategoryID);

                    foreach (var i in installment)
                    {
                        foreach (var p in position)
                        {
                            if (p.CategoryID == i.StatusCategoryID) { isExist = true; }
                        }
                        if (isExist == true)
                        {
                            DateTime date = Convert.ToDateTime(i.InstallmentDate);
                            if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                            {
                                isExist = true;
                            }
                            else { isExist = false; };
                        }

                        if (bank > 0 && isExist == true)
                        {
                            if ((String.Compare(i.BankID.ToString(), bank.ToString()) == 0))
                            {
                                isExist = true;
                            }
                            else { isExist = false; };
                        }

                        if (isExist == true)
                        {
                            amount = i.InstallmentAmount;
                            collection = i.PreviousPayment;

                            studentFee.Fee01 = amount;
                            studentFee.Fee01Collection = collection;
                            studentFee.Status = "";
                            var typeName = "";
                            if (user.SelectedCulture.Trim() == "tr-TR")
                            {
                                typeName = _parameterRepository.GetParameter(i.CategoryID).CategoryName;
                                if (i.StatusCategoryID > 0)
                                    studentFee.Status = _parameterRepository.GetParameter(i.StatusCategoryID).CategoryName;
                            }
                            else
                            {
                                typeName = _parameterRepository.GetParameter(i.CategoryID).Language1;
                                if (i.StatusCategoryID > 0)
                                    studentFee.Status = _parameterRepository.GetParameter(i.StatusCategoryID).Language1;
                            }

                            studentFee.ID = 0;
                            studentFee.UserID = listPanelModel.UserID;
                            studentFee.SchoolID = item.SchoolID;
                            studentFee.StudentID = item.StudentID;
                            studentFee.StudentNumber = item.StudentNumber;
                            studentFee.StudentSerialNumber = item.StudentSerialNumber;
                            studentFee.DateOfRegistration = i.InstallmentDate;
                            studentFee.ClassroomID = classroomID;

                            studentFee.Name = "";
                            if (i.BankID > 0)
                                studentFee.Name = _bankRepository.GetBank((int)i.BankID).BankName;
                            studentFee.BondCity = i.CheckCardNo;
                            studentFee.GuarantorOther = i.Drawer;
                            studentFee.GuarantorAddress = i.Endorser;
                            studentFee.ParentAddress = i.Explanation;

                            studentFee.StudentName = item.FirstName + " " + item.LastName;
                            studentFee.ParentName = item.ParentName;

                            studentFee.TypeCategoryName = typeName;
                            studentFee.TypeAndNo = i.InstallmentNo.ToString();

                            _tempM101Repository.CreateTempM101(studentFee);
                        }
                    }
                }
            }
            string url = "";
            string selectedLanguage = user.SelectedCulture.Trim();
            if (listPanelModel.Prg == 1)
                url = "~/reporting/index/M112ChecksAndBondsByPositions/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode + "&schoolID=" + user.SchoolID;
            else
                url = "~/reporting/index/M112ChecksAndBondsByPositionsDetailed/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode + "&schoolID=" + user.SchoolID;

            return Redirect(url);

        }
        #endregion

        #region List113
        public IActionResult List113(int userID, int msg, int prg)
        {
            var user = _usersRepository.GetUser(userID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartNumber = 0,
                EndNumber = 0,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/SchoolPaymentTypeDataRead13/{schoolID}")]
        public IActionResult SchoolFeeListDataRead13(int schoolID)
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            foreach (var item in paymentType)
            {
                item.IsSelect = true;
                _parameterRepository.UpdateParameter(item);
            }
            return Json(paymentType);
        }
        public async Task<IActionResult> ListPanelInfo13(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubIDOnlyTrue(categoryID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var collectionHeader = new TempM101Header();

            collectionHeader.UserID = user.UserID;
            collectionHeader.SchoolID = user.SchoolID;

            var collection = new TempM101();

            collectionHeader.StartNumber = listPanelModel.StartNumber;
            collectionHeader.EndNumber = listPanelModel.EndNumber;
            _tempM101HeaderRepository.CreateTempM101Header(collectionHeader);

            var isExistStudent = false;
            foreach (var item in students)
            {
                foreach (var f in paymentType)
                {
                    var installment = _studentInstallmentRepository.GetStudentInstallmentByCategory(item.SchoolID, item.StudentID, user.UserPeriod, f.CategoryID);
                    foreach (var i in installment)
                    {
                        if (i.InstallmentNo >= listPanelModel.StartNumber && i.InstallmentNo <= listPanelModel.EndNumber || listPanelModel.StartNumber == 0 && listPanelModel.EndNumber == 0)
                        {
                            collection.ID = 0;
                            collection.UserID = listPanelModel.UserID;
                            collection.SchoolID = item.SchoolID;
                            collection.StudentID = item.StudentID;
                            collection.StudentName = item.FirstName + " " + item.LastName;
                            collection.ParentName = item.ParentName;
                            collection.DateOfRegistration = i.InstallmentDate;

                            var typeName = _parameterRepository.GetParameter(i.CategoryID).CategoryName;
                            collection.TypeCategoryName = typeName;
                            collection.ReceiptNo = i.InstallmentNo;
                            collection.Fee01 = i.InstallmentAmount;

                            isExistStudent = true;
                            _tempM101Repository.CreateTempM101(collection);
                        }
                    }
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();
            string url = null;
            url = "~/reporting/index/M113PaymentTransactionNumbers/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;
            ViewBag.IsSuccess = false;
            if (isExistStudent == false)
            {
                int msg = 1;
                url = "../List113?userID=" + user.UserID + "&msg=" + msg + "&prg=2";
            }
            return Redirect(url);
        }
        #endregion

        #region List114
        public IActionResult List114(int userID, int msg, int prg)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var title = Resources.Resource.StudentFeeStatusListSubDetailed;

            string categoryName = "name";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,

                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = schoolInfo.FinancialYearStart,
                EndListDate = schoolInfo.FinancialYearEnd,
                FeeID = 0,
                Title = title,
                Prg = prg,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        [Route("ListPanel/FeeSubDetailDataRead114/{schoolID}/{subID}/{L}")]
        public IActionResult FeeSubDetailDataRead114(int schoolID, int subID, string L)
        {
            var schoolFee = _schoolFeeRepository.GetSchoolFeeSubControl(schoolID, subID, L);
            return Json(schoolFee);
        }

        public async Task<IActionResult> ListPanelInfo114(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> student = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).OrderBy(b => b.DateOfRegistration).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).OrderBy(b => b.DateOfRegistration).ToList();
            }

            int feeID = listPanelModel.FeeID;
            if (listPanelModel.Lsw == "L3") feeID = listPanelModel.FeeID2;


            var fee = _schoolFeeRepository.GetSchoolFeeSubControl(user.SchoolID, feeID, listPanelModel.Lsw);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            int inx = 1;
            foreach (var item in fee)
            {
                if (user.SelectedCulture.Trim() == "tr-TR")
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Name; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Name; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Name; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Name; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Name; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Name; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Name; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Name; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Name; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Name; }
                }
                else
                {
                    if (inx == 01) { feeNew.HeaderFee01 = item.Language1; }
                    if (inx == 02) { feeNew.HeaderFee02 = item.Language1; }
                    if (inx == 03) { feeNew.HeaderFee03 = item.Language1; }
                    if (inx == 04) { feeNew.HeaderFee04 = item.Language1; }
                    if (inx == 05) { feeNew.HeaderFee05 = item.Language1; }
                    if (inx == 06) { feeNew.HeaderFee06 = item.Language1; }
                    if (inx == 07) { feeNew.HeaderFee07 = item.Language1; }
                    if (inx == 08) { feeNew.HeaderFee08 = item.Language1; }
                    if (inx == 09) { feeNew.HeaderFee09 = item.Language1; }
                    if (inx == 10) { feeNew.HeaderFee10 = item.Language1; }
                }
                inx++;
            }
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, total = 0;
            int outgoing = 0, totalOutgoing = 0, totalBalance;
            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var isExistStudent = false;
            foreach (var item in student)
            {
                amount = 0;
                var isExist = false;
                bool isExist2 = false;
                int classroomID = 0;
                string classroomName = "";
                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                var classroomSo = 0;
                if (classroomID > 0)
                    classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo)
                {
                    isExist = true;
                }
                else { isExist = false; };

                if (listPanelModel.List01Options0 == true && isExist == true)
                {
                    DateTime date = Convert.ToDateTime(item.DateOfRegistration);
                    if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                        (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                    {
                        isExist = true;
                    }
                    else { isExist = false; };
                };

                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (listPanelModel.List01Options1 == true && listPanelModel.List01Options3 == false && isExist == true)
                    if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Ön Kayıt")
                    { isExist = false; }
                    else { isExist = true; };


                if (listPanelModel.List01Options3 == true && isExist == true)
                    if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Ön Kayıt" || statuName == "Takipte(İptal")
                    { isExist = true; }
                    else { isExist = false; };

                studentFee.Fee01 = 0; studentFee.Fee02 = 0; studentFee.Fee03 = 0; studentFee.Fee04 = 0; studentFee.Fee05 = 0;
                studentFee.Fee06 = 0; studentFee.Fee07 = 0; studentFee.Fee08 = 0; studentFee.Fee09 = 0; studentFee.Fee10 = 0;
                studentFee.Fee01Collection = 0; studentFee.Fee02Collection = 0; studentFee.Fee03Collection = 0; studentFee.Fee04Collection = 0; studentFee.Fee05Collection = 0;
                studentFee.Fee06Collection = 0; studentFee.Fee07Collection = 0; studentFee.Fee08Collection = 0; studentFee.Fee09Collection = 0; studentFee.Fee10Collection = 0;
                studentFee.Fee01Balance = 0; studentFee.Fee02Balance = 0; studentFee.Fee03Balance = 0; studentFee.Fee04Balance = 0; studentFee.Fee05Balance = 0;
                studentFee.Fee06Balance = 0; studentFee.Fee07Balance = 0; studentFee.Fee08Balance = 0; studentFee.Fee09Balance = 0; studentFee.Fee10Balance = 0;
                if (isExist == true)
                {
                    inx = 1;
                    total = 0;

                    foreach (var f in fee)
                    {
                        var studentFees = _studentDebtDetailRepository.GetStudentDebtDetailAllCategory(user.UserPeriod, user.SchoolID, item.StudentID, f.SchoolFeeID, (int)f.SchoolFeeSubID).SingleOrDefault();
                        if (studentFees != null)
                        {
                            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFees(user.UserPeriod, f.SchoolFeeID, (int)f.SchoolFeeSubID, 1);

                            amount = (decimal)studentFees.Amount * (int)studentFees.StockQuantity;
                            outgoing = (int)studentFees.StockQuantity;

                            var schoolFeeTableAll = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID22(user.SchoolID, user.UserPeriod, schoolFeeTable.CategoryID);
                            totalOutgoing = (int)schoolFeeTableAll.Max(a => a.SortOrder);
                            totalBalance = (int)schoolFeeTable.StockQuantity;

                            if (inx == 01) { studentFee.Fee01 = amount; studentFee.Fee01Collection = outgoing; studentFee.Fee01Balance = totalBalance; }
                            if (inx == 02) { studentFee.Fee02 = amount; studentFee.Fee02Collection = outgoing; studentFee.Fee02Balance = totalBalance; }
                            if (inx == 03) { studentFee.Fee03 = amount; studentFee.Fee03Collection = outgoing; studentFee.Fee03Balance = totalBalance; }
                            if (inx == 04) { studentFee.Fee04 = amount; studentFee.Fee04Collection = outgoing; studentFee.Fee04Balance = totalBalance; }
                            if (inx == 05) { studentFee.Fee05 = amount; studentFee.Fee05Collection = outgoing; studentFee.Fee05Balance = totalBalance; }
                            if (inx == 06) { studentFee.Fee06 = amount; studentFee.Fee06Collection = outgoing; studentFee.Fee06Balance = totalBalance; }
                            if (inx == 07) { studentFee.Fee07 = amount; studentFee.Fee07Collection = outgoing; studentFee.Fee07Balance = totalBalance; }
                            if (inx == 08) { studentFee.Fee08 = amount; studentFee.Fee08Collection = outgoing; studentFee.Fee08Balance = totalBalance; }
                            if (inx == 09) { studentFee.Fee09 = amount; studentFee.Fee09Collection = outgoing; studentFee.Fee09Balance = totalBalance; }
                            if (inx == 10) { studentFee.Fee10 = amount; studentFee.Fee10Collection = outgoing; studentFee.Fee10Balance = totalBalance; }
                            total += amount;
                            inx++;
                        }

                        if (listPanelModel.List01Options2 == true && total == 0) { isExist = false; };

                        studentFee.ID = 0;
                        studentFee.UserID = listPanelModel.UserID;
                        studentFee.SchoolID = item.SchoolID;
                        studentFee.StudentID = item.StudentID;
                        studentFee.StudentNumber = item.StudentNumber;
                        studentFee.StudentSerialNumber = item.StudentSerialNumber;
                        studentFee.ClassroomID = classroomID;
                        studentFee.StudentName = item.FirstName + " " + item.LastName;
                        studentFee.DateOfRegistration = item.DateOfRegistration;
                        studentFee.NextAmount = total;
                    }
                    if (isExist == true)
                        isExistStudent = true;
                    if (amount > 0)
                        _tempM101Repository.CreateTempM101(studentFee);
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();

            var exitID = 0;
            if (listPanelModel.ExitID == 1) exitID = 1;
            string url = "~/reporting/index/M114StudentFeeStatusListSubDetailed/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List101?userID=" + user.UserID + "studentID = " + listPanelModel.StudentID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }
        public IActionResult FeeNameCombo1(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var fee = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1").OrderBy(a => a.SortOrder);
            return Json(fee);
        }

        [Route("ListPanel/FeeNameCombo2/{userID}/{feeID}")]
        public IActionResult FeeNameCombo2(int userID, int feeID)
        {
            var user = _usersRepository.GetUser(userID);
            var fee = _schoolFeeRepository.GetSchoolFeeLevel2(user.SchoolID, feeID).OrderBy(a => a.SortOrder);
            return Json(fee);
        }

        #endregion

        #region List115
        public IActionResult List115(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
            };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo15(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var schools = _schoolInfoRepository.GetSchoolInfoAllTrue();

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            if (listPanelModel.List01Options0 == false) feeNew.DateTo = DateTime.Now;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();
            decimal amount = 0, collection = 0, balance = 0, cashPayment = 0, delayedPayment = 0;

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var statusID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
            students = _studentRepository.GetStudentSibling1(user.SchoolID, listPanelModel.StudentID).ToList();
            foreach (var item in students)
            {
                amount = 0; collection = 0; balance = 0; cashPayment = 0; delayedPayment = 0;
                bool isExist = true;

                if (isExist == true)
                {
                    var installment = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);
                    var payments = _studentPaymentRepository.GetPaymentOrder(user.SchoolID, user.UserPeriod, item.StudentID);
                    var temp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, item.StudentID);
                    if (temp != null) { cashPayment = temp.CashPayment; }

                    isExist = false;

                    foreach (var ins in installment)
                    {
                        DateTime date = Convert.ToDateTime(ins.InstallmentDate);
                        if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                            (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                        {
                            if (ins.StatusCategoryID != statusID)
                            { delayedPayment += ins.InstallmentAmount; }
                            isExist = true;
                        }
                    }

                    var parentAddress = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);

                    amount = installment.Sum(p => p.InstallmentAmount);
                    amount += cashPayment;
                    collection = payments.Sum(p => p.PaymentAmount);
                    collection += cashPayment;
                    balance = amount - collection;

                    studentFee.ID = 0;
                    studentFee.UserID = listPanelModel.UserID;
                    studentFee.SchoolID = user.SchoolID;
                    studentFee.StudentID = item.StudentID;
                    studentFee.StudentNumber = item.StudentNumber;
                    studentFee.StudentSerialNumber = item.StudentSerialNumber;

                    studentFee.StudentName = item.FirstName + " " + item.LastName;
                    studentFee.ParentName = item.ParentName;
                    if (parentAddress != null)
                        studentFee.ParentMobilePhone = parentAddress.MobilePhone;
                    studentFee.DateOfRegistration = item.DateOfRegistration;
                    studentFee.Fee01 = amount;
                    studentFee.Fee02 = collection;
                    studentFee.Fee03 = delayedPayment;
                    studentFee.Fee04 = balance;
                    studentFee.GenderTypeCategoryID = 0;
                    studentFee.ClassroomID = item.ClassroomID;
                    if (isExist == true)
                        _tempM101Repository.CreateTempM101(studentFee);
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M115StudentTotalDebtList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }

        public IActionResult SiblingStudents(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var student = _studentRepository.GetStudentSibling(user.SchoolID).ToList();
            foreach (var item in student)
            {
                item.FirstName += " " + item.LastName;
            }

            return Json(student);
        }
        #endregion

        #region List116
        public IActionResult List116(int userID, int msg, int prg)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var title = Resources.Resource.StockList;

            string categoryName = "name";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,

                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = schoolInfo.FinancialYearStart,
                EndListDate = schoolInfo.FinancialYearEnd,
                FeeID = 0,
                Title = title,
                Prg = prg,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }
        public async Task<IActionResult> ListPanelInfo116(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            int feeID = listPanelModel.FeeID;

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            //int inx = 1;

            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentFee = new TempM101();

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var isExist = false;
            var debtDetail = _studentDebtDetailRepository.GetStudentDebtDetailAllSchool2(user.SchoolID, user.UserPeriod).DistinctBy(t => t.SchoolFeeID).ToList();

            foreach (var item in debtDetail)
            {
                var student = _studentRepository.GetStudent(item.StudentID);
                DateTime date = Convert.ToDateTime(student.DateOfRegistration);
                if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                    (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                {
                    isExist = true;
                }
                else { isExist = false; };

                if (isExist)
                {
                    var stock = _studentDebtDetailRepository.GetStudentDebtDetailID2(user.SchoolID, user.UserPeriod, item.CategoryID, item.SchoolFeeID, item.StudentDebtID);

                    var schoolFee = _schoolFeeRepository.GetSchoolFee(item.SchoolFeeID);
                    var stockCode = _schoolFeeTableRepository.GetSchoolFees2(user.SchoolID, user.UserPeriod, schoolFee.FeeCategory, item.CategoryID, item.StudentDebtID);
                    var quantity = _schoolFeeTableRepository.GetSchoolFees2(user.SchoolID, user.UserPeriod, schoolFee.FeeCategory, item.SchoolFeeID, item.StudentDebtID);

                    studentFee.ID = 0;
                    studentFee.SchoolID = user.SchoolID;
                    studentFee.UserID = user.UserID;
                    studentFee.StrDate = item.StockCode;
                    studentFee.ParentName = schoolFee.Name;

                    if (quantity != null)
                    {
                        studentFee.AccountReceipt = quantity.StockQuantity;
                    }

                    studentFee.ReceiptNo = stock.Sum(a => a.StockQuantity);
                    studentFee.Fee01 = stock.Sum(a => a.Amount);

                    _tempM101Repository.CreateTempM101(studentFee);
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();


            string url = "~/reporting/index/M116StockList/0/0/?schoolID=" + user.SchoolID + "&userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;


            //var exitID = 0;
            //if (listPanelModel.ExitID == 1) exitID = 1;
            //string url = "~/reporting/index/M114StudentFeeStatusListSubDetailed/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;


            return Redirect(url);
        }

        #endregion

        #region List201
        public async Task<IActionResult> List201(int userID)
        {
            await Task.Delay(100);

            List<Student> student = new List<Student>();

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            if (school.NewPeriod != user.UserPeriod)
            {
                student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }
            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt" || statuName == "Takipte (İptal)") continue;

                string classroomName = "";
                int classroomID = 0;

                var isExist = false;
                if (school.NewPeriod == user.UserPeriod)
                {
                    if (item.ClassroomID > 0)
                    {
                        classroomID = item.ClassroomID;
                        classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                    }
                }
                else
                {
                    isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomID > 0)
                {
                    var tempM101 = new TempM101();
                    tempM101.ID = 0;
                    tempM101.UserID = userID;
                    tempM101.SchoolID = item.SchoolID;
                    tempM101.StudentID = item.StudentID;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.StudentSerialNumber = item.StudentSerialNumber;
                    tempM101.ClassroomName = classroomName;
                    tempM101.ClassroomID = classroomID;
                    tempM101.StudentName = item.FirstName + " " + item.LastName;
                    tempM101.IdNumber = item.IdNumber;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.Name = "";
                    if (classroomID > 0)
                        tempM101.Name = _classroomRepository.GetClassroomID(classroomID).ClassroomTeacher;

                    _tempM101Repository.CreateTempM101(tempM101);
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "~/reporting/index/M201ClassroomLists/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }
        #endregion

        #region List202
        public IActionResult List202(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = DateTime.Now,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo202(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {

                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroomTrueFalse(user.SchoolID).ToList();
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var classroomHeader = new TempM101Header();
            classroomHeader.ID = 0;
            classroomHeader.UserID = user.UserID;
            classroomHeader.SchoolID = user.SchoolID;
            classroomHeader.DateFrom = listPanelModel.StartListDate;
            classroomHeader.HeaderFee01 = user.UserPeriod;
            _tempM101HeaderRepository.CreateTempM101Header(classroomHeader);

            var classroom = new TempM101();
            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (school.NewPeriod == user.UserPeriod)
                {
                    classroomID = item.ClassroomID;
                    classroomName = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                }
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                classroom.ID = 0;
                classroom.SchoolID = item.SchoolID;
                classroom.UserID = user.UserID;
                classroom.StudentID = item.StudentID;

                classroom.GenderTypeCategoryID = item.GenderTypeCategoryID;
                classroom.ClassroomID = classroomID;
                classroom.ClassroomName = classroomName;

                classroom.ReceiptNo = item.RegistrationTypeCategoryID;
                classroom.AccountReceipt = item.StatuCategoryID;

                _tempM101Repository.CreateTempM101(classroom);
            }

            var isExistStudent = true;
            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "~/reporting/index/M202ClassroomLists/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode + "&registrationTypeSubID=" + "3" + "&statuCategorySubID=" + "4";

            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List202?userID=" + user.UserID + "&studentID=" + listPanelModel.StudentID + "&msg=" + msg;
            }

            return Redirect(url);
        }

        #endregion

        #region List203
        public IActionResult List203(int userID, int studentID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                StudentID = studentID,
                ExitID = exitID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = school.SchoolYearStart,
                EndListDate = school.SchoolYearEnd,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo203(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                students = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var classroom = new TempM101();
            foreach (var item in students)
            {
                var isExist = true;

                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (item.DateOfRegistration != null)
                {
                    DateTime date = Convert.ToDateTime(item.DateOfRegistration);
                    if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                        (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                    {
                        isExist = true;
                    }
                    else { isExist = false; };
                };

                if (school.NewPeriod == user.UserPeriod)
                {
                    classroomID = item.ClassroomID;
                    classroomName = _classroomRepository.GetClassroomID(item.ClassroomID).ClassroomName;
                }
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomID > 0 && isExist == true)
                {
                    classroom.ID = 0;
                    classroom.SchoolID = item.SchoolID;
                    classroom.UserID = user.UserID;
                    classroom.StudentID = item.StudentID;

                    classroom.GenderTypeCategoryID = item.GenderTypeCategoryID;
                    classroom.ClassroomID = classroomID;
                    classroom.ClassroomName = classroomName;
                    classroom.ReceiptNo = item.RegistrationTypeCategoryID;
                    classroom.AccountReceipt = item.StatuCategoryID;
                    _tempM101Repository.CreateTempM101(classroom);
                }
            }

            var c = new TempM101Header();
            c.ID = 0;
            c.UserID = user.UserID;
            c.SchoolID = user.SchoolID;
            c.DateFrom = listPanelModel.StartListDate;
            c.DateTo = listPanelModel.EndListDate;
            c.HeaderFee01 = "3";
            c.HeaderFee02 = "4";
            c.HeaderFee03 = "10";

            _tempM101HeaderRepository.CreateTempM101Header(c);
            var isExistStudent = true;

            string selectedLanguage = user.SelectedCulture.Trim();

            var exitID = 0;
            if (listPanelModel.ExitID == 1) exitID = 1;
            string url = "~/reporting/index/M203ClassroomLists/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&registrationTypeSubID=" + c.HeaderFee01 + "&statuCategorySubID=" + c.HeaderFee02;

            if (isExistStudent == false)
            {
                int msg = 1;
                url = "/ListPanel/List203?userID=" + user.UserID + "&studentID=" + listPanelModel.StudentID + "&msg=" + msg + "&exitID=0";
            }
            return Redirect(url);
        }
        #endregion

        #region List204
        public async Task<IActionResult> List204(int userID, int studentID, int exitID)
        {
            await Task.Delay(100);
            List<Student> student = new List<Student>();

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            if (school.NewPeriod != user.UserPeriod)
            {
                student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }
            foreach (var item in student)
            {
                if (studentID != 0 && item.StudentID != studentID) continue;

                string classroomName = "";
                int classroomID = 0;

                var isExist = false;
                if (school.NewPeriod == user.UserPeriod)
                {
                    if (item.ClassroomID > 0)
                    {
                        classroomID = item.ClassroomID;
                        classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                    }
                }
                else
                {
                    isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomID > 0)
                {
                    var tempM101 = new TempM101();
                    tempM101.ID = 0;
                    tempM101.UserID = userID;
                    tempM101.SchoolID = item.SchoolID;
                    tempM101.StudentID = item.StudentID;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.StudentSerialNumber = item.StudentSerialNumber;
                    tempM101.ClassroomName = classroomName;
                    tempM101.ClassroomID = classroomID;
                    tempM101.StudentName = item.FirstName + " " + item.LastName;
                    tempM101.IdNumber = item.IdNumber;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.Name = "";
                    if (classroomID > 0)
                        tempM101.Name = _classroomRepository.GetClassroomID(classroomID).ClassroomTeacher;

                    var studentTemp = _studentTempRepository.GetStudentTemp(item.SchoolID, user.UserPeriod, item.StudentID);
                    if (studentTemp != null && studentTemp.BankID > 0)
                    {
                        tempM101.Name = _bankRepository.GetBank(studentTemp.BankID).BankGivenCode;
                    }

                    _tempM101Repository.CreateTempM101(tempM101);
                }
            }

            string wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/");
            string selectedLanguage = user.SelectedCulture.Trim();
            ViewBag.StudentID = studentID;
            string url = "";
            if (studentID == 0)
                url = "~/reporting/index/M204StudentCard/" + studentID + "/" + exitID + "?userID=" + user.UserID + "&studentID=" + studentID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&wwwRootPath=" + '"' + wwwRootPath + '"';
            else url = "~/reporting/index/M204StudentCardSingle/" + studentID + "/" + exitID + "?userID=" + user.UserID + "&studentID=" + studentID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&wwwRootPath=" + '"' + wwwRootPath + '"';

            return Redirect(url);
        }
        #endregion

        #region List205
        public async Task<IActionResult> List205(int userID, int studentID, int exitID)
        {
            await Task.Delay(100);
            List<Student> student = new List<Student>();

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            if (school.NewPeriod != user.UserPeriod)
            {
                student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }
            foreach (var item in student)
            {
                if (studentID != 0 && item.StudentID != studentID) continue;

                string classroomName = "";
                int classroomID = 0;

                var isExist = false;
                if (school.NewPeriod == user.UserPeriod)
                {
                    if (item.ClassroomID > 0)
                    {
                        classroomID = item.ClassroomID;
                        classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                    }
                }
                else
                {
                    isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomID > 0)
                {
                    var tempM101 = new TempM101();
                    tempM101.ID = 0;
                    tempM101.UserID = userID;
                    tempM101.SchoolID = item.SchoolID;
                    tempM101.StudentID = item.StudentID;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.StudentSerialNumber = item.StudentSerialNumber;
                    tempM101.ClassroomName = classroomName;
                    tempM101.ClassroomID = classroomID;
                    tempM101.StudentName = item.FirstName + " " + item.LastName;
                    tempM101.IdNumber = item.IdNumber;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.Name = "";
                    if (classroomID > 0)
                        tempM101.Name = _classroomRepository.GetClassroomID(classroomID).ClassroomTeacher;

                    var studentTemp = _studentTempRepository.GetStudentTemp(item.SchoolID, user.UserPeriod, item.StudentID);
                    if (studentTemp != null && studentTemp.BankID > 0)
                    {
                        tempM101.Name = _bankRepository.GetBank(studentTemp.BankID).BankGivenCode;
                    }

                    _tempM101Repository.CreateTempM101(tempM101);
                }
            }
            string wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/");
            string selectedLanguage = user.SelectedCulture.Trim();
            ViewBag.StudentID = studentID;
            string url = "";
            if (studentID == 0)
                url = "~/reporting/index/M205StudentCardInvoice/" + studentID + "/" + exitID + "?userID=" + user.UserID + "&studentID=" + studentID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&wwwRootPath=" + '"' + wwwRootPath + '"';
            else url = "~/reporting/index/M205StudentCardInvoiceSingle/" + studentID + "/" + exitID + "?userID=" + user.UserID + "&studentID=" + studentID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&wwwRootPath=" + '"' + wwwRootPath + '"';

            return Redirect(url);
        }
        #endregion

        #region List207
        public async Task<IActionResult> List207(int userID, int studentID, int exitID)
        {
            await Task.Delay(100);
            List<Student> student = new List<Student>();

            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            if (school.NewPeriod != user.UserPeriod)
            {
                student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }
            foreach (var item in student)
            {
                if (studentID != 0 && item.StudentID != studentID) continue;

                string classroomName = "";
                int classroomID = 0;

                var isExist = false;
                if (school.NewPeriod == user.UserPeriod)
                {
                    if (item.ClassroomID > 0)
                    {
                        classroomID = item.ClassroomID;
                        classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                    }
                }
                else
                {
                    isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomID > 0)
                {
                    var tempM101 = new TempM101();
                    tempM101.ID = 0;
                    tempM101.UserID = userID;
                    tempM101.SchoolID = item.SchoolID;
                    tempM101.StudentID = item.StudentID;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.StudentSerialNumber = item.StudentSerialNumber;
                    tempM101.ClassroomName = classroomName;
                    tempM101.ClassroomID = classroomID;
                    tempM101.StudentName = item.FirstName + " " + item.LastName;
                    tempM101.IdNumber = item.IdNumber;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.Name = "";
                    var studentTemp = _studentTempRepository.GetStudentTemp(item.SchoolID, user.UserPeriod, item.StudentID);
                    if (studentTemp != null)
                    {
                        if (studentTemp.BankID > 0)
                        {
                            tempM101.Name = _bankRepository.GetBank(studentTemp.BankID).BankName;
                            tempM101.BondCity = _bankRepository.GetBank(studentTemp.BankID).BankGivenCode;
                            tempM101.BondTypeTitle = _bankRepository.GetBank(studentTemp.BankID).Iban;
                        }
                    }

                    _tempM101Repository.CreateTempM101(tempM101);
                }
            }
            string wwwRootPath = _hostEnvironment.WebRootPath.Replace("\\", "/");
            string selectedLanguage = user.SelectedCulture.Trim();
            ViewBag.StudentID = studentID;
            string url = "";
            if (studentID == 0)
                url = "~/reporting/index/M207RegistrationRenewalFormInvoice/" + studentID + "/" + exitID + "?userID=" + user.UserID + "&studentID=" + studentID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&wwwRootPath=" + '"' + wwwRootPath + '"';
            else url = "~/reporting/index/M207RegistrationRenewalFormInvoiceSingle/" + studentID + "/" + exitID + "?userID=" + user.UserID + "&studentID=" + studentID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode + "&wwwRootPath=" + '"' + wwwRootPath + '"';
            return Redirect(url);
        }
        #endregion

        #region List210
        public IActionResult List210(int userID, int studentID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                StudentID = studentID,
                ExitID = exitID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = school.SchoolYearStart,
                EndListDate = school.SchoolYearEnd,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }
        public async Task<IActionResult> ListPanelInfo210(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            students = _studentRepository.GetOldStudent(user.SchoolID).ToList();
            //students = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var classroom = new TempM101();
            foreach (var item in students)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var isExist = false;
                DateTime date = Convert.ToDateTime(item.DateOfRegistration);
                if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                    (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                {
                    isExist = true;
                }
                if (isExist)
                {
                    var studentAddress = _studentAddressRepository.GetStudentAddress(item.StudentID);
                    var studentParentAddress = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);
                    classroom.ID = 0;
                    classroom.SchoolID = item.SchoolID;
                    classroom.UserID = user.UserID;
                    classroom.StudentID = item.StudentID;
                    classroom.StudentName = item.FirstName + " " + item.LastName;
                    classroom.GenderTypeCategoryID = item.GenderTypeCategoryID;
                    if (studentAddress != null)
                        classroom.StudentNumber = studentAddress.MobilePhone;
                    classroom.BondDate = item.DateOfBird;
                    classroom.DateOfRegistration = item.FirstDateOfRegistration;

                    classroom.ParentName = item.ParentName;
                    if (studentParentAddress != null)
                    {
                        classroom.ReceiptNo = studentParentAddress.KinshipCategoryID;
                        classroom.AccountReceipt = studentParentAddress.ProfessionCategoryID;
                        classroom.ParentMobilePhone = studentParentAddress.MobilePhone;
                        classroom.HomePhone = studentParentAddress.HomePhone;
                        classroom.WorkPhone = studentParentAddress.WorkPhone;
                        classroom.InWriting = studentParentAddress.EMail;
                    }
                    _tempM101Repository.CreateTempM101(classroom);
                }
            }

            var c = new TempM101Header();
            c.ID = 0;
            c.UserID = user.UserID;
            c.SchoolID = user.SchoolID;
            c.DateFrom = listPanelModel.StartListDate;
            c.DateTo = listPanelModel.EndListDate;
            c.HeaderFee01 = "3";
            c.HeaderFee02 = "4";
            c.HeaderFee03 = "10";

            _tempM101HeaderRepository.CreateTempM101Header(c);

            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "~/reporting/index/M210StudentList/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode + "&schoolID=" + user.SchoolID;

            return Redirect(url);
        }
        #endregion

        #region List301
        public IActionResult List301(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options0 = true,
                List01Options1 = false,
                List01Options2 = false,

                List01Options3 = true,
                List01Options4 = false,

            };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo301(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            if (listPanelModel.List01Options0 == false) feeNew.DateTo = DateTime.Now;

            if (listPanelModel.ListOpt1 == 1)
            {
                feeNew.HeaderFee01 = Resources.Resource.Parent2;
                feeNew.HeaderFee02 = Resources.Resource.Parent2;
            }
            else
            {
                feeNew.HeaderFee01 = Resources.Resource.Student2;
                feeNew.HeaderFee02 = Resources.Resource.Student;
            }

            if (listPanelModel.List01Options0 == true) { feeNew.CompanyName = Resources.Resource.IssuedInvoices; };
            if (listPanelModel.List01Options1 == true)
                if (feeNew.CompanyName == null)
                { feeNew.CompanyName = Resources.Resource.UnissuedInvoices; }
                else { feeNew.CompanyName += " / " + Resources.Resource.UnissuedInvoices; };
            if (listPanelModel.List01Options2 == true)
                if (feeNew.CompanyName == null)
                { feeNew.CompanyName = Resources.Resource.IssuedOtherInvoices; }
                else { feeNew.CompanyName += " / " + Resources.Resource.IssuedOtherInvoices; };

            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentInvoice = new TempM101();
            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var cancelID1 = _parameterRepository.GetParameterCategoryName("Takipte (İptal)").CategoryID;
            var cancelID2 = _parameterRepository.GetParameterCategoryName("İptal").CategoryID;

            foreach (var item in students)
            {
                var isExist = false;
                bool isExist2 = false;
                string classroomName = "";
                int classroomID = 0;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var classroomSo = 0;
                if (classroomID > 0) classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo) { isExist = true; } else { isExist = false; };
                if (listPanelModel.List01Options3 == true && (item.StatuCategoryID == cancelID1 || item.StatuCategoryID == cancelID2)) { isExist = false; };
                if (listPanelModel.List01Options4 == true && (item.StatuCategoryID == cancelID1 || item.StatuCategoryID == cancelID2)) { isExist = true; };

                if (isExist == true)
                {
                    var invoice = _studentInvoiceRepository.GetStudentInvoiceAll(user.SchoolID, user.UserPeriod, item.StudentID).ToList();
                    foreach (var inv in invoice)
                    {
                        DateTime date = Convert.ToDateTime(inv.InvoiceDate);
                        if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                            (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                        {
                            isExist = true;
                        }
                        else { isExist = false; };

                        if (isExist == true)
                        {
                            isExist = false;
                            if (listPanelModel.List01Options0 == true)
                                if (inv.InvoiceStatus == true) { isExist = true; };
                            if (listPanelModel.List01Options1 == true)
                                if (inv.InvoiceStatus == false) { isExist = true; };

                            if (isExist == true)
                            {

                                studentInvoice.ID = 0;
                                studentInvoice.UserID = listPanelModel.UserID;
                                studentInvoice.SchoolID = item.SchoolID;
                                studentInvoice.StudentID = item.StudentID;
                                studentInvoice.StudentNumber = item.StudentNumber;
                                studentInvoice.StudentSerialNumber = item.StudentSerialNumber;
                                studentInvoice.ClassroomID = classroomID;
                                if (listPanelModel.ListOpt1 == 1)
                                {
                                    studentInvoice.StudentName = item.ParentName;
                                    studentInvoice.IdNumber = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID).IdNumber;
                                }
                                else
                                {
                                    studentInvoice.StudentName = item.FirstName + " " + item.LastName;
                                    studentInvoice.IdNumber = item.IdNumber; ;
                                }
                                studentInvoice.StudentNumber = item.StudentNumber;
                                studentInvoice.ReceiptNo = inv.InvoiceSerialNo;
                                studentInvoice.BondDate = inv.InvoiceDate;
                                studentInvoice.DateOfRegistration = inv.InvoiceCutDate;
                                if (inv.DUnitPrice == inv.DAmount) studentInvoice.Fee01 -= inv.DTax;
                                else studentInvoice.Fee01 = inv.DUnitPrice;
                                studentInvoice.Fee02 = inv.DTax;
                                studentInvoice.Fee03 = inv.DAmount;
                                studentInvoice.WriteDate = true;
                                if (inv.InvoiceStatus == false) studentInvoice.WriteDate = false;

                                _tempM101Repository.CreateTempM101(studentInvoice);
                            }
                        }
                    }
                }
            }
            if (listPanelModel.List01Options2 == true)
            {
                var isExist = false;
                var invoice = _studentInvoiceRepository.GetStudentInvoiceAll(user.SchoolID, user.UserPeriod, 0).ToList();
                foreach (var inv in invoice)
                {
                    DateTime dates = Convert.ToDateTime(inv.InvoiceDate);
                    if ((DateTime.Compare(dates, date1) == 0 || DateTime.Compare(dates, date1) > 0) &&
                        (DateTime.Compare(dates, date2) == 0 || DateTime.Compare(dates, date2) < 0))
                    {
                        isExist = true;
                    }
                    else { isExist = false; };

                    if (isExist == true)
                        if (inv.InvoiceStatus == true)
                        {
                            var invoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(inv.StudentInvoiceAddressID);

                            studentInvoice.ID = 0;
                            studentInvoice.UserID = listPanelModel.UserID;
                            studentInvoice.SchoolID = user.SchoolID;
                            studentInvoice.StudentID = 0;
                            studentInvoice.ClassroomID = 0;

                            studentInvoice.StudentName = invoiceAddress.InvoiceTitle;
                            studentInvoice.IdNumber = invoiceAddress.InvoiceTaxNumber;

                            studentInvoice.StudentNumber = "";
                            studentInvoice.ReceiptNo = inv.InvoiceSerialNo;
                            studentInvoice.BondDate = inv.InvoiceDate;
                            studentInvoice.DateOfRegistration = inv.InvoiceCutDate;
                            studentInvoice.Fee01 = inv.DUnitPrice;
                            studentInvoice.Fee02 = inv.DTax;
                            studentInvoice.Fee03 = inv.DAmount;
                            studentInvoice.WriteDate = true;
                            if (inv.InvoiceStatus == false) studentInvoice.WriteDate = false;

                            _tempM101Repository.CreateTempM101(studentInvoice);
                        }
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "";

            url = "~/reporting/index/M301StudentInvoiceList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }

        #endregion

        #region List305
        public IActionResult List305(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options0 = true,
                List01Options1 = false,
                List01Options2 = false,

                List01Options3 = true,
                List01Options4 = false,

            };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo305(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            List<Student> students = new List<Student>();
            if (school.NewPeriod != user.UserPeriod)
            {
                var allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                students = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                students = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            if (listPanelModel.List01Options0 == false) feeNew.DateTo = DateTime.Now;

            var fee = _schoolFeeRepository.GetSchoolFeeAllTrue(user.SchoolID, "L1");
            int inx = 1;
            foreach (var item in fee)
            {
                if (inx == 01) { feeNew.HeaderFee01 = item.Name; }
                if (inx == 02) { feeNew.HeaderFee02 = item.Name; }
                if (inx == 03) { feeNew.HeaderFee03 = item.Name; }
                if (inx == 04) { feeNew.HeaderFee04 = item.Name; }
                if (inx == 05) { feeNew.HeaderFee05 = item.Name; }
                if (inx == 06) { feeNew.HeaderFee06 = item.Name; }
                if (inx == 07) { feeNew.HeaderFee07 = item.Name; }
                if (inx == 08) { feeNew.HeaderFee08 = item.Name; }
                if (inx == 09) { feeNew.HeaderFee09 = item.Name; }
                if (inx == 10) { feeNew.HeaderFee10 = item.Name; }
                inx++;
            }

            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentInvoice = new TempM101();
            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var cancelID1 = _parameterRepository.GetParameterCategoryName("Takipte (İptal)").CategoryID;
            var cancelID2 = _parameterRepository.GetParameterCategoryName("İptal").CategoryID;
            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            foreach (var item in students)
            {
                var isExist = false;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo) { isExist = true; } else { isExist = false; };
                if (listPanelModel.List01Options3 == true && (item.StatuCategoryID == cancelID1 || item.StatuCategoryID == cancelID2)) { isExist = false; };
                if (listPanelModel.List01Options4 == true && (item.StatuCategoryID == cancelID1 || item.StatuCategoryID == cancelID2)) { isExist = true; };

                if (isExist == true)
                {
                    var invoice = _studentInvoiceRepository.GetStudentInvoiceAll(user.SchoolID, user.UserPeriod, item.StudentID).ToList();
                    foreach (var inv in invoice)
                    {
                        DateTime date = Convert.ToDateTime(inv.InvoiceDate);
                        if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                            (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                        {
                            isExist = true;
                        }
                        else { isExist = false; };

                        if (isExist == true)
                        {
                            isExist = false;
                            if (listPanelModel.List01Options0 == true)
                                if (inv.InvoiceStatus == true) { isExist = true; };
                            if (listPanelModel.List01Options1 == true)
                                if (inv.InvoiceStatus == false) { isExist = true; };

                            if (isExist == true)
                            {
                                studentInvoice.ID = 0;
                                studentInvoice.UserID = listPanelModel.UserID;
                                studentInvoice.SchoolID = item.SchoolID;
                                studentInvoice.StudentID = item.StudentID;
                                studentInvoice.StudentNumber = item.StudentNumber;
                                studentInvoice.StudentSerialNumber = item.StudentSerialNumber;
                                studentInvoice.ClassroomID = classroomID;

                                studentInvoice.StudentName = item.FirstName + " " + item.LastName;
                                studentInvoice.IdNumber = item.IdNumber;
                                studentInvoice.ParentName = item.ParentName;
                                studentInvoice.ParentIdNumber = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID).IdNumber;

                                studentInvoice.StudentNumber = item.StudentNumber;
                                studentInvoice.ReceiptNo = inv.InvoiceSerialNo;
                                studentInvoice.BondDate = inv.InvoiceDate;
                                studentInvoice.DateOfRegistration = inv.InvoiceCutDate;
                                studentInvoice.Fee01Collection = inv.DUnitPrice - inv.DTax - inv.DDiscount;
                                studentInvoice.Fee02Collection = inv.DDiscount;
                                studentInvoice.Fee03Collection = inv.DTax; ;
                                studentInvoice.Fee04Collection = inv.DAmount;
                                studentInvoice.WriteDate = true;
                                if (inv.InvoiceStatus == false) studentInvoice.WriteDate = false;

                                studentInvoice.Fee01 = 0;
                                studentInvoice.Fee02 = 0;
                                studentInvoice.Fee03 = 0;
                                studentInvoice.Fee04 = 0;
                                studentInvoice.Fee05 = 0;
                                studentInvoice.Fee06 = 0;
                                studentInvoice.Fee07 = 0;
                                studentInvoice.Fee08 = 0;
                                studentInvoice.Fee09 = 0;
                                studentInvoice.Fee10 = 0;
                                inx = 1;
                                var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetail(item.StudentID, inv.StudentInvoiceID);
                                if (invoiceDetail != null)
                                {
                                    foreach (var d in invoiceDetail)
                                    {
                                        if (d.Amount > 0)
                                        {
                                            if (inx == 01) { studentInvoice.Fee01 = d.UnitPrice - d.Tax; }
                                            if (inx == 02) { studentInvoice.Fee02 = d.UnitPrice - d.Tax; }
                                            if (inx == 03) { studentInvoice.Fee03 = d.UnitPrice - d.Tax; }
                                            if (inx == 04) { studentInvoice.Fee04 = d.UnitPrice - d.Tax; }
                                            if (inx == 05) { studentInvoice.Fee05 = d.UnitPrice - d.Tax; }
                                            if (inx == 06) { studentInvoice.Fee06 = d.UnitPrice - d.Tax; }
                                            if (inx == 07) { studentInvoice.Fee07 = d.UnitPrice - d.Tax; }
                                            if (inx == 08) { studentInvoice.Fee08 = d.UnitPrice - d.Tax; }
                                            if (inx == 09) { studentInvoice.Fee09 = d.UnitPrice - d.Tax; }
                                            if (inx == 10) { studentInvoice.Fee10 = d.UnitPrice - d.Tax; }
                                            inx++;
                                        }
                                    }
                                }
                                _tempM101Repository.CreateTempM101(studentInvoice);
                            }
                        }
                    }
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "";
            url = "~/reporting/index/M305StudentInvoiceDetailedList/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }

        #endregion

        #region List310
        public IActionResult List310(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
            };

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo310(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);

            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
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

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;

            if (listPanelModel.List01Options0 == false) feeNew.DateTo = DateTime.Now;

            if (listPanelModel.ListOpt1 == 1)
            {
                feeNew.HeaderFee01 = Resources.Resource.Parent2;
            }
            else
            {
                feeNew.HeaderFee01 = Resources.Resource.Student;
            }

            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var studentInvoice = new TempM101();
            var startClassroom = "";
            var endClassroom = "";
            var startClassroomSo = 0;
            var endClassroomSo = 9999;
            if (listPanelModel.StartClassroom != 0)
            {
                startClassroom = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).ClassroomName;
                endClassroom = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).ClassroomName;
                startClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.StartClassroom).SortOrder;
                endClassroomSo = _classroomRepository.GetClassroomID(listPanelModel.EndClassroom).SortOrder;
            }

            if (startClassroom == null) startClassroom = "0";
            if (endClassroom == null) endClassroom = "0";

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var cancelID1 = _parameterRepository.GetParameterCategoryName("Takipte (İptal)").CategoryID;
            var cancelID2 = _parameterRepository.GetParameterCategoryName("İptal").CategoryID;

            bool isExist2 = false;
            string classroomName = "";
            int classroomID = 0;

            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var isExist = false;

                if (school.NewPeriod == user.UserPeriod)
                    classroomID = item.ClassroomID;
                else
                {
                    isExist2 = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist2)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist2)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }

                var classroomSo = _classroomRepository.GetClassroomID(classroomID).SortOrder;

                if (classroomSo >= startClassroomSo && classroomSo <= endClassroomSo) { isExist = true; } else { isExist = false; };
                if (item.StatuCategoryID == cancelID1 || item.StatuCategoryID == cancelID2) { isExist = false; };
                if (item.StatuCategoryID == cancelID1 || item.StatuCategoryID == cancelID2) { isExist = true; };

                decimal? totalInvoice = 0;
                decimal? issuedInvoice = 0;
                decimal? unissuedInvoice = 0;
                if (isExist == true)
                {
                    var invoice = _studentInvoiceRepository.GetStudentInvoiceAll(user.SchoolID, user.UserPeriod, item.StudentID).ToList();
                    foreach (var inv in invoice)
                    {
                        totalInvoice += inv.DAmount;
                        if (inv.InvoiceStatus == true) { issuedInvoice += inv.DAmount; };
                        if (inv.InvoiceStatus == false) { unissuedInvoice += inv.DAmount; };
                    }
                }
                if (isExist == true)
                {
                    studentInvoice.ID = 0;
                    studentInvoice.UserID = listPanelModel.UserID;
                    studentInvoice.SchoolID = item.SchoolID;
                    studentInvoice.StudentID = item.StudentID;
                    studentInvoice.StudentNumber = item.StudentNumber;
                    studentInvoice.StudentSerialNumber = item.StudentSerialNumber;
                    studentInvoice.ClassroomID = classroomID;
                    if (listPanelModel.ListOpt1 == 1)
                    {
                        studentInvoice.StudentName = item.ParentName;
                        studentInvoice.IdNumber = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID).IdNumber;
                    }
                    else
                    {
                        studentInvoice.StudentName = item.FirstName + " " + item.LastName;
                        studentInvoice.IdNumber = item.IdNumber; ;
                    }
                    studentInvoice.StudentNumber = item.StudentNumber;

                    studentInvoice.Fee01 = totalInvoice;
                    studentInvoice.Fee02 = issuedInvoice;
                    studentInvoice.Fee03 = unissuedInvoice;
                    if (totalInvoice > 0)
                        _tempM101Repository.CreateTempM101(studentInvoice);
                }

            }

            string selectedLanguage = user.SelectedCulture.Trim();
            string url = "~/reporting/index/M310InvoiceListbyTotalAmounts/0/0?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }

        #endregion

        #region List320
        public async Task<IActionResult> List320(int userID)
        {
            await Task.Delay(100);
            List<Student> student = new List<Student>();
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            if (school.NewPeriod != user.UserPeriod)
            {
                student = _studentRepository.GetStudentAllPeriod(user.SchoolID).ToList();
            }
            else
            {
                student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
            }
            foreach (var item in student)
            {
                string classroomName = "";
                int classroomID = item.ClassroomID;

                var isExist = false;
                if (school.NewPeriod == user.UserPeriod)
                {
                    if (item.ClassroomID > 0)
                    {
                        classroomID = item.ClassroomID;
                        classroomName = _classroomRepository.GetClassroomID(classroomID).ClassroomName;
                    }
                }
                else
                {
                    isExist = _studentPeriodsRepository.ExistStudentPeriods(item.SchoolID, item.StudentID, user.UserPeriod);
                    if (isExist)
                    {
                        classroomName = _studentPeriodsRepository.GetStudentPeriod(item.SchoolID, item.StudentID, user.UserPeriod).ClassroomName;
                        isExist = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                        if (isExist)
                            classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID;
                    }
                }
                if (classroomID > 0)
                {
                    var tempM101 = new TempM101();
                    tempM101.ID = 0;
                    tempM101.UserID = userID;
                    tempM101.SchoolID = item.SchoolID;
                    tempM101.StudentID = item.StudentID;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.StudentSerialNumber = item.StudentSerialNumber;
                    tempM101.ClassroomName = classroomName;
                    tempM101.ClassroomID = classroomID;
                    tempM101.StudentName = item.FirstName + " " + item.LastName;
                    tempM101.IdNumber = item.IdNumber;
                    tempM101.StudentNumber = item.StudentNumber;
                    tempM101.GenderTypeCategoryID = 0;
                    _tempM101Repository.CreateTempM101(tempM101);
                }
            }
            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "~/reporting/index/M320StudentInvoiceList/0/0?userID=" + user.UserID + "&period=" + '"' + user.UserPeriod + '"' + "&language=" + '"' + selectedLanguage + '"' + "&schoolID=" + user.SchoolID + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }
        #endregion

        #region List500
        public IActionResult List500(int userID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                ExitID = exitID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                StartListDate = DateTime.Now,
                EndListDate = DateTime.Now,
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo500(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var dailyHeader = new TempM101Header();
            dailyHeader.ID = 0;
            dailyHeader.UserID = user.UserID;
            dailyHeader.SchoolID = user.SchoolID;
            dailyHeader.DateFrom = listPanelModel.StartListDate;
            dailyHeader.DateTo = listPanelModel.EndListDate;
            _tempM101HeaderRepository.CreateTempM101Header(dailyHeader);

            var daily = new TempM101();
            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var accountCode = "";
            if (listPanelModel.StartClassroom > 0)
                accountCode = _parameterRepository.GetParameter(listPanelModel.StartClassroom).CategoryName;

            decimal? check01 = 0, bond01 = 0, ots01 = 0, mailOrder01 = 0, creditCard01 = 0, kmh01 = 0, cashBank01 = 0, cashCase01 = 0;
            decimal? check02 = 0, bond02 = 0, ots02 = 0, mailOrder02 = 0, creditCard02 = 0, kmh02 = 0, cashBank02 = 0, cashCase02 = 0;
            decimal? check03 = 0, bond03 = 0, ots03 = 0, mailOrder03 = 0, creditCard03 = 0, kmh03 = 0, cashBank03 = 0, cashCase03 = 0;
            decimal? check04 = 0, bond04 = 0, ots04 = 0, mailOrder04 = 0, creditCard04 = 0, kmh04 = 0, cashBank04 = 0, cashCase04 = 0;
            decimal? check05 = 0, bond05 = 0, ots05 = 0, mailOrder05 = 0, creditCard05 = 0, kmh05 = 0, cashBank05 = 0, cashCase05 = 0;
            int length1 = 0, length4 = 0, length5 = 0, length6 = 0, length7 = 0, length8 = 0, length9 = 0, length11 = 0;
            if (school.AccountNoID01 != null) length1 = school.AccountNoID01.Length;
            if (school.AccountNoID04 != null) length4 = school.AccountNoID04.Length;
            if (school.AccountNoID05 != null) length5 = school.AccountNoID05.Length;
            if (school.AccountNoID06 != null) length6 = school.AccountNoID06.Length;
            if (school.AccountNoID07 != null) length7 = school.AccountNoID07.Length;
            if (school.AccountNoID08 != null) length8 = school.AccountNoID08.Length;
            if (school.AccountNoID09 != null) length9 = school.AccountNoID09.Length;
            if (school.AccountNoID11 != null) length11 = school.AccountNoID11.Length;

            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);
            foreach (var item in accounting)
            {
                daily.ID = 0;
                daily.UserID = listPanelModel.UserID;
                daily.SchoolID = user.SchoolID;

                string code1 = "";
                string code4 = "";
                string code5 = "";
                string code6 = "";
                string code7 = "";
                string code8 = "";
                string code9 = "";
                string code11 = "";
                int lenght11 = item.AccountCode.Length;
                int lenght44 = item.AccountCode.Length;
                int lenght55 = item.AccountCode.Length;
                int lenght66 = item.AccountCode.Length;
                int lenght77 = item.AccountCode.Length;
                int lenght88 = item.AccountCode.Length;
                int lenght99 = item.AccountCode.Length;
                int lenght1111 = item.AccountCode.Length;

                if (lenght11 >= length1) code1 = item.AccountCode.Substring(0, length1);
                if (lenght44 >= length4) code4 = item.AccountCode.Substring(0, length4);
                if (lenght55 >= length5) code5 = item.AccountCode.Substring(0, length5);
                if (lenght66 >= length6) code6 = item.AccountCode.Substring(0, length6);
                if (lenght77 >= length7) code7 = item.AccountCode.Substring(0, length7);
                if (lenght88 >= length8) code8 = item.AccountCode.Substring(0, length8);
                if (lenght99 >= length9) code9 = item.AccountCode.Substring(0, length9);
                if (lenght1111 >= length11) code11 = item.AccountCode.Substring(0, length11);

                bool isExist = true;
                if (accountCode != "")
                    if (accountCode == item.CodeTypeName) isExist = true;
                    else isExist = false;

                if (item.AccountDate >= date1 && item.AccountDate <= date2)
                {
                    if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase02 += item.Debt;
                    if (code4 == school.AccountNoID04.Substring(0, length4)) check02 += item.Debt;
                    if (code5 == school.AccountNoID05.Substring(0, length5)) bond02 += item.Debt;
                    if (code6 == school.AccountNoID06.Substring(0, length6)) ots02 += item.Debt;
                    if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank02 += item.Debt;
                    if (code8 == school.AccountNoID08.Substring(0, length8))
                        if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard02 += item.Debt;
                        else mailOrder02 += item.Debt;
                    if (code11 == school.AccountNoID11.Substring(0, length11)) kmh02 += item.Debt;

                    if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase04 += item.Credit;
                    if (code4 == school.AccountNoID04.Substring(0, length4)) check04 += item.Credit;
                    if (code5 == school.AccountNoID05.Substring(0, length5)) bond04 += item.Credit;
                    if (code6 == school.AccountNoID06.Substring(0, length6)) ots04 += item.Credit;
                    if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank04 += item.Credit;
                    if (code8 == school.AccountNoID08.Substring(0, length8))
                        if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard04 += item.Credit;
                        else mailOrder04 += item.Credit;
                    if (code11 == school.AccountNoID11.Substring(0, length11)) kmh04 += item.Credit;

                    if (code1 == school.AccountNoID01.Substring(0, length1) && isExist == true)
                    {
                        daily.Status = null;
                        daily.BondTypeTitle = item.AccountCode;
                        daily.Name = item.AccountCodeName;
                        daily.DateOfRegistration = item.AccountDate;
                        daily.InWriting = item.Explanation;
                        daily.CashPayment = item.Debt;
                        daily.TotalFee = item.Credit;
                        _tempM101Repository.CreateTempM101(daily);
                    }
                }
                else
                {
                    if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase01 += item.Debt - item.Credit;
                    if (code4 == school.AccountNoID04.Substring(0, length4)) check01 += item.Debt - item.Credit;
                    if (code5 == school.AccountNoID05.Substring(0, length5)) bond01 += item.Debt - item.Credit;
                    if (code6 == school.AccountNoID06.Substring(0, length6)) ots01 += item.Debt - item.Credit;
                    if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank01 += item.Debt - item.Credit;
                    if (code8 == school.AccountNoID08.Substring(0, length8))
                        if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard01 += item.Debt - item.Credit;
                        else mailOrder01 += item.Debt - item.Credit;
                    if (code11 == school.AccountNoID11.Substring(0, length11)) kmh01 += item.Debt - item.Credit;
                }

                if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase03 = cashCase01 + cashCase02;
                if (code4 == school.AccountNoID04.Substring(0, length4)) check03 = check01 + check02;
                if (code5 == school.AccountNoID05.Substring(0, length5)) bond03 = bond01 + bond02;
                if (code6 == school.AccountNoID06.Substring(0, length6)) ots03 = ots01 + ots02;
                if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank03 = cashBank01 + cashBank02;
                if (code8 == school.AccountNoID08.Substring(0, length8))
                    if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard03 = creditCard01 + creditCard02;
                    else mailOrder03 = mailOrder01 + mailOrder02;
                if (code11 == school.AccountNoID11.Substring(0, length11)) kmh03 = kmh01 + kmh02;

                if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase05 = cashCase03 - cashCase04;
                if (code4 == school.AccountNoID04.Substring(0, length4)) check05 = check03 - check04;
                if (code5 == school.AccountNoID05.Substring(0, length5)) bond05 = bond03 - bond04;
                if (code6 == school.AccountNoID06.Substring(0, length6)) ots05 = ots03 - ots04;
                if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank05 = cashBank03 - cashBank04;
                if (code8 == school.AccountNoID08.Substring(0, length8))
                    if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard05 = creditCard03 - creditCard04;
                    else mailOrder05 = mailOrder03 - mailOrder04;
                if (code11 == school.AccountNoID11.Substring(0, length11)) kmh05 = kmh03 - kmh04;
            }

            //row1
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferPreviousH;
            daily.Status = "row1";

            daily.Fee01 = check01;
            daily.Fee02 = bond01;
            daily.Fee03 = kmh01;
            daily.Fee04 = ots01;
            daily.Fee05 = mailOrder01;
            daily.Fee06 = creditCard01;
            daily.Fee07 = cashBank01;
            daily.Fee08 = cashCase01;
            _tempM101Repository.CreateTempM101(daily);

            //row2
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TodayH;
            daily.Status = "row2";
            daily.Fee01 = check02;
            daily.Fee02 = bond02;
            daily.Fee03 = kmh02;
            daily.Fee04 = ots02;
            daily.Fee05 = mailOrder02;
            daily.Fee06 = creditCard02;
            daily.Fee07 = cashBank02;
            daily.Fee08 = cashCase02;
            _tempM101Repository.CreateTempM101(daily);

            //row3
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TotalH;
            daily.Status = "row3";
            daily.Fee01 = check03;
            daily.Fee02 = bond03;
            daily.Fee03 = kmh03;
            daily.Fee04 = ots03;
            daily.Fee05 = mailOrder03;
            daily.Fee06 = creditCard03;
            daily.Fee07 = cashBank03;
            daily.Fee08 = cashCase03;
            _tempM101Repository.CreateTempM101(daily);

            //row4
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.ExpenseH;
            daily.Status = "row4";
            daily.Fee01 = check04;
            daily.Fee02 = bond04;
            daily.Fee03 = kmh04;
            daily.Fee04 = ots04;
            daily.Fee05 = mailOrder04;
            daily.Fee06 = creditCard04;
            daily.Fee07 = cashBank04;
            daily.Fee08 = cashCase04;
            _tempM101Repository.CreateTempM101(daily);

            //row5
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferNextH;
            daily.Status = "row5";
            daily.Fee01 = check05;
            daily.Fee02 = bond05;
            daily.Fee03 = kmh05;
            daily.Fee04 = ots05;
            daily.Fee05 = mailOrder05;
            daily.Fee06 = creditCard05;
            daily.Fee07 = cashBank05;
            daily.Fee08 = cashCase05;
            _tempM101Repository.CreateTempM101(daily);

            string selectedLanguage = user.SelectedCulture.Trim();

            var exitID = 0;
            if (listPanelModel.ExitID == 1) exitID = 1;
            string url = "~/reporting/index/M500DailyCashSheet/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);
        }

        public IActionResult AccountCodeRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Girişinde Kullanılan Kod").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);
            return Json(parameter);
        }

        #endregion

        #region List501
        public IActionResult List501(int userID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                ExitID = exitID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                StartListDate = DateTime.Now,
                EndListDate = DateTime.Now,
                CategoryName = categoryName,

            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo501(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var daily = new TempM101();
            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);
            var isExist = false;

            var accountTypeName = "";
            string slip = Resources.Resource.Slip;
            if (listPanelModel.StartClassroom > 0)
                if (user.SelectedCulture.Trim() == "tr-TR")
                    accountTypeName = _parameterRepository.GetParameter(listPanelModel.StartClassroom).CategoryName;
                else
                    accountTypeName = _parameterRepository.GetParameter(listPanelModel.StartClassroom).Language1;

            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);
            decimal total = 0;
            foreach (var item in accounting)
            {
                daily.ID = 0;
                daily.UserID = listPanelModel.UserID;
                daily.SchoolID = user.SchoolID;

                if (listPanelModel.StartClassroom == item.VoucherTypeID && listPanelModel.StartNumber == item.VoucherNo && item.AccountDate >= date1 && item.AccountDate <= date2)
                {
                    isExist = true;
                    daily.BondTypeTitle = item.AccountCode;
                    daily.Name = item.AccountCodeName;
                    daily.DateOfRegistration = item.AccountDate;
                    daily.InWriting = item.Explanation;
                    daily.CashPayment = item.Debt;
                    daily.TotalFee = item.Credit;
                    total += Convert.ToDecimal(item.Debt);

                    _tempM101Repository.CreateTempM101(daily);
                }
            }

            var dailyHeader = new TempM101Header();
            dailyHeader.ID = 0;
            dailyHeader.UserID = user.UserID;
            dailyHeader.SchoolID = user.SchoolID;
            dailyHeader.DateFrom = listPanelModel.StartListDate;
            dailyHeader.DateTo = listPanelModel.EndListDate;
            dailyHeader.StartNumber = listPanelModel.StartNumber;
            MoneyToText text = new MoneyToText();
            dailyHeader.InWriting = text.ConvertToText(total);
            dailyHeader.CompanyName = accountTypeName + " " + slip;
            _tempM101HeaderRepository.CreateTempM101Header(dailyHeader);

            string selectedLanguage = user.SelectedCulture.Trim();
            var exitID = 0;
            if (listPanelModel.ExitID == 3) exitID = 3;
            string url = "~/reporting/index/M501AccountingReceiptPrint/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExist == false)
            {
                int msg = 1;
                url = "/ListPanel/List501?userID=" + user.UserID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }
        public IActionResult AccountTypeRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe Fiş Tipi").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);
            return Json(parameter);
        }
        #endregion

        #region List502
        public IActionResult List502(int userID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                ExitID = exitID,
                Periods = user.UserPeriod,

                SelectedCulture = user.SelectedCulture.Trim(),

                FormOpt = true,
                StartListDate = school.FinancialYearStart,
                EndListDate = school.FinancialYearEnd,
                StartAccount = "100",
                EndAccount = "100",
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }
        public async Task<IActionResult> ListPanelInfo502(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var daily = new TempM101();
            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var accountTypeName = "";
            if (listPanelModel.SelectedSchoolCode > 0)
                accountTypeName = _parameterRepository.GetParameter(listPanelModel.SelectedSchoolCode).CategoryName;

            var isExist = false;
            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);
            var accountCodeName = "";
            if (listPanelModel.SelectedSchoolCode != 0)
                accountCodeName = _parameterRepository.GetParameter(listPanelModel.SelectedSchoolCode).CategoryName;

            int lenght = listPanelModel.StartAccount.Length;
            string codeStart = listPanelModel.StartAccount.Substring(0, lenght);
            string codeEnd = listPanelModel.EndAccount.Substring(0, lenght);

            string codeName = _accountCodesRepository.GetAccountCode(codeStart, user.UserPeriod).AccountCodeName;
            decimal total = 0;
            foreach (var item in accounting)
            {
                if ((listPanelModel.List01Options0 && item.Debt > 0) || (listPanelModel.List01Options1 && item.Credit > 0))
                {
                    daily.ID = 0;
                    daily.UserID = listPanelModel.UserID;
                    daily.SchoolID = user.SchoolID;
                    var isExist0 = true;
                    var isExist1 = false;
                    var isExist2 = false;

                    if (item.AccountDate >= date1 && item.AccountDate <= date2)
                    {
                        string code = "";
                        int itemCodeLenght = item.AccountCode.Length;
                        if (itemCodeLenght >= lenght)
                            code = item.AccountCode.Substring(0, lenght);
                        else isExist0 = false;

                        if (accountCodeName != "" && accountCodeName != item.CodeTypeName)
                        {
                            isExist0 = false;
                        }
                        if (String.Compare(code, codeStart) == 0 || String.Compare(code, codeStart) > 0)
                        {
                            isExist1 = true;
                        }
                        if (String.Compare(code, codeEnd) == 0 || String.Compare(code, codeEnd) < 0)
                        {
                            isExist2 = true;
                        }

                        if (isExist0 == true && isExist1 == true && isExist2 == true)
                        {
                            isExist = true;
                            daily.BondTypeTitle = item.AccountCode;
                            daily.Name = item.AccountCodeName;
                            daily.DateOfRegistration = item.AccountDate;
                            daily.AccountReceipt = item.VoucherNo;
                            daily.InWriting = item.Explanation;
                            daily.CashPayment = item.Debt;
                            daily.TotalFee = item.Credit;
                            total += Convert.ToDecimal(item.Debt);

                            _tempM101Repository.CreateTempM101(daily);
                        }

                    }
                }
            }

            ////// Sub Report /////////////////////////////////////////////////////////////////////////////
            var today = DateTime.Today;
            var accounCode = _accountCodesRepository.GetAccountCodeAllTrue(user.UserPeriod);
            decimal? check01 = 0, bond01 = 0, ots01 = 0, mailOrder01 = 0, creditCard01 = 0, kmh01 = 0, cashBank01 = 0, cashCase01 = 0;
            decimal? check02 = 0, bond02 = 0, ots02 = 0, mailOrder02 = 0, creditCard02 = 0, kmh02 = 0, cashBank02 = 0, cashCase02 = 0;
            decimal? check03 = 0, bond03 = 0, ots03 = 0, mailOrder03 = 0, creditCard03 = 0, kmh03 = 0, cashBank03 = 0, cashCase03 = 0;
            decimal? check04 = 0, bond04 = 0, ots04 = 0, mailOrder04 = 0, creditCard04 = 0, kmh04 = 0, cashBank04 = 0, cashCase04 = 0;
            decimal? check05 = 0, bond05 = 0, ots05 = 0, mailOrder05 = 0, creditCard05 = 0, kmh05 = 0, cashBank05 = 0, cashCase05 = 0;
            int length1 = 0, length4 = 0, length5 = 0, length6 = 0, length7 = 0, length8 = 0, length9 = 0, length11 = 0;
            if (school.AccountNoID01 != null) length1 = school.AccountNoID01.Length;
            if (school.AccountNoID04 != null) length4 = school.AccountNoID04.Length;
            if (school.AccountNoID05 != null) length5 = school.AccountNoID05.Length;
            if (school.AccountNoID06 != null) length6 = school.AccountNoID06.Length;
            if (school.AccountNoID07 != null) length7 = school.AccountNoID07.Length;
            if (school.AccountNoID08 != null) length8 = school.AccountNoID08.Length;
            if (school.AccountNoID09 != null) length9 = school.AccountNoID09.Length;
            if (school.AccountNoID11 != null) length11 = school.AccountNoID11.Length;

            string code1 = school.AccountNoID01.Substring(0, length1);
            string code4 = school.AccountNoID04.Substring(0, length4);
            string code5 = school.AccountNoID05.Substring(0, length5);
            string code6 = school.AccountNoID06.Substring(0, length6);
            string code7 = school.AccountNoID07.Substring(0, length7);
            string code8 = school.AccountNoID08.Substring(0, length8);
            string code9 = school.AccountNoID09.Substring(0, length9);
            string code11 = school.AccountNoID11.Substring(0, length11);

            foreach (var item in accounCode)
            {
                string code = "";
                var isExist3 = false;
                lenght = item.AccountCode.Length;
                if (item.AccountCode == school.AccountNoID09.Substring(0, length9))
                    lenght = 6;
                if (item.AccountCode.Count() > 2)
                    code = item.AccountCode.Substring(0, lenght);
                if (code != item.AccountCode) { isExist3 = false; } else { isExist3 = true; };

                var codeTotal = accounting.Where(p => p.AccountCode.Substring(0, 3) == code && p.AccountDate >= date1 && p.AccountDate <= date2);
                var totalDayIn = accounting.Where(p => p.AccountCode.Substring(0, 3) == code && p.AccountDate >= today && p.AccountDate <= today);
                var totalDayOut = accounting.Where(p => p.AccountCode.Substring(0, 3) == code && p.AccountDate < today || p.AccountDate > today);
                if (lenght != 3)
                {
                    codeTotal = accounting.Where(p => p.AccountCode.Contains(code) && p.AccountDate >= date1 && p.AccountDate <= date2);
                    totalDayIn = accounting.Where(p => p.AccountCode.Contains(code) && p.AccountDate >= today && p.AccountDate <= today);
                    totalDayOut = accounting.Where(p => p.AccountCode.Contains(code) && p.AccountDate < today || p.AccountDate > today);
                }

                foreach (var i in accounting)
                {
                    code1 = "";
                    code4 = "";
                    code5 = "";
                    code6 = "";
                    code7 = "";
                    code8 = "";
                    code9 = "";
                    code11 = "";
                    int lengthx1 = item.AccountCode.Length;
                    int lengthx4 = item.AccountCode.Length;
                    int lengthx5 = item.AccountCode.Length;
                    int lengthx6 = item.AccountCode.Length;
                    int lengthx7 = item.AccountCode.Length;
                    int lengthx8 = item.AccountCode.Length;
                    int lengthx9 = item.AccountCode.Length;
                    int lengthx11 = item.AccountCode.Length;

                    if (lengthx1 >= length1) code1 = item.AccountCode.Substring(0, length1);
                    if (lengthx4 >= length4) code4 = item.AccountCode.Substring(0, length4);
                    if (lengthx5 >= length5) code5 = item.AccountCode.Substring(0, length5);
                    if (lengthx6 >= length6) code6 = item.AccountCode.Substring(0, length6);
                    if (lengthx7 >= length7) code7 = item.AccountCode.Substring(0, length7);
                    if (lengthx8 >= length8) code8 = item.AccountCode.Substring(0, length8);
                    if (lengthx9 >= length9) code9 = item.AccountCode.Substring(0, length9);
                    if (lengthx11 >= length11) code11 = item.AccountCode.Substring(0, length11);

                    if (isExist3 == true)
                    {
                        isExist = true;

                        //SubDetail
                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase02 = totalDayIn.Sum(p => p.Debt);
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check02 = totalDayIn.Sum(p => p.Debt);
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond02 = totalDayIn.Sum(p => p.Debt);
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots02 = totalDayIn.Sum(p => p.Debt);
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank02 = totalDayIn.Sum(p => p.Debt);
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard02 = totalDayIn.Sum(p => p.Debt);
                            else mailOrder02 = totalDayIn.Sum(p => p.Debt);
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh02 = totalDayIn.Sum(p => p.Debt);

                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase04 = totalDayIn.Sum(p => p.Credit);
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check04 = totalDayIn.Sum(p => p.Credit);
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond04 = totalDayIn.Sum(p => p.Credit);
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots04 = totalDayIn.Sum(p => p.Credit);
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank04 = totalDayIn.Sum(p => p.Credit);
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard04 = totalDayIn.Sum(p => p.Credit);
                            else mailOrder04 = totalDayIn.Sum(p => p.Credit);
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh04 = totalDayIn.Sum(p => p.Credit);

                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                            else mailOrder01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);


                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase03 = cashCase01 + cashCase02;
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check03 = check01 + check02;
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond03 = bond01 + bond02;
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots03 = ots01 + ots02;
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank03 = cashBank01 + cashBank02;
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard03 = creditCard01 + creditCard02;
                            else mailOrder03 = mailOrder01 + mailOrder02;
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh03 = kmh01 + kmh02;

                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase05 = cashCase03 - cashCase04;
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check05 = check03 - check04;
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond05 = bond03 - bond04;
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots05 = ots03 - ots04;
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank05 = cashBank03 - cashBank04;
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard05 = creditCard03 - creditCard04;
                            else mailOrder05 = mailOrder03 - mailOrder04;
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh05 = kmh03 - kmh04;

                        break;
                    }
                }
            }
            if (creditCard01 > mailOrder01) creditCard01 -= mailOrder01;
            else mailOrder01 -= creditCard01;
            if (creditCard02 > mailOrder02) creditCard02 -= mailOrder02;
            else mailOrder02 -= creditCard02;
            if (creditCard03 > mailOrder03) creditCard03 -= mailOrder03;
            else mailOrder03 -= creditCard03;
            if (creditCard04 > mailOrder04) creditCard04 -= mailOrder04;
            else mailOrder04 -= creditCard04;
            if (creditCard05 > mailOrder05) creditCard05 -= mailOrder05;
            else mailOrder05 -= creditCard05;

            //row1
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferPreviousH;
            daily.Status = "row1";

            daily.Fee01 = check01;
            daily.Fee02 = bond01;
            daily.Fee03 = kmh01;
            daily.Fee04 = ots01;
            daily.Fee05 = mailOrder01;
            daily.Fee06 = creditCard01;
            daily.Fee07 = cashBank01;
            daily.Fee08 = cashCase01;
            _tempM101Repository.CreateTempM101(daily);

            //row2
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TodayH;
            daily.Status = "row2";
            daily.Fee01 = check02;
            daily.Fee02 = bond02;
            daily.Fee03 = kmh02;
            daily.Fee04 = ots02;
            daily.Fee05 = mailOrder02;
            daily.Fee06 = creditCard02;
            daily.Fee07 = cashBank02;
            daily.Fee08 = cashCase02;
            _tempM101Repository.CreateTempM101(daily);

            //row3
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TotalH;
            daily.Status = "row3";
            daily.Fee01 = check03;
            daily.Fee02 = bond03;
            daily.Fee03 = kmh03;
            daily.Fee04 = ots03;
            daily.Fee05 = mailOrder03;
            daily.Fee06 = creditCard03;
            daily.Fee07 = cashBank03;
            daily.Fee08 = cashCase03;
            _tempM101Repository.CreateTempM101(daily);

            //row4
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.ExpenseH;
            daily.Status = "row4";
            daily.Fee01 = check04;
            daily.Fee02 = bond04;
            daily.Fee03 = kmh04;
            daily.Fee04 = ots04;
            daily.Fee05 = mailOrder04;
            daily.Fee06 = creditCard04;
            daily.Fee07 = cashBank04;
            daily.Fee08 = cashCase04;
            _tempM101Repository.CreateTempM101(daily);

            //row5
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferNextH;
            daily.Status = "row5";
            daily.Fee01 = check05;
            daily.Fee02 = bond05;
            daily.Fee03 = kmh05;
            daily.Fee04 = ots05;
            daily.Fee05 = mailOrder05;
            daily.Fee06 = creditCard05;
            daily.Fee07 = cashBank05;
            daily.Fee08 = cashCase05;
            _tempM101Repository.CreateTempM101(daily);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            var dailyHeader = new TempM101Header();
            dailyHeader.ID = 0;
            dailyHeader.UserID = user.UserID;
            dailyHeader.SchoolID = user.SchoolID;
            dailyHeader.BankName = DateTime.Now.ToString("dd.MM.yyyy");

            dailyHeader.DateFrom = listPanelModel.StartListDate;
            dailyHeader.DateTo = listPanelModel.EndListDate;
            dailyHeader.StartNumber = listPanelModel.StartNumber;
            dailyHeader.HeaderFee01 = codeStart;
            dailyHeader.HeaderFee02 = codeEnd;
            MoneyToText text = new MoneyToText();
            dailyHeader.InWriting = codeName;
            dailyHeader.CompanyName = accountTypeName + " " + Resources.Resource.Slip;
            _tempM101HeaderRepository.CreateTempM101Header(dailyHeader);

            string selectedLanguage = user.SelectedCulture.Trim();
            var exitID = 0;
            if (listPanelModel.ExitID == 3) exitID = 3;
            string url = "~/reporting/index/M502AccountancyList/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExist == false)
            {
                int msg = 1;
                url = "/ListPanel/List502?userID=" + user.UserID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }

        [Route("ListPanel/AccountCodesRead/{period}")]
        public IActionResult AccountCodesRead(string period)
        {
            var accountingCode = _accountCodesRepository.GetAccountCodeAllTrue(period);
            return Json(accountingCode);
        }

        [Route("ListPanel/ProcessTypeDataRead/{period}")]
        public IActionResult ProcessTypeDataRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Muhasebe İşlem Tipi").CategoryID;
            var codeType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(codeType);
        }

        #endregion

        #region List503
        public IActionResult List503(int userID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                ExitID = exitID,
                Periods = user.UserPeriod,

                SelectedCulture = user.SelectedCulture.Trim(),

                StartListDate = school.FinancialYearStart,
                EndListDate = school.FinancialYearEnd,
                StartAccount = "100",
                EndAccount = "100",
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo503(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var daily = new TempM101();
            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var accountTypeName = "";
            if (listPanelModel.SelectedSchoolCode > 0)
                accountTypeName = _parameterRepository.GetParameter(listPanelModel.SelectedSchoolCode).CategoryName;

            var isExist = false;

            var accountCode = _accountCodesRepository.GetAccountCodeAllTrue(user.UserPeriod);
            var accounting = _accountingRepository.GetAccountingAll(user.SchoolID, user.UserPeriod);

            string codeStart = listPanelModel.StartAccount.Substring(0, 3);
            string codeEnd = listPanelModel.EndAccount.Substring(0, 3);
            decimal total = 0;
            string code = "";
            foreach (var item in accountCode)
            {
                daily.ID = 0;
                daily.UserID = listPanelModel.UserID;
                daily.SchoolID = user.SchoolID;

                var isExist1 = false;
                var isExist2 = false;
                var isExist3 = false;

                if (item.AccountCode.Count() > 2)
                    code = item.AccountCode.Substring(0, 3);

                if (code != item.AccountCode) { isExist3 = false; } else { isExist3 = true; };

                if (String.Compare(code, codeStart) == 0 || String.Compare(code, codeStart) > 0)
                {
                    isExist1 = true;
                }
                if (String.Compare(code, codeEnd) == 0 || String.Compare(code, codeEnd) < 0)
                {
                    isExist2 = true;
                }

                if (isExist1 == true && isExist2 == true && isExist3 == true)
                {
                    isExist = true;

                    daily.BondTypeTitle = code;
                    daily.Name = item.AccountCodeName;
                    var codeTotal = accounting.Where(p => p.AccountCode.Substring(0, 3) == code);

                    daily.CashPayment = codeTotal.Sum(p => p.Debt);
                    daily.TotalFee = codeTotal.Sum(p => p.Credit);

                    if (daily.CashPayment >= daily.TotalFee) daily.Fee01 = daily.CashPayment - daily.TotalFee;
                    else daily.Fee02 = daily.TotalFee - daily.CashPayment;

                    if (daily.CashPayment > 0 || daily.TotalFee > 0)
                        _tempM101Repository.CreateTempM101(daily);
                }
            }
            ////// Sub Report /////////////////////////////////////////////////////////////////////////////
            var today = DateTime.Today;
            decimal? check01 = 0, bond01 = 0, ots01 = 0, mailOrder01 = 0, creditCard01 = 0, kmh01 = 0, cashBank01 = 0, cashCase01 = 0;
            decimal? check02 = 0, bond02 = 0, ots02 = 0, mailOrder02 = 0, creditCard02 = 0, kmh02 = 0, cashBank02 = 0, cashCase02 = 0;
            decimal? check03 = 0, bond03 = 0, ots03 = 0, mailOrder03 = 0, creditCard03 = 0, kmh03 = 0, cashBank03 = 0, cashCase03 = 0;
            decimal? check04 = 0, bond04 = 0, ots04 = 0, mailOrder04 = 0, creditCard04 = 0, kmh04 = 0, cashBank04 = 0, cashCase04 = 0;
            decimal? check05 = 0, bond05 = 0, ots05 = 0, mailOrder05 = 0, creditCard05 = 0, kmh05 = 0, cashBank05 = 0, cashCase05 = 0;
            int length1 = 0, length4 = 0, length5 = 0, length6 = 0, length7 = 0, length8 = 0, length9 = 0, length11 = 0;
            string code1 = "", code4 = "", code5 = "", code6 = "", code7 = "", code8 = "", code9 = "", code11 = "";
            if (school.AccountNoID01 != null) length1 = school.AccountNoID01.Length;
            if (school.AccountNoID04 != null) length4 = school.AccountNoID04.Length;
            if (school.AccountNoID05 != null) length5 = school.AccountNoID05.Length;
            if (school.AccountNoID06 != null) length6 = school.AccountNoID06.Length;
            if (school.AccountNoID07 != null) length7 = school.AccountNoID07.Length;
            if (school.AccountNoID08 != null) length8 = school.AccountNoID08.Length;
            if (school.AccountNoID09 != null) length9 = school.AccountNoID09.Length;
            if (school.AccountNoID11 != null) length11 = school.AccountNoID11.Length;

            if (length1 > 0) code1 = school.AccountNoID01.Substring(0, length1);
            if (length4 > 0) code4 = school.AccountNoID04.Substring(0, length4);
            if (length5 > 0) code5 = school.AccountNoID05.Substring(0, length5);
            if (length6 > 0) code6 = school.AccountNoID06.Substring(0, length6);
            if (length7 > 0) code7 = school.AccountNoID07.Substring(0, length7);
            if (length8 > 0) code8 = school.AccountNoID08.Substring(0, length8);
            if (length9 > 0) code9 = school.AccountNoID09.Substring(0, length9);
            if (length11 > 0) code11 = school.AccountNoID11.Substring(0, length11);

            foreach (var item in accountCode)
            {
                code = "";
                var isExist3 = false;
                int lenght = 3;
                if (item.AccountCode == school.AccountNoID09.Substring(0, length9))
                    lenght = 6;
                if (item.AccountCode.Count() > 2)
                    code = item.AccountCode.Substring(0, lenght);
                if (code != item.AccountCode) { isExist3 = false; } else { isExist3 = true; };

                var codeTotal = accounting.Where(p => p.AccountCode.Substring(0, 3) == code && p.AccountDate >= date1 && p.AccountDate <= date2);
                var totalDayIn = accounting.Where(p => p.AccountCode.Substring(0, 3) == code && p.AccountDate >= today && p.AccountDate <= today);
                var totalDayOut = accounting.Where(p => p.AccountCode.Substring(0, 3) == code && p.AccountDate < today || p.AccountDate > today);
                if (lenght != 3)
                {
                    codeTotal = accounting.Where(p => p.AccountCode.Contains(code) && p.AccountDate >= date1 && p.AccountDate <= date2);
                    totalDayIn = accounting.Where(p => p.AccountCode.Contains(code) && p.AccountDate >= today && p.AccountDate <= today);
                    totalDayOut = accounting.Where(p => p.AccountCode.Contains(code) && p.AccountDate < today || p.AccountDate > today);
                }

                foreach (var i in accounting)
                {
                    code1 = "";
                    code4 = "";
                    code5 = "";
                    code6 = "";
                    code7 = "";
                    code8 = "";
                    code9 = "";
                    code11 = "";
                    int lengthx1 = item.AccountCode.Length;
                    int lengthx4 = item.AccountCode.Length;
                    int lengthx5 = item.AccountCode.Length;
                    int lengthx6 = item.AccountCode.Length;
                    int lengthx7 = item.AccountCode.Length;
                    int lengthx8 = item.AccountCode.Length;
                    int lengthx9 = item.AccountCode.Length;
                    int lengthx11 = item.AccountCode.Length;

                    if (lengthx1 >= length1) code1 = item.AccountCode.Substring(0, length1);
                    if (lengthx4 >= length4) code4 = item.AccountCode.Substring(0, length4);
                    if (lengthx5 >= length5) code5 = item.AccountCode.Substring(0, length5);
                    if (lengthx6 >= length6) code6 = item.AccountCode.Substring(0, length6);
                    if (lengthx7 >= length7) code7 = item.AccountCode.Substring(0, length7);
                    if (lengthx8 >= length8) code8 = item.AccountCode.Substring(0, length8);
                    if (lengthx9 >= length9) code9 = item.AccountCode.Substring(0, length9);
                    if (lengthx11 >= length11) code11 = item.AccountCode.Substring(0, length11);

                    if (isExist3 == true)
                    {
                        isExist = true;

                        //SubDetail
                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase02 = totalDayIn.Sum(p => p.Debt);
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check02 = totalDayIn.Sum(p => p.Debt);
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond02 = totalDayIn.Sum(p => p.Debt);
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots02 = totalDayIn.Sum(p => p.Debt);
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank02 = totalDayIn.Sum(p => p.Debt);
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard02 = totalDayIn.Sum(p => p.Debt);
                            else mailOrder02 = totalDayIn.Sum(p => p.Debt);
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh02 = totalDayIn.Sum(p => p.Debt);

                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase04 = totalDayIn.Sum(p => p.Credit);
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check04 = totalDayIn.Sum(p => p.Credit);
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond04 = totalDayIn.Sum(p => p.Credit);
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots04 = totalDayIn.Sum(p => p.Credit);
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank04 = totalDayIn.Sum(p => p.Credit);
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard04 = totalDayIn.Sum(p => p.Credit);
                            else mailOrder04 = totalDayIn.Sum(p => p.Credit);
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh04 = totalDayIn.Sum(p => p.Credit);

                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                            else mailOrder01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh01 = totalDayOut.Sum(p => p.Debt) - totalDayOut.Sum(p => p.Credit);


                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase03 = cashCase01 + cashCase02;
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check03 = check01 + check02;
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond03 = bond01 + bond02;
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots03 = ots01 + ots02;
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank03 = cashBank01 + cashBank02;
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard03 = creditCard01 + creditCard02;
                            else mailOrder03 = mailOrder01 + mailOrder02;
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh03 = kmh01 + kmh02;

                        if (code1 == school.AccountNoID01.Substring(0, length1)) cashCase05 = cashCase03 - cashCase04;
                        if (code4 == school.AccountNoID04.Substring(0, length4)) check05 = check03 - check04;
                        if (code5 == school.AccountNoID05.Substring(0, length5)) bond05 = bond03 - bond04;
                        if (code6 == school.AccountNoID06.Substring(0, length6)) ots05 = ots03 - ots04;
                        if (code7 == school.AccountNoID07.Substring(0, length7)) cashBank05 = cashBank03 - cashBank04;
                        if (code8 == school.AccountNoID08.Substring(0, length8))
                            if (code9 == school.AccountNoID09.Substring(0, length9)) creditCard05 = creditCard03 - creditCard04;
                            else mailOrder05 = mailOrder03 - mailOrder04;
                        if (code11 == school.AccountNoID11.Substring(0, length11)) kmh05 = kmh03 - kmh04;

                        break;
                    }
                }
            }
            if (creditCard01 > mailOrder01) creditCard01 -= mailOrder01;
            else mailOrder01 -= creditCard01;
            if (creditCard02 > mailOrder02) creditCard02 -= mailOrder02;
            else mailOrder02 -= creditCard02;
            if (creditCard03 > mailOrder03) creditCard03 -= mailOrder03;
            else mailOrder03 -= creditCard03;
            if (creditCard04 > mailOrder04) creditCard04 -= mailOrder04;
            else mailOrder04 -= creditCard04;
            if (creditCard05 > mailOrder05) creditCard05 -= mailOrder05;
            else mailOrder05 -= creditCard05;

            //row1
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferPreviousH;
            daily.Status = "row1";

            daily.Fee01 = check01;
            daily.Fee02 = bond01;
            daily.Fee03 = kmh01;
            daily.Fee04 = ots01;
            daily.Fee05 = mailOrder01;
            daily.Fee06 = creditCard01;
            daily.Fee07 = cashBank01;
            daily.Fee08 = cashCase01;
            _tempM101Repository.CreateTempM101(daily);

            //row2
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TodayH;
            daily.Status = "row2";
            daily.Fee01 = check02;
            daily.Fee02 = bond02;
            daily.Fee03 = kmh02;
            daily.Fee04 = ots02;
            daily.Fee05 = mailOrder02;
            daily.Fee06 = creditCard02;
            daily.Fee07 = cashBank02;
            daily.Fee08 = cashCase02;
            _tempM101Repository.CreateTempM101(daily);

            //row3
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TotalH;
            daily.Status = "row3";
            daily.Fee01 = check03;
            daily.Fee02 = bond03;
            daily.Fee03 = kmh03;
            daily.Fee04 = ots03;
            daily.Fee05 = mailOrder03;
            daily.Fee06 = creditCard03;
            daily.Fee07 = cashBank03;
            daily.Fee08 = cashCase03;
            _tempM101Repository.CreateTempM101(daily);

            //row4
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.ExpenseH;
            daily.Status = "row4";
            daily.Fee01 = check04;
            daily.Fee02 = bond04;
            daily.Fee03 = kmh04;
            daily.Fee04 = ots04;
            daily.Fee05 = mailOrder04;
            daily.Fee06 = creditCard04;
            daily.Fee07 = cashBank04;
            daily.Fee08 = cashCase04;
            _tempM101Repository.CreateTempM101(daily);

            //row5
            daily.ID = 0;
            daily.UserID = listPanelModel.UserID;
            daily.SchoolID = user.SchoolID;
            daily.TypeCategoryName = Resources.Resource.TransferNextH;
            daily.Status = "row5";
            daily.Fee01 = check05;
            daily.Fee02 = bond05;
            daily.Fee03 = kmh05;
            daily.Fee04 = ots05;
            daily.Fee05 = mailOrder05;
            daily.Fee06 = creditCard05;
            daily.Fee07 = cashBank05;
            daily.Fee08 = cashCase05;
            _tempM101Repository.CreateTempM101(daily);

            ///////////////////////////////////////////////////////////////////////////////////////////////

            var dailyHeader = new TempM101Header();
            dailyHeader.ID = 0;
            dailyHeader.UserID = user.UserID;
            dailyHeader.SchoolID = user.SchoolID;
            dailyHeader.BankName = DateTime.Now.ToString("dd.MM.yyyy");

            dailyHeader.DateFrom = listPanelModel.StartListDate;
            dailyHeader.DateTo = listPanelModel.EndListDate;
            dailyHeader.StartNumber = listPanelModel.StartNumber;
            dailyHeader.HeaderFee01 = codeStart;
            dailyHeader.HeaderFee02 = codeEnd;
            MoneyToText text = new MoneyToText();
            dailyHeader.InWriting = text.ConvertToText(total);
            dailyHeader.CompanyName = accountTypeName + " " + Resources.Resource.Slip;
            _tempM101HeaderRepository.CreateTempM101Header(dailyHeader);

            string selectedLanguage = user.SelectedCulture.Trim();
            var exitID = 0;
            if (listPanelModel.ExitID == 3) exitID = 3;
            string url = "~/reporting/index/M503TrialBalance/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExist == false)
            {
                int msg = 1;
                url = "/ListPanel/List503?userID=" + user.UserID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }

        #endregion

        #region List504
        public IActionResult List504(int userID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            string categoryName = "name";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                ExitID = exitID,
                Periods = user.UserPeriod,

                SelectedCulture = user.SelectedCulture.Trim(),
                StartAccount = "100",
                EndAccount = "991",
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo504(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var daily = new TempM101();
            var accountTypeName = "";
            if (listPanelModel.SelectedSchoolCode > 0)
                accountTypeName = _parameterRepository.GetParameter(listPanelModel.SelectedSchoolCode).CategoryName;

            var isExist = false;
            var accounCode = _accountCodesRepository.GetAccountCodeAllTrue(user.UserPeriod);

            string codeStart = listPanelModel.StartAccount.Substring(0, 3);
            string codeEnd = listPanelModel.EndAccount.Substring(0, 3);
            decimal total = 0;

            foreach (var item in accounCode)
            {
                daily.ID = 0;
                daily.UserID = listPanelModel.UserID;
                daily.SchoolID = user.SchoolID;

                var isExist1 = false;
                var isExist2 = false;

                if (String.Compare(item.AccountCode, codeStart) == 0 || String.Compare(item.AccountCode, codeStart) > 0)
                {
                    isExist1 = true;
                }
                if (String.Compare(item.AccountCode, codeEnd) == 0 || String.Compare(item.AccountCode, codeEnd) < 0)
                {
                    isExist2 = true;
                }

                if (isExist1 == true && isExist2 == true)
                {
                    isExist = true;

                    daily.BondTypeTitle = item.AccountCode;
                    daily.Name = item.AccountCodeName;
                    _tempM101Repository.CreateTempM101(daily);
                }
            }

            var dailyHeader = new TempM101Header();
            dailyHeader.ID = 0;
            dailyHeader.UserID = user.UserID;
            dailyHeader.SchoolID = user.SchoolID;
            dailyHeader.BankName = DateTime.Now.ToString("dd.MM.yyyy");

            dailyHeader.DateFrom = listPanelModel.StartListDate;
            dailyHeader.DateTo = listPanelModel.EndListDate;
            dailyHeader.StartNumber = listPanelModel.StartNumber;
            dailyHeader.HeaderFee01 = codeStart;
            dailyHeader.HeaderFee02 = codeEnd;
            MoneyToText text = new MoneyToText();
            dailyHeader.InWriting = text.ConvertToText(total);
            dailyHeader.CompanyName = accountTypeName + " " + Resources.Resource.Slip;
            _tempM101HeaderRepository.CreateTempM101Header(dailyHeader);

            string selectedLanguage = user.SelectedCulture.Trim();
            var exitID = 0;
            if (listPanelModel.ExitID == 3) exitID = 3;
            string url = "~/reporting/index/M504AccoundPlanList/" + listPanelModel.StudentID + "/" + exitID + "?schoolID=" + user.SchoolID + "&userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExist == false)
            {
                int msg = 1;
                url = "/ListPanel/List504?userID=" + user.UserID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }

        #endregion

        #region List505
        public IActionResult List505(int userID, int msg, int exitID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string categoryName = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US") categoryName = "language1";
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                ExitID = exitID,
                Periods = user.UserPeriod,

                SelectedCulture = user.SelectedCulture.Trim(),

                FormOpt = true,
                StartListDate = school.FinancialYearStart,
                EndListDate = school.FinancialYearEnd,
                StartAccount = "",
                EndAccount = "",
                CategoryName = categoryName,
            };
            ViewBag.IsSuccess = false;
            if (msg > 0) { ViewBag.IsSuccess = true; };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo505(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.SelectedCulture.Trim());
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            var accountTypeName = "";
            if (listPanelModel.SelectedSchoolCode > 0)
                accountTypeName = _parameterRepository.GetParameter(listPanelModel.SelectedSchoolCode).CategoryName;

            var isExist = false;
            var accounting = _accountingRepository.GetAccountingCode(user.SchoolID, user.UserPeriod, listPanelModel.StartAccount);
            var accountCodeName = "";
            if (listPanelModel.SelectedSchoolCode != 0)
                accountCodeName = _parameterRepository.GetParameter(listPanelModel.SelectedSchoolCode).CategoryName;

            string codeStart = listPanelModel.StartAccount.Substring(0, 3);

            string codeName = _accountCodesRepository.GetAccountCode(codeStart, user.UserPeriod).AccountCodeName;
            decimal totalDebt1 = 0, totalCredit1 = 0, totalDebt2 = 0, totalCredit2 = 0;
            foreach (var item in accounting)
            {
                var daily = new TempM101();
                daily.ID = 0;
                daily.UserID = listPanelModel.UserID;
                daily.SchoolID = user.SchoolID;
                var isExist0 = true;
                var isExist1 = false;

                string code = item.AccountCode.Substring(0, 3);

                if (item.AccountDate >= date1 && item.AccountDate <= date2)
                {
                    if (accountCodeName != "" && accountCodeName != item.ProcessTypeName)
                    {
                        isExist0 = false;
                    }
                    if (String.Compare(code, codeStart) == 0 || String.Compare(code, codeStart) > 0)
                    {
                        isExist1 = true;
                    }

                    if (isExist0 == true && isExist1 == true)
                    {
                        isExist = true;
                        daily.BondTypeTitle = item.AccountCode;
                        daily.Name = item.ProcessTypeName;
                        daily.IdNumber = item.DocumentNumber;
                        daily.DateOfRegistration = item.DocumentDate;
                        daily.TypeAndNo = item.TaxNoOrId;

                        daily.DateOfRegistration = item.AccountDate;
                        daily.AccountReceipt = item.VoucherNo;
                        daily.InWriting = item.Explanation;
                        daily.CashPayment = item.Debt;
                        daily.TotalFee = item.Credit;

                        totalDebt2 += (decimal)item.Debt;
                        totalCredit2 += (decimal)item.Credit;
                        daily.Fee01Balance = totalDebt2 - totalCredit2;

                        _tempM101Repository.CreateTempM101(daily);
                    }
                }
                else
                {
                    totalDebt1 += (decimal)item.Debt;
                    totalCredit1 += (decimal)item.Credit;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            var dailyHeader = new TempM101Header();

            dailyHeader.Total = totalDebt1;
            dailyHeader.Collection = totalCredit1;

            dailyHeader.ID = 0;
            dailyHeader.UserID = user.UserID;
            dailyHeader.SchoolID = user.SchoolID;
            dailyHeader.BankName = DateTime.Now.ToString("dd.MM.yyyy");

            dailyHeader.DateFrom = listPanelModel.StartListDate;
            dailyHeader.DateTo = listPanelModel.EndListDate;
            dailyHeader.StartNumber = listPanelModel.StartNumber;
            dailyHeader.HeaderFee01 = codeStart;
            MoneyToText text = new MoneyToText();
            dailyHeader.InWriting = codeName;
            dailyHeader.CompanyName = accountTypeName + " " + Resources.Resource.Slip;
            _tempM101HeaderRepository.CreateTempM101Header(dailyHeader);

            string selectedLanguage = user.SelectedCulture.Trim();
            var exitID = 0;
            if (listPanelModel.ExitID == 7) exitID = 7;
            string url = "~/reporting/index/M505CurrentCardList/" + listPanelModel.StudentID + "/" + exitID + "?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            if (isExist == false)
            {
                int msg = 1;
                url = "/ListPanel/List505?userID=" + user.UserID + "&msg=" + msg + "&exitID=" + exitID;
            }
            return Redirect(url);
        }

        [Route("ListPanel/CurrentAccountCodesRead/{period}")]
        public IActionResult CurrentAccountCodesRead(string period)
        {
            var accountingCode = _accountCodesRepository.GetCurrentCard(period);
            return Json(accountingCode);
        }
        #endregion

        #region List506
        public IActionResult List506(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),

                StartClassroom = 1,
                EndClassroom = 1,
                StartListDate = DateTime.Now.AddMonths(-1),
                EndListDate = DateTime.Now,
                List01Options0 = false,
                List01Options1 = false,
                List01Options2 = false,
            };

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(listPanelModel);
        }

        public async Task<IActionResult> ListPanelInfo506(ListPanelModel listPanelModel)
        {
            await Task.Delay(100);
            List<Student> students = new List<Student>();
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var schools = _schoolInfoRepository.GetSchoolInfoAllTrue();

            //Delete SQL
            MyAppSettingControl appSettings = new MyAppSettingControl();
            appSettings.GetAppSettingAndSqlDeleteFiles("DevConnection", user.SelectedSchoolCode, user.SchoolID, user.UserID);

            var feeNew = new TempM101Header();
            feeNew.ID = 0;
            feeNew.UserID = user.UserID;
            feeNew.SchoolID = user.SchoolID;
            feeNew.DateFrom = listPanelModel.StartListDate;
            feeNew.DateTo = listPanelModel.EndListDate;
            if (listPanelModel.List01Options0 == false) feeNew.DateTo = DateTime.Now;
            _tempM101HeaderRepository.CreateTempM101Header(feeNew);

            var currentCard = new TempM101();
            decimal amount = 0, collection = 0, balance = 0, delayedPayment = 0;

            DateTime date1 = Convert.ToDateTime(listPanelModel.StartListDate);
            DateTime date2 = Convert.ToDateTime(listPanelModel.EndListDate);

            foreach (var sc in schools)
            {
                if (sc.IsSelect == true)
                {
                    var accountCodes = _accountCodesRepository.GetCurrentCard(user.UserPeriod);

                    foreach (var item in accountCodes)
                    {
                        amount = 0; collection = 0; balance = 0; delayedPayment = 0;
                        var isExist = true;

                        var accounting = _accountingRepository.GetAccountingCode(user.SchoolID, user.UserPeriod, item.AccountCode);

                        foreach (var acc in accounting)
                        {
                            if (listPanelModel.List01Options0 == true)
                            {
                                DateTime date = Convert.ToDateTime(acc.AccountDate);
                                if ((DateTime.Compare(date, date1) == 0 || DateTime.Compare(date, date1) > 0) &&
                                    (DateTime.Compare(date, date2) == 0 || DateTime.Compare(date, date2) < 0))
                                {
                                    isExist = true;
                                }
                                else { isExist = false; };
                            }

                            if (isExist == true)
                            {
                                amount += (decimal)acc.Debt;
                                collection += (decimal)acc.Credit;
                                balance = amount - collection;

                                if (listPanelModel.List01Options1 == true)
                                    if (balance == 0) isExist = true;
                                    else isExist = false;
                                if (listPanelModel.List01Options1 == false)
                                    if (balance == 0) isExist = false;

                            }
                        }

                        currentCard.ID = 0;
                        currentCard.UserID = listPanelModel.UserID;
                        currentCard.SchoolID = user.SchoolID;
                        currentCard.SchoolNumber = sc.SchoolID;

                        currentCard.TypeAndNo = item.AccountCode;
                        currentCard.Name = item.AccountCodeName;

                        var detail = _accountCodesDetailRepository.GetAccountCodesDetailID1(item.AccountCodeID);

                        currentCard.ParentMobilePhone = "";
                        currentCard.WorkPhone = "";
                        currentCard.ParentName = "";
                        if (detail != null)
                        {
                            currentCard.ParentMobilePhone = detail.Mobile;
                            currentCard.WorkPhone = detail.Phone1;
                            currentCard.ParentName = detail.AuthorizedPersonName;
                        }
                        currentCard.Fee01 = amount;
                        currentCard.Fee02 = collection;
                        currentCard.Fee03 = delayedPayment;
                        currentCard.Fee04 = balance;

                        _tempM101Repository.CreateTempM101(currentCard);
                    }
                }
            }

            string selectedLanguage = user.SelectedCulture.Trim();

            string url = "~/reporting/index/M506CurrentCardTotalDebtList/0/9?userID=" + user.UserID + "&language=" + '"' + selectedLanguage + '"' + "&schoolCode=" + user.SelectedSchoolCode;

            return Redirect(url);

        }
        #endregion

        #region List600
        public IActionResult List600(int userID, bool isSuccess, string file)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Veritabanı Yedekleme").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var strDate = DateTime.Now.ToString("_yyyyMMdd");

            var listPanelModel = new ListPanelModel
            {
                IsPermission = isPermission,

                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = user.SelectedCulture.Trim(),
                StartListDate = DateTime.Now,
                FormTitle = user.SelectedSchoolCode.ToString(),
                Title = user.SelectedSchoolCode.ToString() + strDate,
            };
            ViewBag.IsSuccess = false;

            string userDate = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["Date"] = userDate;
            TempData["Period"] = user.UserPeriod;
            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);
            if (isSuccess)
            {
                ViewBag.IsSuccess = true;
            }
            ViewBag.BackupFile = file;

            return View(listPanelModel);
        }

        public IActionResult ListPanelInfo600(ListPanelModel listPanelModel)
        {
            var user = _usersRepository.GetUser(listPanelModel.UserID);

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            //string dbServer = connectionString.
            string dbName = user.SelectedSchoolCode.ToString();
            string dbPwd = connectionString.Password;
            string dbUser = connectionString.UserID;
            string dbServer = connectionString.DataSource;

            string backupPath = Path.Combine(_hostEnvironment.WebRootPath, "BACKUP");
            if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

            Directory.SetCurrentDirectory(backupPath);

            string dbBackupPath = backupPath;
            if (!Directory.Exists(dbBackupPath)) Directory.CreateDirectory(dbBackupPath);

            string file = listPanelModel.Title.Trim() + ".bak";

            string dbBackupFileName = dbBackupPath + "\\" + file;
            string zipFile = "";
            string zipPath = "";
            if (DBExist(dbServer, dbUser, dbPwd, dbName))
            {
                if (!Directory.Exists(dbBackupPath)) Directory.CreateDirectory(dbBackupPath);  // Backup directory yok ise yaratilir
                if (System.IO.File.Exists(dbBackupFileName)) System.IO.File.Delete(dbBackupFileName);              // Daha once ayni gun backup alinmis ise silinir

                SqlConnection con = new SqlConnection($@"Data Source={dbServer};User ID={dbUser};Password={dbPwd};Connect Timeout=5000000; Database={dbName};");
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    SqlCommand cmd1 = new SqlCommand("USE [" + dbName + "];", con);
                    SqlCommand cmd2 = new SqlCommand("BACKUP DATABASE [" + dbName + "] TO DISK = '" + dbBackupFileName + "' WITH FORMAT ,MEDIANAME = 'Z_SQLServerBackups' , NAME = 'Full Backup of [" + dbName + "]';", con);
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    con.Close();

                    zipPath = dbBackupFileName.Replace(".bak", ".zip");
                    using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                    {
                        archive.CreateEntryFromFile(dbBackupFileName, file);
                    }
                    zipFile = file.Replace(".bak", ".zip");
                }
                con.Dispose();
            }

            ViewBag.IsSuccess = true;
            var isSuccess = true;
            string url = "/ListPanel/List600?userID=" + user.UserID + "&isSuccess=" + isSuccess + "&file=" + zipFile;

            return Redirect(url);
        }
        public void ListPanelInfo600SqlDelete(string zipDeleteFile)
        {
            string backupPath = Path.Combine(_hostEnvironment.WebRootPath, "BACKUP");
            string zipDeletePath = backupPath + "\\" + zipDeleteFile;
            zipDeleteFile = zipDeleteFile + ".zip";
            string bakDeletePath = backupPath + "\\" + zipDeleteFile.Replace(".zip", ".bak");
            if (System.IO.File.Exists(bakDeletePath)) System.IO.File.Delete(bakDeletePath);
            if (System.IO.File.Exists(zipDeletePath)) System.IO.File.Delete(zipDeletePath);
        }

        private bool DBExist(string dbServer, string dbUser, string dbPwd, string dbName)
        {
            bool dbexist = false;
            try
            {
                string sqlQuery = "SELECT name, database_id, create_date FROM sys.databases; ";
                using (SqlConnection myConnection = new SqlConnection($@"Data Source={dbServer};User ID={dbUser};Password={dbPwd};Connect Timeout=10; Database=master;"))
                {
                    SqlCommand oCmd = new SqlCommand(sqlQuery, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            if (dbName.ToLower() == oReader.GetValue(0).ToString().ToLower())
                            {
                                dbexist = true;
                                break;
                            }
                        }
                        myConnection.Close();
                    }
                }
                return dbexist;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        internal class HashGenerator
        {
            System.Security.Cryptography.MD5 Md5 = System.Security.Cryptography.MD5.Create();

            public string GetMD5Hash(byte[] filebyteArray)
            {
                if (filebyteArray != null && filebyteArray.Length > 0)
                {
                    byte[] hashData = Md5.ComputeHash(filebyteArray, 0, filebyteArray.Length);
                    return HexBytesToString(hashData);
                }

                return null;
            }

            public string HexBytesToString(byte[] bytes)
            {
                string result = "";
                foreach (byte b in bytes) result += b.ToString("x2");
                return result;
            }
        }
    }

}