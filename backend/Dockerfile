# ------------ Phase 1: Build -------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Backend.Api/Backend.Api.csproj", "src/Backend.Api/"]
COPY ["src/Backend.Infrastructure/Backend.Infrastructure.csproj", "src/Backend.Infrastructure/"]
COPY ["src/Backend.Application/Backend.Application.csproj", "src/Backend.Application/"]
COPY ["src/Backend.Domain/Backend.Domain.csproj", "src/Backend.Domain/"]

RUN dotnet restore "src/Backend.Api/Backend.Api.csproj"
COPY . .

RUN dotnet publish "src/Backend.Api/Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:PublishReadyToRun=false

# ------------- Phase 2: Run -------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS final
WORKDIR /app
EXPOSE 8080

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV DOTNET_gcServer=0
ENV DOTNET_TieredPGO=0

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.dll"]