// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;

namespace StorageControls
{
    public partial class PercentageBar : RangeBase
    {
        #region 1. Private variables

        double      _oldValue;             // Stores the previous value

        double      _mainBarHeight;        // The stored Main Bar Height
        double      _trackBarHeight;       // The stored Track Bar Height

        Grid        _containerGrid;        // Reference to the container Grid

        Grid        _mainColumn;           // Reference to the MainBar Column
        Grid        _trackColumn;          // Reference to the TrackBar Column
        Grid        _gapColumn;            // Reference to the Gap Column

        Rectangle   _mainBarRectangle;     // Reference to the Main Bar Rectangle
        Rectangle   _trackBarRectangle;    // Reference to the Track Bar Rectangle

        double      _gapWidth;             // Stores the Gap between Main and Track Bars

        #endregion



        #region 2. Private variable setters

        /// <summary>
        /// Sets the private old Value
        /// </summary>
        void SetOldValue(double value)
        {
            _oldValue = value;
        }

        /// <summary>
        /// Sets the private MainBar Height value
        /// </summary>
        void SetMainBarHeight(double value)
        {
            _mainBarHeight = value;
        }

        /// <summary>
        /// Sets the private TrackBar Height value
        /// </summary>
        void SetTrackBarHeight(double value)
        {
            _trackBarHeight = value;
        }

        /// <summary>
        /// Sets the private Container Grid reference
        /// </summary>
        void SetContainerGrid(Grid grid)
        {
            _containerGrid = grid;
        }

        /// <summary>
        /// Sets the private MainBar Rectangle reference
        /// </summary>
        void SetMainBarRectangle(Rectangle mainBarRectangle)
        {
            _mainBarRectangle = mainBarRectangle;
        }

        /// <summary>
        /// Sets the private TrackBar Rectangle reference
        /// </summary>
        void SetTrackBarRectangle(Rectangle trackBarRectangle)
        {
            _trackBarRectangle = trackBarRectangle;
        }

        #endregion



        #region 3. Private variable getters

        /// <summary>
        /// Gets the old Value
        /// </summary>
        double GetOldValue()
        {
            return _oldValue;
        }

        /// <summary>
        /// Gets the MainBar Height
        /// </summary>
        double GetMainBarHeight()
        {
            return _mainBarHeight;
        }

        /// <summary>
        /// Gets the TrackBar Height
        /// </summary>
        double GetTrackBarHeight()
        {
            return _trackBarHeight;
        }

        /// <summary>
        /// Gets the Container Grid reference
        /// </summary>
        Grid GetContainerGrid()
        {
            return _containerGrid;
        }

        /// <summary>
        /// Gets the MainBar Rectangle reference
        /// </summary>
        Rectangle GetMainBarRectangle()
        {
            return _mainBarRectangle;
        }

        /// <summary>
        /// Gets the TrackBar Rectangle reference
        /// </summary>
        Rectangle GetTrackBarRectangle()
        {
            return _trackBarRectangle;
        }

        #endregion



        #region 4. Initialisation

        /// <summary>
        /// Applies an implicit Style of a matching TargetType
        /// </summary>
        public PercentageBar()
        {
            SizeChanged -= PercentageBar_SizeChanged;
            Unloaded -= PercentageBar_Unloaded;
            IsEnabledChanged -= PercentageBar_IsEnabledChanged;

            this.DefaultStyleKey = typeof(PercentageBar);

            SizeChanged += PercentageBar_SizeChanged;
            Unloaded += PercentageBar_Unloaded;
            IsEnabledChanged += PercentageBar_IsEnabledChanged;
        }



        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion



        #region 5. Handle Property Changes

        /// <summary>
        /// Handles the Main and Track Bar Brushes Changed events
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newBrush"></param>
        private static void BarBrushChanged(DependencyObject d, Brush newBrush)
        { 
            
        }



        /// <summary>
        /// Handles the Main and Track Bar Height's double value Changed events
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newHeight"></param>
        private static void BarHeightChanged(DependencyObject d , double newHeight)
        {
        
        }



        /// <summary>
        /// Handles the PrecentWarning double value Changed event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newPercentValue"></param>
        private static void PercentWarningChanged(DependencyObject d , double newPercentValue)
        { 
        
        }



        /// <summary>
        /// Handles the IsEnabledChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PercentageBar_IsEnabledChanged(object sender , DependencyPropertyChangedEventArgs e)
        {
            var pBar = sender as PercentageBar;

            pBar.UpdateLayout( pBar );
        }



        /// <summary>
        /// Handles the Unloaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PercentageBar_Unloaded(object sender , RoutedEventArgs e)
        {
            var pBar = sender as PercentageBar;

            pBar.UpdateLayout( pBar );
        }


        /// <summary>
        /// Handles the SizeChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PercentageBar_SizeChanged(object sender , SizeChangedEventArgs e)
        {
            var pBar = sender as PercentageBar;

            pBar.UpdateLayout( pBar );
        }



        /// <summary>
        /// Runs when the OnValueChanged event fires
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        private void OnValueChanged(DependencyObject d)
        {

        }

        #endregion



        #region 6. Update functions

            /// <summary>
            /// Updates the layout of a PercentageBar control.
            /// </summary>
            /// <param name="d">The DependencyObject representing the control.</param>
            private void UpdateLayout(DependencyObject d)
        {
            var pBar = (PercentageBar)d;

            // 1. Update the Container and Sizes

            // 2. Update the Bar Heights

            // 3. Set the 3 Column Widths

            // 4. Update the control's VisualState
        }

        #endregion



        #region 7. Conversion return functions

        /// <summary>
        /// Converts a value within a specified range to a percentage.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="minValue">The minimum value of the input range.</param>
        /// <param name="maxValue">The maximum value of the input range.</param>
        /// <returns>The percentage value (between 0 and 100).</returns>
        private double DoubleToPercentage(double value , double minValue , double maxValue)
        {
            // Ensure value is within the specified range
            if ( value < minValue )
            {
                return 0.0; // Below the range
            }
            else if ( value > maxValue )
            {
                return 100.0; // Above the range
            }
            else
            {
                // Calculate the normalized value
                var normalizedValue = (value - minValue) / (maxValue - minValue);

                // Convert to percentage
                var percentage = normalizedValue * 100.0;

                return percentage;
            }
        }



        /// <summary>
        /// Example quadratic ease-in-out function
        /// </summary>
        private double EasingInOutFunction(double t)
        {
            return t < 0.5 ? 2 * t * t : 1 - Math.Pow( -2 * t + 2 , 2 ) / 2;
        }



        /// <summary>
        /// Example ease-out cubic function
        /// </summary>
        static double EaseOutCubic(double t)
        {
            return 1.0 - Math.Pow( 1.0 - t , 3.0 );
        }



        /// <summary>
        /// Converts a percentage value to a formatted string.
        /// </summary>
        /// <param name="percent">The percentage value.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>A formatted string representing the percentage.</returns>
        private string GetPercentageString(double percent , double minValue , double maxValue)
        {
            var rounded = DoubleToPercentage(percent, minValue, maxValue);

            return Math.Round( rounded , 0 ).ToString() + "%";
        }

        #endregion
    }
}
