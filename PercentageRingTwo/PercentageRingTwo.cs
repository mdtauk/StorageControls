using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Net;
using Windows.Foundation;

namespace StorageControls
{
    /// <summary>
    /// A control which takes a value and converts to a percentage displayed on a circular ring
    /// </summary>
    public partial class PercentageRingTwo : Control
    {
        #region 1. Private variables

        double      _containerSize;         // Size of the inner container after padding
        double      _containerCenter;       // Center X and Y value of the inner container
        double      _sharedRadius;          // Radius to be shared by both rings

        double      _oldValue;              // Stores the previous value

        bool        _mainIsThickest;        // True when Main Ring is Thickest of the two
        double      _largerThickness;       // The larger of the two ring thicknesses
        double      _smallerThickness;      // The smaller of the two ring thicknesses

        Canvas      _containerCanvas;       // Reference to the container Canvas
        Path        _mainPath;              // Reference to the Main ring's ArcSegment path
        Path        _trackPath;             // Reference to the Track ring's ArcSegment path
        TextBlock   _testText;              // Temporary reference to the TextBlock in the template

        double      _normalizedMinAngle;    // Stores the normalised Minimum Angle
        double      _normalizedMaxAngle;    // Stores the normalise Maximum Angle
        double      _gapAngle;              // Stores the angle to be used to separate Main and Track rings

        #endregion



        #region 2. Private variable setters

        /// <summary>
        /// Sets the Container size it to the smaller of control's Height and Width.
        /// </summary>
        void SetContainerSize(double controlWidth, double controlHeight)
        {
            _containerSize = Math.Min(controlWidth, controlHeight);
        }

        /// <summary>
        /// Sets the private Container center X and Y value
        /// </summary>
        void SetContainerCenter(double containerSize)
        {
            _containerCenter = (containerSize / 2);
        }

        /// <summary>
        /// Sets the shared Radius by passing in containerSize and thickness.
        /// </summary>
        void SetSharedRadius(double containerSize, double thickness)
        {
            _sharedRadius = (containerSize / 2) - (thickness / 2);
        }

        /// <summary>
        /// Sets the private Old value
        /// </summary>
        void SetOldValue(double value)
        {
            _oldValue = value;
        }

        /// <summary>
        /// Sets the private MainIsThickest Bool
        /// </summary>
        void SetMainIsThickest(double mainThickness, double trackThickness)
        {
            if (mainThickness > trackThickness)
            {
                _mainIsThickest = true;
            }
            else
            {
                _mainIsThickest = false;
            }
        }

        /// <summary>
        /// Sets the private LargerThickness value
        /// </summary>
        void SetLargerThickness(double value)
        {
            _largerThickness = value;
        }

        /// <summary>
        /// Sets the private SmallerThickness value
        /// </summary>
        void SetSmallerThickness(double value)
        {
            _smallerThickness = value;
        }

        /// <summary>
        /// Sets the private Canvas reference
        /// </summary>
        void SetContainerCanvas(Canvas canvas)
        {
            _containerCanvas = canvas;
        }

        /// <summary>
        /// Sets the private Main ring's Path reference
        /// </summary>
        void SetMainRingPath(Path path)
        {
            _mainPath = path;
        }

        /// <summary>
        /// Sets the private Track ring's Path reference
        /// </summary>
        void SetTrackRingPath(Path path)
        {
            _trackPath = path;
        }

        /// <summary>
        /// Sets the private Normalized min angle
        /// </summary>
        void SetNormalizedMinAngle(double angle)
        {
            _normalizedMinAngle = angle;
        }

        /// <summary>
        /// Sets the private Normalized max angle
        /// </summary>
        void SetNormalizedMaxAngle(double angle)
        {
            _normalizedMaxAngle = angle;
        }

        /// <summary>
        /// Sets the private Gap angle
        /// </summary>
        void SetGapAngle(double angle)
        {
            _gapAngle = angle;
        }

        #endregion



        #region 3. Private variable getters

        double GetContainerSize()
        {
            return _containerSize;
        }

        double GetContainerCenter()
        {
            return _containerCenter;
        }

