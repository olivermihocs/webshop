using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DrinkStore.Desktop.View.Converters
{
    public class BoolToConfirmationMessageConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // ellenőrizzük az értéket
            if (value == null || !(value is bool))
                return Binding.DoNothing; // ha nem megfelelő, nem csinálunk semmit

            bool b = (bool)value;
            if (b)
            {
                return "Visszavonás";
            }
            else
            {
                return "Megerősítés";
                
            }
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            // a visszaalakítással nem törődünk
            return DependencyProperty.UnsetValue;
        }
    }
}
