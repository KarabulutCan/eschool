using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IPSerialNumberRepository
    {
        IEnumerable<Models.PSerialNumber> GetPSerialNumbers();
        PSerialNumber GetPSerialNumber(int pSerialNumberID);

        bool CreatePSerialNumber(PSerialNumber pSerialNumber);
        bool UpdatePSerialNumber(PSerialNumber pSerialNumber);
        bool DeletePSerialNumber(PSerialNumber pSerialNumber);
        bool Save();
    }
}
