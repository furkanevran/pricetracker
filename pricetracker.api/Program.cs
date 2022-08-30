using PriceTracker.Extractor;
using PriceTracker.Extractor.Extractors;
using PriceTracker.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExtractors();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/amazon/price", async (string amazonUrl, AmazonPriceExtractor extractor) => await extractor.ExtractPrice(amazonUrl));
app.MapPost("/trendyol/price", async (string trendyolUrl, TrendyolPriceExtractor extractor) => await extractor.ExtractPrice(trendyolUrl));
app.MapPost("/hepsiburada/price", async (string hepsiburadaUrl, HepsiburadaPriceExtractor extractor) => await extractor.ExtractPrice(hepsiburadaUrl));
app.MapPost("/watsons/price", async (string watsonsUrl, WatsonsPriceExtractor extractor) => await extractor.ExtractPrice(watsonsUrl));

app.MapPost("/price", async (string url, IExtractor extractor) => await extractor.ExtractPrice(url));

app.Run();