using System.Security.Claims;
using UrlShortener.Data.Repositories;
using UrlShortener.DTOs;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private const string UnauthenticatedShortenCountKey = "UnauthenticatedShortenCount";
        private const int UnauthenticationShortenLimit = 1;
        private readonly IUrlRepository _urlRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public UrlService(IUrlRepository urlRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _urlRepository = urlRepository;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<UrlMappingDTO> Shorten(string urlOriginal) 
        {
            UrlMapping urlMapping;
            var shortUrl = GenerateShortUrl();
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var shortenCount = _contextAccessor.HttpContext.Session.GetInt32(UnauthenticatedShortenCountKey) ?? 0;
                if (shortenCount >= UnauthenticationShortenLimit)
                {
                    throw new UnauthorizedAccessException("You need to be authenticated to shorten more URLs.");
                }
                _contextAccessor.HttpContext.Session.SetInt32(UnauthenticatedShortenCountKey, shortenCount + 1);
                urlMapping = new UrlMapping(Guid.NewGuid(), urlOriginal, null, shortUrl);
                var urlAdded = await _urlRepository.Add(urlMapping);
            }
            else
            {
                var userId = Guid.Parse((_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                var user = await _userRepository.GetById(userId);


                urlMapping = new UrlMapping(Guid.NewGuid(), urlOriginal, userId, shortUrl);

                var urlAdded = await _urlRepository.Add(urlMapping);
                user.UrlMappings.Add(urlMapping);
                _userRepository.Update(user);
            } 
            var urlMappingDTO = new UrlMappingDTO(urlMapping.Id, urlMapping.OriginalUrl, urlMapping.ShortUrl, UrlMapping.State.Valid); //Mapear
            return urlMappingDTO;

        }
        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
