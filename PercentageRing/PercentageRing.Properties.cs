using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

namespace StorageControls
{
    public partial class PercentageRing : RangeBase
    {
        #region DEPENDENCY PROPERTY REGISTRATION

        // Brushes

        /// <summary>
        /// Identifies the TrackRingBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingBrushProperty =
            DependencyProperty.Register(
                nameof(TrackRingBrush), 
                typeof(Brush), 
                typeof(PercentageRing), 
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the MainRingBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingBrushProperty =
            DependencyProperty.Register(
                nameof(MainRingBrush), 
                typeof(Brush), 
                typeof(PercentageRing), 
                new PropertyMetadata(null));

        // Doubles

        /// <summary>
        /// Identifies the MainRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingThicknessProperty =
            DependencyProperty.Register(
                nameof(MainRingThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(3, OnRingThicknessChanged));

        /// <summary>
        /// Identifies the TrackRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingThicknessProperty =
            DependencyProperty.Register(
                nameof(TrackRingThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(3, OnTrackThicknessChanged));

        /// <summary>
        /// Identifies the ValueAngle dependency property.
        /// </summary>
        protected static readonly DependencyProperty ValueAngleProperty =
            DependencyProperty.Register(
                nameof(ValueAngle), 
                typeof(double), 
                typeof(PercentageRing), 
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the Percent dependency property.
        /// </summary>
        protected static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register(
                nameof(Percent),
                typeof(string),
                typeof(PercentageRing),
                new PropertyMetadata(string.Empty));

        #endregion


        #region PUBLIC PROPERTIES

        // Brushes

        /// <summary>
        /// Gets or sets the Track ring brush.
        /// </summary>
        public Brush TrackRingBrush
        {
            get { return (Brush)GetValue(TrackRingBrushProperty); }
            set { SetValue(TrackRingBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Main ring brush.
        /// </summary>
        public Brush MainRingBrush
        {
            get { return (Brush)GetValue(MainRingBrushProperty); }
            set { SetValue(MainRingBrushProperty, value); }
        }

        // Doubles

        /// <summary>
        /// Gets or sets the thickness of the Main ring.
        /// </summary>
        public double MainRingThickness
        {
            get { return (double)GetValue(MainRingThicknessProperty); }
            set { SetValue(MainRingThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the Track ring.
        /// </summary>
        public double TrackRingThickness
        {
            get { return (double)GetValue(TrackRingThicknessProperty); }
            set { SetValue(TrackRingThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current angle of the Ring (between MinAngle and MaxAngle). Setting the angle will update the Value.
        /// </summary>
        protected double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current value converted to Percentage.
        /// </summary>
        protected double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        #endregion


        #region PROPERTY CHANGED EVENTS

        private static void OnRingThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Run the code to update Main ring thickness values.

            MainRingThicknessChanged(d, (double)e.NewValue);
        }

        private static void OnTrackThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Run the code to update Track ring thickness values.

            TrackRingThicknessChanged(d, (double)e.NewValue);
        }

        #endregion
    }
}