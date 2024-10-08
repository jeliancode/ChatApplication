using Microsoft.UI.Xaml.Data;

namespace LookMeChatApp.ApplicationLayer.Converters;
public class HorizontalAlignmentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isSentByUser = (bool)value;
        return isSentByUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
