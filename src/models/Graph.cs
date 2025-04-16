namespace APSIM.Graphs;

public class Graph
{
    public string Title { get; set; }
    public List<Axis> Axes { get; set; }
    public List<Series> Series { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool ShowLegend { get; set; }
    public enum LegendPositionEnum { TopLeft, BottomLeft, TopRight, BottomRight }
    public LegendPositionEnum LegendPosition { get; set; }
}



