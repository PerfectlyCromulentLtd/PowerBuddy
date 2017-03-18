using PC.PowerBuddy.Models;
using System;

namespace PC.PowerBuddy.ViewModels
{
	public class PowerPlanViewModel : ViewModelBase
	{
		private PowerPlan model;

		public PowerPlanViewModel(PowerPlan model)
		{
			this.model = model;
		}

		public String Name
		{
			get
			{
				return this.model.Name;
			}
		}

		public String Description
		{
			get
			{
				return this.model.Description;
			}
		}

		public bool IsActive
		{
			get
			{
				return this.model.IsActive;
			}
			set
			{
				this.model.IsActive = value;
				this.OnPropertyChanged();

				if (this.model.IsActive)
				{
					this.model.WmiPowerPlan.InvokeMethod("Activate", null);
				}
			}
		}
	}
}
