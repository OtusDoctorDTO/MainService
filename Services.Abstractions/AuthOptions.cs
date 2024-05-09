namespace Services.Abstractions
{
    public class AuthOptions
    {
        public string Issuer { get; set; } = default!;
        public string Key { get; set; } = default!;
    }
}
