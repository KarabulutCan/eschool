using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ITempM101Repository
    {
        IEnumerable<TempM101> GetTempM101All(int schoolID, int userID);
        TempM101 GetTempM101(int ID);
        bool CreateTempM101(TempM101 tempM101);
        bool UpdateTempM101(TempM101 tempM101);
        bool DeleteTempM101(TempM101 tempM101);
        bool ExistTempM101(int ID);

        bool Save();
    }
}
