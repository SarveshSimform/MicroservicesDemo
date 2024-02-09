FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Newsletter.ApiGateway/Newsletter.ApiGateway.csproj", "Newsletter.ApiGateway/"]
RUN dotnet restore "Newsletter.ApiGateway/Newsletter.ApiGateway.csproj"
COPY . .
WORKDIR "/src/Newsletter.ApiGateway"
RUN dotnet build "Newsletter.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Newsletter.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Newsletter.ApiGateway.dll"]
