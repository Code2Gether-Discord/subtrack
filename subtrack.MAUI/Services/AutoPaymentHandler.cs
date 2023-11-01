using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services
{
    public class AutoPaymentHandler
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IDateProvider _dateProvider;
        private readonly ISettingsService _settingsService;
        private readonly ISubscriptionsCalculator _subscriptionsCalculator;

        public AutoPaymentHandler(ISubscriptionService subscriptionService, IDateProvider dateProvider, ISettingsService settingsService, ISubscriptionsCalculator subscriptionsCalculator)
        {
            _subscriptionService = subscriptionService;
            _dateProvider = dateProvider;
            _settingsService = settingsService;
            _subscriptionsCalculator = subscriptionsCalculator;
        }

        public async Task ExecuteAsync()
        {
            var today = _dateProvider.Today;
            var lastTimeRun = await _settingsService.GetByIdAsync<DateTimeSetting>(DateTimeSetting.LastAutoPaymentTimeStampKey);

            if (lastTimeRun?.Value == today)
                return;

            await HandleAsync();

            lastTimeRun.Value = today;
            await _settingsService.UpdateAsync(lastTimeRun);
        }

        private async Task HandleAsync()
        {
            var autoPaidSubs = await _subscriptionService.GetAllAsync(new GetSubscriptionsFilter { AutoPaid = true });

            foreach (var sub in autoPaidSubs)
            {
                if (!_subscriptionsCalculator.IsDue(sub).IsDue)
                    continue;

                await _subscriptionService.AutoPayAsync(sub.Id);
            }
        }
    }
}
