using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UnivercityDB.ViewModel;

namespace UnivercityDB.View
{
    /// <summary>
    /// Логика взаимодействия для UpdatePasswordsWindow.xaml
    /// </summary>
    public partial class UpdatePasswordsWindow : Window
    {
        public UpdatePasswordsWindow(string username)
        {
            InitializeComponent();
            DataContext = new UpdatePasswordsViewModel(username);
        }

    }
}
