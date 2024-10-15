using System.Collections.ObjectModel;
using System.ComponentModel;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Infraestructure.Repositories;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class RoomsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Room> ChatRooms { get; set; }
    public ObservableCollection<ChatMessage> Messages { get; set; }

    public ICommand CreateRoomCommand { get; }
    public ICommand JoinRoomCommand { get; }
    public ICommand NewChatCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand EnterRoomCommand { get; }
    private readonly INavigation navigation;
    private readonly SQLiteDb sQLiteDb;
    private readonly TopicSessionService topicSessionService;
    private readonly AccountSessionService accountSessionService;
    private Room _selectedRoom;
    private string _selectedVersion;
    private string _roomName;


    public event PropertyChangedEventHandler PropertyChanged;

    public RoomsViewModel()
    {
        ChatRooms = [];
        Messages = [];
        topicSessionService = new TopicSessionService();
        accountSessionService = new AccountSessionService();
        navigation = App.NavigationService;
        sQLiteDb = App.SQLiteDb;
        
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
        var roomsRepository = sQLiteDb.RoomRepository;

        var rooms = await roomsRepository.GetAllRoomsAsync();
        foreach (var room in rooms)
        {
            ChatRooms.Add(room);
        }
    }

    private async Task LoadMessagesAsync(string room)
    {
        var messagesRepository = sQLiteDb.MessageRepository;
        var messagesList = await messagesRepository.GetMessagesByRoom(room);

        Messages.Clear();

        foreach (var message in messagesList)
        {
            Messages.Add(message);

        }
    }

    private void EnterRoom()
    {
        if (SelectedRoom != null && !string.IsNullOrEmpty(SelectedVersion))
        {
            topicSessionService.SetCurrentVersion(_selectedVersion);
            topicSessionService.SetUserPath(_selectedRoom.RoomName);
            navigation.NavigateTo("Chat");
        }
    }

    private async void CreateRoom() 
    {
        if (!string.IsNullOrWhiteSpace(RoomName))
        {
            topicSessionService.ClearCurrentVersion();

            var newRoom = new Room
            {
                Id = Guid.NewGuid(),
                RoomName = _roomName,
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
        accountSessionService.ClearCurrentUserId();
        accountSessionService.ClearCurrentUsername();
        navigation.ComeBack();
    }


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
