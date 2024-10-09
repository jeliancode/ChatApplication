using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.ApplicationLayer.ViewModel;
public class SignUpViewModel
{
    private readonly INavigation _navigation;

    public string Username { get; set; }
    public string Password { get; set; }
    public ICommand SignUpCommand { get; }
    public ICommand MoveToLoginCommand { get; }

    public SignUpViewModel(INavigation navegation)
    {
        _navigation = navegation;
        SignUpCommand = new RelayCommand(ExecuteSignUpCommand);
        MoveToLoginCommand = new RelayCommand(ExecureMoveToLoginCommand);
    }

    private void ExecuteSignUpCommand()
    {
        _navigation.NavigateTo("Main");
    }

    private void ExecureMoveToLoginCommand()
    {
        _navigation.NavigateTo("Login");
    }
}
