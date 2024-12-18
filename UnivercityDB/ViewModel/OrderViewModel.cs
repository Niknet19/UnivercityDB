using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.Model;
using UnivercityDB.View;

namespace UnivercityDB.ViewModel
{
    class OrderViewModel : BaseViewModel
    {
        private Permission _permissions;

        public string SearchText { get; set; }

        private string _orderType;
        private SanctionsOrderModel _model;

        public ObservableCollection<SanctionsOrder> Orders { get; set; }

        public SanctionsOrder? SelectedOrder { get; set; }

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

        public OrderViewModel(string orderType, Permission ?permissions)
        {
            _model = new SanctionsOrderModel();
            _orderType = orderType;
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
            var viewmodel = new AddOrderViewModel(_orderType, SelectedOrder == null ? null : SelectedOrder.EmployeeName);
            viewmodel.OrderCreated += Add;
            Window window = new AddOrderWindow { DataContext = viewmodel };
            window.ShowDialog();
        }

        private void Add(Object sender, SanctionsOrder order)
        {
            Orders.Add(order);
        }

        public void UpdateOrder()
        {
            try
            {
                _model.UpdateOrder(_orderType, SelectedOrder);
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
                _model.DeleteOrder(_orderType, ((Order)SelectedOrder).Id);
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
                Orders = new ObservableCollection<SanctionsOrder>(_model.SearchOrder(SearchText, _orderType));
                OnPropertyChanged(nameof(Orders));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
