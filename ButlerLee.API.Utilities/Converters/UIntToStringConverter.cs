using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ButlerLee.API.Converters
{
    public class UIntToStringConverter : JsonConverter<uint>
    {
        public override uint Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out uint number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                if (uint.TryParse(reader.GetString(), out number))
                {
                    return number;
                }
            }

            return reader.GetUInt32();
        }

        public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
