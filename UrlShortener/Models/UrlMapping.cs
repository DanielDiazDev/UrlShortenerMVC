namespace UrlShortener.Models
{
    public class UrlMapping
    {
        public enum State 
        { 
            Valid, 
            Expirated 
        }
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public State StateType { get; set; }
        private UrlMapping() { }
        public UrlMapping(Guid id, string urlOriginal, Guid? userId, string shortUrl)
        {
            Id = id;
            OriginalUrl = urlOriginal;
            CreatedAt = DateTime.UtcNow;
            ExpirationDate = CreatedAt.Value.AddHours(24); 
            StateType = State.Valid;
            UserId = userId;
            ShortUrl = shortUrl;
        }
        public State CheckState()
        {
            return IsExpired() ? State.Expirated : State.Valid; 
        }
        private bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationDate.Value;
        }
    }
}
