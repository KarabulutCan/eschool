using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Collections.Generic;
using System.Linq;


namespace ESCHOOL.Controllers
{
    public class M130SchoolFee : Controller
    {
        ISchoolInfoRepository _schoolInfoRepository;
        IParameterRepository _parameterRepository;
        ISchoolFeeTableRepository _schoolFeeTableRepository;
        ISchoolFeeRepository _schoolFeeRepository;
        IUsersRepository _usersRepository;
        IUsersWorkAreasRepository _usersWorkAreasRepository;
        IWebHostEnvironment _hostEnvironment;
        public M130SchoolFee(
             ISchoolInfoRepository schoolInfoRepository,
             IParameterRepository parameterRepository,
             ISchoolFeeTableRepository schoolFeeTableRepository,
             ISchoolFeeRepository schoolFeeRepository,
             IUsersRepository usersRepository,
             IUsersWorkAreasRepository usersWorkAreasRepository,

        IWebHostEnvironment hostEnvironment)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _parameterRepository = parameterRepository;
            _schoolFeeTableRepository = schoolFeeTableRepository;
            _schoolFeeRepository = schoolFeeRepository;
            _usersRepository = usersRepository;
            _usersWorkAreasRepository = usersWorkAreasRepository;

            _hostEnvironment = hostEnvironment;
        }

        public IActionResult index(int userID, int studentID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID = _parameterRepository.GetParameterCategoryName("Tahsil/Tediye İşlemleri").CategoryID;
            bool isPermission = _usersWorkAreasRepository.GetUsersWorkAreas(userID, categoryID).IsSelect;

            var school = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            string date = string.Format("{0:dd/MM/yyyy}", user.UserDate);

            TempData["SchoolName"] = school.CompanyName;
            TempData["Period"] = user.UserPeriod;
            TempData["Date"] = date;

            ThemeController theme = new ThemeController();
            TempData["theme"] = theme.GetTheme(user.SelectedTheme);
            TempData["themeMobile"] = theme.GetThemeMobile(user.SelectedTheme);
            TempData["color"] = theme.GetThemeColor(user.SelectedTheme);

            string categoryName = "categoryName";
            string categoryNameSub = "name";
            if (user.SelectedCulture.Trim() == "en-US")
            {
                categoryName = "language1";
                categoryNameSub = "language1";
            }

            var schoolViewModel = new SchoolViewModel
            {
                IsPermission = isPermission,
                StudentID = studentID,
                UserID = userID,
                SchoolID = user.SchoolID,
                UserPeriod = user.UserPeriod,
                SelectedCulture = user.SelectedCulture.Trim(),
                CategoryName = categoryName,
                CategoryNameSub = categoryNameSub
            };
            return View(schoolViewModel);
        }

        [Route("M130SchoolFee/SchoolFeeRead/{userID}/{L}")]
        public IActionResult SchoolFeeRead(int userID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var schoolInfo = _schoolInfoRepository.GetSchoolInfo(user.SchoolID);
            var period = user.UserPeriod;

            var categoryID = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID).Where(b=> b.CategoryName != null && b.CategoryName.Trim() != "0");

            var schoolFee = _schoolFeeRepository.GetSchoolFeeAllLevel(user.SchoolID, "L1");
            //var schoolFee = _schoolFeeRepository.GetSchoolFeeAllLevel(user.SchoolID, "L1");
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTableAll(user.SchoolID, user.UserPeriod);

            //False olan kayıtlar varsa SchoolFeeTable dosyasından siliniyor...
            var deletefile = new List<SchoolFeeTable>();
            foreach (var fee in schoolFee)
            {
                if (fee.IsActive == false)
                {
                    deletefile = schoolFeeTable.Where(c => c.SchoolFeeID == fee.SchoolFeeID).ToList();
                }
            }
            if (ModelState.IsValid)
            {
                if (deletefile.Count() > 0)
                {
                    foreach (var df in deletefile)
                    {
                        _schoolFeeTableRepository.DeleteSchoolFeeTable(df);
                    }
                }
                foreach (var df in schoolFeeTable)
                {
                    if (df.FeeCategory == 0)
                    {
                        _schoolFeeTableRepository.DeleteSchoolFeeTable(df);
                    }
                }
            }

