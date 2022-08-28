FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["pricetracker.api/pricetracker.api.csproj", "pricetracker.api/"]
RUN dotnet restore "pricetracker.api/pricetracker.api.csproj"
COPY . .
WORKDIR "/src/pricetracker.api"
RUN dotnet build "pricetracker.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pricetracker.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pricetracker.api.dll"]
