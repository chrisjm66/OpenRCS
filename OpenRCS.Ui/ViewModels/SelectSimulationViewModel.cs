using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Scenario;

namespace OpenRCS.ViewModels;

public partial class SelectSimulationViewModel(MainViewModel mainViewModel) : ViewModelBase
{
    private readonly MainViewModel _mainViewModel = mainViewModel;
    
    [ObservableProperty]
    private ObservableCollection<Scenario> _scenarios = new (ScenarioTester.CreateTestScenarios());
    
    [ObservableProperty]
    private Scenario? _selectedScenario;

    [RelayCommand]
    private void OnClickBack()
    {
        _mainViewModel.GoHome();
    }

    [RelayCommand]
    private void OnClickScenario(Scenario scenario)
    {
        SelectedScenario = scenario;
        _mainViewModel.GoSimulation(scenario);
    }
}
