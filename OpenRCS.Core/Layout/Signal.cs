namespace Core.Layout;

public class Signal(String signalId)
{
    public SignalId Id { get; } = new SignalId(signalId);
}

public record SignalId(String Value);
