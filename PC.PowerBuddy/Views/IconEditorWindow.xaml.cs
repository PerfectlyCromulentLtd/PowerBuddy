using PC.PowerBuddy.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace PC.PowerBuddy.Views
{
	public partial class IconEditorWindow : Window
	{
		private readonly IconEditorViewModel viewModel;
		private readonly ReadOnlyDictionary<int, Visual> iconVisualsBySize;

		public IconEditorWindow()
		{
			InitializeComponent();
			this.iconVisualsBySize = new ReadOnlyDictionary<int, Visual>(
				new Dictionary<int, Visual>()
				{
					{ 256, this.icon256 },
					{ 192, this.icon192 },
					{ 128, this.icon128 },
					{ 96, this.icon96 },
					{ 64, this.icon64 },
					{ 48, this.icon48 },
					{ 32, this.icon32 },
					{ 24, this.icon24 },
					{ 16, this.icon16 }
				});
		}

		public IconEditorWindow(IconEditorViewModel iconEditorViewModel) : this()
		{
			this.viewModel = iconEditorViewModel;
			this.viewModel.IconVisualsBySize = this.iconVisualsBySize;

			this.DataContext = this.viewModel;
		}
	}
}
