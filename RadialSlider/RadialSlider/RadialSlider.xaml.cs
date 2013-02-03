/* Radial Slider control for Windows Phone Developers.
 * 
 * Design and development:
 *  Timo Salomäki (Subsonic Design)
 *  http://subsonicdesign.net
 *  timo@subsonicdesign.net 
 *  
 * Feel free to contact me with feedback, suggestions or fixes.
 * I'm available for freelance work.
 * 
 * Licensed under GNU General Public License, version 2
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Linq;

namespace SubsonicDesign
{
	public partial class RadialSlider : UserControl
	{
		public double EndAngle {
			get {
				return TrackBar.EndAngle;
			}
		}
		
		public string Text {
			get {
				return SliderValueTextBox.Text;
			}
			
			set {
				SliderValueTextBox.Text = value;
			}
		}
		
		// Simply toggles the visibility of the text in center of the control
		[Category("Appearance")]
		public Visibility TextVisible {
			get {
				return SliderValueTextBox.Visibility;
			}
			
			set {
				SliderValueTextBox.Visibility = value;
			}
		}
		
		// The brush used to fill the slider track
		[Category("Appearance")]
		public Brush TrackBarBrush {
			get {
				return TrackBar.Fill;
			}
			
			set {
				TrackBar.Fill = value;
			}
		}
		
		// Brush for the text in the middle of the control area
		[Category("Appearance")]
		public Brush TextBrush {
			get {
				return SliderValueTextBox.Foreground;
			}
			
			set {
				SliderValueTextBox.Foreground = value;
			}
		}

		public int MinimumValue
		{
			get { return minimumValue; }
			set { minimumValue = value; }
		}

		public int MaximumValue
		{
			get { return maximumValue; }
			set { maximumValue = value; }
		}

		public int CurrentValue
		{
			get { return currentValue; }
			set { currentValue = value; SetSliderValue(value, false); }
		}

		/// <summary>
		/// In case someone inserts a value over the maximum by using the textbox input, set the CurrentValue
		/// to zero if this is true. Otherwise the CurrentValue is set to the maximum value
		/// </summary>
		public bool OverflowValueToZero
		{
			get { return overflowValueToMinimum; }
			set { overflowValueToMinimum = value; }
		}

		public bool ShowSliderValue { get; set; }

		private Point zeroAnglePoint;
		private Point centerPoint;
		private int minimumValue;
		private int maximumValue;
		private int currentValue;
		private bool overflowValueToMinimum;

		private double controlWidth;
		private double controlHeight;
		
		public RadialSlider()
		{
			InitializeComponent();

			controlWidth = this.RenderSize.Width;
			controlHeight = this.RenderSize.Height;

			SetInputScope();

			zeroAnglePoint = new Point(this.Width / 2, 0);
			centerPoint = new Point(this.Width / 2, this.Height / 2);
			
			//this.MouseMove +=new System.Windows.Input.MouseEventHandler(Knob_MouseMove);
		}
		
		private void CalculateDraggingPosition(object currentPosition) 
		{
			Point position = (Point)currentPosition;

			double rotation = Math.Atan2(position.X - controlWidth / 2, position.Y - controlHeight / 2);
				
			rotation = 180 - RadianToDegree(rotation);
			rotation = Normalise(rotation);
			SetSliderValue(rotation, true);
		}
		
		/// <summary>
		/// Defines the value of the slider with an input of degrees from 0 to 360 (full circle).
		/// </summary>
		/// <param name="newValue">A degree value between 0 and 360</param>
		/// <param name="isDegrees">If true, degrees will be converted to a slider value. If false, the value
		/// will be set as the slider value.</param>
		private void SetSliderValue(double newValue, bool isDegrees) {
				double oldValue = currentValue;

				if (isDegrees)
				{
					// Calculate the slider value according to minimum and maximum values
					currentValue = (int)(minimumValue + (maximumValue - minimumValue) * newValue / 360);

					// Update the UI thread
					Dispatcher.BeginInvoke(() => {
						TrackBar.EndAngle = newValue; // Visually update the slider
					});
				}

				else if (newValue >= minimumValue && newValue <= maximumValue)
				{
					currentValue = Convert.ToInt32(newValue);

					double calculatedAngle = newValue / (maximumValue - minimumValue) * 360;

					// Update the UI thread
					Dispatcher.BeginInvoke(() =>
					{
						TrackBar.EndAngle = calculatedAngle;
					});
				}

				else
				{
					if (overflowValueToMinimum)
						SetSliderValue(minimumValue, false);
					else
						SetSliderValue(maximumValue, false);

					return;
				}

				// Optionally show the calculated slider value on the control
				if (ShowSliderValue)
					// Update the UI thread
					Dispatcher.BeginInvoke(() => {
						SliderValueTextBox.Text = currentValue.ToString();
					});

				// If the SliderValueChanged event is already initialized...
				if (SliderValueChanged != null)
				{
					//... raise a SliderValueChanged event with the corresponding data
					SliderValueChangedEventArgs newData = new SliderValueChangedEventArgs(oldValue, newValue);
					SliderValueChanged(this, newData);
			}
		}

		#region Helper methods
		static double RadianToDegree(double angle) {
			return angle * (180.0 / Math.PI);
		}
		
		public double Normalise (double degrees) {
			double retval = degrees % 360;
			if (retval < 0)
				retval += 360;
			return retval;
		}

		private void SetInputScope()
		{
			InputScopeNameValue digitsInputNameValue = InputScopeNameValue.Digits;

			SliderValueTextBox.InputScope = new InputScope()
			{
				Names = { new InputScopeName() { NameValue = digitsInputNameValue } }
			};
		}
		#endregion

		#region Events

		//private void Knob_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		//{
		//    Thread calcThread = new Thread(new ParameterizedThreadStart(CalculateDraggingPosition));
		//    calcThread.Start(e.GetPosition(this));
		//}

		void Touch_FrameReported(object sender, TouchFrameEventArgs e)
		{
			TouchPoint touchPoint = e.GetTouchPoints(this)[0];

			/* 
			 * Make sure that the touch point is inside the control, otherwise touch events from any
			 * point inside the app are processed. If we don't add this check and there are multiple
			 * radial sliders on the form, they will all react to all touch events on the parent page.
			 */

			if (Enumerable.Range(0, (int)controlWidth).Contains((int)touchPoint.Position.X) &&
				Enumerable.Range(0, (int)controlHeight).Contains((int)touchPoint.Position.Y))
			{
				Thread calcThread = new Thread(new ParameterizedThreadStart(CalculateDraggingPosition));
				calcThread.Start(touchPoint.Position);
			}
		}

		private void Knob_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			zeroAnglePoint = new Point(this.Width / 2, 0);
			centerPoint = new Point(this.Width / 2, this.Height / 2);

			controlWidth = this.RenderSize.Width;
			controlHeight = this.RenderSize.Height;
		}

		private void SliderValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string sliderTextValue = SliderValueTextBox.Text;

			if (!string.IsNullOrEmpty(sliderTextValue))
			{
				SetSliderValue(Convert.ToInt32(SliderValueTextBox.Text), false);
			}
		}

		private void SliderValueTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			// All the keys but dot is accepted from the digits keyboard
			if (e.Key == Key.Unknown)
			{
				e.Handled = true;
			}
		}

		public delegate void OnSliderValueChanged(object sender, SliderValueChangedEventArgs e);
		public event OnSliderValueChanged SliderValueChanged;
		#endregion

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			System.Windows.Input.Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
		}
	}


	public class SliderValueChangedEventArgs : EventArgs
	{
		private Double oldValue;
		private Double newValue;

		public SliderValueChangedEventArgs(Double oldValue, Double newValue)
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		public Double OldValue
		{
			get { return this.oldValue; }
		}
		public Double NewValue
		{
			get { return this.newValue; }
		}
	}
}