﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class AccommodationTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((AccommodationType)value)
            {
                case AccommodationType.APARTMENT:
                    return "Appartment";
                case AccommodationType.HOUSE:
                    return "House";
                case AccommodationType.HUT:
                    return "Hut";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
