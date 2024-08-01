using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Diagnostics;
using Windows.Foundation;

namespace StorageControls
{
    public sealed partial class RingControl : Path
    {
        private bool        _isUpdating;
        private double      _radius;


        #region Initialisation

        /// <summary>
        /// Initializes a new instance of the <see cref="RingControl" /> class.
        /// </summary>
        public RingControl()
        {
            this.SizeChanged += OnSizeChanged;

            RegisterPropertyChangedCallback( StrokeThicknessProperty, OnStrokeThicknessChanged);
        }

        #endregion



        #region Properties Changing

        /// <summary>
        /// Runs when the Size changes
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            Debug.Write( "On Size Changed " );

            UpdateRadius();

            UpdatePath();
        }



        /// <summary>
        /// Runs when the StrokeThickness Changes
        /// </summary>
        private void OnStrokeThicknessChanged(DependencyObject sender , DependencyProperty dp)
        {
            Debug.Write( "On Stroke Thickness Changed " );

            UpdateRadius();

            UpdatePath();
        }
        #endregion



        #region Update events

        /// <summary>
        /// Suspends path updates until EndUpdate is called
        /// </summary>
        public void BeginUpdate()
        {
            _isUpdating = true;
        }



        /// <summary>
        /// Resumes immediate path updates every time a component property value changes. Updates the path
        /// </summary>
        public void EndUpdate()
        {
            _isUpdating = false;
            UpdatePath();
        }



        /// <summary>
        /// Updates the calculated Radius
        /// </summary>
        private void UpdateRadius()
        {
            var smaller = Math.Min(this.Width, this.Height);

            _radius = ( smaller - this.StrokeThickness ) / 2;
        }


        /// <summary>
        /// Updates the path
        /// </summary>
        private void UpdatePath()
        {
           if (_isUpdating || this.ActualWidth == 0 || _radius <= 0 )
            {
                return;
            }

            if (EndAngle == StartAngle + 360)
            {
                var center =
                    this.Center ??
                    new Point(_radius + (this.StrokeThickness / 2), _radius + (this.StrokeThickness / 2));

                var eg = new EllipseGeometry
                {
                    Center = center,
                    RadiusX = _radius,
                    RadiusY = _radius,
                };

                this.Data = eg;
            }
            else
            {
                var pathGeometry = new PathGeometry();
                var pathFigure = new PathFigure();
                pathFigure.IsClosed = false;

                var center =
                    this.Center ??
                    new Point(_radius + (this.StrokeThickness / 2), _radius + (this.StrokeThickness / 2));

                // Starting Point
                pathFigure.StartPoint =
                    new Point(
                        center.X + Math.Sin(StartAngle * Math.PI / 180) * _radius,
                        center.Y - Math.Cos(StartAngle * Math.PI / 180) * _radius);

                // Arc
                var ArcSegment = new ArcSegment();

                if (Direction == SweepDirection.Counterclockwise)
                {
                    ArcSegment.Point =
                        new Point(
                            center.X + Math.Sin(EndAngle * Math.PI / 180) * _radius,
                            center.Y - Math.Cos(EndAngle * Math.PI / 180) * _radius);
                    ArcSegment.IsLargeArc = (EndAngle - StartAngle) <= 180.0;
                    ArcSegment.SweepDirection = SweepDirection.Counterclockwise;
                }
                else
                {
                    ArcSegment.Point =
                        new Point(
                            center.X + Math.Sin(EndAngle * Math.PI / 180) * _radius,
                            center.Y - Math.Cos(EndAngle * Math.PI / 180) * _radius);
                    ArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
                    ArcSegment.SweepDirection = SweepDirection.Clockwise;
                }
                ArcSegment.Size = new Size(_radius, _radius);

                pathFigure.Segments.Add(ArcSegment);
                pathGeometry.Figures.Add(pathFigure);
                this.InvalidateArrange();
                this.Data = pathGeometry;
            }
        }
        #endregion
    }
}
