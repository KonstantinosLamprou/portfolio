# ------------ Phase 1: Build -------------
FROM [mcr.microsoft.com/dotnet/sdk:10.0](https://mcr.microsoft.com/dotnet/sdk:10.0) AS build
WORKDIR /src

# Projektdateien kopieren und wiederherstellen
COPY ["src/Backend.Api/Backend.Api.csproj", "src/Backend.Api/"]
COPY ["src/Backend.Infrastructure/Backend.Infrastructure.csproj", "src/Backend.Infrastructure/"]
COPY ["src/Backend.Application/Backend.Application.csproj", "src/Backend.Application/"]
COPY ["src/Backend.Domain/Backend.Domain.csproj", "src/Backend.Domain/"]
RUN dotnet restore "src/Backend.Api/Backend.Api.csproj"

# Restlichen Code kopieren und publishen
COPY . .
RUN dotnet publish "src/Backend.Api/Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------------- Phase 2: Run -------------
FROM [mcr.microsoft.com/dotnet/aspnet:10.0-noble](https://mcr.microsoft.com/dotnet/aspnet:10.0-noble) AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.dll"]
