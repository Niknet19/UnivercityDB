using System.Windows;
using UnivercityDB.ViewModel;

namespace UnivercityDB.View
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        public AddEmployee()
        {
            InitializeComponent();
            DataContext = new AddEmployeeViewModel();
        }
    }
}
