FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY AzBatch.Api/. AzBatch.Api/
COPY AzBatch.Abstractions/. AzBatch.Abstractions/
COPY AzBatch.Infrastructure/. AzBatch.Infrastructure/
RUN dotnet build AzBatch.Api -c Release -o /app/build

FROM build as publish
RUN dotnet publish AzBatch.Api -c release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teqniqly.AzBatch.Api.dll"]