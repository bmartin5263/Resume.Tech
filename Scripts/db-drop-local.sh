#!/bin/bash -e

dotnet ef database drop \
  --project ResumeTech.Persistence.EntityFramework/ResumeTech.Persistence.EntityFramework.csproj \
  --startup-project ResumeTech.WebApp/ResumeTech.WebApp.csproj \
  --context ResumeTech.Persistence.EntityFramework.EFCoreContext \
  --configuration Debug \
  --force -- \
  --environment Local