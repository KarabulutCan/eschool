using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IAccountCodesDetailRepository
    {
        AccountCodesDetail GetAccountCodesDetailID1(int accountCodeID);
        AccountCodesDetail GetAccountCodesDetailID2(int accountCodesDetailID, int accountCodeID);
        bool CreateAccountCodesDetail(AccountCodesDetail accountCodesDetail);
        bool UpdateAccountCodesDetail(AccountCodesDetail accountCodesDetail);
        bool DeleteAccountCodesDetail(AccountCodesDetail accountCodesDetail);
        bool Save();
    }
}
