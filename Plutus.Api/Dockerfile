FROM mcr.microsoft.com/dotnet/aspnet:6.0.0-preview.7-bullseye-slim-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.100-preview.7-bullseye-slim-amd64 AS build
WORKDIR /src
COPY ["Plutus.Api/Plutus.Api.csproj", "Plutus.Api/"]
RUN dotnet restore "Plutus.Api/Plutus.Api.csproj"
COPY . .
WORKDIR "/src/Plutus.Api"
RUN dotnet build "Plutus.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plutus.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Plutus.Api.dll"]


