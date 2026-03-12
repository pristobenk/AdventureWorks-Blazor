namespace AdventureWorks.Domain.Customers;

using AdventureWorks.SharedKernel;

public sealed class Customer : Entity
{
    public int CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public string? ContactName { get; set; } = null;
    public string? PhoneNumber { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Address { get; set; } = null;
}
