namespace Core.Layout;

public record TrackCircuitId(string Value);

public record TrackCircuitDef(ICollection<TrackEdgeId> Edges);