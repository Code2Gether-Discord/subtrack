namespace subtrack.MAUI.Exceptions
{
    [Serializable]
    public class SubscriptionNotFoundException : Exception
    {
        public SubscriptionNotFoundException() { }

        public SubscriptionNotFoundException(string message)
            : base(message) { }

        public SubscriptionNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}