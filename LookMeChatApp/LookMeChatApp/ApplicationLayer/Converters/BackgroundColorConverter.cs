using Microsoft.UI.Xaml.Data;
using LookMeChatApp.Infraestructure.Services;

namespace LookMeChatApp.ApplicationLayer.Converters;
public class BackgroundColorConverter : IValueConverter
{
    private readonly AccountSessionService _sessionService = new AccountSessionService();

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        Guid senderId = (Guid)value;
        Guid currentUserId = _sessionService.GetCurrentUserId();
        return senderId == currentUserId ? "#561887" : "#230C48";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
