using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Core.Scenario;
using Core.Scenario.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRCS.Controls;

public partial class SimulationCanvas : UserControl
{
    public static readonly StyledProperty<Scenario?> ScenarioProperty =
        AvaloniaProperty.Register<SimulationCanvas, Scenario?>(nameof(Scenario));

    private const double CanvasPadding = 72;

    public Scenario? Scenario
    {
        get => GetValue(ScenarioProperty);
        set => SetValue(ScenarioProperty, value);
    }

    public SimulationCanvas()
    {
        InitializeComponent();
        SizeChanged += (_, _) => DrawScenario();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ScenarioProperty)
        {
            DrawScenario();
        }
    }

    private void DrawScenario()
    {
        if (DiagramCanvas is null)
        {
            return;
        }

        DiagramCanvas.Children.Clear();

        var scenario = Scenario;
        if (scenario is null || Bounds.Width <= 1 || Bounds.Height <= 1)
        {
            return;
        }

        var positions = CollectPositions(scenario.SignalDiagram);
        if (positions.Count == 0)
        {
            return;
        }

        var bounds = DiagramBounds.Create(positions);
        var scale = Math.Min(
            (Bounds.Width - (CanvasPadding * 2)) / Math.Max(bounds.Width, 1),
            (Bounds.Height - (CanvasPadding * 2)) / Math.Max(bounds.Height, 1));
        scale = Math.Max(scale, 0.01);

        Point Map(DiagramPosition position) => new(
            CanvasPadding + ((position.X - bounds.MinimumX) * scale),
            CanvasPadding + ((position.Y - bounds.MinimumY) * scale));

        foreach (var track in scenario.SignalDiagram.Tracks)
        {
            var line = new Polyline
            {
                Stroke = new SolidColorBrush(Color.Parse("#AEB9D0")),
                StrokeThickness = 6,
                StrokeJoin = PenLineJoin.Round,
                Points = new Points()
            };

            foreach (var position in track.Positions)
            {
                line.Points.Add(Map(position));
            }

            DiagramCanvas.Children.Add(line);
            AddTrackLabel(track, scenario.SimulationLayout, Map);
        }

        foreach (var diagramSwitch in scenario.SignalDiagram.Switches.Values)
        {
            AddSwitch(diagramSwitch, Map);
        }

        foreach (var signal in scenario.SignalDiagram.Signals)
        {
            AddSignal(signal, Map);
        }
    }

    private void AddTrackLabel(DiagramTrack track, Core.Layout.SimulationLayout layout, Func<DiagramPosition, Point> map)
    {
        if (track.Positions.Count == 0 || track.TrackCircuitIds.Count == 0)
        {
            return;
        }

        var position = map(track.Positions.ElementAt(track.Positions.Count / 2));
        var label = new TextBlock
        {
            Text = PrettyName(track.TrackCircuitIds.FirstOrDefault()?.Value ?? "Track"),
            Foreground = new SolidColorBrush(Color.Parse("#DCE5FA")),
            FontSize = 12,
            FontWeight = FontWeight.SemiBold
        };
        Canvas.SetLeft(label, position.X - 36);
        Canvas.SetTop(label, position.Y - 38);
        DiagramCanvas.Children.Add(label);
    }

    private void AddSwitch(DiagramSwitch diagramSwitch, Func<DiagramPosition, Point> map)
    {
        var position = map(diagramSwitch.Position);
        var symbol = new Ellipse
        {
            Width = 18,
            Height = 18,
            Fill = new SolidColorBrush(Color.Parse("#202B45")),
            Stroke = new SolidColorBrush(Color.Parse("#7D8BB0")),
            StrokeThickness = 2
        };
        Canvas.SetLeft(symbol, position.X - (symbol.Width / 2));
        Canvas.SetTop(symbol, position.Y - (symbol.Height / 2));
        DiagramCanvas.Children.Add(symbol);
    }

    private void AddSignal(DiagramSignal signal, Func<DiagramPosition, Point> map)
    {
        var position = map(signal.Position);
        var aspect = signal.SignalId.Value.Contains("ENTRY", StringComparison.Ordinal)
            ? Color.Parse("#58D68D")
            : Color.Parse("#E74C5B");

        var mast = new Line
        {
            StartPoint = new Point(position.X, position.Y + 12),
            EndPoint = new Point(position.X, position.Y + 31),
            Stroke = new SolidColorBrush(Color.Parse("#1C2333")),
            StrokeThickness = 3
        };
        var housing = new Ellipse
        {
            Width = 20,
            Height = 20,
            Fill = new SolidColorBrush(Color.Parse("#131928"))
        };
        var lamp = new Ellipse
        {
            Width = 9,
            Height = 9,
            Fill = new SolidColorBrush(aspect)
        };
        Canvas.SetLeft(housing, position.X - 10);
        Canvas.SetTop(housing, position.Y - 10);
        Canvas.SetLeft(lamp, position.X - 4.5);
        Canvas.SetTop(lamp, position.Y - 4.5);
        DiagramCanvas.Children.Add(mast);
        DiagramCanvas.Children.Add(housing);
        DiagramCanvas.Children.Add(lamp);
    }

    private static List<DiagramPosition> CollectPositions(SignalDiagram diagram)
    {
        var positions = new List<DiagramPosition>();
        foreach (var track in diagram.Tracks)
        {
            positions.AddRange(track.Positions);
        }
        foreach (var signal in diagram.Signals)
        {
            positions.Add(signal.Position);
        }
        foreach (var diagramSwitch in diagram.Switches.Values)
        {
            positions.Add(diagramSwitch.Position);
        }
        return positions;
    }

    private static string PrettyName(string value) => value
        .Replace("TC_", string.Empty, StringComparison.Ordinal)
        .Replace('_', ' ')
        .ToLowerInvariant()
        .Replace("west", "West")
        .Replace("main", "Main")
        .Replace("passing", "Passing")
        .Replace("east", "East");

    private readonly record struct DiagramBounds(double MinimumX, double MinimumY, double MaximumX, double MaximumY)
    {
        public double Width => MaximumX - MinimumX;
        public double Height => MaximumY - MinimumY;

        public static DiagramBounds Create(IReadOnlyList<DiagramPosition> positions)
        {
            var minimumX = positions[0].X;
            var minimumY = positions[0].Y;
            var maximumX = positions[0].X;
            var maximumY = positions[0].Y;

            foreach (var position in positions)
            {
                minimumX = Math.Min(minimumX, position.X);
                minimumY = Math.Min(minimumY, position.Y);
                maximumX = Math.Max(maximumX, position.X);
                maximumY = Math.Max(maximumY, position.Y);
            }

            return new DiagramBounds(minimumX, minimumY, maximumX, maximumY);
        }
    }
}
