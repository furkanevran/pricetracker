using System.Globalization;
using System.Text;
using System.Text.Json;

namespace PriceTracker.Extractor;

public static class JsonPropertyParser
{
    public static T? TryParse<T>(string jsonText, params string[] path)
    {
        jsonText = jsonText.Trim();

        var jsonSpan = Encoding.UTF8.GetBytes(jsonText).AsSpan();
        var json = new Utf8JsonReader(jsonSpan);
        json.Read(); // skip the root StartObject

        var pathDepth = 0;
        var currentDepth = 0;
        var targetDepth = -1;
        string lastPropertyName = null!;
        var lastPathPart = path[^1];

        while (json.Read())
        {
            currentDepth = GetCurrentDepth(json, currentDepth);
            
            if (json.TokenType == JsonTokenType.PropertyName)
                lastPropertyName = json.GetString()!;
            
            if (targetDepth > 0 && targetDepth > currentDepth)
                continue;
            
            if (json.TokenType == JsonTokenType.StartObject)
            {
                if (lastPropertyName != path[pathDepth])
                    targetDepth = currentDepth - 1;
                else
                    pathDepth++;
            }
            else if (pathDepth == path.Length - 1 &&
                     lastPropertyName == lastPathPart &&
                     json.TokenType is JsonTokenType.String or JsonTokenType.Number)
            {
                var convertType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                
                var value = Encoding.UTF8.GetString(json.ValueSpan);
                return (T)Convert.ChangeType(value, convertType, CultureInfo.InvariantCulture);
            }
        }
        
        return default;
    }

    private static int GetCurrentDepth(Utf8JsonReader json, int currentDepth)
    {
        if (json.TokenType == JsonTokenType.EndObject)
            currentDepth--;
        else if (json.TokenType == JsonTokenType.StartObject) 
            currentDepth++;

        return currentDepth;
    }
}