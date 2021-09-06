using System.Windows;

namespace Modellgleis.UndoRedo.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            var vm = Bootstrapper.CreateMainViewModel();
            DataContext = vm;
        }
    }
}
