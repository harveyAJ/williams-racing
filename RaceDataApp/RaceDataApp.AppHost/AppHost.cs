using Projects;

const bool persistData = true;

Console.WriteLine("Starting application...");

var builder = DistributedApplication.CreateBuilder(args);
var username = builder.AddParameter("username", true);
var password = builder.AddParameter("password", true);

//We are setting the default database name (raceDb) via the init.sql script
var postgresPort = 5436;
var postgres = builder.AddPostgres("postgres", username, password, postgresPort)
    .WithPgWeb()
    // Set the name of the default database to auto-create on container startup.
    .WithEnvironment("POSTGRES_DB", "postgres")
    // Mount the SQL scripts directory into the container so that the init scripts run.
    .WithBindMount("../DatabaseContainers/data/postgres", "/docker-entrypoint-initdb.d")
    .WithLifetime(persistData ? ContainerLifetime.Persistent : ContainerLifetime.Session);

var raceDbName = "racedb";
var raceDb = postgres.AddDatabase(raceDbName);

var migrations = builder.AddProject<RaceDataApp_Loader>("race-data-loader")
    .WithReference(raceDb)
    .WaitFor(postgres);

// var migrations = builder.AddProject<RaceDataApp_Loader_Console>("migrations")
//     .WithReference(raceDb)
//     .WaitFor(postgres);

var raceDataAppApi = builder.AddProject<RaceDataApp_Reader>("race-data-api")
    .WithReference(raceDb)
    .WithReference(migrations)
    .WaitFor(postgres);

builder.AddNpmApp("race-data-ui", "../RaceDataApp.Ui")
    .WithReference(raceDataAppApi)
    .WaitFor(raceDataAppApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();