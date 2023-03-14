﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Display()
        {
            return View();
        }
    }
}
