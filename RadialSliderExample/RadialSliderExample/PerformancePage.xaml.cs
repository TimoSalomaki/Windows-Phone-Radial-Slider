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
using SubsonicDesign;

namespace RadialSliderExample
{
	public partial class PerformancePage : PhoneApplicationPage
	{
		public PerformancePage()
		{
			InitializeComponent();

			for (int i = 0; i < 40; i++)
			{
				RadialSlider slider = new RadialSlider();
				slider.Width = 90;
				slider.Height = 90;
				slider.ShowSliderValue = true;
				slider.MinimumValue = 0;
				slider.MaximumValue = 360;
				slider.CurrentValue = i*9;
				slider.AllowKeyboardInput = false;
				wrapPanel.Children.Add(slider);
			}
		}
	}
}