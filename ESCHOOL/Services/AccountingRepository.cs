using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class AccountingRepository : IAccountingRepository
    {
        private SchoolDbContext _accountingContext;
        public AccountingRepository(SchoolDbContext accountingContext)
        {
            _accountingContext = accountingContext;
        }

        IEnumerable<Accounting> IAccountingRepository.GetAccountingVoucherNo(int schoolID, string period, int voucherNo)
        {
            return _accountingContext.Accounting.Where(b => b.SchoolID == schoolID && b.Period == period && b.VoucherNo == voucherNo).ToList();
        }

        IEnumerable<Accounting> IAccountingRepository.GetAccountingDocumentNumber(int schoolID, string period, string documentNumber)
        {
            return _accountingContext.Accounting.Where(b => b.SchoolID == schoolID && b.Period == period && b.DocumentNumber == documentNumber).ToList();
        }
        IEnumerable<Accounting> IAccountingRepository.GetAccountingVoucherNoFalse(int schoolID, string period, int voucherNo)
        {
            return _accountingContext.Accounting.Where(b => b.SchoolID == schoolID && b.Period == period && b.VoucherNo == voucherNo && b.IsTransaction == false).ToList();
        }
        IEnumerable<Accounting> IAccountingRepository.GetAccountingAll(int schoolID, string period)
        {
            return _accountingContext.Accounting.Where(b => b.SchoolID == schoolID && b.Period == period).ToList();
        }

        IEnumerable<Accounting> IAccountingRepository.GetAccountingCode(int schoolID, string period, string accountCode)
        {
            return _accountingContext.Accounting.Where(b => b.SchoolID == schoolID && b.Period == period && b.AccountCode == accountCode).ToList();
        }
        public Accounting GetAccountingID(int accountingID)
        {
            return _accountingContext.Accounting.Where(b => b.AccountingID == accountingID).FirstOrDefault();
        }

        public bool CreateAccounting(Accounting accounting)
        {
            _accountingContext.Add(accounting);
            return Save();
        }

        public bool UpdateAccounting(Accounting accounting)
        {
            _accountingContext.Update(accounting);
            return Save();
        }

        public bool DeleteAccounting(Accounting accounting)
        {
            _accountingContext.Remove(accounting);
            return Save();
        }
        public bool ExistAccounting(int voucherNo)
        {
            return _accountingContext.Accounting.Any(c => c.VoucherNo == voucherNo);

        }
        public bool Save()
        {
            var saved = _accountingContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
