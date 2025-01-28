using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class BankRepository : IBankRepository
    {
        private SchoolDbContext _bankContext;
        public BankRepository(SchoolDbContext bankContext)
        {
            _bankContext = bankContext;
        }

        IEnumerable<Bank> IBankRepository.GetBankAll(int schoolID)
        {
            return _bankContext.Bank.OrderBy(c => c.SortOrder).Where(b => b.SchoolID == schoolID).ToList();
        }
        public Bank GetBank(int bankID)
        {
            return _bankContext.Bank.Where(b => b.BankID == bankID).FirstOrDefault();
        }
        public Bank GetBankName(int schoolID, string bankName)
        {
            return _bankContext.Bank.Where(b => b.SchoolID == schoolID && b.BankName == bankName).FirstOrDefault();
        }
        public bool CreateBank(Bank bank)
        {
            _bankContext.Add(bank);
            return Save();
        }

        public bool UpdateBank(Bank bank)
        {
            _bankContext.Update(bank);
            return Save();
        }

        public bool DeleteBank(Bank bank)
        {
            _bankContext.Remove(bank);
            return Save();
        }

        public bool ExistBank(int schoolID, int bankID)
        {
            return _bankContext.Bank.Any(c => c.SchoolID == schoolID && c.BankID == bankID);
        }
        public bool ExistBankName(int schoolID, string bankName)
        {
            return _bankContext.Bank.Any(c => c.SchoolID == schoolID && c.BankName == bankName);
        }

        public bool Save()
        {
            var saved = _bankContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
