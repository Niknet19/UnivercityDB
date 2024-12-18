using System.Windows;
using UnivercityDB.ViewModel;

namespace UnivercityDB.View
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
