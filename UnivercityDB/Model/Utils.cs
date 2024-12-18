using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace UnivercityDB.Model
{
    public class Utils
    {

        public static string MakeFullName(string firstName, string surname, string patronimic)
        {
            return surname + " " + firstName + " " + patronimic;
        }
        public static (string Surname, string FirstName, string Patronymic) ParseFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                fullName = "% % %";
            }

            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            /*if (parts.Length < 2)
            {
                throw new FormatException("Full name must contain at least a surname and a first name.");
            }*/

            string surname = parts[0];
            string firstName = parts.Length > 1 ? parts[1] : string.Empty;
            string patronymic = parts.Length > 2 ? parts[2] : string.Empty;

            return (surname, firstName, patronymic);
        }
    }


    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
