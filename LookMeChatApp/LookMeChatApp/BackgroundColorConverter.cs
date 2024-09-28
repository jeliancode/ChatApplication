using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace LookMeChatApp;
public class BackgroundColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isSentByUser = (bool)value;
        return isSentByUser ? "#561887" : "#230C48";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
