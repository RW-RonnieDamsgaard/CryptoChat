namespace CryptoChat
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string encryptedMessage, string hmac)
        {
            // Forward the encrypted message and HMAC to all clients
            await Clients.All.SendAsync("ReceiveMessage", user, encryptedMessage, hmac);
        }
    }
}
