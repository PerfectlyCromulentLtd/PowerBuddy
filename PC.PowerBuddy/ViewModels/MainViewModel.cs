using PC.PowerBuddy.Models;
using PC.PowerBuddy.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace PC.PowerBuddy.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private PowerPlanService powerPlanService;
		private ObservableCollection<PowerPlanViewModel> powerPlans;

		public MainViewModel(PowerPlanService powerPlanService)
		{
			this.powerPlanService = powerPlanService;

			if (base.IsInDesigner)
			{
				this.UpdatePowerPlans();
			}
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
			this.PowerPlans = new ObservableCollection<PowerPlanViewModel>(this.powerPlanService.GetPowerPlans().Select(item => new PowerPlanViewModel(item)));
		}
	}
}
