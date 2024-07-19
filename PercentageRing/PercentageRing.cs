using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using Windows.Foundation;
using Windows.Security.Cryptography.Core;

namespace StorageControls
{
    /// <summary>
    /// A control which takes a value and converts to a percentage displayed on a circular ring
    /// </summary>
    //// All calculations are for a 160 x 160 square. The ViewBox control will handle scaling.
    public partial class PercentageRing : RangeBase
    {
        #region Private variables

        // Constant values
        private const double DegToRad = Math.PI / 180;
        private const double Radius = 80;
        private const double MinAngle = 0;
        private const double MaxAngle = 360;
        private const double TrackRingSpacingDeg = 9;
        private const double MainRingSpacingDeg = 36;

        // Updatable values
        private double _mainRingThickness;
        private double _trackRingThickness;

        private double _minValue; 
        private double _maxValue;
        private double _value;

        private double _normalizedMinAngle;
        private double _normalizedMaxAngle;

        #endregion


        public PercentageRing()
        {
            this.DefaultStyleKey = typeof(PercentageRing);
        }


        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateAllRings(this);
        }


        /// <summary>
        /// Runs when the Main ring Thickness property changes
        /// </summary>
        private static void MainRingThicknessChanged(DependencyObject d, double newValue)
        {
            var percentageRing = (PercentageRing)d;

            percentageRing.UpdateMainRingThickness(newValue);

            UpdateAllRings(d);
        }


        /// <summary>
        /// Runs when the Track ring Thickness property changes
        /// </summary>
        private static void TrackRingThicknessChanged(DependencyObject d, double newValue)
        {
            var percentageRing = (PercentageRing)d;

            percentageRing.UpdateTrackRingThickness(newValue);

            UpdateAllRings(d);
        }


        /// <summary>
        /// Updates the private Main ring thickness variable
        /// </summary>
        private void UpdateMainRingThickness(double value)
        {
            _mainRingThickness = value;
        }


        /// <summary>
        /// Updates the private Track ring thickness variable
        /// </summary>
        private void UpdateTrackRingThickness(double value)
        {
            _trackRingThickness = value;
        }


        /// <summary>
        /// Update all rings
        /// </summary>
        private static void UpdateAllRings(DependencyObject d)
        {
            UpdateMainRing(d);

            UpdateTrackRing(d);
        }


        /// <summary>
        /// Update the Main ring
        /// </summary>
        private static void UpdateMainRing(DependencyObject d)
        {
            var percentageRing = (PercentageRing)d;

            var mainRing = percentageRing.GetTemplateChild(ScalePartName) as Path;

            var ringThickness = percentageRing.GetMainRingThickness();
            var valueAngle = percentageRing.ValueToAngle(percentageRing.Value, MinAngle, MaxAngle);
            var ringCentre = percentageRing.GetMainRingRadius(Radius, percentageRing.GetMainRingThickness());

            if (mainRing != null)
            {
                // Between the min value and min value + 1
                if (percentageRing.Value > percentageRing.GetMinValue() && percentageRing.Value < percentageRing.GetMinValue() + 1)
                {
                    percentageRing.DrawArc(d, ringCentre, valueAngle, percentageRing.GetMainArcLargeAngleCheck(valueAngle, 0), (MinAngle), (MinAngle + 0.01), mainRing, ringThickness, SweepDirection.Clockwise);
                    mainRing.StrokeThickness = percentageRing.GetMainRingThickness() * percentageRing.Value;
                }
                else if (percentageRing.Value == percentageRing.GetMaxValue())
                {
                    // At 100% we draw a full circle for the ring
                    var eg = new EllipseGeometry
                    {
                        Center = new Point(Radius, Radius),
                        RadiusX = percentageRing.GetTrackRingRadius(Radius, percentageRing.GetTrackRingThickness())
                    };

                    eg.RadiusY = eg.RadiusX;

                    mainRing.Data = eg;
                    mainRing.StrokeThickness = ringThickness;
                }
                else
                {
                    percentageRing.DrawArc(d, ringCentre, valueAngle, percentageRing.GetMainArcLargeAngleCheck(valueAngle, 0), 0, valueAngle, mainRing, ringThickness, SweepDirection.Clockwise);
                    mainRing.StrokeThickness = ringThickness;
                }
            }
        }


