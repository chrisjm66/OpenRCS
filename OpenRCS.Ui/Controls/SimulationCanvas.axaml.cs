using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Core.Scenario;
using Core.Scenario.Diagram;
using Point = Avalonia.Point;

namespace OpenRCS.Controls;

public partial class SimulationCanvas : UserControl
{
    private const double CanvasPadding = 88;
    private const double MinimumZoom = 0.1;
    private const double MaximumZoom = 3;

    private readonly Camera _camera = new();
    private bool _isPanning;
    private bool _viewInitialized;
    private Point _lastPointerPosition;

    public static readonly StyledProperty<Scenario?> ScenarioProperty =
        AvaloniaProperty.Register<SimulationCanvas, Scenario?>(nameof(Scenario));

    public SimulationCanvas()
    {
        InitializeComponent();
        SizeChanged += (_, _) => DrawScenario();
    }

    public Scenario? Scenario
    {
        get => GetValue(ScenarioProperty);
        set => SetValue(ScenarioProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ScenarioProperty)
        {
            _viewInitialized = false;
            DrawScenario();
        }
    }

    private void DrawScenario()
    {
        if (DiagramCanvas is null) return;

        DiagramCanvas.Children.Clear();

        var scenario = Scenario;
        var viewport = DiagramCanvas.Bounds;
        if (scenario is null || viewport.Width <= 1 || viewport.Height <= 1) return;

        var positions = CollectPositions(scenario.SignalDiagram);
        if (positions.Count == 0) return;

        if (!_viewInitialized)
        {
            FitView(DiagramBounds.Create(positions), viewport.Size);
            _viewInitialized = true;
        }

        Point Map(DiagramPosition position) => WorldToScreen(position.X, position.Y, viewport.Size);

        foreach (var track in scenario.SignalDiagram.Tracks) AddTrack(track, Map);
        foreach (var diagramSwitch in scenario.SignalDiagram.Switches.Values) AddSwitch(diagramSwitch, Map);
        foreach (var signal in scenario.SignalDiagram.Signals) AddSignal(signal, Map);
    }

    private void FitView(DiagramBounds bounds, Size viewport)
    {
        var availableWidth = Math.Max(viewport.Width - CanvasPadding * 2, 1);
        var availableHeight = Math.Max(viewport.Height - CanvasPadding * 2, 1);
        var zoom = Math.Min(availableWidth / Math.Max(bounds.Width, 1), availableHeight / Math.Max(bounds.Height, 1));

        _camera.X = (bounds.MinimumX + bounds.MaximumX) / 2;
        _camera.Y = (bounds.MinimumY + bounds.MaximumY) / 2;
        _camera.Zoom = Math.Clamp(zoom, MinimumZoom, MaximumZoom);
    }

    private Point WorldToScreen(double worldX, double worldY, Size viewport) => new(
        viewport.Width / 2 + (worldX - _camera.X) * _camera.Zoom,
        viewport.Height / 2 + (worldY - _camera.Y) * _camera.Zoom);

    private Point ScreenToWorld(Point screen, Size viewport) => new(
        (screen.X - viewport.Width / 2) / _camera.Zoom + _camera.X,
        (screen.Y - viewport.Height / 2) / _camera.Zoom + _camera.Y);

    private void AddTrack(DiagramTrack track, Func<DiagramPosition, Point> map)
    {
        if (track.Positions.Count < 2) return;

        var line = new Polyline
        {
            Stroke = new SolidColorBrush(Color.Parse("#FFAEBCCD")),
            StrokeThickness = Math.Clamp(5 * _camera.Zoom, 2.5, 14),
            StrokeJoin = PenLineJoin.Round,
            Points = new Points(),
            IsHitTestVisible = false
        };

        foreach (var position in track.Positions) line.Points.Add(map(position));
        DiagramCanvas.Children.Add(line);
    }

