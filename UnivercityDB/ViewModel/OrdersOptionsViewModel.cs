using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.View;
using UnivercityDB.ViewModel;

namespace UnivercityDB.ViewModel
{
    public class OrdersOptionsViewModel : BaseViewModel
    {
        public ICommand ChooseOrderCommand { get; set; }

        private object _currentView;

        private MainViewModel _mainViewModel;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public OrdersOptionsViewModel(MainViewModel main)
        {
            _mainViewModel = main;
            ChooseOrderCommand = new RelayCommand(OnMenuItemSelected);
        }
        public void OnMenuItemSelected(object parameter)
        {
            if (parameter is string selectedMenu)
            {

                if (selectedMenu is "Promotions" or "Penalties")
                {
                    //CurrentView = new OrderWindow(selectedMenu);
                    _mainViewModel.CurrentView = new OrderWindow() { DataContext = new OrderViewModel(selectedMenu, _mainViewModel.UserPermissions.OrdersPermission) };
                };
                if(selectedMenu is "Movements")
                {
                    _mainViewModel.CurrentView = new OrderWindow() { DataContext = new MovementOrderViewModel(_mainViewModel.UserPermissions.OrdersPermission) };
                }
                //MessageBox.Show($"CurrentView изменён: {CurrentView.GetType().Name}");

            }
        }
    }
}
