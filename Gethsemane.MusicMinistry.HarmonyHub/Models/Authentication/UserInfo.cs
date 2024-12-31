using System.Text.Json;

namespace Gethsemane.MusicMinistry.HarmonyHub.Models.Authentication;

public partial class UserInfo
{
    public string? Sub { get; set; }
    public string? Name { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? MiddleName { get; set; }
    public string? Nickname { get; set; }
    public string? PreferredUsername { get; set; }
    public Uri? Profile { get; set; }
    public Uri? Picture { get; set; }
    public Uri? Website { get; set; }
    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
    public string? Gender { get; set; }
    public DateTimeOffset? Birthdate { get; set; }
    public string? Zoneinfo { get; set; }
    public string? Locale { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? PhoneNumberVerified { get; set; }
    public Address? Address { get; set; }
    public long? UpdatedAt { get; set; }
}

public partial class Address
{
    public string? Country { get; set; }
}

public partial class UserInfo
{
    public static UserInfo? FromJson(string json) =>
        JsonSerializer.Deserialize<UserInfo>(json);
}

public static class Serialize
{
    public static string ToJson(this UserInfo self) => JsonSerializer.Serialize(self);
}
