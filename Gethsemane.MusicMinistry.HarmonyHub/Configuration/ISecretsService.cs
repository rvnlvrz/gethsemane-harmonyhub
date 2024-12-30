using Refit;

namespace Gethsemane.MusicMinistry.HarmonyHub.Configuration;

public interface ISecretsService
{
    [Get("/")]
    Task<ApiResponse<Secret>> GetSecretVersionAsync([AliasAs("secretName")] string secretName,
        [AliasAs("secretVersion")] string version,
        CancellationToken ct);
}
