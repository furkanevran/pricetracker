FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["pricetracker.infra/pricetracker.infra.csproj", "pricetracker.infra/"]
RUN dotnet restore "pricetracker.infra/pricetracker.infra.csproj"
COPY . .
WORKDIR "/src/pricetracker.infra"
RUN dotnet build "pricetracker.infra.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pricetracker.infra.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pricetracker.infra.dll"]
