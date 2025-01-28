using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Uyumsoft;

namespace ESCHOOL.Controllers
{
    public class M999ImportToWeb : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IPSerialNumberRepository _pSerialNumberRepository;
        IUsersRepository _usersRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IStudentNoteRepository _studentNoteRepository;
        IClassroomRepository _classroomRepository;
        IDiscountTableRepository _discountTableRepository;
        ISchoolBusServicesRepository _schoolBusServicesRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentDiscountRepository _studentDiscountRepository;
        IParameterRepository _parameterRepository;
        IBankRepository _bankRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentInstallmentPaymentRepository _studentInstallmentPaymentRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        ITempM101HeaderRepository _tempM101HeaderRepository;
        ITempM101Repository _tempM101Repository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        IStudentTempRepository _studentTempRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInvoiceDetailRepository _studentInvoiceDetailRepository;
        IWebHostEnvironment _hostEnvironment;
        public M999ImportToWeb(
            ISchoolInfoRepository schoolInfoRepository,
            IPSerialNumberRepository pSerialNumberRepository,
            IUsersRepository usersRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,
            IStudentNoteRepository studentNoteRepository,
            IClassroomRepository classroomRepository,
            IDiscountTableRepository discountTableRepository,
            ISchoolBusServicesRepository schoolBusServicesRepository,
            ISchoolFeeRepository schoolFeeRepository,
            ISchoolFeeTableRepository schoolFeeTableRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDiscountRepository studentDiscountRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            IStudentInstallmentPaymentRepository studentInstallmentPaymentRepository,
            IStudentPaymentRepository studentPaymentRepository,
            IParameterRepository parameterRepository,
            IAccountingRepository accountingRepository,
            IAccountCodesRepository accountCodesRepository,
            IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
            IStudentInvoiceRepository studentInvoiceRepository,
            IStudentInvoiceDetailRepository studentInvoiceDetailRepository,
            IBankRepository bankRepository,
            IStudentTempRepository studentTempRepository,
            ITempM101HeaderRepository tempM101HeaderRepository,
            ITempM101Repository tempM101Repository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _usersRepository = usersRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _studentNoteRepository = studentNoteRepository;
            _classroomRepository = classroomRepository;
            _discountTableRepository = discountTableRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentDiscountRepository = studentDiscountRepository;
            _parameterRepository = parameterRepository;
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountCodesRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInvoiceDetailRepository = studentInvoiceDetailRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _studentInstallmentPaymentRepository = studentInstallmentPaymentRepository;
            _studentPaymentRepository = studentPaymentRepository;
            _bankRepository = bankRepository;
            _studentTempRepository = studentTempRepository;
            _tempM101Repository = tempM101Repository;
            _tempM101HeaderRepository = tempM101HeaderRepository;

            _hostEnvironment = hostEnvironment;
        }

