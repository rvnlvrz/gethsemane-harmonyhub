using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

public partial record InventoryItemDto
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Name { get; set; }
    public ItemType ItemType { get; set; }
    public int ItemCount { get; set; }
    public List<BorrowerDto> Borrowers { get; set; } = [];
}
