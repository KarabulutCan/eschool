using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IBankRepository
    {
        IEnumerable<Bank> GetBankAll(int schoolID);
        Bank GetBank(int bankID);
        Bank GetBankName(int schoolID, string bankName);
        bool CreateBank(Bank bank);
        bool UpdateBank(Bank bank);
        bool DeleteBank(Bank bank);
        bool ExistBank(int schoolID, int bankID);
        bool ExistBankName(int schoolID, string bankName);

        bool Save();
    }
}
