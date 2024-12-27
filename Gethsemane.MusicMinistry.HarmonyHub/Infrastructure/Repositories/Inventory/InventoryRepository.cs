using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Mappers;
using Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;
using MongoDB.Driver;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories.
    Inventory;

public class InventoryRepository : IInventoryRepository
{
    private readonly IMongoClientProvider _mongoClientProvider;

    public InventoryRepository(IMongoClientProvider mongoClientProvider)
    {
        ArgumentNullException.ThrowIfNull(mongoClientProvider);
        _mongoClientProvider = mongoClientProvider;
    }

    public async Task<InventoryItem[]> GetAllItems(CancellationToken ct)
    {
        var client = await _mongoClientProvider.GetClientAsync();
        var database = client.GetDatabase("HarmonyHub");
        var collection = database.GetCollection<InventoryItemDto>("Inventory");
        var findAsync = await collection.FindAsync(
            i => true,
            cancellationToken: ct);

        var inventoryItemDtos = findAsync.ToList(cancellationToken: ct);

        var inventoryItemList =
            new List<InventoryItem>(inventoryItemDtos.Count);

        inventoryItemList.AddRange(
            inventoryItemDtos.Select(
                InventoryItemMapper.Map));

        return inventoryItemList.ToArray();
    }
}

public interface IInventoryRepository
{
    Task<InventoryItem[]> GetAllItems(CancellationToken ct);
}
