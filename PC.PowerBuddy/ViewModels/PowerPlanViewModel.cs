using PC.PowerBuddy.Models;
using PC.PowerBuddy.Services;
using System;

namespace PC.PowerBuddy.ViewModels
{
	public class PowerPlanViewModel : ViewModelBase
	{
		private readonly IPowerPlan model;
		private readonly NotifyIconService notifyIconService;

		public PowerPlanViewModel(IPowerPlan model, NotifyIconService notifyIconService)
		{
			this.model = model;
			this.notifyIconService = notifyIconService;

			if (this.IsActive)
			{
				this.UpdateIcon();
			}
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
				if (value)
				{
					this.model.Activate();
					this.UpdateIcon();
				}

				this.OnPropertyChanged();
			}
		}

		private void UpdateIcon()
		{
			this.notifyIconService.SetDisplayedIcon(this.model.Id, $"Current power plan:{Environment.NewLine}{this.Name}");
		}
	}
}
