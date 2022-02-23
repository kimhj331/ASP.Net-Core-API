using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ButlerLee.API.Converters
{
    public class MinDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, [AllowNull] DateTime existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
                return DateTime.MinValue;

            return (DateTime)reader.Value;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] DateTime value, Newtonsoft.Json.JsonSerializer serializer)
        {
            DateTime dateTimeValue = value;
            if (dateTimeValue == DateTime.MinValue)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(value.ToString());
        }
        //public override DateTime ReadJson(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    if (string.IsNullOrEmpty(reader.GetString()) == true)
        //        return DateTime.MinValue;

        //    return DateTime.Parse(reader.GetString());
        //}

        //public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        //{
        //    DateTime dateTimeValue = value;
        //    if (dateTimeValue == DateTime.MinValue)
        //    {
        //        writer.WriteNullValue();
        //        return;
        //    }

        //    writer.WriteStringValue(value.ToString());
        //}
    }
}
