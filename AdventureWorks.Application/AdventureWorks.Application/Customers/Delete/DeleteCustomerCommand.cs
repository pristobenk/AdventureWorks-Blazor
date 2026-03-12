using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Customers.Delete;

public sealed record DeleteCustomerCommand(int CustomerId) : ICommand;
