using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PriceTracker.API;
using PriceTracker.API.Attributes;
using PriceTracker.API.Endpoints;
using PriceTracker.API.Helpers;
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

app.MapMinimalEndpoints();


app.Run();