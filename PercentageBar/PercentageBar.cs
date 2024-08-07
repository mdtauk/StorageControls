// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using Windows.Foundation;

namespace StorageControls
{
    public partial class PercentageBar : RangeBase
    {
        #region 1. Private variables

        double                  _oldValue;              // Stores the previous value
                                                       
        double                  _mainBarHeight;         // The stored Main Bar Height
        double                  _trackBarHeight;        // The stored Track Bar Height

        double                  _mainBarMaxWidth;       // The maximum width for the Main Bar
        double                  _trackBarMaxWidth;      // The maximum width for the Track Bar

        Grid                    _containerGrid;         // Reference to the container Grid
        Size                    _containerSize;         // Reference to the container Size
                                                        
        ColumnDefinition        _mainColumn;            // Reference to the MainBar Column
        ColumnDefinition        _trackColumn;           // Reference to the TrackBar Column
        ColumnDefinition        _gapColumn;             // Reference to the Gap Column
                                                        
        Border                  _mainBarBorder;         // Reference to the Main Bar Border
        Border                  _trackBarBorder;        // Reference to the Track Bar Border
                                                        
        double                  _gapWidth;              // Stores the Gap between Main and Track Bars

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
        /// Sets the private MainBar maximum width value
        /// </summary>
        void SetMainBarMaxWidth(double value)
        {
            _mainBarMaxWidth = value;
        }



        /// <summary>
        /// Sets the private TrackBar maximum width value
        /// </summary>
        void SetTrackBarMaxWidth(double value)
        {
            _trackBarMaxWidth = value;
        }

        /// <summary>
        /// Sets the private Container Grid reference
        /// </summary>
        void SetContainerGrid(Grid grid)
        {
            _containerGrid = grid;
        }

        /// <summary>
        /// Sets the private Container Size
        /// </summary>
        void SetContainerSize(Size size)
        {
            _containerSize = size;
        }

        /// <summary>
        /// Sets the private Main ColumnDefinition reference
        /// </summary>
        void SetMainColumn(ColumnDefinition columnDefinition)
        {
            _mainColumn = columnDefinition;
        }

        /// <summary>
        /// Sets the private Track ColumnDefinition reference
        /// </summary>
        void SetTrackColumn(ColumnDefinition columnDefinition)
        {
            _trackColumn = columnDefinition;
        }

        /// <summary>
        /// Sets the private Gap ColumnDefinition reference
        /// </summary>
        void SetGapColumn(ColumnDefinition columnDefinition)
        {
            _gapColumn = columnDefinition;
        }

        /// <summary>
        /// Sets the private MainBar Border reference
        /// </summary>
        void SetMainBarBorder(Border mainBarBorder)
        {
            _mainBarBorder = mainBarBorder;
        }

        /// <summary>
        /// Sets the private TrackBar Border reference
        /// </summary>
        void SetTrackBarBorder(Border trackBarBorder)
        {
            _trackBarBorder = trackBarBorder;
        }

        /// <summary>
        /// Sets the private Gap Width value
        /// </summary>
        void SetGapWidth(double value)
        {
            _gapWidth = value;
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
        /// Gets the MainBar max width
        /// </summary>
        double GetMainBarMaxWidth()
        {
            return _mainBarMaxWidth;
        }

        /// <summary>
        /// Gets the TrackBar max width
        /// </summary>
        double GetTrackBarMaxWidth()
        {
            return _trackBarMaxWidth;
        }

        /// <summary>
        /// Gets the Container Grid reference
        /// </summary>
        Grid GetContainerGrid()
        {
            return _containerGrid;
        }

        /// <summary>
        /// Gets the Container Size
        /// </summary>
        Size GetContainerSize()
        {
            return _containerSize;
        }

        /// <summary>
        /// Gets the Main ColumnDefinition reference
        /// </summary>
        ColumnDefinition GetMainColumn()
        {
            return _mainColumn;
        }

        /// <summary>
        /// Gets the Track ColumnDefinition reference
        /// </summary>
        ColumnDefinition GetTrackColumn()
        {
            return _trackColumn;
        }

        /// <summary>
        /// Gets the Gap ColumnDefinition reference
        /// </summary>
        ColumnDefinition GetGapColumn()
        {
            return _gapColumn;
        }

        /// <summary>
        /// Gets the MainBar Border reference
        /// </summary>
        Border GetMainBarBorder()
        {
            return _mainBarBorder;
        }

        /// <summary>
        /// Gets the TrackBar Border reference
        /// </summary>
        Border GetTrackBarBorder()
        {
            return _trackBarBorder;
        }

        /// <summary>
        /// Gets the Gap Width
        /// </summary>
        double GetGapWidth()
        {
            return _gapWidth;
        }

        #endregion



        #region 4. Initialisation

        /// <inheritdoc/>
        public PercentageBar()
        {
            SizeChanged -= PercentageBar_SizeChanged;
            Unloaded -= PercentageBar_Unloaded;
            IsEnabledChanged -= PercentageBar_IsEnabledChanged;

            this.DefaultStyleKey = typeof( PercentageBar );

            SizeChanged += PercentageBar_SizeChanged;
            Unloaded += PercentageBar_Unloaded;
            IsEnabledChanged += PercentageBar_IsEnabledChanged;
        }



        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateInitialLayout( this );
        }

