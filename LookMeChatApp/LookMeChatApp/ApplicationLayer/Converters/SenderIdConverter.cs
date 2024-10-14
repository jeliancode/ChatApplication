using LookMeChatApp.Infraestructure.Services;
using Microsoft.UI.Xaml.Data;

namespace LookMeChatApp.ApplicationLayer.Converters;

public class SenderIdConverter : IValueConverter
{
    private readonly AccountSessionService _sessionService;

    public SenderIdConverter()
    {
        _sessionService = new AccountSessionService();
    }
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string senderId = value.ToString();
        string currentUserId = _sessionService.GetCurrentUserId().ToString();

        return senderId == currentUserId ? "You" : senderId;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
