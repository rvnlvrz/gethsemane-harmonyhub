using Gethsemane.MusicMinistry.HarmonyHub.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories;

public static class MongoConfiguration
{
    public static void Configure()
    {
        var conventionPack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true), // Ignore unmapped fields
            new CamelCaseElementNameConvention() // Use camelCase for field names
        };

        ConventionRegistry.Register(
            "CustomConventions",
            conventionPack,
            t => true // Apply to all types
        );
    }
}

public class MongoClientProviderProvider : IMongoClientProvider
{
    private readonly Lazy<Task<IMongoClient>> _lazyMongoClient;
    private readonly ISecretsClient _secretsClient;

    public MongoClientProviderProvider(ISecretsClient secretsClient)
    {
        ArgumentNullException.ThrowIfNull(secretsClient);
        _secretsClient = secretsClient;
        _lazyMongoClient =
            new Lazy<Task<IMongoClient>>(InitializeMongoClientAsync);
    }

    private static string AtlasConnectionString => "Atlas-ConnectionString";

    public Task<IMongoClient> GetClientAsync() => _lazyMongoClient.Value;

    private async Task<IMongoClient> InitializeMongoClientAsync()
    {
        var response = await _secretsClient
            .GetSecretVersionAsync(
                CancellationToken.None,
                AtlasConnectionString,
                "latest");

        if (response.Content?.Value == null)
        {
            throw new Exception("Atlas connection string is null");
        }

        var connectionString = response.Content.Value;
        var client = new MongoClient(connectionString);

        MongoConfiguration.Configure();

        return client;
    }
}
