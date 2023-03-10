namespace CarRental.Application.Interfaces.Configuration
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CarContainerName { get; set; }
        string CarContainerPartitionKeyPath { get; set; }
        string EnquiryContainerName { get; set; }
        string EnquiryContainerPartitionKeyPath { get; set; }
        string CarReservationContainerName { get; set; }
        string CarReservationContainerPartitionKeyPath { get; set; }
    }
}
