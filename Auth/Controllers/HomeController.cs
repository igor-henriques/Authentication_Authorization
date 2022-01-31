﻿namespace Auth.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {            
            return View();
        }
    }    
}
