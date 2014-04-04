/*
 * Copyright (C) 2012-2014 Timo Salomäki
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
	public partial class ColorExample : PhoneApplicationPage
	{
		public ColorExample()
		{
			InitializeComponent();
		}

		private byte r;
		private byte g;
		private byte b;

		private void ColorSlider_SliderValueChanged(object sender, SubsonicDesign.SliderValueChangedEventArgs e)
		{
			UpdateColor();
		}

		private void UpdateColor()
		{
			try
			{
				r = Convert.ToByte(redSlider.CurrentValue);
				g = Convert.ToByte(greenSlider.CurrentValue);
				b = Convert.ToByte(blueSlider.CurrentValue);

				LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));
			}

			catch
			{
			}
		}
	}
}