using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;

namespace StorageControls
{
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = MainPartName, Type = typeof(Path))]
    [TemplatePart(Name = TrackPartName, Type = typeof(Path))]

    [TemplatePart(Name = MainStoryboardPartName, Type = typeof(Storyboard))]
    [TemplatePart(Name = MainAnimationXPartName, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = MainAnimationYPartName, Type = typeof(DoubleAnimation))]

    [TemplatePart(Name = TrackStoryboardPartName, Type = typeof(Storyboard))]
    [TemplatePart(Name = TrackAnimationXPartName, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = TrackAnimationYPartName, Type = typeof(DoubleAnimation))]

    public partial class PercentageRing : Control
    {
        internal const string ContainerPartName = "PART_Container";
        internal const string MainPartName = "PART_Main";
        internal const string TrackPartName = "PART_Track";

        internal const string MainStoryboardPartName = "PART_MainStoryboard";
        internal const string MainAnimationXPartName = "PART_MainAnimationX";
        internal const string MainAnimationYPartName = "PART_MainAnimationY";

        internal const string TrackStoryboardPartName = "PART_TrackStoryboard";
        internal const string TrackAnimationXPartName = "PART_TrackAnimationX";
        internal const string TrackAnimationYPartName = "PART_TrackAnimationY";

        internal const string MainArcName = "ARC_Main";
        internal const string TrackArcName = "ARC_Track";

        internal const double Degrees2Radians = Math.PI / 180;
        internal const double MaxAngle = 360;
        internal const double MinAngle = 0;
    }
}