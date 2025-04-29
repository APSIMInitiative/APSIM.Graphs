using System.Drawing;

namespace APSIM.Graphs;

public static class SoilGraph
{
    /// <summary>Convert a soil to a graph PNG.</summary>
    /// <param name="title">Title of graph</param>
    /// <param name="midPoints">Layer mid points</param>
    /// <param name="airdry">Air dry (mm/mm)</param>
    /// <param name="ll15">Lower limit 15 bar (mm/mm)</param>
    /// <param name="dul">Drained upper limit (mm/mm)</param>
    /// <param name="sat">Saturation (mm/mm)</param>
    /// <param name="cll">Crop lower limit - optional (mm/mm)</param>
    /// <param name="cropName">Name of crop - optional</param>
    /// <param name="pawc">Plant available water (mm)</param>
    /// <param name="swMidPoints">The layer mid points for sw.</param>
    /// <param name="sw">The soil water.</param>
    public static Graph Create(string title, IReadOnlyList<double> midPoints,
                               IReadOnlyList<double> airdry, IReadOnlyList<double> ll15,
                               IReadOnlyList<double> dul, IReadOnlyList<double> sat,
                               IReadOnlyList<double> cll = null, string cropName = null, double pawc = double.NaN,
                               IReadOnlyList<double> swMidPoints = null, IReadOnlyList<double> sw = null)
    {
        Graph graph = new()
        {
            Title = title,
            LegendPosition = Graph.LegendPositionEnum.BottomLeft,
            Axes =
            [
                new()
                {
                    Title = "Depth (mm)",
                    Minimum = 0,
                    Position = Axis.PositionEnum.Left,
                    Maximum = (Math.Truncate(midPoints.Last() / 200) + 1) * 200,  // Scale up to the nearest 200
                    IsVisible = true,
                    Inverted = true
                },
                new()
                {
                    Title = "Volumetric Water Content (mm/mm)",
                    Position = Axis.PositionEnum.Top,
                    IsVisible = true
                }
            ]
        };
        IReadOnlyList<double> ll;
        string llName;
        if (ll15 != null)
        {
            ll = ll15;
            llName = "LL15";
        }
        else
        {
            ll = cll;
            llName = cropName;
        }

        if (!double.IsNaN(pawc))
            llName += $" PAWC: {pawc:F0} mm";

        graph.Series =
        [
            new()
            {
                Title = $"{llName}",
                SeriesType = Series.SeriesTypeEnum.Area,
                Points = ll.Zip(midPoints)
                           .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                Points2 = dul.Zip(midPoints)
                             .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                ShowInLegend = true,
                LineType = Series.LineTypeEnum.Solid,
                Colour = Color.LightBlue,
            },
            new()
            {
                Title = "Airdry",
                Points = airdry.Zip(midPoints)
                               .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                ShowInLegend = true,
                LineType = Series.LineTypeEnum.Dot,
                Colour = Color.Red,
            },
            new()
            {
                Title = "LL15",
                Points = ll15.Zip(midPoints)
                             .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                ShowInLegend = true,
                LineType = Series.LineTypeEnum.Solid,
                Colour = Color.Red,
            },
            new()
            {
                Title = "DUL",
                Points = dul.Zip(midPoints)
                            .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                ShowInLegend = true,
                LineType = Series.LineTypeEnum.Solid,
                Colour = Color.Blue,
            },
            new()
            {
                Title = "SAT",
                Points = sat.Zip(midPoints)
                            .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                ShowInLegend = true,
                LineType = Series.LineTypeEnum.Dot,
                Colour = Color.Blue,
            }
        ];

        if (swMidPoints != null && sw != null)
        {
            graph.Series.Add(new Series()
            {
                Title = "SW",
                Points = sw.Zip(swMidPoints)
                           .Select(zip => new DataPoint { X = zip.First, Y = zip.Second }),
                ShowInLegend = true,
                LineType = Series.LineTypeEnum.Solid,
                Colour = Color.Green,
            });
        }
        return graph;
    }
}