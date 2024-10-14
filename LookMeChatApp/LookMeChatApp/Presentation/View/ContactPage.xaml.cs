using LookMeChatApp.ApplicationLayer.ViewModel;
using LookMeChatApp.Infraestructure.Services;

namespace LookMeChatApp.Presentation.View;
public sealed partial class ContactPage : Page
{
    private ContactViewModel _contactViewModel;
    public ContactPage()
    {
        this.InitializeComponent();
        _contactViewModel = new ContactViewModel();
        this.DataContext = _contactViewModel;
    }
}
