﻿using PC.PowerBuddy.Models;
using PC.PowerBuddy.Services;
using System;

namespace PC.PowerBuddy.ViewModels
{
	public class PowerPlanViewModel : ViewModelBase
	{
		private readonly PowerPlan model;
		private readonly NotifyIconService notifyIconService;

		public PowerPlanViewModel(PowerPlan model, NotifyIconService notifyIconService)
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
				else
				{
					this.model.Deactivate();
				}

				this.OnPropertyChanged();
			}
		}

		private void UpdateIcon()
		{
			this.notifyIconService.UpdateDisplayedIcon(this.model.Id, this.Name);
		}
	}
}
