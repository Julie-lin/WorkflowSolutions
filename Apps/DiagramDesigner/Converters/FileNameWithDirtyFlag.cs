using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Flashlight.Converters
{
    public class FileNameWithDirtyFlag : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length != 2)
                throw new InvalidOperationException("The target must provide a string and a boolean property.");

            // todo, DepencencyProperty.UnsetValue
            if (!(values[0] is String) && !(values[1] is bool))
            {
                return String.Format("Unnamed");
            }

            if (values[0] != null && !(values[0] is String))
            {
                return String.Format("The first value of the multibinding is not a string ({0}).", values[0].GetType());
            }


            string fileName = FullPathToFileNameConverter.GetFileName((string)values[0]);

            if (values[1] is bool == false)
                return String.Format("The second value of the multibinding is not a boolean ({0}).", values[1].GetType());

            bool isDirty = (bool)values[1];
            return isDirty ? String.Format("{0} *", fileName) : fileName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
