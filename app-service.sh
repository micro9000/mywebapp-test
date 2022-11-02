#!/bin/bash

RG="az-204-rg"
location="eastasia"
planName="az204aspnetappplan"
webappName="az204aspnetapp202201"
githubRepo="https://github.com/micro9000/mywebapp-test"

echo "Creating Resource Group"
az group create --name $RG --location $location

echo "Creating App service plan"
az appservice plan create --name $planName --resource-group $RG --sku FREE

echo "Creating App service"
az webapp create --name $webappName --resource-group $RG --plan $planName
