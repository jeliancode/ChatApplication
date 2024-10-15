using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Model;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace LookMeChatApp.ApplicationLayer.ViewModel;
public class SignUpViewModel
{
    private readonly INavigation _navigation;

    private const string defaulDescription = "Hello! look me ;)";
    private readonly IUserRepository _userRepository;
    private readonly AccountSessionService _accountSessionService;
    private HashService hashService;
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
        hashService = new HashService();
 
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
                await RegisterUserOnDb(user);
                _navigation.NavigateTo("Rooms");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }  
        }
    }

    private void ExecureMoveToLoginCommand()
    {
        _navigation.NavigateTo("Login");
    }

    private User CreateNewUser()
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        var hashedPassword = hashService.HashPassword(Password, salt);

        var user = new User
            {
                IdUser = Guid.NewGuid(),
                Username = Username,
                Password = Convert.ToBase64String(hashedPassword),
                Description = defaulDescription,
                Salt = Convert.ToBase64String(salt)
        };

        return user;
    }

    private async Task RegisterUserOnDb(User user)
    {
        _accountSessionService.SetCurrentUserId(user.IdUser);
        _accountSessionService.SetCurrentUsername(user.Username);
        await _userRepository.AddUserAsync(user);       
    }
}
