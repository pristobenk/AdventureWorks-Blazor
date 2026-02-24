using Microsoft.AspNetCore.Routing;

namespace AdventureWorks.WebApi.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
