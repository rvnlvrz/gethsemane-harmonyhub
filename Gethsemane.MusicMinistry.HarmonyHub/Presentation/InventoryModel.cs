using System.ComponentModel;

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

[Bindable(BindableSupport.Yes)]
public partial record InventoryViewModel
{
    private readonly INavigator _navigator;

    public InventoryViewModel(
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;
    }

    public string? Title => "Inventory";

    public IState<string> Name => State<string>.Value(this, () => string.Empty);

    public async Task GoToSecond()
    {
        var name = await Name;
        await _navigator.NavigateViewModelAsync<SecondModel>(this, data: new Entity(name!));
    }
}
