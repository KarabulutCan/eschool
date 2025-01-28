using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IDiscountTableRepository
    {
        IEnumerable<DiscountTable> GetDiscountTableAll();
        IEnumerable<DiscountTable> GetDiscountTablePeriod(string period, int schoolID);
        IEnumerable<DiscountTable> GetDiscountTablePeriodOnlyTrue(string period, int schoolID);
        IEnumerable<DiscountTable> GetDiscountTablePeriod2(string period);
        IEnumerable<DiscountTable> GetDiscountTableIDPeriod(int? schoolID, string period);
        DiscountTable GetDiscountTablePeriod3(int ID, string period, int? schoolID);
        DiscountTable GetDiscountTable(int discountTableID);

        bool CreateDiscountTable(DiscountTable discountTable);
        bool UpdateDiscountTable(DiscountTable discountTable);
        bool DeleteDiscountTable(DiscountTable discountTable);

        bool Save();
    }
}
