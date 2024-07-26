using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using UrlShortener.Data.Repositories;
using UrlShortener.DTOs;
using UrlShortener.Models;
using System.Text;
using System.Security.Cryptography;

namespace UrlShortener.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }

        public List<UrlMappingDTO> GetUrlsOfUser()
        {
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return user.UrlMappings.Select(x=> new UrlMappingDTO(x.Id, x.OriginalUrl, x.ShortUrl, x.CheckState())).ToList();
        }

        public async Task<bool> Login(LoginDTO loginDTO)
        {
            var password = Sha256(loginDTO.Password);
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };
            await _contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return true;
        }
        public async Task<bool> Register(RegisterDTO registerDTO)
        {
            var password = Sha256(registerDTO.Password);
            var user = new User(Guid.NewGuid(), registerDTO.UserName, registerDTO.Email, password); 
            var userAdded = await _userRepository.Add(user);
            return true;
        }
        private string Sha256(string password)
        {
            var sb = new StringBuilder();
            using var hash = SHA256.Create();
            var result = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            for(int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
