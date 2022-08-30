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

app.MapPost("/amazon/price", async (string amazonUrl, AmazonExtractor extractor) => await extractor.ExtractPrice(amazonUrl));
app.MapPost("/trendyol/price", async (string trendyolUrl, TrendyolExtractor extractor) => await extractor.ExtractPrice(trendyolUrl));
app.MapPost("/hepsiburada/price", async (string hepsiburadaUrl, HepsiburadaExtractor extractor) => await extractor.ExtractPrice(hepsiburadaUrl));

app.Run();