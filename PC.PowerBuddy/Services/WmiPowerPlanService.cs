using PC.PowerBuddy.Adapters;
using PC.PowerBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace PC.PowerBuddy.Services
{
	[Obsolete("No longer works with latest version of Windows 10.")]
	public class WmiPowerPlanService : IPowerPlanService
	{
		private ManagementScope scope;
		private SelectQuery query;

		public WmiPowerPlanService()
		{
			String connectionString = @"root\cimv2\power";
			this.scope = new ManagementScope(connectionString);
			this.query = new SelectQuery("SELECT * FROM Win32_PowerPlan");
		}

		public IEnumerable<IPowerPlan> GetPowerPlans()
		{
			IEnumerable<IPowerPlan> result;

			using (var searcher = new ManagementObjectSearcher(scope, query))
			{
				var queryResult = searcher.Get().Cast<ManagementObject>();
				result = queryResult.Select(item => new WmiPowerPlanAdapter(item).ToPowerPlan()).ToList();
			}

			return result;
		}
	}
}
