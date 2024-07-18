using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using static System.Formats.Asn1.AsnWriter;

namespace StorageControls
{
    /// <summary>
    /// A Percentage Ring Control.
    /// </summary>
    //// All calculations are for a 160 x 160 square. The ViewBox control will handle scaling.
    public partial class PercentageRing : RangeBase
    {
        // Constant values
        private const double Degrees2Radians = Math.PI / 180;
        private const double Radius = 80;
        private const int iRadius = 80;
        private const double MinAngle = 0;
        private const double MaxAngle = 360;
        private const double ScaleAngle99 = 360 - ScaleAngle01;
        private const double ScaleAngle98 = ScaleAngle99 - TrailAngle01;
        private const double ScaleAngle01 = 36;
        private const double TrailAngle01 = 9;
        private double _normalizedMinAngle;
        private double _normalizedMaxAngle;

        // Updating values
        double _scaleThickness;
        double _trailThickness;
        double _halfScaleThickness;
        double _halfTrailThickness;        

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

            UpdateRings(this);
        }

        private static void ScaleThicknessChanged(DependencyObject d, double newValue)
        {
            // Handles changes in ScaleThickness property values

            //var percentageRing = (PercentageRing)d;

            //percentageRing.UpdateScaleThickness(newValue);

            UpdateRings(d);
        }

        private static void TrailThicknessChanged(DependencyObject d, double newValue)
        {
            // Handles changes in TrailThickness property values
            //var percentageRing = (PercentageRing)d;

            //percentageRing.UpdateTrailThickness(newValue);

            UpdateRings(d);
        }

        private void UpdateScaleThickness(double value)
        {
            _scaleThickness = value;
            _halfScaleThickness = value / 2;
        }

        private void UpdateTrailThickness(double value)
        {
            _trailThickness = value;
            _halfTrailThickness = value / 2;
        }


        /// <summary>
        /// Update the scale and trail rings
        /// </summary>
        private static void UpdateRings(DependencyObject d)
        { 
            var percentageRing = (PercentageRing)d;

            var trailRing = percentageRing.GetTemplateChild(TrailPartName) as Path;
            var scaleRing = percentageRing.GetTemplateChild(ScalePartName) as Path;

            var trailDot = percentageRing.GetTemplateChild(TrailDotPartName) as Ellipse;
            var scaleDot = percentageRing.GetTemplateChild(ScaleDotPartName) as Ellipse;

            // Update Trail, if it is not null
            if (trailRing != null)
            {
                // if value is 0, the trail should be an ellipse
                // if value is > 0 but < 1, the trail should have a gap
                // if value is 99 but < 100, the trail should be a dot
                // if value is 100, the trail should be gone
                // then we draw the trail ring

                if (percentageRing.Value == percentageRing.Minimum)
                {
                    // Draw full circle.
                    var eg = new EllipseGeometry
                    {
                        Center = new Point(Radius, Radius),
                        RadiusX = percentageRing.GetTrailRadius(Radius, percentageRing.TrailThickness)
                    };

                    eg.RadiusY = eg.RadiusX;
                    trailRing.Data = eg;
                    trailRing.StrokeThickness = percentageRing.TrailThickness;
                }
                // TODO Calculate the next value up from minimum, small and large change property maybe
                else if (percentageRing.Value > percentageRing.Minimum && percentageRing.Value < 1)
                {
                    // Draw trail with gap
                    // Start Angle 27 \| CCW <-
                    // End Angle 153 |/

                    // Draw arc segment
                    var pg = new PathGeometry();
                    var pf = new PathFigure
                    {
                        IsClosed = false
                    };
                    var middleOfScale = percentageRing.GetScaleRadius(Radius, percentageRing.ScaleThickness);

                    pf.StartPoint = percentageRing.ScaleRingPoint(percentageRing.ValueToAngle(percentageRing.Value, 0, 360), middleOfScale);

                    var seg = new ArcSegment
                    {
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = percentageRing.ValueToAngle(percentageRing.Value, 0, 360) > 180,
                        Size = new Size(middleOfScale, middleOfScale),
                        Point = percentageRing.ScaleRingPoint(0, middleOfScale)
                    };

                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);

                    scaleRing.Data = pg;
                    scaleRing.StrokeThickness = percentageRing.ScaleThickness;
                }
                // TODO Calculate the next value down from maximum, small and large change property maybe
                else if (percentageRing.Value > 98 && percentageRing.Value < percentageRing.Maximum)
                {
                    // Draw trail as dot
                    // Angle 27 \| CCW <-
                }
                else if (percentageRing.Value == percentageRing.Maximum)
                {
                    // Hide trail ring
                    trailRing.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Draw trail with space between scale path arc

                    trailRing.Visibility = Visibility.Visible;
                }
            }

