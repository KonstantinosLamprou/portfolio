# ------------ Phase 1: Build -------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Alle .csproj-Dateien kopieren (Inklusive dem korrekten 'src/' Pfad von deinem PC)
COPY ["src/Backend.Api/Backend.Api.csproj", "src/Backend.Api/"]
COPY ["src/Backend.Infrastructure/Backend.Infrastructure.csproj", "src/Backend.Infrastructure/"]
COPY ["src/Backend.Application/Backend.Application.csproj", "src/Backend.Application/"]
COPY ["src/Backend.Domain/Backend.Domain.csproj", "src/Backend.Domain/"]

# NuGet-Pakete wiederherstellen
RUN dotnet restore "src/Backend.Api/Backend.Api.csproj"

# gesamten restlichen Quellcode in den Container kopieren
COPY . .

# Das API-Projekt kompilieren und im Release-Modus bereitstellen
RUN dotnet publish "src/Backend.Api/Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------------- Phase 2: Run (Das schlanke finale Image für die Ausführung) -------------

FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS final
WORKDIR /app
EXPOSE 8080

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Kopiere nur die fertig gebauten Dateien aus der Build-Phase hier rüber
COPY --from=build /app/publish .

# Startbefehl für dein Backend
ENTRYPOINT ["dotnet", "Backend.Api.dll"]