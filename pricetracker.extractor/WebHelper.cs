namespace PriceTracker.Extractor;

public class WebHelper
{
    public static async Task<string> GetString(string url)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en,en-US;q=0.5");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.hepsiburada.com/");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-GPC", "1");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Pragma", "no-cache");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "no-cache");

        var html = await client.GetStringAsync(url);
        return html;
    }
}