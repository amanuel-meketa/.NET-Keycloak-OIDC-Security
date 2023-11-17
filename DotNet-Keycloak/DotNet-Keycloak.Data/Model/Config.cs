namespace DotNet_Keycloak.Data.Model
{
    public class Config
    {
        public Guid Id { get; set; }
        public string? ServerRealm { get; set; }
        public string? Metadata { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? TokenExchange { get; set; }
        public string? Audience { get; set; }
    }
}
