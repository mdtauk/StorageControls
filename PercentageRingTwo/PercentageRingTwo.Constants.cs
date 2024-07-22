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
    }
}