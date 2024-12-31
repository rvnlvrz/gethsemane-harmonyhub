using Gethsemane.MusicMinistry.HarmonyHub.Converters;

namespace Gethsemane.MusicMinistry.HarmonyHub.Tests.Converters;

[TestFixture]
[TestOf(typeof(DueDateTimeToStringConverter))]
public class DueDateTimeToStringConverterTests
{
    [TestCaseSource(nameof(FutureDateOnlyConvertCases))]
    [TestCaseSource(nameof(PastDateOnlyConvertCases))]
    public void GivenDateOnly_WhenConverted_ShouldConvertToString(
        DateOnly dateOnly,
        string expectedFormat)
    {
        var sut = new DueDateTimeToStringConverter();

        var result = sut.Convert(dateOnly,
            typeof(DateOnly),
            null,
            null);

        Assert.That(result, Is.EqualTo(expectedFormat));
    }

    public static object[] FutureDateOnlyConvertCases =
    [
        new object[] { DateOnly.FromDateTime(DateTime.Now), "Today" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(3)), "3d" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(7)), "1wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(10)), "1wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(11)), "2wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(29)), "4wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(30)), "1mo" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(44)), "1mo" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(45)), "2mo" },
    ];


    public static object[] PastDateOnlyConvertCases =
    [
        new object[] { DateOnly.FromDateTime(DateTime.Now), "Today" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-3)), "-3d" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-7)), "-1wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), "-1wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-11)), "-2wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-29)), "-4wk" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-30)), "-1mo" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-44)), "-1mo" },
        new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-45)), "-2mo" },
    ];
}
