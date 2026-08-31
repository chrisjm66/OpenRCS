using Avalonia.Controls;
using OpenRCS.ViewModels;

namespace OpenRCS.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();
    }
}