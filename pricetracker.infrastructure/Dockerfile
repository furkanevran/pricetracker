FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["pricetracker.infrastructure/pricetracker.infrastructure.csproj", "pricetracker.infrastructure/"]
RUN dotnet restore "pricetracker.infrastructure/pricetracker.infrastructure.csproj"
COPY . .
WORKDIR "/src/pricetracker.infrastructure"
RUN dotnet build "pricetracker.infrastructure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pricetracker.infrastructure.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pricetracker.infrastructure.dll"]
