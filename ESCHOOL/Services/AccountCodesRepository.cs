using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class AccountCodesRepository : IAccountCodesRepository
    {
        private SchoolDbContext _accountCodesContext;
        public AccountCodesRepository(SchoolDbContext accountCodesContext)
        {
            _accountCodesContext = accountCodesContext;
        }

        IEnumerable<AccountCodes> IAccountCodesRepository.GetAccountCodeAllTrue(string period)
        {
            return _accountCodesContext.AccountCodes.OrderBy(c => c.AccountCode).Where(b => b.Period == period && b.IsActive == true).ToList();
        }
        IEnumerable<AccountCodes> IAccountCodesRepository.GetAccountCodeAll(string period)
        {
            return _accountCodesContext.AccountCodes.Where(c=> c.Period == period).OrderBy(c => c.AccountCode).ToList();
        }

        IEnumerable<AccountCodes> IAccountCodesRepository.GetCurrentCard(string period)
        {
            return _accountCodesContext.AccountCodes.OrderBy(c => c.AccountCodeName).Where(b => b.Period == period && b.IsActive == true && b.IsCurrentCard == true).ToList();
        }
        public AccountCodes GetAccountCodeID(int accountCodeID, string period)
        {
            return _accountCodesContext.AccountCodes.Where(b => b.Period == period && b.AccountCodeID == accountCodeID).FirstOrDefault();
        }
        public AccountCodes GetAccountCode(string accountCodes, string period)
        {
            return _accountCodesContext.AccountCodes.Where(b => b.Period == period && b.AccountCode == accountCodes).FirstOrDefault();
        }
        public AccountCodes GetAccountCode2(string accountCodes, string period)
        {
            return _accountCodesContext.AccountCodes.Where(b => b.Period == period && b.AccountCode == accountCodes && b.IsCurrentCard == true).FirstOrDefault();
        }
        public AccountCodes GetAccountName(string name, string period)
        {
            return _accountCodesContext.AccountCodes.Where(b => b.Period == period && b.AccountCodeName == name).FirstOrDefault();
        }
        public bool CreateAccountCode(AccountCodes accountCodes)
        {
            _accountCodesContext.Add(accountCodes);
            return Save();
        }

        public bool UpdateAccountCode(AccountCodes accountCodes)
        {
            _accountCodesContext.Update(accountCodes);
            return Save();
        }

        public bool DeleteAccountCode(AccountCodes accountCode)
        {
            _accountCodesContext.Remove(accountCode);
            return Save();
        }
        public bool ExistAccountCode(string period, string accountCodes)
        {
            return _accountCodesContext.AccountCodes.Any(c => c.Period == period && c.AccountCode == accountCodes);
        }
        public bool Save()
        {
            var saved = _accountCodesContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
