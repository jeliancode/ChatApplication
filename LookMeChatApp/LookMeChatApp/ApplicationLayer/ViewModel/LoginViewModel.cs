using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class LoginViewModel
{
    private readonly INavigation _navigation;
    public ICommand LoginCommand { get; }
    public ICommand MoveToSignUpCommand { get; }

    public LoginViewModel(INavigation navegation) 
    {
        _navigation = navegation;
        LoginCommand = new RelayCommand(ExecuteLoginCommand);
        MoveToSignUpCommand = new RelayCommand(ExecuteMoveToSignUpCommand);
    }

    private void ExecuteLoginCommand()
    {
        _navigation.NavigateTo("Main");
    }

    private void ExecuteMoveToSignUpCommand()
    {
        _navigation.NavigateTo("SignUp");
    }
}
