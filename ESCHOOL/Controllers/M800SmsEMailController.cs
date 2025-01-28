using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
//SMS
using System.Text;
using System.Threading.Tasks;
//SMS

namespace ESCHOOL.Controllers
{
    public class M800SmsEMail : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IStudentRepository _studentRepository;
        IStudentPeriodsRepository _studentPeriodsRepository;
        IStudentInstallmentRepository _studentInstallmentRepository;
        IStudentAddressRepository _studentAddressRepository;
        IStudentParentAddressRepository _studentParentAddressRepository;
        IStudentFamilyAddressRepository _studentFamilyAddressRepository;
        IClassroomRepository _classroomRepository;
        IParameterRepository _parameterRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        ISmsEmailRepository _smsEmailRepository;

        IWebHostEnvironment _hostEnvironment;
        public M800SmsEMail(
            ISchoolInfoRepository schoolInfoRepository,
            IStudentRepository studentRepository,
            IStudentPeriodsRepository studentPeriodsRepository,
            IStudentInstallmentRepository studentInstallmentRepository,

            IStudentAddressRepository studentAddressRepository,
            IStudentParentAddressRepository studentParentAddressRepository,
            IStudentFamilyAddressRepository studentFamilyAddressRepository,

            IClassroomRepository classroomRepository,
            IParameterRepository parameterRepository,

            IUsersRepository usersRepository,
            IUsersWorkAreasRepository usersWorkAreasRepository,
            ISmsEmailRepository smsEmailRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _studentRepository = studentRepository;
            _studentPeriodsRepository = studentPeriodsRepository;
            _studentInstallmentRepository = studentInstallmentRepository;
            _studentAddressRepository = studentAddressRepository;
            _studentParentAddressRepository = studentParentAddressRepository;
            _studentFamilyAddressRepository = studentFamilyAddressRepository;
            _classroomRepository = classroomRepository;
            _parameterRepository = parameterRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;
            _smsEmailRepository = smsEmailRepository;
            _hostEnvironment = hostEnvironment;
        }

        #region SmsEMail
        [HttpGet]
        public IActionResult SmsEMail(int userID)
        {
            var user = _usersRepository.GetUser(userID);

            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);
            ViewBag.date = user.UserDate;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            var studentViewModel = new StudentViewModel
            {
                UserID = userID,
                Period = user.UserPeriod,
                SchoolID = user.SchoolID,
                SchoolInfo = school,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(studentViewModel);
        }
        #endregion

        #region Students 
        [Route("M800SmsEMail/GridStudentDataRead/{userID}/{classroomID}/{isSmsOrEmail}/{option2}/{strDate1}/{strDate2}")]
        public IActionResult GridStudentDataRead(int userID, int classroomID, Boolean isSmsOrEmail, int option2, string strDate1, string strDate2)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            DateTime date1 = DateTime.Today;
            DateTime date2 = DateTime.Today;

            if (option2 == 2)
            {
                date1 = DateTime.Parse(strDate1);
                date2 = DateTime.Parse(strDate2);
            }

            var today = DateTime.Today;

