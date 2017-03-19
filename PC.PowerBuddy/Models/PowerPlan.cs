using System;
using System.Management;

namespace PC.PowerBuddy.Models
{
	public sealed class PowerPlan : IDisposable
	{
		private readonly ManagementObject wmiPowerPlan;

		public PowerPlan(string name, string description, string instanceId, Guid id, bool isActive, ManagementObject wmiPowerPlan)
		{
			this.Name = name;
			this.Description = description;
			this.InstanceId = instanceId;
			this.Id = id;
			this.IsActive = isActive;
			this.wmiPowerPlan = wmiPowerPlan;
		}

		internal void Activate()
		{
			this.wmiPowerPlan.InvokeMethod("Activate", null);
			this.IsActive = true;
		}

		internal void Deactivate()
		{
			this.IsActive = false;
		}

		public void Dispose()
		{
			this.wmiPowerPlan.Dispose();
		}

		public Guid Id
		{
			get;
		}

		public String Name
		{
			get;
		}

		public String Description
		{
			get;
		}

		public string InstanceId
		{
			get;
		}

		public bool IsActive
		{
			get;
			private set;
		}
	}
}
