# Build script docker build -t nihfix.postgres.mcp -f ./NihFix.Postgres.Mcp/Dockerfile .
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NihFix.Postgres.Mcp/NihFix.Postgres.Mcp.csproj", "NihFix.Postgres.Mcp/"]
RUN dotnet restore "NihFix.Postgres.Mcp/NihFix.Postgres.Mcp.csproj"
COPY . .
WORKDIR "/src/NihFix.Postgres.Mcp"
RUN dotnet build "NihFix.Postgres.Mcp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NihFix.Postgres.Mcp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NihFix.Postgres.Mcp.dll"]