            // Update Scale, if it is not null
            if (scaleRing != null)
            {
                // if value is 0, the scale should be gone
                // if value is > 0 but < 1, the scale should be a dot
                // if value is 99 but < 100, the scale should have a gap
                // if value is 100, the scale should be an ellipse
                // then we draw the Scale ring

                if (percentageRing.Value == percentageRing.Minimum)
                {
                    // Hide scale ring
                    scaleRing.Visibility = Visibility.Collapsed;
                    scaleDot.Visibility = Visibility.Collapsed;
                }
                // TODO Calculate the next value up from minimum, small and large change property maybe
                else if (percentageRing.Value > percentageRing.Minimum && percentageRing.Value < 1)
                {
                    scaleRing.Visibility = Visibility.Collapsed;
                    scaleDot.Visibility = Visibility.Visible;

                    var trailDotTransform = percentageRing.GetTemplateChild(ScaleDotTransformPartName) as ScaleTransform;
                    trailDotTransform.ScaleX = percentageRing.Value;
                    trailDotTransform.ScaleY = percentageRing.Value;

                }
                // TODO Calculate the next value down from maximum, small and large change property maybe
                else if (percentageRing.Value == 98 && percentageRing.Value < percentageRing.Maximum)
                {
                    // Draw arc segment
                    var pg = new PathGeometry();
                    var pf = new PathFigure
                    {
                        IsClosed = false
                    };
                    var middleOfScale = percentageRing.GetScaleRadius(Radius, percentageRing.ScaleThickness);

                    pf.StartPoint = percentageRing.ScaleRingPoint(ScaleAngle98, middleOfScale);

                    var seg = new ArcSegment
                    {
                        SweepDirection = SweepDirection.Counterclockwise,
                        IsLargeArc = percentageRing.ValueToAngle(percentageRing.Value, 0, 360) > 180,
                        Size = new Size(middleOfScale, middleOfScale),
                        Point = percentageRing.ScaleRingPoint(0, middleOfScale)
                    };

                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);

                    scaleRing.Data = pg;
                    scaleRing.StrokeThickness = percentageRing.ScaleThickness;

                    scaleRing.Visibility = Visibility.Visible;
                    scaleDot.Visibility = Visibility.Collapsed;
                }
                else if (percentageRing.Value == percentageRing.Maximum)
                {
                    // Draw full circle.
                    var eg = new EllipseGeometry
                    {
                        Center = new Point(Radius, Radius),
                        RadiusX = percentageRing.GetScaleRadius(Radius, percentageRing.ScaleThickness)
                    };

                    eg.RadiusY = eg.RadiusX;

                    scaleRing.Data = eg;
                    scaleRing.StrokeThickness = percentageRing.ScaleThickness;
                }
                else
                {
                    // Draw arc segment
                    var pg = new PathGeometry();
                    var pf = new PathFigure
                    {
                        IsClosed = false
                    };
                    var middleOfScale = percentageRing.GetScaleRadius(Radius, percentageRing.ScaleThickness);

                    pf.StartPoint = percentageRing.ScaleRingPoint(percentageRing.ValueToAngle(percentageRing.Value, 0, ScaleAngle98), middleOfScale);

                    var seg = new ArcSegment
                    {
                        SweepDirection = SweepDirection.Counterclockwise,
                        IsLargeArc = percentageRing.ValueToAngle(percentageRing.Value, 0, 360) > 180,
                        Size = new Size(middleOfScale, middleOfScale),
                        Point = percentageRing.ScaleRingPoint(0, middleOfScale)
                    };

                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);

                    scaleRing.Data = pg;
                    scaleRing.StrokeThickness = percentageRing.ScaleThickness;

                    scaleRing.Visibility = Visibility.Visible;
                    scaleDot.Visibility = Visibility.Collapsed;
                }
            }


        }

        /// <summary>
        /// Returns the point around the radius of the centre point of the rings
        /// </summary>
        private Point ScaleRingPoint(double angle, double centrePoint)
        {
            double x = (Radius + (Math.Sin(Degrees2Radians * angle) * centrePoint));
            double y = (Radius - (Math.Cos(Degrees2Radians * angle) * centrePoint));

            return new Point(x, y);
        }


        /// <summary>
        /// Returns an adjusted Radius calculated with the thickness of the trail path
        /// </summary>
        private double GetTrailRadius(double radius, double trailThickness)
        {
            return (radius - (trailThickness + (trailThickness / 2)));
        }



        /// <summary>
        /// Returns an adjusted Radius calculated with the thickness of the scale path
        /// </summary>
        private double GetScaleRadius(double radius, double trailThickness)
        {
            return (radius - (trailThickness / 2));
        }




        /// <summary>
        /// Converts a Value to Angle between minAngle - maxAngle
        /// </summary>
        private double ValueToAngle(double value, double minAngle, double maxAngle)
        {
            // Off-scale on the left.
            if (value < Minimum)
            {
                return minAngle;
            }

            // Off-scale on the right.
            if (value > Maximum)
            {
                return maxAngle;
            }

            return ((value - Minimum) / (Maximum - Minimum) * (360 - 0)) + 0;
        }



        private double Mod(double number, double divider)
        {
            var result = number % divider;
            result = result < 0 ? result + divider : result;
            return result;
        }







        /// <summary>
        /// Updates Normalised Angles
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
                UpdateRings(this);
            }
        }

        /// <inheritdoc/>
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            //base.OnMinimumChanged(oldMinimum, newMinimum);
        }

        /// <inheritdoc/>
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            //base.OnMaximumChanged(oldMaximum, newMaximum);
        }

        #endregion
    }
}
