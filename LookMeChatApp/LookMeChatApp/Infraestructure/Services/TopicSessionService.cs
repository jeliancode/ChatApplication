namespace LookMeChatApp.Infraestructure.Services;
public class TopicSessionService
{
    private string CurrentVersion = "CurrentVersion";
    private string CurrentRoomName = "CurrentRoomName";
    private readonly AccountSessionService accountSessionService;

    public TopicSessionService()
    {
        accountSessionService = new AccountSessionService();
    }

    public void SetCurrentVersion(string version)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentVersion] = version;
    }

    public void SetUserPath(string room)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentRoomName] = room;
    }

    public string GetCurrentVersion()
    {
        var currentVersion = ApplicationData.Current.LocalSettings
            .Values[CurrentVersion] as string;
        return !string.IsNullOrEmpty(currentVersion) ? currentVersion : string.Empty;
    }

    public string GetCurrentRoomName()
    {
        var currentRoomName = ApplicationData.Current.LocalSettings
            .Values[CurrentRoomName] as string;
        return !string.IsNullOrEmpty(currentRoomName)? currentRoomName : string.Empty;
    }

    public void ClearCurrentRoomName()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentVersion);
    }

    public void ClearCurrentVersion()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentRoomName);
    }
}
