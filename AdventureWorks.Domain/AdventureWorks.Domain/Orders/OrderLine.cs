using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Orders;

public sealed class OrderLine: Entity
{
    public  int OrderLineId { get; set; }
    public  int OrderId { get; set; }
    public required int ProductId { get; set; }
    public required int Quantity { get; set; } = 0;
    public required decimal UnitPrice { get; set; } = 0;

    public  Order? Order { get; set; }
}
