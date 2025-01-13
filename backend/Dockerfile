FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "Backend.sln"
# Build and publish a release
RUN dotnet publish "WebApi/Application.WebApi.csproj" -c Release -o out --no-restore

# Build database update bundle
RUN dotnet tool install --global dotnet-ef --version 8.0.8
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool restore
RUN dotnet ef migrations bundle --project "DataAccessLayer/PersistenceLayer.DataAccess.csproj" --startup-project "WebApi/Application.WebApi.csproj" -o /App/updateDatabase

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

RUN apt-get update && apt-get install -y jq

COPY --from=build-env /App/out .
COPY --from=build-env /App/updateDatabase /App/updateDatabase
COPY --from=build-env /App/docker-entrypoint.sh /App/docker-entrypoint.sh
EXPOSE 8080
ENTRYPOINT [ "sh" ]
CMD ["/App/docker-entrypoint.sh"]