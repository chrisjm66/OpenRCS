namespace Core.Layout;

public record SimulationLayout(
    Dictionary<SignalId, SignalDef> Signals,
    Dictionary<SwitchId, SwitchDef> Switches,
    Dictionary<TrackNodeId, TrackNodeDef> TrackNodes,
    Dictionary<TrackEdgeId, TrackEdgeDef> TrackEdges,
    Dictionary<TrackCircuitId, TrackCircuitDef> TrackCircuits);