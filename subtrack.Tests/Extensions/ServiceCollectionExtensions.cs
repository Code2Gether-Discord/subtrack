using Microsoft.Extensions.DependencyInjection;
using subtrack.MAUI.DateAndTimeProvider;

namespace subtrack.Tests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDateTimeProvider, DateTimeProvider>();

            return serviceCollection;
        }
    }
}
