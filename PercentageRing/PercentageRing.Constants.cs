// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;

namespace StorageControls
{
    // TemplateParts
    [TemplatePart(Name = ContainerPartName          , Type = typeof(Grid))]
    [TemplatePart(Name = MainRingShapePartName      , Type = typeof(RingShape))]
    [TemplatePart(Name = TrackRingShapePartName     , Type = typeof(RingShape))]

    // VisualStates
    [TemplateVisualState( Name = SafeStateName      , GroupName = ControlStateGroupName )]
    [TemplateVisualState( Name = WarningStateName   , GroupName = ControlStateGroupName )]
    [TemplateVisualState( Name = DisabledStateName  , GroupName = ControlStateGroupName )]



    public partial class PercentageRing : RangeBase
    {
        internal const string ContainerPartName         = "PART_Container";
        internal const string MainRingShapePartName     = "PART_MainRingShape";
        internal const string TrackRingShapePartName    = "PART_TrackRingShape";

        internal const string ControlStateGroupName     = "ControlStates";

        internal const string SafeStateName             = "Safe";
        internal const string WarningStateName          = "Warning";
        internal const string DisabledStateName         = "Disabled";

        internal const double Degrees2Radians   = Math.PI / 180;
        internal const double MaxAngle          = 360;
        internal const double MinAngle          = 0;
    }
}