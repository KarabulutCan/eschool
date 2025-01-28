using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class DiscountTableRepository : IDiscountTableRepository
    {
        private SchoolDbContext _discountTableContext;
        public DiscountTableRepository(SchoolDbContext discountTableContext)
        {
            _discountTableContext = discountTableContext;
        }

        IEnumerable<DiscountTable> IDiscountTableRepository.GetDiscountTableAll()
        {

            return _discountTableContext.DiscountTable.OrderBy(c => c.SortOrder).ToList();
        }
        IEnumerable<DiscountTable> IDiscountTableRepository.GetDiscountTablePeriod(string period, int schoolID)
        {

            return _discountTableContext.DiscountTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID).ToList();
        }
        IEnumerable<DiscountTable> IDiscountTableRepository.GetDiscountTablePeriodOnlyTrue(string period, int schoolID)
        {

            return _discountTableContext.DiscountTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period && c.SchoolID == schoolID && c.IsSelect == true ).ToList();
        }
        IEnumerable<DiscountTable> IDiscountTableRepository.GetDiscountTablePeriod2(string period)
        {

            return _discountTableContext.DiscountTable.OrderBy(c => c.SortOrder).Where(c => c.Period == period).ToList();
        }
        IEnumerable<DiscountTable> IDiscountTableRepository.GetDiscountTableIDPeriod(int? schoolID, string period)
        {

            return _discountTableContext.DiscountTable.OrderBy(c => c.SortOrder).Where(c => c.SchoolID == schoolID && c.Period == period && c.IsActive == true).ToList();
        }

        public DiscountTable GetDiscountTablePeriod3(int ID, string period, int? schoolID)
        {

            return _discountTableContext.DiscountTable.OrderBy(c => c.SortOrder).Where(c => c.DiscountTableID == ID && c.Period == period && c.SchoolID == schoolID).FirstOrDefault();
        }

        public DiscountTable GetDiscountTable(int discountTableID)
        {
            return _discountTableContext.DiscountTable.Where(b => b.DiscountTableID == discountTableID).FirstOrDefault();
        }
        public bool DeleteDiscountTable(DiscountTable discountTable)
        {
            _discountTableContext.Remove(discountTable);
            return Save();
        }
        public bool CreateDiscountTable(DiscountTable discountTable)
        {
            _discountTableContext.Add(discountTable);
            return Save();
        }
        public bool Save()
        {
            var saved = _discountTableContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateDiscountTable(DiscountTable discountTable)
        {
            _discountTableContext.Update(discountTable);
            return Save();
        }


    }
}