        #endregion



        #region 5. Handle Property Changes

        /// <summary>
        /// Handles the Main and Track Bar Brushes Changed events
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newBrush"></param>
        private static void BarBrushChanged(DependencyObject d , Brush newBrush)
        {
            var pBar = d as PercentageBar;

            UpdateControl( pBar );
        }



        /// <summary>
        /// Handles the Main Bar Height's double value Changed event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newHeight"></param>
        private static void MainBarHeightChanged(DependencyObject d , double newHeight)
        {
            var pBar = d as PercentageBar;

            pBar.SetMainBarHeight( newHeight );

            UpdateControl( pBar );
        }



        /// <summary>
        /// Handles the Track Bar Height's double value Changed event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newHeight"></param>
        private static void TrackBarHeightChanged(DependencyObject d , double newHeight)
        {
            var pBar = d as PercentageBar;

            pBar.SetTrackBarHeight( newHeight );

            UpdateControl( pBar );
        }        



        /// <summary>
        /// Handles the PrecentWarning double value Changed event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newPercentValue"></param>
        private static void PercentWarningChanged(DependencyObject d , double newPercentValue)
        {
            var pBar = d as PercentageBar;

            UpdateControl( pBar );
        }



        /// <summary>
        /// Handles the IsEnabledChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PercentageBar_IsEnabledChanged(object sender , DependencyPropertyChangedEventArgs e)
        {
            var pBar = sender as PercentageBar;

            UpdateControl( pBar );
        }



        /// <summary>
        /// Handles the Unloaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PercentageBar_Unloaded(object sender , RoutedEventArgs e)
        {
            var pBar = sender as PercentageBar;

            SizeChanged -= PercentageBar_SizeChanged;
            Unloaded -= PercentageBar_Unloaded;
            IsEnabledChanged -= PercentageBar_IsEnabledChanged;
        }


        /// <summary>
        /// Handles the SizeChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PercentageBar_SizeChanged(object sender , SizeChangedEventArgs e)
        {
            var pBar = sender as PercentageBar;

            UpdateContainer( pBar , (Size)e.NewSize );

            UpdateControl( pBar );
        }



        private void OnValueChanged(DependencyObject d)
        {
            var pBar = (PercentageBar)d;

            UpdateValue( pBar , pBar.Value , pBar.GetOldValue() );
        }

        #endregion



        #region 6. Update functions

        /// <summary>
        /// Updates the initial layout of the PercentageBar control
        /// </summary>
        private void UpdateInitialLayout(DependencyObject d)
        {
            var pBar = d as PercentageBar;

            // Retrieve references to visual elements
            pBar.SetContainerGrid( pBar.GetTemplateChild( ContainerPartName ) as Grid );

            pBar.SetMainColumn( pBar.GetTemplateChild( MainColumnPartName ) as ColumnDefinition );
            pBar.SetTrackColumn( pBar.GetTemplateChild( TrackColumnPartName ) as ColumnDefinition );
            pBar.SetGapColumn( pBar.GetTemplateChild( GapColumnPartName ) as ColumnDefinition );

            pBar.SetMainBarBorder( pBar.GetTemplateChild( MainBorderPartName ) as Border );
            pBar.SetTrackBarBorder( pBar.GetTemplateChild( TrackBorderPartName ) as Border );

            pBar.SetMainBarHeight( pBar.MainBarHeight );
            pBar.SetTrackBarHeight( pBar.TrackBarHeight );
        }



        /// <summary>
        /// Updates the PercentageBar control.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        private static void UpdateControl(DependencyObject d)
        {
            var pBar = (PercentageBar)d;

            // 1. Update the Bar Heights
            UpdateBarHeights( pBar , pBar.GetMainBarHeight() , pBar.GetTrackBarHeight() );

            // 2. Set the 3 Column Widths
            UpdateColumnWidths( pBar , pBar.Value , pBar.Minimum , pBar.Maximum );

            // 3. Update the control's VisualState
            UpdateVisualState( pBar );
        }



        private static void UpdateValue(DependencyObject d , double newValue , double oldValue)
        {
            var pBar = (PercentageBar)d;

            pBar.SetOldValue( oldValue );

            pBar.Percent = pBar.GetPercentageString( pBar.DoubleToPercentage( newValue , pBar.Minimum , pBar.Maximum ) , pBar.Minimum , pBar.Maximum );

            UpdateControl( pBar );
        }



