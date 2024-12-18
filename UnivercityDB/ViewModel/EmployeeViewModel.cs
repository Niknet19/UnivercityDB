using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.Model;
using UnivercityDB.View;

namespace UnivercityDB.ViewModel
{

    public class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<Employee>? _employees;
        private EmployeeModel _model;
        private Permission _permission;

        public Employee SelectedEmployee { get; set; }

        public string SearchText { get; set; }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand UpdateSelectedCommand { get; set; }

        public ICommand JobTitleCommand { get; set; }

        public ICommand DepartmentsCommand { get; set; }

        public ICommand DeleteSelectedCommand { get; set; }

        public ICommand AddEmployeeCommand { get; set; }

        public ICommand PrevEmploymentsCommand { get; set; }


        public EmployeeViewModel(Permission? permission)
        {
            _model = new EmployeeModel();
            _permission = permission;
            Employees = new ObservableCollection<Employee>(_model.GetAllEmployees());
            SearchCommand = new RelayCommand(SearchEmployee);
            AddEmployeeCommand = new RelayCommand(AddEmployee, CanAddEmployee);
            UpdateSelectedCommand = new RelayCommand(UpdateEmployee, CanUpdateEmployee);
            DeleteSelectedCommand = new RelayCommand(DeleteEmployee, CanUpdateEmployee);
            PrevEmploymentsCommand = new RelayCommand(ShowPrevEmployments, CanUpdateEmployee);
            JobTitleCommand = new RelayCommand(ShowJobtitles, CanUpdateEmployee);
            DepartmentsCommand = new RelayCommand(ShowDepartments, CanUpdateEmployee);
        }

        public bool CanUpdateEmployee()
        {
            return SelectedEmployee != null && _permission.CanEdit;
        }

        private bool CanAddEmployee()
        {
            return _permission.CanWrite;
        }


        public void ShowJobtitles()
        {
            Window window = new EmployeeInfoWindow { DataContext = new EmployeeInfoViewModel(SelectedEmployee, "Jobtitles") };
            window.Show();
        }

        public void ShowDepartments()
        {
            Window window = new EmployeeInfoWindow { DataContext = new EmployeeInfoViewModel(SelectedEmployee, "Departments") };
            window.Show();
        }


        public void ShowPrevEmployments()
        {
            Window window = new PrevEmploymentsWindow { DataContext = new PrevEmploymentsVM(SelectedEmployee.Id,
                Utils.MakeFullName(SelectedEmployee.FirstName,SelectedEmployee.Surname,SelectedEmployee.Patronymic), _permission) };
            window.Show();
        }

        public void UpdateEmployee()
        {

            var viewmodel = new AddEmployeeViewModel(SelectedEmployee);
            Window window = new AddEmployee { DataContext = viewmodel };
            viewmodel.EmployeeCreated += UpdateDB;
            window.ShowDialog();

            /*  MessageBox.Show("Данные обновлены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
          }
          catch (Exception ex)
          {
              MessageBox.Show("Ошибка обновления данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
          }*/

        }

        private void UpdateDB(object sender, Employee updatedEmployee)
        {
            try
            {
                updatedEmployee.Id = SelectedEmployee.Id;
                _model.UpdateEmployee(updatedEmployee);
                var oldEmployee = Employees.FirstOrDefault(e => e.Id == SelectedEmployee.Id);
                if (oldEmployee != null)
                {
                    var index = Employees.IndexOf(oldEmployee);
                    Employees[index] = updatedEmployee;
                }

                SelectedEmployee = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteEmployee()
        {

            try
            {
                _model.DeleteEmployee(SelectedEmployee.Id);
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddEmployee()
        {
            var viewmodel = new AddEmployeeViewModel();
            Window window = new AddEmployee { DataContext = viewmodel };
            viewmodel.EmployeeCreated += AddToDB;
            window.ShowDialog();
        }

        private void AddToDB(object sender, Employee newEmployee)
        {
            try
            {
                _model.AddEmployee(newEmployee);
                Employees.Add(newEmployee);
            }catch(Exception ex)
            {
                MessageBox.Show("Не удалось добавить сотрудника: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void SearchEmployee()
        {
            Employees = new ObservableCollection<Employee>(_model.SearchEmployees(SearchText));
        }




    }
}
