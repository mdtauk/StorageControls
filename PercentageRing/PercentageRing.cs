using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;
using Windows.Foundation;

namespace StorageControls
{
    /// <summary>
    /// A control which takes a value and converts to a percentage displayed on a circular ring
    /// </summary>
    public partial class PercentageRing : RangeBase
    {

        #region 1. Private variables

        double              _containerSize;                 // Size of the inner container after padding
        double              _containerCenter;               // Center X and Y value of the inner container
        double              _sharedRadius;                  // Radius to be shared by both rings

        double              _oldValue;                      // Stores the previous value
        double              _oldValueAngle;                 // Stored the old ValueAngle

        double              _mainRingThickness;             // The stored main ring thickness
        double              _trackRingThickness;            // The stored track ring thickness
        ThicknessCheck      _thicknessCheck;                // Determines how the two ring thicknesses compare
        double              _largerThickness;               // The larger of the two ring thicknesses
        double              _smallerThickness;              // The smaller of the two ring thicknesses

        Grid                _containerGrid;                 // Reference to the container Grid
        RingControl         _mainRingControl;               // Reference to the Main RingControl
        RingControl         _trackRingControl;              // Reference to the Track RingControl

        Storyboard          _storyboard;                    // Stores a reference to the control Storyboard
        DoubleAnimation     _mainEndDoubleAnimation;        // Stores a reference to the Main Ring EndAngle Animation
        DoubleAnimation     _trackEndDoubleAnimation;       // Stores a reference to the Track EndAngle Ring Animation

        RectangleGeometry   _clipRect;                      // Clipping RectangleGeometry for the canvas

        double              _normalizedMinAngle;            // Stores the normalised Minimum Angle
        double              _normalizedMaxAngle;            // Stores the normalise Maximum Angle
        double              _gapAngle;                      // Stores the angle to be used to separate Main and Track rings

        #endregion



        #region 2. Private variable setters

        /// <summary>
        /// Sets the Container size to the smaller of control's Height and Width.
        /// </summary>
        void SetContainerSize(double controlWidth, double controlHeight, Thickness padding)
        {
            double correctedWidth = controlWidth - (padding.Left + padding.Right);
            double correctedHeight = controlHeight - (padding.Top + padding.Bottom);

            double check = Math.Min(correctedWidth, correctedHeight);
            double minSize = 8;

            if (check < minSize)
            {
                _containerSize = minSize;
            }
            else
            {
                _containerSize = check;
            }
        }

        /// <summary>
        /// Sets the private Container center X and Y value
        /// </summary>
        void SetContainerCenter(double containerSize)
        {
            _containerCenter = (containerSize / 2);
        }

        /// <summary>
        /// Sets the shared Radius by passing in containerSize and thickness.
        /// </summary>
        void SetSharedRadius(double containerSize, double thickness)
        {
            double check = (containerSize / 2) - (thickness / 2);
            double minSize = 4;

            if (check <= minSize)
            {
                _sharedRadius = minSize;
            }
            else
            {
                _sharedRadius = check;
            }
        }

        /// <summary>
        /// Sets the private old Value
        /// </summary>
        void SetOldValue(double value)
        {
            _oldValue = value;
        }

        /// <summary>
        /// Sets the private old ValueAngle
        /// </summary>
        void SetOldValueAngle(double value)
        {
            _oldValueAngle = value;
        }

        /// <summary>
        /// Sets the private Main Ring Thickness value
        /// </summary>
        void SetMainRingThickness(double value)
        {
            _mainRingThickness = value;
        }

        /// <summary>
        /// Sets the private Track Ting Thickness value
        /// </summary>
        void SetTrackRingThickness(double value)
        {
            _trackRingThickness = value;
        }

        /// <summary>
        /// Sets the private ThicknessCheck enum value
        /// </summary>
        void SetThicknessCheck(double mainThickness, double trackThickness)
        {
            if ( mainThickness > trackThickness )
            {
                _thicknessCheck = ThicknessCheck.Main;
            }
            else if ( mainThickness < trackThickness )
            {
                _thicknessCheck = ThicknessCheck.Track;
            }
            else
            {
                _thicknessCheck = ThicknessCheck.Equal;
            }
        }

        /// <summary>
        /// Sets the private LargerThickness value
        /// </summary>
        void SetLargerThickness(double value)
        {
            _largerThickness = value;
        }

