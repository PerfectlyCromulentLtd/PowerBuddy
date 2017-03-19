using PC.PowerBuddy.Models;
using System;
using System.Management;

namespace PC.PowerBuddy.Adapters
{
	public class WmiPowerPlanAdapter
	{
		private ManagementObject wmiPowerPlan;

		public WmiPowerPlanAdapter(ManagementObject wmiPowerPlan)
		{
			String pathClassName = wmiPowerPlan.ClassPath.ClassName;
			if (wmiPowerPlan.ClassPath.ClassName != "Win32_PowerPlan")
			{
				throw new ArgumentException("Unexpected ManagementObject");
			}

			this.wmiPowerPlan = wmiPowerPlan;
		}

		public PowerPlan ToPowerPlan()
		{
			var name = this.wmiPowerPlan.GetPropertyValue("ElementName").ToString();
			var description = this.wmiPowerPlan.GetPropertyValue("Description").ToString();
			
			var instanceId = this.wmiPowerPlan.GetPropertyValue("InstanceID").ToString();

			
			Guid id = Guid.Empty;
			const int guidStringLength = 38;
			if (instanceId.Length >= guidStringLength)
			{
				Guid.TryParse(instanceId.Substring(instanceId.Length - guidStringLength, guidStringLength), out id);
			}

			bool isActive;
			Boolean.TryParse(this.wmiPowerPlan.GetPropertyValue("IsActive").ToString(), out isActive);

			var result = new PowerPlan(name, description, instanceId, id, isActive, this.wmiPowerPlan);
			return result;
		}
	}
}