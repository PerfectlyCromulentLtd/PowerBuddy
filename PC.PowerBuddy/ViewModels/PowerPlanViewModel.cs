using Microsoft.Practices.Prism.ViewModel;
using PC.PowerBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC.PowerBuddy.ViewModels
{
	public class PowerPlanViewModel : NotificationObject
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
				base.RaisePropertyChanged(() => this.IsActive);

				if (this.model.IsActive)
				{
					this.model.WmiPowerPlan.InvokeMethod("Activate", null);
				}
			}
		}
	}
}