        /// <summary>
        /// Sets the private SmallerThickness value
        /// </summary>
        void SetSmallerThickness(double value)
        {
            _smallerThickness = value;
        }

        /// <summary>
        /// Sets the private Canvas reference
        /// </summary>
        void SetContainerGrid(Grid grid)
        {
            _containerGrid = grid;
        }

        /// <summary>
        /// Sets the private Main RingControl reference
        /// </summary>
        void SetMainRingControl(RingControl ringControl)
        {
            _mainRingControl = ringControl;
        }

        /// <summary>
        /// Sets the private Track RingControl reference
        /// </summary>
        void SetTrackRingControl(RingControl ringControl)
        {
            _trackRingControl = ringControl;
        }

        /// <summary>
        /// Sets the private Storyboard reference
        /// </summary>
        void SetStoryboard(Storyboard storyboard)
        {
            _storyboard = storyboard;
        }

        /// <summary>
        /// Sets the private Main EndPoint DoubleAnimation reference
        /// </summary>
        void SetMainEndDoubleAnimation(DoubleAnimation doubleAnimation)
        {
            _mainEndDoubleAnimation = doubleAnimation;
        }

        /// <summary>
        /// Sets the private Track EndPoint DoubleAnimation reference
        /// </summary>
        void SetTrackEndDoubleAnimation(DoubleAnimation doubleAnimation)
        {
            _trackEndDoubleAnimation = doubleAnimation;
        }

        /// <summary>
        /// Sets the clipping RectangleGeometry for the Canvas
        /// </summary>
        void SetClippingRectGeo(RectangleGeometry clipRectGeo)
        {
            _clipRect = clipRectGeo;
        }

        /// <summary>
        /// Sets the private Normalized min angle
        /// </summary>
        void SetNormalizedMinAngle(double angle)
        {
            _normalizedMinAngle = angle;
        }

        /// <summary>
        /// Sets the private Normalized max angle
        /// </summary>
        void SetNormalizedMaxAngle(double angle)
        {
            _normalizedMaxAngle = angle;
        }

        /// <summary>
        /// Sets the private Gap angle
        /// </summary>
        void SetGapAngle(double angle)
        {
            _gapAngle = angle;
        }

        #endregion



        #region 3. Private variable getters

        /// <summary>
        /// Gets the Container size
        /// </summary>
        double GetContainerSize()
        {
            return _containerSize;
        }

        /// <summary>
        /// Gets the Container Center
        /// </summary>
        double GetContainerCenter()
        {
            return _containerCenter;
        }

        /// <summary>
        /// Gets the Shared Radius
        /// </summary>
        double GetSharedRadius()
        {
            return _sharedRadius;
        }

        /// <summary>
        /// Gets the old Value
        /// </summary>
        double GetOldValue()
        {
            return _oldValue;
        }

        /// <summary>
        /// Gets the old ValueAngle
        /// </summary>
        double GetOldValueAngle()
        {
            return _oldValueAngle;
        }

        /// <summary>
        /// Gets the Main Ring Thickness
        /// </summary>
        double GetMainRingThickness()
        {
            return _mainRingThickness;
        }

        /// <summary>
        /// Gets the Track Ring Thickness
        /// </summary>
        double GetTrackRingThickness()
        {
            return _trackRingThickness;
        }

        /// <summary>
        /// Gets the ThicknessCheck enum value
        /// </summary>
        ThicknessCheck GetThicknessCheck()
        {
            return _thicknessCheck;
        }

        /// <summary>
        /// Gets the Larger Thickness
        /// </summary>
        double GetLargerThickness()
        {
            return _largerThickness;
        }

        /// <summary>
        /// Gets the Smaller Thickness
        /// </summary>
        double GetSmallerThickness()
        {
            return _smallerThickness;
        }

        /// <summary>
        /// Gets the Container Grid reference
        /// </summary>
        Grid GetContainerGrid()
        {
            return _containerGrid;
        }

        /// <summary>
        /// Gets the Main RingControl reference
        /// </summary>
        RingControl GetMainRingControl()
        {
            return _mainRingControl;
        }

        /// <summary>
        /// Gets the Track RingControl reference
        /// </summary>
        RingControl GetTrackRingControl()
        {
            return _trackRingControl;
        }

        /// <summary>
        /// Gets the Storyboard reference
        /// </summary>
        Storyboard GetStoryboard()
        {
            return _storyboard;
        }

