using System;
using System.Globalization;

namespace MQTTTest.WPF.ValueConverters
{
    public class TemperatureValueConverter : BaseValueConverter
    {

        public string Suffix { get; set; } = "°C";

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round((double)value, 1) + Suffix;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.Parse(value.ToString().Replace(Suffix, ""));
        }

    }
}