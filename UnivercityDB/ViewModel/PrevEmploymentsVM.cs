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
    internal class PrevEmploymentsVM: BaseViewModel
    {

        private ObservableCollection<PreviousEmployment>? _employments;
        private PrevEmploymentsModel _model;
        private int _employeeId;
        private Permission _permission;
        public PreviousEmployment ?SelectedEmployment { get; set; }

        public string SearchText { get; set; }

        public string FullName { get; set; }

        public ObservableCollection<PreviousEmployment> Employments
        {
            get => _employments;
            set
            {
                _employments = value;
                OnPropertyChanged(nameof(Employments));
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand UpdateSelectedCommand { get; set; }

        public ICommand DeleteSelectedCommand { get; set; }

        public ICommand AddEmploymentCommand { get; set; }

        public PrevEmploymentsVM(int employeeId,string employeeName, Permission permissions)
        {
            _employeeId = employeeId;
            _permission = permissions;
            FullName = employeeName;
            _model = new PrevEmploymentsModel();
            try
            {
                Employments = new ObservableCollection<PreviousEmployment>(_model.GetPreviousEmploymentsByEmployeeId(employeeId));
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            UpdateSelectedCommand = new RelayCommand(UpdateEmployment, CanUpdateEmployment);
            AddEmploymentCommand = new RelayCommand(AddEmployment, CanAddEmployment);
            DeleteSelectedCommand = new RelayCommand(DeleteEmployment, CanUpdateEmployment);
            SearchCommand = new RelayCommand(Search,CanSearch);
        }

        private bool CanSearch()
        {
            return SearchText != null;
        }

        public void Search()
        {
            Employments = new ObservableCollection<PreviousEmployment>(_model.SearchEmployemntsByCompanyName(SearchText,_employeeId));
        }


        public bool CanUpdateEmployment()
        {
            return SelectedEmployment != null && _permission.CanEdit;
        }

        public bool CanAddEmployment()
        {
            return _permission.CanWrite;
        }


        public void UpdateEmployment()
        {

            try
            {
                _model.UpdatePreviousEmployment(SelectedEmployment);
                MessageBox.Show("Данные обновлены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message, "Оши.ка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void DeleteEmployment()
        {

            try
            {
                _model.DeletereviousEmployment(SelectedEmployment);
                Employments.Remove(SelectedEmployment);
                SelectedEmployment = null;
                MessageBox.Show("Данные удалены", "ОК", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateView()
        {
            Employments = new ObservableCollection<PreviousEmployment> (_model.GetPreviousEmploymentsByEmployeeId(_employeeId));
            OnPropertyChanged(nameof(Employments));
        }
        public void AddEmployment()
        {
            AddPrevEmploymentVM viewModel = new AddPrevEmploymentVM(_employeeId);
            viewModel.UpdateRequested += UpdateView;
            Window window = new AddPrevEmployment { DataContext = viewModel };
            window.ShowDialog();
        }


        /*public void SearchEmployee()
        {
            Employments = new ObservableCollection<PreviousEmployment>(_model.S(SearchText));
        }*/



    }
}
