using LookMeChatApp.Infraestructure.Services;
using Microsoft.UI.Xaml.Data;

namespace LookMeChatApp.ApplicationLayer.Converters;
public class HorizontalAlignmentConverter : IValueConverter
{
    private readonly AccountSessionService _sessionService = new AccountSessionService();
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        Guid senderId = (Guid)value;
        Guid currentUserId = _sessionService.GetCurrentUserId();

        return senderId == currentUserId ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
