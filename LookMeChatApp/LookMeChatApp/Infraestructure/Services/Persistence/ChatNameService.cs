namespace LookMeChatApp.Infraestructure.Services;

public class ChatNameService
{
    private string CurrentRoom = "CurrentRoom";

    public void SetCurrentRoom(string roomName)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentRoom] = roomName;
    }

    public string GetCurrentRoom()
    {
        return ApplicationData.Current.LocalSettings.Values[CurrentRoom] as string;
    }

    public void ClearCurrentRoom()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentRoom);
    }
}
