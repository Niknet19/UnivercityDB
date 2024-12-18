using System.Linq;
using System.Windows;
using System.Windows.Input;
using UnivercityDB.Model;
using UnivercityDB.View;

namespace UnivercityDB.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;

        private string _userName;

       private UserPermissions _userPermissions;

        public UserPermissions UserPermissions
        {
            get => _userPermissions;
            set
            {
                _userPermissions = value;
                OnPropertyChanged(nameof(UserPermissions));
            }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Команда для обработки выбора пункта меню
        public ICommand MenuCommand { get; set; }

        public MainViewModel(string userName)
        {
            UserModel model = new UserModel();
            CurrentView = new AboutProgram();
            MenuCommand = new RelayCommand(OnMenuItemSelected);
            _userName = userName;
            UserPermissions = model.GetMenuPermissions(_userName);
            // Начальное содержимое (например, настройки)
            //CurrentView = new CitiesViewModel();
        }

        // Метод, который вызывается при выборе пункта меню
        private void OnMenuItemSelected(object parameter)
        {
            if (parameter is string selectedMenu)
            {
                switch (selectedMenu)
                {
                    case "City":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.CitiesPermission) };
                        break;
                    case "Street":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.StreetsPermission) };
                        break;
                    case "Departments":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.DepartmentsPermission) };
                        break;  
                    case "Speciality":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.SpecialityPermission) };
                        break;
                    case "AcademicDegree":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.AcademicDegreesPermission) };
                        break;
                    case "AcademicTitle":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.AcademicTitlesPermission) };
                        break;
                    case "Education":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel("educationalinstitution", UserPermissions.EducationPermission) };
                        break;
                    case "Penaltytype":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.OrdersPermission) };
                        break;
                    case "Promotiontype":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.OrdersPermission) };
                        break;
                    case "Jobtitles":
                        CurrentView = new Catalog() { DataContext = new CatalogsViewModel(selectedMenu, UserPermissions.JobtitlesPermission) };
                        break;
                    case "Movements":
                        CurrentView = new OrderWindow() { DataContext = new MovementOrderViewModel(UserPermissions.OrdersPermission) };
                        break;
                    case "Penalties":
                        CurrentView = new OrderWindow() { DataContext = new OrderViewModel(selectedMenu,UserPermissions.OrdersPermission) };
                        break;
                    case "Promotions":
                        CurrentView = new OrderWindow() { DataContext = new OrderViewModel(selectedMenu, UserPermissions.OrdersPermission) };
                        break;
                    case "Orders":
                        CurrentView = new OrdersOptions { DataContext = new OrdersOptionsViewModel(this) };
                        break;
                    case "Employee":
                        CurrentView = new EmployeeWindow() { DataContext = new EmployeeViewModel(UserPermissions.EmployeePermission) };
                        break;
                    case "AddEmployee":
                        CurrentView = new EmployeeWindow() { DataContext = new EmployeeViewModel(UserPermissions.EmployeePermission) };
                        Window addWindow = new AddEmployee();
                        addWindow.ShowDialog();
                        break;
                    case "Documents":
                        CurrentView = new DocumentWindow();
                        break;
                    case "Content":
                        CurrentView = new ContentWindow();
                        break;
                    case "About":
                        CurrentView = new AboutProgram();
                        break;
                    case "UpdatePassword":
                        Window passWindow = new UpdatePasswordsWindow(_userName);
                        passWindow.ShowDialog();
                        break;
                    case "UpdateFontSize":
                        Window fontWindow = new FontSizeWindow { DataContext = new FontSizeSelectorViewModel()};
                        fontWindow.ShowDialog();
                        break;
                }
            }
        }

    }

}
