using Chat.Client.Library.Helpers;
using Chat.Common.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client.Library.Services
{
    public class ChatClient : IChatClient
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        private ChatUser _user;

        private readonly IChatApiClient _chatApiClient;
        private readonly IUserApiClient _userApiClient;

        private int _lastDisplayedId = -1;

        public bool IsConnected { get; private set; }

        public event EventHandler<string> NewMessageRecived;
        public event EventHandler OverwriteLastLine;

        public ChatClient(IChatApiClient chatApiClient, IUserApiClient userApiClient)
        {
            _chatApiClient = chatApiClient;
            _userApiClient = userApiClient;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            _ = username ?? throw new ArgumentNullException(nameof(username));
            _ = password ?? throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await _userApiClient.LoginAsync(username, password);
            if (user is null)
            {
                return false;
            }
            else
            {
                _user = user;
                return true;
            }
        }

        public async Task<bool> CreateUserAsync(string username, string password)
        {
            _ = username ?? throw new ArgumentNullException(nameof(username));
            _ = password ?? throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await _userApiClient.CreateUserAsync(username, password);
            if (user is null)
            {
                return false;
            }
            else
            {
                _user = user;
                return true;
            }
        }

        public void Connect()
        {
            var pool = new Task(async () => await ReciveMessages(_cancellationTokenSource.Token), TaskCreationOptions.LongRunning);
            pool.Start();
            IsConnected = true;
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            if (IsConnected)
            {
                var chatMessage = new ChatMessage
                {
                    Date = DateTime.Now,
                    Author = _user.Name,
                    Message = message
                };
                return await _chatApiClient.SendMessageAsync(chatMessage);
            }
            else
            {
                return IsConnected;
            }
        }

        private async Task ReciveMessages(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var messages = await _chatApiClient.GetChatMessagesAsync();
                var filteredMessages = messages.Where(message => message.IdChatMessage > _lastDisplayedId)
                                                .OrderBy(message => message.IdChatMessage)
                                                .ToList();

                if (filteredMessages.Count == 1 && filteredMessages.Last().Author == _user.Name)
                {
                    OverwriteLastLine?.Invoke(this, null);
                }

                foreach (var message in filteredMessages)
                {
                    NewMessageRecived?.Invoke(this, ChatMessageHelper.GenerateString(message));
                    _lastDisplayedId = message.IdChatMessage;
                }

                await Task.Delay(500);
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
