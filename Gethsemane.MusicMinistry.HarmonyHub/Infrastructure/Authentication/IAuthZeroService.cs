using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;

public interface IAuthZeroService
{
    Task<LoginResult> LoginAsync(CancellationToken cancellationToken);
    Task<BrowserResultType> LogoutAsync(CancellationToken cancellationToken);

    Task<RefreshTokenResult> RefreshTokenAsync(string token,
        CancellationToken cancellationToken);

    string? GetCurrentName(CancellationToken ct);
    string? GetCurrentEmail(CancellationToken ct);
}
