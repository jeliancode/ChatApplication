using System;
using System.Collections.Generic;
using System.ComponentModel;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Infraestructure.Repositories;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.ApplicationLayer.Use_Cases;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class ContactViewModel : INotifyPropertyChanged
{
    private readonly FriendRepository friendRepository;
    private readonly INavigation navigation;
    private readonly SQLiteDb sQLiteDb;
    private readonly AddContactService addContactService;
    private readonly TopicSessionService topicSessionService;
    private readonly AccountSessionService accountSessionService;
    public string ContactName { get; set; }
    public Guid CurrentContactId { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly ConnectClientUseCase<Friend> connectClientUseCase;
    private readonly SendMessageUseCase<Friend> sendMessageUseCase;
    public ICommand SaveCommand { get; }

    public ContactViewModel()
    {
        sQLiteDb = App.SQLiteDb;
        navigation = App.NavigationService;
        var connectionManager = new ConnectionHandler<Friend>();
        addContactService = new AddContactService();
        topicSessionService = new TopicSessionService();
        accountSessionService = new AccountSessionService();
        connectClientUseCase = new ConnectClientUseCase<Friend>(connectionManager);
        sendMessageUseCase = new SendMessageUseCase<Friend>(connectionManager); 
        SaveCommand = new RelayCommand(SaveContact);
        friendRepository = sQLiteDb.FriendRepository;
        CurrentContactId = addContactService.GetCurrentContactId(); 
    }

    private async void SaveContact()
    {
        if (!string.IsNullOrEmpty(ContactName))
        {
            string version = topicSessionService.GetCurrentVersion();
            string room = "contact";
            string user = accountSessionService.GetCurrentUsername(); 
            string topic = $"/{version}/room/{user}/{room}";

            var newFriend = new Friend
            {
                Id = CurrentContactId,
                Username = ContactName
            };

            ContactName = string.Empty;
            OnPropertyChanged(nameof(ContactName));
            await connectClientUseCase.ExecuteAsync();
            await sendMessageUseCase.ExecuteAsync(newFriend, topic);
            await friendRepository.AddFriendAsync(newFriend);

        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
