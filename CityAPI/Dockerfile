#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CityAPI.csproj", "CityAPI/"]
RUN dotnet restore "CityAPI/CityAPI.csproj"
WORKDIR "/src/CityAPI"
COPY . .
RUN dotnet build "CityAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CityAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CityAPI.dll"]