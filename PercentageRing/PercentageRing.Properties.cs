// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

namespace StorageControls
{
    public partial class PercentageRing : RangeBase
    {
        #region Main and Track Ring Brushes (Brush)

        /// <summary>
        /// Identifies the MainRingBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingBrushProperty =
            DependencyProperty.Register(
                nameof(MainRingBrush),
                typeof(Brush),
                typeof(PercentageRing),
                new PropertyMetadata(null, OnBrushChanged));


        /// <summary>
        /// Gets or sets the Main ring brush.
        /// </summary>
        public Brush MainRingBrush
        {
            get { return (Brush)GetValue( MainRingBrushProperty ); }
            set { SetValue( MainRingBrushProperty , value ); }
        }

        ///

        /// <summary>
        /// Gets or sets the Track ring brush.
        /// </summary>
        public Brush TrackRingBrush
        {
            get { return (Brush)GetValue( TrackRingBrushProperty ); }
            set { SetValue( TrackRingBrushProperty , value ); }
        }


        /// <summary>
        /// Identifies the TrackRingBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingBrushProperty =
            DependencyProperty.Register(
                nameof(TrackRingBrush),
                typeof(Brush),
                typeof(PercentageRing),
                new PropertyMetadata(null, OnBrushChanged)); 


        /// <summary>
        /// Handles the change in Main and Track Ring Brush property.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnBrushChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != e.NewValue )
            {
                BrushChanged( d , (Brush)e.NewValue );
            }
        }

        #endregion



        #region Main and Track Ring Start and End Caps (PenLineCap)

        /// <summary>
        /// Identifies the MainRingStartCap dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingStartCapProperty =
            DependencyProperty.Register(
                nameof(MainRingStartCap),
                typeof(PenLineCap),
                typeof(PercentageRing),
                new PropertyMetadata(PenLineCap.Round, OnStrokeCapChanged));


        /// <summary>
        /// Gets or sets the StrokeStartCap for the Main ring.
        /// </summary>
        public PenLineCap MainRingStartCap
        {
            get { return (PenLineCap)GetValue( MainRingStartCapProperty ); }
            set { SetValue( MainRingStartCapProperty , value ); }
        }

        ///

        /// <summary>
        /// Identifies the MainRingEndCap dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingEndCapProperty =
            DependencyProperty.Register(
                nameof(MainRingEndCap),
                typeof(PenLineCap),
                typeof(PercentageRing),
                new PropertyMetadata(PenLineCap.Round, OnStrokeCapChanged));


        /// <summary>
        /// Gets or sets the StrokeEndCap for the Main ring.
        /// </summary>
        public PenLineCap MainRingEndCap
        {
            get { return (PenLineCap)GetValue( MainRingEndCapProperty ); }
            set { SetValue( MainRingEndCapProperty , value ); }
        }

        ///

        /// <summary>
        /// Identifies the TrackRingStartCap dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingStartCapProperty =
            DependencyProperty.Register(
                nameof(TrackRingStartCap),
                typeof(PenLineCap),
                typeof(PercentageRing),
                new PropertyMetadata(PenLineCap.Round, OnStrokeCapChanged));


        /// <summary>
        /// Gets or sets the StrokeStartCap for the Track ring.
        /// </summary>
        public PenLineCap TrackRingStartCap
        {
            get { return (PenLineCap)GetValue( TrackRingStartCapProperty ); }
            set { SetValue( TrackRingStartCapProperty , value ); }
        }

        ///

        /// <summary>
        /// Identifies the TrackRingEndCap dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingEndCapProperty =
            DependencyProperty.Register(
                nameof(TrackRingEndCap),
                typeof(PenLineCap),
                typeof(PercentageRing),
                new PropertyMetadata(PenLineCap.Round, OnStrokeCapChanged));


