using Refit;

namespace Gethsemane.MusicMinistry.HarmonyHub.Configuration;

public interface ISecretsClient
{
    [Get("/")]
    Task<ApiResponse<Secret>> GetSecretVersionAsync(CancellationToken ct,
        [AliasAs("secretName")] string secretName,
        [AliasAs("secretVersion")] string version);
}
