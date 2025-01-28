using DocumentFormat.OpenXml.Spreadsheet;
using ESCHOOL.Models;
using ESCHOOL.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Telerik.Reporting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace ESCHOOL.Controllers
{
    public static class HttpRequestExtension
    {
        public static ICollection<string> QueryStringKeys(this HttpRequest request)
        {
            ICollection<string> keys = request.Query.Keys;
            return keys;
        }
    }
    public class ReportingController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IUsersRepository _usersRepository;
        IStudentRepository _studentRepository;
        IStudentInvoiceRepository _studentInvoiceRepository;
        IStudentInvoiceDetailRepository _studentInvoiceDetailRepository;
        IStudentInvoiceAddressRepository _studentInvoiceAddressRepository;
        IParameterRepository _parameterRepository;
        IAccountCodesDetailRepository _accountCodesDetailRepository;
        IAccountCodesRepository _accountCodesRepository;
        IWebHostEnvironment _hostEnvironment;
        public ReportingController(
             ISchoolInfoRepository schoolInfoRepository,
             IUsersRepository usersRepository,
             IStudentRepository studentRepository,
             IStudentInvoiceAddressRepository studentInvoiceAddressRepository,
             IStudentInvoiceRepository studentInvoiceRepository,
             IStudentInvoiceDetailRepository studentInvoiceDetailRepository,
             IParameterRepository parameterRepository,
             IAccountCodesRepository accountingCodeRepository,
             IAccountCodesDetailRepository accountCodesDetailRepository,
             IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _usersRepository = usersRepository;
            _studentRepository = studentRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _studentInvoiceAddressRepository = studentInvoiceAddressRepository;
            _studentInvoiceRepository = studentInvoiceRepository;
            _studentInvoiceDetailRepository = studentInvoiceDetailRepository;
            _accountCodesRepository = accountingCodeRepository;
            _accountCodesDetailRepository = accountCodesDetailRepository;
            _parameterRepository = parameterRepository;

            _hostEnvironment = hostEnvironment;
        }

        // id Report name, exitID ReportViewer exit parameter
        //0- Main Menu
        //1- Student entry 
        //2- Collection entry
        //3- Accounting entry
        [Route("Reporting/Index/{id}/{studentID}/{exitID}")]
        public async Task<IActionResult> Index(string id, int studentID, int exitID)
        {
            await Task.Delay(10);
            var parameters = string.Empty;
            var keys = Request.QueryStringKeys();
            var queryString = Request.Query;
            int userID = 0;

            foreach (var key in keys)
            {
                StringValues someInt;
                queryString.TryGetValue(key, out someInt);
                if (key == "userID") userID = Convert.ToInt32(someInt);

                parameters = string.Concat(parameters, string.Format("\"{0}\":{1},", key, someInt));
            }
            var user = _usersRepository.GetUser(userID);

            if (!string.IsNullOrEmpty(parameters))
            {
                parameters = parameters.Substring(0, parameters.Length - 1);
                parameters = "parameters: { " + parameters + " }";
            }
            else
            {
                parameters = "parameters: {}";
            }

            var culture = "en-US";
            var cultureUI = user.SelectedCulture.Trim();

            ViewBag.UserID = user.UserID;
            ViewBag.StudentID = studentID;
            ViewBag.Culture = culture;
            ViewBag.CultureUI = cultureUI;
            ViewBag.Parameters = parameters;
            ViewBag.ExitID = exitID;
            string formName = id;
            ViewBag.Path = "";
            ViewBag.ReportType = 0;
            if (formName.Contains("Agreement"))
            {
                ViewBag.ReportType = 1;
                var reportName = id + ".trdx";

                string sourceName = "Agreement.trdx";
                string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
                string reportFullName = Path.Combine(reportDir, reportName);
                if (System.IO.Directory.Exists(reportDir))
                {
                    if (!System.IO.File.Exists(reportFullName))
                        System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
                }
            }
            if (formName.Contains("Invoices"))
            {
                ViewBag.ReportType = 1;
                var reportName = id + ".trdx";
                string sourceName = "Invoices.trdx";
                string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
                string reportFullName = Path.Combine(reportDir, reportName);
                if (System.IO.Directory.Exists(reportDir))
                {
                    if (!System.IO.File.Exists(reportFullName))
                        System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
                }
            }
            if (formName.Contains("KVKK"))
            {
                ViewBag.ReportType = 1;
                var reportName = id + ".trdx";
                string sourceName = "KVKK.trdx";
                string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
                string reportFullName = Path.Combine(reportDir, reportName);
                if (System.IO.Directory.Exists(reportDir))
                {
                    if (!System.IO.File.Exists(reportFullName))
                        System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
                }
            }

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(model: id);
        }

        #region AgreementForm
        [Route("Reporting/Designer/{id}/{studentID}/{exitID}")]
        public async Task<IActionResult> Designer(string id, int studentID, int exitID)
        {
            await Task.Delay(10);
            var parameters = string.Empty;
            var keys = Request.QueryStringKeys();
            var queryString = Request.Query;
            int userID = 0;

            foreach (var key in keys)
            {
                StringValues someInt;
                queryString.TryGetValue(key, out someInt);
                if (key == "userID") userID = Convert.ToInt32(someInt);

                parameters = string.Concat(parameters, string.Format("\"{0}\":{1},", key, someInt));
            }
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            if (!string.IsNullOrEmpty(parameters))
            {
                parameters = parameters.Substring(0, parameters.Length - 1);
                parameters = "parameters: { " + parameters + " }";
            }
            else
            {
                parameters = "parameters: {}";
            }

            var culture = "en-US";
            var cultureUI = user.SelectedCulture.Trim();

            ViewBag.UserID = user.UserID;
            ViewBag.StudentID = studentID;
            ViewBag.Culture = culture;
            ViewBag.CultureUI = cultureUI;
            ViewBag.Parameters = parameters;
            ViewBag.ExitID = exitID;

            string formName = id;
            ViewBag.Path = 0;
            if (formName.Contains("Agreement"))
            {
                ViewBag.Path = 2;
                var reportName = id + ".trdx";

                string sourceName = "Agreement.trdx";
                string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
                string reportFullName = Path.Combine(reportDir, reportName);

                if (System.IO.Directory.Exists(reportDir))
                {
                    if (!System.IO.File.Exists(reportFullName))
                        System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
                }
            }
            if (formName.Contains("KVKK"))
            {
                ViewBag.Path = 2;
                var reportName = id + ".trdx";

                string sourceName = "KVKK.trdx";
                string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
                string reportFullName = Path.Combine(reportDir, reportName);

                if (System.IO.Directory.Exists(reportDir))
                {
                    if (!System.IO.File.Exists(reportFullName))
                        System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
                }
            }
            if (formName.Contains("Invoices"))
            {
                ViewBag.Path = 2;
                var reportName = id + ".trdx";

                string sourceName = "Invoices.trdx";
                string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
                string reportFullName = Path.Combine(reportDir, reportName);

                if (System.IO.Directory.Exists(reportDir))
                {
                    if (!System.IO.File.Exists(reportFullName))
                        System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
                }
            }

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            return View(model: id);
        }
        #endregion

        #region InvoiceForm
        public async Task<IActionResult> Invoice(string id, int userID)
        {
            await Task.Delay(10);
            var parameters = string.Empty;
            var keys = Request.QueryStringKeys();
            var queryString = Request.Query;

            foreach (var key in keys)
            {
                StringValues someInt;
                queryString.TryGetValue(key, out someInt);
                if (key == "userID") userID = Convert.ToInt32(someInt);

                parameters = string.Concat(parameters, string.Format("\"{0}\":{1},", key, someInt));
            }
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);


            if (!string.IsNullOrEmpty(parameters))
            {
                parameters = parameters.Substring(0, parameters.Length - 1);
                parameters = "parameters: { " + parameters + " }";
            }
            else
            {
                parameters = "parameters: {}";
            }

            var culture = "en-US";
            var cultureUI = user.SelectedCulture.Trim();

            ViewBag.UserID = user.UserID;
            ViewBag.Culture = culture;
            ViewBag.CultureUI = cultureUI;
            ViewBag.Parameters = parameters;

            InvoiceExecuteCommand(userID);

            string url = null;
            url = "/Home/index?userID=" + user.UserID;
            return Redirect(url);
        }
        public void InvoiceExecuteCommand(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var reportName = "Invoice.trdx";

            string sourceName = "Invoice.trdx";
            string reportDir = Path.Combine(_hostEnvironment.WebRootPath, "Reports");
            string reportFullName = Path.Combine(reportDir, reportName);

            if (System.IO.Directory.Exists(reportDir))
            {
                if (!System.IO.File.Exists(reportFullName))
                    System.IO.File.Copy(reportDir + "\\" + sourceName, reportFullName, true);
            }

            try
            {
                string exepath = Path.Combine(_hostEnvironment.WebRootPath, "Reports\\Telerik.ReportDesigner.exe");

                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(exepath, reportFullName);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
            }

            catch (Exception) { }
        }
        #endregion

        #region XmlCreate
        // Not:
        // M400Invoice/XmlCreateLoop ile aynı Düzenlemeler Yapılmalıdır

        string Invoices = string.Empty;
        public IActionResult XmlCreate(int userID, int studentID, int studentInvoiceID, int schoolCode, string accountCode, int involceType, int invSW)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);

            Student student = new Student();
            student = _studentRepository.GetStudent(studentID);

            StudentInvoiceAddress studentInvoiceAddress = new StudentInvoiceAddress();
            AccountCodesDetail currentInvoiceAddress = new AccountCodesDetail();
            if (invSW == 3)
            {
                var accountCodeID = _accountCodesRepository.GetAccountCode2(accountCode, user.UserPeriod).AccountCodeID;
                currentInvoiceAddress = _accountCodesDetailRepository.GetAccountCodesDetailID1(accountCodeID);
            }
            else
            {
                if (studentID != 0)
                    studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);
                else
                {
                    if (studentInvoiceID > 0)
                    {
                        var studentInvoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID).StudentInvoiceAddressID;
                        studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddress(studentInvoice);
                    }
                }
            }

            var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);
            var invoiceDetail = _studentInvoiceDetailRepository.GetStudentInvoiceDetail(studentID, studentInvoiceID);


            var schoolTownName = "";
            var schoolCityName = "";
            var studentTownName = "";
            var studentCityName = "";
            if (school.EITownID != 0)
                schoolTownName = _parameterRepository.GetParameter(school.EITownID).CategoryName;
            if (school.EICityID != 0)
                schoolCityName = _parameterRepository.GetParameter(school.EICityID).CategoryName;
            if (studentInvoiceAddress.InvoiceTownParameterID != 0)
                studentTownName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceTownParameterID).CategoryName;
            if (studentInvoiceAddress.InvoiceCityParameterID != 0)
                studentCityName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceCityParameterID).CategoryName;

            decimal? subTotal = (cutinvoice.DUnitPrice - cutinvoice.DDiscount) - cutinvoice.DTax;

            string invCode = "";
            if (invSW == 3)
            {
                if (currentInvoiceAddress.InvoiceProfile) invCode = school.EIInvoiceSerialCode1;
                else invCode = school.EIInvoiceSerialCode2;
            }
            else
            {
                if (studentInvoiceAddress.InvoiceProfile) invCode = school.EIInvoiceSerialCode1;
                else invCode = school.EIInvoiceSerialCode2;
            }

            string invoiceSerialID = invCode.Substring(0, 3) + DateTime.Now.Year + cutinvoice.InvoiceSerialNo.ToString("000000000");
            //FATGIDEN.CSV
            CsvCreate(userID, studentID, studentInvoiceID, schoolCode, accountCode, invoiceSerialID, involceType, invSW);

            //XML 10 IF
            decimal unitPrice = Math.Round(Convert.ToDecimal(cutinvoice.DUnitPrice), school.CurrencyDecimalPlaces);
            decimal exclusiveAmount = Math.Round(Convert.ToDecimal(subTotal), school.CurrencyDecimalPlaces);
            decimal inclusiveAmount = unitPrice + Math.Round(Convert.ToDecimal(cutinvoice.DTax), school.CurrencyDecimalPlaces);
            decimal totalDiscount = Math.Round(Convert.ToDecimal(cutinvoice.DDiscount), school.CurrencyDecimalPlaces);
            decimal payableAmount = inclusiveAmount;

            decimal amount = Convert.ToDecimal(payableAmount);
            MoneyToText text = new MoneyToText();
            string inWrite = "YALNIZ:" + text.ConvertToText(amount);

            int row = 1;
            AddInv("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            //XML 02
            AddInv("<Invoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            AddInv("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"");
            AddInv("xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"");
            AddInv("xmlns:qdt=\"urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2\"");
            AddInv("xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"");
            AddInv("xmlns:udt=\"urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2\"");
            AddInv("xmlns:xades=\"http://uri.etsi.org/01903/v1.3.2#\"");
            AddInv("xmlns:ubltr=\"urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents\"");
            AddInv("xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"");
            AddInv("xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"");
            AddInv("xmlns:ccts=\"urn:un:unece:uncefact:documentation:2\"");
            AddInv("xsi:schemaLocation=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 UBL-Invoice-2.1.xsd\"");
            AddInv("xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\">");

            //XML 03
            AddInv("<cbc:UBLVersionID>2.1</cbc:UBLVersionID>");
            AddInv("<cbc:CustomizationID>TR1.2</cbc:CustomizationID>");
            AddInv("<cbc:ProfileID></cbc:ProfileID>");
            AddInv("<cbc:ID>" + invoiceSerialID + "</cbc:ID>");
            AddInv("<cbc:CopyIndicator>false</cbc:CopyIndicator>");
            AddInv("<cbc:UUID></cbc:UUID>");
            AddInv("<cbc:IssueDate>" + string.Format("{0:yyyy-MM-dd}", cutinvoice.InvoiceCutDate) + "</cbc:IssueDate>");
            AddInv("<cbc:IssueTime>" + DateTime.Now.ToString("HH:mm:ss") + "</cbc:IssueTime>");

            if (cutinvoice.WithholdingTotal > 0)
                AddInv("<cbc:InvoiceTypeCode>TEVKIFAT</cbc:InvoiceTypeCode>");
            else AddInv("<cbc:InvoiceTypeCode>SATIS</cbc:InvoiceTypeCode>");

            if (invSW == 3)
                AddInv("<cbc:Note>" + currentInvoiceAddress.Notes + "</cbc:Note>");
            else AddInv("<cbc:Note>" + studentInvoiceAddress.Notes + "</cbc:Note>");
            AddInv("<cbc:Note>" + inWrite + "</cbc:Note>");
            AddInv("<cbc:DocumentCurrencyCode listVersionID=\"2001\" listName=\"Currency\" listID=\"ISO 4217 Alpha\" listAgencyName=\"United Nations Economic Commission for Europe\">" + "TRY" + "</cbc:DocumentCurrencyCode>");
            AddInv("<cbc:LineCountNumeric>" + row + "</cbc:LineCountNumeric>");

            //XML 04 lOGO
            AddInv("<cac:AdditionalDocumentReference>");
            AddInv("<cbc:ID>invoiceSchmetronControl</cbc:ID>");
            AddInv("<cbc:IssueDate>" + string.Format("{0:yyyy-MM-dd}", cutinvoice.InvoiceCutDate) + "</cbc:IssueDate>");
            AddInv("<cac:Attachment>");
            AddInv("<cbc:EmbeddedDocumentBinaryObject characterSetCode=\"UTF-8\" encodingCode=\"Base64\" mimeCode=\"application/xml\" filename=\"efatura.xslt\">" + "</cbc:EmbeddedDocumentBinaryObject>");
            AddInv("</cac:Attachment>");
            AddInv("</cac:AdditionalDocumentReference>");

            //XML 06 
            AddInv("<cac:AccountingSupplierParty>");
            AddInv("<cac:Party>");
            AddInv("<cbc:WebsiteURI>" + school.EIWebAddress + "</cbc:WebsiteURI>");
            AddInv("<cac:PartyIdentification>");
            AddInv("<cbc:ID schemeID=\"VKN\">" + school.EITaxNo + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");
            //XML 06 Trade Register No
            AddInv("<cac:PartyIdentification>");
            AddInv("<cbc:ID schemeID=\"TICARETSICILNO\">" + school.EITaxNo + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");

            //XML Mersis
            AddInv("<cac:PartyIdentification>");
            AddInv("<cbc:ID schemeID=\"MERSISNO\">" + school.EIMersisNo + "</cbc:ID>");
            AddInv("</cac:PartyIdentification>");

            //XML Mersis Ex
            AddInv("<cac:PartyName>");
            AddInv("<cbc:Name>" + school.EITitle + "</cbc:Name>");
            AddInv("</cac:PartyName>");
            AddInv("<cac:PostalAddress>");
            AddInv("<cbc:StreetName>" + school.EIAddress + "</cbc:StreetName>");
            AddInv("<cbc:BuildingNumber />");
            AddInv("<cbc:CitySubdivisionName>" + schoolTownName + "</cbc:CitySubdivisionName>");
            AddInv("<cbc:CityName>" + schoolCityName + "</cbc:CityName>");
            AddInv("<cbc:PostalZone />");
            AddInv("<cac:Country>");
            AddInv("<cbc:Name>" + school.EICountry + "</cbc:Name>");
            AddInv("</cac:Country>");
            AddInv("</cac:PostalAddress>");
            AddInv("<cac:PartyTaxScheme>");
            AddInv("<cac:TaxScheme>");
            AddInv("<cbc:Name>" + school.EITaxOffice + "</cbc:Name>");
            AddInv("</cac:TaxScheme>");
            AddInv("</cac:PartyTaxScheme>");
            AddInv("<cac:Contact>");

            //XML Phone
            AddInv("<cbc:Telephone>" + school.EIPhone + "</cbc:Telephone>");
            //XML Fax
            AddInv("<cbc:Telefax>" + school.EIFax + "</cbc:Telefax>");
            //XML Email
            AddInv("<cbc:ElectronicMail>" + school.EIEMail + "</cbc:ElectronicMail>");
            //XML Email Ex
            AddInv("</cac:Contact>");
            AddInv("</cac:Party>");
            AddInv("</cac:AccountingSupplierParty>");
            //XML 06 END

            //XML 07
            AddInv("<cac:AccountingCustomerParty>");
            AddInv("<cac:Party>");
            AddInv("<cac:PartyIdentification>");
            if (invSW == 3)
            {
                AddInv("<cbc:ID schemeID=\"VKN\">" + currentInvoiceAddress.InvoiceTaxNumber + "</cbc:ID>");
            }
            else
            {
                if (studentInvoiceAddress.InvoiceTaxOffice != null)
                    AddInv("<cbc:ID schemeID=\"VKN\">" + studentInvoiceAddress.InvoiceTaxNumber + "</cbc:ID>");
                else
                if (student != null) AddInv("<cbc:ID schemeID=\"TCKN\">" + student.IdNumber + "</cbc:ID>");
            }
            AddInv("</cac:PartyIdentification>");

            //XML Company
            if (invSW == 3)
            {
                AddInv("<cac:PartyName>");
                AddInv("<cbc:Name>" + currentInvoiceAddress.InvoiceTitle + "</cbc:Name>");
                AddInv("</cac:PartyName>");
            }
            else
            {
                if (studentInvoiceAddress.InvoiceTaxOffice != null)
                {
                    AddInv("<cac:PartyName>");
                    AddInv("<cbc:Name>" + studentInvoiceAddress.InvoiceTitle + "</cbc:Name>");
                    AddInv("</cac:PartyName>");
                }
            }

            AddInv("<cac:PostalAddress>");
            if (invSW == 3)
                AddInv("<cbc:StreetName>" + currentInvoiceAddress.InvoiceAddress + "</cbc:StreetName>");
            else AddInv("<cbc:StreetName>" + studentInvoiceAddress.InvoiceAddress + "</cbc:StreetName>");
            AddInv("<cbc:BuildingNumber />");
            AddInv("<cbc:CitySubdivisionName>" + studentTownName + "</cbc:CitySubdivisionName>");
            AddInv("<cbc:CityName>" + studentCityName + "</cbc:CityName>");
            AddInv("<cbc:PostalZone />");
            AddInv("<cac:Country>");
            if (invSW == 3)
                AddInv("<cbc:Name>" + currentInvoiceAddress.InvoiceCountry + "</cbc:Name>");
            else AddInv("<cbc:Name>" + studentInvoiceAddress.InvoiceCountry + "</cbc:Name>");
            AddInv("</cac:Country>");
            AddInv("</cac:PostalAddress>");
            AddInv("<cac:PartyTaxScheme>");
            AddInv("<cac:TaxScheme>");
            if (invSW == 3)
                AddInv("<cbc:Name>" + currentInvoiceAddress.InvoiceTaxOffice + "</cbc:Name>");
            else AddInv("<cbc:Name>" + studentInvoiceAddress.InvoiceTaxOffice + "</cbc:Name>");

            AddInv("</cac:TaxScheme>");
            AddInv("</cac:PartyTaxScheme>");
            AddInv("<cac:Contact>");
            if (invSW == 3)
                 AddInv("<cbc:ElectronicMail>" + currentInvoiceAddress.EMail + "</cbc:ElectronicMail>");
            else AddInv("<cbc:ElectronicMail>" + studentInvoiceAddress.EMail + "</cbc:ElectronicMail>");
            AddInv("</cac:Contact>");

            string title = studentInvoiceAddress.InvoiceTitle + " " + " " + " ";
            string[] name = title.Split(' ');
            string firstName = "";
            string lastName = "";
            if (name[2] == null || name[2] == "")
            {
                firstName = name[0];
                lastName = name[1];
            }
            else
            {
                firstName = name[0] + " " + name[1];
                lastName = name[2];
            }

            //PERSON XML
            if (invSW == 3)
            {
                AddInv("<cac:Person>");
                AddInv("<cbc:FirstName>" + firstName + "</cbc:FirstName>");
                AddInv("<cbc:FamilyName>" + lastName + "</cbc:FamilyName>");
                AddInv("</cac:Person>");
            }
            else
            {
                if (studentInvoiceAddress.InvoiceTaxOffice == null)
                {
                    AddInv("<cac:Person>");
                    AddInv("<cbc:FirstName>" + firstName + "</cbc:FirstName>");
                    AddInv("<cbc:FamilyName>" + lastName + "</cbc:FamilyName>");
                    AddInv("</cac:Person>");
                }
            }

            AddInv("</cac:Party>");
            AddInv("</cac:AccountingCustomerParty>");
            //PERSON 07 ex

            //XML IBAN
            AddInv("<cac:PaymentMeans>");
            AddInv("<cbc:PaymentMeansCode>ZZZ</cbc:PaymentMeansCode>");
            AddInv("<cac:PayeeFinancialAccount>");
            AddInv("<cbc:ID>" + school.EIIban + "</cbc:ID>");
            AddInv("<cbc:CurrencyCode>\"TRY\"</cbc:CurrencyCode>");
            AddInv("<cbc:PaymentNote />");
            AddInv("</cac:PayeeFinancialAccount>");
            AddInv("</cac:PaymentMeans>");

            //XML 08
            if (cutinvoice.DDiscount > 0)
            {
                decimal discount = Math.Round(Convert.ToDecimal(cutinvoice.DDiscount), school.CurrencyDecimalPlaces);

                string discountStr = discount.ToString().Replace(',', '.');
                AddInv("<cac:AllowanceCharge>");
                AddInv("<cbc:ChargeIndicator>true</cbc:ChargeIndicator>");
                AddInv("<cbc:Amount currencyID=\"TRY\">" + discountStr + "</cbc:Amount>");
                AddInv("<cbc:BaseAmount currencyID=\"TRY\">" + discountStr + "</cbc:BaseAmount>");
                AddInv("</cac:AllowanceCharge>");
            }

            //XML 09
            AddInv("<cac:TaxTotal>");
            decimal tax = Math.Round(Convert.ToDecimal(cutinvoice.DTax), school.CurrencyDecimalPlaces);
            decimal sTotal = Math.Round(Convert.ToDecimal(subTotal), school.CurrencyDecimalPlaces);

            string taxStr = tax.ToString().Replace(',', '.');
            string sTotalStr = sTotal.ToString().Replace(',', '.');
            string dTaxPercentStr = cutinvoice.DTaxPercent.ToString().Replace(',', '.');

            AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + sTotalStr + "</cbc:TaxAmount>");
            AddInv("<cac:TaxSubtotal>");
            AddInv("<cbc:TaxableAmount currencyID=\"TRY\">" + sTotalStr + "</cbc:TaxableAmount>");
            AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + taxStr + "</cbc:TaxAmount>");
            AddInv("<cbc:CalculationSequenceNumeric>" + "1" + "</cbc:CalculationSequenceNumeric>");
            AddInv("<cbc:Percent>" + dTaxPercentStr + "</cbc:Percent>");
            AddInv("<cac:TaxCategory>");
            AddInv("<cac:TaxScheme>");
            AddInv("<cbc:Name>KDV</cbc:Name>");
            AddInv("<cbc:TaxTypeCode>0015</cbc:TaxTypeCode>");
            AddInv("</cac:TaxScheme>");
            AddInv("</cac:TaxCategory>");
            AddInv("</cac:TaxSubtotal>");
            AddInv("</cac:TaxTotal>");

            //XML Withholding (TEVKİFAT)
            if (cutinvoice.WithholdingTax > 0)
            {
                string withholdingTaxStr = cutinvoice.WithholdingTax.ToString().Replace(',', '.');

                AddInv("<cac:WithholdingTaxTotal>");
                AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + withholdingTaxStr + "</cbc:TaxAmount>");
                AddInv("<cac:TaxSubtotal>");
                AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + withholdingTaxStr + "</cbc:TaxAmount>");
                AddInv("<cbc:CalculationSequenceNumeric>" + "2" + "</cbc:CalculationSequenceNumeric>");
                AddInv("<cbc:Percent>" + "50" + "</cbc:Percent>");
                AddInv("<cac:TaxCategory>");
                AddInv("<cac:TaxScheme>");
                AddInv("<cbc:Name>" + cutinvoice.WithholdingExplanation + "</cbc:Name>");
                AddInv("<cbc:TaxTypeCode>604</cbc:TaxTypeCode>");
                AddInv("</cac:TaxScheme>");
                AddInv("/cac:TaxCategory>");
                AddInv("</cac:TaxSubtotal>");
                AddInv("</cac:WithholdingTaxTotal>");
            }

            string unitPriceStr = unitPrice.ToString().Replace(',', '.');
            string exclusiveAmountStr = exclusiveAmount.ToString().Replace(',', '.');
            string inclusiveAmountStr = inclusiveAmount.ToString().Replace(',', '.');
            string totalDiscountStr = totalDiscount.ToString().Replace(',', '.');
            string payableAmountStr = payableAmount.ToString().Replace(',', '.');

            AddInv("<cac:LegalMonetaryTotal>");
            AddInv("<cbc:LineExtensionAmount currencyID=\"TRY\">" + unitPriceStr + "</cbc:LineExtensionAmount>");
            AddInv("<cbc:TaxExclusiveAmount currencyID=\"TRY\">" + exclusiveAmountStr + "</cbc:TaxExclusiveAmount>");
            AddInv("<cbc:TaxInclusiveAmount currencyID=\"TRY\">" + inclusiveAmountStr + "</cbc:TaxInclusiveAmount>");
            AddInv("<cbc:AllowanceTotalAmount currencyID=\"TRY\">" + totalDiscountStr + "</cbc:AllowanceTotalAmount>");
            AddInv("<cbc:PayableAmount currencyID=\"TRY\">" + payableAmountStr + "</cbc:PayableAmount>");
            AddInv("</cac:LegalMonetaryTotal>");

            //XML 11
            foreach (var item in invoiceDetail)
            {
                if (item.Amount > 0)
                {
                    decimal amount2 = Math.Round(Convert.ToDecimal(item.Amount), school.CurrencyDecimalPlaces);
                    decimal tax2 = Math.Round(Convert.ToDecimal(item.Tax), school.CurrencyDecimalPlaces);
                    row++;
                    AddInv("<cac:InvoiceLine>");
                    AddInv("<cbc:ID>" + row + "</cbc:ID>");
                    AddInv("<cbc:Note />");
                    AddInv("<cbc:InvoicedQuantity unitCode=\"C62\">" + item.Quantity + "</cbc:InvoicedQuantity>");

                    string amount2Str = amount2.ToString().Replace(',', '.');
                    AddInv("<cbc:LineExtensionAmount currencyID=\"TRY\">" + amount2Str + "</cbc:LineExtensionAmount>");
                    AddInv("<cac:TaxTotal>");
                    AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + "0.00" + "</cbc:TaxAmount>");
                    AddInv("<cac:TaxSubtotal>");

                    string tax2Str = tax2.ToString().Replace(',', '.');
                    string taxPercentStr = item.TaxPercent.ToString().Replace(',', '.');

                    AddInv("<cbc:TaxableAmount currencyID=\"TRY\">" + amount2Str + "</cbc:TaxableAmount>");
                    AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + tax2Str + "</cbc:TaxAmount>");
                    AddInv("<cbc:Percent>" + taxPercentStr + "</cbc:Percent>");
                    AddInv("<cac:TaxCategory>");

                    if (item.TaxPercent == 0)
                    {
                        AddInv("<cbc:TaxExemptionReason>" + "Kdv.Kanununun 13.M bendi gereği istisna uygulanmıştır" + "</cbc:TaxExemptionReason>");
                        AddInv("<cbc:TaxExemptionReasonCode>" + "350" + "</cbc:TaxExemptionReasonCode>");
                    }

                    AddInv("<cac:TaxScheme>");
                    AddInv("<cbc:Name>" + "KDV" + "</cbc:Name>");
                    AddInv("<cbc:TaxTypeCode>" + "0015" + "</cbc:TaxTypeCode>");
                    AddInv("</cac:TaxScheme>");
                    AddInv("</cac:TaxCategory>");
                    AddInv("</cac:TaxSubtotal>");

                    //XML Withholding 
                    if (cutinvoice.WithholdingTax > 0)
                    {
                        decimal tax3 = Math.Round(Convert.ToDecimal(cutinvoice.DTax), school.CurrencyDecimalPlaces);
                        decimal tax33 = Math.Round(Convert.ToDecimal(cutinvoice.WithholdingTax), school.CurrencyDecimalPlaces);

                        string tax3Str = tax3.ToString().Replace(',', '.');
                        string tax33Str = tax33.ToString().Replace(',', '.');
                        int percent = (int)cutinvoice.WithholdingPercent1 / (int)cutinvoice.WithholdingPercent2;
                        string percentStr = tax33.ToString().Replace(',', '.');

                        AddInv("<cac:TaxSubtotal>");
                        AddInv("<cbc:TaxableAmount currencyID=\"TRY\">" + tax3Str + "</cbc:TaxableAmount>");
                        AddInv("<cbc:TaxAmount currencyID=\"TRY\">" + tax33Str + "</cbc:TaxAmount>");
                        AddInv("<cbc:CalculationSequenceNumeric>" + "2" + "</cbc:CalculationSequenceNumeric>");
                        AddInv("<cbc:Percent>" + percentStr + "</cbc:Percent>");
                        AddInv("<cac:TaxCategory>");
                        AddInv("<cac:TaxScheme>");
                        AddInv("<cbc:Name>KDV TEVKİFAT</cbc:Name>");
                        AddInv("<cbc:TaxTypeCode>9015</cbc:TaxTypeCode>");
                        AddInv("</cac:TaxScheme");
                        AddInv("</cac:TaxCategory>");
                        AddInv("</cac:TaxSubtotal");
                    }
                    AddInv("</cac:TaxTotal>");

                    //XML 12
                    decimal unitPrice2 = Math.Round(Convert.ToDecimal(item.UnitPrice), school.CurrencyDecimalPlaces);

                    AddInv("<cac:Item>");
                    AddInv("<cbc:Description />");
                    AddInv("<cbc:Name>" + item.Explanation + "</cbc:Name>");
                    AddInv("</cac:Item>");
                    AddInv("<cac:Price>");

                    string unitPrice2Str = unitPrice2.ToString().Replace(',', '.');
                    AddInv("<cbc:PriceAmount currencyID=\"TRY\">" + unitPrice2Str + "</cbc:PriceAmount>");
                    AddInv("</cac:Price>");
                    AddInv("</cac:InvoiceLine>");
                }
            }
            //XML 13
            AddInv("</Invoice>");

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", schoolCode.ToString());

            if (!Directory.Exists(projectDir))
            {
                Directory.CreateDirectory(projectDir);
            }
            string file = "FAT" + invoiceSerialID + ".xml";
            string filePath = Path.Combine(projectDir, file);

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        w.WriteLine(Invoices);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception) { }
            var url = "";
            //mehmet
            EInvoiceSending(userID);

            if (invSW == 1)
                url = "/M400Invoice/InvoicePlan?userID=" + userID + "&studentID=" + studentID;
            if (invSW == 2)
                url = "/M400Invoice/InvoiceManually?userID=" + userID;
            if (invSW == 3)
                url = "/M500Accounting/CurrentCard?userID=" + userID + "&studentID=0";
            return Redirect(url);
        }
        private void AddInv(string InvInfo)
        {
            Invoices += InvInfo + "\n";
        }

        string CsvInvoices = string.Empty;
        public void CsvCreate(int userID, int studentID, int studentInvoiceID, int schoolCode, string accounCode, string invoiceSerialID, int involceType, int invSW)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var studentInvoiceAddress = _studentInvoiceAddressRepository.GetStudentInvoiceAddressID(studentID);
            var cutinvoice = _studentInvoiceRepository.GetStudentInvoiceID(user.UserPeriod, user.SchoolID, studentInvoiceID);
            string profile = "TEMELFATURA";
            if (studentID == 0)
            {
                if (involceType == 1) profile = "TICARIFATURA";
                if (involceType == 2) profile = "ISTISNAFATURASI";
                if (involceType == 3) profile = "SATISFATURASI";
            }
            else
            {
                if (studentInvoiceAddress.InvoiceTypeParameter == 1) profile = "TICARIFATURA";
                if (studentInvoiceAddress.InvoiceTypeParameter == 2) profile = "ISTISNAFATURASI";
                if (studentInvoiceAddress.InvoiceTypeParameter == 3) profile = "SATISFATURASI";
            }

            var studentTownName = "";
            var studentCityName = "";
            if (studentInvoiceAddress.InvoiceTownParameterID != 0)
                studentTownName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceTownParameterID).CategoryName;
            if (studentInvoiceAddress.InvoiceCityParameterID != 0)
                studentCityName = _parameterRepository.GetParameter(studentInvoiceAddress.InvoiceCityParameterID).CategoryName;

            if (school.EIUserName == null) school.EIUserName = "TESTER@VRBN";
            if (school.EIUserPassword == null) school.EIUserPassword = "Vtest*2020*";

            AddCsv(school.EIUserName);
            AddCsv(school.EIUserPassword);
            AddCsv(profile);
            AddCsv(studentInvoiceAddress.InvoiceTitle);
            AddCsv(studentInvoiceAddress.InvoiceAddress);
            AddCsv(studentTownName);
            AddCsv(studentCityName);
            AddCsv(studentInvoiceAddress.InvoiceCountry);
            AddCsv(studentInvoiceAddress.InvoiceTaxOffice);
            AddCsv(studentInvoiceAddress.InvoiceTaxNumber);
            AddCsv(string.Format("{0:yyyyMMdd}", cutinvoice.InvoiceCutDate));
            AddCsv(studentInvoiceAddress.EMail);
            AddCsv(invoiceSerialID);

            string projectDir = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES", schoolCode.ToString());
            string file = "FATGIDEN.CSV";
            string filePath = Path.Combine(projectDir, file);

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        w.WriteLine(CsvInvoices);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception) { }
        }
        private void AddCsv(string CsvInfo)
        {
            CsvInvoices += CsvInfo + ";";
        }
        #endregion

        #region EInvoiceSending
        public void EInvoiceSending(int userID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string entegrator = _parameterRepository.GetParameter(school.EIIntegratorNameID).CategoryName;
            var schoolCode = user.SelectedSchoolCode;

            EInvoiceExecuteCommand(entegrator, schoolCode);
        }
        public void EInvoiceExecuteCommand(string entegrator, int schoolCode)
        {
            try
            {
                string exepath = Path.Combine(_hostEnvironment.ContentRootPath, "INVOICES");
                string dataPath = exepath + "\\" + schoolCode.ToString();
                Directory.SetCurrentDirectory(exepath);

                NCSEFatura.NCSEFatura ef = new NCSEFatura.NCSEFatura();
                ef.Execute(entegrator, dataPath);
            }

            catch (Exception) { }
        }
        #endregion

    }
}