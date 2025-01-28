using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IMultipurposeListRepository
    {
        IEnumerable<MultipurposeList> GetMultipurposeListAll();
        IEnumerable<MultipurposeList> GetMultipurposeListTrue();
        MultipurposeList GetMultipurposeListName(string Name);
        MultipurposeList GetMultipurposeListID(int ID);
        bool CreateMultipurposeList(MultipurposeList multipurposeList);
        bool UpdateMultipurposeList(MultipurposeList multipurposeList);
        bool DeleteMultipurposeList(MultipurposeList multipurposeList);
        bool Save();
    }
}
