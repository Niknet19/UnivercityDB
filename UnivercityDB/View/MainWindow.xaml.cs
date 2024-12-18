using System.Windows;
using UnivercityDB.View;
using UnivercityDB.ViewModel;

namespace UnivercityDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel("admin"); // убрать если нужна авторизация
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
          
            Window window = new Auth() { DataContext = new LoginViewModel() };
            this.Close();
            window.ShowDialog();
        }
    }
}
