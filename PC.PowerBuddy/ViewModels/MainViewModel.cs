using Microsoft.Practices.Prism.ViewModel;
using PC.PowerBuddy.Entities;
using PC.PowerBuddy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC.PowerBuddy.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private PowerPlanQuerier querier;
		private ObservableCollection<PowerPlanViewModel> powerPlans;

		public Action<PowerPlanViewModel> CurrentPlanChanged;

		public MainViewModel()
		{
			this.querier = new PowerPlanQuerier();

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
				this.powerPlans = value;
				base.RaisePropertyChanged(() => this.PowerPlans);
			}
		}

		internal void UpdatePowerPlans()
		{
			this.PowerPlans = new ObservableCollection<PowerPlanViewModel>(this.querier.GetPowerPlans().Select(item =>
			{
				PowerPlanViewModel p = new PowerPlanViewModel(item);
				p.PropertyChanged += (sender, args) =>
				{
					PowerPlanViewModel typedSender = (PowerPlanViewModel) sender;
					if (args.PropertyName == "IsActive" && typedSender.IsActive)
					{
						this.CurrentPlanChanged(typedSender);
					}
				};
				return p;
			}));

			this.CurrentPlanChanged(PowerPlans.Single(x => x.IsActive));
		}
	}
}
