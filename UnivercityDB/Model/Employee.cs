using PropertyChanged;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UnivercityDB.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Employee
    {

        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Пол")]
        public char Gender { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }
        [Display(Name = "Серия паспорта")]
        public int PassportSeries { get; set; }
        [Display(Name = "Номер паспорта")]
        public int PassportNumber { get; set; }
        [Display(Name = "Дата выдачи паспорта")]
        public DateTime PassportIssueDate { get; set; }

        [Display(Name = "Паспорт выдан")]
        public string IssuedBy { get; set; }

        [Display(Name = "Академическая степень")]
        public string AcademicDegreeName { get; set; }
        [Display(Name = "Академическое звание")]
        public string AcademicTitleName { get; set; }

        [Display(Name = "Образование")]
        public string EducationalInstitutionName { get; set; }

        [Display(Name = "Улицп")]
        public string StreetName { get; set; }

        [Display(Name = "Город")]
        public string CityName { get; set; }

        [Display(Name = "Номер дома")]
        public string HouseNumber { get; set; }
        [Display(Name = "Специальность")]
        public string SpecialtyName { get; set; }
        [Display(Name = "Тип документа об образовании")]
        public string DocumentTypeName { get; set; }
        [Display(Name = "Дата начала работы")]
        public DateTime EmploymentStartDate { get; set; }
        [Display(Name = "Год выпуска")]
        public int GraduationYear { get; set; }
        [Display(Name = "Данные документа об образовании")]
        public string EducationDocumentData { get; set; }
    }

}
