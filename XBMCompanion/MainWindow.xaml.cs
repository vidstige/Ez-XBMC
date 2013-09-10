using System.Windows;

namespace XBMCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel.Search();
        }
    }
}
