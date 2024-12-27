using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Riok.Mapperly.Abstractions;

namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

public class InventoryItem(
    string name,
    ItemType itemType,
    int itemCount,
    List<Borrower> borrowers)
{
    /// <summary>
    ///     Id of the inventory item. Will be null if the item has not been sent to a database store yet.
    /// </summary>
    public string? Id { get; set; }

    public string Name { get; set; } = name;
    public ItemType ItemType { get; set; } = itemType;
    public int ItemCount { get; set; } = itemCount;
    public List<Borrower> Borrowers { get; set; } = borrowers;
    [MapperIgnore] public bool HasBorrowers => Borrowers.Count != 0;

    [MapperIgnore]
    public int BorrowedQuantity =>
        Borrowers.Sum(borrower => borrower.BorrowedQuantity);

    [MapperIgnore] public bool IsAvailable => ItemCount >= BorrowedQuantity;
}

public partial record InventoryItemDto
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Name { get; set; }
    public ItemType ItemType { get; set; }
    public int ItemCount { get; set; }
    public List<BorrowerDto> Borrowers { get; set; } = [];
}

public partial record BorrowerDto
{
    public string? BorrowerId { get; set; }
    public int BorrowedQuantity { get; set; }
    public DateTime DueDate { get; set; }
}
