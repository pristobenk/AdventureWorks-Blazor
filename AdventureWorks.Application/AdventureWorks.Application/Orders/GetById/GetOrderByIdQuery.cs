using AdventureWorks.Application.Abstractions.Messaging;

namespace AdventureWorks.Application.Orders.GetById;

public sealed record GetOrderByIdQuery(int OrderId) : IQuery<GetOrderByIdResponse>;
