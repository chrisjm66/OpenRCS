using Core.Layout;
using Core.Scenario.Diagram;

namespace Core.Scenario;

public record Scenario(
    String Id,
    String Name,
    String Description,
    SimulationLayout SimulationLayout,
    SignalDiagram SignalDiagram
    );

public static class ScenarioTester
{
    /// <summary>
    /// Creates small, in-memory scenarios intended for exercising the simulation core.
    /// </summary>
    public static ICollection<Scenario> CreateTestScenarios()
    {
        var westEntry = new TrackNodeId("WEST_BOUNDARY");
        var westJunction = new TrackNodeId("WEST_JUNCTION");
        var eastJunction = new TrackNodeId("EAST_JUNCTION");
        var eastExit = new TrackNodeId("EAST_BOUNDARY");

        var westApproach = new TrackEdgeId("WEST_APPROACH");
        var mainLine = new TrackEdgeId("MAIN_LINE");
        var passingLoop = new TrackEdgeId("PASSING_LOOP");
        var eastApproach = new TrackEdgeId("EAST_APPROACH");

        var layout = new SimulationLayout(
            Signals: new Dictionary<SignalId, SignalDef>
            {
                [new SignalId("SIG_WEST_ENTRY")] = new(new EdgeEnd(westJunction, westApproach)),
                [new SignalId("SIG_WEST_EXIT")] = new(new EdgeEnd(westJunction, mainLine)),
                [new SignalId("SIG_EAST_EXIT")] = new(new EdgeEnd(eastJunction, mainLine)),
                [new SignalId("SIG_EAST_ENTRY")] = new(new EdgeEnd(eastJunction, eastApproach))
            },
            Switches: new Dictionary<SwitchId, SwitchDef>
            {
                [new SwitchId("SW_WEST")] = new(
                    Common: new EdgeEnd(westJunction, westApproach),
                    Normal: new EdgeEnd(westJunction, mainLine),
                    Reverse: new EdgeEnd(westJunction, passingLoop)),
                [new SwitchId("SW_EAST")] = new(
                    Common: new EdgeEnd(eastJunction, eastApproach),
                    Normal: new EdgeEnd(eastJunction, mainLine),
                    Reverse: new EdgeEnd(eastJunction, passingLoop))
            },
            TrackNodes: new Dictionary<TrackNodeId, TrackNodeDef>
            {
                [westEntry] = new(new Point(-400, 0), TrackType.BOUNDARY),
                [westJunction] = new(new Point(-220, 0), TrackType.SWITCH),
                [eastJunction] = new(new Point(220, 0), TrackType.SWITCH),
                [eastExit] = new(new Point(400, 0), TrackType.BOUNDARY)
            },
            TrackEdges: new Dictionary<TrackEdgeId, TrackEdgeDef>
            {
                [westApproach] = new(westEntry, westJunction, [new Point(-400, 0), new Point(-220, 0)], new TrackProperties(true, 60), true, true),
                [mainLine] = new(westJunction, eastJunction, [new Point(-220, 0), new Point(220, 0)], new TrackProperties(true, 80), true, true),
                [passingLoop] = new(westJunction, eastJunction, [new Point(-220, 0), new Point(-100, 110), new Point(100, 110), new Point(220, 0)], new TrackProperties(true, 40), true, true),
                [eastApproach] = new(eastJunction, eastExit, [new Point(220, 0), new Point(400, 0)], new TrackProperties(true, 60), true, true)
            },
            TrackCircuits: new Dictionary<TrackCircuitId, TrackCircuitDef>
            {
                [new TrackCircuitId("TC_WEST_APPROACH")] = new([westApproach]),
                [new TrackCircuitId("TC_MAIN_LINE")] = new([mainLine]),
                [new TrackCircuitId("TC_PASSING_LOOP")] = new([passingLoop]),
                [new TrackCircuitId("TC_EAST_APPROACH")] = new([eastApproach])
            });

        return
        [
            new Scenario(
                Id: "passing-loop",
                Name: "Riverton Passing Loop",
                Description: "Two-way single-track section with a passing loop, entry signals, and two turnouts.",
                SimulationLayout: layout,
                SignalDiagram: new SignalDiagram(
                    Tracks:
                    [
                        new DiagramTrack(
                            [new DiagramPosition(-400, 0), new DiagramPosition(-220, 0)],
                            [new TrackCircuitId("TC_WEST_APPROACH")]),
                        new DiagramTrack(
                            [new DiagramPosition(-220, 0), new DiagramPosition(220, 0)],
                            [new TrackCircuitId("TC_MAIN_LINE")]),
                        new DiagramTrack(
                            [new DiagramPosition(-220, 0), new DiagramPosition(-100, 110), new DiagramPosition(100, 110), new DiagramPosition(220, 0)],
                            [new TrackCircuitId("TC_PASSING_LOOP")]),
                        new DiagramTrack(
                            [new DiagramPosition(220, 0), new DiagramPosition(400, 0)],
                            [new TrackCircuitId("TC_EAST_APPROACH")])
                    ],
                    Signals:
                    [
                        new DiagramSignal(new DiagramPosition(-245, -18), new SignalId("SIG_WEST_ENTRY")),
                        new DiagramSignal(new DiagramPosition(-195, 18), new SignalId("SIG_WEST_EXIT")),
                        new DiagramSignal(new DiagramPosition(195, -18), new SignalId("SIG_EAST_EXIT")),
                        new DiagramSignal(new DiagramPosition(245, 18), new SignalId("SIG_EAST_ENTRY"))
                    ],
                    Switches: new Dictionary<SwitchId, DiagramSwitch>
                    {
                        [new SwitchId("SW_WEST")] = new(new SwitchId("SW_WEST"), new DiagramPosition(-220, 0)),
                        [new SwitchId("SW_EAST")] = new(new SwitchId("SW_EAST"), new DiagramPosition(220, 0))
                    }))
        ];
    }
}
