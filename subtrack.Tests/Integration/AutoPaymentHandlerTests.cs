﻿namespace subtrack.Tests.Integration
{
    public class AutoPaymentHandlerTests : IntegrationTestBase
    {
        private readonly AutoPaymentHandler _sut;
        private readonly SettingsServiceWithCallCount _settingsService;

        public AutoPaymentHandlerTests()
        {
            _sut = _serviceProvider.GetRequiredService<AutoPaymentHandler>();
            _settingsService = (SettingsServiceWithCallCount)_serviceProvider.GetRequiredService<ISettingsService>();
        }

        public override void OnServicesRegistered()
        {
            AddDecorator<ISettingsService, SettingsService, SettingsServiceWithCallCount>();
        }

        [Fact]
        public async Task AutoPaidSubsAreAutoPaid()
        {
            var dueSubs = SaveEntities(new[] {
                CreateSubscription(DateTime.Today.AddYears(-2), autoPaid: true, billingOccurrence: BillingOccurrence.Week),
                CreateSubscription(DateTime.Today.AddYears(-4), autoPaid: true, billingOccurrence: BillingOccurrence.Month),
             });

            await _sut.ExecuteAsync();

            Assert.True(dueSubs.All(s => s.LastPayment.Year == DateTime.Today.Year));
        }

        [Fact]
        public async Task NotAutoPaidSubsAreNotPaid()
        {
            var dueSubs = SaveEntities(new[] {
                CreateSubscription(DateTime.Today.AddMonths(-1), autoPaid: false),
                CreateSubscription(DateTime.Today.AddMonths(-1), autoPaid: false),
             });

            await _sut.ExecuteAsync();

            Assert.True(dueSubs.All(x => x.LastPayment.Date < DateTime.Today));
        }

        [Fact]
        public async Task CanOnlyExecuteOnceADay()
        {
            await _sut.ExecuteAsync();
            await _sut.ExecuteAsync();

            Assert.Equal(1, _settingsService.UpdateCount);
        }
    }

    public class SettingsServiceWithCallCount : ISettingsService
    {
        private readonly ISettingsService _settingsService;

        public SettingsServiceWithCallCount(ISettingsService settingsService) => _settingsService = settingsService;

        public int UpdateCount { get; private set; }

        public Task<T?> GetByIdAsync<T>(string id) where T : SettingsBase => _settingsService.GetByIdAsync<T>(id);

        public Task UpdateAsync<T>(T setting) where T : SettingsBase
        {
            UpdateCount++;
            return _settingsService.UpdateAsync(setting);
        }
    }
}