        /// <summary>
        /// Gets the Main EndAngle DoubleAnimation reference
        /// </summary>
        DoubleAnimation GetMainEndDoubleAnimation()
        {
            return _mainEndDoubleAnimation;
        }

        /// <summary>
        /// Gets the Track EndAngle DoubleAnimation reference
        /// </summary>
        DoubleAnimation GetTrackEndDoubleAnimation()
        {
            return _trackEndDoubleAnimation;
        }

        /// <summary>
        /// Gets the clipping RectangleGeometry reference
        /// </summary>
        RectangleGeometry GetClippingRectGeo()
        {
            return _clipRect;
        }

        /// <summary>
        /// Gets the Normalized Minimuim Angle
        /// </summary>
        double GetNormalizedMinAngle()
        {
            return _normalizedMinAngle;
        }

        /// <summary>
        /// Gets the Normalized Maximum Angle
        /// </summary>
        double GetNormalizedMaxAngle()
        {
            return _normalizedMaxAngle;
        }

        /// <summary>
        /// Gets the Gap Angle
        /// </summary>
        double GetGapAngle()
        {
            return _gapAngle;
        }

        #endregion



        #region 4. Initialization

        /// <summary>
        /// Applies an implicit Style of a matching TargetType
        /// </summary>
        public PercentageRing()
        {
            SizeChanged -= SizeChangedHandler;

            DefaultStyleKey = typeof(PercentageRing);

            SizeChanged += SizeChangedHandler;
        }


        /// <summary>
        /// Runs when the control has it's template applied to it
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeValues();
        }



        /// <summary>
        /// Initializes the values and properties of a PercentageRing control.
        /// </summary>
        private void InitializeValues()
        {
            // Retrieve references to visual elements
            SetContainerGrid(GetTemplateChild(ContainerPartName) as Grid);

            SetMainRingControl(GetTemplateChild(MainRingControlPartName) as RingControl);
            SetTrackRingControl(GetTemplateChild(TrackRingControlPartName) as RingControl);

            if (GetContainerGrid() != null)
            {
                SetStoryboard(GetTemplateChild(StoryboardPartName) as Storyboard);
                SetMainEndDoubleAnimation(GetTemplateChild(MainEndAnimationPartName) as DoubleAnimation);
                SetTrackEndDoubleAnimation(GetTemplateChild(TrackEndAnimationPartName) as DoubleAnimation);
            }

            // Update protected dependency properties
            ValueAngle = DoubleToAngle(Value, Minimum, Maximum, MinAngle, MaxAngle);
            Percent = GetPercentageString(Value, Minimum, Maximum);

            this.UpdateLayout(this, false);
        }

        #endregion



        #region 5. Handle property changes

        /// <summary>
        /// This method handles the SizeChanged event for a control.
        /// It adjusts the width and height of the container based on the new size.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing information about the size change.</param>
        private void SizeChangedHandler(object sender , SizeChangedEventArgs e)
        {
            var pRing = sender as PercentageRing;

            pRing.UpdateLayout( pRing , false );
        }



        /// <summary>
        /// Runs when the OnValueChanged event fires
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        private void OnValueChanged(DependencyObject d)
        {
            var pRing = (PercentageRing)d;

            // Updates the ValueAngle property
            pRing.ValueAngle = pRing.DoubleToAngle( pRing.Value , pRing.Minimum , pRing.Maximum , MinAngle , MaxAngle );

            // Updates the Percent value as a formatted string
            pRing.Percent = pRing.GetPercentageString( pRing.Value , pRing.Minimum , pRing.Maximum );

            pRing.UpdateLayout( pRing , true );
        }



        /// <inheritdoc/>
        protected override void OnMinimumChanged(double oldMinimum , double newMinimum)
        {
            base.OnMinimumChanged( oldMinimum , newMinimum );

            UpdateLayout( this , false );
        }



        /// <inheritdoc/>
        protected override void OnMaximumChanged(double oldMaximum , double newMaximum)
        {
            base.OnMaximumChanged( oldMaximum , newMaximum );

            UpdateLayout( this , false );
        }



        /// <summary>
        /// Runs when either ring Thickness property changes
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        private static void RingThicknessChanged(DependencyObject d)
        {
            var pRing = (PercentageRing)d;

            pRing.UpdateLayout( pRing , false );
        }



        /// <summary>
        /// Runs when either ring Brush property changes
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        private static void BrushChanged(DependencyObject d , Brush newValue)
        {
            var pRing = (PercentageRing)d;

            pRing.UpdateLayout( d , false );
        }


