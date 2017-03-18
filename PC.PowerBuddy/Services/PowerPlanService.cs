using PC.PowerBuddy.Adapters;
using PC.PowerBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace PC.PowerBuddy.Services
{
	public class PowerPlanService
	{
		private ManagementScope scope;
		private SelectQuery query;

		public PowerPlanService()
		{
			String connectionString = @"root\cimv2\power";
			this.scope = new ManagementScope(connectionString);
			this.query = new SelectQuery("SELECT * FROM Win32_PowerPlan");
		}

		public IEnumerable<PowerPlan> GetPowerPlans()
		{
			IEnumerable<PowerPlan> result;

			using (var searcher = new ManagementObjectSearcher(scope, query))
			{
				var queryResult = searcher.Get().Cast<ManagementObject>();
				result = queryResult.Select(item => new WmiPowerPlanAdapter(item).ToPowerPlan()).ToList();
			}

			return result;
		}
	}
}
