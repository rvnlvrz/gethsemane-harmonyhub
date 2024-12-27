using System.Text.Json.Serialization;
using Refit;

namespace Gethsemane.MusicMinistry.HarmonyHub.Configuration;

public interface ISecretsClient
{
    [Get("/")]
    Task<ApiResponse<Secret>> GetSecretVersionAsync(CancellationToken ct,
        [AliasAs("secretName")] string secretName,
        [AliasAs("secretVersion")] string version);
}

public partial class Secret
{
    [JsonPropertyName("value")] public string? Value { get; set; }
}
