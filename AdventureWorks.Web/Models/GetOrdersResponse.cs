namespace AdventureWorks.Web.Models;

public sealed class GetOrdersResponse
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<GetOrderLineResponse> OrderLines { get; set; } = new();
}

public sealed class GetOrderLineResponse
{
    public int OrderLineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
