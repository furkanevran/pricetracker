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

        while (json.Read())
        {
            if (json.TokenType == JsonTokenType.PropertyName)
                lastPropertyName = json.GetString()!;
            else if (json.TokenType == JsonTokenType.EndObject)
                currentDepth--;
            else if (json.TokenType == JsonTokenType.StartObject)
                currentDepth++;

            if (targetDepth > 0 && targetDepth > currentDepth)
                continue;

            if (json.TokenType == JsonTokenType.StartObject)
            {
                if (lastPropertyName != path[pathDepth])
                    targetDepth = currentDepth - 1;
                else
                    pathDepth++;
            }
            else if (pathDepth == path.Length - 1 && lastPropertyName == path[^1] && json.TokenType is JsonTokenType.String or JsonTokenType.Number)
            {
                var value = Encoding.UTF8.GetString(json.ValueSpan);
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
        }
        
        // var currentPath = new string[path.Length];
        // string previousTokenName = null!;
        // var pathDepth = 0;
        // var currentDepth = 0;
        // var targetDepth = -1;
        //
        // var sequenceEqual = true;
        //
        // while (json.Read())
        // {
        //     if (json.TokenType == JsonTokenType.EndObject)
        //     {
        //         currentDepth--;
        //         if (currentDepth == targetDepth)
        //         {
        //             pathDepth--;
        //             targetDepth = -1;
        //         }
        //         sequenceEqual = currentPath[pathDepth] == path[pathDepth];
        //     }
        //     else if (json.TokenType == JsonTokenType.StartObject)
        //     {
        //         currentDepth++;
        //     }
        //
        //     if (!sequenceEqual && targetDepth > currentDepth)
        //         continue;
        //     
        //     if (json.TokenType == JsonTokenType.PropertyName)
        //         previousTokenName = json.GetString()!;
        //     else if (json.TokenType == JsonTokenType.StartObject)
        //     {
        //         currentPath[pathDepth] = previousTokenName;
        //         sequenceEqual = currentPath[pathDepth] == path[pathDepth];
        //         if (!sequenceEqual)
        //             targetDepth = currentDepth;
        //         
        //         pathDepth++;
        //         currentDepth++;
        //     }
        //     else if (json.TokenType is JsonTokenType.String or JsonTokenType.Number)
        //     {
        //         if (pathDepth != path.Length - 1) continue;
        //         var value = Encoding.UTF8.GetString(json.ValueSpan);
        //         return (T)Convert.ChangeType(value, typeof(T));
        //     }
        // }
        
        return default;
    }
}