        /// <summary>
        /// Update the Track ring
        /// </summary>
        private static void UpdateTrackRing(DependencyObject d)
        {
            var percentageRing = (PercentageRing)d;

            var trackRing = percentageRing.GetTemplateChild(TrailPartName) as Path;

            var ringThickness = percentageRing.GetTrackRingThickness();
            var valueAngle = percentageRing.ValueToAngle(percentageRing.Value, 0, 360);
            var ringCentre = percentageRing.GetTrackRingRadius(Radius, percentageRing.GetTrackRingThickness());

            if (trackRing != null)
            {                
                if (percentageRing.Value > 0 && percentageRing.Value < 1)
                {
                    //
                    // Start Angle is 360 - 36 for the spacing.
                    // we want the start to be 360 at value = 0
                    // then when value = 1, to be at 360-36 which is 324
                    //
                    // so a value between 324 and 360 between value 0 and 1
                    //
                    double beginStartAngle = MaxAngle; // Initial start angle
                    double beginEndAngle = MaxAngle - (MainRingSpacingDeg / 4); // Angle when value = 1

                    double endStartAngle = MinAngle; // Initial start angle
                    double endEndAngle = MinAngle + (MainRingSpacingDeg / 4); // Angle when value = 1


                    double adjustStart = (MaxAngle - MainRingSpacingDeg) / valueAngle;

                    percentageRing.DrawArc(d, ringCentre, valueAngle, percentageRing.GetTrackArcLargeAngleCheck(valueAngle, 0), GetAdjustedAngle(beginStartAngle, beginEndAngle, valueAngle), GetAdjustedAngle(endStartAngle, endEndAngle, valueAngle), trackRing, ringThickness, SweepDirection.Counterclockwise);

                    trackRing.StrokeThickness = ringThickness;
                    trackRing.Visibility = Visibility.Visible;
                }
                else if (percentageRing.Value == percentageRing.GetMinValue())
                {
                    // At 0% we draw a full circle as our ring
                    var eg = new EllipseGeometry
                    {
                        Center = new Point(Radius, Radius),
                        RadiusX = percentageRing.GetTrackRingRadius(Radius, percentageRing.GetTrackRingThickness())
                    };

                    eg.RadiusY = eg.RadiusX;

                    trackRing.Data = eg;
                    trackRing.StrokeThickness = percentageRing.GetTrackRingThickness();
                }                      
                else
                {
                    if (valueAngle > (MaxAngle - (MainRingSpacingDeg * 2) - 1) && valueAngle < (MaxAngle - (MainRingSpacingDeg * 2) + MainRingSpacingDeg))
                    {
                        // We fix the end and start points as the track reaches its end
                        percentageRing.DrawArc(d, ringCentre, (MaxAngle - MainRingSpacingDeg), percentageRing.GetTrackArcLargeAngleCheck(valueAngle, MainRingSpacingDeg), (MaxAngle - MainRingSpacingDeg), (MaxAngle - (MainRingSpacingDeg + 1)), trackRing, ringThickness, SweepDirection.Counterclockwise);

                        #region Code to get the thickness value as the angle changes

                        // Calculate the start angle for thinning (subtracting 4 from MainRingSpacingDeg)
                        double startThinningAngle = MaxAngle - (MainRingSpacingDeg - 4);

                        // Calculate the end angle for thinning (based on TrackRingSpacingDeg)
                        double endThinningAngle = MaxAngle - TrackRingSpacingDeg;

                        // Calculate the current thinning angle (adding MainRingSpacingDeg and half of TrackRingSpacingDeg)
                        double currentThinningAngle = valueAngle + MainRingSpacingDeg + (TrackRingSpacingDeg / 2);

                        // Get the initial thickness from the percentage ring
                        double startThickness = percentageRing.GetTrackRingThickness();

                        // Set the end thickness to 0 (assuming thinning means reducing thickness)
                        double endThickness = 0;

                        // Initialize the variable for the current thickness value
                        double currentThicknessValue;

                        // Ensure currentThinningAngle is within the valid range
                        currentThinningAngle = Math.Max(startThinningAngle, Math.Min(endThinningAngle, currentThinningAngle));

                        // Calculate the interpolation factor (t) based on the angle range
                        double t = (currentThinningAngle - startThinningAngle) / (endThinningAngle - startThinningAngle);

                        // Linearly interpolate between startThickness and endThickness
                        currentThicknessValue = startThickness * (1 - t) + endThickness * t;

                        #endregion

                        trackRing.StrokeThickness = currentThicknessValue;
                    }
                    else if (valueAngle > (MaxAngle - (MainRingSpacingDeg * 2) + (TrackRingSpacingDeg * 2) - 1))
                    {
                        // Hide Track ring when it reaches the end of the available track
                        trackRing.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        trackRing.Visibility = Visibility.Visible;

                        percentageRing.DrawArc(d, ringCentre, valueAngle, percentageRing.GetTrackArcLargeAngleCheck(valueAngle, MainRingSpacingDeg), (MaxAngle - MainRingSpacingDeg), (valueAngle + MainRingSpacingDeg), trackRing, ringThickness, SweepDirection.Counterclockwise);
                        trackRing.StrokeThickness = ringThickness;
                    }
                }
            }
        }


