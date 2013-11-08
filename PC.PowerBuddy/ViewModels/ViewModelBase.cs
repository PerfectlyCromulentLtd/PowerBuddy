using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace PC.PowerBuddy.ViewModels
{
	public abstract class ViewModelBase : NotificationObject
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
