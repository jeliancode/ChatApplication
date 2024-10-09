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
        _viewModel = new LoginViewModel(App.NavigationService);
        this.DataContext = _viewModel;
    }
}
