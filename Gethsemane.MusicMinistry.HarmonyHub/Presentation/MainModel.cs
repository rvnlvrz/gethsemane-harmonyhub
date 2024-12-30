using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;

namespace Gethsemane.MusicMinistry.HarmonyHub.Presentation;

[Bindable(BindableSupport.Yes)]
public partial record MainModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAuthZeroService _authZeroService;
    private readonly ITokenCache _tokenCache;
    private readonly INavigator _navigator;

    public MainModel(
        IOptions<AppConfig> appInfo,
        INavigator navigator,
        IAuthenticationService authenticationService,
        IAuthZeroService authZeroService,
        ITokenCache tokenCache)
    {
        ArgumentNullException.ThrowIfNull(appInfo);
        ArgumentNullException.ThrowIfNull(navigator);
        ArgumentNullException.ThrowIfNull(authenticationService);
        ArgumentNullException.ThrowIfNull(authZeroService);
        ArgumentNullException.ThrowIfNull(tokenCache);

        _navigator = navigator;
        _authenticationService = authenticationService;
        _authZeroService = authZeroService;
        _tokenCache = tokenCache;
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
        await User.UpdateAsync(_ => _authZeroService.GetCurrentName(ct), ct);
        await Email.UpdateAsync(_ => _authZeroService.GetCurrentEmail(ct), ct);
    }

    private async Task AuthenticateImpl(CancellationToken ct)
    {
        var isAuthenticated = await _authenticationService.IsAuthenticated();

        if (!isAuthenticated)
        {
            await _authenticationService.LoginAsync(cancellationToken: ct);
        }

        await User.UpdateAsync(_ => _authZeroService.GetCurrentName(ct), ct);
        await Email.UpdateAsync(_ => _authZeroService.GetCurrentEmail(ct), ct);
    }
}
