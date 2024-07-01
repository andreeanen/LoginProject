using LoginProject.Models;

namespace LoginProject.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string?> GetToken(LoginViewModel loginViewModel);
        Task<string> GetImageString(string token);
        Task<UserViewModel> GetUser(string userName, string token);
    }
}