        /// <summary>
        /// Draws an arc segment representing a percentage value of the ring.
        /// </summary>
        /// <param name="d">The DependencyObject representing the PercentageRing.</param>
        /// <param name="ringCentre">The center point of the ring.</param>
        /// <param name="valueAngle">The angle corresponding to the percentage value (in degrees).</param>
        /// <param name="largeArcAngle">A boolean indicating whether the arc is a large arc.</param>
        /// <param name="startAngle">The starting angle of the arc (in degrees).</param>
        /// <param name="endAngle">The ending angle of the arc (in degrees).</param>
        /// <param name="ringPath">The Path control used to display the arc.</param>
        /// <param name="ringThickness">The stroke thickness of the arc.</param>
        /// <param name="sweep">The direction in which the arc sweeps (Clockwise or Counterclockwise).</param>
        private void DrawArc(DependencyObject d, double ringCentre, double valueAngle, bool largeArcAngle, double startAngle, double endAngle, Path ringPath, double ringThickness, SweepDirection sweep)
        {
            var percentageRing = (PercentageRing)d;

            var pg = new PathGeometry();
            var pf = new PathFigure
            {
                IsClosed = false
            };

            // Sets the start point to the top and centre of the canvas
            pf.StartPoint = percentageRing.GetPointAroundRadius(startAngle, ringCentre);

            var seg = new ArcSegment
            {
                SweepDirection = sweep,
                IsLargeArc = largeArcAngle,
                Size = new Size(ringCentre, ringCentre),

                // Sets the end point to an angle, calculated from the value, to a position around the radius of the ring
                Point = percentageRing.GetPointAroundRadius(endAngle, ringCentre)
            };

            pf.Segments.Add(seg);
            pg.Figures.Add(pf);

            ringPath.Data = pg;
        }


        static double GetAdjustedAngle(double startAngle, double endAngle, double valueAngle)
        {
            // Linear interpolation formula (lerp): GetAdjustedAngle = startAngle + valueAngle * (endAngle - startAngle)
            return startAngle + valueAngle * (endAngle - startAngle);
        }


        private bool GetMainArcLargeAngleCheck(double angle, double spacing)
        {
            return angle > 180.0;
        }


        private bool GetTrackArcLargeAngleCheck(double angle, double spacing)
        {
            return angle < 180.0 - (spacing * 2);
        }


