using Avalonia.Controls;
using Avalonia.Controls.Templates;
using OpenRCS.ViewModels;
using OpenRCS.Views;

namespace OpenRCS;

/// <summary>
///     Maps view models to their views without reflection so published, trimmed builds remain reliable.
/// </summary>
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        return param switch
        {
            HomeViewModel => new HomeView(),
            SelectSimulationViewModel => new SelectSimulationView(),
            SimulationViewModel => new SimulationView(),
            null => null,
            _ => new TextBlock { Text = $"No view is registered for {param.GetType().Name}." }
        };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
