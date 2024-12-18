using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{
    public class EmployeeInfoViewModel : BaseViewModel
    {

        private string _employeeName;

        private Employee _employee;

        private string _catalogName;
        public ObservableCollection<string> AllItems { get; set; }

        public ObservableCollection<string> EployeeItems { get; set; }

        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        public string? SelectedItem { get; set; }
        public string? SelectedListItem { get; set; }
        public ICommand AddEmploymentCommand { get; }

        public ICommand DeleteSelectedCommand { get; }

        public EmployeeInfoViewModel(Employee employee, string catalogName)
        {
            _employee = employee;
            _catalogName = catalogName;
            InitializeView();
            EmployeeName = employee.Surname + " " + employee.FirstName + " " + employee.Patronymic;
            AddEmploymentCommand = new RelayCommand(Add, CanAddEmployment);
            DeleteSelectedCommand = new RelayCommand(DeleteSelected, CanDeleteSelected);
        }

        private void InitializeView()
        {
            EmployeeModel model = new EmployeeModel();
            CatalogsModel catalogsModel = new CatalogsModel();
            if (_catalogName == "Jobtitles")
            {
                EployeeItems = new ObservableCollection<string>(model.SearchEmployeeJobtitles(_employee));
            }
            else
            {
                EployeeItems = new ObservableCollection<string>(model.SearchEmployeeDepartments(_employee));
            }
            AllItems = new ObservableCollection<string>(catalogsModel.GetNamesFromTable(_catalogName));
        }


        private void Add()
        {
            EmployeeModel model = new EmployeeModel();
            if (_catalogName == "Jobtitles")
            {
                model.AddJobtitles(_employee, SelectedItem);
                EployeeItems.Add(SelectedItem);
            }
            else
            {
                model.AddDepartments(_employee, SelectedItem);
                EployeeItems.Add(SelectedItem);
            }
            SelectedItem = null;
        }

        private bool CanAddEmployment()
        {
            return !string.IsNullOrEmpty(SelectedItem);
        }

        private void DeleteSelected()
        {
            try
            {
                EmployeeModel model = new EmployeeModel();
                if (_catalogName == "Jobtitles")
                {
                    model.DeleteJobtitles(_employee, SelectedListItem);
                    EployeeItems.Remove(SelectedListItem);
                }
                else
                {
                    model.DeleteDepartments(_employee, SelectedListItem);
                    EployeeItems.Remove(SelectedListItem);
                }
                SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanDeleteSelected()
        {
            return SelectedListItem != null;
        }
    }

}
