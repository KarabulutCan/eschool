using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ESCHOOL.Hubs
{
    public class UsersChatHub : Hub
    {
        public async Task SendMessageAsync()
        {
           await Clients.All.SendAsync("receiveMessage", "Merhaba");
        }
    }
}
