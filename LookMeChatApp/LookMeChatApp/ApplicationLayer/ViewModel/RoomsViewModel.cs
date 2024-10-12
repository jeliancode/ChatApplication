using System.Collections.ObjectModel;
using System.ComponentModel;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.ApplicationLayer.Use_Cases;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class RoomsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Room> ChatRooms { get; set; }
    public ICommand CreateRoomCommand { get; }
    public ICommand JoinRoomCommand { get; }
    public ICommand NewChatCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand EnterRoomCommand { get; }
    private readonly INavigation _navigation;
    private string _selectedVersion;
    private Room _selectedRoom;
    private string _roomName;

    public event PropertyChangedEventHandler PropertyChanged;

    public RoomsViewModel()
    {
        ChatRooms = new ObservableCollection<Room>();
        _navigation = App.NavigationService;

        CreateRoomCommand = new RelayCommand(CreateRoom);
        JoinRoomCommand = new RelayCommand(JoinRoom);
        NewChatCommand = new RelayCommand(NewChat);
        LogoutCommand = new RelayCommand(Logout);
        EnterRoomCommand = new RelayCommand(EnterRoom);

        LoadRoomsAsync();
    }

    public string RoomName
    {
        get => _roomName;
        set
        {
            _roomName = value;
            OnPropertyChanged(nameof(RoomName));
        }
    }

    public string SelectedVersion
    {
        get => _selectedVersion;
        set
        {
            _selectedVersion = value;
            OnPropertyChanged(nameof(SelectedVersion));
        }
    }

    public Room SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            _selectedRoom = value;
            OnPropertyChanged(nameof(SelectedRoom));
        }
    }

    private async void LoadRoomsAsync()
    {
        var rooms = await App.SQLiteDb.RoomRepository.GetAllRoomsAsync();
        foreach (var room in rooms)
        {
            ChatRooms.Add(room);
        }
    }

    private void EnterRoom()
    {
        if (SelectedRoom != null && !string.IsNullOrEmpty(SelectedVersion))
        {
            _navigation.NavigateTo("Chat");
        }
    }

    private async void CreateRoom() 
    {
        if (!string.IsNullOrWhiteSpace(RoomName))
        {
            var newRoom = new Room
            {
                Id = Guid.NewGuid(),
                RoomName = RoomName,
                topicPath = GenerateConnectionPath(RoomName)
            };

            await App.SQLiteDb.RoomRepository.AddRoomAsync(newRoom);

            ChatRooms.Add(newRoom);

            RoomName = string.Empty;
        }
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

    private string GenerateConnectionPath(string roomName)
    {
        return $"/room/+/{roomName}";
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
