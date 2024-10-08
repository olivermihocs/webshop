﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DrinkStore.Desktop.View.Converters
{
    public class PackagingConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // ellenőrizzük az értéket
            if (value == null || !(value is Int32))
                return Binding.DoNothing; // ha nem megfelelő, nem csinálunk semmit

            // ellenőrizzük a paramétert
            if (parameter == null || !(parameter is IList<String>))
                return Binding.DoNothing;

            return (parameter as IList<String>)[(Int32)value];
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            // a visszaalakítással nem törődünk
            return DependencyProperty.UnsetValue;
        }
    }
}
