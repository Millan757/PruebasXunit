using System;
using System.Threading.Tasks;

namespace Chat.Client.Library.Services
{
    public interface IChatClient : IDisposable
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> CreateUserAsync(string username, string password);
        void Connect();
        Task<bool> SendMessageAsync(string message);
        bool IsConnected { get; }
        event EventHandler<string> NewMessageRecived;
        event EventHandler OverwriteLastLine;
    }
}
