using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PC.PowerBuddy.Controls
{
	/// <summary>
	/// Interaction logic for EditableIcon.xaml
	/// </summary>
	public partial class EditableIcon : UserControl
	{
		public static readonly Color DefaultForeground = Colors.White;
		public static readonly Color DefaultBackground = Color.FromArgb(0xFF, 0x13, 0x70, 0x8E);

		public EditableIcon()
		{
			InitializeComponent();
		}

		public Color IconForeground
		{
			get
			{
				return (Color)GetValue(IconForegroundProperty);
			}
			set
			{
				SetValue(IconForegroundProperty, value);
			}
		}

		public static readonly DependencyProperty IconForegroundProperty =
			DependencyProperty.Register(
				nameof(EditableIcon.IconForeground), 
				typeof(Color), 
				typeof(EditableIcon), 
				new PropertyMetadata(EditableIcon.DefaultForeground));

		public Color IconBackground
		{
			get
			{
				return (Color)GetValue(IconBackgroundProperty);
			}
			set
			{
				SetValue(IconBackgroundProperty, value);
			}
		}

		public static readonly DependencyProperty IconBackgroundProperty =
			DependencyProperty.Register(
				nameof(EditableIcon.IconBackground),
				typeof(Color),
				typeof(EditableIcon), 
				new PropertyMetadata(EditableIcon.DefaultBackground));
	}
}
