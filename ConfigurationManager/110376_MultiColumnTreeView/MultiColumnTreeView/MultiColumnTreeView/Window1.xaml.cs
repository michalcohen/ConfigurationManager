using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using System.Globalization;

namespace MultiColumnTreeView
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();
		}
	}


	public class RadTreeListView : RadTreeView
	{
		public System.Windows.Controls.GridViewColumnCollection Columns
		{
			get
			{
				if (columns == null)
				{
					columns = new System.Windows.Controls.GridViewColumnCollection();
				}

				return columns;
			}
		}

		private System.Windows.Controls.GridViewColumnCollection columns;

	}

	public class LevelToIndentConverter : IValueConverter
	{
		public object Convert(object value, Type type, object parameter, CultureInfo culture)
		{
			return new Thickness((int)value * indentSize, 0, 0, 0);
		}

		public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		private const double indentSize = 20;
	}
}
