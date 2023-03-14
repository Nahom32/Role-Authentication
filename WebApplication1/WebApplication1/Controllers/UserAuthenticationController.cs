using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Services;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserAuthenticationController : Controller
        
    {
        private readonly IUserAuthentication _service;
        public UserAuthenticationController(IUserAuthentication _service)
        {
            this._service = _service;  
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            model.Role = "user";
            var result = await _service.RegistrationAsync(model);
            if (result.StatusCode == 0)
            {
                ViewData["msg"] = result.Message;
                return RedirectToAction(nameof(Registration));
            }
            else
            {
                return RedirectToAction(nameof(Registration));
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Reg()
        //{
        //    var model = new RegistrationModel()
        //    {
        //        UserName = "admin",
        //        Name = "Max Wellington",
        //        Email = "abe123@gmail.com",
        //        Password = "Auth@Afri1918#",  

        //    };
        //    model.Role = "user";

        //    var result = await _service.RegistrationAsync(model);
        //    return Ok(result);
        //}
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _service.LoginAsync(model);
                if(result.StatusCode == 1)
                {
                    return RedirectToAction("Display", "Dashboard");
                }
                else
                {
                    TempData["msg"] = result.Message;
                    return RedirectToAction(nameof(Login));
                }

            }
            return View(model);
        }
        public async Task Logout()
        {
            await _service.LogoutAsync();
        }
    }
}
