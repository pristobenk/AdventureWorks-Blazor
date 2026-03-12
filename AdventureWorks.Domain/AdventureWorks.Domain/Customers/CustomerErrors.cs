using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Customers;

public static class CustomerErrors
{
    public static Error NotFound(int id) => Error.NotFound(
        "Customers.NotFound",
        $"The customer with the Id = '{id}' was not found");
}
