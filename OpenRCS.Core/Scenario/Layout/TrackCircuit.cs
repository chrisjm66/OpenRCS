namespace Core.Layout;

public record TrackCircuitId(String Value);

public record TrackCircuitDef(ICollection<TrackEdgeId> Edges);