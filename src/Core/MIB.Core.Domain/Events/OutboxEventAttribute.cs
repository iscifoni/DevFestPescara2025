namespace MIB.Core.Domain.Events
{
    /// <summary>
    /// Attributo per la configurazione della coda da utilizzare
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OutboxEventAttribute : Attribute
    {
        /// <summary>
        /// Nome del servizio di PubSub da utilizzare
        /// </summary>
        public string? PubSubName { get; set; }

        /// <summary>
        /// Nome del topic da utilizzare
        /// </summary>
        public string? TopicName { get; set; }
    }
}