FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base-stage
# Install cultures for alpine.
RUN apk add --no-cache icu-libs 
# 0 - access to cultural data; 1 - run in invariant mode. In docker alpine default value is 1.
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT 0 

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-stage
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore --no-cache

COPY . ./
RUN dotnet publish -c Release -o out

FROM base-stage AS final-stage
COPY --from=build-stage /app/out .
ENTRYPOINT ["dotnet", "MicroserviceApp.CommandsService.dll"]