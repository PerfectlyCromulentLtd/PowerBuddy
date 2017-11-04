using PC.PowerBuddy.Adapters;
using PC.PowerBuddy.Models;
using PowerManagerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace PC.PowerBuddy.Services
{
	public class PowerPlanService : IPowerPlanService
	{
		public PowerPlanService()
		{
		}

		public IEnumerable<IPowerPlan> GetPowerPlans()
		{
			return PowerManager.ListPlans().Select(planId => new PowerPlan(planId));
		}
	}
}
