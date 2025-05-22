namespace APSIM.Graphs.Tests;

[TestFixture]
public class Tests
{
    [Test]
    public void SoilGraphCreate_ShouldReturnGraph()
    {
        double[] midPoints = [ 50, 150, 250 ];
        double[] airdry = [ 0.1, 0.2, 0.3 ];
        double[] ll15 = [ 0.2, 0.3 ,0.4 ];
        double[] dul = [0.4, 0.5, 0.6 ];
        double[] sat = [0.6, 0.7, 0.8 ];

        var graph = SoilGraph.Create("title", midPoints, airdry, ll15, dul, sat, pawc: 123);
        Assert.That(graph.Series.Count, Is.EqualTo(5));
        Assert.That(graph.Title, Is.EqualTo("title"));
        Assert.That(graph.Series[0].Title, Is.EqualTo("Bucket"));
        Assert.That(graph.Series[1].Title, Is.EqualTo("LL15 (PAWC: 123mm)"));
        Assert.That(graph.Series[2].Title, Is.EqualTo("Airdry"));
        Assert.That(graph.Series[3].Title, Is.EqualTo("DUL"));
        Assert.That(graph.Series[4].Title, Is.EqualTo("SAT"));
    }
}
