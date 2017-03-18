using System;
using System.Management;

namespace PC.PowerBuddy.Models
{
	public class PowerPlan
	{
		public Guid Id
		{
			get;
			set;
		}

		public String Name
		{
			get;
			set;
		}

		public String Description
		{
			get;
			set;
		}

		public string InstanceId
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public ManagementObject WmiPowerPlan
		{
			get;
			set;
		}
	}
}
