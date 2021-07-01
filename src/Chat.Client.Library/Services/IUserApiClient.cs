using Chat.Common.Entities;
using System.Threading.Tasks;

namespace Chat.Client.Library.Services
{
    public interface IUserApiClient
    {
        Task<ChatUser> CreateUserAsync(string username, string password);
        Task<ChatUser> LoginAsync(string username, string password);
    }
}
