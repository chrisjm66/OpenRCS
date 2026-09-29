using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace OpenRCS.Controls;

public partial class SimulationController : UserControl
{
    private readonly DispatcherTimer _clockTimer = new() { Interval = TimeSpan.FromSeconds(1) };

    public SimulationController()
    {
        InitializeComponent();
        UpdateClock();
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
        DetachedFromVisualTree += (_, _) => _clockTimer.Stop();
    }

    private void UpdateClock() => ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
}
