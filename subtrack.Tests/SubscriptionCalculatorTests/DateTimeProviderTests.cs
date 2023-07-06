using Microsoft.Extensions.DependencyInjection;
using subtrack.MAUI.DateAndTimeProvider;
using subtrack.Tests.Extensions;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class DateTimeProviderTests
    {
        [Fact]
        public void IocContainer_GetService_IDateTimeProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddServices();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var service = serviceProvider.GetService<IDateTimeProvider>();

            Assert.NotNull(service);
        }
    }
}
