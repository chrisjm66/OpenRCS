using Core.Layout;

namespace Core.Scenario.Diagram;

public record SignalDiagram(
    IReadOnlyList<DiagramTrack> Tracks,
    IReadOnlyList<DiagramSignal> Signals,
    IReadOnlyDictionary<SwitchId, DiagramSwitch> Switches);

public record DiagramPosition(float X, float Y);

public record DiagramSignal(DiagramPosition Position, SignalId SignalId);

public record DiagramTrack(
    ICollection<DiagramPosition> Positions,
    ICollection<TrackCircuitId> TrackCircuitIds
);

public record DiagramSwitch(
    SwitchId SwitchId,
    DiagramPosition Position
);
