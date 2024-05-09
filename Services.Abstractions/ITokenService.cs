
namespace Services.Abstractions
{
    public interface ITokenService
    {
        Task<bool> Validate(string? token);
    }
}
