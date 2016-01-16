using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimaruUWP.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace _9padv4.Converters
{
    public class LoadStateToVisibility : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var loadState = (LoadState)value;
            if (loadState == LoadState.Loaded)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
