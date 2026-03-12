using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Orders;

public static class OrderErrors
{
    public static Error NotFound(int id) => Error.NotFound(
        "Orders.NotFound",
        $"The order with the Id = '{id}' was not found");
}
