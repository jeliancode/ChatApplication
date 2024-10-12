namespace LookMeChatApp.Infraestructure.Services;
public class AccountSessionService
{
    private string CurrentUserId = "CurrentUser";
    private string CurrentUsername = "CurrentUsername";

    public void SetCurrentUserId(Guid userId)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentUserId] = userId.ToString();
    }

    public void SetCurrentUsername(string username)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentUsername] = username;
    }

    public Guid GetCurrentUserId()
    {
        var userId = ApplicationData.Current
            .LocalSettings.Values[CurrentUserId] as string;
        return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty;
    }

    public string GetCurrentUsername()
    {
        var username = ApplicationData.Current
            .LocalSettings.Values [CurrentUsername] as string;
        return !string.IsNullOrEmpty(username)? username : string.Empty;
    }

    public void ClearCurrentUserId()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentUserId);
    }

    public void ClearCurrentUsername()
    {
        ApplicationData.Current.LocalSettings.Values.Remove (CurrentUsername);
    }
}
