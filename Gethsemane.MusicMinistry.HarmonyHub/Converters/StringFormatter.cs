using System.Globalization;
using Microsoft.UI.Xaml.Data;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Gethsemane.MusicMinistry.HarmonyHub.Converters;

public class StringFormatter : IValueConverter
{
    public string? Format { get; set; }

    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return null;
        if ((Format ?? parameter as string) is not { } format) return value.ToString();

        return string.Format(
            CultureInfo.CurrentUICulture,
            format,
            value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException("Only one-way conversion is supported.");
}
