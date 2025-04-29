using System.Drawing;

namespace APSIM.Graphs;

public class Series
{
    public string Title { get; set; }
    public IEnumerable<DataPoint> Points { get; set; }
    public IEnumerable<DataPoint> Points2 { get; set; }
    public bool ShowInLegend { get; set; }
    public enum LineTypeEnum { Solid, Dash, Dot, None }
    public LineTypeEnum LineType { get; set; }
    public Color Colour { get; set; }
    public Color BackgroundColour { get; set; }
    public enum MarkerTypeEnum { None, Circle, Square, Diamond, Triangle, Cross }
    public MarkerTypeEnum MarkerType { get; set; }
    public enum SeriesTypeEnum { XY, Area }
    public SeriesTypeEnum SeriesType { get; set; }
}