        double GetSharedRadius()
        {
            return _sharedRadius;
        }

        double GetOldValue()
        {
            return _oldValue;
        }

        bool CheckMainIsThickest()
        {
            return _mainIsThickest;
        }

        double GetLargerThickness()
        {
            return _largerThickness;
        }

        double GetSmallerThickness()
        {
            return _smallerThickness;
        }

        Canvas GetContainerCanvas()
        {
            return _containerCanvas;
        }

        Path GetMainRingPath()
        {
            return _mainPath;
        }

        Path GetTrackRingPath()
        {
            return _trackPath;
        }

        double GetNormalizedMinAngle()
        {
            return _normalizedMinAngle;
        }

        double GetNormalizedMaxAngle()
        {
            return _normalizedMaxAngle;
        }

        double GetGapAngle()
        {
            return _gapAngle;
        }

        #endregion



        #region 4. Initialization

        /// <summary>
        /// Applies an implicit Style of a matching TargetType
        /// </summary>
        public PercentageRingTwo()
        {
            SizeChanged -= SizeChangedHandler;

            DefaultStyleKey = typeof(PercentageRingTwo);

            SizeChanged += SizeChangedHandler;
        }


        /// <summary>
        /// Runs when the control has it's template applied to it
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeValues();

            _testText = GetTemplateChild(TestTextPartName) as TextBlock;

            if (_testText != null)
            {
                //_testText.Text = "Loaded " + Math.Round(Value, 2) + " " + Percent + System.Environment.NewLine + "min: " + Minimum + System.Environment.NewLine + "max: " + Maximum;
            }
        }



        private void InitializeValues()
        {
            SetContainerCanvas(GetTemplateChild(ContainerPartName) as Canvas);
            SetMainRingPath(GetTemplateChild(MainPartName) as Path);
            SetTrackRingPath(GetTemplateChild(TrackPartName) as Path);

            SetContainerSize(Width, Height);
            SetContainerCenter(GetContainerSize());
            AdjustedSize = GetContainerSize();

            SetMainIsThickest(MainRingThickness, TrackRingThickness);

            if (CheckMainIsThickest() == true)
            {
                SetLargerThickness(MainRingThickness);
                SetSmallerThickness(TrackRingThickness);
            }
            else
            {
                SetLargerThickness(TrackRingThickness);
                SetSmallerThickness(MainRingThickness);
            }

            SetSharedRadius(GetContainerSize(), GetLargerThickness());

            SetGapAngle(GapThicknessToAngle(GetLargerThickness(), GetSmallerThickness(), GetContainerSize()));

            // Update protected dependency properties
            ValueAngle = DoubleToAngle(Value, Minimum, Maximum, MinAngle, MaxAngle);
            Percent = GetPercentageString(Value, Minimum, Maximum);
        }

        #endregion



        #region 5. Variable updates

        /// <summary>
        /// Updates the normalized minimum and maximum angles.
        /// Ensures <strong>_normalizedMinAngle</strong> is in the range [-180, 180)
        /// and <strong>_normalizedMaxAngle</strong> is in the range [0, 360).
        /// </summary>
        void UpdateNormalizedAngles()
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



        void UpdateLayout(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRingTwo)d;

            SetMainIsThickest(MainRingThickness, TrackRingThickness);

            if (CheckMainIsThickest() == true)
            {
                SetLargerThickness(MainRingThickness);
                SetSmallerThickness(TrackRingThickness);
            }
            else
            {
                SetLargerThickness(TrackRingThickness);
                SetSmallerThickness(MainRingThickness);
            }

            SetSharedRadius(GetContainerSize(), GetLargerThickness());

            SetGapAngle(GapThicknessToAngle(GetLargerThickness(), GetSmallerThickness(), GetContainerSize()));

            if (pRing._testText != null)
            {
               pRing._testText.Text = Math.Round(pRing.Value, 2) + " " + pRing.Percent + System.Environment.NewLine + "gapAngle: " + pRing.GetGapAngle();
            }

