namespace SearchAPI.Common.Classes.Identity
{
    public class JwtOptions
    {
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string TokenValidityInMinutes { get; set; } = string.Empty;
        public string RefreshTokenValidityInDays { get; set; } = string.Empty;
    }
}
