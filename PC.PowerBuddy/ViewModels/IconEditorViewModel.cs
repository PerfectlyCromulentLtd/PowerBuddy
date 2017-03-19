using BluwolfIcons;
using Newtonsoft.Json;
using PC.PowerBuddy.Services;
using Prism.Commands;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PC.PowerBuddy.ViewModels
{
	public class IconEditorViewModel : ViewModelBase
	{
		private readonly NotifyIconService notifyIconService;

		private PowerPlanIconViewModel selectedPowerPlan;
		private IEnumerable<PowerPlanIconViewModel> previousState;

		public IconEditorViewModel(NotifyIconService notifyIconService, PowerPlanService powerPlanService)
		{
			this.notifyIconService = notifyIconService;

			this.ApplyCommand = new DelegateCommand(this.Apply);
			this.LoadDefaultsCommand = new DelegateCommand(this.RevertToDefault);
			this.RevertChangesCommand = new DelegateCommand(this.RevertChanges);

			this.CreatePowerPlanViewModels(powerPlanService);
			this.LoadPreviousState();
			this.RevertChanges();
		}

		private void CreatePowerPlanViewModels(PowerPlanService powerPlanService)
		{
			var powerPlans = powerPlanService.GetPowerPlans();

			this.PowerPlans =
				powerPlans
					.Select(item => new PowerPlanIconViewModel(item.Id, item.Name))
					.ToList();

			var selectedPowerPlanId = powerPlans.SingleOrDefault(item => item.IsActive)?.Id;

			if (selectedPowerPlanId.HasValue)
			{
				this.SelectedPowerPlan = this.PowerPlans.Single(item => item.Id == selectedPowerPlanId);
			}
		}

		public IEnumerable<PowerPlanIconViewModel> PowerPlans
		{
			get;
			private set;
		}

		public PowerPlanIconViewModel SelectedPowerPlan
		{
			get
			{
				return this.selectedPowerPlan;
			}
			set
			{
				this.SetProperty(ref this.selectedPowerPlan, value);
				this.ApplyCommand.RaiseCanExecuteChanged();
			}
		}

		public IDictionary<int, Visual> IconVisualsBySize
		{
			get;
			internal set;
		}

		public DelegateCommand ApplyCommand
		{
			get;
			private set;
		}

		private void Apply()
		{
			var powerPlan = this.SelectedPowerPlan;

			if (powerPlan != null)
			{
				var icon = new Icon();

				foreach (var pair in this.IconVisualsBySize)
				{
					var visual = pair.Value;
					var size = pair.Key;

					RenderTargetBitmap bitmapSource = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Default);
					bitmapSource.Render(visual);

					icon.Images.Add(new PngIconImage(bitmapSource));
				}

				var newState = this.previousState.ToList();
				newState.RemoveAll(item => item.Id == powerPlan.Id);
				newState.Add(powerPlan);

				this.SaveState(newState);
				this.LoadPreviousState();

				using (var stream = new MemoryStream())
				{
					icon.Save(stream);
					this.notifyIconService.StoreNewPowerPlanIcon(powerPlan.Id, stream.GetBuffer());
				}
			}
		}

		public DelegateCommand LoadDefaultsCommand
		{
			get;
			private set;
		}

		private void RevertToDefault()
		{
			this.SelectedPowerPlan.RevertToDefault();
		}

		public DelegateCommand RevertChangesCommand
		{
			get;
			private set;
		}

		private void RevertChanges()
		{
			foreach (var vm in this.PowerPlans)
			{
				var previous = this.previousState.SingleOrDefault(item => item.Id == vm.Id);
				if (previous != null)
				{
					vm.Foreground = previous.Foreground;
					vm.Background = previous.Background;
				}
				else
				{
					vm.RevertToDefault();
				}
			}
		}

		private void LoadPreviousState()
		{
			var json = Properties.Settings.Default.PowerPlanColorsJson;
			this.previousState = JsonConvert.DeserializeObject<IEnumerable<PowerPlanIconViewModel>>(json) ?? Enumerable.Empty<PowerPlanIconViewModel>();
		}

		private void SaveState(IEnumerable<PowerPlanIconViewModel> state)
		{
			Properties.Settings.Default.PowerPlanColorsJson = JsonConvert.SerializeObject(state);
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();
		}
	}
}
