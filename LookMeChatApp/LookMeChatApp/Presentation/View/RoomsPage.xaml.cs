using LookMeChatApp.ApplicationLayer.ViewModel;
using LookMeChatApp.Domain.Model;


namespace LookMeChatApp.Presentation.View;
public sealed partial class RoomsPage : Page
{
    private readonly RoomsViewModel _roomsViewModel;
    public RoomsPage()
	{
        _roomsViewModel = new RoomsViewModel();
        this.DataContext = _roomsViewModel;
        this.InitializeComponent();
        
    }

    private void RoomsList_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is Room selectedRoom)
        {
            _roomsViewModel.SelectedRoom = selectedRoom; 
            _roomsViewModel.EnterRoomCommand.Execute(null);
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = sender as ComboBox;

        if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedVersion = selectedItem.Content?.ToString();

            if (!string.IsNullOrEmpty(selectedVersion))
            {
                _roomsViewModel.SelectedVersion = selectedVersion;
            }
        }
    }
}
