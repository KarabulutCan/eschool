using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class SchoolBusServicesRepository : ISchoolBusServicesRepository
    {
        private SchoolDbContext _schoolBusServicesContext;
        public SchoolBusServicesRepository(SchoolDbContext schoolBusServicesContext)
        {
            _schoolBusServicesContext = schoolBusServicesContext;
        }

        IEnumerable<SchoolBusServices> ISchoolBusServicesRepository.GetSchoolBusServicesAll(int schoolID, string period)
        {
            return _schoolBusServicesContext.SchoolBusServices.Where(b => b.SchoolID == schoolID && b.Period == period).OrderBy(c => c.SortOrder).ToList();
        }

        public SchoolBusServices GetSchoolBusServices(int schoolBusServicesID)
        {
            return _schoolBusServicesContext.SchoolBusServices.Where(b => b.SchoolBusServicesID == schoolBusServicesID).FirstOrDefault();
        }
        public SchoolBusServices GetSchoolBusRoute(string schoolBusServicesRoute)
        {
            return _schoolBusServicesContext.SchoolBusServices.Where(b => b.BusRoute == schoolBusServicesRoute).FirstOrDefault();
        }
        public SchoolBusServices GetSchoolBusDrive(string schoolBusServicesDriver)
        {
            return _schoolBusServicesContext.SchoolBusServices.Where(b => b.DriverName == schoolBusServicesDriver).FirstOrDefault();
        }
        public bool CreateSchoolBusServices(SchoolBusServices schoolBusServices)
        {
            _schoolBusServicesContext.Add(schoolBusServices);
            return Save();
        }

        public bool UpdateSchoolBusServices(SchoolBusServices schoolBusServices)
        {
            _schoolBusServicesContext.Update(schoolBusServices);
            return Save();
        }

        public bool DeleteSchoolBusServices(SchoolBusServices schoolBusServices)
        {
            _schoolBusServicesContext.Remove(schoolBusServices);
            return Save();
        }

        public bool ExistSchoolBusServices(int schoolBusServicesID)
        {
            return _schoolBusServicesContext.SchoolBusServices.Any(c => c.SchoolBusServicesID == schoolBusServicesID);
        }

        public bool ExistSchoolBusServicesRoute(string busRoute)
        {
            return _schoolBusServicesContext.SchoolBusServices.Any(c => c.BusRoute == busRoute);
        }
        public bool ExistSchoolBusServicesDriver(string busDriver)
        {
            return _schoolBusServicesContext.SchoolBusServices.Any(c => c.DriverName == busDriver);
        }
        public bool Save()
        {
            var saved = _schoolBusServicesContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
