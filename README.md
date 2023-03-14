# CarRental Application on the Azure cloud

*- This repository contains sample project where different Azure cloud services were used*

*- Some of the concepts were simplified to make it possible to deliver complete solution*


Cars Rental is a fake car rental company which used Microsoft Azure cloud services to implement the system for managing car renting.

*If you like this content, please give it a star!*
![github-start.png](images/github-start.png)

## Use cases are following:

1. Customer can create account (register)
2. Customer can sign in
3. Customer can display list of all available cars
4. Customer can make a reservation for a specific car (if it is not already reserved by other customer)
5. Customer can send new enquiry and attach the file


# General overview of solution architecture

## Solution architecture

Below diagram presents Car Rental Solution architecture. 

<p align="center">
<img src="images/CarsRental-CloudArchitecture.png?raw=true" alt="Image not found"/>
</p>

## Azure Active Directory B2C

Azure Active Directory B2C is an identity service in the Azure cloud that enables user authentication and management. Implementing own identity service can be challenging. Try to think about data storage, secure connections, or token generation and validation. With Azure AD B2C, adding user authentication is much easier. In the Cars Rental solution users can create accounts and login to access some of the functionalities. What is more - with Azure AD B2C, login, registration, password reset, or profile edit pages can be customized and branded (like with the company logo or background). You do not have to implement login, or registration views yourself.


## Azure Key Vault

Security is a very important aspect of every project. Secrets and credentials should be stored in the secure store. This is why Azure Key Vault is used in the Cars Rental solution. Parameters like connection string to the database or storage key are stored in the Azure Key Vault instance.


## Azure Application Insights

Tracking issues in cloud solutions can be challenging. Collecting logs and detecting bugs can be hard. This is why it is good to use an Application Performance Management service like Azure Application Insights. With this Azure cloud service, we can log all events and errors that occure in our solution. Azure Application Insights provides SDKs in many languages (like C# oraz Java) so we can easily integrate them with our application. All logs are then available in the Azure portal, where rich dashboards are displayed with collected log data.


## Azure Web Apps with App Service Plans

Hosting web applications in the Azure cloud is much easier with Azure Web Apps. Cars Rental Web portal is written with Angular framework and Cars Rental API is written with ASP.NET Core. These web applications are hosted using Azure Web Apps. Azure App Service Plans provide a way to scale in and out, up and down so you can apply automatic scale when there is higher load and traffic. With Azure Web Apps we can also use custom SSL certificates.


## Azure Function Apps

Azure Function Apps are serverless services available in the Azure cloud. They are ideal to be used as event handlers for processing events. It is important to mention the cost - you only pay for this service once it is executed. Up to 1 million executions is for free. In the Cars Rental solution, Azure Function App was used to handle events related to sending car's reservation confirmation emails once custom complete reservation in the web portal. This Function App is triggered once there is a new message in the Azure Service Bus queue.


## Azure Service Bus

Azure Service Bus service is a cloud messaging service. With Azure Service Bus we can build reliable and elastic cloud apps with messaging. In the Cars Rental solution, Azure Service Bus queues were used to queue car's reservation confirmations to send emails to customers. Once car's reservation is completed in the web portal, information is passed to the API and saved in the database. After this process, there is new message sent to the queue. Then Azure Function is triggered and new email is sent using Azure SendGrid Service.


## Azure Emails Service

Azure Communication Services Email REST APIs and SDKs is used to send an email messages from service applications.


## Azure Cosmos DB

Azure Service Bus is a globally distributed database available in the Azure cloud. Data about all cars and reservations is stored in this database in the Cars Rental solution.


## Azure Storage Account

Azure Storage Account is one of the oldest services available in the Azure cloud. It provides an easy way to store different kind of files using Blob Storage. In the Cars Rental solution, it is used to store car images that are then displayed in the web portal.


## Azure API Management

Azure API Management is a service that works as a gateway to different APIs behind it. With Azure API Management you can secure your APIs. It provides a different kind of policies so for instance we can implement throttling or validate tokens. In the Cars Rental solution, it was used to protect access to Cars Rental API.
