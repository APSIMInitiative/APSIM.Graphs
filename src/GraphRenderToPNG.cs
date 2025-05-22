using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.ImageSharp;
using OxyPlot.Legends;
using OxyPlot.Series;
using LegendPlacement = OxyPlot.Legends.LegendPlacement;
using OxyLegendOrientation = OxyPlot.Legends.LegendOrientation;
using OxyLegendPosition = OxyPlot.Legends.LegendPosition;

namespace APSIM.Graphs;

public static class GraphRenderToPNG
{
    public static byte[] ToPNG(this Graph graph)
    {
        PlotModel plotModel = graph.ToPlotModel();

        using var stream = new MemoryStream();
        var pngExporter = new PngExporter(width: 600, height: 800);
        pngExporter.Export(plotModel, stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream.ToArray();
    }

    /// <summary>
    /// Converts the Graph model to a PlotModel
    /// </summary>
    /// <param name="graph">The Graph model</param>
    /// <returns>The PlotModel</returns>
    private static PlotModel ToPlotModel(this Graph graph)
    {
        PlotModel plotModel = new()
        {
            Title = graph.Title,
            Background = OxyColors.White,
            DefaultFontSize = 18,
            DefaultFont = "Arial",
            PlotAreaBorderThickness = new(0),
        };

        foreach (var axis in graph.Axes.Where(a => a.IsVisible))
        {
            var position = axis.Position switch
            {
                Axis.PositionEnum.Left => AxisPosition.Left,
                Axis.PositionEnum.Right => AxisPosition.Right,
                Axis.PositionEnum.Top => AxisPosition.Top,
                Axis.PositionEnum.Bottom => AxisPosition.Bottom,
                _ => AxisPosition.Left
            };

            var axisModel = new LinearAxis
            {
                Position = position,
                Title = axis.Title,
                Minimum = axis.Minimum,
                Maximum = axis.Maximum,
                IsAxisVisible = axis.IsVisible,
                StartPosition = axis.Inverted ? 1 : 0,
                EndPosition = axis.Inverted ? 0 : 1,
                AxislineStyle = LineStyle.Solid,
            };

            plotModel.Axes.Add(axisModel);
        }

        foreach (var series in graph.Series)
        {
            LineSeries lineSeries;
            if (series.SeriesType == Series.SeriesTypeEnum.XY)
                lineSeries = new LineSeries();
            else
            {
                var areaSeries = new AreaSeries();
                foreach (var point in series.Points2)
                    areaSeries.Points2.Add(new OxyPlot.DataPoint(point.X, point.Y));
                areaSeries.Fill = OxyColor.FromArgb(series.Colour.A, series.Colour.R, series.Colour.G, series.Colour.B);
                lineSeries = areaSeries;
            }

            lineSeries.Color = OxyColor.FromArgb(series.Colour.A, series.Colour.R, series.Colour.G, series.Colour.B);
            lineSeries.Background = OxyColor.FromArgb(series.BackgroundColour.A, series.BackgroundColour.R, series.BackgroundColour.G, series.BackgroundColour.B);
            lineSeries.LineStyle = series.LineType switch
            {
                Series.LineTypeEnum.Solid => LineStyle.Solid,
                Series.LineTypeEnum.Dash => LineStyle.Dash,
                Series.LineTypeEnum.Dot => LineStyle.Dot,
                Series.LineTypeEnum.None => LineStyle.None,
                _ => LineStyle.Solid
            };
            lineSeries.MarkerType = series.MarkerType switch
            {
                Series.MarkerTypeEnum.None => MarkerType.None,
                Series.MarkerTypeEnum.Circle => MarkerType.Circle,
                Series.MarkerTypeEnum.Square => MarkerType.Square,
                Series.MarkerTypeEnum.Diamond => MarkerType.Diamond,
                Series.MarkerTypeEnum.Triangle => MarkerType.Triangle,
                Series.MarkerTypeEnum.Cross => MarkerType.Cross,
                _ => MarkerType.Circle
            };
            lineSeries.MarkerSize = 6;
            lineSeries.Title = series.Title;
            lineSeries.RenderInLegend = series.ShowInLegend;

            foreach (var point in series.Points)
                lineSeries.Points.Add(new OxyPlot.DataPoint(point.X, point.Y));
            plotModel.Series.Add(lineSeries);
        }

        // Add legend if required
        if (graph.Series.Any(s => s.ShowInLegend))
        {
            var legend = new Legend
            {
                LegendPosition = graph.LegendPosition switch
                {
                    Graph.LegendPositionEnum.TopLeft => OxyLegendPosition.TopLeft,
                    Graph.LegendPositionEnum.BottomLeft => OxyLegendPosition.BottomLeft,
                    Graph.LegendPositionEnum.TopRight => OxyLegendPosition.TopRight,
                    Graph.LegendPositionEnum.BottomRight => OxyLegendPosition.BottomRight,
                    _ => OxyLegendPosition.TopLeft
                },
                LegendOrientation = graph.LegendVertical ? OxyLegendOrientation.Vertical : OxyLegendOrientation.Horizontal,
                LegendPlacement = graph.LegendInside ? LegendPlacement.Inside : LegendPlacement.Outside,
                LegendSymbolLength = 20,
            };
            plotModel.Legends.Add(legend);
        }
        return plotModel;
    }
}
