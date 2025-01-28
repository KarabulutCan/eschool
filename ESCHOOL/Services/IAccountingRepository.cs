using ESCHOOL.Models;
using System;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IAccountingRepository
    {
        IEnumerable<Accounting> GetAccountingVoucherNo(int schoolID, string period, int voucherNo);
        IEnumerable<Accounting> GetAccountingVoucherNoFalse(int schoolID, string period, int voucherNo);
        IEnumerable<Accounting> GetAccountingDocumentNumber(int schoolID, string period, string documentNumber);
        IEnumerable<Accounting> GetAccountingAll(int schoolID, string period);
        IEnumerable<Accounting> GetAccountingCode(int schoolID, string period, string accountCode);
        Accounting GetAccountingID(int accountCodeID);

        bool CreateAccounting(Accounting accounting);
        bool UpdateAccounting(Accounting accounting);
        bool DeleteAccounting(Accounting accounting);
        bool ExistAccounting(int voucherNo);

        bool Save();
    }
}
