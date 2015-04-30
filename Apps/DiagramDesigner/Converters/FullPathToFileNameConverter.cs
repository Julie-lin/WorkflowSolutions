using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Flashlight.Converters
{
    class FullPathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string");
            return GetFileName((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Can not create the full path form the file name.");
        }

        internal static string GetFileName(string path)
        {
            if (path == null) return "Unnamed";
            return Path.GetFileName(path);
        }
    }
}
