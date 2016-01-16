using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimaruUWP.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace _9padv4.Converters
{
    public class MarkToForeground : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var diemchu = (string)value;
            diemchu = diemchu.Replace("+", "");
            switch (diemchu)
            {
                case "A":
                    return new SolidColorBrush(Colors.Green);
                case "B":
                    return new SolidColorBrush(Colors.Teal);
                case "C":
                    return new SolidColorBrush(Colors.Black);
                case "D":
                    return new SolidColorBrush(Colors.Orange);
                case "F":
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Black);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
