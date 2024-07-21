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
                new PropertyMetadata(1, OnTrackThicknessChanged));

        /// <summary>
        /// Identifies the MainRingPadding dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingPaddingProperty =
            DependencyProperty.Register(
                nameof(MainRingPadding),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(0, OnRingPaddingChanged));

        /// <summary>
        /// Identifies the TrackRingPadding dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingPaddingProperty =
            DependencyProperty.Register(
                nameof(TrackRingPadding),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(1, OnTrackPaddingChanged));

        /// <summary>
        /// Identifies the Percent dependency property.
        /// </summary>
        protected static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register(
                nameof(Percent),
                typeof(string),
                typeof(PercentageRing),
                new PropertyMetadata(string.Empty));


        /// <summary>
        /// Identifies the AdjustedSize dependency property.
        /// </summary>
        protected static readonly DependencyProperty AdjustedSizeProperty =
            DependencyProperty.Register(
                nameof(AdjustedSize),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(16));

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
        /// Gets or sets the Padding for the Main ring.
        /// </summary>
        public double MainRingPadding
        {
            get { return (double)GetValue(MainRingPaddingProperty); }
            set { SetValue(MainRingPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Padding for the Track ring.
        /// </summary>
        public double TrackRingPadding
        {
            get { return (double)GetValue(TrackRingPaddingProperty); }
            set { SetValue(TrackRingPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current value converted to Percentage.
        /// </summary>
        protected double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Adjusted Size of the control container.
        /// </summary>
        protected double AdjustedSize
        {
            get { return (double)GetValue(AdjustedSizeProperty); }
            set { SetValue(AdjustedSizeProperty, value); }
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

        private static void OnRingPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Run the code to update Main ring thickness values.

            MainRingPaddingChanged(d, (double)e.NewValue);
        }

        private static void OnTrackPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Run the code to update Track ring thickness values.

            TrackRingPaddingChanged(d, (double)e.NewValue);
        }

        #endregion
    }
}