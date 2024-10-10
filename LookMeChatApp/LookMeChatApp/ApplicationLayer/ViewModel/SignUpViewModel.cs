using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Model;

namespace LookMeChatApp.ApplicationLayer.ViewModel;
public class SignUpViewModel
{
    private readonly INavigation _navigation;

    private const string defaulDescription = "Hello! look me ;)";
    private readonly IUserRepository _userRepository;
    private readonly AccountSessionService _accountSessionService;
    public string Username { get; set; }
    public string Password { get;   set; }
    public string ErrorMessage { get; set; }
    public ICommand SignUpCommand { get; }
    public ICommand MoveToLoginCommand { get; }

    public SignUpViewModel(INavigation navegation, IUserRepository userRepository)
    {
        _navigation = navegation;
        _userRepository = userRepository;
        _accountSessionService = new AccountSessionService();
        SignUpCommand = new RelayCommand(ExecuteSignUpCommand);
        MoveToLoginCommand = new RelayCommand(ExecureMoveToLoginCommand);
    }

    private async void ExecuteSignUpCommand()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Empty fields";
            return;
        }

        var user = CreateNewUser();
        if (user != null)
        {           
            try
            {
                RegisterUserOnDb(user);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: "+e.Message);
            }
            _navigation.NavigateTo("Main");
        }
    }

    private void ExecureMoveToLoginCommand()
    {
        _navigation.NavigateTo("Login");
    }

    private User CreateNewUser()
    {
        var user = new User
        {
            IdUser = Guid.NewGuid(),
            Username = Username,
            Password = Password,
            Description = defaulDescription
        };

        return user;
    }

    private async Task RegisterUserOnDb(User user)
    {
        _accountSessionService.SetCurrentUserId(user.IdUser);
        await _userRepository.AddUserAsync(user);       
    }
}
