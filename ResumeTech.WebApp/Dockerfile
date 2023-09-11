FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ResumeTech.Application/ResumeTech.Application.csproj", "ResumeTech.Application/"]
RUN dotnet restore "ResumeTech.Application/ResumeTech.Application.csproj"
COPY . .
WORKDIR "/src/ResumeTech.Application"
RUN dotnet build "ResumeTech.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ResumeTech.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeTech.Application.dll"]
