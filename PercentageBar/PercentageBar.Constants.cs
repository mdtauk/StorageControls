// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Drawing;

namespace StorageControls
{
    // TemplateParts
    [TemplatePart(Name = ContainerPartName          , Type = typeof( Grid ) )]

    [TemplatePart( Name = LeftColumnPartName        , Type = typeof( ColumnDefinition ) )]
    [TemplatePart( Name = GapColumnPartName         , Type = typeof( ColumnDefinition ) )]
    [TemplatePart( Name = RigthColumnPartName       , Type = typeof( ColumnDefinition ) )]

    [TemplatePart(Name = MainBorderPartName         , Type = typeof( Border ) )]
    [TemplatePart(Name = TrackBorderPartName        , Type = typeof( Border ) )]

    // VisualStates
    [TemplateVisualState( Name = SafeStateName      , GroupName = ControlStateGroupName )]
    [TemplateVisualState( Name = WarningStateName   , GroupName = ControlStateGroupName )]
    [TemplateVisualState( Name = DisabledStateName  , GroupName = ControlStateGroupName )]



    public partial class PercentageBar : RangeBase
    {
        internal const string ContainerPartName         = "PART_Container";

        internal const string LeftColumnPartName        = "PART_LeftColumn";
        internal const string GapColumnPartName         = "PART_GapColumn";
        internal const string RigthColumnPartName       = "PART_RightColumn";

        internal const string MainBorderPartName        = "PART_MainBar";
        internal const string TrackBorderPartName       = "PART_TrackBar";

        internal const string ControlStateGroupName     = "ControlStates";

        internal const string SafeStateName             = "Safe";
        internal const string WarningStateName          = "Warning";
        internal const string DisabledStateName         = "Disabled";
    }
}