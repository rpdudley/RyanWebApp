using System.Net.Http.Json;

namespace RyanWebApp.Client.Services
{
    public class CipherServiceClient
    {
        private readonly HttpClient _httpClient;

        public CipherServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Encrypt(string message)
        {
            var response = await _httpClient.PostAsJsonAsync("api/cipher/encrypt", message);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Decrypt(string encryptedMessage, string token)
        {
            var response = await _httpClient.PostAsJsonAsync("api/cipher/decrypt", new { encryptedMessage, token});
            return await response.Content.ReadAsStringAsync();
        }
    }

}
