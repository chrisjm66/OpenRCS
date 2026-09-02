using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OpenRCS.ViewModels;

public partial class SelectSimulationViewModel(MainViewModel mainViewModel) : ViewModelBase
{
   private readonly MainViewModel _mainViewModel = mainViewModel;
   
   [RelayCommand]
   private void OnClickBack()
   {
     _mainViewModel.GoHome(); 
   }
}