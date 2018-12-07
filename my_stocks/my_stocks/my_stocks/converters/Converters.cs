using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace my_stocks.converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double netChangeValue = (Double)value;

            if (netChangeValue >= 0)
            {
                return Color.Green;
            }
            else
            {
                return Color.Red;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ValueToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double netChangeValue = (Double)value;
            String source;
            if (netChangeValue >= 0)
            {
                source =  "my_stocks.resources.arrow_up.png";
            }
            else
            {
                source = "my_stocks.resources.arrow_down.png";
            }

            return ImageSource.FromResource(source);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean selected = (Boolean)value;
            String[] colors = parameter.ToString().Split(',');

            if (selected)
                return Color.FromHex(colors[0]); //Color.FromHex("#81D4FA");

            return Color.FromHex(colors[1]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntegerToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int selected = (int)value;

            string[] colors =
            {
                "#039BE5",
                "#E53935",
                "#7CB342",
                "#FFB300"
            };

            if(selected < colors.Length)
                return Color.FromHex(colors[selected]);
                
            return Color.DarkBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
