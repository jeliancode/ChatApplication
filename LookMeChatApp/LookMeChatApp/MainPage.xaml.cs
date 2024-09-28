using System.Collections.ObjectModel;
using System.Net.Sockets;
using Windows.ApplicationModel.Chat;


namespace LookMeChatApp;

public sealed partial class MainPage : Page
{
    public ObservableCollection<Message> Messages { get; set; }
    private MessagesManager _messagesManager;
    private TcpClient _tcpClient;
    NetworkStream _stream;
    private const string serverIpAddress = "192.168.1.204";
    private const int serverPort = 6000;
    public MainPage()
    {
        this.InitializeComponent();
        Messages = new ObservableCollection<Message>();
        _messagesManager = new MessagesManager();
        MessagesList.ItemsSource = Messages;
        _tcpClient = new TcpClient(serverIpAddress, serverPort);
        _stream = _tcpClient.GetStream();
    }

    private void SendMessage(object sender, RoutedEventArgs e)
    {

        string messageContent = MessageInput.Text;
        if (!string.IsNullOrWhiteSpace(messageContent))
        {
            Message message = new Message { MessageContent = messageContent, IsSentByUser = true };
            Messages.Add(message);
            _messagesManager.SendMessage(_stream, message);
            MessageInput.Text = "";          
        }
    }
}
