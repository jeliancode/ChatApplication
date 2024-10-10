using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.Infraestructure.Services;
public class NavigationService : INavigation
{

    private readonly Frame _frame;
    public NavigationService(Frame frame)
    { 
        _frame = frame;
    }
    public void ComeBack()
    {
        if (_frame.CanGoBack)
        {
            _frame.GoBack();
        }
    }

    public void NavigateTo(string pageKey)
    {
        Type pageType = Type.GetType($"LookMeChatApp.Presentation.View.{pageKey}Page");
        if (pageType != null)
        {
            _frame.Navigate(pageType);
        }
    }
}
