using System.Reflection;

using Microsoft.AspNetCore.Routing;

namespace MIB.Core.Infrastructure.EndPoint.MinimalApi;

public interface IEndpointHandler
{
    public void MapEndpoint(IEndpointRouteBuilder builder);
}

public static class IEndpointRouteBuilderExtentions
{

    public static void MapEndpointHandlers(this IEndpointRouteBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();
        var endpointHandlerTypes = assembly
            .GetTypes()
            .Where(x => typeof(IEndpointHandler).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        foreach (var type in endpointHandlerTypes)
        {
            if (Activator.CreateInstance(type) is IEndpointHandler handler)
                handler.MapEndpoint(builder);
        }
    }
}