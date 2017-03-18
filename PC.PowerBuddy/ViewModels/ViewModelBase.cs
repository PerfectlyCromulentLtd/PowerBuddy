using Prism.Mvvm;
using System.ComponentModel;
using System.Windows;

namespace PC.PowerBuddy.ViewModels
{
	public abstract class ViewModelBase : BindableBase
	{
		public bool IsInDesigner
		{
			get
			{
				return (bool)
					DesignerProperties
						.IsInDesignModeProperty
						.GetMetadata(typeof(DependencyObject)).DefaultValue;
			}
		}
	}
}
