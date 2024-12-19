namespace LookMeChatApp.Infraestructure.Services;

public class AddContactService
{
    private string CurrentContactId = "CurrentContactId";

    public void SetCurrentContactId(Guid contactId)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentContactId] = contactId.ToString();
    }

    public Guid GetCurrentContactId()
    {
        var contactId = ApplicationData.Current
            .LocalSettings .Values [CurrentContactId] as string;
        return !string.IsNullOrEmpty(contactId) ? Guid.Parse(contactId) : Guid.Empty; 
    }

    public void ClearCurrentContactId()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentContactId);
    }
}
