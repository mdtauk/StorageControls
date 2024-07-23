using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Net;
using System.Security.Cryptography;
using Windows.Foundation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StorageControls
{
    /// <summary>
    /// A control which takes a value and converts to a percentage displayed on a circular ring
    /// </summary>
    public partial class PercentageRing : Control
    {
        #region 1. Private variables

        double              _containerSize;         // Size of the inner container after padding
        double              _containerCenter;       // Center X and Y value of the inner container
        double              _sharedRadius;          // Radius to be shared by both rings

        double              _oldValue;              // Stores the previous value

        bool                _mainIsThickest;        // True when Main Ring is Thickest of the two
        double              _largerThickness;       // The larger of the two ring thicknesses
        double              _smallerThickness;      // The smaller of the two ring thicknesses

        Canvas              _containerCanvas;       // Reference to the container Canvas
        Path                _mainPath;              // Reference to the Main ring's ArcSegment path
        Path                _trackPath;             // Reference to the Track ring's ArcSegment path
        RectangleGeometry   _clipRect;              // Clipping RectangleGeometry for the canvas

        double              _normalizedMinAngle;    // Stores the normalised Minimum Angle
        double              _normalizedMaxAngle;    // Stores the normalise Maximum Angle
        double              _gapAngle;              // Stores the angle to be used to separate Main and Track rings

        #endregion



        #region 2. Private variable setters

        /// <summary>
        /// Sets the Container size to the smaller of control's Height and Width.
        /// </summary>
        void SetContainerSize(double controlWidth, double controlHeight, Thickness padding)
        {
            double correctedWidth = controlWidth - (padding.Left + padding.Right);
            double correctedHeight = controlHeight - (padding.Top + padding.Bottom);
            
            double check = Math.Min(correctedWidth, correctedHeight);
            double minSize = 8;

            if (check < minSize)
            {
                _containerSize = minSize;
            }
            else
            {
                _containerSize = check;
            }
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
            double check = (containerSize / 2) - (thickness / 2);
            double minSize = 4;

            if (check <= minSize)
            {
                _sharedRadius = minSize;
            }
            else
            {
                _sharedRadius = check;
            }
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
        /// Sets the clipping RectangleGeometry for the Canvas
        /// </summary>
        void SetClippingRectGeo(RectangleGeometry clipRectGeo)
        { 
            _clipRect = clipRectGeo;
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

        /// <summary>
        /// Gets the Container size
        /// </summary>
        double GetContainerSize()
        {
            return _containerSize;
        }

        /// <summary>
        /// Gets the Container Center
        /// </summary>
        double GetContainerCenter()
        {
            return _containerCenter;
        }

        /// <summary>
        /// Gets the Shared Radius
        /// </summary>
        double GetSharedRadius()
        {
            return _sharedRadius;
        }

        /// <summary>
        /// Gets the Old value
        /// </summary>
        double GetOldValue()
        {
            return _oldValue;
        }

        /// <summary>
        /// Gets the MainIsThickest boolean value
        /// </summary>
        bool CheckMainIsThickest()
        {
            return _mainIsThickest;
        }

        /// <summary>
        /// Gets the Larger Thickness
        /// </summary>
        double GetLargerThickness()
        {
            return _largerThickness;
        }

        /// <summary>
        /// Gets the Smaller Thickness
        /// </summary>
        double GetSmallerThickness()
        {
            return _smallerThickness;
        }

        /// <summary>
        /// Gets the Container Canvas reference
        /// </summary>
        Canvas GetContainerCanvas()
        {
            return _containerCanvas;
        }

        /// <summary>
        /// Gets the Main Ring Path reference
        /// </summary>
        Path GetMainRingPath()
        {
            return _mainPath;
        }

        /// <summary>
        /// Gets the Track Ring Path reference
        /// </summary>
        Path GetTrackRingPath()
        {
            return _trackPath;
        }

        /// <summary>
        /// Gets the clipping RectangleGeometry reference
        /// </summary>
        RectangleGeometry GetClippingRectGeo()
        {
            return _clipRect;
        }

        /// <summary>
        /// Gets the Normalized Minimuim Angle
        /// </summary>
        double GetNormalizedMinAngle()
        {
            return _normalizedMinAngle;
        }

        /// <summary>
        /// Gets the Normalized Maximum Angle
        /// </summary>
        double GetNormalizedMaxAngle()
        {
            return _normalizedMaxAngle;
        }

        /// <summary>
        /// Gets the Gap Angle
        /// </summary>
        double GetGapAngle()
        {
            return _gapAngle;
        }

        #endregion



        #region 4. Initialization

        /// <summary>
        /// Applies an implicit Style of a matching TargetType
        /// </summary>
        public PercentageRing()
        {
            SizeChanged -= SizeChangedHandler;

            DefaultStyleKey = typeof(PercentageRing);

            SizeChanged += SizeChangedHandler;
        }


        /// <summary>
        /// Runs when the control has it's template applied to it
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeValues();
        }



        /// <summary>
        /// Initializes the values and properties of a PercentageRing control.
        /// </summary>
        private void InitializeValues()
        {
            // Retrieve references to visual elements
            SetContainerCanvas(GetTemplateChild(ContainerPartName) as Canvas);
            SetMainRingPath(GetTemplateChild(MainPartName) as Path);
            SetTrackRingPath(GetTemplateChild(TrackPartName) as Path);

            // Set container size and center
            SetContainerSize(Width, Height, Padding);
            SetContainerCenter(GetContainerSize());
            AdjustedSize = GetContainerSize();

            RectangleGeometry rectGeo = new RectangleGeometry();
            rectGeo.Rect = new Rect(0, 0, AdjustedSize, AdjustedSize);
            SetClippingRectGeo(rectGeo);

            // Determine which ring thickness is thickest
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

            // Calculate the shared radius based on container size and larger thickness
            SetSharedRadius(GetContainerSize(), GetLargerThickness());

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
            // Calculate the modulus of MinAngle to ensure it's within [0, 360)
            var result = Modulus(MinAngle, 360);

            if (result >= 180)
            {
                result = result - 360;
            }

            SetNormalizedMinAngle(result);

            // Calculate the modulus of MaxAngle to ensure it's within [0, 360)
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


        /// <summary>
        /// Updates the layout of a PercentageRing control.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="useTransition">Indicates whether to use a transition effect.</param>
        void UpdateLayout(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRing)d;

            // Determine which ring thickness is thickest
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

            // Calculate the shared radius based on container size and larger thickness
            SetSharedRadius(GetContainerSize(), GetLargerThickness());

            // Calculate the gap angle
            double angle = GapThicknessToAngle(GetSharedRadius(), GetLargerThickness() / 1.5);
            SetGapAngle(angle);

            // Draw all rings with the specified transition
            DrawAllRings(pRing, useTransition, pRing.GetMainRingPath());
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
                // Cast the sender object to a PercentageRing control.
                var pRing = sender as PercentageRing;

                SetContainerSize(Width, Height, Padding);
                SetContainerCenter(GetContainerSize());
                AdjustedSize = GetContainerSize();

                RectangleGeometry rectGeo = new RectangleGeometry();
                rectGeo.Rect = new Rect(0, 0, AdjustedSize, AdjustedSize);
                SetClippingRectGeo(rectGeo);

                // Check if the Container is not null.
                if (GetContainerCanvas() != null)
                {
                    var container = GetContainerCanvas();

                    // Set the _container width and height to the adjusted size (_adjSize).
                    container.Width = GetContainerSize();
                    container.Height = GetContainerSize();
                    container.Clip = GetClippingRectGeo();
                }


                UpdateLayout(pRing, false);
            };
        }



        /// <summary>
        /// Runs when the OnValueChanged event fires
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        private void ValueChanged(DependencyObject d, double newValue, double oldValue)
        {
            if (newValue != oldValue)
            {
                var pRing = (PercentageRing)d;

                if (!double.IsNaN(pRing.Value))
                {
                    pRing.SetOldValue(oldValue);

                    // Updates the ValueAngle property
                    pRing.ValueAngle = pRing.DoubleToAngle(newValue, Minimum, Maximum, MinAngle, MaxAngle);

                    // Updates the Percent value as a formatted string
                    pRing.Percent = pRing.GetPercentageString(newValue, pRing.Minimum, pRing.Maximum);

                    UpdateLayout(d, true);
                }
            };
        }



        /// <summary>
        /// Runs when either ring Thickness property changes
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        private static void RingThicknessChanged(DependencyObject d, double newValue)
        {
            var pRing = (PercentageRing)d;

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
            pRing.UpdateLayout(d, false);
        }



        /// <summary>
        /// Runs when either ring Brush property changes
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        private static void BrushChanged(DependencyObject d, Brush newValue)
        {
            var pRing = (PercentageRing)d;

            pRing.UpdateLayout(d, false);
        }


        /// <summary>
        /// Runs when either ring StartCap and EndCap properties change
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        private static void StrokeCapChanged(DependencyObject d, PenLineCap newValue)
        {
            var pRing = (PercentageRing)d;

            pRing.UpdateLayout(d, false);
        }

        #endregion



        #region 7. Drawing methods

        /// <summary>
        /// Draws both rings
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="useTransition">Indicates whether to use a transition effect.</param>
        private static void DrawAllRings(DependencyObject d, bool useTransition, Path path)
        {
            if (path != null)
            {
                var pRing = (PercentageRing)d;

                DrawMainRing(d, useTransition);

                DrawTrackRing(d, useTransition);
            }
        }



        /// <summary>
        /// Draws The Main ring
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="useTransition">Indicates whether to use a transition effect.</param>
        private static void DrawMainRing(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRing)d;

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
                else if (pRing.Value > (pRing.Minimum) && pRing.Value < pRing.Minimum + 1)
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
                        SweepDirection.Clockwise);

                    path.StrokeThickness = pRing.DrawThicknessTransition(d, pRing.Minimum + 0.01, pRing.Value, pRing.Minimum + 1.01, 0.0, ringThickness, true);
                }
                //
                // Else if the value is greater than or equal to the Maximum
                else if (pRing.Value >= pRing.Maximum)
                {
                    pRing.DrawCircle(d, centre, radius, ringThickness, path);

                    path.StrokeThickness = ringThickness;
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
                        SweepDirection.Clockwise);

                    path.StrokeThickness = ringThickness;
                }
            }
        }



        /// <summary>
        /// Draws the Track ring
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="useTransition">Indicates whether to use a transition effect.</param>
        private static void DrawTrackRing(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRing)d;

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
                if (pRing.Value <= pRing.Minimum + 0.1)
                {
                    if (path != null)
                    {
                        pRing.DrawCircle(d, centre, radius, ringThickness, path);

                        path.StrokeThickness = ringThickness;
                    }
                }
                // Else if the value is between the minimum and the minimum + 1
                else if (pRing.Value > pRing.Minimum && pRing.Value < pRing.Minimum + 1.01)
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
                            SweepDirection.Counterclockwise);

                        path.StrokeThickness = ringThickness;
                    }
                }
                //
                // Else if the value is greater than or equal to the Maximum
                else if (pRing.Value >= pRing.Maximum)
                {
                    pRing.DrawCircle(d, centre, radius, ringThickness, path);

                    path.StrokeThickness = 0;
                }                
                //
                // For any value between Minimum and Maximum
                else
                {
                    var startPoint = pRing.GetPointAroundRadius(MinAngle - gapAngle, radius, containerCentre);
                    var endPoint = pRing.GetPointAroundRadius(pRing.ValueAngle + gapAngle, radius, containerCentre);

                    //
                    // Special condition where the value approaches the end of the track
                    if (pRing.Value > 1 && pRing.ValueAngle > (MaxAngle - (gapAngle * 2)))
                    {
                        var endPoint2 = pRing.GetPointAroundRadius((MaxAngle - gapAngle) - 0.1, radius, containerCentre);

                        pRing.DrawArc(
                            d,
                            path,
                            startPoint,
                            endPoint2,
                            new Size(size, size),
                            pRing.CheckTrackArcLargeAngle(pRing.ValueAngle, gapAngle),
                            SweepDirection.Counterclockwise);

                        path.StrokeThickness = pRing.DrawThicknessTransition(d, (MaxAngle - (gapAngle * 2)), pRing.ValueAngle, (MaxAngle - gapAngle), ringThickness, 0.0, true);
                    }
                    else
                    {
                        pRing.DrawArc(
                            d,
                            path,
                            startPoint,
                            endPoint,
                            new Size(size, size),
                            pRing.CheckTrackArcLargeAngle(pRing.ValueAngle, gapAngle),
                            SweepDirection.Counterclockwise);

                        path.StrokeThickness = ringThickness;

                        path.Visibility = Visibility.Visible;
                    }                    
                }
            }
        }



        /// <summary>
        /// Draws an arc segment on a canvas using the specified parameters.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="ringPath">The Path element to which the arc segment is assigned.</param>
        /// <param name="startPoint">The starting point of the arc.</param>
        /// <param name="endPoint">The ending point of the arc.</param>
        /// <param name="size">The size of the arc (width and height).</param>
        /// <param name="largeArc">Indicates whether the arc should be a large arc.</param>
        /// <param name="sweepDirection">The direction in which the arc sweeps.</param>
        private void DrawArc(DependencyObject d, Path ringPath, Point startPoint, Point endPoint, Size size, bool largeArc, SweepDirection sweepDirection)
        {
            if (ringPath != null)
            {
                var pRing = (PercentageRing)d;

                var pg = new PathGeometry();

                var pf = new PathFigure
                {
                    IsClosed = false,
                    IsFilled = false,
                };

                // Checks to ensure size is not negative or zero
                Size sizeChecked;
                if (size.Height <= 0 || size.Width <= 0)
                {
                    sizeChecked = new Size(10, 10);
                }
                else
                {
                    sizeChecked = size;
                }                

                // Sets the start point to the top and center of the canvas
                pf.StartPoint = startPoint;

                var seg = new ArcSegment
                {
                    SweepDirection = sweepDirection,
                    IsLargeArc = largeArc,
                    Size = sizeChecked,
                    // Sets the end point to an angle, calculated from the value, to a position around the radius of the ring
                    Point = endPoint,
                };

                pf.Segments.Add(seg);
                pg.Figures.Add(pf);

                ringPath.Data = pg;
            };
        }



        /// <summary>
        /// Draws a circular path (ellipse) on a canvas using the specified parameters.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="ringCentre">The center point of the circle.</param>
        /// <param name="ringRadius">The radius of the circle.</param>
        /// <param name="ringThickness">The thickness of the circle (used for the ellipse geometry).</param>
        /// <param name="ringPath">The Path element to which the ellipse geometry is assigned.</param>
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
            };
        }



        /// <summary>
        /// Calculates an interpolated thickness value based on the provided parameters.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="startValue">The starting value for interpolation.</param>
        /// <param name="value">The current value to interpolate.</param>
        /// <param name="endValue">The ending value for interpolation.</param>
        /// <param name="startThickness">The starting thickness value.</param>
        /// <param name="endThickness">The ending thickness value.</param>
        /// <param name="useEasing">Indicates whether to apply an easing function.</param>
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
                var easedT = EasingInOutFunction(t);

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
        /// Calculates an interpolated angle based on the provided parameters.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="startValue">The starting value for interpolation.</param>
        /// <param name="value">The current value to interpolate.</param>
        /// <param name="endValue">The ending value for interpolation.</param>
        /// <param name="startAngle">The starting angle value.</param>
        /// <param name="endAngle">The ending angle value.</param>
        /// <param name="valueAngle">The angle corresponding to the current value.</param>
        /// <param name="useEasing">Indicates whether to apply an easing function.</param>
        /// <returns>The interpolated angle value.</returns>
        private double DrawAdjustedAngle(DependencyObject d, double startValue, double value, double endValue, double startAngle, double endAngle, double valueAngle, bool useEasing)
        {
            // Ensure that value is within the range [startValue, endValue]
            value = Math.Max(startValue, Math.Min(endValue, value));

            // Calculate the interpolation factor (t) between 0 and 1
            var t = (value - startValue) / (endValue - startValue);

            double interpolatedAngle;

            if (useEasing)
            {
                // Apply an easing function
                var easedT = EasingInOutFunction(t);

                // Interpolate the angle
                interpolatedAngle = startAngle + easedT * (endAngle - startAngle);
            }
            else
            {
                // Interpolate the angle
                interpolatedAngle = startAngle + t * (endAngle - startAngle);
            }

            return interpolatedAngle;
        }



        /// <summary>
        /// Example quadratic ease-in-out function
        /// </summary>
        private double EasingInOutFunction(double t)
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
        /// <param name="thicknessA">The Thickness radius to measure.</param>
        /// <param name="radius">The radius of the rings.</param>
        /// <returns>The gap angle (sum of angles for the larger and smaller strokes).</returns>
        private double GapThicknessToAngle(double radius, double thickness)
        {
            if (radius > 0 && thickness > 0)
            {
                // Calculate the maximum number of circles
                double n = Math.PI * (radius / thickness);

                // Calculate the angle between each small circle
                double angle = 360.0 / n;

                return angle;
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
            return angle + (gapAngle * 2) < 180.0;
        }



        /// <summary>
        /// Calculates an adjusted angle using linear interpolation (lerp) between the start and end angles.
        /// </summary>
        /// <param name="startAngle">The initial angle.</param>
        /// <param name="endAngle">The final angle.</param>
        /// <param name="valueAngle">A value between 0 and 1 representing the interpolation factor.</param>
        /// <returns>The adjusted angle based on linear interpolation.</returns>
        private static double GetInterpolatedAngle(double startAngle, double endAngle, double valueAngle)
        {
            // Linear interpolation formula (lerp): GetInterpolatedAngle = (startAngle + valueAngle) * (endAngle - startAngle)
            return (startAngle + valueAngle) * (endAngle - startAngle);
        }

        #endregion
    }
}