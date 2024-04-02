using Newtonsoft.Json;

namespace DicePoker.Utilities;

internal class JsonQTableConverter : JsonConverter<Dictionary<byte[], Dictionary<byte[], double>>>
{
    public override Dictionary<byte[], Dictionary<byte[], double>>? ReadJson(
        JsonReader reader,
        Type objectType,
        Dictionary<byte[], Dictionary<byte[], double>>? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var qTable = new Dictionary<byte[], Dictionary<byte[], double>>(ArrayComparer<byte>.Instance);

        while (reader.Read() && reader.TokenType == JsonToken.StartObject)
        {
            reader.Read(); //name
            reader.Read(); //array

            var state = ReadArray(reader);

            reader.Read(); //name
            reader.Read(); //array

            var actions = new Dictionary<byte[], double>(ArrayComparer<byte>.Instance);

            while (reader.Read() && reader.TokenType == JsonToken.StartObject)
            {
                reader.Read(); //name
                reader.Read(); //array

                var action = ReadArray(reader);

                reader.Read(); //name

                var qValue = reader.ReadAsDouble();

                actions.Add(action, (double)qValue!);

                reader.Read(); //end object
            }

            qTable.Add(state, actions);

            reader.Read(); //end object
        }

        return qTable;
    }

    private static byte[] ReadArray(JsonReader reader)
    {
        List<byte> array = [];

        while (true)
        {
            var number = reader.ReadAsInt32();

            if (number is null)
            {
                break;
            }

            array.Add((byte)number);
        }

        return array.ToArray();
    }

    public override void WriteJson(
        JsonWriter writer,
        Dictionary<byte[],
        Dictionary<byte[], double>>? value,
        JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartArray();
        foreach (var (state, actions) in value)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("state");
            writer.WriteStartArray();
            foreach (var item in state)
            {
                writer.WriteValue(item);
            }
            writer.WriteEndArray();

            writer.WritePropertyName("actions");
            writer.WriteStartArray();
            foreach (var (action, qValue) in actions)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("action");
                writer.WriteStartArray();
                foreach (var item in action)
                {
                    writer.WriteValue(item);
                }
                writer.WriteEndArray();

                writer.WritePropertyName("qValue");
                writer.WriteValue(qValue);

                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }
}
