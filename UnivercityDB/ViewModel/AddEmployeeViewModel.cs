using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        // Поля для каждого свойства Employee
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public List<char> Gender { get; set; }
        public char SelectedGender { get; set; }
        public string BirthDate { get; set; }
        public string Phone { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssueDate { get; set; }
        public string IssuedBy { get; set; }
        public List<string> AcademicDegreeName { get; set; }
        public string SelectedAcademicDegree { get; set; }
        public List<string> AcademicTitleName { get; set; }
        public string SelectedAcademicTitle { get; set; }
        public List<string> EducationalInstitutionName { get; set; }
        public string SelectedEducationalInstitution { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string HouseNumber { get; set; }
        public List<string> SpecialtyName { get; set; }
        public string SelectedSpeciality { get; set; }
        public List<string> DocumentTypeName { get; set; }
        public string SelectedDocumentType { get; set; }
        public string EmploymentStartDate { get; set; }
        public string GraduationYear { get; set; }
        public string EducationDocumentData { get; set; }

        public ICommand SaveCommand { get; set; }

        public event EventHandler<Employee> EmployeeCreated;


        public AddEmployeeViewModel(Employee? employee = null)
        {
            CatalogsModel model = new CatalogsModel();
            SaveCommand = new RelayCommand(SaveEmployee, CanSaveEmployee);
            AcademicDegreeName = model.GetNamesFromTable("academicdegree");
            AcademicTitleName = model.GetNamesFromTable("academictitle");
            Gender = new List<char> { 'М', 'Ж' };
            EducationalInstitutionName = model.GetNamesFromTable("EducationalInstitution");
            SpecialtyName = model.GetNamesFromTable("speciality");
            DocumentTypeName = model.GetNamesFromTable("documenttype");
              
            InitializeFieldsFromEmployee(employee);
            
        }

        // Проверка возможности сохранить объект
        private bool CanSaveEmployee(object parameter)
        {
            // Пример минимальной валидации: проверка обязательных полей
            return !string.IsNullOrWhiteSpace(Surname) &&
                   !string.IsNullOrWhiteSpace(FirstName) &&
                   DateTime.TryParse(BirthDate, out _) &&
                   DateTime.TryParse(PassportIssueDate, out _) &&
                   DateTime.TryParse(EmploymentStartDate, out _);
        }

        // Создание объекта Employee и обработка сохранения
        private void SaveEmployee(object parameter)
        {

            var employee = new Employee
            {
                Surname = Surname,
                FirstName = FirstName,
                Patronymic = Patronymic,
                BirthDate = DateTime.Parse(BirthDate),
                Phone = Phone,
                Gender = SelectedGender,
                PassportSeries = int.Parse(PassportSeries),
                PassportNumber = int.Parse(PassportNumber),
                PassportIssueDate = DateTime.Parse(PassportIssueDate),
                IssuedBy = IssuedBy,
                AcademicDegreeName = SelectedAcademicDegree,
                AcademicTitleName = SelectedAcademicTitle,
                EducationalInstitutionName = SelectedEducationalInstitution,
                StreetName = StreetName,
                CityName = CityName,
                HouseNumber = HouseNumber,
                SpecialtyName = SelectedSpeciality,
                DocumentTypeName = SelectedDocumentType,
                EmploymentStartDate = DateTime.Parse(EmploymentStartDate),
                GraduationYear = int.Parse(GraduationYear),
                EducationDocumentData = EducationDocumentData
            };

            EmployeeCreated?.Invoke(this, employee);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();

        }


        private void InitializeFieldsFromEmployee(Employee? employee)
        {
            // Инициализация строковых свойств
            Surname = employee != null ? employee.Surname : "Иванов";
            FirstName = employee != null ?  employee.FirstName : "Иван";
            Patronymic = employee != null ? employee.Patronymic : "Иванович";
            Phone = employee != null ? employee.Phone : "+79131111111";
            PassportSeries = employee != null ? employee.PassportSeries.ToString() : "1111";
            PassportNumber = employee != null ? employee.PassportNumber.ToString() : "111111";
            PassportIssueDate = employee != null ? employee.PassportIssueDate.ToShortDateString() : DateTime.Now.ToShortDateString();
            IssuedBy = employee != null ? employee.IssuedBy : "МВД РФ";
            StreetName = employee != null ? employee.StreetName : "Ленина";
            CityName = employee != null ? employee.CityName : "Новосибирск";
            HouseNumber = employee != null ? employee.HouseNumber : "221/2";
            EducationDocumentData = employee != null ? employee.EducationDocumentData : "123456";

            // Инициализация дат и других типов данных
            BirthDate = employee != null ? employee.BirthDate.ToShortDateString() : DateTime.Now.AddYears(-40).ToShortDateString();
            EmploymentStartDate = employee != null ? employee.EmploymentStartDate.ToShortDateString() : DateTime.Now.ToShortDateString();
            GraduationYear = employee != null ? employee.GraduationYear.ToString() : "2000";

            // Инициализация списков и выбранных элементов
            SelectedGender = employee != null ? employee.Gender : Gender[0];
            SelectedAcademicDegree = employee != null ? employee.AcademicDegreeName : AcademicDegreeName[0];
            SelectedAcademicTitle = employee != null ? employee.AcademicTitleName: AcademicTitleName[0];
            SelectedEducationalInstitution = employee != null ? employee.EducationalInstitutionName : EducationalInstitutionName[0];
            SelectedSpeciality = employee != null ? employee.SpecialtyName : SpecialtyName[0];
            SelectedDocumentType = employee != null ? employee.DocumentTypeName : DocumentTypeName[0];
        }


    }

}
