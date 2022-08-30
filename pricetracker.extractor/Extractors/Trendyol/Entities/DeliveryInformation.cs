﻿using System.Text.Json.Serialization;

namespace PriceTracker.Extractor.Extractors.Trendyol.Entities;

public class DeliveryInformation
{
    [JsonPropertyName("isRushDelivery")] public bool IsRushDelivery { get; set; }
    [JsonPropertyName("deliveryDate")] public string DeliveryDate { get; set; } = null!;
    [JsonPropertyName("fastDeliveryOptions")] public List<object> FastDeliveryOptions { get; set; } = null!;
}