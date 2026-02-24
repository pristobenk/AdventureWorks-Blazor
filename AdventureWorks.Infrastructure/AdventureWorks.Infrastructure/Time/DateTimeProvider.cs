using AdventureWorks.SharedKernel;

namespace AdventureWorks.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
