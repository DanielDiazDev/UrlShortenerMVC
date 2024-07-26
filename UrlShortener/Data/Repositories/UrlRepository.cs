using UrlShortener.Models;

namespace UrlShortener.Data.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDbContext _context;
        public UrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(UrlMapping urlMapping)
        {
            await _context.UrlMappings.AddAsync(urlMapping);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var url = await GetById(id);
            _context.UrlMappings.Remove(url);
            await _context.SaveChangesAsync();
            return true;
        }

        public List<UrlMapping> GetAll()
        {
            return _context.UrlMappings.ToList();
        }

        public async Task<UrlMapping> GetById(Guid id)
        {
            return await _context.UrlMappings.FindAsync(id);
        }

        public async Task<UrlMapping> GetByName(string shortUrl)
        {
            return _context.UrlMappings.FirstOrDefault(x => x.ShortUrl == shortUrl);
        }
    }
    
}
