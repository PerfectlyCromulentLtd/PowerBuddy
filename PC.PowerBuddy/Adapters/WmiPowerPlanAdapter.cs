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
			var result = new PowerPlan();

			result.Name = this.wmiPowerPlan.GetPropertyValue("ElementName").ToString();
			result.Description = this.wmiPowerPlan.GetPropertyValue("Description").ToString();
			
			String instanceId = this.wmiPowerPlan.GetPropertyValue("InstanceID").ToString();
			result.InstanceId = instanceId;

			int guidStringLength = 38;

			Guid guid = Guid.Empty;

			if (instanceId.Length >= guidStringLength && Guid.TryParse(instanceId.Substring(instanceId.Length - guidStringLength, guidStringLength), out guid))
			{
				result.Id = guid;
			}

			bool isActive;
			if(Boolean.TryParse(this.wmiPowerPlan.GetPropertyValue("IsActive").ToString(), out isActive))
			{
				result.IsActive = isActive;
			}

			result.WmiPowerPlan = this.wmiPowerPlan;

			return result;
		}
	}
}