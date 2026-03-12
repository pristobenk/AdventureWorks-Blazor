using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Domain.Orders;
using System.Collections.Generic;

namespace AdventureWorks.Application.Orders.Create;

public sealed class CreateOrderCommand : ICommand<int>
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CreateOrderLineDto> OrderLines { get; set; } = new();
}

public sealed class CreateOrderLineDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
