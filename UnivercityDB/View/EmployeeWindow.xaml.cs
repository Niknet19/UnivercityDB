using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Data;
using UnivercityDB.ViewModel;

namespace UnivercityDB.View
{
    /// <summary>
    /// Логика взаимодействия для 
    /// Window.xaml
    /// </summary>
    public partial class EmployeeWindow : UserControl
    {
        public EmployeeWindow()
        {
            InitializeComponent();

        }

        public void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Получаем информацию о свойстве
            var property = e.PropertyDescriptor as System.ComponentModel.PropertyDescriptor;
            if (property != null)
            {
                // Считываем атрибут Display
                var displayAttribute = property.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                if (displayAttribute != null)
                {
                    // Задаем заголовок столбца
                    e.Column.Header = displayAttribute.Name;
                }
            }
        }
    }
}
