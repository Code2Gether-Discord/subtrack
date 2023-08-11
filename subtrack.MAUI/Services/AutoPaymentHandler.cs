using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtrack.MAUI.Services
{
    public class AutoPaymentHandler
    {
        private ISubscriptionService _subscriptionService;
        private IDateTimeProvider _dateTimeProvider;
        private ISettingsService _settingsService;
        private GetSubscriptionsFilter filter;
        
        private bool HasExecutedToday(Subscription subscription)
        {
            if(subscription.LastPayment == DateTime.Today)
            {
                return true;
            } 
            return false;
        }
        public async Task ExecuteAsync()
        {
            filter.AutoPaid = true;
            var settings = new DateTimeSetting();

            IEnumerable<Subscription> autoPaidSubs = (IEnumerable<Subscription>)_subscriptionService.GetSubscriptions(filter);
            
            foreach(var sub in autoPaidSubs)
            {
                if(HasExecutedToday(sub))
                {
                    settings.Value = DateTime.Now.Date;
                    
                }
                continue;
            }  
        }
    }
}
