using Gethsemane.MusicMinistry.HarmonyHub.Configuration;
using MongoDB.Driver;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories.
    Inventory;

public class InventoryRepository
{
    private readonly Lazy<ValueTask<IMongoClient>> _lazyMongoClient;

    public InventoryRepository(IAtlasClient atlasClient)
    {
        ArgumentNullException.ThrowIfNull(atlasClient);
        _lazyMongoClient = atlasClient.LazyClient;
    }

    public async Task DoSomethingAsync()
    {
        var client = await _lazyMongoClient.Value;
    }
}

public class AtlasClient : IAtlasClient
{
    private readonly ISecretsClient _secretsClient;

    public AtlasClient(ISecretsClient secretsClient)
    {
        ArgumentNullException.ThrowIfNull(secretsClient);
        _secretsClient = secretsClient;
        LazyClient = new Lazy<ValueTask<IMongoClient>>(CreateMongoClientAsync);
    }

    private static string AtlasConnectionString => "Atlas-ConnectionString";

    public Lazy<ValueTask<IMongoClient>> LazyClient { get; init; }

    private async ValueTask<IMongoClient> CreateMongoClientAsync()
    {
        var response = await _secretsClient
            .GetSecretVersionAsync(
                CancellationToken.None,
                AtlasConnectionString,
                "1");

        if (response.Content?.Value == null)
        {
            throw new Exception("Atlas connection string is null");
        }

        var connectionString = response.Content.Value;
        var client = new MongoClient(connectionString);
        return client;
    }
}

public interface IAtlasClient
{
    public Lazy<ValueTask<IMongoClient>> LazyClient { get; init; }
}
