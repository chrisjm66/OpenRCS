using CommunityToolkit.Mvvm.ComponentModel;
using Core.Scenario;

namespace OpenRCS.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;

    public MainViewModel()
    {
        _currentPage = new HomeViewModel(this);
    }

    public void GoHome()
    {
        CurrentPage = new HomeViewModel(this);
    }

    public void GoStartSimulation()
    {
        CurrentPage = new SelectSimulationViewModel(this);
    }

    public void GoLoadSimulation()
    {
        // TODO 
    }

    public void GoSimulation(Scenario scenario)
    {
        CurrentPage = new SimulationViewModel(this, scenario);
    }

    public void GoSettings()
    {
        // TODO 
    }
}