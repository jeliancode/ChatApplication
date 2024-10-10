using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookMeChatApp.Infraestructure.Services;
public class AccountSessionService
{
    private string CurrentUserId = "CurrentUser";

    public void SetCurrentUserId(Guid userId)
    {
        ApplicationData.Current.LocalSettings.Values[CurrentUserId] = userId.ToString();
    }

    public Guid GetCurrentUserId()
    {
        var userId = ApplicationData.Current
            .LocalSettings.Values[CurrentUserId] as string;
        return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty;
    }

    public void ClearCurrentUserId()
    {
        ApplicationData.Current.LocalSettings.Values.Remove(CurrentUserId);
    }
}