            schoolFee = _schoolFeeRepository.GetSchoolFeeLevel(user.SchoolID, L);
            var sortNoSW = 0;
            if (schoolFeeTable.Count() > 0) sortNoSW = 1;
            foreach (var item in parameterList)
            {
                var sortNo = 0;
                // SCHOOL FEE
                foreach (var feeitem in schoolFee.OrderBy(c => c.SortOrder).OrderBy(p => p.FeeCategory))
                {
                    int? ID = feeitem.SchoolFeeID;
                    var schoolFeeItem = schoolFeeTable.Any(c => c.SchoolFeeID == ID && c.SchoolID == user.SchoolID && c.Period == user.UserPeriod);

                    // False ise yeni kayıt yazılıyor, True ise sadece parameterList gönderiliyor.
                    if (!schoolFeeItem)
                    {
                        var sft = new SchoolFeeTable();
                        sft.SchoolID = user.SchoolID;
                        sft.CategoryID = item.CategoryID;
                        sft.FeeCategory = feeitem.FeeCategory;
                        sft.Period = period;
                        sft.SchoolFeeID = feeitem.SchoolFeeID;
                        sft.SchoolFeeSubID = 0;
                        sft.SchoolFeeTypeAmount = 0;
                        sft.StockQuantity = 0;
                        sft.StockCode = "";
                        if (sortNoSW == 0)
                        {
                            sortNo++;
                        }
                        else
                        {
                            sortNo = (int)schoolFeeTable.Where(a => a.SchoolID == user.SchoolID).Max(a => a.SortOrder) + 1;
                        }

                        sft.SortOrder = sortNo;
                        sft.IsActive = true;

                        _schoolFeeTableRepository.CreateSchoolFeeTable(sft);

                    }
                }
            }
            return Json(parameterList);
        }

        [Route("M130SchoolFee/SchoolFeeDetailRead/{userID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeDetailRead(int userID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(user.SchoolID, L);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllStatus(user.SchoolID, period, categoryID);

            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            foreach (var item in schoolFeeTable)
            {
                var parameter = parameterList.FirstOrDefault(p => p.CategoryID == item.CategoryID);
                if (parameter == null)
                {
                    parameter = parameterList.FirstOrDefault(p => p.CategorySubID == categoryID);
                }

                var schoolFee = schoolFeeList.FirstOrDefault(p => p.SchoolFeeID == item.SchoolFeeID);

                var schoolFeeViewModel = new SchoolFeeViewModel
                {
                    ViewModelID = (int)item.SchoolFeeID,
                    Parameter = parameter,
                    SchoolFeeTable = item,
                    SchoolFee = schoolFee,
                };

                list.Add(schoolFeeViewModel);
            }
            return Json(list);
        }

        [Route("M130SchoolFee/SchoolFeeMoreDetailRead/{userID}/{schoolFeeID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeMoreDetailRead(int userID, int schoolFeeID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel3(user.SchoolID, schoolFeeID, L);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, period, schoolFeeID);

            var schoolFeeTableDetail = schoolFeeTable.Where(b => schoolFeeList.Where(c => b.SchoolFeeSubID == categoryID).Count() > 0).ToList();

            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            foreach (var item in schoolFeeTableDetail)
            {
                var schoolFee = new SchoolFee();
                //if (L == "L2")
                //    schoolFee = schoolFeeList.FirstOrDefault(p => p.CategorySubID == item.SchoolFeeID);
                //else
                schoolFee = _schoolFeeRepository.GetSchoolFee(item.CategoryID);

                string categoryName = schoolFee.Name;
                if (user.SelectedCulture.Trim() == "en-US")
                {
                    categoryName = schoolFee.Language1;
                }
                if (item.SchoolFeeTypeAmount == null) item.SchoolFeeTypeAmount = 0;
                if (item.StockQuantity == null) item.StockQuantity = 0;
                var schoolFeeViewModel = new SchoolFeeViewModel
                {
                    ViewModelID = (int)item.CategoryID,
                    SchoolFeeID = (int)item.SchoolFeeID,

                    //Create runnning
                    FeeCategory = (int)item.FeeCategory,
                    SchoolFeeName = categoryName,
                    SchoolFeeTypeAmount = (decimal)item.SchoolFeeTypeAmount,
                    StockQuantity = (int)item.StockQuantity,
                    StockCode = item.StockCode,
                    Tax = schoolFee.Tax,
                    SchoolID = item.SchoolID,
                    Period = item.Period,

                    SortOrder = (int)item.SortOrder,
                    IsActive = item.IsActive,

                    /////////////////

                    SchoolFeeTable = item,
                    SchoolFee = schoolFee,
                };

                list.Add(schoolFeeViewModel);
            }
            return Json(list);
        }


        [Route("M130SchoolFee/SchoolFeeMoreDetailRead2/{userID}/{viewModelID}/{schoolFeeID}/{categoryID}/{L}")]
        public IActionResult SchoolFeeMoreDetailRead2(int userID, int viewModelID, int schoolFeeID, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel3(user.SchoolID, viewModelID, L);
            var schoolFeeTable = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, period, viewModelID);


            // 1435 1433    
            //var schoolFeeTableDetail = schoolFeeTable.Where(b => schoolFeeList.Where(c => c.SchoolFeeID == b.CategoryID && c.SchoolFeeSubID == b.SchoolFeeID && categoryID == b.SchoolFeeSubID).Count() > 0).ToList();
            var schoolFeeTableDetail = schoolFeeTable.Where(b => schoolFeeList.Where(c => c.SchoolFeeID == b.CategoryID && c.SchoolFeeSubID == viewModelID).Count() > 0).ToList();

            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            foreach (var item in schoolFeeTableDetail)
            {
                var schoolFee = new SchoolFee();
                schoolFee = _schoolFeeRepository.GetSchoolFee(item.CategoryID);
                string categoryName = schoolFee.Name;
                if (user.SelectedCulture.Trim() == "en-US")
                {
                    categoryName = schoolFee.Language1;
                }

                var schoolFeeViewModel = new SchoolFeeViewModel
                {
                    ViewModelID = (int)item.CategoryID,
                    SchoolFeeID = (int)item.SchoolFeeID,

                    //Create runnning
                    FeeCategory = (int)item.FeeCategory,
                    SchoolFeeName = categoryName,
                    SchoolFeeTypeAmount = (int)item.SchoolFeeTypeAmount,
                    StockQuantity = (int)item.StockQuantity,
                    StockCode = item.StockCode,
                    Tax = schoolFee.Tax,
                    SchoolID = item.SchoolID,
                    Period = item.Period,

                    SortOrder = (int)item.SortOrder,
                    IsActive = item.IsActive,

                    /////////////////

                    SchoolFeeTable = item,
                    SchoolFee = schoolFee,
                };

                list.Add(schoolFeeViewModel);
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M130SchoolFee/SchoolFeeDetailCreate/{strResult}/{userID}/{id}/{categoryID}/{L}")]
        public IActionResult SchoolFeeDetailCreate([Bind(Prefix = "models")] string strResult, int userID, int id, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);
            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();

            foreach (var item in json.OrderBy(s => s.SchoolFeeName))
            {
                //SchoolFee
                var fee = new SchoolFee();
                fee.SchoolFeeID = 0;
                fee.FeeCategory = 1;
                fee.SchoolID = user.SchoolID;
                fee.Name = item.SchoolFeeName;
                if (user.SelectedCulture == "en-US")
                    fee.Language1 = item.SchoolFeeName;
                fee.SchoolFeeSubID = id;
                fee.CategoryLevel = L;
                fee.Tax = 8;

                var sortNo = 0;
                var lastNo = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1");
                if (lastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)lastNo.Max(a => a.SortOrder);
                    sortNo++;
                }

                if (fee.SortOrder == 0 || fee.SortOrder == null)
                {
                    fee.SortOrder = sortNo;
                }

                fee.IsActive = item.IsActive;
                fee.IsSelect = true;
                _schoolFeeRepository.CreateSchoolFee(fee);

                var feeTable = new SchoolFeeTable();
                feeTable.SchoolFeeTableID = 0;
                feeTable.FeeCategory = 1;
                feeTable.SchoolID = user.SchoolID;
                feeTable.Period = user.UserPeriod;
                feeTable.CategoryID = fee.SchoolFeeID;
                feeTable.SchoolFeeID = id;
                feeTable.SchoolFeeSubID = categoryID;
                feeTable.SchoolFeeTypeAmount = item.SchoolFeeTypeAmount;
                feeTable.StockQuantity = item.StockQuantity;
                feeTable.StockCode = item.StockCode;
                var sortNo2 = 0;
                var lastNo2 = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, user.UserPeriod, id);
                if (lastNo2.Count() == 0)
                {
                    sortNo2 = 1;
                }
                else
                {
                    sortNo2 = (int)lastNo2.Max(a => a.SortOrder);
                    sortNo2++;
                }

                if (feeTable.SortOrder == 0 || feeTable.SortOrder == null)
                {
                    feeTable.SortOrder = sortNo2;
                }

                feeTable.IsActive = item.IsActive;
                if (ModelState.IsValid)
                {
                    _schoolFeeTableRepository.CreateSchoolFeeTable(feeTable);
                }
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M130SchoolFee/SchoolFeeDetailCreate2/{strResult}/{userID}/{id}/{categoryID}/{L}")]
        public IActionResult SchoolFeeDetailCreate2([Bind(Prefix = "models")] string strResult, int userID, int id, int categoryID, string L)
        {
            var user = _usersRepository.GetUser(userID);
            var period = user.UserPeriod;

            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);
            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();

            foreach (var item in json.OrderBy(s => s.SchoolFeeName))
            {
                //SchoolFee
                var fee = new SchoolFee();
                fee.SchoolFeeID = 0;
                fee.FeeCategory = 1;
                fee.SchoolID = user.SchoolID;
                fee.Name = item.SchoolFeeName;
                if (user.SelectedCulture == "en-US")
                    fee.Language1 = item.SchoolFeeName;
                fee.SchoolFeeSubID = id;
                fee.CategoryLevel = L;
                fee.Tax = 8;

                var sortNo = 0;
                var lastNo = _schoolFeeRepository.GetSchoolFeeAll(user.SchoolID, "L1");
                if (lastNo.Count() == 0)
                {
                    sortNo = 1;
                }
                else
                {
                    sortNo = (int)lastNo.Max(a => a.SortOrder);
                    sortNo++;
                }

                if (fee.SortOrder == 0 || fee.SortOrder == null)
                {
                    fee.SortOrder = sortNo;
                }

                fee.IsActive = item.IsActive;
                fee.IsSelect = true;
                _schoolFeeRepository.CreateSchoolFee(fee);

                var feeTable = new SchoolFeeTable();
                feeTable.SchoolFeeTableID = 0;
                feeTable.FeeCategory = 1;
                feeTable.SchoolID = user.SchoolID;
                feeTable.Period = user.UserPeriod;
                feeTable.CategoryID = fee.SchoolFeeID;
                feeTable.SchoolFeeID = id;
                feeTable.SchoolFeeSubID = categoryID;
                feeTable.SchoolFeeTypeAmount = item.SchoolFeeTypeAmount;
                feeTable.StockQuantity = item.StockQuantity;
                feeTable.StockCode = item.StockCode;
                var sortNo2 = 0;
                var lastNo2 = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, user.UserPeriod, id);
                if (lastNo2.Count() == 0)
                {
                    sortNo2 = 1;
                }
                else
                {
                    sortNo2 = (int)lastNo2.Max(a => a.SortOrder);
                    sortNo2++;
                }

                if (feeTable.SortOrder == 0 || feeTable.SortOrder == null)
                {
                    feeTable.SortOrder = sortNo2;
                }

                feeTable.IsActive = item.IsActive;
                if (ModelState.IsValid)
                {
                    _schoolFeeTableRepository.CreateSchoolFeeTable(feeTable);
                }
            }
            return Json(list);
        }


        [HttpPost]
        [Route("M130SchoolFee/SchoolFeeDetailUpdate1/{strResult}/{userID}/{L}")]
        public IActionResult SchoolFeeDetailUpdate1([Bind(Prefix = "models")] string strResult, int userID, string L)
        {
            var user = _usersRepository.GetUser(userID);

            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);

            SchoolFee fee = new SchoolFee();
            SchoolFeeTable getCode = new SchoolFeeTable();
            var i = 0;
            foreach (var item in json)
            {
                fee = _schoolFeeRepository.GetSchoolFee(item.ViewModelID);
                fee.Name = json[i].SchoolFee.Name;
                if (user.SelectedCulture == "en-US")
                    fee.Language1 = json[i].SchoolFee.Name;
                fee.SortOrder = json[i].SchoolFee.SortOrder;
                fee.IsActive = json[i].SchoolFee.IsActive;
                _schoolFeeRepository.UpdateSchoolFee(fee);

                getCode = _schoolFeeTableRepository.GetSchoolFeeTable(json[i].SchoolFeeTable.SchoolFeeTableID);
                getCode.SchoolFeeTableID = json[i].SchoolFeeTable.SchoolFeeTableID;
                getCode.SchoolID = json[i].SchoolFeeTable.SchoolID;
                //getCode.CategoryID = json[i].SchoolFeeTable.CategoryID;
                getCode.FeeCategory = json[i].SchoolFeeTable.FeeCategory;
                getCode.Period = json[i].SchoolFeeTable.Period;
                getCode.SchoolFeeID = json[i].SchoolFeeTable.SchoolFeeID;
                getCode.SchoolFeeTypeAmount = json[i].SchoolFeeTable.SchoolFeeTypeAmount;
                getCode.StockQuantity = json[i].SchoolFeeTable.StockQuantity;
                getCode.StockCode = json[i].SchoolFeeTable.StockCode;
                getCode.SortOrder = json[i].SchoolFeeTable.SortOrder;
                getCode.IsActive = json[i].SchoolFeeTable.IsActive;

                _schoolFeeTableRepository.UpdateSchoolFeeTable(getCode);

                i = i + 1;
            }

            var paremeterList = _parameterRepository.GetParameterSubID(8);
            var paremeter = paremeterList.FirstOrDefault(p => p.CategoryID == json[0].SchoolFeeTable.CategoryID);

            var ID = json[0].SchoolFeeTable.SchoolFeeID;
            var schoolID = json[0].SchoolFeeTable.SchoolID;

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(schoolID, "L1");

            var schoolFee = schoolFeeList.FirstOrDefault(p => p.SchoolFeeID == ID);

            var schoolFeeViewModel = new SchoolFeeViewModel
            {
                ViewModelID = json[0].SchoolFeeTable.SchoolFeeTableID,
                Parameter = paremeter,
                SchoolFeeTable = getCode,
                SchoolFee = schoolFee,
                SchoolInfo = json[0].SchoolInfo,

            };
            return Json(schoolFeeViewModel);
        }

        [HttpPost]
        [Route("M130SchoolFee/SchoolFeeDetailUpdate2/{strResult}/{userID}/{L}")]
        public IActionResult SchoolFeeDetailUpdate2([Bind(Prefix = "models")] string strResult, int userID, string L)
        {
            var user = _usersRepository.GetUser(userID);

            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);

            SchoolFee fee = new SchoolFee();
            SchoolFeeTable getCode = new SchoolFeeTable();
            var i = 0;
            foreach (var item in json)
            {
                fee = _schoolFeeRepository.GetSchoolFee(item.ViewModelID);
                fee.Name = json[i].SchoolFeeName;
                if (user.SelectedCulture == "en-US")
                    fee.Language1 = json[i].SchoolFeeName;
                fee.SortOrder = json[i].SortOrder;
                fee.IsActive = json[i].IsActive;
                _schoolFeeRepository.UpdateSchoolFee(fee);

                getCode = _schoolFeeTableRepository.GetSchoolFeeTable(json[i].SchoolFeeTable.SchoolFeeTableID);
                getCode.SchoolFeeTableID = json[i].SchoolFeeTable.SchoolFeeTableID;
                getCode.SchoolID = json[i].SchoolID;
                //getCode.CategoryID = json[i].SchoolFeeTable.CategoryID;
                getCode.FeeCategory = json[i].FeeCategory;
                getCode.Period = json[i].Period;
                getCode.SchoolFeeID = json[i].SchoolFeeID;
                getCode.SchoolFeeTypeAmount = json[i].SchoolFeeTypeAmount;
                getCode.StockQuantity = json[i].StockQuantity;
                getCode.StockCode = json[i].StockCode;
                getCode.SortOrder = json[i].SortOrder;
                getCode.IsActive = json[i].IsActive;

                _schoolFeeTableRepository.UpdateSchoolFeeTable(getCode);
                i = i + 1;
            }

            var paremeterList = _parameterRepository.GetParameterSubID(8);
            var paremeter = paremeterList.FirstOrDefault(p => p.CategoryID == json[0].SchoolFeeTable.CategoryID);

            var ID = json[0].SchoolFeeTable.SchoolFeeID;
            var schoolID = json[0].SchoolFeeTable.SchoolID;

            var schoolFeeList = _schoolFeeRepository.GetSchoolFeeLevel(schoolID, "L1");

            var schoolFee = schoolFeeList.FirstOrDefault(p => p.SchoolFeeID == ID);

            var schoolFeeViewModel = new SchoolFeeViewModel
            {
                ViewModelID = json[0].SchoolFeeTable.SchoolFeeTableID,
                Parameter = paremeter,
                SchoolFeeTable = getCode,
                SchoolFee = schoolFee,
                SchoolInfo = json[0].SchoolInfo,

            };
            return Json(schoolFeeViewModel);
        }

        [HttpPost]
        public IActionResult SchoolFeeDetailDelete([Bind(Prefix = "models")] string strResult)
        {
            var json = new JavaScriptSerializer().Deserialize<List<SchoolFeeViewModel>>(strResult);
            List<SchoolFeeViewModel> list = new List<SchoolFeeViewModel>();
            SchoolFee fee = new SchoolFee();
            SchoolFeeTable getCode = new SchoolFeeTable();
            var i = 0;
            foreach (var item in json)
            {
                fee = _schoolFeeRepository.GetSchoolFee(item.ViewModelID);
                _schoolFeeRepository.DeleteSchoolFee(fee);

                getCode = _schoolFeeTableRepository.GetSchoolFeeTable(json[i].SchoolFeeTable.SchoolFeeTableID);
                _schoolFeeTableRepository.DeleteSchoolFeeTable(getCode);
                i++;
            }
            return Json(list);
        }

        [HttpPost]
        [Route("M130SchoolFee/SchoolFeeDetailTransfer/{userID}/{viewModelID}/{schoolFeeID}/{schoolFeeSubID}")]
        public IActionResult SchoolFeeDetailTransfer(int userID, int viewModelID, int schoolFeeID, int schoolFeeSubID)
        {
            var user = _usersRepository.GetUser(userID);
            var categoryID2 = _parameterRepository.GetParameterCategoryName("Sınıf Tipleri").CategoryID;
            var parameterList = _parameterRepository.GetParameterSubID(categoryID2);
            var schoolFeeSource = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID(user.SchoolID, user.UserPeriod, schoolFeeID);

            //public SchoolFeeTable GetSchoolFees2(int schoolID, string period, int feeCategory, int categoryID, int schoolFeeID)

            var sFT = _schoolFeeTableRepository.GetSchoolFees22(user.SchoolID, user.UserPeriod, 1, viewModelID, schoolFeeID, schoolFeeSubID);
            foreach (var item in parameterList)
            {
                if (item.CategoryID != schoolFeeSubID)
                {
                    var sortNo = 0;
                    //foreach (var s in schoolFeeSource)
                    //{
                    var schoolFeeTarget = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID21(user.SchoolID, user.UserPeriod, item.CategoryID, schoolFeeID, schoolFeeSubID).Where(b => b.FeeCategory == 1).ToList();
                    //var schoolFeeTarget = _schoolFeeTableRepository.GetSchoolFeeTablePeriodAllSchoolFeeID2(user.SchoolID, user.UserPeriod, viewModelID, schoolFeeID).Where(b => b.FeeCategory == 1).ToList();
                    if (schoolFeeTarget.Count() == 0)
                    {
                        var sortNoSW = 0;
                        if (schoolFeeTarget.Count() > 0) sortNoSW = 1;

                        var sft = new SchoolFeeTable();
                        sft.SchoolFeeTableID = 0;
                        sft.SchoolID = user.SchoolID;
                        sft.CategoryID = viewModelID;
                        sft.FeeCategory = 1;
                        sft.Period = user.UserPeriod;
                        sft.SchoolFeeID = schoolFeeID;
                        sft.SchoolFeeSubID = item.CategoryID; ;
                        sft.SchoolFeeTypeAmount = sFT.SchoolFeeTypeAmount;
                        sft.StockQuantity = sFT.StockQuantity;
                        sft.StockCode = sFT.StockCode;
                        if (sortNoSW == 0)
                        {
                            sortNo++;
                        }
                        else
                        {
                            sortNo = (int)schoolFeeTarget.Where(a => a.SchoolID == user.SchoolID).Max(a => a.SortOrder) + 1;
                        }

                        sft.SortOrder = sortNo;
                        sft.IsActive = true;

                        _schoolFeeTableRepository.CreateSchoolFeeTable(sft);
                    }
                }
                //}
            }
            return Json(true);
        }
    }
}




