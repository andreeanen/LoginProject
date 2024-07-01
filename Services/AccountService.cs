using LoginProject.Models;
using LoginProject.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;


namespace LoginProject.Services
{
    public class AccountService : IAccountService 
    { 
        public async Task<string?> GetToken(LoginViewModel loginViewModel)
        {
            using (var client = new HttpClient())
            {
                var url = "https://services2.i-centrum.se/recruitment/auth";
                var json = JsonConvert.SerializeObject(loginViewModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);
                return jsonResponse["token"]?.ToString() ?? string.Empty;       
            }
        }

        public async Task<string> GetImageString(string token)
        {
            using (var client = new HttpClient())
            {
                var url = "https://services2.i-centrum.se/recruitment/profile/avatar";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);
                return jsonResponse["data"]?.ToString() ?? string.Empty;
            }
        }

        public async Task<UserViewModel> GetUser(string userName, string token)
        {
            var base64Image = await GetImageString(token) ?? string.Empty;

            return new UserViewModel
            {
                UserName = userName,
                ImageBase64 = base64Image,
            };
        }
    }
}
