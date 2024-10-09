using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using LookMeChatApp.ApplicationLayer.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace LookMeChatApp.Presentation.View;
public sealed partial class SignUpPage : Page
{
    private readonly SignUpViewModel _viewModel;
    public SignUpPage()
    {
        this.InitializeComponent();

        _viewModel = new SignUpViewModel(App.NavigationService);
        this.DataContext = _viewModel;
    }
}
