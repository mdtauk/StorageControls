using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace StorageControls
{
    public partial class PercentageRing : RangeBase
    {
        #region DEPENDENCY PROPERTY REGISTRATION

        // Brushes

        /// <summary>
        /// Identifies the TrailBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrailBrushProperty =
            DependencyProperty.Register(
                nameof(TrailBrush), 
                typeof(Brush), 
                typeof(PercentageRing), 
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ScaleBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(
                nameof(ScaleBrush), 
                typeof(Brush), 
                typeof(PercentageRing), 
                new PropertyMetadata(null));

        // Doubles

        /// <summary>
        /// Identifies the ScaleThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleThicknessProperty =
            DependencyProperty.Register(
                nameof(ScaleThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(3, OnScaleThicknessChanged));

        /// <summary>
        /// Identifies the TrailThickness dependency property.
        /// </summary>
        public static readonly DependencyProperty TrailThicknessProperty =
            DependencyProperty.Register(
                nameof(TrailThickness),
                typeof(double),
                typeof(PercentageRing),
                new PropertyMetadata(3, OnTrailThicknessChanged));

        #endregion

        #region PUBLIC PROPERTIES

        // Brushes

        /// <summary>
        /// Gets or sets the trail brush.
        /// </summary>
        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale brush.
        /// </summary>
        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        // Doubles

        /// <summary>
        /// Gets or sets the thickness of the scale ring.
        /// </summary>
        public double ScaleThickness
        {
            get { return (double)GetValue(ScaleThicknessProperty); }
            set { SetValue(ScaleThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the trail ring.
        /// </summary>
        public double TrailThickness
        {
            get { return (double)GetValue(TrailThicknessProperty); }
            set { SetValue(TrailThicknessProperty, value); }
        }

        #endregion

        #region PROPERTY CHANGED EVENTS

        private static void OnScaleThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Run the code to update thickness values.

            //ScaleThicknessChanged(d, (double)e.NewValue);
        }

        private static void OnTrailThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Run the code to update thickness values.

            //TrailThicknessChanged(d, (double)e.NewValue);
        }

        #endregion
    }
}