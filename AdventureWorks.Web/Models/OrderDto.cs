namespace AdventureWorks.Web.Models;

public sealed class OrderLineFormModel
{
    public int? OrderLineId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}

public sealed class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CreateOrderLineRequest> OrderLines { get; set; } = new();
}

public sealed class CreateOrderLineRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public sealed class GetOrderByIdResponse
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<GetOrderByIdLineResponse> OrderLines { get; set; } = new();
}

public sealed class GetOrderByIdLineResponse
{
    public int OrderLineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public sealed class UpdateOrderRequest
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<UpdateOrderLineRequest> OrderLines { get; set; } = new();
}

public sealed class UpdateOrderLineRequest
{
    public int? OrderLineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
