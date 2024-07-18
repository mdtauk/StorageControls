using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;

namespace StorageControls
{
    // Template Parts
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = TrailDotPartName, Type = typeof(Ellipse))]
    [TemplatePart(Name = ScaleDotPartName, Type = typeof(Ellipse))]
    [TemplatePart(Name = TrailDotTransformPartName, Type = typeof(ScaleTransform))]
    [TemplatePart(Name = ScaleDotTransformPartName, Type = typeof(ScaleTransform))]

    public partial class PercentageRing
    {
        // Path Control Parts
        internal const string TrailPartName = "PART_TrailPath";
        internal const string ScalePartName = "PART_ScalePath";
        internal const string CanvasPartName = "PART_RingCanvas";
        internal const string TrailDotPartName = "PART_TrailDot";
        internal const string ScaleDotPartName = "PART_ScaleDot";
        internal const string TrailDotTransformPartName = "PART_TrailDotScaleTransform";
        internal const string ScaleDotTransformPartName = "PART_ScaleDotScaleTransform";
    }
}
