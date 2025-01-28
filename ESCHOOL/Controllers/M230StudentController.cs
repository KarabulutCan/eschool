using DocumentFormat.OpenXml.Spreadsheet;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Controllers
{
    public class M230StudentController : Controller
    {

        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentDebtRepository _studentDebtRepository;
        IStudentDebtDetailTableRepository _studentDebtDetailTableRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        ITempDataRepository _tempDataRepository;
        IStudentPaymentRepository _studentPaymentRepository;

        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IStudentNoteRepository _studentNoteRepository;
        IStudentTempRepository _studentTempRepository;
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
        IUsersRepository _usersRepository;
        IUsersLogRepository _usersLogRepository;
        IAccountingRepository _accountingRepository;
        IAccountCodesRepository _accountCodesRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;

        IWebHostEnvironment _hostEnvironment;
        public M230StudentController(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentDebtRepository studentDebtRepository,
            IStudentDebtDetailTableRepository studentDebtDetailTableRepository,
            IStudentInstallmentRepository studentInstallmentRepository,
            ITempDataRepository tempDataRepository,
            IStudentPaymentRepository studentPaymentRepository,

            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,
            IStudentNoteRepository studentNoteRepository,
            IStudentTempRepository studentTempRepository,
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
            IUsersRepository usersRepository,
            IUsersLogRepository usersLogRepository,
            IAccountingRepository accountingRepository,
            IAccountCodesRepository accountingCodeRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _studentDebtRepository = studentDebtRepository;
            _studentDebtDetailTableRepository = studentDebtDetailTableRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _tempDataRepository = tempDataRepository;
            _discountTableRepository = discountTableRepository;
            _studentDiscountRepository = studentDiscountRepository;
            _studentPaymentRepository = studentPaymentRepository;

            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _studentNoteRepository = studentNoteRepository;
            _studentTempRepository = studentTempRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _bankRepository = bankRepository;
            _classroomRepository = classroomRepository;
            _pSerialNumberRepository = pSerialNumberRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _parameterRepository = parameterRepository;
            _schoolBusServicesRepository = schoolBusServicesRepository;
            _usersRepository = usersRepository;
            _usersLogRepository = usersLogRepository;
            _accountingRepository = accountingRepository;
            _accountCodesRepository = accountingCodeRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _hostEnvironment = hostEnvironment;
        }
        #region Refund
        [HttpGet]
        public IActionResult Refund(int studentID, int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var student = _studentRepository.GetStudent(studentID);
          
            var categoryID = _parameterRepository.GetParameterCategoryName("Ücret Değişikliği").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

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
            if (studentTemp == null)
            {
                studentTemp = new StudentTemp();
                studentTemp.IsCancel = false;
                studentTemp.IsCancelPreviousRefund = false;
                studentTemp.IsSingleNamePaper = false;
                _studentTempRepository.CreateStudentTemp(studentTemp);
            }
            else
            if (studentTemp.Installment == 0)
            {
                studentTemp.Installment = school.DefaultInstallment;
                _studentTempRepository.UpdateStudentTemp(studentTemp);
            }

            var pSerialNumber = _pSerialNumberRepository.GetPSerialNumber(student.SchoolID);
            if (pSerialNumber == null)
                pSerialNumber = new PSerialNumber();

            categoryID = _parameterRepository.GetParameterCategoryName("Entegratörler").CategoryID;
            var parameter = _parameterRepository.GetParameterSubID(categoryID);

            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(user.SchoolID, studentID, user.UserPeriod);
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

            foreach (var item in studentinstallment)
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
                Period = user.UserPeriod,
                SchoolID = user.SchoolID,
                StudentID = studentID,
                Student = student,
                StudentTemp = studentTemp,
                PSerialNumber = pSerialNumber,
                Parameter = parameter,
                SchoolInfo = school,
                SelectedCulture = user.SelectedCulture.Trim(),
            };
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Refund(StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid)
                ViewBag.IsSuccess = true;

            var period = studentViewModel.Period;
            var school = _schoolInfoRepository.GetSchoolInfo(studentViewModel.Student.SchoolID);
            var getCode = _studentTempRepository.GetStudentTemp(studentViewModel.Student.SchoolID, period, studentViewModel.Student.StudentID);
            string classroomName = "";
            if (ModelState.IsValid)
            {
                getCode.RefundSW = "N";
                if (studentViewModel.StudentTemp.IsCancelPreviousRefund == true)
                {
                    getCode.RefundAmount1 = studentViewModel.StudentTemp.RefundAmount1;
                    getCode.RefundAmount2 = studentViewModel.StudentTemp.RefundAmount2;
                    getCode.RefundAmount3 = studentViewModel.StudentTemp.RefundAmount3;
                }
                else
                {
                    getCode.RefundAmount1 += studentViewModel.StudentTemp.RefundAmount1;
                    getCode.RefundAmount2 += studentViewModel.StudentTemp.RefundAmount2;
                    getCode.RefundAmount3 += studentViewModel.StudentTemp.RefundAmount3;
                }
                getCode.DebitAccountCodeID = studentViewModel.StudentTemp.DebitAccountCodeID;
                getCode.RefundAccountCodeID1 = studentViewModel.StudentTemp.RefundAccountCodeID1;
                getCode.RefundAccountCodeID2 = studentViewModel.StudentTemp.RefundAccountCodeID2;
                getCode.RefundAccountCodeID3 = studentViewModel.StudentTemp.RefundAccountCodeID3;
                getCode.AboutRefund2 = studentViewModel.StudentTemp.AboutRefund2;
                getCode.IsCancel = studentViewModel.StudentTemp.IsCancel;
                getCode.IsCancelPreviousRefund = studentViewModel.StudentTemp.IsCancelPreviousRefund;

                ViewBag.IsSuccess = false;
                _studentTempRepository.UpdateStudentTemp(getCode);

                //Debt Records will Delete
                var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(school.SchoolID, studentViewModel.Student.StudentID, period).ToList();
                foreach (var item in studentinstallment)
                {
                    if (item.PreviousPayment > 0)
                    {
                        if (item.InstallmentAmount != item.PreviousPayment)
                        {
                            item.InstallmentAmount = item.PreviousPayment;
                            item.StatusCategoryID = _parameterRepository.GetParameterCategoryName("Tahsil").CategoryID;
                            _studentInstallmentRepository.UpdateStudentInstallment(item);
                        }
                    }
                    else
                    {
                        _studentInstallmentRepository.DeleteStudentInstallment(item);
                    }
                }

                ///////////////////////////// ACCOUNTING ////////////////////////////////////
                var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.Student.SchoolID);
                if (pserialNumbers == null)
                    pserialNumbers = new PSerialNumber();
                var lastNumber = pserialNumbers.AccountSerialNo += 1;
                pserialNumbers.AccountSerialNo = lastNumber;
                _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);


                var tempData = _tempDataRepository.GetTempData(studentViewModel.Student.StudentID);
                var classroom = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID);

                classroomName = classroom.ClassroomName;
                int temdatacount = tempData.Count();
                int count = studentinstallment.Count();
                decimal total340 = 0;
                var sortOrder = 0;

                decimal isCash1 = 0;
                decimal isCash2 = 0; decimal tempdatatotal = 0;
                decimal total = 0;

                if (tempData != null)
                {
                    foreach (var item in tempData)
                    {
                        tempdatatotal += item.InstallmentAmount;
                        if (item.CashPayment > 0) isCash1 = item.CashPayment;
                    }
                }
                isCash2 = _studentTempRepository.GetStudentTemp(studentViewModel.Student.SchoolID, period, studentViewModel.Student.StudentID).CashPayment;

                if (studentinstallment != null)
                {
                    foreach (var item in studentinstallment)
                    {
                        if (item.PreviousPayment > 0)
                        {
                            if (item.InstallmentAmount != item.PreviousPayment)
                            {
                                total += item.InstallmentAmount - item.PreviousPayment;
                            }
                        }
                    }
                }
                // isEqual
                if (temdatacount != count || tempdatatotal != total)
                {
                    if (tempData != null)
                    {
                        foreach (var item in tempData)
                        {
                            if (item.InstallmentAmount != item.PreviousPayment)
                            {
                                sortOrder += 1;
                                total340 += item.InstallmentAmount - item.PreviousPayment;
                                AccountingCreate2(studentViewModel, school, item, sortOrder, period, classroomName, lastNumber);
                            }
                        }
                        if (total340 > 0)
                        {
                            sortOrder += 1;
                            AccountingCreate3(studentViewModel, school, sortOrder, period, classroomName, total340, lastNumber);
                        }
                    }
                    sortOrder = 1;
                    AccountingCreate4(studentViewModel, school, sortOrder, period, classroomName);

                    if (tempData != null)
                    {
                        foreach (var item in tempData)
                        {
                            _tempDataRepository.DeleteTempData(item);
                        }
                    }

                }
            }

            //////Users Log//////////////////
            var log = new UsersLog();
            log.SchoolID = studentViewModel.SchoolID;
            log.Period = studentViewModel.Period;
            log.UserID = studentViewModel.UserID;
            log.TransactionID = _parameterRepository.GetParameterCategoryName("Öğrenci İade").CategoryID;
            log.UserLogDate = DateTime.Now;
            classroomName = _classroomRepository.GetClassroomID(studentViewModel.Student.ClassroomID).ClassroomName;
            var studentTemp = _studentTempRepository.GetStudentTemp(studentViewModel.SchoolID, studentViewModel.Period, studentViewModel.StudentID);
            decimal cashPayment = Math.Round(studentTemp.CashPayment, school.CurrencyDecimalPlaces);
            decimal subTotal = Math.Round(studentTemp.SubTotal, school.CurrencyDecimalPlaces);
            int intstallment = studentTemp.Installment;
            decimal refund = Math.Round(studentTemp.RefundAmount1 + getCode.RefundAmount2 + getCode.RefundAmount3, school.CurrencyDecimalPlaces);

            log.UserLogDescription = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " Öğrenci Kaydında İade İşlemleri Yapıldı, " + "Sınıfı:" + classroomName + ", Peşinat:" + cashPayment + "," + intstallment + " Ay Vadeli, " + "Toplam:" + subTotal + ", İade Edilen Tutar:" + refund;
            _usersLogRepository.CreateUsersLog(log);
            ///////////////////////////////////

            if (studentViewModel.StudentTemp.IsCancel == true)
                return RedirectToAction("CancellationProcedures", "M250Student", new { studentID = studentViewModel.Student.StudentID, userID = studentViewModel.UserID });
            else
                return RedirectToAction("AddOrEditStudent", "M210Student", new { studentID = studentViewModel.Student.StudentID, userID = studentViewModel.UserID });
        }

        [HttpPost]
        [Route("M230Student/AboutRefund/{schoolID}/{aboutRefund}/{period}/{studentid}")]
        public IActionResult AboutRefund(int schoolID, string aboutRefund, string period, int studentid)
        {
            if (ModelState.IsValid)
            {
                var getCode = _studentTempRepository.GetStudentTemp(schoolID, period, studentid);
                getCode.AboutRefund2 = aboutRefund;
                _studentTempRepository.UpdateStudentTemp(getCode);
            }
            return View();
        }
        #endregion

        #region Accounting
        public void AccountingCreate2(StudentViewModel studentViewModel, SchoolInfo schoolInfo, TempData item, int sortOrder, string period, string classroomName, int lastNumber)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            var accounting = new Accounting();
            DateTime dt = Convert.ToDateTime(item.InstallmentDate);
            int DD = dt.Day;
            int MM = dt.Month;
            string YY = dt.ToString("yy");

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.Student.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);

            string controlCode = schoolInfo.AccountNoID05;
            if (controlCode == null) controlCode = "121";

            var paymentTypetxt = "";
            string code = null;

            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string deptCode = schoolInfo.CompanyShortCode;

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
            string periodTxt = " Dönemi, ";
            string monthTxt = " Ayı ";
            string deptText = " Nolu Şb.";
            var exp3 = " Nolu Makbuz, ";
            string mainCodeName = "ALACAK SENETLERİ";
            var exp4 = " Tarihli " + paymentTypetxt + " Tediyesi ";
            if (studentViewModel.SelectedCulture.Trim() == "en-US")
            {
                periodTxt = " Period, ";
                monthTxt = " Month ";
                deptText = " Branch No.";
                exp3 = " Receipt No, ";
                mainCodeName = "CERTIFICATES CREDIT";
                exp4 = " Dated " + paymentTypetxt + " Payment ";
            }

            controlCode = code + " " + periodCode + " " + YY + " " + MM + " " + deptCode;
            //PaymentNo
            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var bankName = _bankRepository.GetBank(studentViewModel.StudentTemp.BankID).BankName;

            accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentReceiptNo + exp3 + DD + "/" + MM + "/" + YY + exp4 + "/" + bankName;

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
            accounting.AccountCode = controlCode;
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
        public void AccountingCreate3(StudentViewModel studentViewModel, SchoolInfo schoolInfo, int sortOrder, string period, string classroomName, decimal total340, int lastNumber)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            var accounting = new Accounting();
            string studentNo = studentViewModel.Student.StudentNumber;
            string studentName = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + "'ün ";
            string deptName = schoolInfo.CompanyShortName + " Şb. ";

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.Student.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            //PaymentNo
            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            //340 ACOOUNT RECORDS 
            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string deptCode = schoolInfo.CompanyShortCode;
            string periodTxt = " Dönemi, ";
            string controlCode = schoolInfo.AccountNoID02;
            if (controlCode == null) controlCode = "340";
            string codeName = "ALINAN SİPARİŞ AVANSLARI";
            string mainCodeName = codeName;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;

            string code340 = controlCode;
            controlCode = code340 + " " + periodCode + " " + deptCode + " " + studentNo;
            codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;

            bool isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }

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
            accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + DateTime.Today.ToString("dd.MM.yyyy") + exp5;
            accounting.Debt = total340;
            accounting.Credit = 0;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);
        }
        public void AccountingCreate4(StudentViewModel studentViewModel, SchoolInfo schoolInfo, int sortOrder, string period, string classroomName)
        {
            var accountingCode = new AccountCodes();
            accountingCode.Period = period;
            string studentNo = studentViewModel.Student.StudentNumber;
            string studentName = studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + "'ün ";
            string periodCode = period.Substring(2, 2) + period.Substring(7, 2);
            string periodTxt = " Dönemi, ";
            string deptName = schoolInfo.CompanyShortName + " Şb. ";
            string deptCode = schoolInfo.CompanyShortCode;

            string controlCode = studentViewModel.StudentTemp.RefundAccountCodeID1;
            if (controlCode == null) controlCode = "100";
            string controlCode100 = controlCode;
            string codeName = "KASA";
            bool isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
            else
            {
                controlCode = _accountCodesRepository.GetAccountCode(schoolInfo.RefundAccountNoID1, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(schoolInfo.RefundAccountNoID1, period).AccountCodeName;
            }

            ///////////////////////////  610 ////////////////////////////////////////
            controlCode = studentViewModel.StudentTemp.DebitAccountCodeID;
            if (controlCode == null) controlCode = "610";
            codeName = "SATIŞTAN İADELER";
            string code610 = controlCode;
            string mainCodeName = codeName;
            controlCode = code610 + " " + periodCode + " " + deptCode + " " + studentNo;
            codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;

            isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
            if (!isExist)
            {
                CodeCreate(accountingCode, controlCode, codeName);
            }
            else
            {
                controlCode = _accountCodesRepository.GetAccountCode(schoolInfo.RefundDebtAccountID, period).AccountCode;
                codeName = _accountCodesRepository.GetAccountCode(schoolInfo.RefundDebtAccountID, period).AccountCodeName;
            }

            var studentinstallment = _studentInstallmentRepository.GetStudentInstallment(schoolInfo.SchoolID, studentViewModel.Student.StudentID, period);

            var accounting = new Accounting();

            var pserialNumbers = _pSerialNumberRepository.GetPSerialNumber(studentViewModel.Student.SchoolID);
            if (pserialNumbers == null)
                pserialNumbers = new PSerialNumber();

            //100 ACOOUNT RECORDS 
            var lastNumber = pserialNumbers.AccountSerialNo += 1;
            pserialNumbers.AccountSerialNo = lastNumber;

            //PaymentNo
            var paymentNo = pserialNumbers.PaymentNo += 1;
            pserialNumbers.PaymentNo = paymentNo;

            var paymentReceiptNo = pserialNumbers.PaymentReceiptNo += 1;
            pserialNumbers.PaymentReceiptNo = paymentReceiptNo;
            _pSerialNumberRepository.UpdatePSerialNumber(pserialNumbers);

            var cashPayment = _studentTempRepository.GetStudentTemp(studentViewModel.Student.SchoolID, period, studentViewModel.Student.StudentID).CashPayment;
            var bankName = _bankRepository.GetBank(studentViewModel.StudentTemp.BankID).BankName;
            var exp1 = " ";
            var exp2 = " ";
            decimal total340 = 0;
            var catID = _parameterRepository.GetParameterCategoryName("Mahsup").CategoryID;

            //610 ACOOUNT RECORDS 1
            if (studentViewModel.StudentTemp.RefundAmount1 > 0)
            {
                total340 += studentViewModel.StudentTemp.RefundAmount1;
                controlCode = code610 + " " + periodCode + " " + deptCode + " " + studentNo;
                codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;

                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                accounting.AccountingID = 0;
                accounting.SchoolID = schoolInfo.SchoolID;
                accounting.Period = period;
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = paymentNo.ToString();
                accounting.DocumentDate = DateTime.Today;

                exp1 = " Nolu Makbuz ";
                exp2 = " Tarihli Öğrenci İade Dekontu.";
                accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2;
                accounting.Debt = studentViewModel.StudentTemp.RefundAmount1;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }
            //610 ACOOUNT RECORDS 2
            if (studentViewModel.StudentTemp.RefundAmount2 > 0)
            {
                sortOrder += 1;
                total340 += studentViewModel.StudentTemp.RefundAmount2;

                controlCode = code610 + " " + periodCode + " " + deptCode + " " + studentNo;
                codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;

                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                accounting.AccountingID = 0;
                accounting.SchoolID = schoolInfo.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = paymentNo.ToString();
                accounting.DocumentDate = DateTime.Today;

                exp1 = " Nolu Makbuz ";
                exp2 = " Tarihli Öğrenci İade Dekontu.";
                accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2;
                accounting.Debt = studentViewModel.StudentTemp.RefundAmount2;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }
            //610 ACOOUNT RECORDS 3
            if (studentViewModel.StudentTemp.RefundAmount3 > 0)
            {
                sortOrder += 1;
                total340 += studentViewModel.StudentTemp.RefundAmount3;

                controlCode = code610 + " " + periodCode + " " + deptCode + " " + studentNo;
                codeName = studentName + periodCode + periodTxt + deptName + mainCodeName;

                isExist = _accountCodesRepository.ExistAccountCode(period, controlCode);
                if (!isExist)
                {
                    CodeCreate(accountingCode, controlCode, codeName);
                }

                accounting.AccountingID = 0;
                accounting.SchoolID = schoolInfo.SchoolID;
                accounting.Period = period;
                //Parameter TABLE da 21 Nolu Bölüm İngilizce için de düzenlenecek
                accounting.VoucherTypeID = catID;
                accounting.VoucherNo = (int)lastNumber;
                accounting.AccountDate = DateTime.Today;
                accounting.AccountCode = controlCode;
                accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
                accounting.CodeTypeName = "";
                accounting.DocumentNumber = paymentNo.ToString();
                accounting.DocumentDate = DateTime.Today;

                exp1 = " Nolu Makbuz ";
                exp2 = " Tarihli Öğrenci İade Dekontu.";
                accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2;
                accounting.Debt = studentViewModel.StudentTemp.RefundAmount3;
                accounting.Credit = 0;
                accounting.SortOrder = sortOrder;
                accounting.IsTransaction = false;
                _accountingRepository.CreateAccounting(accounting);
            }

            //100 ACOOUNT RECORDS 
            sortOrder += 1;
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolInfo.SchoolID;
            accounting.Period = period;

            accounting.VoucherTypeID = catID;
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode100;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode100, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentNo.ToString();
            accounting.DocumentDate = DateTime.Today;

            exp1 = " Nolu Makbuz ";
            exp2 = " Tarihli Öğrenci Kasa İade Dekontu.";
            accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + DateTime.Today.ToString("dd.MM.yyyy") + exp2;
            accounting.Debt = 0;
            accounting.Credit = total340;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);


            //340 ACOOUNT RECORDS 
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
            accounting.VoucherNo = (int)lastNumber;
            accounting.AccountDate = DateTime.Today;
            accounting.AccountCode = controlCode;
            accounting.AccountCodeName = _accountCodesRepository.GetAccountCode(controlCode, period).AccountCodeName;
            accounting.CodeTypeName = "";
            accounting.DocumentNumber = paymentReceiptNo.ToString();
            accounting.DocumentDate = DateTime.Today;
            var exp5 = "İle Yapılan Öğrenci İadeleri.";
            accounting.Explanation = classroomName + " " + studentViewModel.Student.FirstName + " " + studentViewModel.Student.LastName + " " + paymentNo + exp1 + exp5;
            accounting.Debt = total340;
            accounting.Credit = 0;
            accounting.SortOrder = sortOrder;
            accounting.IsTransaction = false;
            _accountingRepository.CreateAccounting(accounting);

            //340 ACOOUNT RECORDS 
            sortOrder += 1;
            accounting.AccountingID = 0;
            accounting.SchoolID = schoolInfo.SchoolID;
            accounting.Debt = 0;
            accounting.Credit = total340;
            accounting.SortOrder = sortOrder;
            _accountingRepository.CreateAccounting(accounting);
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
        [Route("M230Student/SchoolDebtDataRead/{userID}/{studentid}")]
        public IActionResult SchoolDebtDataRead(int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            List<StudentDebtViewModel> list = new List<StudentDebtViewModel>();

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
                studentDebtViewModel.FeeName = item.Name;
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
        [Route("M230Student/SchoolDebtDataUpdate/{strResult}")]
        public IActionResult SchoolDebtDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<StudentDebtViewModel>>(strResult);
            List<StudentDebt> list = new List<StudentDebt>();

            var i = 0;
            var student = _studentRepository.GetStudent(json[i].StudentID);
            var typeId = 0;
            if (student.ClassroomID > 0)
                typeId = _classroomRepository.GetClassroomID(student.ClassroomID).ClassroomTypeID;
            foreach (var item in json)
            {
                var getCode = _studentDebtRepository.GetStudentDebtID(json[i].SchoolID, json[i].StudentDebtID);

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
                        getCode.IsList = json[i].IsList;
                        list.Add(getCode);
                        _studentDebtRepository.CreateStudentDebt(getCode);
                    }
                }
                else
                if (ModelState.IsValid)
                {
                    getCode.StudentDebtID = json[i].StudentDebtID;
                    getCode.UnitPrice = json[i].UnitPrice;
                    getCode.Discount = json[i].Discount;
                    getCode.Amount = json[i].Amount;
                    getCode.IsList = json[i].IsList;
                    getCode.ClassroomTypeID = typeId;
                    _studentDebtRepository.UpdateStudentDebt(getCode);
                }
                i = i + 1;
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

        //Installment
        [Route("M230Student/InstallmentDataRead/{userID}/{studentid}")]
        public IActionResult InstallmentDataRead(int userID, int studentid)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;
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
                };

                list.Add(studentViewModel);
            }

            return Json(list);
        }

        [Route("M230Student/PaymentsDataRead/{userID}/{studentID}")]
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

        [Route("M230Student/SerialNumbersDataRead/{userID}")]
        public IActionResult SerialNumbersDataRead(int userID)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.ToString();
            var user = _usersRepository.GetUser(userID);

            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
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
                inx++;
                list.Add(schoolProccessSerialNumbers);
            }
            return Json(list);
        }

        [Route("M230Student/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }

        public IActionResult BankNameCombo(int schoolID)
        {
            var bankNameType = _bankRepository.GetBankAll(schoolID);
            return Json(bankNameType);
        }

        [Route("M230Student/AccountCodes/{period}")]
        public IActionResult AccountCodes(string period)
        {
            var accountingCode = _accountCodesRepository.GetAccountCodeAllTrue(period);
            return Json(accountingCode);
        }

        [Route("M230Student/TempDataDelete/{studentID}")]
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
    }

}

