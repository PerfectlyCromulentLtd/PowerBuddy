using PC.PowerBuddy.Controls;
using PC.PowerBuddy.Models;
using PC.PowerBuddy.Services;
using System;
using System.Windows.Media;

namespace PC.PowerBuddy.ViewModels
{
	public class PowerPlanIconViewModel : ViewModelBase
	{
		private Color foreground;
		private Color background;

		public PowerPlanIconViewModel(Guid id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public Guid Id
		{
			get;
		}

		public string Name
		{
			get;
		}

		public Color Foreground
		{
			get
			{
				return this.foreground;
			}
			set
			{
				this.SetProperty(ref this.foreground, value);
			}
		}

		public Color Background
		{
			get
			{
				return this.background;
			}
			set
			{
				this.SetProperty(ref this.background, value);
			}
		}

		internal void RevertToDefault()
		{
			this.Foreground = EditableIcon.DefaultForeground;
			this.Background = EditableIcon.DefaultBackground;
		}
	}
}
