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
		#region Properties

		#region Commonly used values

		public string Text
		{
			get
			{
				return sliderTextBoxText;
			}

			set
			{
				sliderTextBoxText = value;

				if (!showSliderValue)
				{
					SliderValueTextBox.Text = sliderTextBoxText;
				}
			}
		}

		public double EndAngle 
		{
			get 
			{ 
				return Slider.EndAngle;
			}

			private set
			{
				Slider.EndAngle = value;
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

		#endregion

		#region Brushes

		// The brush used to fill the slider track
		[Category("Appearance")]
		public Brush SliderBrush {
			get {
				return Slider.Fill;
			}
			
			set {
				Slider.Fill = value;
			}
		}

		// Brush for the text in the middle of the control area
		[Category("Appearance")]
		public Brush TextBrush
		{
			get
			{
				return SliderValueTextBox.Foreground;
			}

			set
			{
				SliderValueTextBox.Foreground = value;
			}
		}

		[Category("Appearance")]
		public Brush OuterEllipseBrush
		{
			get
			{
				return OuterEllipse_Dark.Fill;
			}

			set
			{
				OuterEllipse_Dark.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush SecondOuterEllipseBrush
		{
			get
			{
				return OuterEllipse_Lighter.Fill;
			}

			set
			{
				OuterEllipse_Lighter.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush SecondOuterEllipseTopBrush
		{
			get
			{
				return OuterEllipse_DarkGradient.Fill;
			}

			set
			{
				OuterEllipse_DarkGradient.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush ShineBrush
		{
			get
			{
				return Shine.Fill;
			}

			set
			{
				Shine.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush SliderBorderShadowInner
		{
			get
			{
				return BorderShadow_Slider.Fill;
			}

			set
			{
				BorderShadow_Slider.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush SliderTrackBrush
		{
			get
			{
				return SliderTrack_Black.Fill;
			}

			set
			{
				SliderTrack_Black.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush InnerBorderBrush
		{
			get
			{
				return InnerBorder.Fill;
			}

			set
			{
				InnerBorder.Fill = value;
			}
		}

		[Category("Appearance")]
		public Brush InnerEllipseBrush
		{
			get
			{
				return InnerEllipse_Dark.Fill;
			}

			set
			{
				InnerEllipse_Dark.Fill = value;
			}
		}

		#endregion

		#region Opacities and visibilities

		[Category("Appearance")]
		public double SliderOpacity {
			get {
				return Slider.Opacity;
			}
			
			set {
				Slider.Opacity = value;
			}
		}

		[Category("Appearance")]
		public double ShineOpacity
		{
			get
			{
				return Shine.Opacity;
			}

			set
			{
				Shine.Opacity = value;
			}
		}		
		
		[Category("Appearance")]
		public double SliderTrackOpacity {
			get {
				return SliderTrack_Black.Opacity;
			}
			
			set {
				SliderTrack_Black.Opacity = value;
			}
		}		
		
		[Category("Appearance")]
		public double InnerEllipseOpacity {
			get {
				return InnerEllipse_Dark.Opacity;
			}
			
			set {
				InnerEllipse_Dark.Opacity = value;
			}
		}

		// Simply toggles the visibility of the text in center of the control
		[Category("Appearance")]
		public Visibility TextVisible
		{
			get
			{
				return SliderValueTextBox.Visibility;
			}

			set
			{
				SliderValueTextBox.Visibility = value;
			}
		}

		#endregion

		#region Booleans

		/// <summary>
		/// In case someone inserts a value over the maximum by using the textbox input, set the CurrentValue
		/// to zero if this is true. Otherwise the CurrentValue is set to the maximum value
		/// </summary>
		public bool OverflowValueToZero
		{
			get { return overflowValueToMinimum; }
			set { overflowValueToMinimum = value; }
		}

		[DefaultValue(true)]
		public bool AllowKeyboardInput
		{
			get 
			{
				return allowKeyboardInput; 
			}

			set 
			{ 
				allowKeyboardInput = value;

				if (this.IsEnabled)
				{
					SliderValueTextBox.IsEnabled = value;
				}
			}
		}

		public bool ShowSliderValue
		{
			get
			{
				return showSliderValue;
			}

			set 
			{
				showSliderValue = value;

				if (showSliderValue == true)
					SetSliderValue(this.CurrentValue, false);
				else
					this.Text = sliderTextBoxText;
			}
		}

		#endregion

		#endregion

		#region Private fields
		private string sliderTextBoxText = "";
		private int minimumValue;
		private int maximumValue;
		private int currentValue;
		private double controlWidth;
		private double controlHeight;
		private bool showSliderValue;
		private bool overflowValueToMinimum;
		private bool allowKeyboardInput = true;
		private Point zeroAnglePoint;
		private Point centerPoint;
		#endregion

		#region Constructor

		public RadialSlider()
		{
			InitializeComponent();

			controlWidth = this.RenderSize.Width;
			controlHeight = this.RenderSize.Height;

			SetInputScope();

			zeroAnglePoint = new Point(this.Width / 2, 0);
			centerPoint = new Point(this.Width / 2, this.Height / 2);

			this.Unloaded += new RoutedEventHandler(RadialSlider_Unloaded);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Defines the value of the slider with an input of degrees from 0 to 360 (full circle).
		/// </summary>
		/// <param name="newValue">A degree value between 0 and 360</param>
		/// <param name="isDegrees">If true, degrees will be converted to a slider value. If false, the value
		/// will be set as the slider value.</param>
		private void SetSliderValue(double newValue, bool isDegrees)
		{
			double oldValue = currentValue;

			if(!isDegrees && (newValue < minimumValue || newValue > maximumValue)) {
				if (overflowValueToMinimum)
					SetSliderValue(minimumValue, false);
				else
					SetSliderValue(maximumValue, false);

				return;
			}

			if (isDegrees)
				// Calculate the slider value according to minimum and maximum values
				currentValue = (int)(minimumValue + (maximumValue - minimumValue) * newValue / 360);

			else
			{
				currentValue = Convert.ToInt32(newValue);
				newValue = newValue / (maximumValue - minimumValue) * 360;
			}

			// Update the UI thread
			Dispatcher.BeginInvoke(() =>
			{
				Slider.EndAngle = newValue; // Visually update the slider
			});
			

			// Optionally show the calculated slider value on the control
			if (ShowSliderValue)
				// Update the UI thread
				Dispatcher.BeginInvoke(() =>
				{
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

		private void CalculateDraggingPosition(object currentPosition)
		{
			Point position = (Point)currentPosition;

			double rotation = Math.Atan2(position.X - controlWidth / 2, position.Y - controlHeight / 2);

			rotation = 180 - RadianToDegree(rotation);
			rotation = Normalise(rotation);
			SetSliderValue(rotation, true);
		}

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

		private bool IsPointInsideEllipseShape(Ellipse ellipseToUse, Point point)
		{
			EllipseGeometry ellipse  = new EllipseGeometry();
			ellipse.RadiusX = ellipseToUse.RenderSize.Width / 2;
			ellipse.RadiusY = ellipseToUse.RenderSize.Height / 2;

			// Get absolute position of the ellipse inside parent control
			//Point absolutePosition = ellipse.Transform.Transform(new Point(0, 0));
			var transform = ellipseToUse.TransformToVisual(this);
			Point absolutePosition = transform.Transform(new Point(0, 0));

			ellipse.Center = new Point(absolutePosition.X + ellipse.RadiusX,
										absolutePosition.Y + ellipse.RadiusY);

			if (ellipse.Bounds.Contains(point))
			{
				double xPositionInsideBounds = point.X - absolutePosition.X;
				double yPositionInsideBounds = point.Y - absolutePosition.Y;

				if (PointInsideEllipse(xPositionInsideBounds, ellipse.RadiusX, yPositionInsideBounds, ellipse.RadiusY))
				{
					return true;
				}
			}

			return false;
		}

		private bool PointInsideEllipse(double x, double a, double y, double b)
		{
			double xDistance = x - a;
			xDistance = xDistance < 0 ? xDistance * -1 : xDistance;

			double yDistance = y - b;
			yDistance = yDistance < 0 ? yDistance * -1 : yDistance;


			double ellipseCalculation = (Math.Pow(xDistance, 2) / Math.Pow(a, 2)) +
										(Math.Pow(yDistance, 2) / Math.Pow(b, 2));

			if (ellipseCalculation < 1)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Events

		void Touch_FrameReported(object sender, TouchFrameEventArgs e)
		{
			TouchPoint touchPoint = e.GetTouchPoints(this)[0];

			// Slider value can only be modified when the control itself is enabled and the touch point
			// is inside the control (which is elliptic). Also check if the touch position is not in the middle
			// of the control where the textbox is.
			if (this.IsEnabled && PointInsideEllipse(touchPoint.Position.X, controlWidth / 2, touchPoint.Position.Y, controlHeight / 2)
				&& !IsPointInsideEllipseShape(InnerEllipse_Dark, touchPoint.Position))
			{
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
			if (showSliderValue)
			{
				string sliderTextValue = SliderValueTextBox.Text;

				if (!string.IsNullOrEmpty(sliderTextValue))
				{
					SetSliderValue(Convert.ToInt32(SliderValueTextBox.Text), false);
				}
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

		void RadialSlider_Unloaded(object sender, RoutedEventArgs e)
		{
			Touch.FrameReported -= new TouchFrameEventHandler(Touch_FrameReported);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
		}

		private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal", true);
				if (allowKeyboardInput)
				{
					SliderValueTextBox.IsEnabled = true;
				}
			}

			else
			{
				VisualStateManager.GoToState(this, "Disabled", true);
				SliderValueTextBox.IsEnabled = false;
			}
		}

		private void UserControl_GotFocus(object sender, System.Windows.RoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Focused", true);
		}

		private void UserControl_LostFocus(object sender, System.Windows.RoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Unfocused", true);
		}

		#region Custom events

		public delegate void OnSliderValueChanged(object sender, SliderValueChangedEventArgs e);
		public event OnSliderValueChanged SliderValueChanged;

		#endregion

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