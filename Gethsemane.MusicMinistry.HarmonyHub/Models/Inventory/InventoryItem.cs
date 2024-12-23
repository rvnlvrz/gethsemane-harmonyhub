namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

public class InventoryItem(string name, ItemType itemType, int itemCount, List<Borrower> borrowers)
{
    /// <summary>
    ///     Id of the inventory item. Will be null if the item has not been sent to a database store yet.
    /// </summary>
    public string? Id { get; set; }

    public string Name { get; set; } = name ?? throw new ArgumentNullException(nameof(name));
    public ItemType ItemType { get; set; } = itemType;
    public int ItemCount { get; set; } = itemCount;
    public List<Borrower> Borrowers { get; set; } = borrowers ?? throw new ArgumentNullException(nameof(borrowers));
    public bool HasBorrowers => Borrowers.Count != 0;
    public int BorrowedQuantity => Borrowers.Sum(borrower => borrower.BorrowedQuantity);
    public bool IsAvailable => ItemCount >= BorrowedQuantity;
}