        /// <summary>
        /// Runs when either ring StartCap and EndCap properties change
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="newValue">The new value.</param>
        private static void StrokeCapChanged(DependencyObject d , PenLineCap newValue)
        {
            var pRing = (PercentageRing)d;

            pRing.UpdateLayout( d , false );
        }

        #endregion



        #region 6. Update functions

        /// <summary>
        /// Updates the layout of a PercentageRing control.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="useTransition">Indicates whether to use a transition effect.</param>
        private void UpdateLayout(DependencyObject d , bool useTransition)
        {
            var pRing = (PercentageRing)d;

            // 1. Update the size
            UpdateContainerSize( d );

            // 2. Update the Thicknesses
            UpdateStrokeThicknesses( pRing , pRing.MainRingThickness , true );
            UpdateStrokeThicknesses( pRing , pRing.TrackRingThickness , false );

            // 3. Calculate the shared radius based on container size and larger thickness
            if ( pRing.GetThicknessCheck() == ThicknessCheck.Equal )
            {
                pRing.SetSharedRadius( GetContainerSize() , 0);
            }
            else
            {
                pRing.SetSharedRadius( GetContainerSize() , GetLargerThickness() );
            }

            // 4. Calculate the gap angle
            double angle = pRing.GapThicknessToAngle(pRing.GetSharedRadius(), ( pRing.GetLargerThickness() * 0.75 ) );
            SetGapAngle( angle );

            // 5. Draw all rings with the specified transition
            DrawAllRings( pRing , useTransition );
        }



        /// <summary>
        /// Updates the normalized minimum and maximum angles.
        /// Ensures <strong>_normalizedMinAngle</strong> is in the range [-180, 180)
        /// and <strong>_normalizedMaxAngle</strong> is in the range [0, 360).
        /// </summary>
        private void UpdateNormalizedAngles()
        {
            // Calculate the modulus of MinAngle to ensure it's within [0, 360)
            var result = Modulus(MinAngle, 360);

            if (result >= 180)
            {
                result = result - 360;
            }

            SetNormalizedMinAngle(result);

            // Calculate the modulus of MaxAngle to ensure it's within [0, 360)
            result = Modulus(MaxAngle, 360);

            if (result < 180)
            {
                result = result + 360;
            }

            // Clamps max to 360
            if (result > 0 + 360)
            {
                result = result - 360;
            }

            SetNormalizedMaxAngle(result);
        }



        private void UpdateContainerSize(DependencyObject d)
        {
            var pRing = (PercentageRing)d;

            SetContainerSize( pRing.Width , pRing.Height , pRing.Padding );
            SetContainerCenter( GetContainerSize() );
            AdjustedSize = GetContainerSize();

            RectangleGeometry rectGeo = new RectangleGeometry();
            rectGeo.Rect = new Rect( 0 , 0 , AdjustedSize , AdjustedSize );
            SetClippingRectGeo( rectGeo );

            var container = GetContainerGrid();

            // Check if the Container is not null.
            if ( container != null )
            {
                // Set the _container width and height to the adjusted size (_adjSize).
                container.Width = GetContainerSize();
                container.Height = GetContainerSize();
                container.Clip = GetClippingRectGeo();
            }
        }



        private void UpdateStrokeThicknesses(DependencyObject d, double newValue, bool isMainRing)
        {
            var pRing = (PercentageRing)d;

            double mainRingThickness = pRing.GetMainRingThickness();
            double trackRingThickness = pRing.GetTrackRingThickness();

            // We want to limit the Thickness values to no more than 1/5 of the container size
            if ( isMainRing )
            {
                if ( newValue > ( pRing.GetContainerSize() / 5 ) )
                {
                    mainRingThickness = ( pRing.GetContainerSize() / 5 );
                }
                else
                {
                    mainRingThickness = newValue;
                }
            }
            else
            {
                if ( newValue > ( pRing.GetContainerSize() / 5 ) )
                {
                    trackRingThickness = ( pRing.GetContainerSize() / 5 );
                }
                else
                {
                    trackRingThickness = newValue;
                }
            }

            pRing.SetMainRingThickness( mainRingThickness );
            pRing.SetTrackRingThickness( trackRingThickness );

            pRing.SetThicknessCheck( pRing.GetMainRingThickness() , pRing.GetTrackRingThickness() );

            if ( pRing.GetThicknessCheck() == ThicknessCheck.Main )
            {
                pRing.SetLargerThickness( pRing.GetMainRingThickness() );
                pRing.SetSmallerThickness( pRing.GetTrackRingThickness() );
            }
            else if ( pRing.GetThicknessCheck() == ThicknessCheck.Track )
            {
                pRing.SetLargerThickness( pRing.GetTrackRingThickness() );
                pRing.SetSmallerThickness( pRing.GetMainRingThickness() );
            }
            else // ThicknessCheck == Equal
            {
                pRing.SetLargerThickness( pRing.GetMainRingThickness() );
                pRing.SetSmallerThickness( pRing.GetMainRingThickness() );
            }            
        }

