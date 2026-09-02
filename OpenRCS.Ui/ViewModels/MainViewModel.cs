using System;
using System.Diagnostics;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OpenRCS.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    
    [ObservableProperty]
    private ViewModelBase _currentPage;

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

    public void GoSettings()
    {
       // TODO 
    }

}