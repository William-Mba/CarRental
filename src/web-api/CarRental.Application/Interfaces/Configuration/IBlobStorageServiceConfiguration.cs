﻿namespace CarRental.Application.Interfaces.Configuration
{
    public interface IBlobStorageServiceConfiguration
    {
        string ContainerName { get; set; }
        string ConnectionString { get; set; }
        string Key { get; set; }
        string AccountName { get; set; }
    }
}