            DrawAllRings(pRing, useTransition);
        }

        #endregion



        #region 6. Handle property changes

        /// <summary>
        /// This method handles the SizeChanged event for a control.
        /// It adjusts the width and height of the container based on the new size.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing information about the size change.</param>
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != e.NewSize)
            {
                // Cast the sender object to a PercentageRingTwo control.
                var pRing = sender as PercentageRingTwo;

                SetContainerSize(Width, Height);
                SetContainerCenter(GetContainerSize());
                AdjustedSize = GetContainerSize();

                // Check if the Container is not null.
                if (GetContainerCanvas() != null)
                {
                    var container = GetContainerCanvas();

                    // Set the _container width and height to the adjusted size (_adjSize).
                    container.Width = GetContainerSize();
                    container.Height = GetContainerSize();
                }

                UpdateLayout(pRing, false);
            }
;
        }



        /// <summary>
        /// Runs when the OnValueChanged event fires
        /// </summary>
        private void ValueChanged(DependencyObject d, double newValue, double oldValue)
        {
            if (newValue != oldValue)
            {
                var pRing = (PercentageRingTwo)d;

                if (!double.IsNaN(pRing.Value))
                {
                    pRing.SetOldValue(oldValue);

                    // Updates the ValueAngle property
                    pRing.ValueAngle = pRing.DoubleToAngle(newValue, Minimum, Maximum, MinAngle, MaxAngle);

                    // Updates the Percent value as a formatted string
                    pRing.Percent = pRing.GetPercentageString(newValue, pRing.Minimum, pRing.Maximum);

                    UpdateLayout(d, true);

                    if (pRing._testText != null)
                    {
                        // pRing._testText.Text = Math.Round(pRing.Value, 2) + " " + pRing.Percent + System.Environment.NewLine + "angle: " + pRing.ValueAngle + System.Environment.NewLine + "size: " + pRing.AdjustedSize + System.Environment.NewLine + "min: " + pRing.Minimum + System.Environment.NewLine + "max: " + pRing.Maximum;
                    }
                }
            }
;
        }



        /// <summary>
        /// Runs when either Ring Thickness property changes
        /// </summary>
        private static void RingThicknessChanged(DependencyObject d, double newValue)
        {
            var pRing = (PercentageRingTwo)d;

            pRing.SetMainIsThickest(pRing.MainRingThickness, pRing.TrackRingThickness);

            if (pRing.CheckMainIsThickest())
            {
                pRing.SetLargerThickness(pRing.MainRingThickness);
                pRing.SetSmallerThickness(pRing.TrackRingThickness);
            }
            else
            {
                pRing.SetLargerThickness(pRing.MainRingThickness);
                pRing.SetSmallerThickness(pRing.TrackRingThickness);
            }

            pRing.SetGapAngle(pRing.GapThicknessToAngle(pRing.GetLargerThickness(), pRing.GetSmallerThickness(), pRing.GetContainerSize()));
            pRing.UpdateLayout(d, false);
        }

        #endregion



        #region 7. Drawing methods

        /// <summary>
        /// Draws both rings
        /// </summary>
        private static void DrawAllRings(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRingTwo)d;

            DrawMainRing(d, useTransition);

            DrawTrackRing(d, useTransition);
        }



        private static void DrawMainRing(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRingTwo)d;

            var path = pRing.GetMainRingPath();

            if (path != null)
            {
                var ringThickness = pRing.MainRingThickness;
                var containerCentre = pRing.GetContainerCenter();

                var radius = pRing.GetSharedRadius();
                var gapAngle = pRing.GetGapAngle();
                var centre = new Point(containerCentre, containerCentre);
                var size = pRing.GetSharedRadius();

                //
                // If the value is less or equal to the Minimum
                if (pRing.Value <= pRing.Minimum)
                {
                    if (path != null)
                    {
                        pRing.DrawCircle(d, centre, radius, ringThickness, path);
                        path.StrokeThickness = 0;
                    }
                }
                //
                // Else if the value is between the minimum (with an offset) and the minimum + 1
                else if (pRing.Value > (pRing.Minimum + 0.01) && pRing.Value < pRing.Minimum + 1)
                {
                    var startPoint = pRing.GetPointAroundRadius(MinAngle, radius, containerCentre);
                    var endPoint = pRing.GetPointAroundRadius(MinAngle + 0.1, radius, containerCentre);

                    pRing.DrawArc(
                        d,
                        path,
                        startPoint,
                        endPoint,
                        new Size(size, size),
                        pRing.CheckMainArcLargeAngle(pRing.ValueAngle, 0),
                        SweepDirection.Clockwise,
                        gapAngle);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Turquoise);
                    path.StrokeThickness = pRing.DrawThicknessTransition(d, pRing.Minimum + 0.01, pRing.Value, pRing.Minimum + 1.01, 0.0, ringThickness, true);

                    path.Opacity = 1;
                }
                //
                // Else if the value is greater than or equal to the Maximum
                else if (pRing.Value >= pRing.Maximum)
                {
                    pRing.DrawCircle(d, centre, radius, ringThickness, path);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Magenta);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
                //
                // For any value between Minimum and Maximum
                else
                {
                    var startPoint = pRing.GetPointAroundRadius(MinAngle, radius, containerCentre);
                    var endPoint = pRing.GetPointAroundRadius(pRing.ValueAngle - 0.1, radius, containerCentre);

                    pRing.DrawArc(
                        d,
                        path,
                        startPoint,
                        endPoint,
                        new Size(size, size),
                        pRing.CheckMainArcLargeAngle(pRing.ValueAngle, 0),
                        SweepDirection.Clockwise,
                        gapAngle);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Goldenrod);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
        }



        private static void DrawTrackRing(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRingTwo)d;

            var path = pRing.GetTrackRingPath();

            if (path != null)
            {
                var ringThickness = pRing.TrackRingThickness;
                var containerCentre = pRing.GetContainerCenter();

                var radius = pRing.GetSharedRadius();
                var gapAngle = pRing.GetGapAngle();
                var centre = new Point(containerCentre, containerCentre);

                var size = radius;

                // If the value is less or equal to the Minimum
                if (pRing.Value <= pRing.Minimum)
                {
                    if (path != null)
                    {
                        pRing.DrawCircle(d, centre, radius, ringThickness, path);

                        path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.DarkTurquoise);
                        path.StrokeThickness = ringThickness;

                        path.Opacity = 1;
                    }
                }
                // Else if the value is between the minimum and the minimum + 1
                else if (pRing.Value > pRing.Minimum && pRing.Value < pRing.Minimum + 1)
                {
                    if (path != null)
                    {
                        var beginStartAngle = MaxAngle;                     // Initial start angle
                        var beginEndAngle = MaxAngle - gapAngle;            // Angle when value = 1

                        var endStartAngle = MinAngle;                       // Initial start angle
                        var endEndAngle = MinAngle + gapAngle;              // Angle when value = 1

                        double adjustedStartAngle = pRing.DrawAdjustedAngle(pRing, pRing.Minimum, pRing.Value, pRing.Minimum + 1, beginStartAngle, beginEndAngle, pRing.ValueAngle, true);
                        var adjustedEndAngle = pRing.DrawAdjustedAngle(pRing, pRing.Minimum, pRing.Value, pRing.Minimum + 1, endStartAngle, endEndAngle, pRing.ValueAngle, true);

                        var startPoint = pRing.GetPointAroundRadius(adjustedStartAngle, radius, containerCentre);
                        var endPoint = pRing.GetPointAroundRadius(adjustedEndAngle, radius, containerCentre);

                        pRing.DrawArc(
                            d,
                            path,
                            startPoint,
                            endPoint,
                            new Size(size, size),
                            pRing.CheckTrackArcLargeAngle(pRing.ValueAngle, gapAngle),
                            SweepDirection.Counterclockwise,
                            gapAngle);

                        path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.DarkMagenta);
                        path.StrokeThickness = ringThickness;

                        path.Opacity = 1;
                    }
                }
                //
                // Else if the value is greater than or equal to the Maximum
                else if (pRing.Value >= pRing.Maximum)
                {
                    pRing.DrawCircle(d, centre, radius, ringThickness, path);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.DarkGoldenrod);
                    path.StrokeThickness = 0;

                    path.Opacity = 1;
                }
                //
                // For any value between Minimum and Maximum
                else
                {
                    var startPoint = pRing.GetPointAroundRadius(-gapAngle, radius, containerCentre);
                    var endPoint = pRing.GetPointAroundRadius(pRing.ValueAngle + gapAngle, radius, containerCentre);

                    pRing.DrawArc(
                        d,
                        path,
                        startPoint,
                        endPoint,
                        new Size(size, size),
                        pRing.CheckTrackArcLargeAngle(pRing.ValueAngle, gapAngle),
                        SweepDirection.Counterclockwise,
                        gapAngle);

                    path.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Black);
                    path.StrokeThickness = ringThickness;

                    path.Opacity = 1;
                }
            }
        }



        private void DrawArc(DependencyObject d, Path ringPath, Point startPoint, Point endPoint, Size size, bool largeArc, SweepDirection sweepDirection, double test)
        {
            if (ringPath != null)
            {
                var pRing = (PercentageRingTwo)d;

                var pg = new PathGeometry();

                var pf = new PathFigure
                {
                    IsClosed = false,
                    IsFilled = false,
                };

                // Sets the start point to the top and centre of the canvas
                pf.StartPoint = startPoint;

                var seg = new ArcSegment
                {
                    SweepDirection = sweepDirection,
                    IsLargeArc = largeArc,
                    Size = size,
                    // Sets the end point to an angle, calculated from the value, to a position around the radius of the ring
                    Point = endPoint,
                };

                pf.Segments.Add(seg);
                pg.Figures.Add(pf);

                ringPath.Data = pg;

                if (pRing._testText != null)
                {
                    //pRing._testText.Text = pRing.Percent + "startPoint: " + pf.StartPoint + System.Environment.NewLine + "size: " + size + System.Environment.NewLine + "endPoint:" + endPoint + System.Environment.NewLine + test;
                }
            }
        }



        private void DrawCircle(DependencyObject d, Point ringCentre, double ringRadius, double ringThickness, Path ringPath)
        {
            if (ringPath != null)
            {
                var eg = new EllipseGeometry
                {
                    Center = ringCentre,
                    RadiusX = ringRadius,
                };

                eg.RadiusY = eg.RadiusX;

                ringPath.Data = eg;
            }
        }



        /// <summary>
        /// Interpolates a thickness value between startThickness and endThickness based on the value moving between startValue and endValue.
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="startValue">The start value of the range.</param>
        /// <param name="value">The current value within the range.</param>
        /// <param name="endValue">The end value of the range.</param>
        /// <param name="startThickness">The starting thickness value.</param>
        /// <param name="endThickness">The ending thickness value.</param>
        /// <returns>The interpolated thickness value.</returns>
        private double DrawThicknessTransition(DependencyObject d, double startValue, double value, double endValue, double startThickness, double endThickness, bool useEasing)
        {
            // Ensure that value is within the range [startValue, endValue]
            value = Math.Max(startValue, Math.Min(endValue, value));

            // Calculate the interpolation factor (t) between 0 and 1
            var t = (value - startValue) / (endValue - startValue);

            double interpolatedThickness;

            if (useEasing)
            {
                // Apply an easing function (e.g., quadratic ease-in-out)
                var easedT = EasingFunction(t);

                // Interpolate the thickness
                interpolatedThickness = startThickness + easedT * (endThickness - startThickness);
            }
            else
            {
                // Interpolate the thickness
                interpolatedThickness = startThickness + t * (endThickness - startThickness);
            }

            return interpolatedThickness;
        }



        /// <summary>
        /// Calculates an adjusted angle using linear interpolation (lerp) between the start and end angles.
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="startValue">The start value of the range.</param>
        /// <param name="value">The current value within the range.</param>
        /// <param name="endValue">The end value of the range.</param>
        /// <param name="startAngle">The initial angle.</param>
        /// <param name="endAngle">The final angle.</param>
        /// <param name="value">The current value within the range.</param>
        /// <returns>The adjusted angle based on linear interpolation.</returns>
        private double DrawAdjustedAngle(DependencyObject d, double startValue, double value, double endValue, double startAngle, double endAngle, double valueAngle, bool useEasing)
        {
            // Ensure that value is within the range [startValue, endValue]
            value = Math.Max(startValue, Math.Min(endValue, value));

            // Calculate the interpolation factor (t) between 0 and 1
            var t = (value - startValue) / (endValue - startValue);

            double interpolatedAngle;

            if (useEasing)
            {
                // Apply an easing function (e.g., quadratic ease-in-out)
                var easedT = EasingFunction(t);

                // Interpolate the thickness
                interpolatedAngle = startAngle + easedT * (endAngle - startAngle);
            }
            else
            {
                // Interpolate the thickness
                interpolatedAngle = startAngle + t * (endAngle - startAngle);
            }

            return interpolatedAngle;
        }



        // Example quadratic ease-in-out function
        private double EasingFunction(double t)
        {
            // Quadratic ease-in-out
            return t < 0.5 ? 2 * t * t : 1 - Math.Pow(-2 * t + 2, 2) / 2;
        }

        #endregion



        #region 8. Conversion return methods

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
            var angleInRadians = (Degrees2Radians * angle);

            // Calculate x and y coordinates
            var x = (centrePoint + (Math.Sin(angleInRadians) * radius));
            var y = (centrePoint - (Math.Cos(angleInRadians) * radius));

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
                var normalizedValue = (value - minValue) / (maxValue - minValue);

                // Convert to percentage
                var percentage = normalizedValue * 100.0;

                return percentage;
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
            var rounded = DoubleToPercentage(percent, minValue, maxValue);

            return Math.Round(rounded, 0).ToString() + "%";
        }



        /// <summary>
        /// Calculates the total angle needed to accommodate a gap between two strokes around a circle.
        /// </summary>
        /// <param name="thicknessA">The first thickness to compare.</param>
        /// <param name="thicknessB">The second thickness compare.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The gap angle (sum of angles for the larger and smaller strokes).</returns>
        private double GapThicknessToAngle(double largerThickness, double smallerThickness, double radius)
        {
            if (!double.IsNaN(largerThickness) || !double.IsNaN(smallerThickness) && !double.IsNaN(radius))
            {
                // Calculate the percentage of largerThickness relative to the radius
                var largerPercentage = largerThickness / radius * 100;

                double largerAngle;

                // Convert the percentage to an angle within the specified range
                if (largerPercentage > 80)
                {
                    largerAngle = DoubleToAngle(80, Minimum, Maximum, MinAngle, MaxAngle);
                }
                else
                {
                    largerAngle = DoubleToAngle(largerPercentage, Minimum, Maximum, MinAngle, MaxAngle);
                }

                // Calculate the percentage of largerThickness relative to the radius
                var smallerPercentage = smallerThickness / radius * 100;

                double smallerAngle;

                // Convert the percentage to an angle within the specified range
                if (smallerPercentage > 80)
                {
                    smallerAngle = DoubleToAngle(80, Minimum, Maximum, MinAngle, MaxAngle);
                }
                else
                {
                    smallerAngle = DoubleToAngle(smallerPercentage, Minimum, Maximum, MinAngle, MaxAngle);
                }

                return largerAngle;
            }

            return 0;
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
            return angle < 180.0 - (gapAngle * 2);
        }



        /// <summary>
        /// Calculates an adjusted angle using linear interpolation (lerp) between the start and end angles.
        /// </summary>
        /// <param name="startAngle">The initial angle.</param>
        /// <param name="endAngle">The final angle.</param>
        /// <param name="valueAngle">A value between 0 and 1 representing the interpolation factor.</param>
        /// <returns>The adjusted angle based on linear interpolation.</returns>
        private static double GetAdjustedAngle(double startAngle, double endAngle, double valueAngle)
        {
            // Linear interpolation formula (lerp): GetAdjustedAngle = (startAngle + valueAngle) * (endAngle - startAngle)
            return (startAngle + valueAngle) * (endAngle - startAngle);
        }

        #endregion
    }
}