#!/bin/bash 

if [ -z "$1" ] 
then
    echo "Exiting as migration name is not specified"
    exit 1
else 
    dotnet ef migrations add $1 --project ./Plutus.Infrastructure/Plutus.Infrastructure.csproj --startup-project ./Plutus.Api/Plutus.Api.csproj 
fi

