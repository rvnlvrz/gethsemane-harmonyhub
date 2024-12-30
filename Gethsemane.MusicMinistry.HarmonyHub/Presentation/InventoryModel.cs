using System.ComponentModel;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories.Inventory;
using Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

[Bindable(BindableSupport.Yes)]
public partial record InventoryModel
{
    private readonly INavigator _navigator;
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryModel(INavigator navigator, IInventoryRepository inventoryRepository)
    {
        ArgumentNullException.ThrowIfNull(navigator);
        ArgumentNullException.ThrowIfNull(inventoryRepository);

        _navigator = navigator;
        _inventoryRepository = inventoryRepository;
    }

    public string? Title => "Inventory";

    private IListFeed<InventoryItem> InventoryItems =>
        ListFeed.Async(async ct => await _inventoryRepository.GetAllItems(ct));

    public IListFeed<InventoryItem> AllItems => InventoryItems;

    public IListFeed<InventoryItem> BorrowedItems =>
        InventoryItems.Where(item => item.BorrowedQuantity > 0);
}
