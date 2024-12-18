using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnivercityDB.Model;

namespace UnivercityDB.ViewModel
{
    class UpdatePasswordsViewModel: BaseViewModel
    {
        public string OldPassword { get; set; } 
        public string NewPassword { get; set; }

        public string UserName { get; private set; }
        public ICommand UpdatePasswordsCommand { get; set; }

        public UpdatePasswordsViewModel(string username)
        {
            UpdatePasswordsCommand = new RelayCommand(Update, CanUpdate);
            UserName = username;
        }

        private bool CanUpdate()
        {
            return NewPassword != null && UserName != null;
        }

        private void Update()
        {
            UserModel model = new UserModel();
            try
            {
                string password=null;
               
                //model.AuthenticateUser(UserName, OldPassword);
                model.UpdateUser(UserName, NewPassword);
                Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Не удалось обновить пароль: {ex.Message}","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
