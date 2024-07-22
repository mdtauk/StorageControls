using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.DataProvider;

namespace StorageControls
{
    public partial class PercentageRingTwo : Control
    {
        #region Dependency Property Registration

        //
        // Brushes


        //
        // Doubles

        /// <summary>
        /// Identifies the MainRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty MainRingThicknessProperty =
            DependencyProperty.Register(
                nameof(MainRingThickness),
                typeof(double),
                typeof(PercentageRingTwo),
                new PropertyMetadata(3.0, OnRingThicknessChanged));

        /// <summary>
        /// Identifies the TrackRingThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackRingThicknessProperty =
            DependencyProperty.Register(
                nameof(TrackRingThickness),
                typeof(double),
                typeof(PercentageRingTwo),
                new PropertyMetadata(1.0, OnRingThicknessChanged));

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), 
                typeof(double), 
                typeof(PercentageRingTwo), 
                new PropertyMetadata(12.0, OnValueChanged));

        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(double),
                typeof(PercentageRingTwo),
                new PropertyMetadata(0.0, OnMinimumChanged));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(double),
                typeof(PercentageRingTwo),
                new PropertyMetadata(100.0, OnMaximumChanged));

        /// <summary>
        /// Identifies the ValueAngle dependency property.
        /// </summary>
        protected static readonly DependencyProperty ValueAngleProperty =
            DependencyProperty.Register(
                nameof(ValueAngle),
                typeof(double),
                typeof(PercentageRingTwo),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the AdjustedSize dependency property.
        /// </summary>
        protected static readonly DependencyProperty AdjustedSizeProperty =
            DependencyProperty.Register(
                nameof(AdjustedSize),
                typeof(double),
                typeof(PercentageRingTwo),
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
                typeof(PercentageRingTwo),
                new PropertyMetadata(string.Empty));

        #endregion



        #region Public Properties

        //
        // Brushes


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
        /// Fires when Main Ring Thickness property changes
        /// </summary>
        private static void OnRingThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                RingThicknessChanged(d, (double)e.NewValue);
            }
        }
        
        /// <summary>
        /// Fires when Value property changes
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                var pRing = d as PercentageRingTwo;

                pRing.ValueChanged(d, (double)e.OldValue, (double)e.NewValue);
            }
        }


        /// <summary>
        /// Fires when Minimum value property changes
        /// </summary>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                var pRing = d as PercentageRingTwo;
            }
        }


        /// <summary>
        /// Fires when Maximum vale property changes
        /// </summary>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                var pRing = d as PercentageRingTwo;
            }
        }

        #endregion
    }
}