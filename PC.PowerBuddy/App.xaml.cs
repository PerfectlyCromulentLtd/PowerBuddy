using PC.PowerBuddy.Services;
using PC.PowerBuddy.ViewModels;
using PC.PowerBuddy.Views;
using System.Windows;

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

        private void Application_Startup(object sender, StartupEventArgs startupEventArgs)
        {
			var notifyIconService = new NotifyIconService();
			var powerPlanService = new PowerPlanService();

			this.MainWindow = new MainWindow(new MainViewModel(powerPlanService, notifyIconService), notifyIconService);
			this.MainWindow.Show();

			IconEditorWindow editor = null;

			notifyIconService.IconEditorLaunchRequested += (s, e) =>
			{
				if (editor == null || !editor.IsVisible)
				{
					editor = new IconEditorWindow(new IconEditorViewModel(notifyIconService, powerPlanService));
					editor.Owner = this.MainWindow;
					editor.Show();
				}
				else
				{
					editor.Activate();
				}
			};
		}
	}
}
