using UrlShortener.DTOs;

namespace UrlShortener.Services
{
    public interface IUserService
    {
        List<UrlMappingDTO> GetUrlsOfUser();
        Task<bool> Login(LoginDTO loginDTO);
        Task<bool> Register(RegisterDTO registerDTO);
    }
}
