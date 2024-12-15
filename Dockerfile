FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

COPY ["src/GigAuth.Api/GigAuth.Api.csproj", "GigAuth.Api/"]
RUN dotnet restore "GigAuth.Api/GigAuth.Api.csproj"

COPY src/ ./src/

WORKDIR "/src/GigAuth.Api"
RUN dotnet build "GigAuth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GigAuth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base as final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GigAuth.Api.dll"]