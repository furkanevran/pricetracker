using System.Text.Json;
using HtmlAgilityPack;
using PriceTracker.Extractor.Extractors.Amazon.Entities;

namespace PriceTracker.Extractor.Extractors.Amazon;

public class AmazonExtractor : IExtractor
{
    public async Task<double> ExtractPrice(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        
        var jsonMetadata =  doc.DocumentNode
            .Descendants("div")
            .First(div => div.HasClass("twister-plus-buying-options-price-data"))
            .InnerText;
        
        var metadata = JsonSerializer.Deserialize<AmazonMetadata[]>(jsonMetadata);
        return metadata![0].PriceAmount;
    }
}
