using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IExcelDataRepository
    {
        IEnumerable<ExcelData> GetExcelData();

        bool CreateExcelData(ExcelData excelData);
        bool UpdateExcelData(ExcelData excelData);
        bool DeleteExcelData(ExcelData excelData);
        bool Save();
    }
}
