using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookMeChatApp.Domain.Interface;
public interface INavigation
{
    void NavigateTo(string pageKey);
    void ComeBack();
}
