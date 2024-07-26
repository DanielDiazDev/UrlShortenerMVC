using UrlShortener.Models;

namespace UrlShortener.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> Add(User user);
        public Task<User> GetById(Guid id);
        public List<User> GetAll();
        public bool Update(User user);
        public Task<bool> Delete(Guid id);
    }
}
