using Microsoft.UI.Xaml.Data;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Gethsemane.MusicMinistry.HarmonyHub.Converters;

public class BorrowedCountVisibilityConverter : IValueConverter
{
    public string? Format { get; set; }

    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return null;
        if (value is not int count) return value.ToString();

        return count > 0 ? "Visible" : "Collapsed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException("Only one-way conversion is supported.");
}
