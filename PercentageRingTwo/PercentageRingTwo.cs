using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Diagnostics;
using Windows.Devices.Radios;
using Windows.Foundation;

namespace StorageControls
{
    /// <summary>
    /// A control which takes a value and converts to a percentage displayed on a circular ring
    /// </summary>
    public partial class PercentageRingTwo : Control
    {
        #region Private variables

        private Canvas      _container;              // Reference to the container Canvas
        private Path        _mainPath;               // Reference to the Main ring's ArcSegment path
        private Path        _trackPath;              // Reference to the Track ring's ArcSegment path
        private TextBlock   _testText;               // Temporary reference to the TextBlock in the template

        private double      _oldValue;               // Stores the previous value

        private double      _minValue;               // Stores the Minimum value
        private double      _maxValue;               // Stores the Maximum value
        private double      _normalizedMinAngle;     // Stores the normalised Minimum Angle
        private double      _normalizedMaxAngle;     // Stores the normalise Maximum Angle

        private double      _controlWidth;           // Stores the Width value of the control
        private double      _controlHeight;          // Stores the Height value of the control
        private Point       _controlCenter;          // Stores the Centre Point of the control

        private double      _adjSize;                // controlWidth or controlHeight, whichever is smaller to preserve square
        private Point       _adjCentre;              // center Point of the adjusted size = (adjSize / 2 ) , (adjSize / 2 ); 

        private bool        _gapAngle;               // Stores the angle to be used to separate Main and Track rings

        private double      _mainRingThickness;      // Stores Main Ring Thickness
        private double      _trackRingThickness;     // Stores Track Ring Thickness

        private double      _mainRingRadius;         // Stores Main Ring Radius
        private double      _trackRingRadius;        // Stores Track Ring Radius

        #endregion



        #region Private variable setters

        /// <summary>
        /// Sets the adjusted size it to the smaller of _controlHeight and _controlWidth.
        /// </summary>
        private void SetAdjustedSize(double controlWidth, double controlHeight)
        {
            _adjSize = Math.Min(controlWidth, controlHeight);
        }



        /// <summary>
        /// Sets the adjusted size by setting it to the smaller of _controlHeight and _controlWidth.
        /// </summary>
        private void SetAdjustedCentre(double adjustedSize)
        {
            _adjCentre = new Point((adjustedSize / 2), (adjustedSize / 2));
        }



        /// <summary>
        /// Sets the private Main ring Radius
        /// </summary>
        private void SetMainRingRadius(double adjustedSize, double mainRingThickness)
        {
            _mainRingRadius = (adjustedSize / 2) - (mainRingThickness / 2);
        }



        /// <summary>
        /// Sets the private Track ring Radius
        /// </summary>
        private void SetTrackRingRadius(double adjustedSize, double trackRingThickness)
        {
            _trackRingRadius = (adjustedSize / 2) - (trackRingThickness / 2);
        }



        /// <summary>
        /// Sets the private Width of the control
        /// </summary>
        private void SetControlWidth(double value)
        {
            _controlWidth = value;
        }



        /// <summary>
        /// Sets the private Height of the control
        /// </summary>
        private void SetControlHeight(double value)
        {
            _controlHeight = value;
        }



        /// <summary>
        /// Sets the private control center Point based on control Width and Height
        /// </summary>
        private void SetControlCenterPoint(double controlWidth, double controlHeight)
        {
            _controlCenter = new Point((controlWidth / 2), (controlHeight / 2));
        }



        /// <summary>
        /// Sets the private Container canvas
        /// </summary>
        private void SetContainer(Canvas canvas)
        {
            _container = canvas;
        }



        /// <summary>
        /// Sets the private Main path
        /// </summary>
        private void SetMainPath(Path path)
        {
            _mainPath = path;
        }



        /// <summary>
        /// Sets the private Main path
        /// </summary>
        private void SetTrackPath(Path path)
        {
            _trackPath = path;
        }



        /// <summary>
        /// Sets the private Main Ring Thickness
        /// </summary>
        private void SetMainRingThickness(double value)
        {
            _mainRingThickness = value;
        }



        /// <summary>
        /// Sets the private Track Ring Thickness
        /// </summary>
        private void SetTrackRingThickness(double value)
        {
            _trackRingThickness = value;
        }



        /// <summary>
        /// Sets the private Old value
        /// </summary>
        private void SetOldValue(double value)
        {
            _oldValue = value;
        }



