namespace AdventureWorks.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
