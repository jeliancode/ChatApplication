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
        MessagesList.ItemsSource = Messages;
        _messagesManager = new MessagesManager(OnMessageReceived);

        try
        {
            _tcpClient = new TcpClient(serverIpAddress, serverPort);
            _stream = _tcpClient.GetStream();
            _messagesManager.StartReceiving(_stream);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Error al conectarse al servidor: " + ex.Message);
        }
    }

    private void SendMessage(object sender, RoutedEventArgs e)
    {
        string messageContent = MessageInput.Text;
        if (!string.IsNullOrWhiteSpace(messageContent))
        {
            if (_stream != null && _stream.CanWrite)
            {
                Message messageSent = new Message { MessageContent = messageContent, IsSentByUser = true };
                Messages.Add(messageSent);
                _messagesManager.SendMessage(_stream, messageContent);
                MessageInput.Text = "";
            }
            else
            {
                Console.WriteLine("La conexión no está disponible.");
            }
        }
    }

    private void OnMessageReceived(string message)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            Message messageReceived = new Message { MessageContent = message, IsSentByUser = false };
            Messages.Add(messageReceived);
        });
    }

    private void CloseConnection()
    {
        if (_stream != null)
        {
            _messagesManager.CloseConnection(_stream);
        }
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        CloseConnection();  // Cerrar la conexión cuando se navega fuera de la página
    }
}
