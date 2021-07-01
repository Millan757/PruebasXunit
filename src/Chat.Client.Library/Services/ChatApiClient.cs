using Chat.Common.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Library.Services
{
    public class ChatApiClient : IChatApiClient
    {
        private readonly HttpClient _httpClient;

        public ChatApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> SendMessageAsync(ChatMessage message)
        {
            var response = await _httpClient.PostAsync("ChatMessages", new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync()
        {
            var response = await _httpClient.GetAsync("ChatMessages");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ChatMessage>>(content);
            return result;
        }
    }
}
