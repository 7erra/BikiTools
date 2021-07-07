using BikiTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BikiTools.WPF.Converter
{
    /// <summary>
    /// Data binding between a selected property and a radio button.
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/a/20730096
    /// </remarks>
    public class HighlightOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
}
