using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;

public interface IAuthZeroClient
{
    Task<LoginResult> LoginAsync(CancellationToken cancellationToken);
    Task<BrowserResultType> LogoutAsync(CancellationToken cancellationToken);

    Task<RefreshTokenResult> RefreshTokenAsync(string token,
        CancellationToken cancellationToken);

    string? GetCurrentName();
    string? GetCurrentEmail();
}
