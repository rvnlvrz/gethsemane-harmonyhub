using System.ComponentModel;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories.Inventory;

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

[Bindable(BindableSupport.Yes)]
public partial record InventoryModel
{
    private readonly INavigator _navigator;
    private readonly IAtlasClient _atlasClient;

    public InventoryModel(INavigator navigator, IAtlasClient atlasClient)
    {
        _navigator = navigator;
        _atlasClient = atlasClient;
    }

    public string? Title => "Inventory";
}
