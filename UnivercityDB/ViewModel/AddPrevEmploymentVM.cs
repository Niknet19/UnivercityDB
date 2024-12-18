using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{
    internal class AddPrevEmploymentVM: BaseViewModel
    {
        public string CompanyName { get; set; }
        public string Street { get; set;}
        public string City { get; set; }

        public List<string> Positions { get; set; }

        public string SelectedPosition { get; set; }

        public string HouseNumber { get; set; }

        public string Phone { get; set; }

        public string Reason { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        private int EmployeeId { get; set; }

        public ICommand SaveCommand { get; set; }

        public event Action UpdateRequested;

        public AddPrevEmploymentVM(int employeeId)
        {
            CatalogsModel model = new CatalogsModel();
            EmployeeId = employeeId;
            Positions = model.GetNamesFromTable("jobtitles");
            SaveCommand = new RelayCommand(SaveEmployment, CanSave);
            InitializeProperties();
        }

        private bool CanSave(object parameter)
        {
            // Пример минимальной валидации: проверка обязательных полей
            return !string.IsNullOrWhiteSpace(CompanyName) &&
                   !string.IsNullOrWhiteSpace(SelectedPosition) &&
                   DateTime.TryParse(StartDate, out _) &&
                   DateTime.TryParse(EndDate, out _);
        }

        private void SaveEmployment(object parameter)
        {
            try
            {
                var prevEmployment = new PreviousEmployment
                {
                    EmployeeId = EmployeeId,
                    City = City,
                    Street = Street,
                    HouseNumber = HouseNumber,
                    Position = SelectedPosition,
                    CompanyPhone = Phone,
                    ReasonForLeaving = Reason,
                    StartDate = DateTime.Parse(StartDate),
                    EndDate = DateTime.Parse(EndDate),
                    CompanyName = CompanyName
                };

                PrevEmploymentsModel model = new();
                model.AddPreviousEmployment(prevEmployment);
                MessageBox.Show("Успешно!", "Добавлено!", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateRequested?.Invoke();
                Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
                // Здесь можно вызвать метод для сохранения сотрудника в БД
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeProperties()
        {
            CompanyName = "Название организации";
            Street = "Ленина";
            City = "Москва";
            SelectedPosition = Positions[0]; // Устанавливаем первую должность как выбранную
            HouseNumber = "42";
            Phone = "+71234567890";
            Reason = "Уволен по собственному желанию";
            StartDate = DateTime.Now.AddYears(-1).ToShortDateString(); // Начало работы год назад
            EndDate = DateTime.Now.ToShortDateString(); // Текущая дата
        }



    }
}
