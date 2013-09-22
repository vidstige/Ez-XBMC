using System.Windows;

namespace EzXBMC
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void Browse_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel.Browse();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.WindowLoaded();
        }
    }
}
