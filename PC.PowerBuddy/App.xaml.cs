using PC.PowerBuddy.Controls;
using PC.PowerBuddy.Interop;
using PC.PowerBuddy.Services;
using PC.PowerBuddy.ViewModels;
using PC.PowerBuddy.Views;
using System;
using System.Windows;
using System.Windows.Interop;

namespace PC.PowerBuddy
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        public App()
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
			var viewModel = new MainViewModel(new PowerPlanService());

			this.MainWindow = new MainWindow(viewModel);
			this.MainWindow.Loaded += (s, ea) =>
			{
				this.HideFromAltTab(this.MainWindow);
				viewModel.UpdatePowerPlans();
			};

			this.MainWindow.Show();
        }

		private void HideFromAltTab(Window window)
		{
			WindowInteropHelper wndHelper = new WindowInteropHelper(window);

			int exStyle = (int)Win32Interop.GetWindowLong(wndHelper.Handle, (int)Win32Interop.GetWindowLongFields.GWL_EXSTYLE);

			exStyle |= (int)Win32Interop.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
			Win32Interop.SetWindowLong(wndHelper.Handle, (int)Win32Interop.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
		}
	}
}
