FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
COPY --from=sudobmitch/base:scratch / /
RUN useradd -m app
COPY . /code
# COPY entrypoint.sh /usr/bin/

RUN mv ./code/scripts/entrypoint.sh /usr/bin

ENTRYPOINT ["/usr/bin/entrypoint.sh"]
# CMD ["dotnet", "test", "code/Plutus.Api.IntegrationTests/Plutus.Api.IntegrationTests.csproj"]
USER app