        /// <summary>
        /// Returns the point around the radius of the centre point of the rings
        /// </summary>
        /// /// <param name="angle">Angle in degrees.</param>
        /// <param name="centrePoint">Distance from the center to the scaled point.</param>
        /// <returns>The scaled point coordinates.</returns>
        private Point GetPointAroundRadius(double angle, double centrePoint)
        {
            double AngInRad = (DegToRad * angle);

            double x = (Radius + (Math.Sin(AngInRad) * centrePoint));
            double y = (Radius - (Math.Cos(AngInRad) * centrePoint));

            return new Point(x, y);
        }


        /// <summary>
        /// Returns an adjusted Radius calculated with the thickness of the trail path
        /// </summary>
        private double GetTrackRingRadius(double radius, double trailThickness)
        {
            return (radius - (trailThickness + (trailThickness / 2)));
        }


        /// <summary>
        /// Returns an adjusted Radius calculated with the thickness of the scale path
        /// </summary>
        private double GetMainRingRadius(double radius, double trailThickness)
        {
            return (radius - (trailThickness / 2));
        }

       
        /// <summary>
        /// Converts a Value to Angle between minAngle - maxAngle
        /// </summary>
        private double ValueToAngle(double value, double minAngle, double maxAngle)
        {
            // If value is below the Minimum set
            if (value < _minValue)
            {
                return minAngle;
            }

            // If value is above the Maximum set
            if (value > _maxValue)
            {
                return maxAngle;
            }

            return ((value - _minValue) / (_maxValue - _minValue) * (maxAngle - minAngle)) + 0;
        }


        /// <summary>
        /// Returns the modulus (remainder) of a number divided by a divider.
        /// </summary>
        private double Mod(double number, double divider)
        {
            // This function ensures that the result is
            // always positive or zero, regardless of the input values.

            var result = number % divider;
            result = result < 0 ? result + divider : result;
            return result;
        }


        /// <summary>
        /// Gets the normalized minimum angle.
        /// </summary>
        /// <value>The minimum angle in the range from -180 to 180.</value>
        protected double NormalizedMinAngle => _normalizedMinAngle;


        /// <summary>
        /// Gets the normalized maximum angle.
        /// </summary>
        /// <value>The maximum angle, in the range from -180 to 540.</value>
        protected double NormalizedMaxAngle => _normalizedMaxAngle;


        /// <summary>
        /// Gets the Main ring's Thickness
        /// </summary>
        private double GetMainRingThickness()
        {
            return _mainRingThickness;
        }


        /// <summary>
        /// Gets the Track ring's Thickness
        /// </summary>
        private double GetTrackRingThickness()
        {
            return _trackRingThickness;
        }


        /// <summary>
        /// Gets the controls Maximum value
        /// </summary>
        private double GetMaxValue()
        {
            return _maxValue;
        }


        /// <summary>
        /// Gets the controls Minimum value
        /// </summary>
        private double GetMinValue()
        {
            return _minValue;
        }


        /// <summary>
        /// Updates normalised angles
        /// </summary>
        private void UpdateNormalizedAngles()
        {
            var result = Mod(MinAngle, 360);

            if (result >= 180)
            {
                result = result - 360;
            }

            _normalizedMinAngle = result;

            result = Mod(MaxAngle, 360);

            if (result < 180)
            {
                result = result + 360;
            }
            // Clamps max to 360
            if (result > 0 + 360)
            {
                result = result - 360;
            }

            _normalizedMaxAngle = result;
        }


        #region RANGEBASE EVENTS

        /// <inheritdoc/>
        protected override void OnValueChanged(double oldValue, double newValue)
        {            
            base.OnValueChanged(oldValue, newValue);

            if (oldValue != newValue)
            {
                _value = newValue;
                UpdateAllRings(this);
            }
        }

        /// <inheritdoc/>
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);

            if (oldMinimum != newMinimum)
            {
                _minValue = newMinimum;
                UpdateAllRings(this);
            }
        }

        /// <inheritdoc/>
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);

            if (oldMaximum != newMaximum)
            {
                _maxValue = newMaximum;
                UpdateAllRings(this);
            }
        }

        #endregion
    }
}
