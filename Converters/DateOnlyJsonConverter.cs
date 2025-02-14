using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wafi.SampleTest.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateStr = reader.GetString();
        return dateStr != null 
            ? DateOnly.ParseExact(dateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture)
            : default;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat, System.Globalization.CultureInfo.InvariantCulture));
    }
}