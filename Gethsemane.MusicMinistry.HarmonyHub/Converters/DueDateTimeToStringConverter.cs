using System.Text;
using Microsoft.UI.Xaml.Data;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Gethsemane.MusicMinistry.HarmonyHub.Converters;

public class DueDateTimeToStringConverter : IValueConverter
{
    public string? Format { get; set; }

    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not DateOnly dueDate) return null;

        var now = DateOnly.FromDateTime(DateTime.Now);
        var daysBeforeDue = dueDate.DayNumber - now.DayNumber;

        var stringBuilder = new StringBuilder(3);

        switch (GetAbsoluteValue(daysBeforeDue))
        {
            case 0:
                stringBuilder.Append("Today");
                return stringBuilder.ToString();
            case < 7:
                stringBuilder.Append(daysBeforeDue);
                stringBuilder.Append('d');
                return stringBuilder.ToString();
            case < 30:
                var weeks = (int)Math.Round((double)daysBeforeDue / 7);
                stringBuilder.Append(weeks);
                stringBuilder.Append("wk");
                return stringBuilder.ToString();
            case >= 30:
                var months = (int)Math.Round((double)daysBeforeDue / 30);
                stringBuilder.Append(months);
                stringBuilder.Append("mo");
                return stringBuilder.ToString();
            default:
                stringBuilder.Append("N/A");
                return stringBuilder.ToString();
        }
    }

    private static int GetAbsoluteValue(int number)
    {
        return number < 0 ? -number : number;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException("Only one-way conversion is supported.");
}
