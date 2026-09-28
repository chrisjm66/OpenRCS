using System;
using System.Net.Mime;
using Avalonia;
using CommunityToolkit.Mvvm.Input;

namespace OpenRCS.ViewModels;

public partial class HomeViewModel(MainViewModel mainViewModel) : ViewModelBase
{
    private readonly MainViewModel _mainViewModel = mainViewModel;

    [RelayCommand]
    private void OnClickStartSimulation()
    {
        _mainViewModel.GoStartSimulation();
    }

    [RelayCommand]
    private void OnClickLoadSimulation()
    {
        _mainViewModel.GoLoadSimulation();
    }

    [RelayCommand]
    private void OnClickSettings()
    {
        _mainViewModel.GoSettings();
    }

    [RelayCommand]
    private void OnClickQuit()
    {
        Environment.Exit(0);
    }
}