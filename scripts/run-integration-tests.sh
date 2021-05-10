#!/bin/bash 

if [ "$1" == "-b" ] 
then
    docker-compose -f docker-compose.yml -f docker-compose.test.override.yml up --build --abort-on-container-exit integration-tests
else
    docker-compose -f docker-compose.yml -f docker-compose.test.override.yml up --no-build --abort-on-container-exit integration-tests
fi