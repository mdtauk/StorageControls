using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace StorageControls
{
    // Template Parts
    [TemplatePart(Name = TrackPartName, Type = typeof(Path))]
    [TemplatePart(Name = MainPartName, Type = typeof(Path))]
    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]

    public partial class PercentageRing
    {
        // Path Control Parts
        internal const string TrackPartName = "PART_TrackPath";
        internal const string MainPartName = "PART_MainPath";
        internal const string CanvasPartName = "PART_RingCanvas";
    }
}