        #endregion



        #region 7. Drawing methods

        /// <summary>
        /// Draws both rings
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="useTransition">Indicates whether to use a transition effect.</param>
        private static void DrawAllRings(DependencyObject d, bool useTransition)
        {
            var pRing = (PercentageRing)d;

            RingControl mainRing = pRing.GetMainRingControl();
            RingControl trackRing = pRing.GetTrackRingControl();

            if (mainRing != null && trackRing != null)
            {
                //
                // Grab the elements we need
                Storyboard storyboard = pRing.GetStoryboard();
                DoubleAnimation mainAnimation = pRing.GetMainEndDoubleAnimation();
                DoubleAnimation trackAnimation = pRing.GetTrackEndDoubleAnimation();
                                
                DrawRingStrokes( pRing , mainRing , trackRing );
                DrawRingAngles(pRing , mainRing , trackRing );
                DrawRingSizes( pRing , mainRing , trackRing );

                /*
                //
                // Now to Animate or set the values
                //if ( useTransition )
                //{
                //    mainRing.StartAngle = animToMainStart;
                //    trackRing.StartAngle = animToTrackStart;


                //    if ( pRing.ValueAngle > ( MinAngle + pRing.GetGapAngle() ) && pRing.ValueAngle < MaxAngle - ( pRing.GetGapAngle() * 2 ) )
                //    {                    
                //        if ( storyboard != null )
                //        {
                //            mainAnimation.To = animToMainEnd;
                //            trackAnimation.To = animToTrackEnd;

                //            storyboard.Begin();
                //        }
                //    }
                //}
                //else
                //{
                //    mainRing.EndAngle = animToMainEnd;
                //    trackRing.EndAngle = animToTrackEnd;
                //}

                //mainRing.StartAngle = animToMainStart;
                //trackRing.StartAngle = animToTrackStart;

                //mainRing.EndAngle = animToMainEnd;
                //trackRing.EndAngle = animToTrackEnd;
                */
            }
        }



        private static void DrawRingSizes(DependencyObject d , RingControl mainRing , RingControl trackRing)
        {
            var pRing = (PercentageRing)d;

            //
            // Set sizes for the rings as needed
            if ( pRing.GetThicknessCheck() == ThicknessCheck.Main )
            {
                mainRing.Width = pRing.GetContainerSize();
                mainRing.Height = pRing.GetContainerSize();

                trackRing.Width = pRing.GetContainerSize() - ( pRing.GetLargerThickness() / 2 );
                trackRing.Height = pRing.GetContainerSize() - ( pRing.GetLargerThickness() / 2 );
            }
            else if ( pRing.GetThicknessCheck() == ThicknessCheck.Track )
            {
                mainRing.Width = pRing.GetContainerSize() - ( pRing.GetLargerThickness() / 2 );
                mainRing.Height = pRing.GetContainerSize() - ( pRing.GetLargerThickness() / 2 );

                trackRing.Width = pRing.GetContainerSize();
                trackRing.Height = pRing.GetContainerSize();
            }
            else // ThicknessCheck == Equal
            {
                mainRing.Width = pRing.GetContainerSize();
                mainRing.Height = pRing.GetContainerSize();

                trackRing.Width = pRing.GetContainerSize();
                trackRing.Height = pRing.GetContainerSize();
            }

            //
            // We then set the Center properties because we have set the Width and Height
            mainRing.Center = new Point( mainRing.Width / 2 , mainRing.Height / 2 );
            trackRing.Center = new Point( trackRing.Width / 2 , trackRing.Height / 2 );

            mainRing.UpdateLayout();
            trackRing.UpdateLayout();
        }



