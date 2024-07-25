using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;

namespace StorageControls
{
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = MainRingControlPartName, Type = typeof(RingControl))]
    [TemplatePart(Name = TrackRingControlPartName, Type = typeof(RingControl))]

    [TemplatePart(Name = StoryboardPartName, Type = typeof(Storyboard))]

    [TemplatePart(Name = MainStartAnimationPartName, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = MainEndAnimationPartName, Type = typeof(DoubleAnimation))]

    [TemplatePart(Name = TrackStartAnimationPartName, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = TrackEndAnimationPartName, Type = typeof(DoubleAnimation))]

    public partial class PercentageRing : RangeBase
    {
        internal const string ContainerPartName = "PART_Container";
        internal const string MainRingControlPartName = "PART_MainRingControl";
        internal const string TrackRingControlPartName = "PART_TrackRingControl";

        internal const string StoryboardPartName = "PART_Storyboard";

        internal const string MainStartAnimationPartName = "PART_MainStartDoubleAnimation";
        internal const string MainEndAnimationPartName = "PART_MainEndDoubleAnimation";

        internal const string TrackStartAnimationPartName = "PART_TrackStartDoubleAnimation";
        internal const string TrackEndAnimationPartName = "PART_TrackEndDoubleAnimation";

        internal const double Degrees2Radians = Math.PI / 180;
        internal const double MaxAngle = 360;
        internal const double MinAngle = 0;
    }
}