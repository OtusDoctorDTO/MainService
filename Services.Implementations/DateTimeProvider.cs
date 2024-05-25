using Services.Abstractions;

namespace Services.Implementations
{
    public class DateTimeProvider: IDateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;
        public DateTime GetUtcNow() => DateTime.UtcNow;
    }
}
