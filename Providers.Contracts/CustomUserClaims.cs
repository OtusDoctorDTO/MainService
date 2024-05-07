namespace Providers.Contracts
{
    public record CustomUserClaims(
        string Name = null!,
        string Email = null!,
        string UserId = null!,
        string NameIdentifier = null!,
        string MobilePhone = null!);
}
