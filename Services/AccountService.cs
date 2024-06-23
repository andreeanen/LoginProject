using LoginProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;


namespace LoginProject.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetToken(LoginViewModel loginViewModel)
        {
            var url = "https://services2.i-centrum.se/recruitment/auth";
            var json = JsonConvert.SerializeObject(loginViewModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);
            return jsonResponse["token"]?.ToString() ?? string.Empty;       
        }

        public async Task<string> GetImageString(string token)
        {
            var url = "https://services2.i-centrum.se/recruitment/profile/avatar";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);
            return jsonResponse["data"]?.ToString() ?? string.Empty;
        }
    }
}
