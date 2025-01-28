using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ITempPlanRepository
    {
        IEnumerable<TempPlan> GetTempPlan();
        IEnumerable<TempPlan> GetTempPlanSelect();
        TempPlan GetTempPlanID(int planId);
        bool CreateTempPlan(TempPlan tempPlan);
        bool UpdateTempPlan(TempPlan tempPlan);
        bool DeleteTempPlan(TempPlan tempPlan);
        bool Save();
    }
}
