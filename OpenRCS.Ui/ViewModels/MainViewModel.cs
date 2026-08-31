using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenRCS.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}