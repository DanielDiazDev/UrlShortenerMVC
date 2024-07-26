using UrlShortener.Models;

namespace UrlShortener.Data.Repositories
{
    public interface IUrlRepository
    {
        public Task<bool> Add(UrlMapping urlMapping);
        public Task<UrlMapping> GetById(Guid id);
        public Task<UrlMapping> GetByName(string shortUrl);
        public List<UrlMapping> GetAll();
        public Task<bool> Delete(Guid id);
    }
    
}
