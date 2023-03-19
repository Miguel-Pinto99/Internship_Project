using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project1.JsonConverter
{
    public class NullableTimeSpanJsonConverter : JsonConverter<TimeSpan?>
    {

        public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert is null)
            {
                throw new ArgumentNullException(nameof(typeToConvert));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return TimeSpan.FromTicks(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value.Ticks);
            }
            else
            {
                writer.WriteNullValue();
            }

        }
    }

}
