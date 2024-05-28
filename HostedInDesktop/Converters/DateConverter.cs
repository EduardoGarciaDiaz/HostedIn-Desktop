using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Converters
{
    public class DateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string dateStr && DateTime.TryParse(dateStr, out DateTime date))
            {
                return date.ToString("d MMM yyyy", CultureInfo.CurrentCulture);
            }
            return value;

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
