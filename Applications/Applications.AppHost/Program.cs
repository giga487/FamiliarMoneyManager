var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache").WithDataVolume(isReadOnly: false);


var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password).WithPgAdmin();
var postgresdb = postgres.AddDatabase("postgresdb", "TransactionApp");

var apiService = builder.AddProject<Projects.Applications_ApiService>("apiservice").
    WithReference(postgresdb).WaitFor(postgresdb)
    .WithEnvironment("Encryption__Key", builder.Configuration["Encryption:Key"]);

builder.AddProject<Projects.Applications_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
