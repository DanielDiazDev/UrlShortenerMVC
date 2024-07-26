using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Data.Repositories;
using UrlShortener.DTOs;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService; 

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _userService.Login(loginDTO);
            return Json(true);
        }
        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _userService.Register(registerDTO);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> LogOut() 
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
    
}
