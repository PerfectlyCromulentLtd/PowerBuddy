using PC.PowerBuddy.Models;
using PC.PowerBuddy.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PC.PowerBuddy.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly PowerPlanService powerPlanService;
		private readonly NotifyIconService notifyIconService;

		private ObservableCollection<PowerPlanViewModel> powerPlans;

		public MainViewModel()
		{
			if (base.IsInDesigner)
			{
				this.powerPlanService = new PowerPlanService();
				this.UpdatePowerPlans();
			}
			else
			{
				throw new InvalidOperationException("The parameterless constructor should only be used by the WPF designer.");
			}
		}

		public MainViewModel(PowerPlanService powerPlanService, NotifyIconService notifyIconService)
		{
			this.powerPlanService = powerPlanService;
			this.notifyIconService = notifyIconService;
		}

		public ObservableCollection<PowerPlanViewModel> PowerPlans
		{
			get
			{
				return this.powerPlans;
			}
			set
			{
				this.SetProperty(ref this.powerPlans, value);
			}
		}

		internal void UpdatePowerPlans()
		{
			this.PowerPlans = new ObservableCollection<PowerPlanViewModel>(this.powerPlanService.GetPowerPlans().Select(item => new PowerPlanViewModel(item, this.notifyIconService)));
		}
	}
}