        #region ImportToWeb
        public IActionResult ImportToWeb(int userID, int msg)
        {
            var user = _usersRepository.GetUser(userID);

            DecadeController periodList = new DecadeController();
            var period = periodList.Period();

            int year = (DateTime.Now.Year + 1);
            DateTime sYear = new DateTime(year, 1, 1);
            string newperiod = (sYear.ToString("yyyy") + "-" + (sYear.AddYears(1).ToString("yyyy")));
            var listPanelModel = new ListPanelModel
            {
                SchoolID = user.SchoolID,
                UserID = user.UserID,
                SelectedCulture = "tr-TR",
                Periods = user.UserPeriod,
                StartNumber = 3,
                NewPeriod = newperiod,
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

        public IActionResult ImportToWebInfo(ListPanelModel listPanelModel)
        {
            var user = _usersRepository.GetUser(listPanelModel.UserID);
            bool isExistSub = false;

            string filePath = null;
            string[] words = null;
            string[] readText = null;
            string file = null;
            string periodTmp = null;
            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "D:");
            int inx = 0;
            bool isExist = false;
            #region SchoolInfo

            for (int i = 1; i <= listPanelModel.StartNumber; i++)
            {
                file = "SchoolInfo" + i.ToString("D2") + ".CSV";
                SchoolInfo schoolInfo = new SchoolInfo();
                PSerialNumber serialNumber = new PSerialNumber();
                SchoolFee schoolFee = new SchoolFee();

                isExist = false;
                filePath = Path.Combine(projectDir, file);
                if (System.IO.File.Exists(filePath)) isExist = true;
                if (isExist)
                {
                    readText = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("iso-8859-9"));

                    foreach (var csv in readText)
                    {
                        words = csv.Split(';');

                        schoolInfo.CompanyName = words[001].Trim();
                        schoolInfo.CompanyAddress = words[002].Trim() + " " + words[003].Trim() + " " + words[004].Trim() + " " + words[005].Trim() + " " + words[006].Trim();
                        schoolInfo.CompanyShortCode = words[007].Trim();
                        schoolInfo.CompanyShortName = words[008].Trim();
                        schoolInfo.CompanyNameForBond = words[001].Trim();
                        schoolInfo.LogoName = "default.png";

                        if (words[009].Trim() == "İÇEL" || words[009].Trim() == "İçel") words[009] = "MERSİN";
                        if (words[009].Trim() == "AFYON" || words[009].Trim() == "Afyon") words[009] = "AFYONKARAHİSAR";
                        if (words[009].Trim() == "İZMİT" || words[009].Trim() == "İzmit") words[009] = "KOCAELİ";
                        if (words[009].Trim() == "ADAPAZARI" || words[009].Trim() == "Adapazarı") words[009] = "SAKARYA";
                        schoolInfo.CityNameForBondID = 0;
                        isExist = _parameterRepository.ExistCategoryName(words[009].Trim());
                        if (isExist && words[009].Trim() != "")
                            schoolInfo.CityNameForBondID = _parameterRepository.GetParameterCategoryName(words[009].Trim()).CategoryID;

                        schoolInfo.AuthorizedPersonName1 = words[010].Trim();
                        schoolInfo.AuthorizedPersonName2 = words[011].Trim();
                        schoolInfo.AuthorizedPersonName3 = words[012].Trim();
                        schoolInfo.CopiesOfForm = Convert.ToInt16(words[013]);

                        if (words[014].Trim() != " ") words[014] = "0";
                        serialNumber.CollectionNo = Convert.ToInt32(words[014]);
                        serialNumber.CollectionReceiptNo = Convert.ToInt32(words[015]);
                        schoolInfo.PrintQuantity = Convert.ToInt16(words[016]);
                        serialNumber.PaymentNo = Convert.ToInt32(words[017]);
                        serialNumber.PaymentReceiptNo = Convert.ToInt32(words[018]);

                        string str = DateControl(1, words[019]); if (str == null) schoolInfo.FinancialYearStart = null; else schoolInfo.FinancialYearStart = DateTime.Parse(str);
                        str = DateControl(2, words[020]); if (str == null) schoolInfo.FinancialYearEnd = null; else schoolInfo.FinancialYearEnd = DateTime.Parse(str);

                        serialNumber.AccountSerialNo = Convert.ToInt32(words[021]);
                        schoolInfo.AccountNoID01 = words[022].Trim();
                        schoolInfo.AccountNoID02 = words[023].Trim();
                        schoolInfo.AccountNoID03 = words[024].Trim();
                        schoolInfo.AccountNoID04 = words[025].Trim();
                        schoolInfo.AccountNoID05 = words[026].Trim();
                        schoolInfo.AccountNoID06 = words[027].Trim();
                        schoolInfo.AccountNoID07 = words[028].Trim();
                        schoolInfo.AccountNoID08 = words[029].Trim();
                        schoolInfo.AccountNoID09 = words[030].Trim();
                        schoolInfo.AccountNoID10 = words[031].Trim();
                        schoolInfo.AccountNoID11 = words[032].Trim();

                        schoolInfo.RefundDebtAccountID = words[033].Trim();
                        schoolInfo.RefundAccountNoID1 = words[034].Trim();
                        schoolInfo.RefundAccountNoID2 = words[035].Trim();
                        schoolInfo.RefundAccountNoID3 = words[036].Trim();

                        schoolInfo.EmailSMTPUserName = words[057].Trim();
                        schoolInfo.EmailSMTPPassword = words[058].Trim();
                        schoolInfo.EmailSMTPPort = words[059].Trim();
                        schoolInfo.EmailSMTPSsl = false;
                        if (Convert.ToInt32(words[060]) == 1) schoolInfo.EmailSMTPSsl = true;
                        schoolInfo.EmailSMTPAddress = words[061].Trim();
                        schoolInfo.EmailSMTPHost = words[062].Trim();
                        schoolInfo.Phone1 = words[063].Trim();

                        str = DateControl(3, words[066]); if (str == null) schoolInfo.SchoolYearStart = null; else schoolInfo.SchoolYearStart = DateTime.Parse(str);
                        str = DateControl(4, words[067]); if (str == null) schoolInfo.SchoolYearEnd = null; else schoolInfo.SchoolYearEnd = DateTime.Parse(str);

                        schoolInfo.FormTitle = words[083];
                        schoolInfo.EITitle = words[086].Trim();
                        schoolInfo.EIAddress = words[087].Trim();

                        schoolInfo.EICityID = 0;
                        if (words[088].Trim() == "İÇEL" || words[088].Trim() == "İçel") words[088] = "MERSİN";
                        if (words[088].Trim() == "AFYON" || words[088].Trim() == "Afyon") words[088] = "AFYONKARAHİSAR";
                        if (words[088].Trim() == "İZMİT" || words[088].Trim() == "İzmit") words[088] = "KOCAELİ";
                        if (words[088].Trim() == "ADAPAZARI" || words[088].Trim() == "Adapazarı") words[088] = "SAKARYA";
                        isExist = _parameterRepository.ExistCategoryName(words[088].Trim());
                        if (isExist && words[088].Trim() != "")
                            schoolInfo.EICityID = _parameterRepository.GetParameterCategoryName(words[088].Trim()).CategoryID;

                        schoolInfo.EITownID = 0;
                        isExist = _parameterRepository.ExistCategoryName(words[089].Trim());
                        isExistSub = _parameterRepository.ExistCategoryNameSub(schoolInfo.EICityID, words[089].Trim());
                        if (isExist && isExistSub && words[089].Trim() != "")
                            schoolInfo.EITownID = _parameterRepository.GetParameterCategoryName2(schoolInfo.EICityID, words[089].Trim()).CategoryID;

                        schoolInfo.EICountry = words[090].Trim();
                        schoolInfo.EITaxOffice = words[091].Trim();
                        schoolInfo.EITaxNo = words[092].Trim();
                        schoolInfo.EIWebAddress = words[093].Trim();
                        schoolInfo.EIEMail = words[094].Trim();
                        schoolInfo.EIPhone = words[095].Trim();
                        schoolInfo.EIFax = words[096].Trim();

                        schoolInfo.EIInvoiceSerialCode1 = words[097].Trim();
                        schoolInfo.EIInvoiceSerialCode2 = words[098].Trim();
                        schoolInfo.EIIsActive = false;
                        if (words[099] == "1") schoolInfo.EIIsActive = true;

                        schoolInfo.EIUserName = words[100].Trim();
                        schoolInfo.EIUserPassword = words[101].Trim();
                        schoolInfo.EITradeRegisterNo = words[102].Trim();
                        schoolInfo.EIIban = words[104].Trim();
                        schoolInfo.EIMersisNo = words[103].Trim();

                        schoolInfo.EIIntegratorNameID = 0;
                        if (words[105].Trim() == "UYUMSOFT")
                            schoolInfo.EIIntegratorNameID = _parameterRepository.GetParameterCategoryName("Uyumsoft").CategoryID;
                        if (words[105].Trim() == "VERIBAN")
                            schoolInfo.EIIntegratorNameID = _parameterRepository.GetParameterCategoryName("Veriban").CategoryID;

                        if (words[106].Trim() == "") words[106] = "0";
                        if (words[107].Trim() == "") words[107] = "0";
                        if (words[108].Trim() == "") words[108] = "0";
                        if (words[109].Trim() == "") words[109] = "0";
                        if (words[110].Trim() == "") words[110] = "0";
                        if (words[111].Trim() == "") words[111] = "0";
                        if (words[112].Trim() == "") words[112] = "0";
                        if (words[113].Trim() == "") words[113] = "0";
                        if (words[114].Trim() == "") words[114] = "0";
                        if (words[115].Trim() == "") words[115] = "0";
                        if (words[116].Trim() == "") words[116] = "0";
                        if (words[117].Trim() == "") words[117] = "0";
                        if (words[118].Trim() == "") words[118] = "0";
                        if (words[119].Trim() == "") words[119] = "0";
                        if (words[120].Trim() == "") words[120] = "0";
                        if (words[121].Trim() == "") words[121] = "0";
                        if (words[122].Trim() == "") words[122] = "0";
                        if (words[123].Trim() == "") words[123] = "0";
                        if (words[124].Trim() == "") words[124] = "0";
                        if (words[125].Trim() == "") words[125] = "0";
                        if (words[126].Trim() == "") words[126] = "0";
                        if (words[127].Trim() == "") words[127] = "0";
                        if (words[128].Trim() == "") words[128] = "0";
                        if (words[129].Trim() == "") words[129] = "0";
                        if (words[130].Trim() == "") words[130] = "0";

                        serialNumber.RegisterNo = Convert.ToInt32(words[106]);
                        serialNumber.CheckNo = Convert.ToInt32(words[107]);
                        serialNumber.BondNo = Convert.ToInt32(words[108]);
                        serialNumber.OtsNo1 = Convert.ToInt32(words[109]);
                        serialNumber.OtsNo2 = Convert.ToInt32(words[109]);
                        serialNumber.MailOrderNo = Convert.ToInt32(words[110]);
                        serialNumber.CreditCardNo = Convert.ToInt32(words[111]);
                        serialNumber.GovernmentPromotionNo = Convert.ToInt32(words[112]);
                        serialNumber.KmhNo = Convert.ToInt32(words[113]);
                        serialNumber.AccountSerialNo = Convert.ToInt32(words[114]);

                        serialNumber.InvoiceName1 = words[116].Trim();
                        serialNumber.InvoiceName2 = words[117].Trim();
                        serialNumber.InvoiceName3 = words[118].Trim();
                        serialNumber.InvoiceName4 = words[119].Trim();
                        serialNumber.InvoiceName5 = words[120].Trim();
                        serialNumber.InvoiceSerialNo1 = Convert.ToInt32(words[121]);
                        serialNumber.InvoiceSerialNo2 = Convert.ToInt32(words[122]);
                        serialNumber.InvoiceSerialNo3 = Convert.ToInt32(words[123]);
                        serialNumber.InvoiceSerialNo4 = Convert.ToInt32(words[124]);
                        serialNumber.InvoiceSerialNo5 = Convert.ToInt32(words[125]);
                        serialNumber.InvoiceSerialNo11 = Convert.ToInt32(words[126]);
                        serialNumber.InvoiceSerialNo22 = Convert.ToInt32(words[127]);
                        serialNumber.InvoiceSerialNo33 = Convert.ToInt32(words[128]);
                        serialNumber.InvoiceSerialNo44 = Convert.ToInt32(words[129]);
                        serialNumber.InvoiceSerialNo55 = Convert.ToInt32(words[130]);

                        schoolInfo.DefaultInstallment = 10;
                        schoolInfo.CurrencyDecimalPlaces = 2;
                        schoolInfo.SortOption = false;
                        schoolInfo.SortType = 1;
                        schoolInfo.SchoolID = 0;
                        schoolInfo.IsActive = true;
                        schoolInfo.DefaultShowDept = true;
                        //_schoolInfoRepository.CreateSchoolInfo(schoolInfo);

                        serialNumber.PSerialNumberID = schoolInfo.SchoolID;
                        _pSerialNumberRepository.CreatePSerialNumber(serialNumber);
                        bool isSubmit = false;
                        for (int u = 1; u < 11; u++)
                        {
                            isSubmit = false;
                            if (u == 01 && words[047].Trim() != "") isSubmit = true;
                            if (u == 02 && words[048].Trim() != "") isSubmit = true;
                            if (u == 03 && words[049].Trim() != "") isSubmit = true;
                            if (u == 04 && words[050].Trim() != "") isSubmit = true;
                            if (u == 05 && words[051].Trim() != "") isSubmit = true;
                            if (u == 06 && words[052].Trim() != "") isSubmit = true;
                            if (u == 07 && words[053].Trim() != "") isSubmit = true;
                            if (u == 08 && words[054].Trim() != "") isSubmit = true;
                            if (u == 09 && words[055].Trim() != "") isSubmit = true;
                            if (u == 10 && words[056].Trim() != "") isSubmit = true;
                            if (isSubmit)
                            {
                                schoolFee.SchoolFeeID = 0;
                                schoolFee.FeeCategory = 1;
                                schoolFee.SchoolID = schoolInfo.SchoolID;
                                schoolFee.CategoryLevel = "L1";
                                schoolFee.SortOrder = u;
                                schoolFee.IsActive = true;
                                schoolFee.IsSelect = true;

                                if (u == 01) schoolFee.Tax = Convert.ToByte(words[037]);
                                if (u == 02) schoolFee.Tax = Convert.ToByte(words[038]);
                                if (u == 03) schoolFee.Tax = Convert.ToByte(words[039]);
                                if (u == 04) schoolFee.Tax = Convert.ToByte(words[040]);
                                if (u == 05) schoolFee.Tax = Convert.ToByte(words[041]);
                                if (u == 06) schoolFee.Tax = Convert.ToByte(words[042]);
                                if (u == 07) schoolFee.Tax = Convert.ToByte(words[043]);
                                if (u == 08) schoolFee.Tax = Convert.ToByte(words[044]);
                                if (u == 09) schoolFee.Tax = Convert.ToByte(words[045]);
                                if (u == 10) schoolFee.Tax = Convert.ToByte(words[046]);

                                if (u == 01) schoolFee.Name = words[047].Trim();
                                if (u == 02) schoolFee.Name = words[048].Trim();
                                if (u == 03) schoolFee.Name = words[049].Trim();
                                if (u == 04) schoolFee.Name = words[050].Trim();
                                if (u == 05) schoolFee.Name = words[051].Trim();
                                if (u == 06) schoolFee.Name = words[052].Trim();
                                if (u == 07) schoolFee.Name = words[053].Trim();
                                if (u == 08) schoolFee.Name = words[054].Trim();
                                if (u == 09) schoolFee.Name = words[055].Trim();
                                if (u == 10) schoolFee.Name = words[056].Trim();
                                _schoolFeeRepository.CreateSchoolFee(schoolFee);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Classroom
            file = "Classroom.CSV";
            Classroom classroom = new Classroom();

            isExist = false;
            filePath = Path.Combine(projectDir, file);
            if (System.IO.File.Exists(filePath)) isExist = true;
            if (isExist)
            {
                readText = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("iso-8859-9"));
                inx = 0;
                periodTmp = null;
                foreach (var csv in readText)
                {
                    words = csv.Split(';');
                    if (words[003].Trim() != "")
                    {
                        int max = listPanelModel.StartNumber;
                        int schoolCode = Convert.ToInt32(words[000].Trim());
                        if (schoolCode <= max)
                        {
                            classroom.Period = words[001] + "-" + words[002];
                            if (classroom.Period != periodTmp) { inx = 0; periodTmp = classroom.Period; }

                            inx++;
                            classroom.ClassroomID = 0;
                            classroom.SchoolID = Convert.ToInt32(words[000]);


                            classroom.ClassroomName = words[003].Trim();

                            if (words[004].Trim() == "") words[004] = "0";
                            if (words[004].Trim() == "ANA SINIFI") words[004] = "Ana Sınıfı";

                            bool isClassroomType = false;
                            if (words[004].Trim() != "0")
                                isClassroomType = _parameterRepository.ExistCategoryName(words[004]);

                            if (!isClassroomType)
                            {
                                var ct = new Parameter();
                                ct.CategoryID = 0;
                                ct.CategorySubID = 8;
                                ct.CategoryName = words[004];
                                ct.Language1 = words[004];
                                ct.Language2 = words[004];
                                ct.Language3 = words[004];
                                ct.Language4 = words[004];
                                ct.Color = null;
                                ct.CategoryLevel = "L2";
                                ct.SortOrder = inx;
                                ct.IsActive = true;
                                ct.IsProtected = true;
                                ct.NationalityCode = "TR";
                                ct.IsSelect = null;
                                ct.IsDirtySelect = null;
                                _parameterRepository.CreateParameter(ct);
                            }

                            if (words[004].Trim() != "0")
                                classroom.ClassroomTypeID = _parameterRepository.GetParameterCategoryName(words[004]).CategoryID;


                            classroom.ClassroomTeacher = words[005].Trim();

                            if (words[006].Trim() == null || words[006].Trim() == "") words[006] = "0";
                            classroom.RoomQuota = Convert.ToInt32(words[006]);
                            classroom.SortOrder = inx;
                            classroom.IsActive = true;
                            _classroomRepository.CreateClassroom(classroom);
                        }
                    }
                }
            }
            #endregion
            #region Student
            //Geriye yönelik Yıl sayısı

            int YY1 = Convert.ToInt32(listPanelModel.NewPeriod.Substring(2, 2));
            int YY2 = Convert.ToInt32(listPanelModel.NewPeriod.Substring(7, 2));
            string period = null;

            Student student = new Student();
            StudentPeriods studentPeriods = new StudentPeriods();
            StudentAddress studentAddress = new StudentAddress();
            StudentParentAddress studentParentAddress = new StudentParentAddress();
            StudentFamilyAddress studentFamilyAddress = new StudentFamilyAddress();
            StudentNote studentNote = new StudentNote();
            StudentInvoiceAddress address = new StudentInvoiceAddress();
            bool isPrv = false;
            bool isExist3 = false;

            words = null;
            for (int p = 1; p < 3; p++)
            {
                for (int i = 1; i <= listPanelModel.StartNumber; i++)
                {
                    file = "Student" + i.ToString("D2") + "-" + YY1 + YY2 + ".CSV";
                    period = "20" + YY1 + "-" + "20" + YY2;

                    isExist = false;
                    filePath = Path.Combine(projectDir, file);
                    if (System.IO.File.Exists(filePath)) isExist = true;
                    if (isExist)
                    {
                        readText = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("iso-8859-9"));
                        inx = 0;
                        foreach (var csv in readText)
                        {
                            words = csv.Split(';');
                            inx++;
                            isExist = true;
                            student.StudentID = 0;
                            student.SchoolID = i;

                            //if (words[000].Trim() == "18802053186" || words[000].Trim() == "10017218572")
                            //     isExist3 = _studentRepository.ExistStudentIdNumber(words[000].Trim());
                            //else goto CONTINUE;

                            student.FirstName = words[001].Trim();
                            student.LastName = words[002].Trim();
                            student.ClassroomID = 0;
                            if (isPrv)
                            {
                                isExist3 = _studentRepository.ExistStudentIdNumber(words[000].Trim());
                                if (isExist3) goto CONTINUE;
                            }
                            else
                            {
                                if (words[003].Trim() != "")
                                {
                                    isExist = _classroomRepository.ExistClassroomName(period, words[003].Trim());
                                    if (isExist)
                                        student.ClassroomID = _classroomRepository.GetClassroomNamePeriod(period, words[003].Trim()).ClassroomID;
                                }
                            }

                            if (words[004].Trim() != "")
                                student.StatuCategoryID = _parameterRepository.GetParameterCategoryName(words[004]).CategoryID;
                            if (words[005].Trim() != "")
                                student.GenderTypeCategoryID = _parameterRepository.GetParameterCategoryName(words[005]).CategoryID;
                            if (words[005] == "Erkek") student.StudentPicture = "male.jpg";
                            else student.StudentPicture = "female.jpg";
                            student.IdNumber = words[006].Trim();
                            student.StudentNumber = words[166].Trim();
                            student.StudentSerialNumber = Convert.ToInt32(words[183]);

                            student.BloodGroupCategoryID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[112].Trim());
                            if (isExist && words[112].Trim() != "")
                                student.BloodGroupCategoryID = _parameterRepository.GetParameterCategoryName(words[112].Trim()).CategoryID;

                            student.NationalityID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[110].Trim());
                            if (isExist && words[110].Trim() != "")
                                student.NationalityID = _parameterRepository.GetParameterCategoryName(words[110].Trim()).CategoryID;

                            student.ReligiousID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[111].Trim());
                            if (isExist && words[111].Trim() != "")
                                student.ReligiousID = _parameterRepository.GetParameterCategoryName(words[111].Trim()).CategoryID;

                            string str = DateControl(5, words[007]); if (str == null) student.DateOfRegistration = null; else student.DateOfRegistration = DateTime.Parse(str);

                            student.RegistrationTypeCategoryID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[008].Trim());
                            if (isExist && words[008].Trim() != "")
                                student.RegistrationTypeCategoryID = _parameterRepository.GetParameterCategoryName(words[008].Trim()).CategoryID;

                            student.IsActive = true;
                            student.IsPension = false;
                            //--> EV Adresi

                            student.SchoolBusDepartureID = 0;
                            isExist = _schoolBusServicesRepository.ExistSchoolBusServicesRoute(words[015].Trim());
                            if (isExist)
                                student.SchoolBusDepartureID = _schoolBusServicesRepository.GetSchoolBusRoute(words[015]).SchoolBusServicesID;

                            //--> Parrent
                            student.PreviousSchoolID = 0;
                            string txtx = words[084].Trim();
                            isExist = _parameterRepository.ExistCategoryName(txtx);
                            if (isExist)
                                student.PreviousSchoolID = _parameterRepository.GetParameterCategoryName(txtx).CategoryID;
                            else
                            {
                                if (txtx != "")
                                {
                                    int ID = _parameterRepository.GetParameterCategoryName("Geldiği Okul").CategoryID;
                                    var sort = _parameterRepository.GetParameterSubID(ID);
                                    var getCode = new Parameter();
                                    getCode.CategoryID = 0;
                                    getCode.CategorySubID = ID;
                                    getCode.CategoryName = txtx;
                                    getCode.CategoryLevel = "L2";
                                    int inxs = (int)sort.Max(a => a.SortOrder);
                                    inxs++;
                                    getCode.SortOrder = inxs;
                                    getCode.IsActive = true;
                                    getCode.IsProtected = true;
                                    getCode.NationalityCode = "TR";
                                    getCode.IsSelect = null;

                                    _parameterRepository.CreateParameter(getCode);

                                    student.PreviousSchoolID = getCode.CategoryID;
                                }
                            }

                            student.PreviousBranchID = 0;
                            txtx = words[085].Trim();
                            isExist = _parameterRepository.ExistCategoryName(txtx);
                            if (isExist)
                                student.PreviousBranchID = _parameterRepository.GetParameterCategoryName(txtx).CategoryID;
                            else
                            {
                                if (txtx != "")
                                {
                                    int ID = _parameterRepository.GetParameterCategoryName("Geldiği Bölüm").CategoryID;
                                    var sort = _parameterRepository.GetParameterSubID(ID);
                                    var getCode = new Parameter();
                                    getCode.CategoryID = 0;
                                    getCode.CategorySubID = ID;
                                    getCode.CategoryName = txtx;
                                    getCode.CategoryLevel = "L2";
                                    int inxs = (int)sort.Max(a => a.SortOrder);
                                    inxs++;
                                    getCode.SortOrder = inxs;
                                    getCode.IsActive = true;
                                    getCode.IsProtected = true;
                                    getCode.NationalityCode = "TR";
                                    getCode.IsSelect = null;
                                    _parameterRepository.CreateParameter(getCode);

                                    student.PreviousBranchID = getCode.CategoryID;
                                }
                            }

                            str = DateControl(6, words[090]); if (str == null) student.ScholarshipStartDate = null; else student.ScholarshipStartDate = DateTime.Parse(str);
                            str = DateControl(7, words[091]); if (str == null) student.ScholarshipEndDate = null; else student.ScholarshipEndDate = DateTime.Parse(str);

                            if (words[096].Trim() == "") words[096] = "0";
                            student.ScholarshipRate = Convert.ToByte(words[096]);

                            str = DateControl(8, words[097]); if (str == null) student.DateOfBird = null; else student.DateOfBird = DateTime.Parse(str);

                            student.IsExplanationShow = false;
                            if (words[107] == "1") student.IsExplanationShow = true;

                            student.SchoolBusStatuID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[108].Trim());
                            if (isExist && words[108].Trim() != "")
                                student.SchoolBusStatuID = _parameterRepository.GetParameterCategoryName(words[108].Trim()).CategoryID; ;

                            student.SchoolBusReturnID = 0;
                            isExist = _schoolBusServicesRepository.ExistSchoolBusServicesRoute(words[151].Trim());
                            if (isExist)
                                student.SchoolBusReturnID = _schoolBusServicesRepository.GetSchoolBusRoute(words[151]).SchoolBusServicesID;

                            student.SchoolBusDepartureDriverID = 0;
                            isExist = _schoolBusServicesRepository.ExistSchoolBusServicesDriver(words[164].Trim());
                            if (isExist)
                                student.SchoolBusDepartureDriverID = _schoolBusServicesRepository.GetSchoolBusDrive(words[164]).SchoolBusServicesID;

                            student.SchoolBusReturnDriverID = 0;
                            isExist = _schoolBusServicesRepository.ExistSchoolBusServicesDriver(words[165].Trim());
                            if (isExist)
                                student.SchoolBusReturnDriverID = _schoolBusServicesRepository.GetSchoolBusDrive(words[165]).SchoolBusServicesID;

                            str = DateControl(9, words[160]); if (str == null) student.FirstDateOfRegistration = null; else student.FirstDateOfRegistration = DateTime.Parse(str);

                            student.ParentName = words[017].Trim();
                            _studentRepository.CreateStudent(student);

                            //StudentParentAddress
                            studentParentAddress.StudentParentAddressID = 0;
                            studentParentAddress.StudentID = student.StudentID;
                            studentParentAddress.ParentGenderTypeCategoryID = 20;
                            studentParentAddress.ParentPicture = "male.jpg";

                            if (words[018].Trim() == "ANNESİ" || words[018].Trim() == "ANNE") words[018] = "Annesi";
                            if (words[018].Trim() == "BABASI" || words[018].Trim() == "BABA") words[018] = "Babası";
                            studentParentAddress.KinshipCategoryID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[018].Trim());
                            if (isExist && words[018].Trim() != "")
                                studentParentAddress.KinshipCategoryID = _parameterRepository.GetParameterCategoryName(words[018].Trim()).CategoryID;

                            studentParentAddress.ProfessionCategoryID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[182].Trim());

                            txtx = words[182].Trim();
                            if (isExist && words[182].Trim() != "")
                                studentParentAddress.ProfessionCategoryID = _parameterRepository.GetParameterCategoryName(words[182].Trim()).CategoryID;
                            else
                            {
                                if (txtx != "")
                                {
                                    int ID = _parameterRepository.GetParameterCategoryName("Mesleği").CategoryID;
                                    var sort = _parameterRepository.GetParameterSubID(ID);
                                    var getCode = new Parameter();
                                    getCode.CategoryID = 0;
                                    getCode.CategorySubID = ID;
                                    getCode.CategoryName = txtx;
                                    getCode.CategoryLevel = "L2";
                                    int inxs = (int)sort.Max(a => a.SortOrder);
                                    inxs++;
                                    getCode.SortOrder = inxs;
                                    getCode.IsActive = true;
                                    getCode.IsProtected = true;
                                    getCode.NationalityCode = "TR";
                                    getCode.IsSelect = null;

                                    _parameterRepository.CreateParameter(getCode);

                                    studentParentAddress.ProfessionCategoryID = getCode.CategoryID;
                                }
                            }

                            studentParentAddress.IdNumber = words[019].Trim();
                            studentParentAddress.MobilePhone = words[020].Trim();
                            studentParentAddress.EMail = words[021].Trim();
                            studentParentAddress.HomeAddress = words[022].Trim();

                            studentParentAddress.HomeCityParameterID = 0;
                            if (words[023].Trim() == "İÇEL" || words[023].Trim() == "İçel") words[023] = "MERSİN";
                            if (words[023].Trim() == "AFYON" || words[023].Trim() == "Afyon") words[023] = "AFYONKARAHİSAR";
                            if (words[023].Trim() == "İZMİT" || words[023].Trim() == "İzmit") words[023] = "KOCAELİ";
                            if (words[023].Trim() == "ADAPAZARI" || words[023].Trim() == "Adapazarı") words[023] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[023].Trim());
                            if (isExist && words[023].Trim() != "")
                                studentParentAddress.HomeCityParameterID = _parameterRepository.GetParameterCategoryName(words[023].Trim()).CategoryID;

