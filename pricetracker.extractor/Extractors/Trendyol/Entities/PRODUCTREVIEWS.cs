﻿using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class PRODUCTREVIEWS
{
    [JsonPropertyName("translateKey")] public string TranslateKey { get; set; } = null!;
    [JsonPropertyName("selector")] public string Selector { get; set; } = null!;
}