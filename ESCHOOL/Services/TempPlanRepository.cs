using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class TempPlanRepository : ITempPlanRepository
    {
        private SchoolDbContext _tempPlanContext;
        public TempPlanRepository(SchoolDbContext tempPlanContext)
        {
            _tempPlanContext = tempPlanContext;
        }

        public IEnumerable<TempPlan> GetTempPlan()
        {
            return _tempPlanContext.TempPlan.ToList();
        }
        public IEnumerable<TempPlan> GetTempPlanSelect()
        {
            return _tempPlanContext.TempPlan.Where(b => b.IsActive == true).ToList();
        }
        public TempPlan GetTempPlanID(int planId)
        {
            return _tempPlanContext.TempPlan.Where(b => b.PlanID == planId).SingleOrDefault();
        }

        public bool CreateTempPlan(TempPlan tempPlan)
        {
            _tempPlanContext.Add(tempPlan);
            return Save();
        }
        public bool UpdateTempPlan(TempPlan tempPlan)
        {
            _tempPlanContext.Update(tempPlan);
            return Save();
        }
        public bool DeleteTempPlan(TempPlan tempPlan)
        {
            _tempPlanContext.Remove(tempPlan);
            return Save();
        }

        public bool Save()
        {
            var saved = _tempPlanContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
