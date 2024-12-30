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
    public string? Id { get; init; }

    public string Name { get; } = name;
    public ItemType ItemType { get; } = itemType;
    public int ItemCount { get; } = itemCount;
    public List<Borrower> Borrowers { get; } = borrowers;
    [MapperIgnore] public bool HasBorrowers => Borrowers.Count != 0;

    [MapperIgnore]
    public int BorrowedQuantity =>
        Borrowers.Sum(borrower => borrower.BorrowedQuantity);

    [MapperIgnore] public bool IsAvailable => ItemCount >= BorrowedQuantity;
}