        /// <summary>
        /// Sets the private Minimum value
        /// </summary>
        private void SetMinimumValue(double value)
        {
            _minValue = value;
        }



        /// <summary>
        /// Sets the private Maximum value
        /// </summary>
        private void SetMaximumValue(double value)
        {
            _maxValue = value;
        }



        /// <summary>
        /// Sets the private Normalized min angle
        /// </summary>
        private void SetNormalizedMinAngle(double value)
        {
            _normalizedMinAngle = value;
        }



        /// <summary>
        /// Sets the private Normalized max angle
        /// </summary>
        private void SetNormalizedMaxAngle(double value)
        {
            _normalizedMaxAngle = value;
        }

        #endregion



        #region Initialisation

        /// <summary>
        /// Applies an implicit Style of a matching TargetType
        /// </summary>
        public PercentageRingTwo()
        {
            SizeChanged -= SizeChangedHandler;

            this.DefaultStyleKey = typeof(PercentageRingTwo);

            SizeChanged += SizeChangedHandler;
        }



        /// <summary>
        /// Runs when the control has it's template applied to it
        /// </summary>
        protected override void OnApplyTemplate()
        {
           base.OnApplyTemplate();

            // Get references to the ArcSegment paths
            SetContainer(GetTemplateChild(ContainerPartName) as Canvas);
            SetMainPath(GetTemplateChild(MainPartName) as Path);
            SetTrackPath(GetTemplateChild(TrackPartName) as Path);

            // Update dependency properties
            this.ValueAngle = this.DoubleToAngle(this.Value, Minimum, Maximum, MinAngle, MaxAngle);
            this.Percent = this.GetPercentageString(this.Value, this.Minimum, this.Maximum);

            _testText = this.GetTemplateChild(TestTextPartName) as TextBlock;
            if (_testText != null)
            {
                _testText.Text = "Loaded " + Math.Round(this.Value, 2) + " " + this.Percent + System.Environment.NewLine + "min: " + this.Minimum + System.Environment.NewLine + "max: " + this.Maximum;
            }
        }

        #endregion



        #region Variable updates

        /// <summary>
        /// Updates the normalized minimum and maximum angles.
        /// Ensures <strong>_normalizedMinAngle</strong> is in the range [-180, 180)
        /// and <strong>_normalizedMaxAngle</strong> is in the range [0, 360).
        /// </summary>
        private void UpdateNormalizedAngles()
        {
            var result = Modulus(MinAngle, 360);

            if (result >= 180)
            {
                result = result - 360;
            }

            SetNormalizedMinAngle(result);

            result = Modulus(MaxAngle, 360);

            if (result < 180)
            {
                result = result + 360;
            }
            // Clamps max to 360
            if (result > 0 + 360)
            {
                result = result - 360;
            }

            SetNormalizedMaxAngle(result);
        }



        private void UpdateLayoutVariables(DependencyObject d)
        {
            var pRing = (PercentageRingTwo)d;

            // Control layout
            pRing.SetControlHeight(pRing.Height);
            pRing.SetControlWidth(pRing.Width);

            // We remove padding from the control center point
            double paddedWidth = pRing.Width - (pRing.Padding.Left + pRing.Padding.Right);
            double paddedHeight = pRing.Height - (pRing.Padding.Top + pRing.Padding.Bottom);


            // Now we adjust the container shape
            pRing.SetAdjustedSize(paddedWidth, paddedHeight);
            pRing.AdjustedSize = _adjSize;

            pRing.SetAdjustedCentre(pRing.AdjustedSize);

            // Main ring layout
            pRing.SetMainRingRadius(pRing.AdjustedSize, pRing.MainRingThickness);

            // Track ring layout
            pRing.SetTrackRingRadius(pRing.AdjustedSize, pRing.TrackRingThickness);
        }

        #endregion



        #region Handle property changes

        /// <summary>
        /// This method handles the SizeChanged event for a control.
        /// It adjusts the width and height of the container based on the new size.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing information about the size change.</param>
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            // Cast the sender object to a PercentageRingTwo control.
            var pRing = sender as PercentageRingTwo;

            // Call the UpdateLayoutVariables method with the pRing object.
            UpdateLayoutVariables(pRing);
            UpdateAllRings(pRing);

            // Check if the _container is not null.
            if (_container != null)
            {
                // Set the _container width and height to the adjusted size (_adjSize).
                _container.Width = _adjSize;
                _container.Height = _adjSize;
            }
        }



