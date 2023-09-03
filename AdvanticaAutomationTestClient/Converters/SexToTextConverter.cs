using AdvanticaAutomationTestClient.Localization;
using System;
using System.Globalization;
using System.Windows.Data;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Converters
{
    public class SexToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var workerSex = (Sex)value;
            var result = string.Empty;

            switch(workerSex)
            {
                case Sex.Male: result = Resources.MainMale; break;
                case Sex.Female: result = Resources.MainFemale; break; 
                case Sex.Default: result = Resources.MainUndefined; break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
