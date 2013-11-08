using PC.PowerBuddy.Adapters;
using PC.PowerBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace PC.PowerBuddy.Models
{
	public class PowerPlanQuerier
	{
		private ManagementScope scope;
		private SelectQuery query;

		public PowerPlanQuerier()
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