        /// <summary>
        /// Runs when the OnValueChanged event fires
        /// </summary>
        private void ValueChanged(DependencyObject d, double newValue, double oldValue)
        {
            var pRing = (PercentageRingTwo)d;

            if (!double.IsNaN(pRing.Value))
            {
                pRing.SetOldValue(oldValue);

                // Updates the ValueAngle property
                pRing.ValueAngle = pRing.DoubleToAngle(newValue, Minimum, Maximum, MinAngle, MaxAngle);

                // Updates the Percent value as a formatted string
                pRing.Percent = pRing.GetPercentageString(newValue, pRing.Minimum, pRing.Maximum);

                pRing.UpdateLayoutVariables(d);

                DrawMainRing(d);
                DrawTrackRing(d);

                if (pRing._testText != null)
                {
                    pRing._testText.Text = Math.Round(pRing.Value, 2) + " " + pRing.Percent + System.Environment.NewLine + "min: " + pRing.Minimum + System.Environment.NewLine + "max: " + pRing.Maximum;
                }
            }
        }



        /// <summary>
        /// Runs when the Main ring Thickness property changes
        /// </summary>
        private static void MainRingThicknessChanged(DependencyObject d, double newValue)
        {
            var pRing = (PercentageRingTwo)d;

            pRing.SetMainRingThickness(newValue);

            pRing.UpdateLayoutVariables(d);
            UpdateAllRings(d);
        }



        /// <summary>
        /// Runs when the Track ring Thickness property changes
        /// </summary>
        private static void TrackRingThicknessChanged(DependencyObject d, double newValue)
        {
            var pRing = (PercentageRingTwo)d;

            pRing.SetTrackRingThickness(newValue);

            pRing.UpdateLayoutVariables(d);
            UpdateAllRings(d);
        }

        #endregion



        #region Drawing methods

        /// <summary>
        /// Update all rings
        /// </summary>
        private static void UpdateAllRings(DependencyObject d)
        {
            var pRing = (PercentageRingTwo)d;

            //pRing.UpdateSpacingAngle(pRing.GetMainRingThickness() / 3);

            DrawMainRing(d);

            DrawTrackRing(d);
        }



