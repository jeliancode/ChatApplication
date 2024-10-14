using System;
using System.Collections.Generic;
using System.ComponentModel;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Infraestructure.Repositories;
using LookMeChatApp.Infraestructure.Services;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class ContactViewModel : INotifyPropertyChanged
{
    private readonly FriendRepository _friendRepository;
    private readonly INavigation _navigation;
    private readonly SQLiteDb sQLiteDb;
    private AddContactService addContactService;
    public string ContactName { get; set; }
    public Guid CurrentContactId { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand SaveCommand { get; }

    public ContactViewModel()
    {
        sQLiteDb = App.SQLiteDb;
        _friendRepository = sQLiteDb.FriendRepository;
        _navigation = App.NavigationService;
        addContactService = new AddContactService();
        SaveCommand = new RelayCommand(SaveContact);
        CurrentContactId = addContactService.GetCurrentContactId();
    }

    private async void SaveContact()
    {
        if (!string.IsNullOrEmpty(ContactName))
        {

            var newFriend = new Friend
            {
                Id = CurrentContactId,
                Userame = ContactName
            };

            ContactName = string.Empty;
            OnPropertyChanged(nameof(ContactName));

            await _friendRepository.AddFriendAsync(newFriend);
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
