namespace CryptoChat
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string encryptedMessage, string hmac)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, encryptedMessage, hmac);
        }
    }
}
