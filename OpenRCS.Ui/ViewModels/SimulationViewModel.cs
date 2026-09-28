using Core.Scenario;

namespace OpenRCS.ViewModels;

public class SimulationViewModel(MainViewModel mainViewModel, Scenario scenario) : ViewModelBase
{
   private readonly MainViewModel _mainViewModel = mainViewModel;
   public Scenario Scenario { get; } = scenario;
   public string Title => Scenario.Name;
   public string Description => Scenario.Description;
}