        /// <summary>
        /// Gets or sets the StrokeEndCap for the Track ring.
        /// </summary>
        public PenLineCap TrackRingEndCap
        {
            get { return (PenLineCap)GetValue( TrackRingEndCapProperty ); }
            set { SetValue( TrackRingEndCapProperty , value ); }
        }

        ///

        /// <summary>
        /// Handles the change in Main and Track ring StartCap and EndCap properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnStrokeCapChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != e.NewValue )
            {
                StrokeCapChanged( d , (PenLineCap)e.NewValue );
            }
        }

        #endregion



        #region Main and Track Ring Thickness (double)

        /// <summary>
        /// Identifies the MainRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingThicknessProperty =
            DependencyProperty.Register(
                nameof(MainRingThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(3.0, OnRingThicknessChanged));


        /// <summary>
        /// Gets or sets the thickness of the Main Ring.
        /// </summary>
        public double MainRingThickness
        {
            get { return (double)GetValue( MainRingThicknessProperty ); }
            set { SetValue( MainRingThicknessProperty , value ); }
        }

        ///

        /// <summary>
        /// Identifies the TrackRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingThicknessProperty =
            DependencyProperty.Register(
                nameof(TrackRingThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(1.0, OnRingThicknessChanged));


        /// <summary>
        /// Gets or sets the thickness of the Track ring.
        /// </summary>
        public double TrackRingThickness
        {
            get { return (double)GetValue( TrackRingThicknessProperty ); }
            set { SetValue( TrackRingThicknessProperty , value ); }
        }

        ///

        /// <summary>
        /// Handles the change in Main and Track Ring Thickness properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnRingThicknessChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != e.NewValue )
            {
                RingThicknessChanged( d );
            }
        }

        #endregion



        #region PercentWarning (double)

        /// <summary>
        /// Identifies the MainRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty PercentWarningProperty =
            DependencyProperty.Register(
                nameof(PercentWarning),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(75.1, OnPercentWarningChanged));


        /// <summary>
        /// Gets or sets the thickness of the Main Ring.
        /// </summary>
        public double PercentWarning
        {
            get { return (double)GetValue( PercentWarningProperty ); }
            set { SetValue( PercentWarningProperty , value ); }
        }


        /// <summary>
        /// Handles the change in Main and Track Ring Thickness properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnPercentWarningChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != e.NewValue )
            {
                PercentWarningChanged( d , (double)e.NewValue );
            }
        }

        #endregion



        #region ValueAngle Protected (double)

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
        /// Gets or sets the current angle of the Ring (between MinAngle and MaxAngle).
        /// </summary>
        protected double ValueAngle
        {
            get { return (double)GetValue( ValueAngleProperty ); }
            set { SetValue( ValueAngleProperty , value ); }
        }

        #endregion



        #region AdjustedSize Protected (double)

        /// <summary>
        /// Identifies the AdjustedSize dependency property.
        /// </summary>
        protected static readonly DependencyProperty AdjustedSizeProperty =
            DependencyProperty.Register(
                nameof(AdjustedSize),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(16.0));


        /// <summary>
        /// Gets or sets the AdjustedSize of the control.
        /// </summary>
        protected double AdjustedSize
        {
            get { return (double)GetValue( AdjustedSizeProperty ); }
            set { SetValue( AdjustedSizeProperty , value ); }
        }

        #endregion



        #region Percent Protected (string)

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
        /// Gets or sets the current value converted to Percentage as a string.
        /// </summary>
        protected string Percent
        {
            get { return (string)GetValue( PercentProperty ); }
            set { SetValue( PercentProperty , value ); }
        }

        #endregion



        #region Inherited Property Change Events

        /// <inheritdoc/>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            SetOldValue(oldValue);
            SetOldValueAngle( DoubleToAngle( oldValue , Minimum , Maximum , MinAngle , MaxAngle ) );

            base.OnValueChanged( oldValue , newValue );

            OnValueChanged( this );
        }

        #endregion
    }
}