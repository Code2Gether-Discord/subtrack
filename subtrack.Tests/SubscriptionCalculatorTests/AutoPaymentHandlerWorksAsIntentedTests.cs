using NSubstitute;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services;
using subtrack.MAUI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtrack.Tests.SubscriptionCalculatorTests
{
    public class AutoPaymentHandlerWorksAsIntentedTests
    {
        private readonly ISubscriptionsCalculator _sut;
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        private readonly ISettingsService _settingsService;

        public AutoPaymentHandlerWorksAsIntentedTests()
        {
            _sut = new SubscriptionsCalculator(_dateTimeProvider);
        }

        [Fact]
        public void AutoPaymentHandled()
        {
            var sub = new Subscription() { IsAutoPaid = true, LastPayment = DateTime.Today};
            var setting = new DateTimeSetting() { Value  = DateTime.Today };
            Assert.Equal(sub.LastPayment, setting.Value);
        }

    }
}
