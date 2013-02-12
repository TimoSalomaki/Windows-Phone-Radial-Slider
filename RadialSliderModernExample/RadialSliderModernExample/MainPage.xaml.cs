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

namespace RadialSliderModernExample
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		private void sliderValueChanged(object sender, SubsonicDesign.SliderValueChangedEventArgs e)
		{
			if (radialSliderModernRed != null)
			{
				byte red = Convert.ToByte(radialSliderModernRed.CurrentValue);
				byte green = Convert.ToByte(radialSliderModernGreen.CurrentValue);
				byte blue = Convert.ToByte(radialSliderModernBlue.CurrentValue);
				byte alpha = Convert.ToByte(radialSliderModernAlpha.CurrentValue);

				Dispatcher.BeginInvoke(() =>
				{
					LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));
				});
			}
		}
	}
}