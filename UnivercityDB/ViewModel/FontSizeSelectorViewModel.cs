using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UnivercityDB.ViewModel
{
    internal class FontSizeSelectorViewModel: BaseViewModel
    {
        private double _selectedFontSize;

        public double SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {   if (value > 6 && value < 50)
                {
                    _selectedFontSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ApplyCommand { get; set; }

        public FontSizeSelectorViewModel()
        {
            // Команда для применения значения
            _selectedFontSize = 12;
            ApplyCommand = new RelayCommand(UpdateFontSize);
        }
        
        private void UpdateFontSize()
        {
            Application.Current.Resources["GlobalFontSize"] = SelectedFontSize;
            OnPropertyChanged();
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
        }
    }
}
