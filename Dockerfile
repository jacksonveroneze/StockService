FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env

WORKDIR /app

ADD ./src app

RUN dotnet restore app/JacksonVeroneze.StockService.Api/JacksonVeroneze.StockService.Api.csproj

RUN dotnet publish app/JacksonVeroneze.StockService.Api/JacksonVeroneze.StockService.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

RUN apk add icu-libs curl
RUN apk add --update curl && \
    rm -rf /var/cache/apk/*

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "JacksonVeroneze.StockService.Api.dll"]
