// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

namespace StorageControls
{
    public partial class PercentageBar : RangeBase
    {
        #region Main and Track Bar Brushes (Brush)

        /// <summary>
        /// Identifies the MainBarBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty MainBarBrushProperty =
            DependencyProperty.Register(
                nameof(MainBarBrush),
                typeof(Brush),
                typeof(PercentageBar),
                new PropertyMetadata(null, OnBarBrushChanged));


        /// <summary>
        /// Gets or sets the MainBar Brush.
        /// </summary>
        public Brush MainBarBrush
        {
            get { return (Brush)GetValue( MainBarBrushProperty ); }
            set { SetValue( MainBarBrushProperty , value ); }
        }

        ///

        /// <summary>
        /// Identifies the TrackBarBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackBarBrushProperty =
            DependencyProperty.Register(
                nameof(TrackBarBrush),
                typeof(Brush),
                typeof(PercentageBar),
                new PropertyMetadata(null, OnBarBrushChanged));


        /// <summary>
        /// Gets or sets the TrackBar Brush.
        /// </summary>
        public Brush TrackBarBrush
        {
            get { return (Brush)GetValue( TrackBarBrushProperty ); }
            set { SetValue( TrackBarBrushProperty , value ); }
        }


        /// <summary>
        /// Handles the change in MainBar and TrackBar Brush properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnBarBrushChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != e.NewValue )
            {
                BarBrushChanged( d , (Brush)e.NewValue );
            }
        }

        #endregion



        #region Main and Track Bar Height (double)

        /// <summary>
        /// Identifies the MainBarHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty MainBarHeightProperty =
            DependencyProperty.Register(
                nameof(MainBarHeight),
                typeof(double),
                typeof(PercentageBar),
                new PropertyMetadata(4.0, OnBarHeightChanged));


        /// <summary>
        /// Gets or sets the height of the Main Bar.
        /// </summary>
        public double MainBarHeight
        {
            get { return (double)GetValue( MainBarHeightProperty ); }
            set { SetValue( MainBarHeightProperty , value ); }
        }

        ///

        /// <summary>
        /// Identifies the TrackBarHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackBarHeightProperty =
            DependencyProperty.Register(
                nameof(TrackBarHeight),
                typeof(double),
                typeof(PercentageBar),
                new PropertyMetadata(2.0, OnBarHeightChanged));


        /// <summary>
        /// Gets or sets the height of the Track Bar.
        /// </summary>
        public double TrackBarHeight
        {
            get { return (double)GetValue( TrackBarHeightProperty ); }
            set { SetValue( TrackBarHeightProperty , value ); }
        }

        ///

        /// <summary>
        /// Handles the change in MainBar and TrackBar Height properties.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="e">The event arguments containing the old and new values.</param>
        private static void OnBarHeightChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != e.NewValue )
            {
                BarHeightChanged( d , (double)e.NewValue );
            }
        }

        #endregion



        #region PercentWarning (double)

        /// <summary>
        /// Identifies the PercentWarning dependency property.
        /// </summary>
        public static readonly DependencyProperty PercentWarningProperty =
            DependencyProperty.Register(
                nameof(PercentWarning),
                typeof(double),
                typeof(PercentageBar),
                new PropertyMetadata(75.1, OnPercentWarningChanged));


        /// <summary>
        /// Gets or sets the PercentWarning double value.
        /// </summary>
        public double PercentWarning
        {
            get { return (double)GetValue( PercentWarningProperty ); }
            set { SetValue( PercentWarningProperty , value ); }
        }


        /// <summary>
        /// Handles the change in the PercentWarning property.
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



        #region Percent Protected (string)

        /// <summary>
        /// Identifies the Percent dependency property.
        /// </summary>
        protected static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register(
                nameof(Percent),
                typeof(string),
                typeof(PercentageBar),
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
        protected override void OnValueChanged(double oldValue , double newValue)
        {
            SetOldValue( oldValue );

            base.OnValueChanged( oldValue , newValue );

            OnValueChanged( this );
        }

        #endregion
    }
}
