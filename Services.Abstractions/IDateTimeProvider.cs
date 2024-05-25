namespace Services.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();
        DateTime GetUtcNow();
    }
}
