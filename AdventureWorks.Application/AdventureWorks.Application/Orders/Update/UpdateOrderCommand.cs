using AdventureWorks.Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace AdventureWorks.Application.Orders.Update;

public sealed class UpdateOrderCommand : ICommand
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<UpdateOrderLineDto> OrderLines { get; set; } = new();
}

public sealed class UpdateOrderLineDto
{
    public int? OrderLineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
