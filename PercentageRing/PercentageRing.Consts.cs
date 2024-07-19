using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace StorageControls
{
    // Template Parts
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]

    public partial class PercentageRing
    {
        // Path Control Parts
        internal const string TrailPartName = "PART_TrailPath";
        internal const string ScalePartName = "PART_ScalePath";
        internal const string CanvasPartName = "PART_RingCanvas";
    }
}