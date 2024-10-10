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

public sealed partial class RoomsPage : Page
{
    private readonly RoomsViewModel _roomsViewModel;
    public RoomsPage()
    {
        this.InitializeComponent();
        _roomsViewModel = new RoomsViewModel();
        this.DataContext = _roomsViewModel;
    }
}