        private static void DrawRingStrokes(DependencyObject d , RingControl mainRing , RingControl trackRing)
        {
            var pRing = (PercentageRing)d;

            //
            // Value is below or at its Minimum
            if ( pRing.Value <= pRing.Minimum )
            {
                mainRing.StrokeThickness = 0;
                trackRing.StrokeThickness = pRing.GetTrackRingThickness();
            }
            //
            // Value is between it's Minimum and its Minimum + 1 (between 0 and 1)
            else if ( pRing.Value > pRing.Minimum && pRing.Value < pRing.Minimum + 1 )
            {
                mainRing.StrokeThickness = pRing.DrawThicknessTransition( pRing , pRing.Minimum , pRing.Value , pRing.Minimum + 1 , 0.0 , pRing.GetMainRingThickness() , true );
                trackRing.StrokeThickness = pRing.GetTrackRingThickness();
            }
            //
            // Value is at or above its Maximum value
            else if ( pRing.Value >= pRing.Maximum )
            {
                mainRing.StrokeThickness = pRing.GetMainRingThickness();
                trackRing.StrokeThickness = 0;
            }
            //
            // Any value between the Minimum + 1 and the Maximum value
            else
            {
                mainRing.StrokeThickness = pRing.MainRingThickness;

                if ( pRing.ValueAngle > ( MaxAngle + 1.0 ) - ( pRing.GetGapAngle() * 2 ) )
                {
                    mainRing.StrokeThickness = pRing.GetMainRingThickness();
                    trackRing.StrokeThickness = pRing.DrawThicknessTransition( pRing , ( MaxAngle + 0.1 ) - ( pRing.GetGapAngle() * 2 ) , pRing.ValueAngle , ( MaxAngle ) - ( pRing.GetGapAngle() ) , pRing.GetTrackRingThickness() , 0.0 , true );
                }
                else
                {
                    mainRing.StrokeThickness = pRing.GetMainRingThickness();
                    trackRing.StrokeThickness = pRing.GetTrackRingThickness();
                }
            };
        }



        private static void DrawRingAngles(DependencyObject d , RingControl mainRing , RingControl trackRing)
        {
            var pRing = (PercentageRing)d;

            double animToMainStart = MinAngle;
            double animToMainEnd;
            double animToTrackStart;
            double animToTrackEnd;

            //
            // Value is below or at its Minimum
            if ( pRing.Value <= pRing.Minimum )
            {
                animToMainEnd = MinAngle;

                animToTrackStart = MinAngle;
                animToTrackEnd = MaxAngle;
            }
            //
            // Value is between it's Minimum and its Minimum + 1 (between 0 and 1)
            else if ( pRing.Value > pRing.Minimum && pRing.Value < pRing.Minimum + 1 )
            {
                animToMainEnd = pRing.ValueAngle;

                //
                // We need to interpolate the track start and end angles between pRing.Minimum and pRing.Minimum + 1
                double interpolatedStartTo    = pRing.DrawAdjustedAngle(pRing, pRing.Minimum - 0.01, pRing.Value, pRing.Minimum + 1,
                                                                          MinAngle, MinAngle - pRing.GetGapAngle(), pRing.ValueAngle, true);

                double interpolatedEndTo      = pRing.DrawAdjustedAngle(pRing, pRing.Minimum - 0.01, pRing.Value, pRing.Minimum + 1,
                                                                          MinAngle, MinAngle + pRing.GetGapAngle(), pRing.ValueAngle, true);

                animToTrackStart = interpolatedStartTo;
                animToTrackEnd = interpolatedEndTo;
            }
            //
            // Value is at or above its Maximum value
            else if ( pRing.Value >= pRing.Maximum )
            {
                animToMainEnd = MaxAngle;

                animToTrackStart = MinAngle;
                animToTrackEnd = MaxAngle;
            }
            //
            // Any value between the Minimum + 1 and the Maximum value
            else
            {
                animToMainEnd = pRing.ValueAngle;

                animToTrackStart = MinAngle - pRing.GetGapAngle();

                //
                // When the trackRing's EndAngle meets or exceeds its adjusted StartAngle
                if ( pRing.ValueAngle > ( MaxAngle + 1.0 ) - ( pRing.GetGapAngle() * 2 ) )
                {
                    animToTrackEnd = ( MaxAngle - 1 ) - pRing.GetGapAngle();
                }
                else
                {
                    animToTrackEnd = pRing.ValueAngle + pRing.GetGapAngle();
                }
            }

            mainRing.StartAngle = animToMainStart;
            trackRing.StartAngle = animToTrackStart;

            mainRing.EndAngle = animToMainEnd;
            trackRing.EndAngle = animToTrackEnd;
        }



