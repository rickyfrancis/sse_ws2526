using System.Text.Json;

namespace SSE;

public static class RpcCodec
{
    private sealed class Envelope
    {
        public string Type { get; set; } = string.Empty;
        public JsonElement Payload { get; set; }
    }

    private sealed class RpcPayload
    {
        public string Name { get; set; } = string.Empty;
        public string[] Args { get; set; } = [];
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string Marshall(object input)
    {
        var envelope = input switch
        {
            double d => CreateEnvelope("double", d),
            string s => CreateEnvelope("string", s),
            string[] array => CreateEnvelope("stringArray", array),
            RpcRequest rpc => CreateEnvelope("rpc", new RpcPayload { Name = rpc.Name, Args = rpc.Args }),
            _ => throw new ArgumentException($"Unsupported message type: {input.GetType().Name}")
        };

        return JsonSerializer.Serialize(envelope, JsonOptions);
    }

    public static object Demarshall(string line)
    {
        var envelope = JsonSerializer.Deserialize<Envelope>(line, JsonOptions)
            ?? throw new FormatException("Invalid payload: empty envelope");

        return envelope.Type switch
        {
            "double" => envelope.Payload.GetDouble(),
            "string" => envelope.Payload.GetString() ?? string.Empty,
            "stringArray" => envelope.Payload.Deserialize<string[]>(JsonOptions) ?? [],
            "rpc" => ParseRpc(envelope.Payload),
            _ => throw new FormatException($"Unknown message type '{envelope.Type}'")
        };
    }

    private static RpcRequest ParseRpc(JsonElement payload)
    {
        var rpc = payload.Deserialize<RpcPayload>(JsonOptions)
            ?? throw new FormatException("Invalid rpc payload");
        return new RpcRequest(rpc.Name, rpc.Args ?? []);
    }

    private static Envelope CreateEnvelope<TPayload>(string type, TPayload payload)
    {
        return new Envelope
        {
            Type = type,
            Payload = JsonSerializer.SerializeToElement(payload, JsonOptions)
        };
    }
}
