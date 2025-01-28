using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ISchoolBusServicesRepository
    {
        IEnumerable<SchoolBusServices> GetSchoolBusServicesAll(int schoolID, string period);
        SchoolBusServices GetSchoolBusServices(int schoolBusServiceID);
        SchoolBusServices GetSchoolBusRoute(string schoolBusServicesRoute);
        SchoolBusServices GetSchoolBusDrive(string schoolBusServicesDrive);

        bool CreateSchoolBusServices(SchoolBusServices schoolBusServices);
        bool UpdateSchoolBusServices(SchoolBusServices schoolBusServices);
        bool DeleteSchoolBusServices(SchoolBusServices schoolBusServices);
        bool ExistSchoolBusServices(int schoolBusServicesID);
        bool ExistSchoolBusServicesRoute(string busRoute);
        bool ExistSchoolBusServicesDriver(string busDrive);
        bool Save();
    }
}