        /// <summary>
        /// Calculates an interpolated thickness value based on the provided parameters.
        /// </summary>
        /// <param name="d">The DependencyObject representing the control.</param>
        /// <param name="startValue">The starting value for interpolation.</param>
        /// <param name="value">The current value to interpolate.</param>
        /// <param name="endValue">The ending value for interpolation.</param>
        /// <param name="startThickness">The starting thickness value.</param>
        /// <param name="endThickness">The ending thickness value.</param>
        /// <param name="useEasing">Indicates whether to apply an easing function.</param>
        /// <returns>The interpolated thickness value.</returns>
        private double DrawThicknessTransition(DependencyObject d, double startValue, double value, double endValue, double startThickness, double endThickness, bool useEasing)
            {
                // Ensure that value is within the range [startValue, endValue]
                value = Math.Max(startValue, Math.Min(endValue, value));

                // Calculate the interpolation factor (t) between 0 and 1
                var t = (value - startValue) / (endValue - startValue);

                double interpolatedThickness;

                if (useEasing)
                {
                    // Apply an easing function (e.g., quadratic ease-in-out)
                    var easedT = EasingInOutFunction(t);

                    // Interpolate the thickness
                    interpolatedThickness = startThickness + easedT * (endThickness - startThickness);
                }
                else
                {
                    // Interpolate the thickness
                    interpolatedThickness = startThickness + t * (endThickness - startThickness);
                }

                return interpolatedThickness;
            }



            /// <summary>
            /// Calculates an interpolated angle based on the provided parameters.
            /// </summary>
            /// <param name="d">The DependencyObject representing the control.</param>
            /// <param name="startValue">The starting value for interpolation.</param>
            /// <param name="value">The current value to interpolate.</param>
            /// <param name="endValue">The ending value for interpolation.</param>
            /// <param name="startAngle">The starting angle value.</param>
            /// <param name="endAngle">The ending angle value.</param>
            /// <param name="valueAngle">The angle corresponding to the current value.</param>
            /// <param name="useEasing">Indicates whether to apply an easing function.</param>
            /// <returns>The interpolated angle value.</returns>
            private double DrawAdjustedAngle(DependencyObject d, double startValue, double value, double endValue, double startAngle, double endAngle, double valueAngle, bool useEasing)
            {
                // Ensure that value is within the range [startValue, endValue]
                value = Math.Max(startValue, Math.Min(endValue, value));

                // Calculate the interpolation factor (t) between 0 and 1
                var t = (value - startValue) / (endValue - startValue);

                double interpolatedAngle;

                if (useEasing)
                {
                    // Apply an easing function
                    var easedT = EasingInOutFunction(t);

                    // Interpolate the angle
                    interpolatedAngle = startAngle + easedT * (endAngle - startAngle);
                }
                else
                {
                    // Interpolate the angle
                    interpolatedAngle = startAngle + t * (endAngle - startAngle);
                }

                return interpolatedAngle;
            }

            #endregion



        #region 8. Conversion return methods

        /// <summary>
        /// Calculates a point on a circle given an angle and radius.
        /// </summary>
        /// <param name="angle">Angle in degrees.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="centrePoint">Distance from the origin to the center of the circle.</param>
        /// <returns>A Point representing the (x, y) coordinates of the calculated point.</returns>
        private Point GetPointAroundRadius(double angle, double radius, double centrePoint)
        {
            // Convert angle from degrees to radians
            var angleInRadians = (Degrees2Radians * angle);

            // Calculate x and y coordinates
            var x = (centrePoint + (Math.Sin(angleInRadians) * radius));
            var y = (centrePoint - (Math.Cos(angleInRadians) * radius));

            // Create a Point object with the calculated coordinates
            return new Point(x, y);
        }



        /// <summary>
        /// Converts a value within a specified range to an angle within another specified range.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="minValue">The minimum value of the input range.</param>
        /// <param name="maxValue">The maximum value of the input range.</param>
        /// <param name="minAngle">The minimum angle of the output range (in degrees).</param>
        /// <param name="maxAngle">The maximum angle of the output range (in degrees).</param>
        /// <returns>The converted angle.</returns>
        private double DoubleToAngle(double value, double minValue, double maxValue, double minAngle, double maxAngle)
        {
            // If value is below the Minimum set
            if (value < minValue)
            {
                return minAngle;
            }

            // If value is above the Maximum set
            if (value > maxValue)
            {
                return maxAngle;
            }

            // Calculate the interpolated angle
            return ((value - minValue) / (maxValue - minValue) * (maxAngle - minAngle));
        }



