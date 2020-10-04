using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp_SignalR_.Hubs{
    public class ChatHub:Hub{
        public async Task Send(string message){
            await Clients.All.SendAsync("ReceiveMessage",message);
        }
    }
}