using Chat.Common.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chat.Client.Library.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ChatUser> CreateUserAsync(string username, string password)
        {
            var message = new { username, password };
            var postContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });
            try
            {
                var response = await _httpClient.PostAsync($"ChatUsers?username={username}&password={password}", postContent);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ChatUser>(content);
                return result;
            }
            catch (HttpRequestException)
            {
                return null;
            }

        }

        public async Task<ChatUser> LoginAsync(string username, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ChatUsers?username={username}&password={password}");
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ChatUser>(content);
                return result;
            }
            catch (HttpRequestException)
            {
                return null;
            }

        }
    }
}