                            studentParentAddress.HomeTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[024].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentParentAddress.HomeCityParameterID, words[024].Trim());
                            if (isExist && isExistSub && words[024].Trim() != "")
                                studentParentAddress.HomeTownParameterID = _parameterRepository.GetParameterCategoryName2(studentParentAddress.HomeCityParameterID, words[024].Trim()).CategoryID;

                            studentParentAddress.HomeZipCode = words[025].Trim();
                            studentParentAddress.HomePhone = words[026].Trim();
                            studentParentAddress.WorkAddress = words[027].Trim();

                            studentParentAddress.WorkCityParameterID = 0;

                            if (words[028].Trim() == "İÇEL" || words[028].Trim() == "İçel") words[028] = "MERSİN";
                            if (words[028].Trim() == "AFYON" || words[028].Trim() == "Afyon") words[028] = "AFYONKARAHİSAR";
                            if (words[028].Trim() == "İZMİT" || words[028].Trim() == "İzmit") words[028] = "KOCAELİ";
                            if (words[028].Trim() == "ADAPAZARI" || words[028].Trim() == "Adapazarı") words[028] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[028].Trim());
                            if (isExist && words[028].Trim() != "")
                                studentParentAddress.WorkCityParameterID = _parameterRepository.GetParameterCategoryName(words[028].Trim()).CategoryID;

                            studentParentAddress.WorkTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[029].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentParentAddress.WorkCityParameterID, words[029].Trim());
                            if (isExist && isExistSub && words[029].Trim() != "")
                                studentParentAddress.WorkTownParameterID = _parameterRepository.GetParameterCategoryName2(studentParentAddress.WorkCityParameterID, words[029].Trim()).CategoryID;

                            studentParentAddress.WorkZipCode = words[030].Trim();
                            studentParentAddress.WorkPhone = words[031].Trim();

                            str = DateControl(10, words[032]); if (str == null) studentParentAddress.DebtorDOB = null; else studentParentAddress.DebtorDOB = DateTime.Parse(str);

                            // words[033];
                            studentParentAddress.DebtorPlaceOfBirth = words[034].Trim();
                            studentParentAddress.IsSMS = false;

                            if (words[100] == "1") studentParentAddress.IsSMS = true;
                            studentParentAddress.IsEmail = false;
                            if (words[101] == "1") studentParentAddress.IsEmail = true;


                            studentParentAddress.Name3 = words[074].Trim();
                            studentParentAddress.Address3 = words[075].Trim();

                            studentParentAddress.CityParameterID3 = 0;

                            if (words[076].Trim() == "İÇEL" || words[076].Trim() == "İçel") words[076] = "MERSİN";
                            if (words[076].Trim() == "AFYON" || words[076].Trim() == "Afyon") words[076] = "AFYONKARAHİSAR";
                            if (words[076].Trim() == "İZMİT" || words[076].Trim() == "İzmit") words[076] = "KOCAELİ";
                            if (words[076].Trim() == "ADAPAZARI" || words[076].Trim() == "Adapazarı") words[076] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[076].Trim());
                            if (isExist && words[076].Trim() != "")
                                studentParentAddress.CityParameterID3 = _parameterRepository.GetParameterCategoryName(words[076].Trim()).CategoryID;

                            studentParentAddress.TownParameterID3 = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[077].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentParentAddress.CityParameterID3, words[077].Trim());
                            if (isExist && isExistSub && words[077].Trim() != "")
                                studentParentAddress.TownParameterID3 = _parameterRepository.GetParameterCategoryName2(studentParentAddress.CityParameterID3, words[077].Trim()).CategoryID;

                            studentParentAddress.ZipCode3 = words[078].Trim();
                            studentParentAddress.MobilePhone3 = words[079].Trim();
                            studentParentAddress.HomePhone3 = null;
                            studentParentAddress.WorkPhone3 = words[080].Trim(); ;

                            studentParentAddress.IdNumber3 = words[161].Trim();

                            studentParentAddress.CardType1 = false;
                            if (words[126] == "V") studentParentAddress.CardType1 = true;

                            string txt = words[127].Trim();
                            studentParentAddress.CardNumber1 = null;
                            if (txt != "" && txt.Length > 15) studentParentAddress.CardNumber1 = txt.Substring(0, 16);

                            studentParentAddress.CardExpiryCVC1 = words[127].Substring(16, 04);
                            studentParentAddress.CardBankName1 = words[128].Trim();

                            if (words[129].Trim() == "" || words[129].Trim() == "00" || words[129].Trim().Count() < 2) words[129] = "";
                            if (words[129].Trim() != "")
                            {
                                studentParentAddress.CardExpiryYear1 = words[129].Substring(0, 4);
                                studentParentAddress.CardExpiryMonth1 = words[129].Substring(4, 2);
                            }
                            else
                            {
                                studentParentAddress.CardExpiryYear1 = null;
                                studentParentAddress.CardExpiryMonth1 = null;
                            }
                            studentParentAddress.CardAccountCuttingDay1 = words[130].Trim();

                            studentParentAddress.CardType2 = false;
                            if (words[131] == "V") studentParentAddress.CardType2 = true;

                            txt = words[132].Trim();
                            studentParentAddress.CardNumber2 = null;
                            if (txt != "") studentParentAddress.CardNumber2 = txt.Substring(0, 16);

                            studentParentAddress.CardExpiryCVC2 = words[132].Substring(16, 04);
                            studentParentAddress.CardBankName2 = words[133].Trim();

                            studentParentAddress.DebtorFatherName = words[137].Trim();

                            if (words[135].Trim() == "" || words[135].Trim() == "00" || words[135].Trim().Count() < 2) words[135] = "";
                            if (words[135].Trim() != "")
                            {
                                studentParentAddress.CardExpiryYear2 = words[135].Substring(0, 4);
                                studentParentAddress.CardExpiryMonth2 = words[135].Substring(4, 2);
                            }
                            else
                            {
                                studentParentAddress.CardExpiryYear2 = null;
                                studentParentAddress.CardExpiryMonth2 = null;
                            }
                            studentParentAddress.CardNameOnCard1 = words[137].Trim();
                            studentParentAddress.CardNameOnCard2 = words[137].Trim();

                            studentParentAddress.CardAccountCuttingDay2 = words[134].Trim();
                            //words[137];
                            studentParentAddress.DebtorName = words[137].Trim();
                            studentParentAddress.DebtorAddress = words[138].Trim();

                            studentParentAddress.DebtorCityID = 0;
                            if (words[139].Trim() == "İÇEL" || words[139].Trim() == "İçel") words[139] = "MERSİN";
                            if (words[139].Trim() == "AFYON" || words[139].Trim() == "Afyon") words[139] = "AFYONKARAHİSAR";
                            if (words[139].Trim() == "İZMİT" || words[139].Trim() == "İzmit") words[139] = "KOCAELİ";
                            if (words[139].Trim() == "ADAPAZARI" || words[139].Trim() == "Adapazarı") words[139] = "SAKARYA";

                            isExist = _parameterRepository.ExistCategoryName(words[139].Trim());
                            if (isExist && words[139].Trim() != "")
                                studentParentAddress.DebtorCityID = _parameterRepository.GetParameterCategoryName(words[139].Trim()).CategoryID;

                            studentParentAddress.DebtorTownID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[140].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentParentAddress.DebtorCityID, words[140].Trim());
                            if (isExist && isExistSub && words[140].Trim() != "")
                                studentParentAddress.DebtorTownID = _parameterRepository.GetParameterCategoryName2(studentParentAddress.DebtorCityID, words[140].Trim()).CategoryID;

                            studentParentAddress.DebtorZipCode = words[141].Trim();
                            studentParentAddress.DebtorHomePhone = words[142].Trim();
                            studentParentAddress.DebtorWorkPhone = words[143].Trim();
                            studentParentAddress.DebtorMobilePhone = words[144].Trim();
                            //words[146];

                            studentParentAddress.GuarantorName = words[152].Trim();
                            studentParentAddress.GuarantorAddress = words[153].Trim();

                            studentParentAddress.GuarantorCityParameterID = 0;
                            if (words[154].Trim() == "İÇEL" || words[154].Trim() == "İçel") words[154] = "MERSİN";
                            if (words[154].Trim() == "AFYON" || words[154].Trim() == "Afyon") words[154] = "AFYONKARAHİSAR";
                            if (words[154].Trim() == "İZMİT" || words[154].Trim() == "İzmit") words[154] = "KOCAELİ";
                            if (words[154].Trim() == "ADAPAZARI" || words[154].Trim() == "Adapazarı") words[154] = "SAKARYA";

                            isExist = _parameterRepository.ExistCategoryName(words[154].Trim());
                            if (isExist && words[154].Trim() != "")
                                studentParentAddress.GuarantorCityParameterID = _parameterRepository.GetParameterCategoryName(words[154].Trim()).CategoryID;

                            studentParentAddress.GuarantorTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[155].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentParentAddress.GuarantorCityParameterID, words[155].Trim());
                            if (isExist && isExistSub && words[155].Trim() != "")
                                studentParentAddress.GuarantorTownParameterID = _parameterRepository.GetParameterCategoryName2(studentParentAddress.GuarantorCityParameterID, words[155].Trim()).CategoryID;

                            studentParentAddress.GuarantorZipCode = words[156].Trim();
                            studentParentAddress.GuarantorOther = words[157].Trim();
                            studentParentAddress.GuarantorPhone = words[158].Trim();
                            studentParentAddress.GuarantorId = words[161].Trim();

                            _studentParentAddressRepository.CreateStudentParentAddress(studentParentAddress);

                            //StudentFamilyAddress
                            studentFamilyAddress.StudentFamilyAddressID = 0;
                            studentFamilyAddress.StudentID = student.StudentID;
                            studentFamilyAddress.FatherName = words[044].Trim();

                            studentFamilyAddress.FatherProfessionCategoryID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[045].Trim());
                            txtx = words[045].Trim();
                            if (isExist && words[045].Trim() != "")
                                studentFamilyAddress.FatherProfessionCategoryID = _parameterRepository.GetParameterCategoryName(words[045].Trim()).CategoryID;
                            else
                            {
                                if (txtx != "")
                                {
                                    int ID = _parameterRepository.GetParameterCategoryName("Mesleği").CategoryID;
                                    var sort = _parameterRepository.GetParameterSubID(ID);
                                    var getCode = new Parameter();
                                    getCode.CategoryID = 0;
                                    getCode.CategorySubID = ID;
                                    getCode.CategoryName = txtx;
                                    getCode.CategoryLevel = "L2";
                                    int inxs = (int)sort.Max(a => a.SortOrder);
                                    inxs++;
                                    getCode.SortOrder = inxs;
                                    getCode.IsActive = true;
                                    getCode.IsProtected = true;
                                    getCode.NationalityCode = "TR";
                                    getCode.IsSelect = null;

                                    _parameterRepository.CreateParameter(getCode);

                                    studentFamilyAddress.FatherProfessionCategoryID = getCode.CategoryID;
                                }
                            }

                            if (words[046].Trim().Count() != 11) words[046] = "";
                            studentFamilyAddress.FatherIdNumber = words[046].Trim();
                            studentFamilyAddress.FatherMobilePhone = words[047].Trim();
                            studentFamilyAddress.FatherEMail = words[048].Trim();
                            studentFamilyAddress.FatherHomeAddress = words[049].Trim();

                            studentFamilyAddress.FatherHomeCityParameterID = 0;

                            if (words[050].Trim() == "İÇEL" || words[050].Trim() == "İçel") words[050] = "MERSİN";
                            if (words[050].Trim() == "AFYON" || words[050].Trim() == "Afyon") words[050] = "AFYONKARAHİSAR";
                            if (words[050].Trim() == "İZMİT" || words[050].Trim() == "İzmit") words[050] = "KOCAELİ";
                            if (words[050].Trim() == "ADAPAZARI" || words[050].Trim() == "Adapazarı") words[050] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[050].Trim());
                            if (isExist && words[050].Trim() != "")
                                studentFamilyAddress.FatherHomeCityParameterID = _parameterRepository.GetParameterCategoryName(words[050].Trim()).CategoryID;

                            studentFamilyAddress.FatherHomeTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[051].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentFamilyAddress.FatherHomeCityParameterID, words[051].Trim());
                            if (isExist && isExistSub && words[051].Trim() != "")
                                studentFamilyAddress.FatherHomeTownParameterID = _parameterRepository.GetParameterCategoryName2(studentFamilyAddress.FatherHomeCityParameterID, words[051].Trim()).CategoryID;

                            studentFamilyAddress.FatherHomeZipCode = words[052].Trim();
                            studentFamilyAddress.FatherHomePhone = words[053].Trim();
                            studentFamilyAddress.FatherWorkAddress = words[054].Trim();

                            studentFamilyAddress.FatherWorkCityParameterID = 0;

                            if (words[055].Trim() == "İÇEL" || words[055].Trim() == "İçel") words[055] = "MERSİN";
                            if (words[055].Trim() == "AFYON" || words[055].Trim() == "Afyon") words[055] = "AFYONKARAHİSAR";
                            if (words[055].Trim() == "İZMİT" || words[055].Trim() == "İzmit") words[055] = "KOCAELİ";
                            if (words[055].Trim() == "ADAPAZARI" || words[055].Trim() == "Adapazarı") words[055] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[055].Trim());
                            if (isExist && words[055].Trim() != "")
                                studentFamilyAddress.FatherWorkCityParameterID = _parameterRepository.GetParameterCategoryName(words[055].Trim()).CategoryID;

                            studentFamilyAddress.FatherWorkTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[056].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentFamilyAddress.FatherWorkCityParameterID, words[056].Trim());
                            if (isExist && isExistSub && words[056].Trim() != "")
                                studentFamilyAddress.FatherWorkTownParameterID = _parameterRepository.GetParameterCategoryName2(studentFamilyAddress.FatherWorkCityParameterID, words[056].Trim()).CategoryID;

                            if (words[057].Trim().Count() > 10) words[057] = "";
                            studentFamilyAddress.FatherWorkZipCode = words[057].Trim();
                            studentFamilyAddress.FatherWorkPhone = words[058].Trim();

                            studentFamilyAddress.MotherName = words[059].Trim();

                            studentFamilyAddress.MotherProfessionCategoryID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[060].Trim());
                            txtx = words[060].Trim();
                            if (isExist && words[060].Trim() != "")
                                studentFamilyAddress.MotherProfessionCategoryID = _parameterRepository.GetParameterCategoryName(words[060].Trim()).CategoryID;
                            else
                            {
                                if (txtx != "")
                                {
                                    int ID = _parameterRepository.GetParameterCategoryName("Mesleği").CategoryID;
                                    var sort = _parameterRepository.GetParameterSubID(ID);
                                    var getCode = new Parameter();
                                    getCode.CategoryID = 0;
                                    getCode.CategorySubID = ID;
                                    getCode.CategoryName = txtx;
                                    getCode.CategoryLevel = "L2";
                                    int inxs = (int)sort.Max(a => a.SortOrder);
                                    inxs++;
                                    getCode.SortOrder = inxs;
                                    getCode.IsActive = true;
                                    getCode.IsProtected = true;
                                    getCode.NationalityCode = "TR";
                                    getCode.IsSelect = null;

                                    _parameterRepository.CreateParameter(getCode);

                                    studentFamilyAddress.MotherProfessionCategoryID = getCode.CategoryID;
                                }
                            }

                            studentFamilyAddress.MotherIdNumber = words[061].Trim();
                            studentFamilyAddress.MotherMobilePhone = words[062];
                            studentFamilyAddress.MotherEMail = words[063].Trim();
                            studentFamilyAddress.MotherHomeAddress = words[064].Trim();

                            studentFamilyAddress.MotherHomeCityParameterID = 0;
                            if (words[065].Trim() == "İÇEL" || words[065].Trim() == "İçel") words[065] = "MERSİN";
                            if (words[065].Trim() == "AFYON" || words[065].Trim() == "Afyon") words[065] = "AFYONKARAHİSAR";
                            if (words[065].Trim() == "İZMİT" || words[065].Trim() == "İzmit") words[065] = "KOCAELİ";
                            if (words[065].Trim() == "ADAPAZARI" || words[065].Trim() == "Adapazarı") words[065] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[065].Trim());
                            if (isExist && words[065].Trim() != "")
                                studentFamilyAddress.MotherHomeCityParameterID = _parameterRepository.GetParameterCategoryName(words[065].Trim()).CategoryID;

                            studentFamilyAddress.MotherHomeTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[066].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentFamilyAddress.MotherHomeCityParameterID, words[066].Trim());
                            if (isExist && isExistSub && words[066].Trim() != "")
                                studentFamilyAddress.MotherHomeTownParameterID = _parameterRepository.GetParameterCategoryName2(studentFamilyAddress.MotherHomeCityParameterID, words[066].Trim()).CategoryID;

                            studentFamilyAddress.MotherHomeZipCode = words[067].Trim();
                            studentFamilyAddress.MotherHomePhone = words[068].Trim();
                            studentFamilyAddress.MotherWorkAddress = words[069].Trim();

                            studentFamilyAddress.MotherWorkCityParameterID = 0;
                            if (words[070].Trim() == "İÇEL" || words[070].Trim() == "İçel") words[070] = "MERSİN";
                            if (words[070].Trim() == "AFYON" || words[070].Trim() == "Afyon") words[070] = "AFYONKARAHİSAR";
                            if (words[070].Trim() == "İZMİT" || words[070].Trim() == "İzmit") words[070] = "KOCAELİ";
                            if (words[070].Trim() == "ADAPAZARI" || words[070].Trim() == "Adapazarı") words[070] = "SAKARYA";

                            isExist = _parameterRepository.ExistCategoryName(words[070].Trim());
                            if (isExist && words[070].Trim() != "")
                                studentFamilyAddress.MotherWorkCityParameterID = _parameterRepository.GetParameterCategoryName(words[070].Trim()).CategoryID;

                            studentFamilyAddress.MotherWorkTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[071].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentFamilyAddress.MotherWorkCityParameterID, words[071].Trim());
                            if (isExist && isExistSub && words[071].Trim() != "")
                                studentFamilyAddress.MotherWorkTownParameterID = _parameterRepository.GetParameterCategoryName2(studentFamilyAddress.MotherWorkCityParameterID, words[071].Trim()).CategoryID;

                            studentFamilyAddress.MotherWorkZipCode = words[072].Trim();
                            studentFamilyAddress.MotherWorkPhone = words[073].Trim();

                            studentFamilyAddress.MotherIsSMS = false;
                            if (words[102] == "1") studentFamilyAddress.MotherIsSMS = true;
                            studentFamilyAddress.MotherIsEmail = false;
                            if (words[103] == "1") studentFamilyAddress.MotherIsEmail = true;
                            studentFamilyAddress.FatherIsSMS = false;
                            if (words[104] == "1") studentFamilyAddress.FatherIsSMS = true;
                            studentFamilyAddress.FatherIsEmail = false;
                            if (words[105] == "1") studentFamilyAddress.FatherIsEmail = true;
                            _studentFamilyAddressRepository.CreateStudentFamilyAddress(studentFamilyAddress);

                            //StudentInvoiceAddress
                            address.StudentInvoiceAddressID = 0;
                            address.StudentID = student.StudentID; ;

                            address.InvoiceTitle = words[167].Trim();
                            address.InvoiceAddress = words[168].Trim();

                            address.InvoiceCityParameterID = 0;
                            if (words[169].Trim() == "İÇEL" || words[169].Trim() == "İçel") words[169] = "MERSİN";
                            if (words[169].Trim() == "AFYON" || words[169].Trim() == "Afyon") words[169] = "AFYONKARAHİSAR";
                            if (words[169].Trim() == "İZMİT" || words[169].Trim() == "İzmit") words[169] = "KOCAELİ";
                            if (words[169].Trim() == "ADAPAZARI" || words[169].Trim() == "Adapazarı") words[169] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[169].Trim());
                            if (isExist && words[169].Trim() != "")
                                address.InvoiceCityParameterID = _parameterRepository.GetParameterCategoryName(words[169].Trim()).CategoryID;

                            address.InvoiceTownParameterID = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[170].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(address.InvoiceCityParameterID, words[170].Trim());
                            if (isExist && isExistSub && words[170].Trim() != "")
                                address.InvoiceTownParameterID = _parameterRepository.GetParameterCategoryName2(address.InvoiceCityParameterID, words[170].Trim()).CategoryID;

                            address.InvoiceCountry = words[171].Trim();

                            address.InvoiceTaxOffice = words[172].Trim();
                            address.InvoiceTaxNumber = words[173].Trim();

                            address.InvoiceZipCode = null;
                            address.EMail = words[0174].Trim();
                            address.Notes = words[175].Trim();
                            address.AccountCode = words[176].Trim();

                            address.InvoiceProfile = true;
                            if (words[177] == "1") address.InvoiceProfile = false;
                            address.InvoiceTypeParameter = 0;
                            if (words[178] == "1") address.InvoiceTypeParameter = 0;

                            address.WebAddress = words[179].Trim();
                            address.Phone = words[180].Trim();
                            address.Fax = words[181].Trim();
                            address.IsInvoiceDetailed = false;
                            address.IsInvoiceDiscount = false;
                            _studentInvoiceAddressRepository.CreateStudentInvoiceAddress(address);


                            //StudentNote
                            studentNote.StudentNoteID = 0;
                            studentNote.StudentID = student.StudentID;
                            studentNote.Note1 = words[081].Trim();
                            studentNote.Note2 = words[082].Trim();
                            studentNote.Note3 = words[083].Trim();
                            _studentNoteRepository.CreateStudentNote(studentNote);

                            //StudentAddress
                            studentAddress.StudentAddressID = 0;
                            studentAddress.StudentID = student.StudentID;
                            studentAddress.Address1 = words[009].Trim();

                            studentAddress.CityParameterID1 = 0;
                            if (words[010].Trim() == "İÇEL" || words[010].Trim() == "İçel") words[010] = "MERSİN";
                            if (words[010].Trim() == "AFYON" || words[010].Trim() == "Afyon") words[010] = "AFYONKARAHİSAR";
                            if (words[010].Trim() == "İZMİT" || words[010].Trim() == "İzmit") words[010] = "KOCAELİ";
                            if (words[010].Trim() == "ADAPAZARI" || words[010].Trim() == "Adapazarı") words[010] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[010].Trim());
                            if (isExist && words[010].Trim() != "")
                                studentAddress.CityParameterID1 = _parameterRepository.GetParameterCategoryName(words[010].Trim()).CategoryID;

                            studentAddress.TownParameterID1 = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[011].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentAddress.CityParameterID1, words[11].Trim());
                            if (isExist && isExistSub && words[011].Trim() != "")
                                studentAddress.TownParameterID1 = _parameterRepository.GetParameterCategoryName2(studentAddress.CityParameterID1, words[011].Trim()).CategoryID;

                            studentAddress.ZipCode1 = words[012].Trim();
                            studentAddress.MobilePhone = words[013].Trim();
                            studentAddress.EMail = words[014].Trim();
                            studentAddress.Address2 = words[086].Trim();

                            studentAddress.CityParameterID2 = 0;
                            if (words[087].Trim() == "İÇEL" || words[087].Trim() == "İçel") words[087] = "MERSİN";
                            if (words[087].Trim() == "AFYON" || words[087].Trim() == "Afyon") words[087] = "AFYONKARAHİSAR";
                            if (words[087].Trim() == "İZMİT" || words[087].Trim() == "İzmit") words[087] = "KOCAELİ";
                            if (words[087].Trim() == "ADAPAZARI" || words[087].Trim() == "Adapazarı") words[087] = "SAKARYA";
                            isExist = _parameterRepository.ExistCategoryName(words[087].Trim());
                            if (isExist && words[087].Trim() != "")
                                studentAddress.CityParameterID2 = _parameterRepository.GetParameterCategoryName(words[087].Trim()).CategoryID;

                            studentAddress.TownParameterID2 = 0;
                            isExist = _parameterRepository.ExistCategoryName(words[088].Trim());
                            isExistSub = _parameterRepository.ExistCategoryNameSub(studentAddress.CityParameterID2, words[088].Trim());
                            if (isExist && isExistSub && words[088].Trim() != "")
                                studentAddress.TownParameterID2 = _parameterRepository.GetParameterCategoryName2(studentAddress.CityParameterID2, words[088].Trim()).CategoryID;

                            studentAddress.ZipCode2 = words[089].Trim();
                            studentAddress.IsSms = false;
                            if (words[098] == "1") studentAddress.IsSms = true;
                            studentAddress.IsEMail = false;
                            if (words[099] == "1") studentAddress.IsEMail = true;
                            _studentAddressRepository.CreateStudentAddress(studentAddress);

                        CONTINUE:;
                        }
                    }
                }
                YY1--;
                YY2--;
                isPrv = true;
            }
            #endregion

            string url = "/Home/Index?userID=" + user.UserID + "&msg=0";
            return Redirect(url);
        }


        #region DateControl
        public string DateControl(int order, string strDate)
        {
            string strDate1 = strDate;

            bool isIntString = strDate.All(char.IsDigit);
            if (isIntString == false) strDate = "";

            int length = strDate.Trim().Length;
            string returnDate = null;

            string txt = strDate.Replace(".", "").Replace("-", "").Replace(" ", ""); strDate = txt;
            if (strDate.Trim() == null || strDate.Trim() == "00000000" || strDate.Trim() == "0000000" || strDate.Trim() == "000000" || strDate.Trim() == "00000" || strDate.Trim() == "0000" || strDate.Trim() == "000" || strDate.Trim() == "00" || strDate.Trim() == "0" ||
                strDate.Trim() == "" || length < 8) strDate = "";
            if (strDate.Trim() != "")
            {
                if (strDate.Substring(0, 4) == "0000" || strDate.Substring(0, 2) == "00") strDate = "";
                else
                    if (strDate.Substring(4, 2) == "00") strDate = "";
                else
                    if (strDate.Substring(6, 2) == "00") strDate = "";
            }

            if (strDate.Trim() != "")
            {
                int year = Convert.ToInt32(strDate.Substring(0, 4));
                int month = Convert.ToInt32(strDate.Substring(4, 2));
                int day = Convert.ToInt32(strDate.Substring(6, 2));

                string newDay = "30";
                if (month > 12)
                {
                    month = 10;
                    strDate = strDate.Remove(4, 2).Insert(4, "10");
                }
                //February 28 or 29 Control
                if (month == 2 && day > 28)
                {
                    newDay = "28";

                    int day1 = year / 4;
                    decimal day2 = Decimal.Divide(year, 4);

                    if (day1 == day2) newDay = "29";
                    strDate = strDate.Remove(6, 2).Insert(6, newDay);
                }
                else
                {
                    if (month == 01 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 03 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 04 && day > 30) { newDay = "30"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 05 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 06 && day > 30) { newDay = "30"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 07 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 08 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 09 && day > 30) { newDay = "30"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 10 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 11 && day > 30) { newDay = "30"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                    if (month == 12 && day > 31) { newDay = "31"; strDate = strDate.Remove(6, 2).Insert(6, newDay); }
                }

                strDate = strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2);

                returnDate = DateTime.Parse(strDate).ToString();
            }

            return returnDate;
        }
        #endregion

        [Route("M999ImportToWeb/PeriodDataRead/{plusYear}")]
        public IActionResult PeriodDataRead(int plusYear)
        {
            DecadeController periodList = new DecadeController();
            var mylist = new List<Parameter>();
            periodList.Decade(mylist, plusYear);
            return Json(mylist);
        }
    }
    #endregion


}
