using PowerManagerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC.PowerBuddy.Models
{
	class PowerPlan : IPowerPlan, IDisposable
	{
		public PowerPlan(Guid id)
		{
			this.Id = id;
		}

		public Guid Id
		{
			get;
		}

		public string Name =>
			PowerManager.GetPlanName(this.Id);

		public string Description =>
			PowerManager.GetPlanDescription(this.Id);

		public bool IsActive =>
			PowerManager.GetActivePlan() == this.Id;

		public void Activate() =>
			PowerManager.SetActivePlan(this.Id);

		#region IDisposable Support
		private bool isDisposed = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
					//nothing
				}

				isDisposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
