﻿using subtrack.MAUI.Services;
using subtrack.DAL.Entities;
using subtrack.MAUI.Data;

namespace subtrack.MAUI.Response
{
    public class SubscriptionsMonthResponse
    {
        public DateTime CurrentDate { get; set; }
        public decimal Cost { get; set; }
        public IEnumerable<SubscriptionResponse> SubscriptionResponse { get; }

        public SubscriptionsMonthResponse(DateTime dateTime)
        {
            CurrentDate = dateTime;
        }
    }
}
