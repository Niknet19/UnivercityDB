using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private UserModel _userModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; set; }
        public ICommand AddUserCommand { get; set; }

        public LoginViewModel()
        {
            _userModel = new UserModel();
            LoginCommand = new RelayCommand(Authenticate);
            AddUserCommand = new RelayCommand(RegisterUser);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public void Authenticate(object parameter)
        {

            if (parameter is PasswordBox passwordBox)
            {
                var password = passwordBox.Password;
                bool isAuthenticated = _userModel.AuthenticateUser(Username, password);
                if (isAuthenticated)
                {
                    MessageBox.Show("Успешная авторизация!", "Успешно", MessageBoxButton.OK,MessageBoxImage.Information);
                    Window window = new MainWindow { DataContext = new MainViewModel(Username) };
                    Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
                    window.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public bool CanRegisterUser()
        {
            return Username != null;
        }



        public void RegisterUser(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                var password = passwordBox.Password;
                var userModel = new UserModel();
                bool isAdded = userModel.AddUser(Username, password);

                if (isAdded)
                {
                    MessageBox.Show("Пользователь зарегистрирован.");
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации.");
                }
            }
        }



        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
