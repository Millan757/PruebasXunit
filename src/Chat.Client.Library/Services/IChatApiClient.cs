using Chat.Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Client.Library.Services
{
    public interface IChatApiClient
    {
        Task<bool> SendMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetChatMessagesAsync();
    }
}
