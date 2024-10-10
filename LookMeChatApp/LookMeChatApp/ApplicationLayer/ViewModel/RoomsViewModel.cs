using System.Collections.ObjectModel;
using System.ComponentModel;
using LookMeChatApp.Domain.Model;
using Microsoft.UI.Xaml.Controls;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class RoomsViewModel
{
    public ObservableCollection<Room> ChatRooms { get; set; }
    public ICommand CreateRoomCommand { get; }
    public ICommand JoinRoomCommand { get; }
    public ICommand NewChatCommand { get; }
    public ICommand LogoutCommand { get; }
    private string defaultRoom = "messages";

    public event PropertyChangedEventHandler PropertyChanged;

    public RoomsViewModel()
    {
        ChatRooms = new ObservableCollection<Room>
        {
            new Room
            { 
                Id = Guid.NewGuid(),
                RoomName = defaultRoom,
            }
        };

        CreateRoomCommand = new RelayCommand(CreateRoom);
        JoinRoomCommand = new RelayCommand(JoinRoom);
        NewChatCommand = new RelayCommand(NewChat);
        LogoutCommand = new RelayCommand(Logout);
    }

    private void CreateRoom()
    {
    }

    private void JoinRoom()
    {
    }

    private void NewChat()
    {
    }

    private void Logout()
    {
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
