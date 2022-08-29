﻿using System.Text.Json.Serialization;

namespace pricetracker.extractor.Extractors.Trendyol.Entities;

public class Value
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("id")] public int Id { get; set; }
}