using System.Text.Json.Serialization;

namespace Gethsemane.MusicMinistry.HarmonyHub.Models;

public partial class Secret
{
    [JsonPropertyName("value")] public string? Value { get; set; }
}
