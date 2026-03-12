using AdventureWorks.Domain.Todos;
using AdventureWorks.Domain.Users;
using AdventureWorks.Domain.Products;
using AdventureWorks.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Domain.Orders;

namespace AdventureWorks.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<Product> Products { get; }
    DbSet<Customer> Customers { get; }

    DbSet<Order> Orders { get; }
    DbSet<OrderLine> OrderLines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
