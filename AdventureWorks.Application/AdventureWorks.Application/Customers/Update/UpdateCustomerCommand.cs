using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Customers.Update;

public sealed class UpdateCustomerCommand : ICommand
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}
