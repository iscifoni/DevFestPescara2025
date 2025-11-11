using Aspire.Hosting;
using CommunityToolkit.Aspire.Hosting.Dapr;
using System.Collections.Immutable;

var builder = DistributedApplication.CreateBuilder(args);

var serviceUserName = builder.AddParameter("username", "admin", secret: true);
var servicePassword = builder.AddParameter("password", "Password1!", secret: true);



var cache = builder.AddRedis("cache");

var activeMQ = builder.AddContainer("activemqartemiscontainer", "apache/activemq-artemis", "2.37.0")
    .WithEnvironment("ARTEMIS_JOURNAL_TYPE", "NIO")
    .WithEnvironment("ARTEMIS_USER", "admin")
    .WithEnvironment("ARTEMIS_PASSWORD", "admin")
    .WithHttpEndpoint(8161, 8161)
    .WithEndpoint(61616, 61616, "tcp");


var rabbitMQ = builder
    .AddRabbitMQ("rabbitmq", serviceUserName, servicePassword, 58074)b
    .WithManagementPlugin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var pubsub = builder.AddDaprPubSub("pubsub", new DaprComponentOptions { LocalPath = "AzComponent" })
    .WaitFor(activeMQ)
    .WaitFor(rabbitMQ);

// SQL Server (database persistente)
var sqlServer = builder.AddSqlServer("sqlserver", servicePassword, 1433)
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WaitFor(pubsub);

var mibDb = sqlServer.AddDatabase("MIBDb");


var apiService = builder.AddProject<Projects.MIB_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(sqlServer);

builder.AddProject<Projects.MIB_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.MIB_Microservices_OrderService_Api>("mib-microservices-orderservice-api")
    .WithReference(mibDb)
    .WaitFor(sqlServer)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "mib-microservices-orderservice-api",
        AppPort = 7094, //Note that this argument is required if you intend to configure pubsub, actors or workflows as of Aspire v9.0 
                        //DaprGrpcPort = 50001,
                        //DaprHttpPort = 3500,
                        //MetricsPort = 9090,
        ResourcesPaths = ImmutableHashSet.Create("AzComponent"),
        AppProtocol = "https"
    });


builder.Build().Run();
