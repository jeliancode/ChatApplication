using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookMeChatApp.Model;
public class Message
{
    public string? MessageId { get; set; }
    public string? MessageContent { get; set; }
    public string? Username { get; set; }
    public string? Room { get; set; }
    public bool? IsSentByUser { get; set; }
}
