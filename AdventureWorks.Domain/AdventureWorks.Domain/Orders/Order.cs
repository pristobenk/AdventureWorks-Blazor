using AdventureWorks.Domain.Customers;
using AdventureWorks.SharedKernel;

namespace AdventureWorks.Domain.Orders;

public sealed class Order:Entity
{
    public  int OrderId { get; set; }
    public required int CustomerId { get; set; }
    public required DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; } = 0;
    public Customer? Customer { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

}
