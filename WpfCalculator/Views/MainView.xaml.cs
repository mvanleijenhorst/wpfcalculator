using System.Windows;
using WpfCalculator.ViewModels;

namespace WpfCalculator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}
