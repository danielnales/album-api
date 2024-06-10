FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80

# Copy everything
COPY ./Album-Api ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80

ENTRYPOINT ["dotnet", "Album.Api.dll"]
