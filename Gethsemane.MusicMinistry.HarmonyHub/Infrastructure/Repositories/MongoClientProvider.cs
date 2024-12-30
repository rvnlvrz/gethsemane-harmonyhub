using System.IdentityModel.Tokens.Jwt;
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

public class MongoClientProvider : IMongoClientProvider
{
    private readonly IAuthenticationService _authenticationService;
    private readonly Lazy<Task<IMongoClient>> _lazyMongoClient;
    private readonly ISecretsService _secretsService;
    private readonly ITokenCache _tokenCache;

    public MongoClientProvider(ISecretsService secretsService,
        ITokenCache tokenCache,
        IAuthenticationService authenticationService)
    {
        ArgumentNullException.ThrowIfNull(secretsService);
        ArgumentNullException.ThrowIfNull(tokenCache);
        ArgumentNullException.ThrowIfNull(authenticationService);

        _secretsService = secretsService;
        _tokenCache = tokenCache;
        _authenticationService = authenticationService;
        _lazyMongoClient =
            new Lazy<Task<IMongoClient>>(InitializeMongoClientAsync);
    }

    private static string AtlasConnectionString => "Atlas-ConnectionString";

    public Task<IMongoClient> GetClientAsync() => _lazyMongoClient.Value;

    private async Task<IMongoClient> InitializeMongoClientAsync()
    {
        var accessToken = await _tokenCache.AccessTokenAsync();

        if (IsJwtExpired(accessToken))
        {
            await _authenticationService.RefreshAsync();
        }

        var response = await _secretsService
            .GetSecretVersionAsync(
                AtlasConnectionString,
                "latest",
                CancellationToken.None);

        if (response.Content?.Value == null)
        {
            throw new Exception("Atlas connection string is null");
        }

        var connectionString = response.Content.Value;
        var client = new MongoClient(connectionString);

        MongoConfiguration.Configure();

        return client;
    }

    private static bool IsJwtExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT");
        }

        var jwtToken = handler.ReadJwtToken(token);

        // Get the expiration claim (exp)
        var expiration = jwtToken.ValidTo;

        // Check if the token is expired
        return DateTime.UtcNow >= expiration;
    }
}
