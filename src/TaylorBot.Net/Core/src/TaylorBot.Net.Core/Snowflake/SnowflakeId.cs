﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaylorBot.Net.Core.Snowflake;

[JsonConverter(typeof(SnowflakeIdConverter))]
public record SnowflakeId(ulong Id)
{
    public SnowflakeId(string id) : this(ulong.Parse(id)) { }

    public override string ToString() => $"{Id}";

    public static implicit operator string(SnowflakeId id) => $"{id}";
    public static implicit operator ulong(SnowflakeId id) => id.Id;
    public static implicit operator SnowflakeId(string id) => new(id);
    public static implicit operator SnowflakeId(ulong id) => new(id);
}

public class SnowflakeIdConverter : JsonConverter<SnowflakeId>
{
    public override SnowflakeId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var rawString = reader.GetString();
        return rawString == null ? throw new JsonException() : new(rawString);
    }

    public override void Write(Utf8JsonWriter writer, SnowflakeId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
