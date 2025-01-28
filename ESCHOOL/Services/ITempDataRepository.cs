using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ITempDataRepository
    {
        IEnumerable<TempData> GetTempData(int tempDataID);
        bool CreateTempData(TempData tempData);
        bool UpdateTempData(TempData tempData);
        bool DeleteTempData(TempData tempData);
        bool ExistTempData(int tempData);

        bool Save();
    }
}
