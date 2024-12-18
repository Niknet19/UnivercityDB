using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{
    public class AddOrderViewModel : BaseViewModel
    {
        public List<string> Types { get; set; }

        private string _type;
        public string SelectedType { get; set; }
        public string FullName { get; set; }

        public string Date { get; set; }

        public string OrderNumber { get; set; }

        public string Reason { get; set; }

        public event EventHandler<SanctionsOrder> OrderCreated;
        public event EventHandler<MovementOrder> MovementCreated;
        public ICommand SaveCommand { get; set; }
        public AddOrderViewModel(string type, string? employeeName)
        {
            _type = type;
            CatalogsModel model = new CatalogsModel();
            SaveCommand = new RelayCommand(Add, CanAdd);
            if (type == "Promotions")
            {
                Types = new List<string>(model.GetNamesFromTable("promotiontype"));
            }
            if (type == "Penalties")
            {
                Types = new List<string>(model.GetNamesFromTable("penaltytype"));
            }
            InitializeProperties(employeeName);
        }

        private bool CanAdd()
        {
            return FullName != null && DateTime.TryParse(Date, out _);
        }
        public void Add()
        {
            EmployeeModel employeeModel = new();
            int eid = 0;
            try
            {
                eid = employeeModel.SearchCurrentEmployees(FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сотрудник с таким ФИО не найден","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if (_type == "Penalties" || _type == "Promotions")
            {
                SanctionsOrder order = new SanctionsOrder
                {
                    EmployeeName = FullName,
                    Date = DateTime.Parse(Date),
                    OrderNumber = OrderNumber,
                    SanctionType = SelectedType
                };
                SanctionsOrderModel orderModel = new();
                try
                {
                    orderModel.AddOrder(_type, order, eid);
                    OrderCreated?.Invoke(this, order);
                    MessageBox.Show("Успешно");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
            
            if(_type == "Movements")
            {
                MovementOrder movement = new MovementOrder
                {
                    EmployeeName = FullName,
                    Date = DateTime.Parse(Date),
                    OrderNumber = OrderNumber,
                    Reason = Reason
                };

                MovementsOrderModel model = new();
                model.AddMovement(movement, eid);
                MovementCreated?.Invoke(this, movement);
            }
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
        }

        private void InitializeProperties(string? employeeName)
        {
            FullName = employeeName != null ? employeeName : "Иванов Иван Иванович";
            SelectedType = (_type == "Movements") ? "Перемещение"  : Types[0];
            OrderNumber = "1111";
            Date = DateTime.Now.ToShortDateString();
            Reason = "Причина перемещения";
        }
    }
}
