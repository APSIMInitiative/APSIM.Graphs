namespace APSIM.Graphs;

public class Axis
{
    public string Title { get; set; }
    public enum PositionEnum { Left, Right, Top, Bottom }
    public PositionEnum Position { get; set; }
    public double Minimum { get; set; } = double.NaN;
    public double Maximum { get; set; } = double.NaN;
    public double Interval { get; set; } = double.NaN;
    public bool IsVisible { get; set; }
    public bool Inverted { get; set; }
}