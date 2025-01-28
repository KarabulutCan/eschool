using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace ESCHOOL.Controllers
{
    public class M220StudentController : Controller
    {

        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentDebtDetailRepository _studentDebtDetailRepository;
        IStudentDebtDetailTableRepository _studentDebtDetailTableRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentInstallmentPaymentRepository _studentInstallmentPaymentRepository;
        ITempDataRepository _tempDataRepository;
        IStudentPaymentRepository _studentPaymentRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IStudentNoteRepository _studentNoteRepository;
        IStudentTempRepository _studentTempRepository;
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
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;

        IUsersRepository _usersRepository;
        IUsersLogRepository _usersLogRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;

        IWebHostEnvironment _hostEnvironment;
        public M220StudentController(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDebtDetailRepository studentDebtDetailRepository,
            IStudentDebtDetailTableRepository studentDebtDetailTableRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            IStudentInstallmentPaymentRepository studentInstallmentPaymentRepository,
            ITempDataRepository tempDataRepository,
            IStudentPaymentRepository studentPaymentRepository,

            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,
            IStudentNoteRepository studentNoteRepository,
            IStudentTempRepository studentTempRepository,
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
            _studentDebtDetailRepository = studentDebtDetailRepository;
            _studentDebtDetailTableRepository = studentDebtDetailTableRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _studentInstallmentPaymentRepository = studentInstallmentPaymentRepository;
            _tempDataRepository = tempDataRepository;
            _discountTableRepository = discountTableRepository;
            _studentDiscountRepository = studentDiscountRepository;
            _studentPaymentRepository = studentPaymentRepository;

            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _studentNoteRepository = studentNoteRepository;
            _studentTempRepository = studentTempRepository;
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

            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountingCodeRepository;

            _usersRepository = usersRepository;
            _usersLogRepository = usersLogRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }
        #region ChangeInStudent
        [HttpGet]
        public IActionResult ChangeInStudent(int studentID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var student = _studentRepository.GetStudent(studentID);
            int schoolID = student.SchoolID;

            var categoryID = _parameterRepository.GetParameterCategoryName("Ücret Değişikliği").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            categoryID = _parameterRepository.GetParameterCategoryName("İndirim İşlemleri").CategoryID;
            bool isPermissionDiscount = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            ViewBag.date = user.UserDate;

            bool isExist2 = false;
            int classroomID = 0;
            string classroomName = "";
            if (school.NewPeriod != user.UserPeriod)
            {
                isExist2 = _studentPeriodsRepository.ExistStudentPeriods(student.SchoolID, student.StudentID, user.UserPeriod);
                if (isExist2)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(student.SchoolID, student.StudentID, user.UserPeriod).ClassroomName;
                    isExist2 = _classroomRepository.ExistClassroomName(user.UserPeriod, classroomName);
                    if (isExist2)
                    { classroomID = _classroomRepository.GetClassroomNamePeriod(user.UserPeriod, classroomName).ClassroomID; }

                    student.ClassroomID = classroomID;
                }
            }

            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            if (studentTemp != null)
            {
                if (studentTemp.Installment == 0)
                {
                    studentTemp.Installment = school.DefaultInstallment;
                    _studentTempRepository.UpdateStudentTemp(studentTemp);
                }
            }
            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            if (pSerialNumber == null)
                pSerialNumber = new PSerialNumber();

            categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(student.SchoolID, student.StudentID, user.UserPeriod);
            // Clean Data
            var tempDataClean = _tempDataRepository.GetTempData(student.StudentID);
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
                tempData.CheckCardNo = item.CheckCardNo;
                tempData.PreviousPayment = item.PreviousPayment;
                tempData.FeeName = item.FeeName;
                tempData.StatusCategoryID = item.StatusCategoryID;
                tempData.AccountReceiptNo = item.AccountReceiptNo;
                if (item.PaymentDate != null)
                    tempData.PaymentDate = (DateTime)item.PaymentDate;
                tempData.CashPayment = studentTemp.CashPayment;

                _tempDataRepository.CreateTempData(tempData);
            }

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);
            TempData["newYear"] = 0;
            if (school.NewPeriod == user.UserPeriod) TempData["newYear"] = 1;
            //var studentPeriod = _studentPeriodsRepository.GetStudentPeriod(schoolID = user.SchoolID, studentID, user.UserPeriod);

            string categoryName1 = "dicountName";
            string categoryName2 = "categoryName";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName1 = "language1";
                categoryName2 = "language1";
            }
            var studentViewModel = new StudentViewModel
            {
                IsPermission = isPermission,
                IsPermissionDiscount = isPermissionDiscount,
                UserID = userID,
                Period = user.UserPeriod,
                SchoolID = schoolID,
                StudentID = studentID,
                Student = student,
                StudentInstallment2 = studentInstallment,
                StudentTemp = studentTemp,
                PSerialNumber = pSerialNumber,
                Parameter = parameter,
                SchoolInfo = school,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName1 = categoryName1,
                CategoryName2 = categoryName2
            };

            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeInStudent(StudentViewModel studentViewModel)
        {
            var user = _usersRepository.GetUser(studentViewModel.UserID);
            var school = _schoolInfoRepository.GetSchoolInfo(studentViewModel.SchoolID);

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();
            var lastNumber = pserialNumbers.AccountSerialNo += 1;
            pserialNumbers.AccountSerialNo = lastNumber;

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;

            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var period = studentViewModel.Period;
            var tempData = _tempDataRepository.GetTempData(studentViewModel.StudentID);
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(studentViewModel.SchoolID, studentViewModel.StudentID, period);
            var sortOrder = 0;
            bool isFirst = true;
            decimal isCash1 = 0;
            decimal isCash2 = 0;
            bool isCashChange = false;

            bool isExist = false;
            string classroomName = "";

            if (school.NewPeriod == user.UserPeriod)
            {
                if (studentViewModel.Student.ClassroomID > 0)
                {
                    classroomName = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;
                }
            }
            else
            {
                isExist = _studentPeriodsRepository.ExistStudentPeriods(studentViewModel.SchoolID, studentViewModel.Student.StudentID, user.UserPeriod);
                if (isExist)
                {
                    classroomName = _studentPeriodsRepository.GetStudentPeriod(studentViewModel.SchoolID, studentViewModel.Student.StudentID, user.UserPeriod).ClassroomName;
                }
            }

            decimal total340 = 0;

            int temdatacount = tempData.Count();
            int count = studentinstallment.Count();
            decimal tempdatatotal = 0;
            decimal total = 0;

            if (tempData != null)
            {
                foreach (var item in tempData)
                {
                    tempdatatotal += item.InstallmentAmount;
                    if (item.CashPayment > 0) isCash1 = item.CashPayment;
                }
            }
            var studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, period, studentViewModel.StudentID);
            if (studentTemp != null)
            {
                isCash2 = studentTemp.CashPayment;
            }

            if (isCash1 != isCash2) isCashChange = true;

            if (studentinstallment != null)
            {
                foreach (var item in studentinstallment)
                {
                    total += item.InstallmentAmount;
                }
            }
            // isEqual
            if (temdatacount != count || tempdatatotal != total)
            {
                if (tempData != null)
                {
                    foreach (var item in tempData)
                    {
                        sortOrder += 1;
                        if (item.InstallmentAmount != item.PreviousPayment)
                        {
                            if (isFirst)
                            {
                                if (isCashChange == true) total340 += item.CashPayment;
                                AccountingCreate1(studentViewModel, school, item, sortOrder, period, classroomName, lastNumber, isCashChange);
                                isFirst = false;
                            }
                            total340 += item.InstallmentAmount - item.PreviousPayment;
                            AccountingCreate2(studentViewModel, school, item, sortOrder, period, classroomName, lastNumber, paymentReceiptNo);
                        }
                    }
                    if (total340 > 0)
                        AccountingCreate3(studentViewModel, school, sortOrder, period, classroomName, total340, lastNumber);
                }
                AccountingCreate4(studentViewModel, school, sortOrder, period, classroomName, lastNumber, isCashChange);

                if (tempData != null)
                {
                    foreach (var item in tempData)
                    {
                        _tempDataRepository.DeleteTempData(item);
                    }
                }
            }

            //////Users Log//////////////////
            //var user = _usersRepository.GetUser(userID);
            var log = new UsersLog();
            log.SchoolID = studentViewModel.SchoolID;
            log.Period = studentViewModel.Period;
            log.UserID = studentViewModel.UserID;
            log.TransactionID = _parameterRepository.GetParameterCategoryName("Öğrenci Değişiklik").CategoryID;
            log.UserLogDate = DateTime.Now;
            string classroom = classroomName;
            studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, studentViewModel.Period, studentViewModel.StudentID);
            decimal cashPayment = 0;
            decimal subTotal = 0;
            int intstallment = 0;
            if (studentTemp != null)
            {
                cashPayment = Math.Round(studentTemp.CashPayment, school.CurrencyDecimalPlaces);
                subTotal = Math.Round(studentTemp.SubTotal, school.CurrencyDecimalPlaces);
                intstallment = studentTemp.Installment;
            }

            log.UserLogDescription = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " Öğrenci Kaydında Değişiklik Yapıldı, " + "Sınıfı:" + classroom + ", Peşinat:" + cashPayment + "," + intstallment + " Ay Vadeli, " + "Toplam:" + subTotal;
            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            return RedirectToAction("AddOrEditStudent", "M210Student", new { studentID = studentViewModel.StudentID, userID = +studentViewModel.UserID });
            //return View(studentViewModel);
        }

        [HttpPost]
        [Route("M220Student/AboutRefund/{schoolID}/{aboutRefund}/{period}/{studentid}")]
        public IActionResult AboutRefund(int schoolID, string aboutRefund, string period, int studentid)
        {
            if (ModelState.IsValid)
            {
                var getCode = _studentTempRepository.GetStudentTemp(schoolID, period, studentid);
                getCode.AboutRefund1 = aboutRefund;
                _studentTempRepository.UpdateStudentTemp(getCode);
            }
            return View();
        }
        #endregion

        #region Accounting
        public void AccountingCreate1(StudentViewModel studentViewModel, SchoolInfo schoolInfo, TempData item, int sortOrder, string period, string classrooms, int lastNumber, bool isCashChange)
        {
            var accounting = new Accounting();
            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            //100 ACOOUNT RECORDS 
            //PaymentNo
            string controlCode = schoolInfo.AccountNoID01;
            if (controlCode == null) controlCode = "100";

            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var bankName = _bankRepository.GetBank(studentViewModel.StudentTemp.BankID).BankName;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolInfo.SchoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentNo.ToString();
            accounting.DocumentDate = DateTime.Today;

            var exp1 = " Nolu Makbuz ";
            var exp2 = " Tarihli Nakit Tediyesi ";
            accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
            if (item.CashPayment > 0 && isCashChange == true)
            {
                accounting.Credit = item.CashPayment;

                accounting.Debt = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }
        }
        public void AccountingCreate2(StudentViewModel studentViewModel, SchoolInfo schoolInfo, TempData item, int sortOrder, string period, string classrooms, int lastNumber, int paymentReceiptNo)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            var accounting = new Accounting();
            string code = null;
            var bankName = _bankRepository.GetBank(studentViewModel.StudentTemp.BankID).BankName;
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string controlCode = schoolInfo.AccountNoID05;
            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string deptCode = schoolInfo.CompanyShortCode;

            var paymentTypetxt = "";

            DateTime dt = Convert.ToDateTime(item.InstallmentDate);
            int DD = dt.Day;
            int MM = dt.Month;
            string YY = dt.ToString("yy");

            var param = parameters.FirstOrDefault(p => p.CategoryID == item.CategoryID);
            //categoryID = param.CategoryID;

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

            string deptText = " Nolu Şb.";
            var exp3 = " Nolu Makbuz, ";
            string periodTxt = " Dönemi, ";
            string monthTxt = " Ayı ";
            string mainCodeName = "ALACAK SENETLERİ";
            var exp4 = " Tarihli " + paymentTypetxt + " Tediyesi ";
            if (studentViewModel.SelectedCulture.Trim() == "en-US")
            {
                deptText = " Branch No.";
                exp3 = "Receipt No, ";
                periodTxt = " Period, ";
                monthTxt = " Month ";
                mainCodeName = "Notes receivable";
                exp4 = " Dated " + paymentTypetxt + " Payment ";
            }


            controlCode = code + " " + periodCode + " " + YY + " " + MM + " " + deptCode;
            if (controlCode == null) controlCode = "121";

            accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

            var codeName = periodCode + " " + periodTxt + " " + YY + "/" + MM + monthTxt + " " + deptCode + " " + deptText + ", " + mainCodeName;
            bool isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            accounting.AccountingID = 0;
            accounting.SchoolID = schoolInfo.SchoolID;
            accounting.Period = period;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode; ;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;

            accounting.Debt = 0;
            accounting.Credit = item.InstallmentAmount;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);
        }
        public void AccountingCreate3(StudentViewModel studentViewModel, SchoolInfo schoolInfo, int sortOrder, string period, string classrooms, decimal total340, int lastNumber)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            var accounting = new Accounting();

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();
            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string deptCode = schoolInfo.CompanyShortCode;
            string studentNo = studentViewModel.Student.StudentNumber;
            string studentName = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + "'ün ";
            string deptName = schoolInfo.CompanyShortName + " Şb. ";

            string periodTxt = " Dönemi, ";
            string codeName = "ALINAN SİPARİŞ AVANSLARI";
            string mainCodeName = codeName;
            string controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";

            string code340 = controlCode;
            controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;
            codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;
            bool isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            //340 ACOOUNT RECORDS 
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolInfo.SchoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            var exp5 = " Tarihinde Öğrenciden Alınanların İptali";
            accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + DateTime.Today.ToString("dd.MM.yyyy") + exp5;
            accounting.Debt = total340;
            accounting.Credit = 0;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);
        }
        public void AccountingCreate4(StudentViewModel studentViewModel, SchoolInfo schoolInfo, int sortOrder, string period, string classrooms, int lastNumber, bool isCashChange)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            var accounting = new Accounting();

            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string year = period.Substring(7, 2);
            string periodTxt = " Dönemi, ";
            string mainCodeName = "";

            string deptCode = schoolInfo.CompanyShortCode;
            string deptName = schoolInfo.CompanyShortName + " Şb. ";
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            string studentNo = studentViewModel.Student.StudentNumber;
            string studentName = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + "'ün ";

            string controlCode = schoolInfo.AccountNoID01;
            if (controlCode == null) controlCode = "100";
            string codeName = "KASA";
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

            //100 ACOOUNT RECORDS 
            //PaymentNo
            controlCode = schoolInfo.AccountNoID01;
            if (controlCode == null) controlCode = "100";

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();
            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;
            decimal cashPayment = 0;
            var studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, period, studentViewModel.Student.StudentID);
            if (studentTemp != null)
            {
                cashPayment = studentTemp.CashPayment;
            }

            var bankName = _bankRepository.GetBank(studentViewModel.StudentTemp.BankID).BankName;
            var exp1 = " ";
            var exp2 = " ";
            decimal total340 = 0;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;
            if (cashPayment > 0 && isCashChange == true)
            {
                total340 += cashPayment;
                accounting.AccountingID = 0;
                accounting.SchoolID = schoolInfo.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = paymentNo.ToString();
                accounting.DocumentDate = DateTime.Today;

                exp1 = " Nolu Makbuz ";
                exp2 = " Tarihli Nakit Tahsili ";
                accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
                accounting.Debt = cashPayment;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }
            //121 ACOOUNT RECORDS 
            controlCode = schoolInfo.AccountNoID05;
            if (controlCode == null) controlCode = "121";

            categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            parameters = _parameterRepository.GetParameterSubID(categoryID);
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(studentViewModel.SchoolID, studentViewModel.Student.StudentID, period);

            foreach (var item in studentinstallment)
            {
                if (item.PreviousPayment == 0)
                {
                    total340 += item.InstallmentAmount;

                    DateTime dt = Convert.ToDateTime(item.InstallmentDate);
                    int DD = dt.Day;
                    int MM = dt.Month;
                    string YY = dt.ToString("yy");
                    string code = null;
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
                    string monthTxt = " Ayı ";
                    string deptText = " Nolu Şb.";
                    mainCodeName = "ALACAK SENETLERİ";
                    var exp4 = " Tarihli " + paymentTypetxt + " Tahsili ";
                    if (studentViewModel.SelectedCulture.Trim() == "en-US")
                    {
                        exp3 = " Receipt No, ";
                        monthTxt = " Month ";
                        deptText = " Branch No.";
                        mainCodeName = "CERTIFICATES CREDIT";
                        exp4 = " Dated " + paymentTypetxt + " Collection ";
                    }

                    controlCode = code + " " + periodCode + " " + YY + " " + MM + " " + deptCode;
                    accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

                    codeName = periodCode + " " + periodTxt + " " + YY + "/" + MM + monthTxt + " " + deptCode + " " + deptText + ", " + mainCodeName;
                    isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                    if (!isExist)
                    {
                        CodeCreate(accountingCode, controlCode, codeName);
                    }

                    sortOrder += 1;
                    accounting.AccountingID = 0;
                    accounting.SchoolID = schoolInfo.SchoolID;
                    accounting.Period = period;
                    //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                    accounting.VoucherTypeID = catID;
                    accounting.VoucherNo = lastNumber;
                    accounting.AccountDate = DateTime.Today;
                    accounting.AccountCode = controlCode;
                    accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                    accounting.CodeTypeName = "";
                    accounting.DocumentNumber = paymentReceiptNo.ToString();
                    accounting.DocumentDate = DateTime.Today;

                    accounting.Debt = item.InstallmentAmount;
                    accounting.Credit = 0;
                    accounting.SortOrder = sortOrder;
                    accounting.IsTransaction = false;
                    _accountingRepository.CreateAccounting(accounting);
                }
            }

            //340 ACOOUNT RECORDS 
            controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";


            periodTxt = " Dönemi, ";
            codeName = "ALINAN SİPARİŞ AVANSLARI";
            mainCodeName = codeName;
            controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";

            string code340 = controlCode;
            controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;
            codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;
            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

            sortOrder += 1;
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolInfo.SchoolID;
            accounting.Period = period;
            //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            var exp5 = " Tarihinde Öğrenciden Alınan Toplam";
            accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + DateTime.Today.ToString("dd.MM.yyyy") + exp5;
            accounting.Debt = 0;
            accounting.Credit = total340;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

            //340-600 ACOOUNT RECORDS 
            if (cashPayment > 0 && isCashChange == true)
            {
                controlCode = schoolInfo.AccountNoID02;
                if (controlCode == null) controlCode = "340";

                periodTxt = " Dönemi, ";
                codeName = "ALINAN SİPARİŞ AVANSLARI";
                mainCodeName = codeName;
                controlCode = schoolInfo.AccountNoID02;
                if (controlCode == null) controlCode = "340";

                code340 = controlCode;
                controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;
                codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                sortOrder += 1;
                accounting.AccountingID = 0;
                accounting.SchoolID = schoolInfo.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = paymentReceiptNo.ToString();
                accounting.DocumentDate = DateTime.Today;
                accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
                accounting.Debt = cashPayment;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }
            //600 ACOOUNT RECORDS 
            if (cashPayment > 0 && isCashChange == true)
            {
                controlCode = schoolInfo.AccountNoID03;
                if (controlCode == null) controlCode = "600";
                string code600 = controlCode;
                periodTxt = " Dönemi, ";
                codeName = "YURT İÇİ SATIŞLAR";
                mainCodeName = codeName;
                controlCode = schoolInfo.AccountNoID02;
                if (controlCode == null) controlCode = "340";

                controlCode = code600 + " " + periodCode + " " + deptCode + " " + "01";
                codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;
                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                sortOrder += 1;
                accounting.AccountingID = 0;
                accounting.SchoolID = schoolInfo.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = paymentReceiptNo.ToString();
                accounting.DocumentDate = DateTime.Today;
                accounting.Explanation = classrooms + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2 + "/" + bankName;
                accounting.Debt = 0;
                accounting.Credit = cashPayment;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
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
        #endregion

        #region Others
        [Route("M220Student/SchoolDebtDataRead/{period}/{userID}/{studentid}")]
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
        [Route("M220Student/SchoolFeeDataTableRead/{period}/{userID}/{classroomID}/{feeID}/{ID}/{unitPrice}")]
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


        [HttpPost]
        [Route("M220Student/SchoolDebtDataUpdate/{strResult}/{userID}/{studentID}/{classroomID}")]
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
        [Route("M220Student/GridFeesUpdate/{userID}/{studentID}/{classroomID}/{schoolFeeID}/{unitPrice}/{discount}/{isList}/{isPriceUpdate}/{isListUpdate}")]
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


        [HttpPost]
        [Route("M220Student/DetailedRefreshDataRead/{userID}/{studentID}")]
        public IActionResult DetailedRefreshDataRead(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

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
                    getCode.InstallmentTable = 10;
                    getCode.PaymentStartDateTable = DateTime.Now; ;

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
                    studentDebtDetailedViewModel.SchoolFeeID = item.SchoolFeeID;
                    studentDebtDetailedViewModel.AmountTable = debt.Amount;
                    studentDebtDetailedViewModel.InstallmentTable = 10;
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

        [Route("M220Student/SchoolDebtDetailedDataRead/{userID}/{studentID}")]
        public IActionResult SchoolDebtDetailedDataRead(int userID, int studentID)
        {
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
                        getCode.InstallmentTable = 10;
                        getCode.PaymentStartDateTable = DateTime.Now; ;

                        _studentDebtDetailTableRepository.CreateStudentDebtDetailTable(getCode);
                        _studentDebtDetailTableRepository.Save();
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
                studentDebtDetailedViewModel.InstallmentTable = 10;
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
                        _studentDebtDetailTableRepository.Save();
                    }
                }
                else
                if (ModelState.IsValid)
                {
                    getCode.StudentDebtTableID = json[i].StudentDebtTableID;
                    getCode.InstallmentTable = json[i].InstallmentTable;
                    getCode.PaymentStartDateTable = json[i].PaymentStartDateTable;

                    _studentDebtDetailTableRepository.UpdateStudentDebtDetailTable(getCode);
                    _studentDebtDetailTableRepository.Save();
                }
                i = i + 1;
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M220Student/DiscountDataRead/{strResult}/{userID}/{studentid}")]
        public IActionResult DiscountDataRead(int strResult, int userID, int studentID)
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
            if (strResult != 0)
            {
                foreach (var item in discounttable)
                {
                    var studentDiscountViewModel = new StudentDiscountViewModel();
                    var dis = _studentDiscountRepository.GetDiscount(studentID, period, item.SchoolID, item.DiscountTableID, strResult);

                    studentDiscountViewModel.StudentID = studentID;
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
            }
            return Json(list);
        }

        [Route("M220Student/DiscountDataUpdate/{strResult}/{unitPrice}")]
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

                i++;
            }

            var discount = _studentDiscountRepository.GetDiscountID(json[0].StudentDebtID);
            foreach (var item in discount)
            {
                //total += Convert.ToInt32(json[i].DiscountApplied);
                total += item.DiscountApplied;
            }

            var getCode = _studentDebtRepository.GetStudentDebtID(json[0].SchoolID, json[0].StudentDebtID);
            if (getCode != null)
            {
                getCode.UnitPrice = unitPrice;
                getCode.Amount = unitPrice - total;
                getCode.Discount = total;
                getCode.ClassroomTypeID = typeId;
                _studentDebtRepository.UpdateStudentDebt(getCode);
                //_studentDebtRepository.Save();
            }
            return Json(list);
        }

        //Installment
        [Route("M220Student/InstallmentDataRead/{period}/{userID}/{studentid}/{isFilter}")]
        public IActionResult InstallmentDataRead(string period, int userID, int studentid, bool isFilter)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameterList2 = _parameterRepository.GetParameterSubID(categoryID2);
            var categoryID3 = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var parameterList3 = _parameterRepository.GetParameterSubID(categoryID3);

            var bankList = _bankRepository.GetBankAll(user.SchoolID);
            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentid, user.UserPeriod);

            List<StudentViewModel> list = new List<StudentViewModel>();

            foreach (var item in studentinstallment)
            {
                var parameter2 = parameterList2.FirstOrDefault(p => p.CategoryID == item.CategoryID);
                var parameter3 = parameterList3.FirstOrDefault(p => p.CategoryID == item.StatusCategoryID);

                if (parameter2 == null)
                {
                    parameter2 = parameterList2.FirstOrDefault(p => p.CategorySubID == categoryID2);
                }
                if (parameter3 == null)
                {
                    parameter3 = parameterList3.FirstOrDefault(p => p.CategorySubID == categoryID3);
                }

                var bank = bankList.FirstOrDefault(p => p.BankID == item.BankID);
                if (item.InstallmentAmount == 0) 
                    _studentInstallmentRepository.DeleteStudentInstallment(item);
                else
                {
                    var studentViewModel = new StudentViewModel
                    {
                        ViewModelID = item.StudentInstallmentID,
                        //Student = student,
                        StudentInstallment = item,
                        Parameter2 = parameter2,
                        Parameter3 = parameter3,
                        Bank = bank,
                    };

                    //list.Add(studentViewModel);
                    if (isFilter == false)
                        list.Add(studentViewModel);
                    else
                        if (item.PreviousPayment == 0)
                        list.Add(studentViewModel);
                }
            }

            return Json(list);
        }

        [Route("M220Student/StudentInstallmentDataReadFilter/{period}/{userID}/{studentid}")]
        public IActionResult StudentInstallmentDataReadFilter(string period, int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameterList2 = _parameterRepository.GetParameterSubID(categoryID2);
            var categoryID3 = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var parameterList3 = _parameterRepository.GetParameterSubID(categoryID3);

            var bankList = _bankRepository.GetBankAll(user.SchoolID);
            var installment = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentid, period);

            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in installment)
            {
                var parameter2 = parameterList2.FirstOrDefault(p => p.CategoryID == item.CategoryID);
                var parameter3 = parameterList3.FirstOrDefault(p => p.CategoryID == item.StatusCategoryID);

                if (parameter2 == null)
                {
                    parameter2 = parameterList2.FirstOrDefault(p => p.CategorySubID == categoryID2);
                }
                if (parameter3 == null)
                {
                    parameter3 = parameterList3.FirstOrDefault(p => p.CategorySubID == categoryID3);
                }

                var bank = bankList.FirstOrDefault(p => p.BankID == item.BankID);

                var studentViewModel = new StudentViewModel
                {
                    ViewModelID = item.StudentInstallmentID,
                    StudentInstallment = item,
                    Parameter2 = parameter2,
                    Parameter3 = parameter3,
                    Bank = bank,
                    SchoolInfo = schoolInfo,
                };

                list.Add(studentViewModel);
            }
            return Json(list);
        }

        [Route("M220Student/CalculateInstallment/{installment}/{singlepaper}/{cashpayment}/{subtotal}/{dateString}/{paymenttypecomboBox}/{banknamecomboBox}/{studentid}/{userID}/{byAmount}/{pressbuttonindex}")]
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
            var studentpayments = _studentPaymentRepository.GetStudentPayment(user.SchoolID, period, studentid);

            if (studentinstallments != null)
            {
                foreach (var item in studentinstallments)
                {
                    if (item.PreviousPayment > 0)
                    {
                        //mehmet
                        item.InstallmentAmount = item.PreviousPayment;
                        //item.PreviousPayment = item.InstallmentAmount;

                        var catID = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
                        var position = _parameterRepository.GetParameterSubIDOnlyTrue2(catID);
                        var statusID = position.Where(b => b.CategoryName == "Tahsil").FirstOrDefault().CategoryID;

                        item.StatusCategoryID = statusID;
                        _studentInstallmentRepository.UpdateStudentInstallment(item);

                        nonDeleted += Convert.ToInt32(item.InstallmentAmount);

                        foreach (var payment in studentpayments)
                        {
                            payment.BalanceAmount = 0;
                            payment.ReceiptNo = 123;
                            _studentPaymentRepository.UpdateStudentPayment(payment);
                        }
                    }
                    else
                    {
                        _studentInstallmentRepository.DeleteStudentInstallment(item);
                    }
                }
            }

            //totalInstallment = (totalDebt - cashpayment) - nonDeleted;
            totalInstallment = (totalDebt - cashpayment);

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
                        inst.InstallmentDate = transactiondate.AddDays(periodOfTime);
                        transactiondate = transactiondate.AddDays(periodOfTime);
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
        [Route("M220Student/InstallmentDataUpdate/{strResult}")]
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
                i++;
            }

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameterList2 = _parameterRepository.GetParameterSubID(categoryID2);
            var categoryID3 = _parameterRepository.GetParameterCategoryName("Çek / Senet Pozisyonları").CategoryID;
            var parameterList3 = _parameterRepository.GetParameterSubID(categoryID3);

            var bankList = _bankRepository.GetBankAll(json[0].StudentInstallment.SchoolID);
            var bank = bankList.FirstOrDefault(p => p.BankID == json[0].StudentInstallment.BankID);

            var parameter2 = parameterList2.FirstOrDefault(p => p.CategoryID == json[0].StudentInstallment.CategoryID);
            var parameter3 = parameterList3.FirstOrDefault(p => p.CategoryID == json[0].StudentInstallment.StatusCategoryID);

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
                SchoolID = installment.StudentInstallmentID,
                StudentInstallment = installment,
                Parameter2 = parameter2,
                Parameter3 = parameter3,
                Bank = bank,
            };
            return Json(studentViewModel);
        }

        [Route("M220Student/PaymentsDataRead/{userID}/{studentID}")]
        public IActionResult PaymentsDataRead(int userID, int studentID)
        {
            decimal totaldebt = 0, totalpaid = 0, cashPayment = 0, paid = 0, remainingDebit = 0;

            var user = _usersRepository.GetUser(userID);
            string culture = user.SelectedCulture.Trim();
            var studentTemp = _studentTempRepository.GetStudentTemp(user.SchoolID, user.UserPeriod, studentID);
            var period = user.UserPeriod;

            var studentInstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, period);

            decimal refund = 0;
            if (studentTemp != null)
            {
                cashPayment = studentTemp.CashPayment;
                refund = studentTemp.RefundAmount1 + studentTemp.RefundAmount2 + studentTemp.RefundAmount3;
                totaldebt += studentTemp.CashPayment;
            }
            foreach (var item in studentInstallment)
            {
                if (item.PreviousPayment > 0) paid += item.PreviousPayment;
                totaldebt += item.InstallmentAmount;
            }
            totalpaid = cashPayment + paid;
            remainingDebit = totaldebt - totalpaid;
            if (refund > 0)
            {
                totaldebt -= refund;
            }
            string[] dim1;
            if (culture == "tr-TR")
            {
                dim1 = new string[] { "Borç Tutarı", "İade Edilen Tutar", "Peşin Ödenen Tutar", "Ödenen Taksitler", "Toplam Ödenen Tutar ", "Kalan Borç Tutarı " };
            }
            else
            {
                dim1 = new string[] { "Debt Amount", "Refund", "Prepaid Amount", "Paid Installments", "Total Paid Amount", "Remaining Debt Amount" };
            }

            decimal[] dim2 = new decimal[] { totaldebt, refund, cashPayment, paid, totalpaid, remainingDebit };

            List<StudentPaymentInfo> list = new List<StudentPaymentInfo>();

            var inx = 0;
            for (int i = 0; i < 6; i++)
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

        [Route("M220Student/SerialNumbersDataRead/{userID}")]
        public IActionResult SerialNumbersDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            string culture = user.SelectedCulture.Trim();

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            var serialNumbers = _pSerialNumberRepository.GetPSerialNumber(user.SchoolID);
            if (serialNumbers == null)
                serialNumbers = new PSerialNumber();

            string[] dim1;
            if (culture == "tr-TR")
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
        public IActionResult PaymentTypeDataRead()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var paymentType = _parameterRepository.GetParameterSubID(categoryID);
            return Json(paymentType);
        }

        [Route("M220Student/GetNumberDataRead/{schoolID}/{categoryID}")]
        public IActionResult GetNumberDataRead(int schoolID, int categoryID)
        {
            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(schoolID);
            var number = 0;

            var parameters = _parameterRepository.GetParameter(categoryID);
            //var param = parameters.FirstOrDefault(p => p.CategoryID == categoryID);
            var categoryName = parameters.CategoryName;

            if (categoryName == "Banka" || categoryName == "BANKA") number = pSerialNumber.BondNo += 1;
            if (categoryName == "Çek" || categoryName == "ÇEK") number = pSerialNumber.CheckNo += 1;
            if (categoryName == "Elden" || categoryName == "ELDEN") number = pSerialNumber.BondNo += 1;
            if (categoryName == "Kmh" || categoryName == "KMH") number = pSerialNumber.KmhNo += 1;
            if (categoryName == "Kredi kartı" || categoryName == "KREDİ KARTI") number = pSerialNumber.CreditCardNo += 1;
            if (categoryName == "Mail order" || categoryName == "MAİL ORDER") number = pSerialNumber.MailOrderNo += 1;
            if (categoryName == "Ots_1" || categoryName == "OTS_1") number = pSerialNumber.OtsNo1 += 1;
            if (categoryName == "Ots_2" || categoryName == "OTS_2") number = pSerialNumber.OtsNo2 += 1;
            if (categoryName == "Teşvik" || categoryName == "TEŞVİK") number = pSerialNumber.GovernmentPromotionNo += 1;

            _pSerialNumberRepository.UpdatePSerialNumber(pSerialNumber);
            return Json(new { lastnumber = number });
        }

        [HttpGet]
        [Route("M220Student/BankTypeDataRead/{userID}")]
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

        [Route("M220Student/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }

        [Route("M220Student/SchoolCombo")]
        public IActionResult SchoolCombo()
        {
            var schoolInfo = _schoolInfoRepository.GetSchoolInfoAllTrue();
            return Json(schoolInfo);
        }
        public IActionResult PaymentTypeCombo()
        {
            var registrationType = _parameterRepository.GetParameterSubID(5);
            return Json(registrationType);
        }

        public IActionResult BankNameCombo(int userSchoolID)
        {
            var bankNameType = _bankRepository.GetBankAll(userSchoolID);
            return Json(bankNameType);
        }

        [Route("M220Student/TempDataDelete/{studentID}")]
        public IActionResult TempDataDelete(int studentID)
        {
            var tempData = _tempDataRepository.GetTempData(studentID);
            foreach (var item in tempData)
            {
                _tempDataRepository.DeleteTempData(item);
            }

            return Json(true);
        }
        #endregion

        #region DetailFee 

        [Route("M220Student/SchoolFeeDetailRead/{userID}/{schoolFeeID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeDetailRead(int userID, int schoolFeeID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(user.SchoolID, L).Where(b => b.FeeCategory == 1);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID2(user.SchoolID, period, categoryID, schoolFeeID).Where(b => b.FeeCategory == 1);

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

        [Route("M220Student/SchoolFeeMoreDetailRead1/{userID}/{studentID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeMoreDetailRead1(int userID, int studentID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(user.SchoolID, L);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, period, categoryID);

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

        [Route("M220Student/SchoolFeeMoreDetailRead2/{userID}/{studentID}/{schoolFeeID}/{studentDebtID}/{L}")]
        public IActionResult SchoolFeeMoreDetailRead2(int userID, int studentID, int schoolFeeID, int studentDebtID, string L)
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
        [Route("M220Student/SchoolFeeMoreDetailUpdate/{strResult}/{userID}/{L}/{schoolFeeID}/{schoolFeeIDMore}/{categoryID}/{studentID}")]
        public IActionResult SchoolFeeMoreDetailUpdate([Bind(Prefix = "models")] string strResult, int userID, string L, int schoolFeeID, int schoolFeeIDMore, int categoryID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);

            var ID = 0;
            decimal total = 0;
            var i = 0;
            //if (L == "L3") ID = schoolFeeID;
            //else ID = schoolFeeIDMore;
            //SchoolFee subControl = new SchoolFee();

            ID = schoolFeeID;
            foreach (var d in json)
            {
                //If the level input has changed
                if (L == "L2")
                {
                    var subControl = _schoolFeeRepository.GetSchoolFeeSubControl(user.SchoolID, schoolFeeIDMore, "L2");
                    foreach (var c in subControl)
                    {
                        var studentDebtDetailChange = _studentDebtDetailRepository.GetStudentDebtDetailID1(json[i].SchoolID, studentID, c.SchoolFeeID);
                        foreach (var item in studentDebtDetailChange)
                        {
                            _studentDebtDetailRepository.DeleteStudentDebtDetail(item);
                        }
                    }
                }

                var studentDebtDetail = _studentDebtDetailRepository.GetStudentDebtDetailID1(json[i].SchoolID, studentID, schoolFeeIDMore);
                foreach (var item in studentDebtDetail)
                {
                    _studentDebtDetailRepository.DeleteStudentDebtDetail(item);
                }

                studentDebtDetail = _studentDebtDetailRepository.GetStudentDebtDetailID1(json[i].SchoolID, studentID, ID);
                foreach (var item in studentDebtDetail)
                {
                    _studentDebtDetailRepository.DeleteStudentDebtDetail(item);
                }
                break;
            }


            i = 0;
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
                total += json[i].SchoolFeeTypeAmount;
                i = i + 1;
            }

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
                else studentDebt.SchoolFeeID = ID;
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
    }

}

