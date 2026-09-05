namespace Backend.Configuration
{
    /// <summary>
    /// Signing and validation settings for the bearer tokens, bound from the "Jwt"
    /// configuration section.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// The HMAC signing key. Deliberately absent from appsettings.json: the
        /// development value lives in appsettings.Development.json, and anything
        /// deployed must supply its own through an environment variable or a secret
        /// store. A key committed to the repository is a key everyone has.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        public int ExpiryMinutes { get; set; } = 120;

        /// <summary>
        /// Fails fast at startup rather than issuing tokens nobody can trust.
        /// HMAC-SHA256 needs at least 256 bits of key material.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Issuer))
                throw new InvalidOperationException("Jwt:Issuer is not configured.");

            if (string.IsNullOrWhiteSpace(Audience))
                throw new InvalidOperationException("Jwt:Audience is not configured.");

            if (string.IsNullOrWhiteSpace(Key))
                throw new InvalidOperationException(
                    "Jwt:Key is not configured. Set it through user-secrets or an environment variable.");

            if (System.Text.Encoding.UTF8.GetByteCount(Key) < 32)
                throw new InvalidOperationException(
                    "Jwt:Key must be at least 32 bytes for HMAC-SHA256.");

            if (ExpiryMinutes <= 0)
                throw new InvalidOperationException("Jwt:ExpiryMinutes must be greater than zero.");
        }
    }
}
