using System.IdentityModel.Tokens.Jwt;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Configuration;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;

public class AuthZeroService : IAuthZeroService
{
    private readonly IConfiguration _configuration;
    private readonly ITokenCache _tokenCache;
    private readonly Auth0Client _client;

    public AuthZeroService(IConfiguration configuration, ITokenCache tokenCache)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(tokenCache);

        _configuration = configuration;
        _tokenCache = tokenCache;
        _client = new Auth0Client(
            new Auth0ClientOptions
            {
                Domain = _configuration["Auth0:Domain"],
                ClientId = _configuration["Auth0:ClientId"],
                RedirectUri = _configuration["Auth0:RedirectUri"],
                PostLogoutRedirectUri = _configuration["Auth0:RedirectUri"],
                Scope = "openid profile email api offline_access"
            });
    }

    public async Task<(string? name, string? email)> GetUserInfoAsync(CancellationToken ct =
        default)
    {
        var tokens = await _tokenCache.GetAsync(ct);

        var hasValue = tokens.TryGetValue(TokenCacheExtensions.IdTokenKey, out var idToken);

        if (hasValue)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(idToken))
            {
                return (null, null);
            }

            var jwtContents = handler.ReadJwtToken(idToken);

            var name = jwtContents.Payload["name"].ToString() ?? null;
            var email = jwtContents.Payload["email"].ToString() ?? null;

            return (name, email);
        }

        return (null, null);
    }

    public async Task<LoginResult> LoginAsync(CancellationToken ct)
    {
        var loginResult = await _client.LoginAsync(
            cancellationToken: ct,
            extraParameters: new
            {
                audience = _configuration["Auth0:Audience"]
            });

        return loginResult;
    }

    public async Task<BrowserResultType> LogoutAsync(CancellationToken ct)
    {
        var logoutResult = await _client.LogoutAsync(cancellationToken: ct);
        return logoutResult;
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(string token,
        CancellationToken ct)
    {
        var refreshResult = await _client.RefreshTokenAsync(token, ct);
        return refreshResult;
    }
}
