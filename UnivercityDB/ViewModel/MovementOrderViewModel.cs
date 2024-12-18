using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UnivercityDB.Model;
using UnivercityDB.View;

namespace UnivercityDB.ViewModel
{
    public class MovementOrderViewModel : BaseViewModel
    {
        private Permission _permissions;

        public string SearchText { get; set; }

        private MovementsOrderModel _model;

        public ObservableCollection<MovementOrder> Orders { get; set; }

        public MovementOrder? SelectedOrder { get; set; }

        /*public ObservableCollection<Order> Orders
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }*/
        public ICommand SearchCommand { get; set; }
        public ICommand UpdateSelectedCommand { get; set; }

        public ICommand DeleteSelectedCommand { get; set; }

        public ICommand AddOrderCommand { get; set; }

        public MovementOrderViewModel(Permission ?permissions)
        {
            _model = new MovementsOrderModel();
            SearchCommand = new RelayCommand(Search);
            DeleteSelectedCommand = new RelayCommand(DeleteOrder, CanDelete);
            UpdateSelectedCommand = new RelayCommand(UpdateOrder, CanUpdate);
            AddOrderCommand = new RelayCommand(AddOrder, СanAdd);
            _permissions = permissions;
        }

        private bool СanAdd()
        {
            return _permissions.CanWrite;
        }

        public void AddOrder()
        {
            var viewmodel = new AddOrderViewModel("Movements",SelectedOrder == null ? null : SelectedOrder.EmployeeName);
            viewmodel.MovementCreated += Add;
            Window window = new AddMovementWindow { DataContext = viewmodel };
            window.ShowDialog();
        }

        private void Add(Object sender, MovementOrder movement)
        {
            Orders.Add(movement);
        }

        public void UpdateOrder()
        {
            try
            {
                _model.UpdateMovement(SelectedOrder);
                MessageBox.Show("Данные обновлены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обновить данные." + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUpdate()
        {
            return SelectedOrder != null && _permissions.CanEdit;
        }

        private bool CanDelete()
        {
            return SelectedOrder != null && _permissions.CanDelete;
        }

        public void DeleteOrder()
        {
            try
            {
                _model.DeleteMovement(SelectedOrder.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить данные." + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Orders.Remove(SelectedOrder);
            SelectedOrder = null;
        }

        public void Search()
        {

            try
            {
                Orders = new ObservableCollection<MovementOrder>(_model.SearchMovement(SearchText));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
