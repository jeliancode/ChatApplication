using Microsoft.UI.Xaml.Data;

namespace LookMeChatApp.ApplicationLayer.Converters;
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
