﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace UnivercityDB.View
{
    /// <summary>
    /// Логика взаимодействия для PrevEmploymentsWindow.xaml
    /// </summary>
    public partial class PrevEmploymentsWindow : Window
    {
        public PrevEmploymentsWindow()
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