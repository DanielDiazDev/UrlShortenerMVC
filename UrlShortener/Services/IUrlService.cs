using System.Xml.Serialization;
using UrlShortener.DTOs;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<UrlMappingDTO> Shorten(string urlOriginal);
    }
}
