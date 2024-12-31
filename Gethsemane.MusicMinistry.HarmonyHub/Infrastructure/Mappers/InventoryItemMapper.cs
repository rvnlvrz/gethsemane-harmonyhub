using Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;
using Riok.Mapperly.Abstractions;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Mappers;

[Mapper]
[UseStaticMapper(typeof(BorrowerMapper))]
public static partial class InventoryItemMapper
{
    public static partial InventoryItemDto Map(InventoryItem inventoryItem);

    public static partial InventoryItem Map(
        InventoryItemDto inventoryItemDto);
}

[Mapper]
public static partial class BorrowerMapper
{
    [MapProperty(nameof(Borrower.Id), nameof(BorrowerDto.BorrowerId))]
    public static partial BorrowerDto Map(Borrower borrower);

    [MapProperty(nameof(BorrowerDto.BorrowerId), nameof(Borrower.Id))]
    public static partial Borrower Map(
        BorrowerDto borrowerDto);
}