        private static void UpdateBarHeights(DependencyObject d , double mainBarHeight , double trackBarHeight)
        {
            var pBar = (PercentageBar)d;

            Border mainBorder = pBar.GetMainBarBorder();
            Border trackBorder = pBar.GetTrackBarBorder();

            if ( mainBorder != null || trackBorder != null )
            {
                mainBorder.Height = mainBarHeight;
                trackBorder.Height = trackBarHeight;

                mainBorder.CornerRadius = new CornerRadius( mainBarHeight / 2 );
                trackBorder.CornerRadius = new CornerRadius( trackBarHeight / 2 );
            }

            // Finds the larger of the two height values
            double calculatedGap = Math.Max(mainBarHeight, trackBarHeight);

            pBar.SetGapWidth( calculatedGap );
        }



        private static void UpdateColumnWidths(DependencyObject d , double value , double minValue , double maxValue)
        {
            var pBar = (PercentageBar)d;

            ColumnDefinition gapColumn = pBar.GetGapColumn();
            ColumnDefinition mainColumn = pBar.GetMainColumn();
            ColumnDefinition trackColumn = pBar.GetTrackColumn();

            if ( gapColumn != null || mainColumn != null || trackColumn != null )
            {
                if ( pBar.GetContainerSize().Width > pBar.GetTrackBarHeight() || pBar.GetContainerSize().Width > pBar.GetMainBarHeight() )
                {                 
                    double valuePercent = pBar.DoubleToPercentage(pBar.Value, pBar.Minimum, pBar.Maximum);
                    double minPercent   = pBar.DoubleToPercentage(pBar.Minimum, pBar.Minimum, pBar.Maximum);
                    double maxPercent   = pBar.DoubleToPercentage(pBar.Maximum, pBar.Minimum, pBar.Maximum);

                    if ( valuePercent <= minPercent )           // Value is <= Minimum
                    {
                        gapColumn.Width = new GridLength(0);

                        mainColumn.Width = new GridLength(0);

                        trackColumn.MaxWidth = 0;
                        trackColumn.MinWidth = pBar.GetContainerSize().Width;

                        mainColumn.MaxWidth = pBar.GetContainerSize().Width;
                        mainColumn.MinWidth = 0;
                    }
                    else if ( valuePercent >= maxPercent )     // Value is >= Maximum
                    {
                        gapColumn.Width = new GridLength( 0 );

                        trackColumn.Width = new GridLength( 0 );

                        trackColumn.MaxWidth = pBar.GetContainerSize().Width;
                        trackColumn.MinWidth = 0;

                        mainColumn.MaxWidth = 0;
                        mainColumn.MinWidth = pBar.GetContainerSize().Width;
                    }
                    else
                    {
                        gapColumn.Width = new GridLength( pBar.GetGapWidth() );

                        mainColumn.MaxWidth = pBar.GetContainerSize().Width - ( pBar.GetMainBarHeight() + pBar.GetTrackBarHeight() );
                        trackColumn.MaxWidth = pBar.GetContainerSize().Width - ( pBar.GetMainBarHeight() + pBar.GetTrackBarHeight() );

                        mainColumn.MinWidth = pBar.GetMainBarHeight();
                        trackColumn.MinWidth = pBar.GetTrackBarHeight();

                        double calculatedMainWidth = (mainColumn.MaxWidth / 100) * valuePercent;

                        mainColumn.Width = new GridLength( calculatedMainWidth );
                        trackColumn.Width = new GridLength( 1 , GridUnitType.Star );
                    }
                }
            }
        }



        private static void UpdateContainer(DependencyObject d , Size newSize)
        {
            var pBar = (PercentageBar)d;

            double containerWidth = newSize.Width - ( pBar.Padding.Left + pBar.Padding.Right );
            double containerHeight = newSize.Height - ( pBar.Padding.Top + pBar.Padding.Bottom );

            pBar.SetContainerSize( new Size( containerWidth, containerHeight ));
        }



        private static void UpdateVisualState(DependencyObject d)
        {
            var pBar = (PercentageBar)d;

            if ( pBar.IsEnabled == false )
            {
                VisualStateManager.GoToState( pBar , DisabledStateName , true );
            }
            else
            {
                double currentPercentage = pBar.DoubleToPercentage( pBar.Value , pBar.Minimum , pBar.Maximum );

                if ( currentPercentage >= pBar.PercentWarning )
                {
                    VisualStateManager.GoToState( pBar , WarningStateName , true );
                }
                else
                {
                    VisualStateManager.GoToState( pBar , SafeStateName , true );
                }
            }
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
