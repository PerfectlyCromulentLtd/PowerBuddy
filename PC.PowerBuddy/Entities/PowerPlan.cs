using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC.PowerBuddy.Entities
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

		public System.Management.ManagementObject WmiPowerPlan
		{
			get;
			set;
		}
	}
}