        private static void DrawMainRing(DependencyObject d)
        {
            var pRing = (PercentageRingTwo)d;

            var path = pRing._mainPath;

            var ringThickness = pRing._mainRingThickness;
            var valueAngle = pRing.DoubleToAngle(pRing.Value, pRing.Minimum, pRing.Maximum, MinAngle, MaxAngle);
            var oldValueAngle = pRing.DoubleToAngle(pRing._oldValue, pRing.Minimum, pRing.Maximum, MinAngle, MaxAngle);
            var ringRadius = pRing._mainRingRadius;
            var ringRadiusAdjust = pRing.AdjustedSize / 2;
            var ringSizeAdjust = (pRing.AdjustedSize / 2) - (pRing._mainRingThickness / 2);
            Point ringCentre = pRing._adjCentre;

            // If the value is less or equal to the Minimum
            if (pRing.Value <= pRing.Minimum)
            {
                if (path != null)
                {
                    pRing.DrawCircle(d, ringCentre, ringRadius, ringThickness, path);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
            // Else if the value is between the minimum and the minimum + 1
            else if (pRing.Value > pRing.Minimum && pRing.Value < pRing.Minimum + 1)
            {
                if (path != null)
                {
                    pRing.DrawArc(
                        d,
                        ringRadiusAdjust,
                        ringSizeAdjust,
                        pRing._normalizedMaxAngle + 0.001,
                        oldValueAngle,
                        pRing.CheckMainArcLargeAngle(valueAngle, 0),
                        pRing._normalizedMinAngle,
                        pRing._normalizedMaxAngle,
                        path,
                        pRing._mainRingThickness,
                        SweepDirection.Clockwise);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Magenta);
                    path.StrokeThickness = pRing.DrawThicknessTransition(d, valueAngle, 4);

                    path.Opacity = 1;
                }
            }
            // Else if the value is greater than or equal to the Maximum
            else if (pRing.Value >= pRing.Maximum)
            {
                if (path != null)
                {
                    pRing.DrawCircle(d, ringCentre, ringRadius, ringThickness, path);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.PaleTurquoise);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
            // For any value between Minimum and Maximum
            else
            {
                if (path != null)
                {
                    pRing.DrawArc(
                        d,
                        ringRadiusAdjust,
                        ringSizeAdjust,
                        valueAngle,
                        oldValueAngle,
                        pRing.CheckMainArcLargeAngle(valueAngle, 0),
                        pRing._normalizedMinAngle,
                        pRing._normalizedMaxAngle,
                        path,
                        pRing._mainRingThickness,
                        SweepDirection.Clockwise);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Yellow);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
        }



        private static void DrawTrackRing(DependencyObject d)
        {
            var pRing = (PercentageRingTwo)d;

            var path = pRing._trackPath;

            var ringThickness = pRing._trackRingThickness;
            var valueAngle = pRing.DoubleToAngle(pRing.Value, pRing.Minimum, pRing.Maximum, MinAngle, MaxAngle);
            var oldValueAngle = pRing.DoubleToAngle(pRing._oldValue, pRing.Minimum, pRing.Maximum, MinAngle, MaxAngle);
            var ringRadius = pRing._trackRingRadius;
            var ringRadiusAdjust = pRing.AdjustedSize / 2;
            var ringSizeAdjust = (pRing.AdjustedSize / 2) - (pRing._trackRingThickness / 2);
            Point ringCentre = pRing._adjCentre;

            var spacing = pRing.GapThicknessToAngle(pRing._mainRingThickness, pRing._trackRingThickness, ringRadiusAdjust);

            // If the value is less or equal to the Minimum
            if (pRing.Value <= pRing.Minimum)
            {
                if (path != null)
                {
                    pRing.DrawCircle(d, ringCentre, ringRadius, ringThickness, path);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
            // Else if the value is between the minimum and the minimum + 1
            else if (pRing.Value > pRing.Minimum && pRing.Value < pRing.Minimum + 1)
            {
                if (path != null)
                {
                    double beginStartAngle = MaxAngle ;                     // Initial start angle
                    double beginEndAngle = MaxAngle - spacing;              // Angle when value = 1

                    double endStartAngle = MinAngle;                        // Initial start angle
                    double endEndAngle = MinAngle + spacing;                // Angle when value = 1

                    pRing.DrawArc(
                        d,
                        ringRadiusAdjust,
                        ringSizeAdjust,
                        valueAngle,
                        oldValueAngle,
                        pRing.CheckTrackArcLargeAngle(valueAngle, 0),
                        GetAdjustedAngle(beginStartAngle, beginEndAngle, valueAngle),
                        GetAdjustedAngle(endStartAngle, endEndAngle, valueAngle),
                        path,
                        pRing._mainRingThickness,
                        SweepDirection.Counterclockwise);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.LightSkyBlue);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
            // Else if the value is greater than or equal to the Maximum
            else if (pRing.Value >= pRing.Maximum)
            {
                if (path != null)
                {
                    pRing.DrawCircle(d, ringCentre, ringRadius, ringThickness, path);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.SeaGreen);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
            // For any value between Minimum and Maximum
            else
            {
                if (path != null)
                {
                    pRing.DrawArc(
                        d,
                        ringRadiusAdjust,
                        ringSizeAdjust,
                        valueAngle + spacing,
                        oldValueAngle,
                        !pRing.CheckMainArcLargeAngle(valueAngle + spacing/5, 0),
                        pRing._normalizedMaxAngle - spacing,
                        pRing._normalizedMinAngle - spacing,
                        path,
                        pRing._mainRingThickness,
                        SweepDirection.Counterclockwise);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
        }



        /// <summary>
        /// Draws an arc segment representing a percentage value of the ring.
        /// </summary>
        /// <param name="d">The DependencyObject representing the PercentageRing.</param>
        /// <param name="ringSize">The center point of the ring.</param>
        /// <param name="valueAngle">The angle corresponding to the percentage value (in degrees).</param>
        /// <param name="oldValueAngle">The previous angle corresponding to the percentage value (in degrees).</param>
        /// <param name="largeArcAngle">A boolean indicating whether the arc is a large arc.</param>
        /// <param name="startAngle">The starting angle of the arc (in degrees).</param>
        /// <param name="endAngle">The ending angle of the arc (in degrees).</param>
        /// <param name="ringPath">The Path control used to display the arc.</param>
        /// <param name="ringThickness">The stroke thickness of the arc.</param>
        /// <param name="sweep">The direction in which the arc sweeps (Clockwise or Counterclockwise).</param>
        private void DrawArc(DependencyObject d, double ringRadius, double ringSize, double valueAngle, double oldValueAngle, bool largeArcAngle, double startAngle, double endAngle, Path ringPath, double ringThickness, SweepDirection sweep)
        {
            if (ringPath != null)
            {
                var pRing = (PercentageRingTwo)d;

                var pg = new PathGeometry();
                var pf = new PathFigure
                {
                    IsClosed = false
                };

                // Sets the start point to the top and centre of the canvas
                pf.StartPoint = pRing.GetPointAroundRadius(startAngle, ringRadius, ringSize);

                var seg = new ArcSegment
                {
                    SweepDirection = sweep,
                    IsLargeArc = largeArcAngle,
                    Size = new Size(ringSize, ringSize),
                    // Sets the end point to an angle, calculated from the value, to a position around the radius of the ring
                    Point = pRing.GetPointAroundRadius(valueAngle, ringRadius, ringSize)
                };

                pf.Segments.Add(seg);
                pg.Figures.Add(pf);

                ringPath.Data = pg;
            }
        }


        private void DrawCircle(DependencyObject d, Point ringCentre, double ringAdjustedRadius, double ringThickness, Path ringPath)
        {
            if (ringPath != null)
            {
                var eg = new EllipseGeometry
                {
                    Center = ringCentre,
                    RadiusX = ringAdjustedRadius
                };

                eg.RadiusY = eg.RadiusX;

                ringPath.Data = eg;
            }
        }



        private double DrawThicknessTransition(DependencyObject d, double valueAngle, double finalAngle)
        {
            var pRing = (PercentageRingTwo)d;

            // Calculate the start angle for thinning
            double startThinningAngle = MinAngle;

            // Calculate the end angle for thinning
            double endThinningAngle = MinAngle + finalAngle;

            // Calculate the current thinning angle
            double currentThinningAngle = valueAngle;

            // Get the initial thickness from the percentage ring
            double startThickness = 0;

            // Set the end thickness to 0
            double endThickness = pRing._mainRingThickness;

            // Initialize the variable for the current thickness value
            double currentThicknessValue;

            // Ensure currentThinningAngle is within the valid range
            currentThinningAngle = Math.Max(startThinningAngle, Math.Min(endThinningAngle, currentThinningAngle));

            // Calculate the interpolation factor (t) based on the angle range
            double t = (currentThinningAngle - startThinningAngle) / (endThinningAngle - startThinningAngle);

            // Linearly interpolate between startThickness and endThickness
            currentThicknessValue = startThickness * (1 - t) + endThickness * t;

            return currentThicknessValue;
        }

        #endregion



        #region Conversion return methods

        /// <summary>
        /// Calculates a point on a circle given an angle and radius.
        /// </summary>
        /// <param name="angle">Angle in degrees.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="centrePoint">Distance from the origin to the center of the circle.</param>
        /// <returns>A Point representing the (x, y) coordinates of the calculated point.</returns>
        private Point GetPointAroundRadius(double angle, double radius, double centrePoint)
        {
            // Convert angle from degrees to radians
            double angleInRadians = (Degrees2Radians * angle);

            // Calculate x and y coordinates
            double x = (radius + (Math.Sin(angleInRadians) * centrePoint));
            double y = (radius - (Math.Cos(angleInRadians) * centrePoint));

            // Create a Point object with the calculated coordinates
            return new Point(x, y);
        }



        /// <summary>
        /// Converts a value within a specified range to an angle within another specified range.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="minValue">The minimum value of the input range.</param>
        /// <param name="maxValue">The maximum value of the input range.</param>
        /// <param name="minAngle">The minimum angle of the output range (in degrees).</param>
        /// <param name="maxAngle">The maximum angle of the output range (in degrees).</param>
        /// <returns>The converted angle.</returns>
        private double DoubleToAngle(double value, double minValue, double maxValue, double minAngle, double maxAngle)
        {
            // If value is below the Minimum set
            if (value < minValue)
            {
                return minAngle;
            }

            // If value is above the Maximum set
            if (value > maxValue)
            {
                return maxAngle;
            }

            // Calculate the interpolated angle
            return ((value - minValue) / (maxValue - minValue) * (maxAngle - minAngle));
        }



        /// <summary>
        /// Converts a value within a specified range to a percentage.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="minValue">The minimum value of the input range.</param>
        /// <param name="maxValue">The maximum value of the input range.</param>
        /// <returns>The percentage value (between 0 and 100).</returns>
        private double DoubleToPercentage(double value, double minValue, double maxValue)
        {
            // Ensure value is within the specified range
            if (value < minValue)
            {
                return 0.0; // Below the range
            }
            else if (value > maxValue)
            {
                return 100.0; // Above the range
            }
            else
            {
                // Calculate the normalized value
                double normalizedValue = (value - minValue) / (maxValue - minValue);

                // Convert to percentage
                double percentage = normalizedValue * 100.0;

                return percentage ;
            }
        }



        /// <summary>
        /// Converts a percentage value to a formatted string.
        /// </summary>
        /// <param name="percent">The percentage value.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>A formatted string representing the percentage.</returns>
        private string GetPercentageString(double percent, double minValue, double maxValue)
        {
            double rounded = DoubleToPercentage(percent, minValue, maxValue);

            return Math.Round(rounded, 0).ToString() + "%";
        }



        /// <summary>
        /// Calculates the total angle needed to accommodate a gap between two strokes around a circle.
        /// </summary>
        /// <param name="thicknessA">The first thickness to compare.</param>
        /// <param name="thicknessB">The second thickness compare.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The gap angle (sum of angles for the larger and smaller strokes).</returns>
        private double GapThicknessToAngle(double thicknessA, double thicknessB, double radius)
        {
            var largerThickness = Math.Max(thicknessA, thicknessB);
            var smallerThickness = Math.Min(thicknessA, thicknessB);

            // Calculate the percentage of largerThickness relative to the radius
            double percentage = (largerThickness / (radius / 2)) * 100;

            // Convert the percentage to an angle within the specified range
            double largeAngle = DoubleToAngle(percentage, Minimum, Maximum, MinAngle, MaxAngle);

            // Calculate the percentage of smallerThickness relative to the radius
            double smallPercentage = (smallerThickness / radius) * 100;

            // Convert the smallPercentage to an angle
            double smallAngle = DoubleToAngle(smallPercentage, Minimum, Maximum, MinAngle, MaxAngle);

            // Return the sum of the large and small angles as the gap angle
            return largeAngle + smallAngle;
        }



        /// <summary>
        /// Calculates the modulus of a number with respect to a divider.
        /// The result is always positive or zero, regardless of the input values.
        /// </summary>
        /// <param name="number">The input number.</param>
        /// <param name="divider">The divider (non-zero).</param>
        /// <returns>The positive modulus result.</returns>
        private double Modulus(double number, double divider)
        {
            // Calculate the modulus
            var result = number % divider;

            // Ensure the result is positive or zero
            result = result < 0 ? result + divider : result;

            return result;
        }



        /// <summary>
        /// Checks if the provided angle and gapAngle is greater than 180 degrees
        /// </summary>
        /// <param name="angle">The angle we want to compare (in degrees).</param>
        /// <param name="gapAngle">The gap angle we may need to account for (in degrees).</param>
        /// <returns>A boolean True if the angle is greater than 180 degrees</returns>
        private bool CheckMainArcLargeAngle(double angle, double gapAngle)
        {
            return angle > 180.0;
        }



        /// <summary>
        /// Checks if the provided angle is less than 180 degrees minus the gapAngle
        /// </summary>
        /// <param name="angle">The angle we want to compare (in degrees).</param>
        /// <param name="gapAngle">The gap angle we may need to account for (in degrees).</param>
        /// <returns>A boolean True if the angle is less than 180 degrees - minus two times the gapAngle</returns>
        private bool CheckTrackArcLargeAngle(double angle, double gapAngle)
        {
            return angle > 180.0 ;
        }



        /// <summary>
        /// Calculates an adjusted angle using linear interpolation (lerp) between the start and end angles.
        /// </summary>
        /// <param name="startAngle">The initial angle.</param>
        /// <param name="endAngle">The final angle.</param>
        /// <param name="valueAngle">A value between 0 and 1 representing the interpolation factor.</param>
        /// <returns>The adjusted angle based on linear interpolation.</returns>
        static double GetAdjustedAngle(double startAngle, double endAngle, double valueAngle)
        {
            // Linear interpolation formula (lerp): GetAdjustedAngle = (startAngle + valueAngle) * (endAngle - startAngle)
            return (startAngle + valueAngle) * (endAngle - startAngle);
        }

        #endregion
    }
}