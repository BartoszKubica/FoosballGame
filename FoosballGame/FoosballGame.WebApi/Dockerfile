#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FoosballGame.WebApi/FoosballGame.WebApi.csproj", "FoosballGame.WebApi/"]
COPY ["FoosballGame.Infrastructure/FoosballGame.Infrastructure.csproj", "FoosballGame.Infrastructure/"]
COPY ["FoosballGame.Domain/FoosballGame.Domain.csproj", "FoosballGame.Domain/"]
COPY ["FoosballGame.Contracts/FoosballGame.Contracts.csproj", "FoosballGame.Contracts/"]
RUN dotnet restore "FoosballGame.WebApi/FoosballGame.WebApi.csproj"
COPY . .
WORKDIR "/src/FoosballGame.WebApi"
RUN dotnet build "FoosballGame.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoosballGame.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoosballGame.WebApi.dll"]
