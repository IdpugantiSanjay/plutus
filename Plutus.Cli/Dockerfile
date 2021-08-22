#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Plutus.Cli/Plutus.Cli.csproj", "Plutus.Cli/"]
COPY ["Plutus.Application/Plutus.Application.csproj", "Plutus.Application/"]
COPY ["Plutus.Domain/Plutus.Domain.csproj", "Plutus.Domain/"]
RUN dotnet restore "Plutus.Cli/Plutus.Cli.csproj"
COPY . .
WORKDIR "/src/Plutus.Cli"
RUN dotnet build "Plutus.Cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plutus.Cli.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN echo "alias plutus='$(readlink -f ./Plutus.Cli)'" >> ~/.bashrc

ENTRYPOINT ["dotnet", "Plutus.Cli.dll"]