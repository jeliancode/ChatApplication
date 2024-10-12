using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Model;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class LoginViewModel
{
    private readonly INavigation _navigation;
    private readonly IUserRepository _userRepository;
    private readonly AccountSessionService _accountSessionService;
    public string Username { get; set; }
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
    public ICommand LoginCommand { get; }
    public ICommand MoveToSignUpCommand { get; }

    public LoginViewModel(INavigation navegation, IUserRepository userRepository) 
    {
        _navigation = navegation;
        _userRepository = userRepository;
        _accountSessionService = new AccountSessionService();
        LoginCommand = new RelayCommand(ExecuteLoginCommand);
        MoveToSignUpCommand = new RelayCommand(ExecuteMoveToSignUpCommand);
    }

    private async void ExecuteLoginCommand()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Empty fields";
            return;
        }

        var user = await GetUserByUsername(Username);
        if (user == null)
        {
            ErrorMessage = "User not foud";
            return;
        }

        if (user.Password != Password)
        {
            ErrorMessage = "Incorrect password";
            return;
        }

        _accountSessionService.SetCurrentUsername(user.Username);
        _accountSessionService.SetCurrentUserId(user.IdUser);
        _navigation.NavigateTo("Rooms");
    }

    private void ExecuteMoveToSignUpCommand()
    {
        _navigation.NavigateTo("SignUp");
    }

    private async Task<User?> GetUserByUsername(string username)
    {
        return await _userRepository.FindByUsernameAsync(username);
    }
}
