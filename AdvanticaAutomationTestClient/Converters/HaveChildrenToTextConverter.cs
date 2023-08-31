using System;
using System.Globalization;
using System.Windows.Data;

namespace AdvanticaAutomationTestClient.Converters
{
    public class HaveChildrenToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var haveChildren = (bool)value;

            return haveChildren ? "Да" : "Нет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