    private void AddSwitch(DiagramSwitch diagramSwitch, Func<DiagramPosition, Point> map)
    {
        var position = map(diagramSwitch.Position);
        var size = Math.Clamp(16 * _camera.Zoom, 10, 22);
        var symbol = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(Color.Parse("#FF1B2534")),
            Stroke = new SolidColorBrush(Color.Parse("#FF71839D")),
            StrokeThickness = 1.5,
            IsHitTestVisible = false
        };
        Canvas.SetLeft(symbol, position.X - size / 2);
        Canvas.SetTop(symbol, position.Y - size / 2);
        DiagramCanvas.Children.Add(symbol);
    }

    private void AddSignal(DiagramSignal signal, Func<DiagramPosition, Point> map)
    {
        var position = map(signal.Position);
        var scale = Math.Clamp(_camera.Zoom, 0.75, 1.5);
        var aspect = signal.SignalId.Value.Contains("ENTRY", StringComparison.Ordinal)
            ? Color.Parse("#FF43B982")
            : Color.Parse("#FFEA6A73");

        var mast = new Line
        {
            StartPoint = new Point(position.X, position.Y + 11 * scale),
            EndPoint = new Point(position.X, position.Y + 28 * scale),
            Stroke = new SolidColorBrush(Color.Parse("#FF344155")),
            StrokeThickness = 2.5,
            IsHitTestVisible = false
        };
        var housingSize = 18 * scale;
        var housing = new Ellipse
        {
            Width = housingSize,
            Height = housingSize,
            Fill = new SolidColorBrush(Color.Parse("#FF101722")),
            IsHitTestVisible = false
        };
        var lampSize = 8 * scale;
        var lamp = new Ellipse
        {
            Width = lampSize,
            Height = lampSize,
            Fill = new SolidColorBrush(aspect),
            IsHitTestVisible = false
        };
        Canvas.SetLeft(housing, position.X - housingSize / 2);
        Canvas.SetTop(housing, position.Y - housingSize / 2);
        Canvas.SetLeft(lamp, position.X - lampSize / 2);
        Canvas.SetTop(lamp, position.Y - lampSize / 2);
        DiagramCanvas.Children.Add(mast);
        DiagramCanvas.Children.Add(housing);
        DiagramCanvas.Children.Add(lamp);
    }

    private void OnDiagramPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(DiagramCanvas);
        if (!point.Properties.IsRightButtonPressed) return;

        _isPanning = true;
        _lastPointerPosition = point.Position;
        e.Pointer.Capture(DiagramCanvas);
        e.Handled = true;
    }

    private void OnDiagramPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isPanning) return;

        var position = e.GetPosition(DiagramCanvas);
        Pan(position - _lastPointerPosition);
        _lastPointerPosition = position;
        e.Handled = true;
    }

    private void OnDiagramPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        StopPanning(e.Pointer);
        e.Handled = true;
    }

    private void OnDiagramPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e) => StopPanning(e.Pointer);

    private void StopPanning(IPointer pointer)
    {
        _isPanning = false;
        if (pointer.Captured == DiagramCanvas) pointer.Capture(null);
    }

    private void OnDiagramPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var delta = e.Delta;
        if (delta == default) return;

        var pointerPosition = e.GetPosition(DiagramCanvas);
        var isPinchZoom = e.KeyModifiers.HasFlag(KeyModifiers.Control);
        var isTrackpadScroll = !isPinchZoom && (OperatingSystem.IsMacOS() || Math.Abs(delta.X) > double.Epsilon);

        if (isTrackpadScroll)
        {
            // macOS reports two-finger trackpad scrolls as wheel deltas. Keep movement directly under the gesture.
            Pan(new Point(-delta.X * 20, -delta.Y * 20));
        }
        else
        {
            var zoomFactor = Math.Exp(delta.Y * (isPinchZoom ? 0.18 : 0.08));
            ZoomAt(pointerPosition, zoomFactor);
        }

        e.Handled = true;
    }

    private void Pan(Point screenDelta)
    {
        _camera.X -= screenDelta.X / _camera.Zoom;
        _camera.Y -= screenDelta.Y / _camera.Zoom;
        DrawScenario();
    }

    private void ZoomAt(Point screenPosition, double factor)
    {
        var viewport = DiagramCanvas.Bounds.Size;
        var before = ScreenToWorld(screenPosition, viewport);
        _camera.Zoom = Math.Clamp(_camera.Zoom * factor, MinimumZoom, MaximumZoom);
        var after = ScreenToWorld(screenPosition, viewport);
        _camera.X += before.X - after.X;
        _camera.Y += before.Y - after.Y;
        DrawScenario();
    }

    private static List<DiagramPosition> CollectPositions(SignalDiagram diagram)
    {
        var positions = new List<DiagramPosition>();
        foreach (var track in diagram.Tracks) positions.AddRange(track.Positions);
        foreach (var signal in diagram.Signals) positions.Add(signal.Position);
        foreach (var diagramSwitch in diagram.Switches.Values) positions.Add(diagramSwitch.Position);
        return positions;
    }

    private sealed class Camera
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Zoom { get; set; } = 1;
    }

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
