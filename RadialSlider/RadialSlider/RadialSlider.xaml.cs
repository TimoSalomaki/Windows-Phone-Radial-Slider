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
		
		public RadialSlider()
		{
			InitializeComponent();

			SetInputScope();

			zeroAnglePoint = new Point(this.Width / 2, 0);
			centerPoint = new Point(this.Width / 2, this.Height / 2);
			
			this.MouseMove +=new System.Windows.Input.MouseEventHandler(Knob_MouseMove);
		}

		private void Knob_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			CalculateDraggingPosition(e.GetPosition(this));
		}
		
		private void CalculateDraggingPosition(Point currentPosition) 
		{
			double rotation = Math.Atan2(currentPosition.X - this.RenderSize.Width/2, currentPosition.Y - this.RenderSize.Height/2);
				
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
			if (newValue >= minimumValue && newValue <= maximumValue)
			{
				double oldValue = currentValue;

				if (isDegrees)
				{
					TrackBar.EndAngle = newValue; // Visually update the slider

					// Calculate the slider value according to minimum and maximum values
					currentValue = (int)(minimumValue + (maximumValue - minimumValue) * newValue / 360);
				}

				else
				{
					currentValue = Convert.ToInt32(newValue);
					TrackBar.EndAngle = newValue / (maximumValue - minimumValue) * 360;
				}

				// Optionally show the calculated slider value on the control
				if (ShowSliderValue)
					SliderValueTextBox.Text = currentValue.ToString();

				// If the SliderValueChanged event is already initialized...
				if (SliderValueChanged != null)
				{
					//... raise a SliderValueChanged event with the corresponding data
					SliderValueChangedEventArgs newData = new SliderValueChangedEventArgs(oldValue, newValue);
					SliderValueChanged(this, newData);
				}
			}

			else
			{
				if (overflowValueToMinimum)
					SetSliderValue(minimumValue, false);
				else
					SetSliderValue(maximumValue, false);
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
		private void Knob_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			zeroAnglePoint = new Point(this.Width / 2, 0);
			centerPoint = new Point(this.Width / 2, this.Height / 2);
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