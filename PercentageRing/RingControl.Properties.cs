using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System.Diagnostics;
using Windows.Foundation;

namespace StorageControls
{
    public sealed partial class RingControl : Path
    {
        #region StartAngle

        /// <summary>
        /// The start angle property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(RingControl),
                new PropertyMetadata(
                    0d,
                    OnStartAngleChanged));

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>
        /// The start angle.
        /// </value>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        private static void OnStartAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingControl)sender;

            var oldStartAngle = (double)e.OldValue;
            var newStartAngle = (double)e.NewValue;

            target.OnStartAngleChanged(oldStartAngle, newStartAngle);
        }

        private void OnStartAngleChanged(double oldStartAngle, double newStartAngle)
        {
            Debug.Write("On Start Angle Changed ");

            UpdatePath();
        }

        #endregion



        #region EndAngle
        /// <summary>
        /// The end angle property.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(
                "EndAngle",
                typeof(double),
                typeof(RingControl),
                new PropertyMetadata(
                    0d,
                    OnEndAngleChanged));

        /// <summary>
        /// Gets or sets the end angle.
        /// </summary>
        /// <value>
        /// The end angle.
        /// </value>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        private static void OnEndAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingControl)sender;

            var oldEndAngle = (double)e.OldValue;
            var newEndAngle = (double)e.NewValue;

            target.OnEndAngleChanged(oldEndAngle, newEndAngle);
        }

        private void OnEndAngleChanged(double oldEndAngle, double newEndAngle)
        {
            Debug.Write("On End Angle Changed ");

            UpdatePath();
        }
        #endregion



        #region Direction
        /// <summary>
        /// The Direction property.
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                "Direction",
                typeof(SweepDirection),
                typeof(RingControl),
                new PropertyMetadata(
                    SweepDirection.Clockwise,
                    OnDirectionChanged));

        /// <summary>
        /// Gets or sets the Direction.
        /// </summary>
        /// <value>
        /// The SweepDirection.
        /// </value>
        public SweepDirection Direction
        {
            get { return (SweepDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        private static void OnDirectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingControl)sender;

            var oldDirection = (SweepDirection)e.OldValue;
            var newDirection = (SweepDirection)e.NewValue;

            target.OnDirectionChanged(oldDirection, newDirection);
        }

        private void OnDirectionChanged(SweepDirection oldDirection, SweepDirection newDirection)
        {
            Debug.Write("On Direction Changed ");

            UpdatePath();
        }
        #endregion



        #region Center
        /// <summary>
        /// Center Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(
                "Center",
                typeof(Point?),
                typeof(RingControl),
                new PropertyMetadata(null, OnCenterChanged));

        /// <summary>
        /// Gets or sets the Center property. This dependency property 
        /// indicates the center point.
        /// Center point is calculated based on Radius and StrokeThickness if not specified.    
        /// </summary>
        public Point? Center
        {
            get { return (Point?)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Center property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingControl)d;
            Point? oldCenter = (Point?)e.OldValue;
            Point? newCenter = target.Center;
            target.OnCenterChanged(oldCenter, newCenter);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Center property.
        /// </summary>
        /// <param name="oldCenter">The old Center value</param>
        /// <param name="newCenter">The new Center value</param>
        private void OnCenterChanged(
            Point? oldCenter, Point? newCenter)
        {
            Debug.Write("On Center Changed ");

            UpdatePath();
        }
        #endregion
    }
}
