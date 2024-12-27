using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

[Bindable(BindableSupport.Yes)]
public partial record MainModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAuthZeroClient _authZeroClient;
    private readonly INavigator _navigator;

    public MainModel(
        IOptions<AppConfig> appInfo,
        INavigator navigator,
        IAuthenticationService authenticationService,
        IAuthZeroClient authZeroClient)
    {
        _navigator = navigator;
        _authenticationService = authenticationService;
        _authZeroClient = authZeroClient;
        Title = "Harmony Hub";
        Title += $" - {appInfo?.Value?.Environment}";
    }

    public IState<string> User => State<string>.Value(this, () => "Logged out");

    public IState<string> Email =>
        State<string>.Value(this, () => "Please sign-in");

    public ICommand Authenticate => new AsyncRelayCommand(AuthenticateImpl);

    public ICommand Logout => new AsyncRelayCommand(LogoutImpl);

    public string? Title { get; }

    public IState<string> Name => State<string>.Value(this, () => string.Empty);

    private async Task LogoutImpl(CancellationToken ct)
    {
        await _authenticationService.LogoutAsync();
        await User.UpdateAsync(x => _authZeroClient.GetCurrentName(), ct);
        await Email.UpdateAsync(x => _authZeroClient.GetCurrentEmail(), ct);
    }

    private async Task AuthenticateImpl(CancellationToken ct)
    {
        await _authenticationService.LoginAsync(cancellationToken: ct);
        await User.UpdateAsync(x => _authZeroClient.GetCurrentName(), ct);
        await Email.UpdateAsync(x => _authZeroClient.GetCurrentEmail(), ct);
    }

    public async Task GoToSecond()
    {
        var name = await Name;
        await _navigator.NavigateViewModelAsync<SecondModel>(
            this,
            data: new Entity(name!));
    }
}
