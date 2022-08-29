using System.Text.Json;
using HtmlAgilityPack;

namespace pricetracker.extractor.Extractors.Amazon;

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
        return metadata![0].priceAmount;
    }
    
    private record AmazonMetadata(string displayPrice,
        double priceAmount,
        string currencySymbol,
        string integerValue,
        string decimalSeparator,
        string fractionalValue,
        string symbolPosition,
        bool hasSpace,
        bool showFractionalPartIfEmpty,
        string offerListingId,
        string locale,
        string buyingOptionType);
}
