namespace Gethsemane.MusicMinistry.HarmonyHub.Models;

public record AppConfig
{
    public string? Environment { get; init; }
}

public record InventoryConfig
{
    public int DueDateDayOffset { get; init; } = 7;
}
