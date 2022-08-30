using Microsoft.AspNetCore.Mvc;
using PriceTracker.API;
using PriceTracker.Extractor;
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

app.MapPost("/price", async ([FromBody] UrlRequest urlRequest, IExtractor extractor) => await extractor.ExtractPrice(urlRequest.Url));

app.Run();