using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using Windows.Foundation;

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
                if (percentageRing.Value == percentageRing.GetMaxValue())
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
                    #region Code for drawing arc based on value around the ring
                    var pg = new PathGeometry();
                    var pf = new PathFigure
                    {
                        IsClosed = false
                    };

                    // Sets the start point to the top and centre of the canvas
                    pf.StartPoint = percentageRing.GetPointAroundRadius(0, ringCentre);

                    var seg = new ArcSegment
                    {
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = valueAngle > 180.0,
                        Size = new Size(ringCentre, ringCentre),

                        // Sets the end point to an angle, calculated from the value, to a position around the radius of the ring
                        Point = percentageRing.GetPointAroundRadius(valueAngle, ringCentre)
                    };

                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);

                    mainRing.Data = pg;
                    mainRing.StrokeThickness = ringThickness;
                    #endregion
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
                if (percentageRing.Value == percentageRing.GetMinValue())
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
                    //
                    // Draw the Track ring as the inverse of the Main ring to start with
                    //
                    #region Code for drawing inverse arc based on value around the ring
                    var pg = new PathGeometry();
                    var pf = new PathFigure
                    {
                        IsClosed = false
                    };

                    // NEED TO ADJUST START POINT TO BE NOT AT 0
                    pf.StartPoint = percentageRing.GetPointAroundRadius(360-36, ringCentre);

                    var seg = new ArcSegment
                    {
                        SweepDirection = SweepDirection.Counterclockwise,
                        IsLargeArc = valueAngle < 180.0-(36*2),
                        Size = new Size(ringCentre, ringCentre),

                        // Sets the end point to an angle, calculated from the value, to a position around the radius of the ring
                        Point = percentageRing.GetPointAroundRadius(valueAngle+36, ringCentre)
                    };

                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);

                    trackRing.Data = pg;
                    trackRing.StrokeThickness = ringThickness;
                    #endregion
                }
            }
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
