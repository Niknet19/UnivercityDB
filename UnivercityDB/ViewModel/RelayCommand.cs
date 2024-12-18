using System;
using System.Windows.Input;

namespace UnivercityDB.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeWithParam;
        private readonly Action _executeWithoutParam;
        private readonly Func<object, bool> _canExecuteWithParam;
        private readonly Func<bool> _canExecuteWithoutParam;

        // Конструктор для команд с параметрами
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParam = canExecute;
        }

        // Конструктор для команд без параметров
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeWithoutParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithoutParam = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Метод для проверки, может ли команда быть выполнена (с параметром или без)
        public bool CanExecute(object parameter)
        {
            if (_executeWithParam != null)
                return _canExecuteWithParam?.Invoke(parameter) ?? true;

            return _canExecuteWithoutParam?.Invoke() ?? true;
        }

        // Метод для выполнения команды (с параметром или без)
        public void Execute(object parameter)
        {
            if (_executeWithParam != null)
                _executeWithParam(parameter);
            else
                _executeWithoutParam();
        }
    }
}
