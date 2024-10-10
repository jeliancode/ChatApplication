using System;
using System.Collections.Generic;
using System.IO;
using LookMeChatApp.ApplicationLayer.ViewModel;

namespace LookMeChatApp.Presentation.View;
public sealed partial class LoginPage : Page
{

    private readonly LoginViewModel _viewModel;
    public LoginPage()
    {
        this.InitializeComponent();
        var userRepository = App.SQLiteDb.UserRepository;
        _viewModel = new LoginViewModel(App.NavigationService, userRepository);
        this.DataContext = _viewModel;
    }
}
