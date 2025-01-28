using DocumentFormat.OpenXml.Office.CoverPageProps;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Barcode;
using Unity.Policy;
using static SQLite.SQLite3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ESCHOOL.Controllers
{
    public class LoginController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IUsersRepository _usersRepository;
        IUsersLogRepository _usersLogRepository;
        ICustomersRepository _customersRepository;
        IParameterRepository _parameterRepository;
        IWebHostEnvironment _hostEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(
            ISchoolInfoRepository schoolInfoRepository,
            IUsersRepository usersRepository,
            IUsersLogRepository usersLogRepository,
            ICustomersRepository customersRepository,
            IParameterRepository parameterRepository,
            IWebHostEnvironment hostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _usersRepository = usersRepository;
            _usersLogRepository = usersLogRepository;
            _parameterRepository = parameterRepository;
            _customersRepository = customersRepository;

            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
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

        public IActionResult Users(int schoolCode)
        {
            //await Task.Delay(100);
            string culture = "";
            var users = _usersRepository.GetUserSchool(schoolCode);
            if (users != null)
            {
                culture = users.SelectedCulture;
            }
            else
            {
                DataBaseDefaultParameterCopy(schoolCode);
            }

            var schoolViewModel = new SchoolViewModel
            {
                //UserID = users.UserID,
                SchoolID = schoolCode,
                SelectedCulture = culture,
            };
            return View(schoolViewModel);
        }

        [HttpPost]
        [Route("Login/LoginControlDataRead/{schoolCode}/{schoolEmail}")]
        public async Task<IActionResult> LoginControlDataRead(int schoolCode, string schoolEmail)
        {
            await Task.Delay(100);
            bool isExist1 = false;
            bool isExist2 = false;
            bool isExpireDate = false;
            bool isExpireDateMsg = false;
            DateTime date = DateTime.Now;

            var expireDate = "";
            var customer = _customersRepository.GetCustomer(schoolCode);

            if (customer != null)
            {
                DateTime msgDate = Convert.ToDateTime(customer.ExpireDate).AddDays(-10);
                DateTime exdate = Convert.ToDateTime(customer.ExpireDate);
                expireDate = exdate.ToShortDateString();

                if (ModelState.IsValid)
                {
                    if (customer.CustomerID == schoolCode) isExist1 = true;
                    if (customer.Email == schoolEmail) isExist2 = true;
                    if (date > customer.ExpireDate) isExpireDate = true;
                    else
                      if (date > msgDate) isExpireDateMsg = true;
                }
            }

            if (isExist1 == true && isExist2 == true)
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

            return Json(new { IsExist1 = isExist1, IsExist2 = isExist2, IsExpireDate = isExpireDate, IsExpireDateMsg = isExpireDateMsg, ExpireDate = expireDate });
        }


        [HttpPost]
        //[Route("Login/UserControlDataRead/{schoolCode}/{userName}/{password}")]
        //public async Task<IActionResult> UserControlDataRead(int schoolCode, string userName, [FromBody] string password)
        public async Task<IActionResult> UserControlDataRead(IFormCollection collection)
        {
            await Task.Delay(100);
            //int userID = 0;
            bool isUserExist = false;
            bool isPasswordExist = false;
            bool isWorkingHours = false;
            bool isActive = false;

            int schoolCode = Convert.ToInt32(collection["schoolCode"]);
            string userName = collection["userName"];
            string password = collection["password"];

            var users = _usersRepository.GetUsersAllLogin();
            var customer = _customersRepository.GetCustomer(schoolCode);

            var school = new SchoolInfo();
            if (customer != null)
            {
                if (ModelState.IsValid)
                {
                    if (customer.CustomerID == schoolCode)
                    {
                        if (users != null)
                        {
                            foreach (var item in users)
                            {
                                if (item.UserName == userName && item.SelectedSchoolCode == schoolCode)
                                {
                                    //userID = item.UserID;
                                    isUserExist = true;

                                    if (item.UserPassword == password && item.IsActive == true)
                                    {
                                        isPasswordExist = true;
                                        isActive = true;

                                        school = _schoolInfoRepository.GetSchoolInfo(item.SchoolID);
                                        var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");

                                        string selectText = Resources.Resource.Select;
                                        if (school.TimeZoneLocation != null && school.TimeZoneLocation != selectText)
                                            info = TimeZoneInfo.FindSystemTimeZoneById(school.TimeZoneLocation);

                                        DateTimeOffset localServerTime = DateTimeOffset.Now;
                                        DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                                        string check1 = localTime.TimeOfDay.ToString();
                                        string[] check2 = check1.Split(':');
                                        double check = double.Parse(check2[0]) + double.Parse(check2[1]) / 60;

                                        string from1 = item.UserShiftFrom.ToString();
                                        string[] from2 = from1.Split(':');
                                        double from = double.Parse(from2[0]) + double.Parse(from2[1]) / 60;

                                        string to1 = item.UserShiftTo.ToString();
                                        string[] to2 = to1.Split(':');
                                        double to = double.Parse(to2[0]) + double.Parse(to2[1]) / 60;

                                        if (to < from) to += 24;

                                        if (check >= from && check <= to)
                                            isWorkingHours = true;
                                        else
                                            isWorkingHours = false;

                                        var cookieschoolCode = "culture";
                                        CookieOptions cookieOptions = new CookieOptions
                                        {
                                            Expires = DateTimeOffset.Now.AddYears(10)
                                        };
                                        var cookie2 = _httpContextAccessor.HttpContext.Request.Cookies[cookieschoolCode];
                                        if (cookie2 != null)
                                            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieschoolCode);

                                        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieschoolCode, item.SelectedCulture, cookieOptions);

                                    }
                                }
                            }
                        }
                    }
                }
            }

            int userID = 0;
            bool isUser = _usersRepository.ExistUsernameAndPassword(userName, password);
            if (isUser)
            {
                userID = _usersRepository.GetUser(userName, password).UserID;
            }

            var user = _usersRepository.GetUser(userID);
            int transactionID = 0;
            string userLogDescription = "";
            string culture = "tr-TR";
            if (user != null)
            {
                culture = user.SelectedCulture.Trim();
                transactionID = _parameterRepository.GetParameterCategoryName("Oturum Açıldı").CategoryID;
                if (culture == "tr-TR")
                {
                    userLogDescription = "Programa Giriş Yapıldı, " + "Kurum Kodu:" + user.SelectedSchoolCode + ", ePosta:" + customer.Email + ", Kullanıcı kodu:" + userName;
                }
                if (culture == "en-US")
                {
                    userLogDescription = "Login to the Program, " + "Code:" + user.SelectedSchoolCode + ", eMail:" + customer.Email + ", User Code:" + userName;
                }
            }

            if (isUserExist == true && isPasswordExist == true)
            {
                user.SelectedSchoolCode = schoolCode;
                user.UserDate = DateTime.Now;
                user.LoginDate = DateTime.Now;
                user.IsLogin = true;
                _usersRepository.UpdateUsers(user);

                //////Users Log//////////////////
                var log = new UsersLog();
                log.UserLogID = 0;
                log.SchoolID = user.SchoolID;
                log.Period = user.UserPeriod;
                log.UserID = user.UserID;
                log.TransactionID = transactionID;
                log.UserLogDate = DateTime.Now;
                log.UserLogDescription = userLogDescription;
                _usersLogRepository.CreateUsersLog(log);
                ////////////////////////////////
            }

            return Json(new { IsUserExist = isUserExist, IsPasswordExist = isPasswordExist, UserID = userID, UserName = userName, IsWorkingHours = isWorkingHours, IsActive = isActive });
        }

        public void DataBaseDefaultParameterCopy(int schoolCode)
        {
            // This command copies from "ESchoolDB" to new DATABASE
            MyAppSettingControl appSettings = new MyAppSettingControl();
            string conn = appSettings.GetAppSetting("DevConnection");
            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;

            var cs = $"Data Source={dataSource};Initial Catalog={schoolCode};User Id=sa;Password={password};";

            string c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Parameter ON";
            string c2 = $"INSERT INTO [{schoolCode}].DBO.Parameter(CategoryID, CategorySubID, CategoryName, Language1, Language2, Language3, Language4, Color, CategoryLevel, SortOrder, IsActive, IsProtected, NationalityCode, IsSelect, IsDirtySelect)";
            string c3 = $"SELECT * FROM ESchoolDB.DBO.Parameter";
            string c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Parameter OFF";
            string command1 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Classroom ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.Classroom (ClassroomID,SchoolID,Period,ClassroomName,ClassroomTypeID,ClassroomTeacher,RoomQuota,SortOrder,IsActive)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.Classroom";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Classroom OFF";
            string command2 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.SchoolInfo ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.SchoolInfo (SchoolID,LogoName,PhotoName,CompanyName,CompanyShortCode,CompanyShortName" +
                  ",CompanyNameForBond,CityNameForBondID,CompanyAddress,Phone1,Phone2,MobilePhone,Fax,TaxOffice,TaxNo,CompanyEmail,WebSite" +
                  ",AuthorizedPersonName1,AuthorizedPersonName2,AuthorizedPersonName3,NameOnReceipt,FormTitle,FormOpt,EmailSMTPHost,EmailSMTPUserName,EmailSMTPPassword,EmailSMTPAddress,EmailSMTPPort" +
                  ",EmailSMTPSsl,CopiesOfForm,PrintQuantity,InvoiceOnDate,SchoolYearStart,SchoolYearEnd,FinancialYearStart,FinancialYearEnd" +
                  ",DefaultInstallment,CurrencyDecimalPlaces,DefaultPaymentTypeCategoryID,DefaultBankID,DefaultShowDept,DateFormat,IsShowFeesBottomLine" +
                  ",TimeZoneLocation,IsActive,EIIsActive,EIIntegratorNameID,EIUserName,EIUserPassword,EIInvoiceSerialCode1,EIInvoiceSerialCode2,EITitle" +
                  ",EIAddress,EITownID,EICityID,EICountry,EIPhone,EIFax,EITaxOffice,EITaxNo,EITradeRegisterNo,EIMersisNo,EIEMail,EIWebAddress,EIIban,InvoiceProfile,InvoiceTypeParameter,ParameterExceptionCode" +
                  ",AccountNoID01,AccountNoID02,AccountNoID03,AccountNoID04,AccountNoID05,AccountNoID06,AccountNoID07,AccountNoID08,AccountNoID09" +
                  ",AccountNoID10,AccountNoID11,RefundDebtAccountID,RefundAccountNoID1,RefundAccountNoID2,RefundAccountNoID3,CancelCreditNoID,CancelDebtNoID" +
                  ",SmsServerNameID,SmsCredits,SmsUserName,SmsUserPassword,SmsTitle,Char01,Char02,Char03,Char04,Char05,Char06,IsChar01,IsChar02,IsChar03,IsChar04,IsChar05,IsChar06,Char01Explanation" +
                  ",Char02Explanation,Char03Explanation,Char04Explanation,Char05Explanation,Char06Explanation,Char01Max,Char02Max,Char03Max,Char04Max,Char05Max,Char06Max,SortType,SortOption,IsSelect,SortOrder,NewPeriod,IsFormPrint)";

            c3 = $"SELECT * FROM ESchoolDB.DBO.SchoolInfo";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.SchoolInfo OFF";
            string command3 = c1 + " " + c2 + " " + c3 + " " + c4;

            c2 = $"INSERT INTO [{schoolCode}].DBO.PSerialNumber (PSerialNumberID,InvoiceName1,InvoiceSerialNo1,InvoiceSerialNo11,InvoiceName2,InvoiceSerialNo2,InvoiceSerialNo22" +
                  ",InvoiceName3,InvoiceSerialNo3,InvoiceSerialNo33,InvoiceName4,InvoiceSerialNo4,InvoiceSerialNo44,InvoiceName5,InvoiceSerialNo5,InvoiceSerialNo55" +
                  ",CollectionNo,CollectionReceiptNo,PaymentNo,PaymentReceiptNo,RegisterNo,BondNo,CheckNo,MailOrderNo,OtsNo1,OtsNo2,CreditCardNo,KmhNo" +
                  ",GovernmentPromotionNo,AccountSerialNo,Number2,Number3,Number4,Number5)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.PSerialNumber";
            string command4 = c2 + " " + c3;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Users ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.Users (UserID,FirstName,LastName,UserName,UserPassword,UserDate,IsLogin,LoginDate,SelectedSchoolCode,SelectedCulture,SelectedLanguage" +
                  ",UserShiftFrom,UserShiftTo,UserPicture,UserViewPicture,GenderTypeCategoryID" +
                  ",DateOfBird,IdNumber,BloodGroupCategoryID,NationalityID,ReligiousID,HomePhone,MobilePhone" +
                  ",HomeAddress,HomeCityParameterID,HomeTownParameterID,HomeZipCode,EMail,UserDateOfJoining,Profession" +
                  ",UserComments,SelectedTheme,SchoolID,UserPeriod,SortOrder,IsSupervisor,IsActive)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.Users";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Users OFF";
            string command5 = c1 + " " + c2 + " " + c3 + " " + c4;
            c2 = $"UPDATE [{schoolCode}].dbo.Users set SelectedSchoolCode={schoolCode}";
            string command55 = c2;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.UsersWorkAreas ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.UsersWorkAreas (UsersWorkAreaID,UserID,CategoryID,IsSchool,IsSelect,IsDirtySelect)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.UsersWorkAreas";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.UsersWorkAreas OFF";
            string command6 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.AccountCodes ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.AccountCodes (AccountCodeID,Period, AccountCode,AccountCodeName,Language1, Language2, Language3, Language4, IsCurrentCard, IsActive)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.AccountCodes";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.AccountCodes OFF";
            string command7 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Bank ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.Bank (BankID,SchoolID,BankName,BankBranchCode,BankGivenCode,AccountDecimation,AccountNo,Iban,PeriodOfTime,SortOrder,IsActive)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.Bank";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.Bank OFF";
            string command8 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.DiscountTable ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.DiscountTable (DiscountTableID,SchoolID,Period,DiscountName,Language1,Language2,Language3,Language4,DiscountPercent,DiscountAmount,IsPasswordRequired,SortOrder,IsActive,IsSelect,IsDirtySelect)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.DiscountTable";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.DiscountTable OFF";
            string command9 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.SchoolBusServices ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.SchoolBusServices (SchoolBusServicesID,SchoolID,Period,PlateNo,DriverName,BusPhone,BusRoute,BusTeacher,BusTeacherPhone,Explanation,SortOrder,IsActive)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.SchoolBusServices";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.SchoolBusServices OFF";
            string command10 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.SchoolFee ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.SchoolFee (SchoolFeeID,SchoolFeeSubID,FeeCategory,SchoolID,Name, Language1, Language2, Language3, Language4,CategoryLevel,Tax,SortOrder,IsActive,IsSelect)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.SchoolFee";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.SchoolFee OFF";
            string command11 = c1 + " " + c2 + " " + c3 + " " + c4;

            c1 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.MultipurposeList ON";
            c2 = $"INSERT INTO [{schoolCode}].DBO.MultipurposeList (MultipurposeListID, UserID, Lenght, Name, Language1, Language2, Language3, Language4, Condition, Conditions1, Conditions2, Type, IsSelect)";
            c3 = $"SELECT * FROM ESchoolDB.DBO.MultipurposeList";
            c4 = $"SET IDENTITY_INSERT [{schoolCode}].dbo.MultipurposeList OFF";
            string command12 = c1 + " " + c2 + " " + c3 + " " + c4;

            using (SqlConnection connection = new SqlConnection(cs))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(command1, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command2, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command3, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command4, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command5, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command55, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command6, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command7, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command8, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command9, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command10, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command11, connection)) command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(command12, connection)) command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}