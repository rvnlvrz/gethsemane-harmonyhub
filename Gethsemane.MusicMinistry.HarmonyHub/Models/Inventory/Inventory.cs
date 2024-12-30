namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

public class InventoryAggregateRoot
{
    public List<InventoryItem> Items { get; set; } = new();

    public void AddItem(InventoryItem item) => throw new NotImplementedException();

    public void UpdateItem(InventoryItem item) => throw new NotImplementedException();

    public void RemoveItem(InventoryItem item) => throw new NotImplementedException();

    public IEnumerable<InventoryItem> GetItems() => Items;
    public IEnumerable<InventoryItem> GetBorrowedItems() => Items.Where(i => i.HasBorrowers);
    public IEnumerable<InventoryItem> GetAvailableItems() => Items.Where(i => i.IsAvailable);
}
