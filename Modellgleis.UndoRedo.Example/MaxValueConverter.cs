using System;
using System.Globalization;
using System.Windows.Data;

namespace Modellgleis.UndoRedo.Example
{
    public class MaxValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int i && i== int.MaxValue)
            {
                return "Alle";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if("Alle".Equals(value?.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return int.MaxValue;
            }
            return value; 
        }
    }
}
