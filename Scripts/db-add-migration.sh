#!/bin/bash -e

echo "Migration Name: $1";
dotnet ef migrations add \
  --project ResumeTech.Persistence.EntityFramework/ResumeTech.Persistence.EntityFramework.csproj \
  --startup-project ResumeTech.WebApp/ResumeTech.WebApp.csproj \
  --context ResumeTech.Persistence.EntityFramework.EFCoreContext \
  --configuration Debug "$1" \
  --output-dir Database/Migrations -- \
  --environment Local