using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LookMeChatApp.Domain.Model;
public class Friend
{
    [PrimaryKey, Unique]
    public int Id { get; set; }
    [NotNull]
    public string Userame { get; set; }
}
