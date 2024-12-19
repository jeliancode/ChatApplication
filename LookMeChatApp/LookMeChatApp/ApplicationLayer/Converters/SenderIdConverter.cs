using LookMeChatApp.Domain.Model;
using LookMeChatApp.Infraestructure.Repositories;
using LookMeChatApp.Infraestructure.Services;
using Microsoft.UI.Xaml.Data;

namespace LookMeChatApp.ApplicationLayer.Converters;

public class SenderIdConverter : IValueConverter
{
    private readonly AccountSessionService _sessionService;
    private readonly FriendRepository friendRepository;
    private string senderName;

    public SenderIdConverter()
    {
        _sessionService = new AccountSessionService();
        friendRepository = App.SQLiteDb.FriendRepository;
    }
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        
        string senderId = value.ToString();
        string currentUserId = _sessionService.GetCurrentUserId().ToString();
        if (senderId == currentUserId)
        {
            return "You";
        }
        
        string? senderName = GetById(Guid.Parse(senderId)).GetAwaiter().GetResult();
        if (senderName != null)
        {
            return senderName;
        }

        return senderId;
    }



    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    private async Task<string?> GetById(Guid id)
    {
        var contact = await friendRepository.FindByIdAsync(id);
        return contact?.Username;
    }
}
