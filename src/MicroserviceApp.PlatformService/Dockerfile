FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-stage
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final-stage
WORKDIR /app
COPY --from=build-stage /app/out .
ENTRYPOINT ["dotnet", "MicroserviceApp.PlatformService.dll"]