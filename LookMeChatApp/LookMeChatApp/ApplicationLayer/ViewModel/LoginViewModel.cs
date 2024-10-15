using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Model;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class LoginViewModel
{
    private readonly INavigation _navigation;
    private readonly IUserRepository _userRepository;
    private readonly AccountSessionService accountSessionService;
    private HashService hashService;
    public string Username { get; set; }
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
    public ICommand LoginCommand { get; }
    public ICommand MoveToSignUpCommand { get; }

    public LoginViewModel(INavigation navegation, IUserRepository userRepository) 
    {
        _navigation = navegation;
        _userRepository = userRepository;
        accountSessionService = new AccountSessionService();
        hashService = new HashService();
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
        
        byte [] salt = Convert.FromBase64String(user.Salt);
        var hashedPassword = hashService.HashPassword(Password, salt);

        if (!Convert.ToBase64String(hashedPassword).Equals(user.Password))
        {
            ErrorMessage = "Incorrect password";
            return;
        }

        accountSessionService.SetCurrentUsername(user.Username);
        accountSessionService.SetCurrentUserId(user.IdUser);
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
