namespace CarRental.Application.Interfaces.Configuration
{
    public interface IMessagingServiceConfiguration
    {
        string ListenAndSendConnectionString { get; set; }
        string QueueName { get; set; }
    }
}
