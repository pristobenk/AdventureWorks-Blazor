using AdventureWorks.Domain.Todos;
using AdventureWorks.Domain.Users;
using AdventureWorks.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<Product> Products { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
