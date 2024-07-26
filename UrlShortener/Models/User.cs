namespace UrlShortener.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UrlMapping> UrlMappings { get; set; } = new List<UrlMapping>();

        public User(Guid id, string userName, string email, string password)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Password = password;
        }

    }
}
