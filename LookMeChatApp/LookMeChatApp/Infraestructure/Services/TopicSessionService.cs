namespace LookMeChatApp.Infraestructure.Services;
public class TopicSessionService
{
    private string CurrentTopic = "CurrentTopic";
    private string CurrentUserPath = "CurrentUserPath";
    private readonly AccountSessionService accountSessionService;

    public TopicSessionService()
    {
        accountSessionService = new AccountSessionService();
    }

    public void SetCurrentTopic(string version, string room)
    {
        var newTopicPath = $"/{version}/room/+/{room}";
        ApplicationData.Current.LocalSettings.Values[CurrentTopic] = newTopicPath;
    }

    public void SetUserPath(string version, string room)
    {
        var username = accountSessionService.GetCurrentUsername();
        var newUserPath = $"/{version}/room/{username}/{room}";
        ApplicationData.Current.LocalSettings.Values[CurrentUserPath] = newUserPath;
    }
    public string GetCurrentTopic()
    {
        var currentTopicPath = ApplicationData.Current.LocalSettings
            .Values[CurrentTopic] as string;
        return !string.IsNullOrEmpty(currentTopicPath)? currentTopicPath : string.Empty;
    }

    public string GetCurrentUserPath()
    {
        var currentUserPath = ApplicationData.Current.LocalSettings
            .Values[CurrentUserPath] as string;
        return !string.IsNullOrEmpty(currentUserPath)? currentUserPath : string.Empty;
    }

    public void ClearCurrentTopicData()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentTopic);
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentUserPath);
    }
}
