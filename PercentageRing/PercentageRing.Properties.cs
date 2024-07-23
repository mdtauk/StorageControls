using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.DataProvider;

namespace StorageControls
{
    public partial class PercentageRing : Control
    {
        #region Dependency Property Registration

        //
        // Brushes

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
        /// Identifies the TrackRingBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingBrushProperty =
            DependencyProperty.Register(
                nameof(TrackRingBrush),
                typeof(Brush),
                typeof(PercentageRing),
                new PropertyMetadata(null, OnBrushChanged));

        //
        // End Caps

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
        /// Identifies the MainRingEndCap dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingEndCapProperty =
            DependencyProperty.Register(
                nameof(MainRingEndCap),
                typeof(PenLineCap),
                typeof(PercentageRing),
                new PropertyMetadata(PenLineCap.Round, OnStrokeCapChanged));

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
        /// Identifies the TrackRingEndCap dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingEndCapProperty =
            DependencyProperty.Register(
                nameof(TrackRingEndCap), 
                typeof(PenLineCap), 
                typeof(PercentageRing), 
                new PropertyMetadata(PenLineCap.Round, OnStrokeCapChanged));

        //
        // Doubles

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
        /// Identifies the TrackRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingThicknessProperty =
            DependencyProperty.Register(
                nameof(TrackRingThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(1.0, OnRingThicknessChanged));

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), 
                typeof(double), 
                typeof(PercentageRing), 
                new PropertyMetadata(12.0, OnValueChanged));

        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(0.0, OnMinimumChanged));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(100.0, OnMaximumChanged));

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
        /// Identifies the AdjustedSize dependency property.
        /// </summary>
        protected static readonly DependencyProperty AdjustedSizeProperty =
            DependencyProperty.Register(
                nameof(AdjustedSize),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(16.0));

        //
        // Strings

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



        #region Public Properties

        //
        // Brushes

        /// <summary>
        /// Gets or sets the Main ring brush.
        /// </summary>
        public Brush MainRingBrush
        {
            get { return (Brush)GetValue(MainRingBrushProperty); }
            set { SetValue(MainRingBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Track ring brush.
        /// </summary>
        public Brush TrackRingBrush
        {
            get { return (Brush)GetValue(TrackRingBrushProperty); }
            set { SetValue(TrackRingBrushProperty, value); }
        }

        //
        // End Caps

        /// <summary>
        /// Gets or sets the StrokeStartCap for the Main ring.
        /// </summary>
        public PenLineCap MainRingStartCap
        {
            get { return (PenLineCap)GetValue(MainRingStartCapProperty); }
            set { SetValue(MainRingStartCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeEndCap for the Main ring.
        /// </summary>
        public PenLineCap MainRingEndCap
        {
            get { return (PenLineCap)GetValue(MainRingEndCapProperty); }
            set { SetValue(MainRingEndCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeStartCap for the Track ring.
        /// </summary>
        public PenLineCap TrackRingStartCap
        {
            get { return (PenLineCap)GetValue(TrackRingStartCapProperty); }
            set { SetValue(TrackRingStartCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeEndCap for the Track ring.
        /// </summary>
        public PenLineCap TrackRingEndCap
        {
            get { return (PenLineCap)GetValue(TrackRingEndCapProperty); }
            set { SetValue(TrackRingEndCapProperty, value); }
        }

        //
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
        /// Gets or sets the Value.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Minimum.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Maximum.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        #endregion



        #region Protected Properties        

        /// <summary>
        /// Gets or sets the current angle of the Ring (between MinAngle and MaxAngle).
        /// </summary>
        protected double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the AdjustedSize of the control.
        /// </summary>
        protected double AdjustedSize
        {
            get { return (double)GetValue(AdjustedSizeProperty); }
            set { SetValue(AdjustedSizeProperty, value); }
        }

        //
        // Strings

        /// <summary>
        /// Gets or sets the current value converted to Percentage as a string.
        /// </summary>
        protected string Percent
        {
            get { return (string)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        #endregion



        #region Property Change Events

        /// <summary>
        /// Handles the change in Main and Track ring brush properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                BrushChanged(d, (Brush)e.NewValue);
            }
        }

        /// <summary>
        /// Handles the change in Main and Track ring StartCap and EndCap properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnStrokeCapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                StrokeCapChanged(d, (PenLineCap)e.NewValue);
            }
        }

        /// <summary>
        /// Handles the change in Main and Track ring Thickness properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnRingThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                RingThicknessChanged(d, (double)e.NewValue);
            }
        }

        /// <summary>
        /// Handles the change in Value property.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                var pRing = d as PercentageRing;

                pRing.ValueChanged(d, (double)e.OldValue, (double)e.NewValue);
            }
        }


        /// <summary>
        /// Handles the change in Minimum value property.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                var pRing = d as PercentageRing;
            }
        }


        /// <summary>
        /// Handles the change in Maximum value property.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                var pRing = d as PercentageRing;
            }
        }

        #endregion
    }
}