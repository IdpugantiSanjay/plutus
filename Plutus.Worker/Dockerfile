#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Plutus.Worker/Plutus.Worker.csproj", "Plutus.Worker/"]
RUN dotnet restore "Plutus.Worker/Plutus.Worker.csproj"
COPY . .
WORKDIR "/src/Plutus.Worker"
RUN dotnet build "Plutus.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plutus.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plutus.Worker.dll"]