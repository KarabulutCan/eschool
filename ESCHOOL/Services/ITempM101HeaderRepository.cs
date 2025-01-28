using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ITempM101HeaderRepository
    {
        IEnumerable<TempM101Header> GetTempM101HeaderAll(int schoolID, int userID);
        bool CreateTempM101Header(TempM101Header tempM101Header);
        bool UpdateTempM101Header(TempM101Header tempM101Header);
        bool DeleteTempM101Header(TempM101Header tempM101Header);
        bool ExistTempM101Header(int ID);

        bool Save();
    }
}
