using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using System;

namespace StorageControls
{
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = MainPartName, Type = typeof(Path))]
    [TemplatePart(Name = TrackPartName, Type = typeof(Path))]
    [TemplatePart(Name = TestTextPartName, Type = typeof(TextBlock))]

    public partial class PercentageRingTwo : Control
    {
        internal const string ContainerPartName = "PART_Container";
        internal const string MainPartName = "PART_Main";
        internal const string TrackPartName = "PART_Track";
        internal const string TestTextPartName = "PART_TestText";

        internal const double Degrees2Radians = Math.PI / 180;
        internal const double MaxAngle = 360;
        internal const double MinAngle = 0;






        /*
         * 
        /// Control Values
        /
        double controlWidth;
        double controlHeight;
        Point controlCentre             = ( controlWidth / 2 ) , ( controlHeight / 2 );
        double controlRadiusX           = ( controlWidth / 2 );
        double controlRadiusY           = ( controlHeight / 2 );

        double adjSize;                 // controlWidth or controlHeight, whichever is smaller to preserve square
        double adjCentre;               = ( adjSize / 2 ) , ( adjSize / 2 ); 


        /// Main Ring Values
        /
        double mainRingThickness;       // Percentage of ring Radius
        double mainRingCentre           = controlCentre;
        double mainAdjRadius            = adjSize - ( mainRingThickness / 2 );        
        
        Point mainStartPoint            = mainRingCentre , ( mainRingThickness / 2 ); 
        Point mainEndPoint              // Calculated point around the radius
        Size mainRingSize               = ( mainAdjRadius * 2 ) , (mainAdjRadius * 2 );

        
        /// Track Ring Values
        /
        double trackRingThickness;      // Percentage of ring Radius
        double trackRingCentre          = controlCentre;
        double trackAdjRadius           = adjSize - ( trackRingThickness / 2 );        
        
        Point trackStartPoint           = trackRingCentre , ( trackRingThickness / 2 ); 
        Point trackEndPoint             // Calculated point around the radius
        Size trackRingSize              = ( trackAdjRadius * 2 ) , (trackAdjRadius * 2 );
        
        
        double mainThicknessAsAngle     // mainRingThickness converted to Angle
        double trackThicknessAsAngle    // trackRingThickness converted to Angle
        double gapAngle                 // Use the large of the two thickness angles
                                        // then add half of each to create a gap
        *
        */








        /*
         * Things we want to do
         * 
         * Calculate our variables
         * 
         * Get Value and ensure it is between the Minimum and Maximum values
         * 
         * Convert that to a percentage
         * 
         * Convert the percentage to degrees between MinAngle(0) and MaxAngle(360)
         *
         * Initialise the control with 0 as the initial starting value
         *
         * Draw our main arc and animate it to the actual value as we draw
         * 
         * We also draw our track arc with start and end angles adjusted to make gaps
         */
    }
}