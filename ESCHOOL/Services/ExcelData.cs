using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class ExcelDataRepository : IExcelDataRepository
    {
        private SchoolDbContext _excelDataContext;
        public ExcelDataRepository(SchoolDbContext excelDataContext)
        {
            _excelDataContext = excelDataContext;
        }

        public IEnumerable<ExcelData> GetExcelData()
        {
            return _excelDataContext.ExcelData.ToList();
        }
        public bool CreateExcelData(ExcelData excelData)
        {
            _excelDataContext.Add(excelData);
            return Save();
        }
        public bool UpdateExcelData(ExcelData excelData)
        {
            _excelDataContext.Update(excelData);
            return Save();
        }

        public bool DeleteExcelData(ExcelData excelData)
        {
            _excelDataContext.Remove(excelData);
            return Save();
        }
        public bool Save()
        {
            var saved = _excelDataContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
