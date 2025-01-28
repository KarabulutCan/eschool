using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IAccountCodesRepository
    {
        IEnumerable<AccountCodes> GetAccountCodeAll(string period);
        IEnumerable<AccountCodes> GetAccountCodeAllTrue(string period);
        IEnumerable<AccountCodes> GetCurrentCard(string period);
        
        AccountCodes GetAccountCodeID(int code, string period);
        AccountCodes GetAccountCode(string code, string period);
        AccountCodes GetAccountCode2(string code, string period);
        AccountCodes GetAccountName(string name, string period);
        bool CreateAccountCode(AccountCodes accountCodes);
        bool UpdateAccountCode(AccountCodes accountCodes);
        bool DeleteAccountCode(AccountCodes accountCodes);
        bool ExistAccountCode(string period, string code);
        bool Save();
    }
}
