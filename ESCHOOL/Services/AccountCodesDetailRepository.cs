using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace ESCHOOL.Services
{
    public class AccountCodesDetailRepository : IAccountCodesDetailRepository
    {
        private SchoolDbContext _accountCodesDetailContext;
        public AccountCodesDetailRepository(SchoolDbContext accountCodesDetailContext)
        {
            _accountCodesDetailContext = accountCodesDetailContext;
        }

        public AccountCodesDetail GetAccountCodesDetailID1(int accountCodeID)
        {
            return _accountCodesDetailContext.AccountCodesDetail.Where(b => b.AccountCodeID == accountCodeID).FirstOrDefault();
        }
        public AccountCodesDetail GetAccountCodesDetailID2(int accountCodesDetailID, int accountCodeID)
        {
            return _accountCodesDetailContext.AccountCodesDetail.Where(b => b.AccountCodeDetailID == accountCodesDetailID && b.AccountCodeID == accountCodeID).FirstOrDefault();
        }
       
        public bool CreateAccountCodesDetail(AccountCodesDetail accountCodesDetail)
        {
            _accountCodesDetailContext.Add(accountCodesDetail);
            return Save();
        }

        public bool UpdateAccountCodesDetail(AccountCodesDetail accountCodesDetail)
        {
            _accountCodesDetailContext.Update(accountCodesDetail);
            return Save();
        }

        public bool DeleteAccountCodesDetail(AccountCodesDetail accountCodesDetail)
        {
            _accountCodesDetailContext.Remove(accountCodesDetail);
            return Save();
        }

        public bool Save()
        {
            var saved = _accountCodesDetailContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
