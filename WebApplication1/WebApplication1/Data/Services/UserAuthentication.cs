using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication1.Models;



namespace WebApplication1.Data.Services
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthentication(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;  
        }
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var search = await _userManager.FindByEmailAsync(model.Email);
            Status status = new Status();
            if (search == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid Login Attempt";
                return status;
            }
            if(!await _userManager.CheckPasswordAsync(search,model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Login Attempt";
                return status;
            }
            var signInRequest = await _signInManager.PasswordSignInAsync(search, model.Password, true, true);
            if(signInRequest.Succeeded)
            {
                var userRoles = await _userManager.GetUsersInRoleAsync(search.ToString());
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, search.UserName)
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", userRole.Name));

                }
                status.StatusCode = 1;
                status.Message = "Login Succeded";
            }
            else if(signInRequest.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Invalid User";

            }
            return status;  
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }

        public async Task<Status> RegistrationAsync(RegistrationModel model)
        {
            var status = new Status();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exists";
                return status;
            }
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = model.UserName,
                Name = model.Name,
                PictureUrl = "",
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Registration Failed";
                return status;
            }
            if (await _roleManager.RoleExistsAsync(model.Role)) 
            {
                await _userManager.AddToRoleAsync(applicationUser, model.Role);
            }
            status.StatusCode = 1;
            status.Message = "Registration Successful";
            return status;
        }
    }
}
