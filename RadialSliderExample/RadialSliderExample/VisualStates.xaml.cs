using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace RadialSliderExample
{
	public partial class VisualStates : PhoneApplicationPage
	{
		public VisualStates()
		{
			InitializeComponent();
		}

		private void toggle_Checked(object sender, RoutedEventArgs e)
		{
			isEnabledToggle.Content = "True";
			radialSlider1.IsEnabled = true;
		}

		private void toggle_Unchecked(object sender, RoutedEventArgs e)
		{
			isEnabledToggle.Content = "False";
			radialSlider1.IsEnabled = false;
		}
	}
}