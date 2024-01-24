namespace SGCodeChallenge
{
    public record class JwtOptions(
        string Issuer,
        string Audience,
        string SigningKey,
        int ExpirationSeconds
    )
    {
        public JwtOptions() : this("", "", "", 0) { }
    };
}
