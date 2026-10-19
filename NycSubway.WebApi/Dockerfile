FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NycSubway.WebApi/NycSubway.WebApi.csproj", "NycSubway.WebApi/"]
COPY ["NycSubway.Core/NycSubway.Core.csproj", "NycSubway.Core/"]
COPY ["NycSubway.Database/NycSubway.Database.csproj", "NycSubway.Database/"]
COPY ["NycSubway.Infrastructure/NycSubway.Infrastructure.csproj", "NycSubway.Infrastructure/"]
RUN dotnet restore "NycSubway.WebApi/NycSubway.WebApi.csproj"

COPY . .
WORKDIR "/src/NycSubway.WebApi"
RUN dotnet build "NycSubway.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NycSubway.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NycSubway.WebApi.dll"]