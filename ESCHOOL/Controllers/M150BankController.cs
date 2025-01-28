using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Collections.Generic;

namespace ESCHOOL.Controllers
{
    public class M150BankController : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IBankRepository _bankRepository;
        IUsersRepository _usersRepository;
        IWebHostEnvironment _hostEnvironment;
        public M150BankController(
             IBankRepository bankRepository,
             ISchoolInfoRepository schoolInfoRepository,
             IUsersRepository usersRepository,
        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _bankRepository = bankRepository;
            _usersRepository = usersRepository;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult index(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            var schoolViewModel = new SchoolViewModel
            {
                StudentID = studentID,
                UserID = userID,
                SchoolID = user.SchoolID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim()
            };
            return View(schoolViewModel);
        }

        #region read
        [Route("M150Bank/BankDataRead/{id}")]
        public IActionResult BankDataRead(int id)
        {
            var bank = _bankRepository.GetBankAll(id);

            foreach (var item in bank)
            {
                if (item.PeriodOfTime == 0 || item.PeriodOfTime < 5)
                {
                    item.PeriodOfTime = 30;
                    _bankRepository.UpdateBank(item);
                }
            }
            return Json(bank);
        }
        #endregion
        #region create, update, delete
        [HttpPost]
        public IActionResult BankDataCreate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Bank>>(strResult);
            List<Bank> list = new List<Bank>();

            var i = 0;
            foreach (var item in json)
            {
                var bank = new Bank();
                bank.BankID = 0;
                bank.SchoolID = json[i].SchoolID;
                bank.BankName = json[i].BankName;
                bank.BankBranchCode = json[i].BankBranchCode;
                bank.BankGivenCode = json[i].BankGivenCode;
                bank.AccountDecimation = json[i].AccountDecimation;
                bank.AccountNo = json[i].AccountNo;
                bank.Iban = json[i].Iban;
                bank.PeriodOfTime = json[i].PeriodOfTime;
                if (bank.PeriodOfTime == 0) bank.PeriodOfTime = 30;
                bank.SortOrder = json[i].SortOrder;
                bank.IsActive = json[i].IsActive;
                list.Add(bank);
                if (ModelState.IsValid)
                {
                    _bankRepository.CreateBank(bank);
                }
                i = i + 1;
            }
            return Json(list);
        }
        [HttpPost]
        public IActionResult BankDataUpdate([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Bank>>(strResult);

            Bank bank = new Bank();
            var i = 0;
            foreach (var item in json)
            {
                var getCode = _bankRepository.GetBank(json[i].BankID);

                getCode.BankID = json[i].BankID;
                getCode.SchoolID = json[i].SchoolID;
                getCode.BankName = json[i].BankName;
                getCode.BankBranchCode = json[i].BankBranchCode;
                getCode.BankGivenCode = json[i].BankGivenCode;
                getCode.AccountDecimation = json[i].AccountDecimation;
                getCode.AccountNo = json[i].AccountNo;
                getCode.Iban = json[i].Iban;
                getCode.PeriodOfTime = json[i].PeriodOfTime;
                if (bank.PeriodOfTime == 0) bank.PeriodOfTime = 30;
                getCode.SortOrder = json[i].SortOrder;
                getCode.IsActive = json[i].IsActive;
                if (ModelState.IsValid)
                {
                    _bankRepository.UpdateBank(getCode);
                }
                i = i + 1;
            }

            return Json(bank);
        }
        [HttpPost]
        public IActionResult BankDataDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<Bank>>(strResult);
            List<Bank> list = new List<Bank>();

            var i = 0;
            foreach (var item in json)
            {
                var bank = new Bank();
                bank.BankID = json[i].BankID;
                bank.SchoolID = json[i].SchoolID;
                bank.BankName = json[i].BankName;
                bank.BankBranchCode = json[i].BankBranchCode;
                bank.BankGivenCode = json[i].BankGivenCode;
                bank.AccountDecimation = json[i].AccountDecimation;
                bank.AccountNo = json[i].AccountNo;
                bank.Iban = json[i].Iban;
                bank.PeriodOfTime = json[i].PeriodOfTime;
                bank.SortOrder = json[i].SortOrder;
                bank.IsActive = json[i].IsActive;
                list.Add(bank);
                if (ModelState.IsValid)
                {
                    _bankRepository.DeleteBank(bank);
                }
                i = i + 1;
            }
            return Json(list);
        }
        #endregion


    }
}