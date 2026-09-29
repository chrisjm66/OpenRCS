namespace Core.Layout;

public record TrackNodeId(string Value);

public record TrackNodeDef(Point Position, TrackType TrackType);

public record TrackEdgeId(string Value);

public record TrackEdgeDef(
    TrackNodeId From,
    TrackNodeId To,
    ICollection<Point> Geometry,
    TrackProperties TrackProperties,
    bool AllowsToFrom,
    bool AllowsFromTo);

public record TrackProperties(bool Electrified, int SpeedLimit);

public record Point(float X, float Y);

public record EdgeEnd(TrackNodeId TrackNodeId, TrackEdgeId TrackEdgeId);

public enum TrackType
{
    BOUNDARY,
    BUFFER,
    SWITCH,
    CROSSING
}