using System.Collections.Generic;
using PC.PowerBuddy.Models;

namespace PC.PowerBuddy.Services
{
	public interface IPowerPlanService
	{
		IEnumerable<IPowerPlan> GetPowerPlans();
	}
}