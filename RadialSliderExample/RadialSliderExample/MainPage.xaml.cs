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
using System.Collections.ObjectModel;

namespace RadialSliderExample
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
		{
			ObservableCollection<MainListBoxItem> mainListBoxItems = new ObservableCollection<MainListBoxItem> {
				new MainListBoxItem { Text = "Color example", 
					Description = "RGB color editing using three RadialSliders" }, 
				new MainListBoxItem { Text = "Big slider", 
					Description = "The look and feel of a really big slider" },
				new MainListBoxItem { Text = "Performance test", 
					Description = "See how the app performas with dozens of sliders on the same page" },
				new MainListBoxItem { Text = "Visual States", 
					Description = "Control behaviours and graphics on various different states" },
				new MainListBoxItem { Text = "Elliptic Shapes", 
					Description = "Non-circlular shapes and how they work for RadialSlider" }
			};

			MainListBox.ItemsSource = mainListBoxItems;
		}

		private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Quick and dirty navigation to the proper page
			try
			{
				switch (MainListBox.SelectedIndex)
				{
					case 0:
						NavigationService.Navigate(new Uri("/ColorExample.xaml", UriKind.Relative));
						break;

					case 1:
						NavigationService.Navigate(new Uri("/BigSlider.xaml", UriKind.Relative));
						break;

					case 2:
						NavigationService.Navigate(new Uri("/PerformancePage.xaml", UriKind.Relative));
						break;

					case 3:
						NavigationService.Navigate(new Uri("/VisualStates.xaml", UriKind.Relative));
						break;

					case 4:
						NavigationService.Navigate(new Uri("/EllipticShapes.xaml", UriKind.Relative));
						break;
				}
			}

			catch
			{
			}
		}
	}

	public class MainListBoxItem
	{
		public string Text { get; set; }
		public string Description { get; set; }
	}
}