using static UrlShortener.Models.UrlMapping;

namespace UrlShortener.DTOs
{
    public record UrlMappingDTO(Guid Id, string OriginalUrl, string ShortUrl, State State);
}
