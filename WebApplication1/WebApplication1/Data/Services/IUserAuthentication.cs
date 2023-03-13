using WebApplication1.Models;

namespace WebApplication1.Data.Services
{
    public interface IUserAuthentication
    {
        public  Task<Status> LoginAsync(LoginModel model);
        public  Task LogoutAsync();
        public Task<Status> RegistrationAsync(RegistrationModel model);


    }
}
