using System.Net.Security;
using Core.Scenario;
namespace OpenRCS.ViewModels;

public class SimulationViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel ;
    public Scenario Scenario { get; } 
    public string Title => Scenario.Name;
    public string Description => Scenario.Description;
    private Server.Server _server;

    public SimulationViewModel(MainViewModel mainViewModel, Scenario scenario)
    {
        _mainViewModel = mainViewModel;
        Scenario = scenario;
        _server = new Server.Server();
        _server.StartServerAsync(); 
    }
    
    ~SimulationViewModel()
    {
        _server.StopServerAsync();
    }
}