            string strCurrency = "TL";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                strCurrency = "";
            }
            IEnumerable<Student> allStudents = null;
            List<Student> student = new List<Student>();

            if (school.NewPeriod != user.UserPeriod)
            {
                if (classroomID == 0)
                    allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID);
                else
                    allStudents = _studentRepository.GetStudentAllPeriod(user.SchoolID).Where(b => b.ClassroomID == classroomID);

                var studenPeriod = _studentPeriodsRepository.GetStudentAll(user.SchoolID, user.UserPeriod);
                student = allStudents.Where(s => studenPeriod.Where(p => p.StudentID == s.StudentID).Count() > 0).ToList();
            }
            else
            {
                if (classroomID == 0)
                    student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).ToList();
                else
                    student = _studentRepository.GetStudentAllWithClassroom(user.SchoolID).Where(b => b.ClassroomID == classroomID).ToList();
            }

            var categoryID = _parameterRepository.GetParameterCategoryName("Ödeme Şekli").CategoryID;
            var parameters = _parameterRepository.GetParameterSubID(categoryID);
            string classroomName = "";
        
            List<SmsEmailViewModel> list = new List<SmsEmailViewModel>();
            foreach (var item in student)
            {
                var statuName = _parameterRepository.GetParameter(item.StatuCategoryID).CategoryName;
                if (statuName == "İptal" || statuName == "Kayıt Dondurdu" || statuName == "Pasif Kayıt") continue;

                var isExist = true;
                if (isExist == true)
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

                        var installment = _studentInstallmentRepository.GetStudentInstallment(item.SchoolID, item.StudentID, user.UserPeriod);

                        var studentAddress = _studentAddressRepository.GetStudentAddress(item.StudentID);
                        var studentParent = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);
                        var studentFamily = _studentFamilyAddressRepository.GetStudentFamilyAddress(item.StudentID);

                        if (studentAddress != null && !studentAddress.IsSms) studentAddress.MobilePhone = "***";
                        if (studentAddress != null && !studentAddress.IsEMail) studentAddress.EMail = "***";
                        if (studentParent != null && !studentParent.IsSMS) studentParent.MobilePhone = "***";
                        if (studentParent != null && !studentParent.IsEmail) studentParent.EMail = "***";

                        if (studentFamily != null && !studentFamily.FatherIsSMS) studentFamily.FatherMobilePhone = "***";
                        if (studentFamily != null && !studentFamily.FatherIsEmail) studentFamily.FatherEMail = "***";
                        if (studentFamily != null && !studentFamily.MotherIsSMS) studentFamily.MotherMobilePhone = "***";
                        if (studentFamily != null && !studentFamily.MotherIsEmail) studentFamily.MotherEMail = "***";

                        if (option2 != 2)
                        {
                            var smsEmailViewModel = new SmsEmailViewModel();
                            {
                                smsEmailViewModel.SmsEmailID = 0;
                                smsEmailViewModel.Period = user.UserPeriod;
                                smsEmailViewModel.SchoolID = user.SchoolID;
                                smsEmailViewModel.StudentID = item.StudentID;
                                smsEmailViewModel.StudentNumber = item.StudentNumber;
                                smsEmailViewModel.Name = item.FirstName + " " + item.LastName;
                                smsEmailViewModel.StudentClassroom = classroomName;
                                smsEmailViewModel.DateOfBird = item.DateOfBird;
                                if (studentAddress != null)
                                {
                                    smsEmailViewModel.StudentMobilePhone = studentAddress.MobilePhone;
                                    smsEmailViewModel.StudentEMail = studentAddress.EMail;
                                }
                                if (studentParent != null)
                                {
                                    smsEmailViewModel.ParentMobilePhone = studentParent.MobilePhone;
                                    smsEmailViewModel.ParentEMail = studentParent.EMail;
                                }
                                if (studentFamily != null)
                                {
                                    smsEmailViewModel.FatherMobilePhone = studentFamily.FatherMobilePhone;
                                    smsEmailViewModel.FatherEMail = studentFamily.FatherEMail;
                                    smsEmailViewModel.MotherMobilePhone = studentFamily.MotherMobilePhone;
                                    smsEmailViewModel.MotherEMail = studentFamily.MotherEMail;
                                }
                                smsEmailViewModel.UnpaidDebtMessage = "";
                                smsEmailViewModel.TotalDebtMessage = "";
                                smsEmailViewModel.IsStatu = false;

                                int i1 = 0, i2 = 0;
                                string msgUnpaid = "";
                                string msgDebt = "";
                                var date = "";
                                decimal amount = 0;
                                foreach (var ins in installment)
                                {
                                    if (ins.InstallmentAmount != ins.PreviousPayment)
                                    {
                                        var param = parameters.FirstOrDefault(p => p.CategoryID == ins.CategoryID);
                                        var categoryName = param.CategoryName;
                                        if (user.SelectedCulture.Trim() == "en-US")
                                        {
                                            categoryName = param.Language1;
                                        }

                                        DateTime expireDate = Convert.ToDateTime(ins.InstallmentDate);
                                        date = expireDate.ToShortDateString();
                                        amount = ins.InstallmentAmount - ins.PreviousPayment;
                                        if (ins.InstallmentDate <= today)
                                        {
                                            i1 += 1;
                                            msgUnpaid += i1.ToString() + ") " + date + " " + categoryName + " " + String.Format("{0:0.00}", amount) + strCurrency + ", ";
                                        }
                                        i2 += 1;
                                        msgDebt += i2.ToString() + ") " + date + " " + categoryName + " " + String.Format("{0:0.00}", amount) + strCurrency + ", ";
                                    }
                                }
                                smsEmailViewModel.UnpaidDebtMessage = msgUnpaid;
                                smsEmailViewModel.TotalDebtMessage = msgDebt;
                            };
                            list.Add(smsEmailViewModel);
                        }
                        else
                        {
                            DateTime dt = Convert.ToDateTime(item.DateOfBird);
                            DateTime DOB = new DateTime(DateTime.Today.Year, dt.Month, dt.Day);
                            if (DOB >= date1 && DOB <= date2)
                            {
                                var smsEmailViewModel = new SmsEmailViewModel();
                                {
                                    smsEmailViewModel.SmsEmailID = 0;
                                    smsEmailViewModel.Period = user.UserPeriod;
                                    smsEmailViewModel.SchoolID = user.SchoolID;
                                    smsEmailViewModel.StudentID = item.StudentID;
                                    smsEmailViewModel.StudentNumber = item.StudentNumber;
                                    smsEmailViewModel.Name = item.FirstName + " " + item.LastName;
                                    smsEmailViewModel.StudentClassroom = classroomName;
                                    smsEmailViewModel.DateOfBird = item.DateOfBird;
                                    if (studentAddress != null)
                                    {
                                        smsEmailViewModel.StudentMobilePhone = studentAddress.MobilePhone;
                                        smsEmailViewModel.StudentEMail = studentAddress.EMail;
                                    }
                                    if (studentParent != null)
                                    {
                                        smsEmailViewModel.ParentMobilePhone = studentParent.MobilePhone;
                                        smsEmailViewModel.ParentEMail = studentParent.EMail;
                                    }
                                    if (studentFamily != null)
                                    {
                                        smsEmailViewModel.FatherMobilePhone = studentFamily.FatherMobilePhone;
                                        smsEmailViewModel.FatherEMail = studentFamily.FatherEMail;
                                        smsEmailViewModel.MotherMobilePhone = studentFamily.MotherMobilePhone;
                                        smsEmailViewModel.MotherEMail = studentFamily.MotherEMail;
                                    }
                                    smsEmailViewModel.UnpaidDebtMessage = "";
                                    smsEmailViewModel.TotalDebtMessage = "";
                                    smsEmailViewModel.IsStatu = false;
                                };
                                list.Add(smsEmailViewModel);
                            }
                        }
                    }
                }
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M800SmsEMail/GridStudentDataCreate/{strResult}/{isSmsOrEmail}/{option1}/{option2}/{userID}/{titleTxt}/{messageTxt}")]
        public IActionResult GridStudentDataCreate([Bind(Prefix = "models")] string strResult, Boolean isSmsOrEmail, int option1, int option2, int userID, string titleTxt, string messageTxt)
        {
            var user = _usersRepository.GetUser(userID);

            var json = new JavaScriptSerializer().Deserialize<List<SmsEmailViewModel>>(strResult);
            List<SmsEmail> list = new List<SmsEmail>();

            var deleteSmsEMail = _smsEmailRepository.GetSmsEmailAll(user.SchoolID);
            foreach (var item in deleteSmsEMail)
            {
                _smsEmailRepository.DeleteSmsEmail(item);
            }

            var i = 0;
            foreach (var item in json)
            {
                var smsEmail = new SmsEmail();
                smsEmail.SmsEmailID = 0;
                smsEmail.StudentID = item.StudentID;
                smsEmail.SchoolID = item.SchoolID;
                smsEmail.DateOfBird = item.DateOfBird;
                smsEmail.IsMobile = isSmsOrEmail;

                var studentAddress = _studentAddressRepository.GetStudentAddress(item.StudentID);
                var studentParent = _studentParentAddressRepository.GetStudentParentAddress(item.StudentID);
                var studentFamily = _studentFamilyAddressRepository.GetStudentFamilyAddress(item.StudentID);

                if (studentParent != null && studentParent.IsSMS == true && option1 == 1 && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentParent.MobilePhone;
                if (studentFamily != null && studentFamily.MotherIsSMS == true && option1 == 2 && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentFamily.MotherMobilePhone;
                if (studentFamily != null && studentFamily.FatherIsSMS == true && option1 == 3 && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentFamily.FatherMobilePhone;
                if (studentAddress != null && studentAddress.IsSms == true && option1 == 5 && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentAddress.MobilePhone;

                if (studentParent != null && studentParent.IsEmail == true && option1 == 1 && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentParent.EMail;
                if (studentFamily != null && studentFamily.MotherIsEmail == true && option1 == 2 && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentFamily.MotherEMail;
                if (studentFamily != null && studentFamily.FatherIsEmail == true && option1 == 3 && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentFamily.FatherEMail;
                if (studentAddress != null && studentAddress.IsEMail == true && option1 == 5 && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentAddress.EMail;

                if (option1 == 4)
                {
                    if (studentAddress.MobilePhone != null && studentAddress != null && studentAddress.IsSms == true && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentAddress.MobilePhone;
                    if (studentAddress.EMail != null && studentAddress != null && studentAddress.IsEMail == true && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentAddress.EMail;

                    if (studentParent.MobilePhone != null && studentParent != null && studentAddress.IsSms == true && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentParent.MobilePhone;
                    if (studentParent.EMail != null && studentParent != null && studentAddress.IsEMail == true && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentParent.EMail;

                    if (studentFamily.FatherMobilePhone != null && studentFamily != null && studentFamily.FatherIsSMS == true && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentFamily.FatherMobilePhone;
                    if (studentFamily.FatherEMail != null && studentFamily != null && studentFamily.FatherIsEmail == true && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentFamily.FatherEMail;

                    if (studentFamily.MotherMobilePhone != null && studentFamily != null && studentFamily.MotherIsSMS == true && isSmsOrEmail == true) smsEmail.MobilePhoneOrEMail = studentFamily.MotherMobilePhone;
                    if (studentFamily.MotherEMail != null && studentFamily != null && studentFamily.MotherIsEmail == true && isSmsOrEmail == false) smsEmail.MobilePhoneOrEMail = studentFamily.MotherEMail;
                }

                if (option2 == 1 || option2 == 2)
                {
                    smsEmail.Title = titleTxt;
                    smsEmail.Message = messageTxt;
                }
                else if (option2 == 3)
                {
                    smsEmail.Title = titleTxt;
                    smsEmail.Message = json[i].UnpaidDebtMessage;
                }
                else
                {
                    smsEmail.Title = titleTxt;
                    smsEmail.Message = json[i].TotalDebtMessage;
                }

                smsEmail.IsStatu = false;
                list.Add(smsEmail);

                if (ModelState.IsValid)
                {
                    _smsEmailRepository.CreateSmsEmail(smsEmail);
                }
                i += 1;

                CsvCreate(user.SelectedSchoolCode, smsEmail);

            }

            SMSExecuteCommand(user.SelectedSchoolCode, "send");

            return Json(list);
        }
        #endregion

        #region CsvSMS 
        string CsvSMS = string.Empty;
        public void CsvCreate(int selectedSchoolCode, SmsEmail smsEmail)
        {
            if (smsEmail.IsMobile)
            {
                AddCsv("2");
            }
            else
            {
                AddCsv("1");
            }
            AddCsv(smsEmail.MobilePhoneOrEMail);
            AddCsv(smsEmail.Title);
            AddCsv(smsEmail.Message);

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "SMS", selectedSchoolCode.ToString());
            string file = "LISTE.CSV";
            string filePath = Path.Combine(projectDir, file);

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        w.WriteLine(CsvSMS);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception) { }

            AddCsvNewRow();
        }
        private void AddCsv(string CsvInfo)
        {
            CsvSMS += CsvInfo + ";";
        }
        private void AddCsvNewRow()
        {
            CsvSMS += "\n";
        }
        #endregion

        #region SMSSending
        public void SMSExecuteCommand(int schoolCode, string exeName)
        {
            if (exeName == "send") // Mesaj
            {
                MesajGonder msg = new MesajGonder(schoolCode.ToString(), _hostEnvironment.ContentRootPath);
                msg.Execute();
            }
            else // SMS
            {
                SMSKredi sms = new SMSKredi(schoolCode.ToString(), _hostEnvironment.ContentRootPath);
                sms.Execute();
            }

        }
        #endregion

        #region Result 
        [Route("M800SmsEMail/GridResultDataRead/{userID}")]
        public IActionResult GridResultDataRead(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var result = _smsEmailRepository.GetSmsEmailAll(user.SchoolID);

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "SMS", user.SelectedSchoolCode.ToString());
            string file = "LISTE2.CSV";
            string[] readText = null;
            string filePath = Path.Combine(projectDir, file);
            if (System.IO.File.Exists(filePath))
                readText = System.IO.File.ReadAllLines(filePath);

            List<SmsEmailViewModel> list = new List<SmsEmailViewModel>();
            foreach (var item in result)
            {
                var smsEmailViewModel = new SmsEmailViewModel();
                {
                    var student = _studentRepository.GetStudent(item.StudentID);
                    smsEmailViewModel.Name = student.FirstName + " " + student.LastName;
                    smsEmailViewModel.DateOfBird = item.DateOfBird;
                    smsEmailViewModel.MobilePhoneOrEMail = item.MobilePhoneOrEMail;
                    smsEmailViewModel.Title = item.Title;
                    smsEmailViewModel.Message = item.Message;
                    smsEmailViewModel.IsStatu = false;

                    string phoneOrEmail = "";
                    string resultTxt = "";
                    if (readText != null)
                    {
                        foreach (var csv in readText)
                        {
                            string[] words = csv.Split(';');
                            phoneOrEmail = words[1];
                            if (phoneOrEmail == item.MobilePhoneOrEMail)
                            {
                                if (words[4] == "1")
                                {
                                    resultTxt = Resources.Resource.NotSent;
                                    smsEmailViewModel.TransactionMessage = resultTxt;
                                }
                                else
                                {
                                    resultTxt = Resources.Resource.Sent;
                                    smsEmailViewModel.IsStatu = true;
                                    smsEmailViewModel.TransactionMessage = resultTxt;
                                }
                            }
                        }
                    }
                };
                list.Add(smsEmailViewModel);
            }
            return Json(list);
        }


        [Route("M800SmsEMail/SmsAccountInfo/{userID}")]
        public IActionResult SmsAccountInfo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            SMSExecuteCommand(user.SelectedSchoolCode, "credit");

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "SMS", user.SelectedSchoolCode.ToString());
            string file = "KIMLIK.INI";
            string filePath = Path.Combine(projectDir, file);

            var MyIni = new IniFile();
            MyIni = new IniFile(filePath);
            string credit = MyIni.Read("SMSKALAN", "TcKimlikNoBul");
            if (credit == null || credit == "")
                school.SmsCredits = 0;
            else school.SmsCredits = Int32.Parse(credit);
            _schoolInfoRepository.UpdateSchoolInfo(school);

            var serverName = "";
            if (school.SmsServerNameID != 0)
                serverName = _parameterRepository.GetParameter(school.SmsServerNameID).CategoryName;

            int lenght = school.CompanyName.Length;
            if (lenght > 50) lenght = 50;
            var companyName = school.CompanyName.Substring(0, lenght);
            int ssl = 0;
            if (school.EmailSMTPSsl == true) ssl = 1;

            if (companyName != null) MyIni.Write("MSENDER", companyName, "TcKimlikNoBul");
            if (school.EmailSMTPUserName != null) MyIni.Write("EMAILUSER", school.EmailSMTPUserName, "TcKimlikNoBul");
            if (school.EmailSMTPPassword != null) MyIni.Write("EMAILPASS", school.EmailSMTPPassword, "TcKimlikNoBul");
            if (school.EmailSMTPPort != null) MyIni.Write("PORT", school.EmailSMTPPort, "TcKimlikNoBul");
            if (ssl != 0) MyIni.Write("SSL", ssl.ToString(), "TcKimlikNoBul");
            if (school.EmailSMTPAddress != null) MyIni.Write("EMAIL", school.EmailSMTPAddress, "TcKimlikNoBul");
            if (school.EmailSMTPHost != null) MyIni.Write("MAILSERVER", school.EmailSMTPHost, "TcKimlikNoBul");
            if (school.SmsUserName != null) MyIni.Write("SMSUSER", school.SmsUserName, "TcKimlikNoBul");
            if (school.SmsUserPassword != null) MyIni.Write("SMSPASS", school.SmsUserPassword, "TcKimlikNoBul");
            if (school.SmsTitle != null) MyIni.Write("SMSNAME", school.SmsTitle.ToString(), "TcKimlikNoBul");
            if (serverName != null) MyIni.Write("SMSSERV", serverName, "TcKimlikNoBul");

            return Json(new { credit = credit });
        }
        #endregion

        #region Div1Update
        [Route("M800SmsEMail/Div1Update/{userID}/{servicePproviderID}/{smsCredits}/{smsUserName}/{smsUserPassword}/{smsTitle}")]
        public IActionResult Div1Update(int userID, int servicePproviderID, int smsCredits, string smsUserName, string smsUserPassword, string smsTitle)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            schoolInfo.SmsServerNameID = servicePproviderID;
            schoolInfo.SmsCredits = smsCredits;
            schoolInfo.SmsUserName = smsUserName;
            schoolInfo.SmsUserPassword = smsUserPassword;
            schoolInfo.SmsTitle = smsTitle;
            _schoolInfoRepository.UpdateSchoolInfo(schoolInfo);

            return Json(true);
        }
        #endregion

        #region ClassroomCombo

        [Route("M800SmsEMail/ClassroomCombo/{userID}")]
        public IActionResult ClassroomCombo(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var classroom = _classroomRepository.GetClassroomPeriods(user.SchoolID, user.UserPeriod);
            return Json(classroom);
        }
        public IActionResult ServiceProviderCombo()
        {
            var categoryID = _parameterRepository.GetParameterCategoryName("Sms. Servis Sağlayıcı").CategoryID;
            var registrationType = _parameterRepository.GetParameterSubID(categoryID);

            return Json(registrationType);
        }
        #endregion
    }



    #region SMS - Mesaj section
    #region Mesaj section
    public class MesajGonder
    {
        Logging Log = new Logging();
        private string uname = "", pwd = "", emailaddr = "", emailuser = "", emailpwd = "", mailhost = "",
                       mailport = "", isssl = "", smsname = "", smsserv = "", act = "13";
        private string schoolCode = "";
        private string fPath = "";

        public MesajGonder(string code, string filePath)
        {
            Log.DeleteLogFile();
            Log.Write("========= Mesaj ===========");
            Log.Write("Mesaj SchoolCode :" + code + " FilePath : " + filePath);
            schoolCode = code;
            fPath = filePath;
        }

        public void Execute()
        {
            Log.Write("Execute start");

            NCSIni ini;
            string appPath, fname;
            appPath = fPath + "\\SMS";
            ini = new NCSIni(appPath + "\\" + schoolCode + "\\KIMLIK.ini");
            fname = appPath + "\\" + schoolCode + "\\LISTE.csv";
            Log.Write("Inifile : " + appPath + "\\" + schoolCode + "\\KIMLIK.ini");
            Log.Write("Filename : " + fname);

            if (File.Exists(fname))
            {
                Log.Write(fname + " exist");
                List<DataClass> datalist = new List<DataClass>();

                GetDataTableFromCsv(fname, datalist);

                uname = ini.IniReadValue("TcKimlikNoBul", "SMSUSER").Trim();
                pwd = ini.IniReadValue("TcKimlikNoBul", "SMSPASS").Trim();
                smsname = ini.IniReadValue("TcKimlikNoBul", "SMSNAME").Trim();
                emailaddr = ini.IniReadValue("TcKimlikNoBul", "EMAIL").Trim();
                emailuser = ini.IniReadValue("TcKimlikNoBul", "EMAILUSER").Trim();
                emailpwd = ini.IniReadValue("TcKimlikNoBul", "EMAILPASS").Trim();
                mailhost = ini.IniReadValue("TcKimlikNoBul", "MAILSERVER").Trim();
                mailport = ini.IniReadValue("TcKimlikNoBul", "PORT").Trim();
                smsserv = ini.IniReadValue("TcKimlikNoBul", "SMSSERV").Trim();
                isssl = ini.IniReadValue("TcKimlikNoBul", "SSL").Trim();

                if (datalist.Where(a => a.Type == 1).Count() > 0)
                {
                    Log.Write("E-postalar gönderiliyor...");
                    SmtpClient smtpcli = new SmtpClient();
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailuser, emailpwd);
                    smtp.Host = mailhost;
                    if (mailport != null && mailport != "")
                        smtp.Port = Convert.ToInt32(mailport);
                    if (isssl == "0")
                    {
                        smtp.EnableSsl = false;
                    }
                    else
                    {
                        smtp.EnableSsl = true;
                    }
                    try
                    {
                        foreach (DataClass data in datalist.Where(a => a.Type == 1))
                        {
                            MailMessage mail = new MailMessage(emailaddr, data.To);
                            mail.From = new MailAddress(emailaddr);
                            mail.IsBodyHtml = true;

                            string Body = data.Data;
                            mail.Body = Body;
                            mail.Subject = data.Subject;
                            smtp.Send(mail);
                            data.Result = "1";
                        }
                        Log.Write(datalist.Where(a => a.Type == 1).Count() + " e-posta gönderildi.");
                    }
                    catch
                    {
                        Log.Write("Mail gönderim hatası. Bilgileri kontrol ediniz.");
                    }
                }

                if (datalist.Where(a => a.Type == 2).Count() > 0)
                {
                    Log.Write("Kısa mesajlar gönderiliyor...");
                    string res = "";
                    Log.Write("Sms service : [" + smsserv + "]");
                    if (smsserv == "NETGSM")
                    {
                        string xmldata = getxml(datalist, uname, pwd, "Netgsm", smsname);
                        res = XMLPOST("https://api.netgsm.com.tr/sms/send/xml", xmldata).ToString();
                    }
                    else

                        if (smsserv == "FONIVA")
                    {
                        string xmldata = getxmlFoniva(datalist, uname, pwd, act, smsname);
                        res = XMLPOST2("http://smsapi.foniva.net/", xmldata).ToString();
                    }
                    else
                    {
                        string xmldata = getxml2(datalist, uname, pwd, "Netgsm", smsname); // parametrenin Lira olması gerekmiyor mu?
                        res = XMLPOST("http://www.lira.com.tr/services/api.php?islem=sms", xmldata).ToString();
                    }

                    if (res == "-1")
                    {
                        Log.Write("Bağlantı hatası");
                    }
                    else
                    {
                        foreach (DataClass data in datalist.Where(a => a.Type == 2 && a.To.Length > 9))
                        {
                            data.Result = "1";
                        }
                        Log.Write(datalist.Where(a => a.Type == 2).Count() + " kısa mesaj gönderildi.");
                    }
                }
                Log.Write("WriteResult.");
                WriteResult(datalist, appPath + "\\" + schoolCode);
            }
            else
            {
                Log.Write("Liste.csv dosyası bulunamadı");
            }
            Log.Write("Execute end.");
        }

        private void GetDataTableFromCsv(string path, List<DataClass> l)
        {
            using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1254)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';'); // csv delimiter

                    List<string> strList = new List<string>();

                    foreach (var item in fields)
                    {
                        var tmp = item.Clone() as string;
                        while (tmp.Contains("  "))
                            tmp = tmp.Replace("  ", " ");

                        tmp = tmp.Trim();
                        strList.Add(tmp);
                    }

                    fields = strList.ToArray();

                    if (fields[0].Trim() == "1")
                    {
                        DataClass dc = new DataClass(fields[1], "<p>" + fields[3] + "</p><p>" + fields[4] + "</p><p>" + fields[5] + "</p>", fields[2], 1);
                        l.Add(dc);
                    }
                    else if (fields[0].Trim() == "2")
                    {
                        DataClass dc = new DataClass(fields[1], fields[2], "", 2);
                        l.Add(dc);
                    }
                }
            }
        }
        private void WriteResult(List<DataClass> lst, string path)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\liste2.csv"))
            {
                foreach (DataClass data in lst)
                {
                    string line = data.Type + ";" + data.To + ";" + data.Subject + ";" + data.Data + ";" + data.Result;
                    file.WriteLine(line);
                }
            }
        }

        private async Task<string> XMLPOST(string PostAddress, string xmlData)
        {
            try
            {
                HttpClient wUpload = new HttpClient();
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);

                var content = new ByteArrayContent(bPostArray);
                var r = await wUpload.PutAsync(PostAddress, content);
                string sWebPage = r.ToString();

                return sWebPage;
            }
            catch
            {
                return "-1";
            }
        }

        private async Task<string> XMLPOST2(string PostAddress, string xmlData)
        {
            try
            {
                HttpClient wUpload = new HttpClient();
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);

                var content = new ByteArrayContent(bPostArray);
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("text/xml");
                var r = await wUpload.PutAsync(PostAddress, content);
                string sWebPage = r.ToString();

                return sWebPage;
            }
            catch
            {
                return "-1";
            }
        }

        private string GETCREDIT(string uname, string pwd)
        {
            string kalan = "";
            var url = "https://api.netgsm.com.tr/balance/list/get/?usercode=" + uname + "&password=" + pwd;

            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.Timeout = new TimeSpan(0, 0, 10);
            var response = client.GetAsync(url).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            string sonuc = result.ToString();

            if (sonuc.Trim().StartsWith("00"))
            {
                string[] arr = sonuc.Split(' ');
                if (arr[1] != null && arr[1].Length > 0)
                {
                    kalan = arr[1];
                }
            }
            return kalan;
        }

        private string getxml(List<DataClass> lst, string uname, string pwd, string compname, string baslik)
        {
            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'?>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company dil=\"TR\">" + compname + "</company>";
            ss += "<usercode>" + uname + "</usercode>";
            ss += "<password>" + pwd + "</password>";
            ss += "<startdate></startdate>";
            ss += "<stopdate></stopdate>";
            ss += "<type>n:n</type>";
            ss += "<msgheader>" + baslik + "</msgheader>";
            ss += "</header>";
            ss += "<body>";
            foreach (DataClass data in lst.Where(a => a.Type == 2 && a.To.Length > 9))
            {
                ss += "<mp><msg><![CDATA[" + data.Data + "]]></msg><no>" + data.To + "</no></mp>";
            }
            ss += "</body>";
            ss += "</mainbody>";

            return ss;
        }

        private string getxml2(List<DataClass> lst, string uname, string pwd, string compname, string baslik)
        {
            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'?>";
            ss += "<BILGI>";
            ss += "<KULLANICI_ADI>" + uname + "</KULLANICI_ADI>";
            ss += "<SIFRE>" + pwd + "</SIFRE>";
            ss += "<GONDERIM_TARIH></GONDERIM_TARIH>";
            ss += "<BASLIK>" + baslik + "</BASLIK>";
            ss += "</BILGI>";
            ss += "<ISLEM>";
            foreach (DataClass data in lst.Where(a => a.Type == 2 && a.To.Length > 9))
            {
                ss += "<YOLLA><MESAJ><![CDATA[" + data.Data + "]]></MESAJ><NO>" + data.To + "</NO></YOLLA>";
            }
            ss += "</ISLEM>";
            return ss;
        }

        private string getxmlFoniva(List<DataClass> lst, string uname, string pwd, string act, string originator)
        {
            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'?>";
            ss += "<MultiTextSMS>";
            ss += "<UserName>" + uname + "</UserName>";
            ss += "<PassWord>" + pwd + "</PassWord>";
            ss += "<Action>" + act + "</Action>";

            ss += "<Messages>";
            foreach (DataClass data in lst.Where(a => a.Type == 2 && a.To.Length > 9))
            {
                ss += "<Message><Mesgbody><![CDATA[" + data.Data + "]]></Mesgbody><Number>" + data.To + "</Number></Message>";
            }

            ss += "</Messages>";
            ss += "<Originator>" + originator + "</Originator>";

            ss += "</MultiTextSMS>";
            return ss;
        }
    }

    public class DataClass
    {
        public string To;
        public string Data;
        public int Type;
        public string Subject;
        public string Result;
        public DataClass(string to, string data, string subject, int type)
        {
            To = to.Replace(" ", "");
            Data = data;
            Type = type;
            Subject = subject;
            Result = "0";
        }
    }
    #endregion
    #region SMS section
    public class SMSKredi
    {

        Logging Log = new Logging();
        private string schoolCode = "";
        private string fPath = "";

        public SMSKredi(string code, string filePath)
        {
            Log.DeleteLogFile();
            Log.Write("========= SMS ===========");
            Log.Write("SMS SchoolCode :" + code);
            schoolCode = code;
            fPath = filePath;
        }

        public void Execute()
        {
            Log.Write("Execute start.");
            NCSIni ini;

            ini = new NCSIni(fPath + "\\SMS\\" + schoolCode + "\\KIMLIK.ini");

            Log.Write("Inifile : " + fPath + "\\SMS\\" + schoolCode + "\\KIMLIK.ini");

            string uname = ini.IniReadValue("TcKimlikNoBul", "SMSUSER").Trim();
            string pwd = ini.IniReadValue("TcKimlikNoBul", "SMSPASS").Trim();
            string smsname = ini.IniReadValue("TcKimlikNoBul", "SMSNAME").Trim();
            string smsserv = ini.IniReadValue("TcKimlikNoBul", "SMSSERV").Trim();
            string act = "3";

            string kalan = " ";
            string xmldata = " ";

            Log.Write("Sms services :" + smsserv);
            if (smsserv == "NETGSM")
            {
                kalan = GETCREDIT(uname, pwd, smsserv);
            }
            else if (smsserv == "FONIVA")
            {
                xmldata = " ";
                List<DataClass> datalist = new List<DataClass>();
                act = "4";
                xmldata = GETCREDITFONIVA(datalist, uname, pwd, act);
                string UserId = XMLFONIVA("http://smsapi.foniva.net/", xmldata).ToString();

                string[] parcalar;
                string k = "\r\n";
                char[] ayrac = k.ToCharArray();

                parcalar = UserId.Split(ayrac);
                kalan = parcalar[0];
            }

            if (kalan != "")
            {
                Log.Write("IniWriteValue, Kalan Kredi : " + kalan);
                ini.IniWriteValue("TcKimlikNoBul", "SMSKALAN", kalan);
            }
            Log.Write("Execute end.");
        }

        private string GETCREDIT(string uname, string pwd, string smsserv)
        {
            string kalan = "";
            if (smsserv == "NETGSM" || smsserv == "")
            {
                var  url = "https://api.netgsm.com.tr/balance/list/get/?usercode=" + uname + "&password=" +  pwd;
                
                // .Net 6
                var handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                string sonuc = result.ToString();
                // .Net 6

                if (sonuc.Trim().StartsWith("00"))
                {
                    string[] arr = sonuc.Split(' ');
                    if (arr[1] != null && arr[1].Length > 0)
                    {
                        kalan = arr[1];
                    }
                }
            }
            return kalan;
        }

        private string GETCREDITFONIVA(List<DataClass> lst, string uname, string pwd, string act)
        {
            string ss = "";
            ss += "<UserControl>";
            ss += "<UserName>" + uname + "</UserName>";
            ss += "<PassWord>" + pwd + "</PassWord>";
            ss += "<Action>" + act + "</Action>";
            ss += "</UserControl>";
            return ss;
        }

        private async Task<string> XMLFONIVA(string PostAddress, string xmlData)
        {
            try
            {
                HttpClient wUpload = new HttpClient();
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                var content = new ByteArrayContent(bPostArray);
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("text/xml");
                var r = await wUpload.PutAsync(PostAddress, content);
                string sWebPage = r.ToString();

                return sWebPage;
            }
            catch
            {
                return "-1";
            }
        }
    }
    #endregion

    public class Logging
    {
        public void Write(string sInfo)
        {
            StreamWriter sw = File.AppendText("NCSLog.txt");
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + sInfo.ToString());
            sw.Flush();
            sw.Close();
        }
        public void DeleteLogFile()
        {
            if (File.Exists("NCSLog.txt"))
                File.Delete("NCSLog.txt");
        }

        #endregion
    }
}