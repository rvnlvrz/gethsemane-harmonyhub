using Gethsemane.MusicMinistry.HarmonyHub.Models.Inventory;
using Microsoft.UI.Xaml.Data;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Gethsemane.MusicMinistry.HarmonyHub.Converters;

public class ItemTypeEnumConverter : IValueConverter
{
    public string? Format { get; set; }

    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return null;
        if (value is not ItemType itemType) return value.ToString();

        return itemType switch
        {
            ItemType.SheetMusic => "Sheet Music",
            ItemType.SongBook => "Song Book",
            _ => "Undefined"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException("Only one-way conversion is supported.");
}
