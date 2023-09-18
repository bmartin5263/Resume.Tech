#!/bin/bash -e

dotnet ef database update \
  --project ResumeTech.Persistence.EntityFramework/ResumeTech.Persistence.EntityFramework.csproj \
  --startup-project ResumeTech.WebApp/ResumeTech.WebApp.csproj \
  --context ResumeTech.Persistence.EntityFramework.EFCoreContext \
  --configuration Debug -- \
  --environment Local