        /// <summary>
        /// Converts a value within a specified range to a percentage.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="minValue">The minimum value of the input range.</param>
        /// <param name="maxValue">The maximum value of the input range.</param>
        /// <returns>The percentage value (between 0 and 100).</returns>
        private double DoubleToPercentage(double value, double minValue, double maxValue)
        {
            // Ensure value is within the specified range
            if (value < minValue)
            {
                return 0.0; // Below the range
            }
            else if (value > maxValue)
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
        /// Converts a percentage value to a formatted string.
        /// </summary>
        /// <param name="percent">The percentage value.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>A formatted string representing the percentage.</returns>
        private string GetPercentageString(double percent, double minValue, double maxValue)
        {
            var rounded = DoubleToPercentage(percent, minValue, maxValue);

            return Math.Round(rounded, 0).ToString() + "%";
        }



        /// <summary>
        /// Calculates the total angle needed to accommodate a gap between two strokes around a circle.
        /// </summary>
        /// <param name="thicknessA">The Thickness radius to measure.</param>
        /// <param name="radius">The radius of the rings.</param>
        /// <returns>The gap angle (sum of angles for the larger and smaller strokes).</returns>
        private double GapThicknessToAngle(double radius, double thickness)
        {
            if (radius > 0 && thickness > 0)
            {
                // Calculate the maximum number of circles
                double n = Math.PI * (radius / thickness);

                // Calculate the angle between each small circle
                double angle = 360.0 / n;

                return angle;
            }
            return 0;
        }



        /// <summary>
        /// Calculates the modulus of a number with respect to a divider.
        /// The result is always positive or zero, regardless of the input values.
        /// </summary>
        /// <param name="number">The input number.</param>
        /// <param name="divider">The divider (non-zero).</param>
        /// <returns>The positive modulus result.</returns>
        private double Modulus(double number, double divider)
        {
            // Calculate the modulus
            var result = number % divider;

            // Ensure the result is positive or zero
            result = result < 0 ? result + divider : result;

            return result;
        }



        /// <summary>
        /// Checks if the provided angle and gapAngle is greater than 180 degrees
        /// </summary>
        /// <param name="angle">The angle we want to compare (in degrees).</param>
        /// <param name="gapAngle">The gap angle we may need to account for (in degrees).</param>
        /// <returns>A boolean True if the angle is greater than 180 degrees</returns>
        private bool CheckMainArcLargeAngle(double angle, double gapAngle)
        {
            return angle > 180.0;
        }



        /// <summary>
        /// Checks if the provided angle is less than 180 degrees minus the gapAngle
        /// </summary>
        /// <param name="angle">The angle we want to compare (in degrees).</param>
        /// <param name="gapAngle">The gap angle we may need to account for (in degrees).</param>
        /// <returns>A boolean True if the angle is less than 180 degrees - minus two times the gapAngle</returns>
        private bool CheckTrackArcLargeAngle(double angle, double gapAngle)
        {
            return angle + (gapAngle * 2) < 180.0;
        }



        /// <summary>
        /// Calculates an adjusted angle using linear interpolation (lerp) between the start and end angles.
        /// </summary>
        /// <param name="startAngle">The initial angle.</param>
        /// <param name="endAngle">The final angle.</param>
        /// <param name="valueAngle">A value between 0 and 1 representing the interpolation factor.</param>
        /// <returns>The adjusted angle based on linear interpolation.</returns>
        private static double GetInterpolatedAngle(double startAngle, double endAngle, double valueAngle)
        {
            // Linear interpolation formula (lerp): GetInterpolatedAngle = (startAngle + valueAngle) * (endAngle - startAngle)
            return (startAngle + valueAngle) * (endAngle - startAngle);
        }



        /// <summary>
        /// Example quadratic ease-in-out function
        /// </summary>
        private double EasingInOutFunction(double t)
        {
            return t < 0.5 ? 2 * t * t : 1 - Math.Pow(-2 * t + 2, 2) / 2;
        }



        /// <summary>
        /// Example ease-out cubic function
        /// </summary>
        static double EaseOutCubic(double t)
        {
            return 1.0 - Math.Pow(1.0 - t, 3.0);
        }

        #endregion

    }
}