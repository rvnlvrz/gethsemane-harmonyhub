using MongoDB.Driver;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.
    Repositories;

public interface IMongoClientProvider
{
    Task<IMongoClient> GetClientAsync();
}
